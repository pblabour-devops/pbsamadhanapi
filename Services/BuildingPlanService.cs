using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace pbsamadhannetcoreapi.Services.Implementations
{
    public class BuildingPlanService : IBuildingPlanService
    {
        private readonly IGenericRepository<BuildingPlan> _iGR_BuildingPlanDetail;
        private readonly IGenericRepository<BuildingPlan_AreaDetail> _iGR_BuildingPlan_AreaDetail;
        private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IZoneRepository _iZoneRepository;
        private readonly IApplicationManagementService<BuildingPlan> _iApplicationMamnagementService;
       
        public BuildingPlanService(IGenericRepository<BuildingPlan> iBuildingPlanDetail,
            IGenericRepository<DistrictLgd> iGR_DistrictLgd, IZoneRepository iZoneRepository,
            IApplicationManagementService<BuildingPlan> IApplicationMamnagementService,
            AppDbContext context,
            IGenericRepository<BuildingPlan_AreaDetail> iBuildingPlan_AreaDetail)
        {
            _iGR_BuildingPlanDetail = iBuildingPlanDetail;
            _iGR_DistrictLgd = iGR_DistrictLgd;
            _iZoneRepository = iZoneRepository;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iGR_BuildingPlan_AreaDetail = iBuildingPlan_AreaDetail;
        }

        #region Building Plan Details
        public async Task<GenericFormModel<BuildingPlan>> GetBuildingPlanDetail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<BuildingPlan> genericFormModel = new GenericFormModel<BuildingPlan>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                //Initialization of list templates
                genericFormModel.ListTemplateLists.Add(new ListTemplate()
                {
                    ListTypeCode = "Districts",
                    ListItems = _iGR_DistrictLgd.GetAll().Select(x => new GenericListTemplate() { ID = x.DistrictLgdId, Text = x.DistrictName }).ToList()
                });
                // Check if this is an existing record
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_BuildingPlanDetail
                       .GetAsync(x => x.BuildingPlanId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;

                    //if form model is available
                    if (genericFormModel.FormModel != null)
                    {
                        var tehsils = await _iZoneRepository.GetTehsilsByDistrictRefId(genericFormModel.FormModel.Applicant_DistrictRefId);
                        genericFormModel.ListTemplateLists.Add(new ListTemplate()
                        {
                            ListTypeCode = "Applicant_Tehsils",
                            ListItems = tehsils.Select(x => new GenericListTemplate() { ID = x.ID, Text = x.Text }).ToList()
                        });
                    }
                }
                else //New Record
                {
                    genericFormModel.FormModel = new BuildingPlan();

                    // Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.AppRefId, ApplicationTypeEnum.BUILDING_PLAN_OSH, (genericFormModel.FormModel != null ? genericFormModel.FormModel.BuildingPlanId : id), "BP");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_BuildingPlanDetail(BuildingPlan formModel, string userName)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<BuildingPlan>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.BuildingPlanId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_BuildingPlanDetail.Update(formModel);
                        //Save changes
                        await _iGR_BuildingPlan_AreaDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BuildingPlanId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<BuildingPlan>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_OSH, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, userName,0, formModel.InvestPunjab_AppId, false, 0,0, null, null, formModel.ProjectSiteVersion);
                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
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
        #endregion

        #region BuildingPlan AreaDetail
        public async Task<GenericFormModel<List<BuildingPlan_AreaDetail>>> GetBuildingPlanAreaDetail(Int64 id)
        {
            GenericFormModel<List<BuildingPlan_AreaDetail>> genericFormModel = new GenericFormModel<List<BuildingPlan_AreaDetail>>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.ListTemplateLists.Add(new ListTemplate()
                {
                    ListTypeCode = "Districts",
                    ListItems = _iGR_DistrictLgd.GetAll().Select(x => new GenericListTemplate() { ID = x.DistrictLgdId, Text = x.DistrictName }).ToList()
                });
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_BuildingPlanDetail
                         .GetAsync(x => x.BuildingPlanId == id,  //Conditions         
                           null,                //Orders          
                           x => x.BuildingPlan_AreaDetail, x => x.Application)      //Includes
                         .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault().BuildingPlan_AreaDetail.ToList();
                    genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                    genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                }
                else //New Record
                {
                    genericFormModel.FormModel = new List<BuildingPlan_AreaDetail>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(0, ApplicationTypeEnum.BUILDING_PLAN_OSH, (genericFormModel.FormModel.Count() > 0 ? genericFormModel.FormModel.FirstOrDefault().BuildingPlanRefId : id), "AD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdateBuildingPlanAreaDetail(BuildingPlan_AreaDetail formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<BuildingPlan_AreaDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    formModel = GenericModelOps<BuildingPlan_AreaDetail>.SetNullAllNevigationProperties(formModel);
                    _iGR_BuildingPlan_AreaDetail.Insert(formModel);
                    await _iGR_BuildingPlan_AreaDetail.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        #endregion BuildingPlan AreaDetail

        #region Details
        public async Task<GenericFormModel<BuildingPlanViewModels>> GetBuildingPlanDetail(long id)
        {
            GenericFormModel<BuildingPlanViewModels> genericFormModel = new GenericFormModel<BuildingPlanViewModels>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new BuildingPlanViewModels();
                genericFormModel.FormModel.BuildingPlan = new BuildingPlan();
                genericFormModel.FormModel.BuildingPlan_AreaDetail = new BuildingPlan_AreaDetail();

                var parentWithChildObject = await _iGR_BuildingPlanDetail
                        .GetAsync(x => x.BuildingPlanId == id,  //Conditions         
                          null,                //Orders          
                          x => x.BuildingPlan_AreaDetail, x => x.Application)      //Includes
                        .ConfigureAwait(false);

                genericFormModel.FormModel.BuildingPlan = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.BuildingPlan_AreaDetail = parentWithChildObject.FirstOrDefault().BuildingPlan_AreaDetail.FirstOrDefault();

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.BuildingPlan.AppRefId, ApplicationTypeEnum.BUILDING_PLAN_OSH, (genericFormModel.FormModel != null ? genericFormModel.FormModel.BuildingPlan.BuildingPlanId : id), "LOCK");
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
        public async Task<GenericServiceResultTemplate> LockApplication(Int64 appRefId, int AppActionType,string remarks)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = new CustomeValidationResult();
                genericServiceResultTemplate.CustomeValidationResult.IsValid = true;
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                if (appRefId != 0)
                {
                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.BUILDING_PLAN_OSH) != "")
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
    }
}
