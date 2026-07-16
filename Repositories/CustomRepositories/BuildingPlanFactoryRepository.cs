using EFCore.BulkExtensions;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.CustomRepositories
{
    public class BuildingPlanFactoryRepository : IBuildingPlanFactoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_BuildingPlanHUDPaymentDetail;
        public BuildingPlanFactoryRepository(AppDbContext context, IGenericRepository<BuildingPlanHUDPaymentDetail> iGR_BuildingPlanHUDPaymentDetail)
        {
            _context = context;
            _iGR_BuildingPlanHUDPaymentDetail = iGR_BuildingPlanHUDPaymentDetail;
        }
        public async Task<GenericResponseTemplateModel<ApplicationActionViewModel>> AddUpdate_RaiseFee(List<BuildingPlanHUDPaymentDetail> requestData, bool isForVerification)
        {
            GenericResponseTemplateModel<ApplicationActionViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationActionViewModel>();
            try
            {
                genericServiceResultTemplate.HasError = false;
                if (requestData.Count > 0)
                {
                    var application = _context.Applications.Where(x => x.AppId == requestData.First().AppRefId && x.IsDeleted == false).FirstOrDefault();
                    requestData = requestData.Select(x => { x.PaymentBatchCounter = application.PaymentBatchCounter; return x; }).ToList();

                    if(isForVerification == true)
                    {
                        await _context.BulkUpdateAsync<BuildingPlanHUDPaymentDetail>(requestData);
                    }
                    else
                    {
                        await _context.BulkInsertAsync<BuildingPlanHUDPaymentDetail>(requestData);
                    }
                    
                    var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                    string actionName = _context.ApplicationActionCodes.Where(x => x.ActionCode == 400).FirstOrDefault().ActionName;

                    if(isForVerification == false) // Forward to ADDF for raised fee verification
                    {
                        genericServiceResultTemplate.ResponseDataModel = new ApplicationActionViewModel()
                        {
                            AppActionType = 400,
                            Receiver_UserRefId = "1aee535c-4794-4e7b-acaa-e5a0acf4af33", // projectSite.UserRefId,
                            Remarks = actionName,
                            AppRefId = requestData.First().AppRefId,
                            PdfNameGUID = "",
                            PublicAppRefNum = "",
                            CheckListFormJson = ""
                        };
                    }
                    else
                    {
                        genericServiceResultTemplate.ResponseDataModel = new ApplicationActionViewModel()
                        {
                            AppActionType = 401,
                            Receiver_UserRefId = projectSite.UserRefId,
                            Remarks = actionName,
                            AppRefId = requestData.First().AppRefId,
                            PdfNameGUID = "",
                            PublicAppRefNum = "",
                            CheckListFormJson = ""
                        };
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

        public async Task<GenericFormModel<List<BuildingPlanHUDPaymentDetail>>> Get_RaisedFeeList(long appRefId, long paymentBatchCounter)
        {
            GenericFormModel<List<BuildingPlanHUDPaymentDetail>> genericFormModel = new GenericFormModel<List<BuildingPlanHUDPaymentDetail>>();
            try
            {
                paymentBatchCounter = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault().PaymentBatchCounter;

                var parentObject = await _iGR_BuildingPlanHUDPaymentDetail.GetAsync(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter, null, x => x.FeesHeader).ConfigureAwait(false);
                genericFormModel.FormModel = parentObject.ToList();
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<bool>> Update_RaisedFeeDetail(BuildingPlanFactoryPaymentDetailViewModal requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>();
            try
            {
                genericServiceResultTemplate.HasError = false;
                if (requestData.BuildingPlanHUDPaymentDetailList.Count > 0)
                {
                    await _context.BulkUpdateAsync<BuildingPlanHUDPaymentDetail>(requestData.BuildingPlanHUDPaymentDetailList);
                    if (requestData.AppDocRefId > 0)
                    {
                        var appDocument = _context.ApplicationDocuments.Where(x => x.AppDocId == requestData.AppDocRefId).FirstOrDefault();
                        appDocument.IsLocked = true;
                        _context.ApplicationDocuments.Update(appDocument);
                        await _context.SaveChangesAsync();
                    }
                    genericServiceResultTemplate.ResponseDataModel = true;
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
