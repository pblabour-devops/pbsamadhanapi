using Microsoft.EntityFrameworkCore; 
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class AutoApproveProcessService : IAutoApproveProcessService
    {
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private IPdfOprationsService _IPdfOprations;
        private readonly IApplicationManagementService<ApplicationAction> _iApplicationMamnagementService;
        public AutoApproveProcessService(AppDbContext context, 
            IGeneric_SP_Repository iGeneric_SP_Repository, 
            IPdfOprationsService IPdfOprations,
            IApplicationManagementService<ApplicationAction> iApplicationManagementService)
        {
            _IPdfOprations = IPdfOprations;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iApplicationMamnagementService = iApplicationManagementService;
        }
        public async Task<GenericResponseTemplateModel<bool>> GenerateCertificates(AutoApproveActionParmsViewModel requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                var autoapproveAbleFiles = await _context.AutoApprove_ProcessFilesLogs
                    .Where(x => x.AutoApproveProcessStatusType == AutoApproveProcessStatusTypeEnum.PENDING && x.ApplicationType == requestData.ApplicationType)
                    .Include(x=>x.Application).ThenInclude(x=>x.ProjectSites)
                    .ToListAsync();

                foreach (var item in autoapproveAbleFiles)
                {
                    try
                    {
                        var userName = _context.Users.Where(x => x.Id == item.Officer_UserRefId).Select(x => x.UserName).FirstOrDefault();
                        CertificateGenerateServiceResultTemplate certResp = await _IPdfOprations.GenerateCertificate(item.AppRefId, requestData.ApplicationType, userName, true);
                        if (certResp.HasException)
                        {
                            item.AutoApproveProcessStatusType = AutoApproveProcessStatusTypeEnum.ERROR;
                            item.AutoApproveProcessRemarks = certResp.Exceptions;
                        }
                        else
                        {
                            ApplicationActionViewModel applicationAction = new ApplicationActionViewModel()
                            {
                                AppActionType = (int)AppActionTypeEnum.AUTO_APPROVAL_COMPLETED,
                                AppDocumentRefId = 0,
                                AppRefId = item.AppRefId,
                                CheckListFormJson = null,
                                IsDocumentUploaded = false,
                                PdfNameGUID = certResp.PdfNameGUID,

                                PublicAppRefNum = item.Application.PublicAppRefNum,
                                Receiver_ProfileRefId = 0,
                                Receiver_UserRefId = item.Application.ProjectSites.UserRefId,

                                Remarks = "Application approved as Auto Approved..",
                                UserId = "7B35AB04-5FFF-4C17-951E-7C653E71484F"
                            };
                            var resp = await _iApplicationMamnagementService.RecordApplicationAction(applicationAction, "7B35AB04-5FFF-4C17-951E-7C653E71484F");
                            if (resp.HasError)
                            {
                                item.AutoApproveProcessStatusType = AutoApproveProcessStatusTypeEnum.ERROR;
                                item.AutoApproveProcessRemarks = resp.ErrorDesc;
                            }
                            else
                            {
                                item.AutoApproveProcessStatusType = AutoApproveProcessStatusTypeEnum.COMPLETED;
                                item.AutoApproveProcessRemarks = "Auto Approve process completed";
                                item.CertificateFileName = item.Application.PublicAppRefNum;
                            }
                        }
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="AutoApproveId", ParmValue= item.AutoApproveId.ToString(), isNumber=true },
                            new StoreProcedureParm (){ ParmName="AutoApproveProcessStatusType", ParmValue= ((int)item.AutoApproveProcessStatusType).ToString(), isNumber=true },
                            new StoreProcedureParm (){ ParmName="AutoApproveProcessRemarks", ParmValue= item.AutoApproveProcessRemarks.ToString(), isNumber=false }
                        };
                        var resp1 = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_AutoApprove_UpdateFileLogs", storeProcedureParms);
                    }
                    catch(Exception ex)
                    {
                        throw ex;
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
