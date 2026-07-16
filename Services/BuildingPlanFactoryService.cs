using EFCore.BulkExtensions;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
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
    public class BuildingPlanFactoryService : IBuildingPlanFactoryService
    {
        private readonly IGenericRepository<BuildingPlanFactory_GeneralDetail> _iGR_BuildingPlanFactory_GeneralDetail;
        private readonly IApplicationManagementService<BuildingPlanFactory_GeneralDetail> _iApplicationMamnagementService;
        private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_AppFeeDetail;
        private readonly IPaymentManagerRepository _iPaymentManagerRepository;
        private readonly IPaymentManagerService _iPaymentManagerService;
        private readonly IBuildingPlanFactoryRepository _iBuildingPlanFactoryRepository;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly IProjectSiteService _iProjectSiteService;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<BuildingPlanFactory_Declaration_Stability_Certificate> _iGR_BuildingPlanFactory_Declaration_Stability_Certificate;
        private readonly IApplicationManagementService<BuildingPlanFactory_Declaration_Stability_Certificate> _iDeclaration_Stability_Certificate_ApplicationMamnagementService;
        private IAuthService _iAuthService;
        public BuildingPlanFactoryService(IGenericRepository<BuildingPlanFactory_GeneralDetail> iGR_BuildingPlanFactory_GeneralDetail,
             IApplicationManagementService<BuildingPlanFactory_GeneralDetail> IApplicationMamnagementService,
              IGenericRepository<BuildingPlanHUDPaymentDetail> iGR_AppFeeDetail,
             IPaymentManagerRepository iPaymentManagerRepository,
             IPaymentManagerService iPaymentManagerService,
             IBuildingPlanFactoryRepository iBuildingPlanFactoryRepository,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             IProjectSiteService iProjectSiteService,
             AppDbContext context,
             IGenericRepository<BuildingPlanFactory_Declaration_Stability_Certificate> iGR_BuildingPlanFactory_Declaration_Stability_Certificate,
             IApplicationManagementService<BuildingPlanFactory_Declaration_Stability_Certificate> iDeclaration_Stability_Certificate_ApplicationMamnagementService
,             IAuthService iAuthService)
        {
            _iGR_BuildingPlanFactory_GeneralDetail = iGR_BuildingPlanFactory_GeneralDetail;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iGR_AppFeeDetail = iGR_AppFeeDetail;
            _iPaymentManagerRepository = iPaymentManagerRepository;
            _iPaymentManagerService = iPaymentManagerService;
            _iBuildingPlanFactoryRepository = iBuildingPlanFactoryRepository;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iProjectSiteService = iProjectSiteService;
            _context = context;
            _iGR_BuildingPlanFactory_Declaration_Stability_Certificate = iGR_BuildingPlanFactory_Declaration_Stability_Certificate;
            _iDeclaration_Stability_Certificate_ApplicationMamnagementService = iDeclaration_Stability_Certificate_ApplicationMamnagementService;
            _iAuthService = iAuthService;
        }
        public async Task<GenericFormModel<BuildingPlanFactory_GeneralDetail>> GetBuildingPlanFactoryGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<BuildingPlanFactory_GeneralDetail> genericFormModel = new GenericFormModel<BuildingPlanFactory_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_BuildingPlanFactory_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.IPin =Convert.ToInt64(genericFormModel.FormModel.Application.InvestPunjab_Ipin);
                    genericFormModel.FormModel.InvestPunjab_AppId = genericFormModel.FormModel.Application.InvestPunjab_AppId;
                }
                
                else //New Record
                {
                    genericFormModel.FormModel = new BuildingPlanFactory_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN, (genericFormModel.FormModel != null ? genericFormModel.FormModel.BuildingPlanFactoryId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(BuildingPlanFactory_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<BuildingPlanFactory_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.BuildingPlanFactoryId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_BuildingPlanFactory_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_BuildingPlanFactory_GeneralDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BuildingPlanFactoryId;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<BuildingPlanFactory_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin,formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                        if(genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated== true)
                        {
                            // Update Native Appid by IPIN

                            var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN, formModel.ApplicationPurposeType);


                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN, AppActionTypeEnum.APP_SAVE_DRAFT);
                        }
                        else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                        {
                            genericServiceResultTemplate.HasException = true;
                            genericServiceResultTemplate.Exceptions = new Exception();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        #region Details
        public async Task<GenericFormModel<BuildingPlanFactoryViewModel>> GetBuildingPlanFactoryDetail(long id)
        {
            GenericFormModel<BuildingPlanFactoryViewModel> genericFormModel = new GenericFormModel<BuildingPlanFactoryViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new BuildingPlanFactoryViewModel();
                genericFormModel.FormModel.GeneralDetail = new BuildingPlanFactory_GeneralDetail();

                var parentWithChildObject = await _iGR_BuildingPlanFactory_GeneralDetail
                        .GetAsync(x => x.AppRefId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Application)      //Includes
                        .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
               
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.BuildingPlanFactoryId : id), "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Details

        #region Lock Application
        public async Task<GenericServiceResultTemplate> LockApplication(Int64 appRefId, int AppActionType, string remarks)
        {
            GenericResponseTemplateModel<Application> genericServiceResultTemplateApplications = new GenericResponseTemplateModel<Application>();
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = new CustomeValidationResult();
                genericServiceResultTemplate.CustomeValidationResult.IsValid = true;
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                if (appRefId != 0)
                {

                    genericServiceResultTemplateApplications.ResponseDataModel = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
                    var ApplicationType = genericServiceResultTemplateApplications.ResponseDataModel.ApplicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE ? ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE : ApplicationTypeEnum.BUILDING_PLAN;

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE) !="")
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = true;
                    }
                    else
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = false;
                        genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
                    }
                }
                else
                {
                    genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        #endregion Lock Application

        #region Building Plan Factory Raise Fee
        public async Task<GenericFormModel<RaiseFeeParmsViewModel>> GetBuildingPlanFactoryRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = new GenericFormModel<RaiseFeeParmsViewModel>();
            try
            {
                genericFormModel.FormModel = new RaiseFeeParmsViewModel();
                Int64 feeHeaderId = 0;
                FeesHeader feesHeader = new FeesHeader();

                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN)
                {
                    feeHeaderId = 19;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId,  ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms = new List<FeeCalculatorInfoParmsViewModel>();
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0
                    });

                }

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<RaiseFeeParmsViewModel>> VerifyBuildingPlanFactoryRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, int paymentBatchCounter)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = new GenericFormModel<RaiseFeeParmsViewModel>();
            try
            {
                paymentBatchCounter = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault().PaymentBatchCounter;
                var parentObject = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter);

                genericFormModel.FormModel = new RaiseFeeParmsViewModel();
                Int64 feeHeaderId = 0;
                FeesHeader feesHeader = new FeesHeader();

                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN)
                {
                    feeHeaderId = 19;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms = new List<FeeCalculatorInfoParmsViewModel>();
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        //AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 19).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentBatchCounter = paymentBatchCounter
                    });
                }

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<ApplicationActionViewModel>> AddUpdate_BuildingPlanFactoryRaiseFee(RaiseFeeParmsViewModel requestData)
        {
            GenericResponseTemplateModel<ApplicationActionViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationActionViewModel>();
            try
            {
                genericServiceResultTemplate.HasError = false;
               
                if (requestData.FeeCalculatorInfoParms.Count > 0)
                {
                    List<BuildingPlanHUDPaymentDetail> paymentDetailList = new List<BuildingPlanHUDPaymentDetail>();
                    int paymentBatchCounter = requestData.FeeCalculatorInfoParms.FirstOrDefault().PaymentBatchCounter;

                    if (paymentBatchCounter == 0)
                    {
                        if (requestData.IsForVerification == false)
                        {
                            var res = await _iApplicationMamnagementService.IncreasePaymentBatchCounter(requestData.FeeCalculatorInfoParms.FirstOrDefault().AppRefId);
                            paymentBatchCounter = res.ResponseDataModel;
                        }
                    }

                    foreach (var item in requestData.FeeCalculatorInfoParms)
                    {
                        paymentDetailList.Add(new BuildingPlanHUDPaymentDetail()
                        {
                            AmountRaised = item.AmountCalculated,
                            AmountAlreadyPaid = 0,
                            AppRefId = item.AppRefId,
                            FeeHeaderRefId = item.FeeHeaderId,
                            IsFeeApplicable = true,
                            PaymentBatchCounter = paymentBatchCounter,
                            Createddate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                            PaymentDetailId = item.PaymentDetailId
                        });
                    }
                    genericServiceResultTemplate = await _iBuildingPlanFactoryRepository.AddUpdate_RaiseFee(paymentDetailList, requestData.IsForVerification);
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<FeesHeader> GetFeeHeader(Int64 feeHeaderId)
        {
            return await _iPaymentManagerRepository.GetFeeHeader(feeHeaderId);
        }

        public async Task<GenericFormModel<List<BuildingPlanHUDPaymentDetail>>> Get_BuildingPlanFactoryRaisedFeeList(Int64 appRefId, Int64 paymentBatchCounter)
        {
            GenericFormModel<List<BuildingPlanHUDPaymentDetail>> genericFormModel = new GenericFormModel<List<BuildingPlanHUDPaymentDetail>>();
            try
            {
                genericFormModel = await _iBuildingPlanFactoryRepository.Get_RaisedFeeList(appRefId, paymentBatchCounter);
                if (!genericFormModel.HasError && genericFormModel.FormModel.Count() > 0)
                {
                    genericFormModel.FormModel = genericFormModel.FormModel.Select(x => { x.AmountPayable = x.AmountRaised - x.AmountAlreadyPaid; return x; }).ToList();
                    genericFormModel.FormModel[genericFormModel.FormModel.FindIndex(x => x.FeeHeaderRefId == 19)].Description = "1 % of building cost as per CA certificate";
                    genericFormModel.FormModel[genericFormModel.FormModel.FindIndex(x => x.FeeHeaderRefId == 20)].Description = "Rs 60 per square meter of covered area.";

                }
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> Update_BuildingPlanFactoryRaisedFeeDetail(BuildingPlanFactoryPaymentDetailViewModal requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>();
            try
            {
                genericServiceResultTemplate.HasError = false;
                genericServiceResultTemplate = await _iBuildingPlanFactoryRepository.Update_RaisedFeeDetail(requestData);
                genericServiceResultTemplate.ResponseDataModel = true;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        #endregion

        #region Declaration Stability Certificate
        public async Task<GenericFormModel<BuildingPlanFactory_Declaration_Stability_Certificate>> GetDeclarationStabilityCertificate(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<BuildingPlanFactory_Declaration_Stability_Certificate> genericFormModel = new GenericFormModel<BuildingPlanFactory_Declaration_Stability_Certificate>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                var competentPersonLists = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");
                var empaneledEngineersLists = await _iAuthService.GetOfficerDetailsByRoleName("ENGR");
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_BuildingPlanFactory_Declaration_Stability_Certificate
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                                        genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.IPin = Convert.ToInt64(genericFormModel.FormModel.Application.InvestPunjab_Ipin);
                    genericFormModel.FormModel.InvestPunjab_AppId = genericFormModel.FormModel.Application.InvestPunjab_AppId;
                    genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                    genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                    {
                        SelectListTypeCode = "BuildingPlanStabilityAuthorityTypeEnum",
                        SelectListItems = EnumOps.GetEnumAsSelectList<BuildingPlanStabilityAuthorityTypeEnum>()
                    });
                    //Initialization of list templates
    

                    genericFormModel.FormModel.CompetentPersonList = competentPersonLists.ListData;


                    genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersLists.ListData;

                }

                else //New Record
                {
                    genericFormModel.FormModel = new BuildingPlanFactory_Declaration_Stability_Certificate();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuildingPlanStabilityAuthorityTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuildingPlanStabilityAuthorityTypeEnum>()
                });

                //Initialization of list templates
                genericFormModel.FormModel.CompetentPersonList = competentPersonLists.ListData;
                genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersLists.ListData;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, (genericFormModel.FormModel != null ? genericFormModel.FormModel.DeclarationStabilityCertificateId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_DeclarationStabilityCertificate(BuildingPlanFactory_Declaration_Stability_Certificate formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<BuildingPlanFactory_Declaration_Stability_Certificate>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {

                    if (formModel.DeclarationStabilityCertificateId != 0) //Existing record
                    {
                        _iGR_BuildingPlanFactory_Declaration_Stability_Certificate.Update(formModel);

                        await _iGR_BuildingPlanFactory_Declaration_Stability_Certificate.SavechangeAsync();

                        await _iProjectSiteService.SetProjectSiteCircle(formModel.ProjectSiteRefId, formModel.FactoryCircleRefId, 0,0 );

                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.DeclarationStabilityCertificateId;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                    }
                    else //New record
                    {

                        formModel = GenericModelOps<BuildingPlanFactory_Declaration_Stability_Certificate>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iDeclaration_Stability_Certificate_ApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);
                        var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                        await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, formModel.FactoryCircleRefId, 0,0);


                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                        {

                            await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, formModel.ApplicationPurposeType);

                            var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, ApplicationPurposeTypeEnum.GRANT_LICENCE);

                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, AppActionTypeEnum.APP_SAVE_DRAFT);
                        }
                        else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                        {
                            genericServiceResultTemplate.HasException = true;
                            genericServiceResultTemplate.Exceptions = new Exception();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericFormModel<BuildingPlanFactory_Declaration_Stability_CertificateViewModel>> GetDeclarationStabilityCertificateDetail(long id)
        {
            GenericFormModel<BuildingPlanFactory_Declaration_Stability_CertificateViewModel> genericFormModel = new GenericFormModel<BuildingPlanFactory_Declaration_Stability_CertificateViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new BuildingPlanFactory_Declaration_Stability_CertificateViewModel();
                genericFormModel.FormModel.GeneralDetail = new BuildingPlanFactory_Declaration_Stability_Certificate();

                var parentWithChildObject = await _iGR_BuildingPlanFactory_Declaration_Stability_Certificate
                                .GetAsync(x => x.AppRefId == id,  //Conditions         
                                  null,                //Orders
                        x => x.Application, x => x.Application.ApplicationAction)   
                                                   .ConfigureAwait(false);

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                genericFormModel.FormModel.GeneralDetail.FactoryCircleRefId = projectProfileInfo.FormModel.FactoryCircleRefId;
                genericFormModel.FormModel.GeneralDetail.DistrictRefId = projectProfileInfo.FormModel.DistrictRefId;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.DeclarationStabilityCertificateId : id), "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        
        #endregion
    }
}
