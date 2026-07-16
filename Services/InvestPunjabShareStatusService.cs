using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class InvestPunjabShareStatusService : IInvestPunjabShareStatusService
    {
        public IConfiguration Configuration { get; }
        private readonly IGenericRepository<BusinessFirstShareStatusLog> _iGR_BusinessFirstShareStatusLog;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        public InvestPunjabShareStatusService(IConfiguration configuration,
            IGenericRepository<BusinessFirstShareStatusLog> iGR_BusinessFirstShareStatusLog,
            IGeneric_SP_Repository iGeneric_SP_Repository)
        {
            Configuration = configuration;
            _iGR_BusinessFirstShareStatusLog = iGR_BusinessFirstShareStatusLog;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
        }

        public async Task<GenericResponseTemplateModel<bool>> ShareStatusToBusinessFirst(InvestPunjabShareStatusParmsViewModel statusReq)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = true
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms1 = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= statusReq.AppRefId.ToString(), isNumber=true },
                    new StoreProcedureParm (){ ParmName="IsDirty", ParmValue= "1", isNumber=false }
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SetApplicationIsDirtyStatus", storeProcedureParms1);

                if (statusReq != null)
                {
                    //var statusId = statusReq.StatusId;
                    //statusReq.StatusId = (statusReq.StatusId == 404 || statusReq.StatusId == 102) ? 9 : statusReq.StatusId == 200 ? 5 : statusReq.StatusId == 500 ? 15 : statusReq.StatusId;

                    // Call Stored Procedure
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="appRefId", ParmValue=statusReq.AppRefId.ToString(), isNumber=true}
                    };

                    var totalTakenTimeInfo = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetTotalTakenTimeByDepartmentViewModel>("dbo.sp_GetTotalTakenTimeByDepartment", storeProcedureParms);

                    var requestData = new
                    {
                        iPin = statusReq.IPin,
                        AppId = statusReq.InvestPunjab_AppId,
                        statusId = statusReq.StatusId == 5 ? 2 : statusReq.StatusId,
                        statusDesc = statusReq.StatusDesc,
                        comments = statusReq.Comments,
                        senderName = statusReq.SenderName,
                        senderDesignation = statusReq.SenderDesignation,
                        receiverName = statusReq.ReceiverName,
                        receiverDesignation = statusReq.ReceiverDesignation,
                        clearanceIssuedOn = statusReq.ClearanceIssuedOn,
                        clearanceExpiredOn = statusReq.ClearanceExpiredOn,
                        licenseNo = statusReq.LicenseNo,
                        clearanceFile = statusReq.ClearanceFile,
                        statusDate = statusReq.StatusDate,
                        integrationSource = statusReq.IntegrationSource,
                        deemedApproval = (statusReq.StatusId == 205 || statusReq.StatusId == 206) ? "true" : "false",
                        departmentTakenTotalTime = totalTakenTimeInfo.FirstOrDefault().TotalTime,
                        timeType = totalTakenTimeInfo.FirstOrDefault().TimeType,
                        appActionLogId = statusReq.appActionLogId
                    };
                    

                    BusinessFirstShareStatusLog businessFirstShareStatusLog = new BusinessFirstShareStatusLog()
                    {
                        ApplicationType = statusReq.ApplicationType,
                        AppActionType = (AppActionTypeEnum)statusReq.appActionType,
                        ApiURL = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("ShareStatusApiUrl").Value,
                        StatusSentOn = DateTime.Now,
                        RequestJSON = JsonConvert.SerializeObject(requestData),
                        IsRequestCompeleted = false,
                        RequestCompletionOn = null,
                        ResponseJson = "",
                        TriedCount = + 1,
                        AppRefId = statusReq.AppRefId,
                        SyncStatusProcessEngineRefId=null
                    };

                    _iGR_BusinessFirstShareStatusLog.Insert(businessFirstShareStatusLog);
                    await _iGR_BusinessFirstShareStatusLog.SavechangeAsync();

                    // Post Request data to Invest punjab
                    string tokenUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("GetBusinessFirstTokenUrl").Value;
                    string integrationKey = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("InvestPunjabApiTokenIntegrationKey").Value;
                    InvestPunjabApiTokenViewModel investPunjabApiToken = null;
                    using (var httpClient = new HttpClient())
                    {
                        var tokenRequestData = new
                        {
                            IntegrationKey = integrationKey
                        };

                        StringContent content = new StringContent(JsonConvert.SerializeObject(tokenRequestData), Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync(tokenUrl, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            investPunjabApiToken = JsonConvert.DeserializeObject<InvestPunjabApiTokenViewModel>(apiResponse);
                        }
                    }
                    if (investPunjabApiToken != null && investPunjabApiToken.Token != null)
                    {
                        string url = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("ShareStatusApiUrl").Value;
                        using (var httpClient = new HttpClient())
                        {
                            httpClient.DefaultRequestHeaders.Add("authorization", investPunjabApiToken.Token);
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            StringContent content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                            using (var response = await httpClient.PostAsync(url, content))
                            {
                                businessFirstShareStatusLog.RequestCompletionOn = DateTime.Now;
                                businessFirstShareStatusLog.IsRequestCompeleted = true;
                                businessFirstShareStatusLog.ResponseJson = await response.Content.ReadAsStringAsync();
                                _iGR_BusinessFirstShareStatusLog.Update(businessFirstShareStatusLog);
                                await _iGR_BusinessFirstShareStatusLog.SavechangeAsync();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

    }
}