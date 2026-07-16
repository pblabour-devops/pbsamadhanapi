using EFCore.BulkExtensions;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.CustomRepositories
{
    public class BuildingPlanHUDRepository: IBuildingPlanHUDRepository
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_BuildingPlanHUDPaymentDetail;
        public BuildingPlanHUDRepository(AppDbContext context, IGenericRepository<BuildingPlanHUDPaymentDetail> iGR_BuildingPlanHUDPaymentDetail)
        {
            _context = context;
            _iGR_BuildingPlanHUDPaymentDetail = iGR_BuildingPlanHUDPaymentDetail;
        }
        public async Task<GenericResponseTemplateModel<ApplicationActionViewModel>> AddUpdate_RaiseFee(List<BuildingPlanHUDPaymentDetail> requestData, bool isForVerification, string remarks, bool isTimeLineFlow)
        {
            GenericResponseTemplateModel<ApplicationActionViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationActionViewModel>();
            try
            {
                genericServiceResultTemplate.HasError = false;
                if (requestData.Count > 0)
                {
                    var application = _context.Applications.Where(x => x.AppId == requestData.First().AppRefId && x.IsDeleted == false).FirstOrDefault();
                    requestData = requestData.Select(x => { x.PaymentBatchCounter = application.PaymentBatchCounter; return x; }).ToList();
                    var appAction = _context.ApplicationActions.Where(x => x.ApplicationRefId == requestData.First().AppRefId).FirstOrDefault();

                    if(isForVerification == true)
                    {
                        var applicationActionLogId = _context.BuildingPlanHUDPaymentDetails
                            .Where(x => 
                            x.AppRefId == requestData.Select(x => x.AppRefId).FirstOrDefault() && 
                            x.PaymentBatchCounter == requestData.Select(x => x.PaymentBatchCounter).FirstOrDefault())
                            .Select(x=>x.ApplicationActionLogId).FirstOrDefault();
                        
                        await _context.BulkUpdateAsync<BuildingPlanHUDPaymentDetail>(requestData);
                        
                        List<BuildingPlanHUDPaymentDetail_log> logDetail = new List<BuildingPlanHUDPaymentDetail_log>();
                        foreach (var item in requestData)
                        {
                            logDetail.Add(new BuildingPlanHUDPaymentDetail_log() 
                            {
                                AmountAlreadyPaid = item.AmountAlreadyPaid,
                                AmountRaised = item.AmountRaised,
                                ApplicationActionLogId = applicationActionLogId,
                                AppRefId = item.AppRefId,
                                Createddate = item.Createddate,
                                FeeHeaderRefId = item.FeeHeaderRefId,
                                IsFeeApplicable = item.IsFeeApplicable,
                                LastModifiedDate = item.LastModifiedDate,
                                NonTreasuryCode = item.NonTreasuryCode,
                                PaymentBatchCounter = item.PaymentBatchCounter,
                            });
                        }
                        await _context.BulkInsertAsync<BuildingPlanHUDPaymentDetail_log>(logDetail);
                    }
                    else
                    {
                        await _context.BulkInsertAsync<BuildingPlanHUDPaymentDetail>(requestData);
                    }
                    
                    var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                    string actionName = _context.ApplicationActionCodes.Where(x => x.ActionCode == 400).FirstOrDefault().ActionName;

                    if (isTimeLineFlow)
                    {
                        genericServiceResultTemplate.ResponseDataModel = new ApplicationActionViewModel()
                        {
                            AppActionType = appAction.AppActionType == 214 ? 215 : 401,
                            Receiver_UserRefId = projectSite.UserRefId,
                            Receiver_ProfileRefId = _context.UserProfileMapping.Where(x => x.UserRefId == projectSite.UserRefId).Select(x => x.UserProfileRefId).FirstOrDefault(),
                            Remarks = remarks,
                            AppRefId = requestData.First().AppRefId,
                            PdfNameGUID = "",
                            PublicAppRefNum = "",
                            CheckListFormJson = "",
                            PaymentBatchCounter = application.PaymentBatchCounter,
                            RaisedFeeAmount = 0,
                            RaisedFeeReason = "NA",
                            Workers_MaxDuringYear = 0,
                            PowerKW_Installed = 0,
                            ExistingWorkers_MaxDuringYear = 0,
                            ExistingPowerKW_Installed = 0
                        };

                        //var appActionLogs = _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == requestData.First().AppRefId && x.AppActionType == 401).OrderByDescending(x => x.ApplicationActionLogId).FirstOrDefault();
                        //var actionTimeLine = _context.AppActionTimeLines.Where(x => x.AppActionLogRefId == appActionLogs.ApplicationActionLogId).ToList();
                        //if (actionTimeLine != null)
                        //{
                        //    actionTimeLine = actionTimeLine.Select(x => { x.IsProcessed = true; return x; }).ToList();
                        //    await _context.BulkInsertOrUpdateAsync(actionTimeLine);
                        //}
                    }
                    else
                    {
                        if (isForVerification == false) // Forward to ADDF for raised fee verification
                        {
                            genericServiceResultTemplate.ResponseDataModel = new ApplicationActionViewModel()
                            {
                                AppActionType = appAction.AppActionType == 214 ? 215 :  400,
                                Receiver_UserRefId = appAction.AppActionType == 214 ? projectSite.UserRefId : "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9", // ADDF,
                                Receiver_ProfileRefId = _context.UserProfileMapping.Where(x => x.UserRefId == "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" && x.IsActive == true).Select(x => x.UserProfileRefId).FirstOrDefault(),
                                Remarks = remarks,
                                AppRefId = requestData.First().AppRefId,
                                PdfNameGUID = "",
                                PublicAppRefNum = "",
                                CheckListFormJson = "",
                                PaymentBatchCounter = application.PaymentBatchCounter,
                                RaisedFeeAmount = 0,
                                RaisedFeeReason = "NA",
                                Workers_MaxDuringYear = 0,
                                PowerKW_Installed = 0,
                                ExistingWorkers_MaxDuringYear = 0,
                                ExistingPowerKW_Installed = 0
                            };
                        }
                        else
                        {
                            genericServiceResultTemplate.ResponseDataModel = new ApplicationActionViewModel()
                            {
                                AppActionType = appAction.AppActionType == 214 ? 215 : 401,
                                Receiver_UserRefId = projectSite.UserRefId,
                                Receiver_ProfileRefId = _context.UserProfileMapping.Where(x => x.UserRefId == projectSite.UserRefId).Select(x => x.UserProfileRefId).FirstOrDefault(),
                                Remarks = remarks,
                                AppRefId = requestData.First().AppRefId,
                                PdfNameGUID = "",
                                PublicAppRefNum = "",
                                CheckListFormJson = "",
                                PaymentBatchCounter = application.PaymentBatchCounter,
                                RaisedFeeAmount = 0,
                                RaisedFeeReason = "NA",
                                Workers_MaxDuringYear = 0,
                                PowerKW_Installed = 0,
                                ExistingWorkers_MaxDuringYear = 0,
                                ExistingPowerKW_Installed = 0
                            };
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

        public async Task<GenericFormModel<List<BuildingPlanHUDPaymentDetail>>> Get_RaisedFeeList(long appRefId, long paymentBatchCounter)
        {
            GenericFormModel<List<BuildingPlanHUDPaymentDetail>> genericFormModel = new GenericFormModel<List<BuildingPlanHUDPaymentDetail>>();
            try
            {
                if (paymentBatchCounter > -1)
                {
                    paymentBatchCounter = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault().PaymentBatchCounter;

                }

                var parentObject = await _iGR_BuildingPlanHUDPaymentDetail.GetAsync(x => x.AppRefId == appRefId && x.PaymentBatchCounter == (paymentBatchCounter > 0 ? paymentBatchCounter : x.PaymentBatchCounter), null, x => x.FeesHeader).ConfigureAwait(false);
                genericFormModel.FormModel = parentObject.ToList();
                //genericFormModel.FormModel
                genericFormModel.FormModel = genericFormModel.FormModel.Select(x => { x.HasDedicatedTreasuryCode = x.FeesHeader.HasDedicatedTreasuryCode; x.DedicatedTreasurCode = x.FeesHeader.DedicatedTreasurCode.Trim(); x.DedicatedDDOCode = x.FeesHeader.DedicatedDDOCode; return x; }).ToList();
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<bool>> Update_RaisedFeeDetail(BuildingPlanHUDPaymentDetailViewModal requestData)
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
