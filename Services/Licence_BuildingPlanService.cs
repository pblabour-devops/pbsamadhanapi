using EFCore.BulkExtensions;
using Microsoft.Extensions.Configuration;
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
    public class Licence_BuildingPlanService : ILicence_BuildingPlanService
    {
        private readonly IGenericRepository<Licence_Proposed_BuildingPlan_GeneralDetail> _iGR_Licence_Proposed_BuildingPlan_GeneralDetail;
        private readonly IGenericRepository<Licence_Existing_BuildingPlan_GeneralDetail> _iGR_Licence_Existing_BuildingPlan_GeneralDetail;
        private readonly IGenericRepository<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> _iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail;
        private readonly IApplicationManagementService<Licence_Proposed_BuildingPlan_GeneralDetail> _iApplicationMamnagementService;
        private readonly IApplicationManagementService<Licence_Existing_BuildingPlan_GeneralDetail> _i_BPE_ApplicationManagementService;
        private readonly IApplicationManagementService<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> _i_BPAA_ApplicationManagementService;
        private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_AppFeeDetail;
        private readonly IPaymentManagerRepository _iPaymentManagerRepository;
        private readonly IPaymentManagerService _iPaymentManagerService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly AppDbContext _context;
        private readonly IProjectSiteService _iProjectSiteService;
        private IAuthService _iAuthService;
        private IConfiguration _iConfiguration { get; }
        private readonly IBuildingPlanFactoryRepository _iBuildingPlanFactoryRepository;

        public Licence_BuildingPlanService(IGenericRepository<Licence_Proposed_BuildingPlan_GeneralDetail> iGR_Licence_Proposed_BuildingPlan_GeneralDetail,
             IGenericRepository<Licence_Existing_BuildingPlan_GeneralDetail> iGR_Licence_Existing_BuildingPlan_GeneralDetail,
             IGenericRepository<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail,
             IApplicationManagementService<Licence_Proposed_BuildingPlan_GeneralDetail> IApplicationMamnagementService,
             ApplicationManagementService<Licence_Existing_BuildingPlan_GeneralDetail> i_BPE_ApplicationManagementService,
             ApplicationManagementService<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> i_BPAA_ApplicationManagementService,
             IGenericRepository<BuildingPlanHUDPaymentDetail> iGR_AppFeeDetail,
             IPaymentManagerRepository iPaymentManagerRepository,
             IPaymentManagerService iPaymentManagerService,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             AppDbContext context,
             IProjectSiteService iProjectSiteService,
             IAuthService authService,
             IConfiguration iConfiguration,
             IBuildingPlanFactoryRepository iBuildingPlanFactoryRepository)
        {
            _iGR_Licence_Proposed_BuildingPlan_GeneralDetail = iGR_Licence_Proposed_BuildingPlan_GeneralDetail;
            _iGR_Licence_Existing_BuildingPlan_GeneralDetail = iGR_Licence_Existing_BuildingPlan_GeneralDetail;
            _iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail = iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail;
            _i_BPE_ApplicationManagementService = i_BPE_ApplicationManagementService;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _i_BPAA_ApplicationManagementService = i_BPAA_ApplicationManagementService;
            _iGR_AppFeeDetail = iGR_AppFeeDetail;
            _iPaymentManagerRepository = iPaymentManagerRepository;
            _iPaymentManagerService = iPaymentManagerService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _context = context;
            _iProjectSiteService = iProjectSiteService;
            _iAuthService = authService;
            _iConfiguration = iConfiguration;
            _iBuildingPlanFactoryRepository = iBuildingPlanFactoryRepository;
        }

        #region Proposed Building Plan
        public async Task<GenericFormModel<Licence_Proposed_BuildingPlan_GeneralDetail>> GetProposedBuildingPlanGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<Licence_Proposed_BuildingPlan_GeneralDetail> genericFormModel = new GenericFormModel<Licence_Proposed_BuildingPlan_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_Proposed_BuildingPlan_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x => x.Application.ApplicationAction, x=> x.Application.ProjectSites)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.IPin =Convert.ToInt64(genericFormModel.FormModel.Application.InvestPunjab_Ipin);
                    genericFormModel.FormModel.InvestPunjab_AppId = genericFormModel.FormModel.Application.InvestPunjab_AppId;
                    genericFormModel.FormModel.FactoryCircleRefId = genericFormModel.FormModel.Application.ProjectSites.FactoryCircleRefId;
                }
                
                else //New Record
                {
                    genericFormModel.FormModel = new Licence_Proposed_BuildingPlan_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                //Initialization of Enum types of lists
                
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "IndustryColorCodeByPPCBTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<IndustryColorCodeByPPCBTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuildingPlanApprovalAuthorityTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuildingPlanApprovalAuthorityTypeEnum>()
                });

                var competentPersonList = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");
                genericFormModel.FormModel.CompetentPersonList = competentPersonList.ListData;

                var empaneledArchitectsList = await _iAuthService.GetOfficerDetailsByRoleName("ARCH");
                genericFormModel.FormModel.EmpaneledArchitectsList = empaneledArchitectsList.ListData;

                var empaneledEngineersList = await _iAuthService.GetOfficerDetailsByRoleName("ENGR");
                genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersList.ListData;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, (genericFormModel.FormModel != null ? genericFormModel.FormModel.ProposedBuildingPlanId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_Proposed_BuildingPlan_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_Proposed_BuildingPlan_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.ProposedBuildingPlanId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_Licence_Proposed_BuildingPlan_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_Licence_Proposed_BuildingPlan_GeneralDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.ProposedBuildingPlanId;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Licence_Proposed_BuildingPlan_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin,formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                        if(genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated== true)
                        {
                            var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, formModel.FactoryCircleRefId, 0,0);

                            // Update Native Appid by IPIN
                            var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, AppActionTypeEnum.APP_SAVE_DRAFT);
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
        public async Task<GenericFormModel<ProposedBuildingPlanViewModel>> GetProposedBuildingPlanDetail(long id)
        {
            GenericFormModel<ProposedBuildingPlanViewModel> genericFormModel = new GenericFormModel<ProposedBuildingPlanViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new ProposedBuildingPlanViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_Proposed_BuildingPlan_GeneralDetail();

                var parentWithChildObject = await _iGR_Licence_Proposed_BuildingPlan_GeneralDetail
                        .GetAsync(x => x.AppRefId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Application, x=> x.Application.ProjectSites)      //Includes
                        .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                genericFormModel.FormModel.GeneralDetail.DistrictRefId = parentWithChildObject.FirstOrDefault().Application.ProjectSites.DistrictRefId;
                genericFormModel.FormModel.GeneralDetail.FactoryCircleRefId = parentWithChildObject.FirstOrDefault().Application.ProjectSites.FactoryCircleRefId;
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.ProposedBuildingPlanId : id), "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion

        #region Existing Building Plan
        public async Task<GenericFormModel<Licence_Existing_BuildingPlan_GeneralDetail>> GetExistingBuildingPlanGeneralDetail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_Existing_BuildingPlan_GeneralDetail> genericFormModel = new GenericFormModel<Licence_Existing_BuildingPlan_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_Existing_BuildingPlan_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x => x.Application.ApplicationAction, x => x.Application.ProjectSites)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.IPin = Convert.ToInt64(genericFormModel.FormModel.Application.InvestPunjab_Ipin);
                    genericFormModel.FormModel.InvestPunjab_AppId = genericFormModel.FormModel.Application.InvestPunjab_AppId;
                    genericFormModel.FormModel.FactoryCircleRefId = genericFormModel.FormModel.Application.ProjectSites.FactoryCircleRefId;
                }

                else //New Record
                {
                    genericFormModel.FormModel = new Licence_Existing_BuildingPlan_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                //Initialization of Enum types of lists

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "IndustryColorCodeByPPCBTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<IndustryColorCodeByPPCBTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuildingPlanApprovalAuthorityTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuildingPlanApprovalAuthorityTypeEnum>()
                });

                var competentPersonList = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");
                genericFormModel.FormModel.CompetentPersonList = competentPersonList.ListData;

                //var empaneledArchitectsList = await _iAuthService.GetOfficerDetailsByRoleName("ARCH");
                //genericFormModel.FormModel.EmpaneledArchitectsList = empaneledArchitectsList.ListData;

                var empaneledEngineersList = await _iAuthService.GetOfficerDetailsByRoleName("ENGR");
                genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersList.ListData;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_EXISTING, (genericFormModel.FormModel != null ? genericFormModel.FormModel.ExistingBuildingPlanId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_ExistingBuildingPlanGeneralDetail(Licence_Existing_BuildingPlan_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_Existing_BuildingPlan_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.ExistingBuildingPlanId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_Licence_Existing_BuildingPlan_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_Licence_Existing_BuildingPlan_GeneralDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.ExistingBuildingPlanId;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Licence_Existing_BuildingPlan_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _i_BPE_ApplicationManagementService.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_EXISTING, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null,formModel.ProjectSiteVersion);
                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                        {
                            var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, formModel.FactoryCircleRefId, 0,0);

                            // Update Native Appid by IPIN
                            var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_EXISTING, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_EXISTING, AppActionTypeEnum.APP_SAVE_DRAFT);
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
        public async Task<GenericFormModel<ExistingBuildingPlanViewModel>> GetExistingBuildingPlanDetail(long id)
        {
            GenericFormModel<ExistingBuildingPlanViewModel> genericFormModel = new GenericFormModel<ExistingBuildingPlanViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new ExistingBuildingPlanViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_Existing_BuildingPlan_GeneralDetail();

                var parentWithChildObject = await _iGR_Licence_Existing_BuildingPlan_GeneralDetail
                        .GetAsync(x => x.AppRefId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Application, x => x.Application.ProjectSites)      //Includes
                        .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                genericFormModel.FormModel.GeneralDetail.DistrictRefId = parentWithChildObject.FirstOrDefault().Application.ProjectSites.DistrictRefId;
                genericFormModel.FormModel.GeneralDetail.FactoryCircleRefId = parentWithChildObject.FirstOrDefault().Application.ProjectSites.FactoryCircleRefId;
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_EXISTING, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.ExistingBuildingPlanId : id), "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion

        #region Addition_Amendment Building Plan
        public async Task<GenericFormModel<Licence_Addition_Amendment_BuildingPlan_GeneralDetail>> GetAddition_AmendmentBuildingPlanGeneralDetail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> genericFormModel = new GenericFormModel<Licence_Addition_Amendment_BuildingPlan_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x => x.Application.ApplicationAction, x => x.Application.ProjectSites)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.IPin = Convert.ToInt64(genericFormModel.FormModel.Application.InvestPunjab_Ipin);
                    genericFormModel.FormModel.InvestPunjab_AppId = genericFormModel.FormModel.Application.InvestPunjab_AppId;
                    genericFormModel.FormModel.FactoryCircleRefId = genericFormModel.FormModel.Application.ProjectSites.FactoryCircleRefId;
                }

                else //New Record
                {
                    genericFormModel.FormModel = new Licence_Addition_Amendment_BuildingPlan_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                //Initialization of Enum types of lists

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "IndustryColorCodeByPPCBTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<IndustryColorCodeByPPCBTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuildingPlanApprovalAuthorityTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuildingPlanApprovalAuthorityTypeEnum>()
                });
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ExistingBuildingPlanTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ExistingBuildingPlanTypeEnum>()
                });

                var competentPersonList = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");
                genericFormModel.FormModel.CompetentPersonList = competentPersonList.ListData;

                //var empaneledArchitectsList = await _iAuthService.GetOfficerDetailsByRoleName("ARCH");
                //genericFormModel.FormModel.EmpaneledArchitectsList = empaneledArchitectsList.ListData;

                var empaneledEngineersList = await _iAuthService.GetOfficerDetailsByRoleName("ENGR");
                genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersList.ListData;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, (genericFormModel.FormModel != null ? genericFormModel.FormModel.Addition_AmendmentBuildingPlanId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_Addition_AmendmentBuildingPlanGeneralDetail(Licence_Addition_Amendment_BuildingPlan_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_Addition_Amendment_BuildingPlan_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.Addition_AmendmentBuildingPlanId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.Addition_AmendmentBuildingPlanId;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Licence_Addition_Amendment_BuildingPlan_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _i_BPAA_ApplicationManagementService.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null,formModel.ProjectSiteVersion);

                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                        {
                            var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, formModel.FactoryCircleRefId, 0,0);

                            // Update Native Appid by IPIN
                            var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, AppActionTypeEnum.APP_SAVE_DRAFT);
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
        public async Task<GenericFormModel<Addition_AmendmentBuildingPlanViewModel>> GetAddition_AmendmentBuildingPlanDetail(long id)
        {
            GenericFormModel<Addition_AmendmentBuildingPlanViewModel> genericFormModel = new GenericFormModel<Addition_AmendmentBuildingPlanViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Addition_AmendmentBuildingPlanViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_Addition_Amendment_BuildingPlan_GeneralDetail();

                var parentWithChildObject = await _iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail
                        .GetAsync(x => x.AppRefId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Application, x => x.Application.ProjectSites)      //Includes
                        .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                genericFormModel.FormModel.GeneralDetail.DistrictRefId = parentWithChildObject.FirstOrDefault().Application.ProjectSites.DistrictRefId;
                genericFormModel.FormModel.GeneralDetail.FactoryCircleRefId = parentWithChildObject.FirstOrDefault().Application.ProjectSites.FactoryCircleRefId;
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.Addition_AmendmentBuildingPlanId : id), "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion

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

                    // Convert the int to the enum type
                    ApplicationTypeEnum applicationType = (ApplicationTypeEnum)genericServiceResultTemplateApplications.ResponseDataModel.ApplicationType;

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, applicationType) !="")
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

        #region Raise Fee
        public async Task<GenericFormModel<RaiseFeeParmsViewModel>> GetBuildingPlanFactoryRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = new GenericFormModel<RaiseFeeParmsViewModel>();
            try
            {
                genericFormModel.FormModel = new RaiseFeeParmsViewModel();
                Int64 feeHeaderId = 0;
                FeesHeader feesHeader = new FeesHeader();

                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED)
                {
                    feeHeaderId = 19;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
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
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = false,
                        NonTreasuryCode = _iConfiguration.GetSection("PaymentGatewayConfigs").GetSection("NonTreasuryCodes").GetSection("BOCWCess").Value
                    });
                }

                genericFormModel.FormModel.FeeCalculatorInfoParms = genericFormModel.FormModel.FeeCalculatorInfoParms.OrderBy(x => x.IsTreasuryPayment).ToList();

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
                            PaymentDetailId = item.PaymentDetailId,
                            NonTreasuryCode = item.NonTreasuryCode
                        });
                    }
                    //genericServiceResultTemplate = await _iBuildingPlanFactoryRepository.AddUpdate_RaiseFee(paymentDetailList, requestData.IsForVerification, requestData.Remarks);
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
        #endregion
    }
}
