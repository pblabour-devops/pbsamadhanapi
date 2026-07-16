using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Org.BouncyCastle.Ocsp;
using DocumentFormat.OpenXml.Office2013.Excel;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Spreadsheet;
using EFCore.BulkExtensions;

namespace pbsamadhannetcoreapi.Services
{
    public class AdminService : IAdminService
    {
        private readonly IGenericRepository<ProjectSite> _iGR_ProjectSite;
        private readonly AppDbContext _context;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IDapperRepository _iDapperRepository;
        private IPaymentManagerService _iPaymentManagerService;
        private readonly IApplicationManagementService<dynamic> _iApplicationMamnagementService;
        private readonly IProjectSiteService _iProjectSiteService;
        public IConfiguration Configuration { get; }
        public AdminService(IGenericRepository<ProjectSite> iGR_ProjectSite,
            AppDbContext context, IThirdPartyInegrationsService iThirdPartyInegrationsService,
            IGeneric_SP_Repository iGeneric_SP_Repository, IDapperRepository iDapperRepository,
            IConfiguration configuration, IPaymentManagerService iPaymentManagerService,
            IApplicationManagementService<dynamic> iApplicationMamnagementService,
            IProjectSiteService iProjectSiteService)
        {
            _iGR_ProjectSite = iGR_ProjectSite;
            _context = context;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iDapperRepository = iDapperRepository;
            Configuration = configuration;
            _iPaymentManagerService = iPaymentManagerService;
            _iApplicationMamnagementService = iApplicationMamnagementService;
            _iProjectSiteService = iProjectSiteService;
        }

        public async Task<GenericResponseTemplateModel<StatusManagerResponseParmsViewModel>> SearchApplicationByIPin(StatusManagerRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<StatusManagerResponseParmsViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<StatusManagerResponseParmsViewModel>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="investPunjab_Ipin", ParmValue=requestData.IPin, isNumber=false},
                    new StoreProcedureParm (){ ParmName="investPunjab_AppId", ParmValue=requestData.ApplicationId, isNumber=false},
                    new StoreProcedureParm (){ ParmName="applicationType", ParmValue=((int)requestData.ApplicationType).ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="applicationPurposeType", ParmValue= ((int)requestData.ApplicationPurposeType).ToString(), isNumber=true}
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<StatusManagerResponseParmsViewModel>("sp_Admin_GetCurrentApplicationStatusByIPin", storeProcedureParms).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = resp.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> ShareStaus(Int64 appId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = new GenericResponseTemplateModel<string>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = ""
            };
            try
            {
                genericServiceResultTemplate = await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appId, applicationType, appActionType);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> UnlockedApplication(Int64 appId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                var application = _context.Applications.Where(x => x.AppId == appId && x.ApplicationType == applicationType && x.IsDeleted == false).FirstOrDefault();
                application.IsLocked = false;
                application.IsAllowEdit = true;
                application.ApplicationLifeCycleStatusType = appActionType == AppActionTypeEnum.APP_OBJECTION ? ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION : ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                application.ApplicationLifeCycleLastStatusOn = DateTime.Now;
                _context.Update<Application>(application);
                await _context.SaveChangesAsync();
                genericServiceResultTemplate.ResponseDataModel = true;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<GetAdminDashboardDetailsViewModel>> SearchApplication(SearchApplicationParmsViewModel requestData)
        {
            var response = new GenericResponseTemplateModel<GetAdminDashboardDetailsViewModel>()
            {
                HasError = false,
                ErrorDesc = ""
            };

            try
            {
                var result = new GetAdminDashboardDetailsViewModel();

                using (var conn = _context.Database.GetDbConnection())
                {
                    await conn.OpenAsync();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "sp_GetAdminDashboardDetails_New";
                        command.CommandType = CommandType.StoredProcedure;

                        var param = command.CreateParameter();
                        param.ParameterName = "@SearchParams";
                        param.Value = requestData.SearchParams ?? (object)DBNull.Value;
                        command.Parameters.Add(param);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            // 1. User Details
                            var userDetails = new List<AdminDashboardUserDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    userDetails.Add(new AdminDashboardUserDetailsViewModel
                                    {
                                        UserName = reader["UserName"]?.ToString(),
                                        FullName = reader["FullName"]?.ToString(),
                                        MobileNo = reader["MobileNo"]?.ToString(),
                                        Email = reader["Email"]?.ToString()
                                    });
                                }
                            }
                            result.UserDetails = userDetails;

                            // 2. Establishment Details
                            await reader.NextResultAsync();
                            var establishmentDetails = new List<AdminDashboardEstablishmentDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    establishmentDetails.Add(new AdminDashboardEstablishmentDetailsViewModel
                                    {
                                        EstablishmentName = reader["EstablishmentName"]?.ToString(),
                                        Address = reader["Address"]?.ToString(),
                                        DistrictName = reader["DistrictName"]?.ToString(),
                                        TehsilName = reader["TehsilName"]?.ToString(),
                                        CircleName = reader["CircleName"]?.ToString(),
                                        ProjectPurpose = reader["ProjectPurpose"]?.ToString()
                                    });
                                }
                            }
                            result.EstablishmentDetails = establishmentDetails;

                            // 3. Application Details
                            await reader.NextResultAsync();
                            var applicationDetails = new List<AdminDashboardApplicationDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    applicationDetails.Add(new AdminDashboardApplicationDetailsViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt32(reader["AppId"]) : 0,
                                        PublicAppRefNum = reader["PublicAppRefNum"]?.ToString(),
                                        ServiceName = reader["ServiceName"]?.ToString(),
                                        InvestPunjab_Ipin = reader["InvestPunjab_Ipin"]?.ToString(),
                                        InvestPunjab_AppId = reader["InvestPunjab_AppId"]?.ToString(),
                                        CreatedOnDate = reader["CreatedOnDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedOnDate"]) : default(DateTime),
                                        ApplicationLifeCycleStatusType = reader["ApplicationLifeCycleStatusType"]?.ToString(),
                                        FormStatus = reader["FormStatus"]?.ToString()
                                    }); ;
                                }
                            }
                            result.ApplicationDetails = applicationDetails;

                            // 4. Application Actions
                            await reader.NextResultAsync();
                            var applicationActions = new List<AdminDashboardActionLogViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    applicationActions.Add(new AdminDashboardActionLogViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt64(reader["AppId"]) : 0,
                                        AppActionType = reader["AppActionType"] != DBNull.Value ? Convert.ToInt32(reader["AppActionType"]) : 0,
                                        ActionName = reader["ActionName"]?.ToString(),
                                        SenderUserName = reader["SenderUserName"]?.ToString(),
                                        SenderName = reader["SenderName"]?.ToString(),
                                        ReceiverUserName = reader["ReceiverUserName"]?.ToString(),
                                        ReceiverName = reader["ReceiverName"]?.ToString(),
                                        ActionDate = reader["ActionDate"] != DBNull.Value ? Convert.ToDateTime(reader["ActionDate"]) : default(DateTime),
                                        Comment = reader["Comment"]?.ToString()
                                    });
                                }
                            }
                            result.ApplicationActions = applicationActions;

                            // 5. Application Action Logs
                            await reader.NextResultAsync();
                            var applicationActionLogs = new List<AdminDashboardActionLogViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    applicationActionLogs.Add(new AdminDashboardActionLogViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt64(reader["AppId"]) : 0,
                                        AppActionType = reader["AppActionType"] != DBNull.Value ? Convert.ToInt32(reader["AppActionType"]) : 0,
                                        ActionName = reader["ActionName"]?.ToString(),
                                        SenderUserName = reader["SenderUserName"]?.ToString(),
                                        SenderName = reader["SenderName"]?.ToString(),
                                        ReceiverUserName = reader["ReceiverUserName"]?.ToString(),
                                        ReceiverName = reader["ReceiverName"]?.ToString(),
                                        ActionDate = reader["ActionDate"] != DBNull.Value ? Convert.ToDateTime(reader["ActionDate"]) : default(DateTime),
                                        Comment = reader["Comment"]?.ToString()
                                    });
                                }
                            }
                            result.ApplicationActionLogs = applicationActionLogs;

                            // 6. Payment Details
                            await reader.NextResultAsync();
                            var paymentDetails = new List<AdminDashboardPaymentDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    paymentDetails.Add(new AdminDashboardPaymentDetailsViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt32(reader["AppId"]) : 0,
                                        TransactionAmount = reader["TransactionAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TransactionAmount"]) : 0,
                                        TransactionDate = reader["TransactionDate"] != DBNull.Value ? Convert.ToDateTime(reader["TransactionDate"]) : default(DateTime),
                                        UniquePaymentGatewayTransactionId = reader["UniquePaymentGatewayTransactionId"]?.ToString(),
                                        TransactionStatus = reader["TransactionStatus"]?.ToString()
                                    });
                                }
                            }
                            result.PaymentDetails = paymentDetails;

                            // 7. Approval Details
                            await reader.NextResultAsync();
                            var approvalDetails = new List<AdminDashboardApprovalDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    approvalDetails.Add(new AdminDashboardApprovalDetailsViewModel
                                    {
                                        SrNo = reader["Sr. No"] != DBNull.Value ? Convert.ToInt32(reader["Sr. No"]) : 0,
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt32(reader["AppId"]) : 0,
                                        ServiceName = reader["ServiceName"]?.ToString(),
                                        LicenceNumber = reader["LicenceNumber"]?.ToString(),
                                        ApprovalDate = reader["ApprovalDate"] != DBNull.Value ? Convert.ToDateTime(reader["ApprovalDate"]) : default(DateTime),
                                        ClearanceExpiredOn = reader["ClearanceExpiredOn"] != DBNull.Value ? Convert.ToDateTime(reader["ClearanceExpiredOn"]) : default(DateTime),
                                        LicencePath = reader["LicencePath"]?.ToString()
                                    });
                                }
                            }
                            result.ApprovalDetails = approvalDetails;


                            // 8. Welfare Contribution Details
                            await reader.NextResultAsync();
                            var welfareContributionDetails = new List<AdminDashboardWelfareContributionDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    welfareContributionDetails.Add(new AdminDashboardWelfareContributionDetailsViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt64(reader["AppId"]) : 0,
                                        FinancialYear = reader["FinancialYear"]?.ToString(),
                                        TimeSlot = reader["TimeSlot"]?.ToString(),
                                        Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0,
                                        PWBCessCollectionId = reader["PWBCessCollectionId"] != DBNull.Value ? Convert.ToInt64(reader["PWBCessCollectionId"]) : 0,
                                    });
                                }
                            }
                            result.WelfareContributionDetails = welfareContributionDetails;


                            // 9. Annual Return Details
                            await reader.NextResultAsync();
                            var annualReturnDetails = new List<AdminDashboardAnnualReturnDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    annualReturnDetails.Add(new AdminDashboardAnnualReturnDetailsViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt64(reader["AppId"]) : 0,
                                        AccNo = reader["AccNo"]?.ToString(),
                                        ReturnYear = reader["ReturnYear"]?.ToString(),
                                        ReturnType = reader["ReturnType"]?.ToString(),
                                        ReturnID = reader["ReturnID"] != DBNull.Value ? Convert.ToInt64(reader["ReturnID"]) : 0,
                                    });
                                }
                            }
                            result.AnnualReturnDetails = annualReturnDetails;


                            // 10. Annual Return Details
                            await reader.NextResultAsync();
                            var inspectionDetails = new List<AdminDashboardInspectionDetailsViewModel>();
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    inspectionDetails.Add(new AdminDashboardInspectionDetailsViewModel
                                    {
                                        AppId = reader["AppId"] != DBNull.Value ? Convert.ToInt64(reader["AppId"]) : 0,
                                        EstablishmentName = reader["EstablishmentName"]?.ToString(),
                                        LicenceNumber = reader["LicenceNumber"]?.ToString(),
                                        FactoryHazardousCategoryType = reader["FactoryHazardousCategoryType"]?.ToString(),
                                        InspectionDoneOn_Factory_Wing = reader["InspectionDoneOn_Factory_Wing"] != DBNull.Value ? Convert.ToDateTime(reader["InspectionDoneOn_Factory_Wing"]) : default(DateTime),
                                        InspectionSubmittedFactoryWing = reader["InspectionSubmittedFactoryWing"]?.ToString(),
                                        InspectionDoneOn_Labour_Wing = reader["InspectionDoneOn_Labour_Wing"] != DBNull.Value ? Convert.ToDateTime(reader["InspectionDoneOn_Labour_Wing"]) : default(DateTime),
                                        InspectionSubmittedLabourWing = reader["InspectionSubmittedLabourWing"]?.ToString(),
                                    });
                                }
                            }
                            result.InspectionDetails = inspectionDetails;


                        }
                    }
                }

                response.ResponseDataModel = result;
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorDesc = ex.Message;
            }

            return response;
        }

        // ************* Using Dapper
        //public async Task<GenericResponseTemplateModel<GetAdminDashboardDetailsViewModel>> SearchApplication(SearchApplicationParmsViewModel requestData)
        //{
        //    var response = new GenericResponseTemplateModel<GetAdminDashboardDetailsViewModel>
        //    {
        //        HasError = false,
        //        ErrorDesc = ""
        //    };

        //    try
        //    {
        //        var data = await _iDapperRepository.GetMultipleResultSets<SqlMapper.GridReader>("sp_GetAdminDashboardDetails",new { SearchParams = requestData.SearchParams ?? (object)DBNull.Value });
        //        if (data == null)
        //        {
        //            response.HasError = true;
        //            response.ErrorDesc = "No data returned from stored procedure.";
        //            return response;
        //        }

        //        // Map all result sets to your view model lists
        //        var result = new GetAdminDashboardDetailsViewModel
        //        {
        //            UserDetails = data.Read<AdminDashboardUserDetailsViewModel>().ToList(),
        //            EstablishmentDetails = data.Read<AdminDashboardEstablishmentDetailsViewModel>().ToList(),
        //            ApplicationDetails = data.Read<AdminDashboardApplicationDetailsViewModel>().ToList(),
        //            ApplicationActions = data.Read<AdminDashboardActionLogViewModel>().ToList(),
        //            ApplicationActionLogs = data.Read<AdminDashboardActionLogViewModel>().ToList(),
        //            PaymentDetails = data.Read<AdminDashboardPaymentDetailsViewModel>().ToList(),
        //            ApprovalDetails = data.Read<AdminDashboardApprovalDetailsViewModel>().ToList()
        //        };

        //        // IMPORTANT: Dispose the GridReader to release connection
        //        data.Dispose();

        //        response.ResponseDataModel = result;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.HasError = true;
        //        response.ErrorDesc = ex.Message;
        //    }

        //    return response;
        //}


        public async Task<GenericResponseTemplateModel<List<AllTransactionsByAppRefIdViewModel>>> SearchTransactions(Int64 appRefId)
        {
            GenericResponseTemplateModel<List<AllTransactionsByAppRefIdViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<AllTransactionsByAppRefIdViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= appRefId.ToString(), isNumber=true}
                };

                List<AllTransactionsByAppRefIdViewModel> allTransactions = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AllTransactionsByAppRefIdViewModel>("dbo.sp_Get_AllTransactionsByAppRefId", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = allTransactions;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> VerifyTransactionByAppRefId(Int64 appRefId, string uniquePaymentGatewayTransactionId, string paymentTreasuryType)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = new GenericResponseTemplateModel<string>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                if(paymentTreasuryType =="1") // Treasury Payment
                {
                   await VerifyTreasuryTransaction(appRefId, uniquePaymentGatewayTransactionId);
                }
                else // Non-treasury Payment
                {
                    await VerifyNonTreasuryTransaction(appRefId, uniquePaymentGatewayTransactionId);
                }
                genericServiceResultTemplate.ResponseDataModel = appRefId.ToString();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        private async Task<HttpResponseMessage> ExecuteAPI(string URL, string PostData)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
            request.Content = new StringContent(PostData, Encoding.UTF8, "application/json");
            HttpContent inputContent = new StringContent(PostData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(URL, inputContent).Result;
            return response;
        }

        public async Task<bool> VerifyTreasuryTransaction(Int64 appRefId, string uniquePaymentGatewayTransactionId)
        {
            ifms_data data = new ifms_data();
            data.challandata = new Challandata()
            {
                deptRefNo = uniquePaymentGatewayTransactionId,
                clientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IntegratingAgency").Value,
                deptCode = "LAB",
                challanDate = DateTime.Now
            };

            string json = JsonConvert.SerializeObject(data.challandata);
            data.chcksum = PaymentGatewayEncDecryOps.GenerateChecksum_IFMS(json, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ChecksumKey").Value);
            string jsonCHK = JsonConvert.SerializeObject(data);
            string encData = PaymentGatewayEncDecryOps.Encrypt_IFMS(jsonCHK, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretIV").Value);
            checkdata cHeader = new checkdata()
            {
                encData = encData,
                clientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ClientId").Value,
                clientSecret = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("ClientSecret").Value,
                transactionID = new Random().Next(100000, 999999).ToString(),
                ipAddress = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IPAllow").Value,
                integratingAgency = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("IntegratingAgency").Value
            };

            var reqData = JsonConvert.SerializeObject(cHeader);
            HttpResponseMessage resMsg = await ExecuteAPI(Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("VerifyPaymentGatewayTargetUrl").Value, reqData);
            ResponseDataViewModel_IFMS paymnetVerificationResponse = new ResponseDataViewModel_IFMS();
            string paymentGatewayReturnData = resMsg.Content.ReadAsStringAsync().Result;
            paymnetVerificationResponse = JsonConvert.DeserializeObject<ResponseDataViewModel_IFMS>(paymentGatewayReturnData);

            if (paymnetVerificationResponse != null)
            {
                //string decData = PaymentGatewayEncDecryOps.Decrypt_IFMS(paymnetVerificationResponse.encData, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS").GetSection("SecretIV").Value);
                //ChallandataViewModel challandata = new ChallandataViewModel();
                //challandata = JsonConvert.DeserializeObject<ChallandataViewModel>(decData);
                await _iPaymentManagerService.HandlePaymentGatewayResponse(paymnetVerificationResponse, PaymentGatewayTypeEnum.IFMS);
            }
            return true;
        }
        public async Task<bool> VerifyNonTreasuryTransaction(Int64 appRefId, string uniquePaymentGatewayTransactionId)
        {
            // Step 1: Read challan data from JSON
            string challanDataJson = "";
            string requestdataJson = "";
            NonTreasuryChallanDetails nonTreasury = new NonTreasuryChallanDetails()
            {
                DeptRefNo = uniquePaymentGatewayTransactionId,
                ClientId = "0",
                Service = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("Service").Value,
                ChallanDate = DateTime.Now
            };
            string nonTreasuryJson = JsonConvert.SerializeObject(nonTreasury);


            NonTreasuryVerifyRequest verifyRequest = new NonTreasuryVerifyRequest();
            verifyRequest.ChallanDetails = JsonConvert.DeserializeObject<NonTreasuryChallanDetails>(nonTreasuryJson);

            // Step 2: Get encryption helper and checksum
            string rawChecksum = $"{verifyRequest.ChallanDetails.DeptRefNo}|{verifyRequest.ChallanDetails.ChallanDate}|{verifyRequest.ChallanDetails.Service}";




            string checksumData = PaymentGatewayEncDecryOps.GenerateChecksum_IFMS(rawChecksum, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("ChecksumKey").Value);

            NonTreasuryVerifyRequest iFMS_RequestData = new NonTreasuryVerifyRequest()
            {
                Checksum = checksumData,
                ChallanDetails = verifyRequest.ChallanDetails,
            };

            requestdataJson = JsonConvert.SerializeObject(iFMS_RequestData);
            challanDataJson = PaymentGatewayEncDecryOps.Encrypt_IFMS(requestdataJson, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("SecretIV").Value);

            // Step 3: Create Header
            NonTreasuryHeader header = new NonTreasuryHeader
            {
                EncData = challanDataJson,
                DeptRefNo = verifyRequest.ChallanDetails.DeptRefNo,
                ClientId = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("ClientId").Value,
                ClientSecret = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("ClientSecret").Value,
                TransactionID = new Random().Next(100000, 999999).ToString(),
                IPAddress = "103.118.160.168",
                Service = Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("Service").Value,
            };

            string postData = JsonConvert.SerializeObject(header);

            HttpResponseMessage resMsg = await ExecuteAPI(Configuration["PaymentGatewayConfigs:IFMS_Non_Treasury:VerifyPaymentGatewayTargetUrl"],postData);
            NonTreasuryResponseDataViewModel_IFMS paymnetVerificationResponse = new NonTreasuryResponseDataViewModel_IFMS();
            string paymentGatewayReturnData = resMsg.Content.ReadAsStringAsync().Result;
            paymnetVerificationResponse = JsonConvert.DeserializeObject<NonTreasuryResponseDataViewModel_IFMS>(paymentGatewayReturnData);

            if (paymnetVerificationResponse != null)
            {  
                await HandlePaymentGatewayResponse(paymnetVerificationResponse, PaymentGatewayTypeEnum.IFMS_NON_TREASURY);
            }

            return true;
        }

        public async Task<GenericFormModel<PaymentGatewayResponseToUiViewModel>> HandlePaymentGatewayResponse(NonTreasuryResponseDataViewModel_IFMS responseObject, PaymentGatewayTypeEnum paymentGatewayType)
        {
            GenericFormModel<PaymentGatewayResponseToUiViewModel> genericFormModel = new GenericFormModel<PaymentGatewayResponseToUiViewModel>();
            PaymentGatewayResponseViewModel paymentGatewayResponse = null;
            string gatewayResponseType = string.Empty;

            if (paymentGatewayType == PaymentGatewayTypeEnum.IFMS_NON_TREASURY)
            {
                NonTreasuryResponseDataViewModel_IFMS responseData = responseObject;
                string encData = "";
                Int64 appRefId = 0;
                string bankRefNumber = "";
                decimal transactionAmount = 0;
                int paymentBatchCounter = 0;
                int paymentPartCounter = 0;

                encData = PaymentGatewayEncDecryOps.Decrypt_IFMS(responseData.encData, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("SecretKey").Value, Configuration.GetSection("PaymentGatewayConfigs").GetSection("IFMS_Non_Treasury").GetSection("SecretIV").Value);
                PaymentGatewayResponseTemplateViewModel_IFMS resData = JsonConvert.DeserializeObject<PaymentGatewayResponseTemplateViewModel_IFMS>(encData);

                //responseData.statusCode = "SC300";
                if (responseData.statusCode != "SC300")
                {
                    var transation = _context.AppFeeTransactions.Where(x => x.UniquePaymentGatewayTransactionId == resData.challandata.deptRefNo);
                    appRefId = transation.FirstOrDefault().AppRefId;
                    paymentBatchCounter = transation.FirstOrDefault().PaymentBatchCounter;
                    paymentPartCounter = transation.FirstOrDefault().PaymentPartCounter;
                    bankRefNumber = "";
                    transactionAmount = 0;
                }
                else
                {

                    //appRefId = Convert.ToUInt32((resData.challandata.add1));

                    var transation = _context.AppFeeTransactions.Where(x => x.UniquePaymentGatewayTransactionId == resData.challandata.deptRefNo && x.PaymentTreasuryType ==  PaymentTreasuryTypeEnum.NON_TREASURY);
                    appRefId = transation.FirstOrDefault().AppRefId;
                    paymentBatchCounter = transation.FirstOrDefault().PaymentBatchCounter;
                    paymentPartCounter = transation.FirstOrDefault().PaymentPartCounter;

                    bankRefNumber = resData.challandata.bank_Res.BankRefNo;
                    transactionAmount = Convert.ToInt32(resData.challandata.totalAmt);
                }

                gatewayResponseType = responseData.statusCode;
                paymentGatewayResponse = new PaymentGatewayResponseViewModel()
                {
                    IsWebRequestCycleCompleted = true,
                    ResponseBodyData = encData,
                    ResponseMessage = responseData.msg,
                    ResponseReceivedOn = DateTime.Now,
                    UniquePaymentGatewayTransactionId = resData.challandata.deptRefNo,
                    AppRefId = appRefId,
                    PaymentBatchCounter = paymentBatchCounter,
                    PaymentPartCounter = paymentPartCounter,
                    PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY,

                    TransactionFinalStatusType = gatewayResponseType == "SC300" ? TransactionFinalStatusTypeEnum.SUCCEED :
                        gatewayResponseType == "SC310" ? TransactionFinalStatusTypeEnum.PENDING :
                        (gatewayResponseType == "EC301" || gatewayResponseType == "EC302" || gatewayResponseType == "EC303" || gatewayResponseType == "EC304"
                        || gatewayResponseType == "EC305" || gatewayResponseType == "EC306" || gatewayResponseType == "EC307") ? TransactionFinalStatusTypeEnum.FAILED :
                        TransactionFinalStatusTypeEnum.UNKNOWN
                };

                var userProfile = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(appRefId);
                var projectSites = await _iProjectSiteService.Get_ProjectSiteByProjectSiteId(Convert.ToInt64(userProfile.FormModel.ProjectSiteId), appRefId, userProfile.FormModel.ProjectSiteVersion);

                //Update gateway response to view - model
                genericFormModel.FormModel = new PaymentGatewayResponseToUiViewModel()
                {
                    PaymentGatewatResponseText = responseObject.msg,
                    TransactionFinalStatus = paymentGatewayResponse.TransactionFinalStatusType,
                    TransactionDate = DateTime.Now,
                    AppRefId = appRefId,
                    EstablishmentName = projectSites.ResponseDataModel.EstablishmentName,
                    EstablishmentAddress = projectSites.ResponseDataModel.Address,
                    PublicAppRefNum = projectSites.ResponseDataModel.PublicAppRefNum,
                    ApplicationType = projectSites.ResponseDataModel.ApplicationType,
                    ApplicationPurposeType = projectSites.ResponseDataModel.ApplicationPurposeType,
                    BankRefNumber = bankRefNumber,
                    BankTransactionRefNumberType = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                    TransactionAmount = Convert.ToInt32(transactionAmount),
                    PaymentGatewayType = paymentGatewayType
                };

                //Initialization of Enum types of lists
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationPurposeTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationPurposeTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BankTransactionReferenceTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BankTransactionReferenceTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "TransactionFinalStatusTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<TransactionFinalStatusTypeEnum>()
                });

               var transactionUpdateResp = await UpdateTransactionResponse(paymentGatewayResponse);

                if (transactionUpdateResp.ResponseDataModel == 1 && paymentGatewayResponse.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED) // Payment Success
                {
                    await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appRefId, (ApplicationTypeEnum)genericFormModel.FormModel.ApplicationType, AppActionTypeEnum.APP_SUBMITTED);
                }



            }
            return genericFormModel;
        }



        public async Task<GenericResponseTemplateModel<int>> UpdateTransactionResponse(PaymentGatewayResponseViewModel paymentGatewayResponse)
        {

            GenericResponseTemplateModel<int> genericServiceResultTemplate = new GenericResponseTemplateModel<int>() { ResponseDataModel = 0 };
            try
            {
                genericServiceResultTemplate.HasError = false;
                if (paymentGatewayResponse != null)
                {

                    AppFeeTransaction appFeeTransaction = _context.AppFeeTransactions.Where(x =>
                                                        x.UniquePaymentGatewayTransactionId == paymentGatewayResponse.UniquePaymentGatewayTransactionId
                                                        && x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter
                                                        && x.PaymentBatchCounter == paymentGatewayResponse.PaymentBatchCounter).FirstOrDefault();
                    if (appFeeTransaction != null)
                    {
                        appFeeTransaction.IsWebRequestCycleCompleted = paymentGatewayResponse.IsWebRequestCycleCompleted;
                        appFeeTransaction.ResponseBodyData = paymentGatewayResponse.ResponseBodyData;
                        appFeeTransaction.ResponseReceivedOn = paymentGatewayResponse.ResponseReceivedOn;
                        appFeeTransaction.ResponseMessage = paymentGatewayResponse.ResponseMessage;
                        appFeeTransaction.TransactionFinalStatusType = paymentGatewayResponse.TransactionFinalStatusType;
                        appFeeTransaction.BankTransactionRefNumber1 = paymentGatewayResponse.UniquePaymentGatewayTransactionId;
                        appFeeTransaction.BankTransactionRefNumber1_Type = BankTransactionReferenceTypeEnum.CIN;
                        appFeeTransaction.BankSettlementOn = DateTime.Now;
                        appFeeTransaction.PaymentBatchCounter = paymentGatewayResponse.PaymentBatchCounter;
                        appFeeTransaction.PaymentTreasuryType = paymentGatewayResponse.PaymentTreasuryType;

                        _context.Update(appFeeTransaction);
                        _context.SaveChanges();



                        var paymentParts = _context.AppPaymentParts.Where(x => x.AppRefId == paymentGatewayResponse.AppRefId
                                                                           && x.PaymentBatchCounter == paymentGatewayResponse.PaymentBatchCounter).ToList();


                        if (paymentParts.Where(x => x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter).Count() > 0)
                        {
                            var paymentPart = paymentParts.Where(x => x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter).FirstOrDefault();
                            paymentPart.TransactionFinalStatusType = paymentGatewayResponse.TransactionFinalStatusType;
                            _context.Update(paymentPart);
                            _context.SaveChanges();
                            paymentParts[paymentParts.ToList().FindIndex(x => x.PaymentPartCounter == paymentGatewayResponse.PaymentPartCounter)].TransactionFinalStatusType = paymentGatewayResponse.TransactionFinalStatusType;
                        }

                        GenericServiceResultTemplate appServiceResponse = new GenericServiceResultTemplate();
                        if (appFeeTransaction.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED)
                        {
                            int hasAllPartsPaid = 0;
                            hasAllPartsPaid = (paymentParts.Count() == 0 || (paymentParts.Count() > 0 && paymentParts.Count() == paymentParts.Count(x => x.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED))) ? 1 : 0;
                            genericServiceResultTemplate.ResponseDataModel = hasAllPartsPaid;
                            appServiceResponse = await _iApplicationMamnagementService.RecordAppFeeSuccessTransactionEntry(appFeeTransaction.AppRefId, appFeeTransaction.AppFeeTransactionId, paymentGatewayResponse.PaymentPartCounter, paymentGatewayResponse.PaymentBatchCounter, hasAllPartsPaid);

                        }

                        if (appServiceResponse.HasException)
                        {
                            genericServiceResultTemplate.HasError = true;
                            genericServiceResultTemplate.ErrorDesc = appServiceResponse.Exceptions.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }



        public async Task<GenericResponseTemplateModel<bool>> UpdateApplicationActionLogs(Int64 appRefId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                var appFeeTransaction = _context.AppFeeTransactions.Where(x => x.AppRefId == appRefId && x.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED).FirstOrDefault();
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appId", ParmValue=appRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="appFeeTransactionRefId", ParmValue=appFeeTransaction.AppFeeTransactionId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="paymentPartCounter", ParmValue=appFeeTransaction.PaymentPartCounter.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="paymentBatchCounter", ParmValue=appFeeTransaction.PaymentBatchCounter.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="hasAllPartsPaid", ParmValue="1", isNumber=true}
                };
                await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("sp_Application_Record_AppFee_Success_Transaction_Entry", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = true;

                if (genericServiceResultTemplate.ResponseDataModel == true)
                {
                    var application = _context.Applications.Where(x => x.AppId == appRefId).FirstOrDefault();
                    await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appRefId, (ApplicationTypeEnum)application.ApplicationType, AppActionTypeEnum.APP_SUBMITTED);
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> RegisterEmpanelledPerson(UserArchitectAdditionalInfoMapping formModel)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = new GenericResponseTemplateModel<string>();
            try
            {
                if (formModel != null)
                {
                    string registrationNumber_DOF = "";
                    // Check If Already Registered
                    UserProfile userProfile1 = await _context.UserProfiles.Where(x => x.FirstName.Contains(formModel.ArchitectName) || x.MobileNo == formModel.ContactNumber).FirstOrDefaultAsync();
                    if(userProfile1 != null)
                    {
                        UserProfileMapping profileMapping = await _context.UserProfileMapping.Where(x => x.UserProfileRefId == userProfile1.UserProfileId).FirstOrDefaultAsync();
                        registrationNumber_DOF = await _context.UserArchitectAdditionalInfoMappings.Where(x => x.UserRefId == profileMapping.UserRefId).Select(x => x.RegistrationNumber_DOF).FirstOrDefaultAsync();
                    }

                    if (userProfile1 == null)
                    {
                        string userName = "";
                        string roleId = "";
                        String registrationNo = "";
                        if (formModel.EmpanelledType == (int)BuildingPlanApprovalAuthorityTypeEnum.COMPETENT_PERSON)
                        {
                            userName = "CP." + formModel.ArchitectName.Trim().Replace(" ", "");
                            roleId = "674E6F88-8D81-4605-AD96-5CD121E74611";
                            registrationNo = "DOFPB-CPPR-";
                        }
                        else if (formModel.EmpanelledType == (int)BuildingPlanApprovalAuthorityTypeEnum.EMPANELED_ARCHITECT)
                        {
                            userName = "AR." + formModel.ArchitectName.Trim().Replace(" ", "");
                            roleId = "C4D90BAD-D770-48BC-A97B-441CEAD5F49B";
                            registrationNo = "DOFPB-ARCH-";
                        }
                        else if (formModel.EmpanelledType == (int)BuildingPlanApprovalAuthorityTypeEnum.EMPANELED_ENGINEER)
                        {
                            userName = "Er." + formModel.ArchitectName.Trim().Replace(" ", "");
                            roleId = "3103952B-BC50-45CE-9B30-4FD9709BBBEE";
                            registrationNo = "DOFPB-ENGR-";
                        }

                        // Create User-
                        User user = new User()
                        {
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "",
                            Email = formModel.Email,
                            EmailConfirmed = true,
                            Id = Guid.NewGuid().ToString(),
                            LockoutEnabled = false,
                            LockoutEnd = null,
                            NormalizedEmail = "NoEmail@some.com",
                            NormalizedUserName = userName,
                            PasswordHash = "mHwh59uDN9MAGOBITRYJkR3FWK62fgqAjBKhFUePd8A=",
                            PhoneNumber = formModel.ContactNumber,
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "rI93o3GC0KZOhH7A+TX+8g==",
                            TwoFactorEnabled = false,
                            UserName = "AR." + formModel.ArchitectName.Trim().Replace(" ", ""),
                            IsEnabled = true,
                            IsTestUser = false
                        };

                        await _context.AddAsync<User>(user);
                        await _context.SaveChangesAsync();

                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="RoleId", ParmValue=roleId, isNumber=false},
                        new StoreProcedureParm (){ ParmName="UserId", ParmValue=user.Id, isNumber=false}
                    };
                        await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_MapUserWithRole", storeProcedureParms);

                        // Insert into userProfile
                        var fullName = formModel.ArchitectName;
                        var firstName = fullName.Substring(0, fullName.IndexOf(" "));
                        var lastName = fullName.Substring(fullName.IndexOf(" ") + 1);

                        UserProfile userProfile = new UserProfile()
                        {
                            FirstName = firstName,
                            MiddleName = "",
                            LastName = lastName,
                            MobileNo = formModel.ContactNumber,
                            FatherName = formModel.ArchitectfatherName,
                            AlternateMobileNo = formModel.ContactNumber,
                            Email = formModel.Email,
                            AlternateEmail = formModel.Email,
                            State = "Punjab",
                            TehsilId = 1,
                            DistrictId = 1,
                            PinCode = "000000",
                            Signature = "",
                            ProfilePhoto = "",
                            IsActive = true,
                            IsDeleted = false,
                            Createddate = DateTime.Now,
                            LastModifiedDate = DateTime.Now
                        };
                        await _context.AddAsync(userProfile);
                        await _context.SaveChangesAsync();

                        //- Get the primary key of table UserProfile - UserProfileId (After insert)
                        Int64 insertedUserProfileId = userProfile.UserProfileId;

                        // Insert into UserProfileMapping
                        UserProfileMapping userProfileMapping = new UserProfileMapping()
                        {
                            DateOfUserAssign = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false,
                            Createddate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                            UserRefId = user.Id,
                            UserProfileRefId = insertedUserProfileId
                        };
                        await _context.AddAsync(userProfileMapping);
                        await _context.SaveChangesAsync();


                        // Insert into UserArchitectAdditionalInfoMapping

                        Random generator = new Random();
                        string randomNo = generator.Next(0, 1000000).ToString("D6");

                        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles");

                        // --- Process CouncilOfArchitectApproval (PDF or Word) ---
                        if (!string.IsNullOrWhiteSpace(formModel.CouncilOfArchitectApproval))
                        {
                            string base64Data = formModel.CouncilOfArchitectApproval;
                            string extension = ".pdf";

                            if (base64Data.StartsWith("data:"))
                            {
                                var header = base64Data.Substring(0, base64Data.IndexOf(","));
                                var mimeType = header.Split(':')[1].Split(';')[0];

                                extension = mimeType switch
                                {
                                    "application/pdf" => ".pdf",
                                    "application/msword" => ".doc",
                                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ".docx",
                                    _ => ".pdf"
                                };

                                base64Data = base64Data.Substring(base64Data.IndexOf(",") + 1);
                            }

                            string fileName = Guid.NewGuid().ToString() + extension;
                            string filePath = Path.Combine(uploadPath, fileName);
                            byte[] fileBytes = Convert.FromBase64String(base64Data);
                            File.WriteAllBytes(filePath, fileBytes);
                            formModel.CouncilOfArchitectApproval = fileName;
                        }

                        // --- Process SpecimenSignature (image only) ---
                        if (!string.IsNullOrWhiteSpace(formModel.SpecimenSignature))
                        {
                            string base64Data = formModel.SpecimenSignature;
                            string extension = ".jpg"; // default

                            if (base64Data.StartsWith("data:"))
                            {
                                var header = base64Data.Substring(0, base64Data.IndexOf(","));
                                var mimeType = header.Split(':')[1].Split(';')[0];

                                extension = mimeType switch
                                {
                                    "image/jpeg" => ".jpg",
                                    "image/jpg" => ".jpg",
                                    "image/png" => ".png",
                                    "image/svg+xml" => ".svg",
                                    _ => ".jpg"
                                };

                                base64Data = base64Data.Substring(base64Data.IndexOf(",") + 1);
                            }

                            string fileName = Guid.NewGuid().ToString() + extension;
                            string filePath = Path.Combine(uploadPath, fileName);
                            byte[] fileBytes = Convert.FromBase64String(base64Data);
                            File.WriteAllBytes(filePath, fileBytes);

                            formModel.SpecimenSignature = fileName;
                        }

                        UserArchitectAdditionalInfoMapping userArchitectAdditionalInfo = new UserArchitectAdditionalInfoMapping()
                        {
                            UserRefId = user.Id,
                            RegistrationNumber_DOF = registrationNo + randomNo,
                            CommunicationAddress = formModel.CommunicationAddress,
                            RegistrationNumber_COA = formModel.RegistrationNumber_COA,
                            RegistrationIssuedOn_COA = formModel.RegistrationIssuedOn_COA,
                            RegistrationValidUpto_COA = formModel.RegistrationValidUpto_COA,
                            TermAndCondition1 = formModel.TermAndCondition1,
                            TermAndCondition2 = formModel.TermAndCondition2,
                            CouncilOfArchitectApproval = formModel.CouncilOfArchitectApproval,
                            SpecimenSignature = formModel.SpecimenSignature
                        };
                        await _context.AddAsync(userArchitectAdditionalInfo);
                        await _context.SaveChangesAsync();

                        genericServiceResultTemplate.ResponseDataModel = userArchitectAdditionalInfo.RegistrationNumber_DOF;
                    }
                    else
                    {
                        genericServiceResultTemplate.ResponseDataModel = "User Is Already Exist." + "Registration No is : " + registrationNumber_DOF;
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


        public async Task<GenericListModel<DepartmentOfficialDetailsViewModel>> GetDepartmentOfficialDetailsByRoleName(string roleName)
        {
            GenericListModel<DepartmentOfficialDetailsViewModel> genericListModel = new GenericListModel<DepartmentOfficialDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RoleName", ParmValue=roleName.ToString(), isNumber=false}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DepartmentOfficialDetailsViewModel>("sp_GetDepartmentOfficialDetailsByRoleName", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }

        public async Task<GenericListModel<DepartmentOfficialListViewModel>> GetDepartmentOfficialList(string roleName)
        {
            GenericListModel<DepartmentOfficialListViewModel> genericListModel = new GenericListModel<DepartmentOfficialListViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RoleName", ParmValue=roleName.ToString(), isNumber=false}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DepartmentOfficialListViewModel>("sp_GetDepartmentOfficialList", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }

        public async Task<GenericServiceResultTemplate> UpdateOfficerTransfer(UpdateOfficerTransferViewModel updateOfficerTransfer)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                string filename = "NA";
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserName", ParmValue=updateOfficerTransfer.Username.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="CurrentProfileId", ParmValue=updateOfficerTransfer.CurrentProfileId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="TransferProfileId", ParmValue=updateOfficerTransfer.tranferofficerProfileId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="Role", ParmValue=updateOfficerTransfer.role.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="TransferOrder", ParmValue=filename.ToString(), isNumber=false},

                };
                var userProfileMappingId = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Officer_ProfileTransfer", storeProcedureParms);

                byte[] bytes = Convert.FromBase64String(updateOfficerTransfer.base64);
                filename = "TransferOrder-" + userProfileMappingId.Value.ToString() + ".pdf";
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TransferOrder")).Root + filename;
                File.WriteAllBytes(SaveDirectoryPath, bytes);


            }

            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        //public async Task<GenericServiceResultTemplate> ResetPasswordbyAdmin(ResetPasswordViewModel resetPassword)
        //{
        //    GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
        //    try
        //    {
        //        var user = await _iDapperRepository.Get<string>("SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = @UserName", new { UserName = resetPassword.Username }, CommandType.Text).ConfigureAwait(false);
        //        if (user != null)
        //        {
        //            //var newpasswordhash = HashPassword(resetPassword.Password, user.FirstOrDefault().SecurityStamp);

        //        }
        //        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
        //        storeProcedureParms = new List<StoreProcedureParm>()
        //        {
        //            new StoreProcedureParm (){ ParmName="Username", ParmValue=resetPassword.Username.ToString(), isNumber=false},
        //        };
        //        var status = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_PasswordReset", storeProcedureParms);


        //    }

        //    catch (Exception ex)
        //    {
        //        genericServiceResultTemplate.HasException = true;
        //        genericServiceResultTemplate.Exceptions = ex;
        //    }
        //    return genericServiceResultTemplate;
        //}

        public async Task<GenericServiceResultTemplate> UpdateEmpanelledPersonProfileDetails(UpdateEmpanelledPersonProfileDetailsViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                UserProfile userProfile = await _context.UserProfiles.Where(x => x.UserProfileId == requestData.UserProfileId).FirstOrDefaultAsync();
                if(userProfile!= null)
                {
                    var fullName = requestData.OfficerFullName;
                    var firstName = fullName.Substring(0, fullName.IndexOf(" "));
                    var lastName = fullName.Substring(fullName.IndexOf(" ") + 1);

                    userProfile.FirstName = firstName;
                    userProfile.LastName = lastName;
                    userProfile.MobileNo = requestData.ContactNo;
                    userProfile.Email = requestData.Email;
                    _context.Update<UserProfile>(userProfile);
                    await _context.SaveChangesAsync();
                }

                UserArchitectAdditionalInfoMapping userArchitectAdditionalInfo = await _context.UserArchitectAdditionalInfoMappings.Where(x => x.UserRefId == requestData.Id).FirstOrDefaultAsync();
                if(userArchitectAdditionalInfo != null)
                {
                    userArchitectAdditionalInfo.RegistrationIssuedOn_COA = requestData.RegistrationIssuedOn;
                    userArchitectAdditionalInfo.RegistrationValidUpto_COA = requestData.RegistrationValidUpto;
                    _context.Update<UserArchitectAdditionalInfoMapping>(userArchitectAdditionalInfo);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterations: 1000))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(cb: 32));
            }
        }

        public async Task<GenericServiceResultTemplate> UpdateOfficerProfileDetails(UpdateOfficerProfileDetailsViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                UserProfile userProfile = await _context.UserProfiles.Where(x => x.UserProfileId == requestData.UserProfileId).FirstOrDefaultAsync();
                if (userProfile != null)
                {
                    var fullName = requestData.UserProfileName;
                    var firstName = fullName.Substring(0, fullName.IndexOf(" "));
                    var lastName = fullName.Substring(fullName.IndexOf(" ") + 1);

                    //userProfile.FirstName = firstName;
                    //userProfile.LastName = lastName;
                    userProfile.MobileNo = requestData.ContactNo;
                    userProfile.Email = requestData.Email;
                    _context.Update<UserProfile>(userProfile);
                    await _context.SaveChangesAsync();

                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                    storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="UserId", ParmValue=requestData.UserRefId, isNumber=false},
                        new StoreProcedureParm (){ ParmName="FirstName", ParmValue=firstName, isNumber=false},
                        new StoreProcedureParm (){ ParmName="MiddleName", ParmValue="", isNumber=false},
                        new StoreProcedureParm (){ ParmName="LastName", ParmValue=lastName, isNumber=false},
                        new StoreProcedureParm (){ ParmName="MobileNumber", ParmValue=requestData.ContactNo, isNumber=false},
                        new StoreProcedureParm (){ ParmName="Email", ParmValue=requestData.Email, isNumber=false},

                    };
                    var userProfileMappingId = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Legacy_UpdateOfficerProfileDetails", storeProcedureParms);
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }


        public async Task<GenericResponseTemplateModel<List<AppPaymentPart>>> SearchAppPaymentPartByAppRefId(Int64 appRefId)
        {
            GenericResponseTemplateModel<List<AppPaymentPart>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<AppPaymentPart>>();
            try
            {
                genericServiceResultTemplate.ResponseDataModel = await _context.AppPaymentParts.Where(x => x.AppRefId == appRefId && x.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.NEW).ToListAsync();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> UnlockedRTBFeeDetails(Int64 appRefId)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                var paymentParts = await _context.AppPaymentParts.Where(x => x.AppRefId == appRefId && x.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.NEW).ToListAsync();
                if (paymentParts != null)
                {
                    await _context.BulkDeleteAsync(paymentParts.ToList());
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
    }
}
