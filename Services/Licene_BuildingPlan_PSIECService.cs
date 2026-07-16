using Microsoft.Extensions.Configuration;
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
using pbsamadhannetcoreapi.Repositories;

namespace pbsamadhannetcoreapi.Services
{
    public class Licene_BuildingPlan_PSIECService : ILicene_BuildingPlan_PSIECService
    {
        private readonly IGenericRepository<Licence_BuildingPlan_PSIEC_GeneralDetail> _iGR_Licence_BuildingPlan_PSIEC_GeneralDetail;
        private readonly IApplicationManagementService<Licence_BuildingPlan_PSIEC_GeneralDetail> _iApplicationMamnagementService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly AppDbContext _context;
        private IAuthService _iAuthService;
        private IConfiguration _iConfiguration { get; }
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        public Licene_BuildingPlan_PSIECService(IGenericRepository<Licence_BuildingPlan_PSIEC_GeneralDetail> iGR_Licence_BuildingPlan_PSIEC_GeneralDetail,
             IApplicationManagementService<Licence_BuildingPlan_PSIEC_GeneralDetail> IApplicationMamnagementService,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             IGenericRepository<BuildingPlanHUD_RTB_Mapping> iGR_BuildingPlanHUD_RTB_Mapping,
             AppDbContext context,
             IAuthService authService,
             IConfiguration iConfiguration,
             IGeneric_SP_Repository iGeneric_SP_Repository)
        {
            _iGR_Licence_BuildingPlan_PSIEC_GeneralDetail = iGR_Licence_BuildingPlan_PSIEC_GeneralDetail;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _context = context;
            _iAuthService = authService;
            _iConfiguration = iConfiguration;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
        }

        #region General Details
        public async Task<GenericFormModel<Licence_BuildingPlan_PSIEC_GeneralDetail>> GetBuildingPlan_PSIEC_GeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<Licence_BuildingPlan_PSIEC_GeneralDetail> genericFormModel = new GenericFormModel<Licence_BuildingPlan_PSIEC_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_BuildingPlan_PSIEC_GeneralDetail
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
                    genericFormModel.FormModel = new Licence_BuildingPlan_PSIEC_GeneralDetail();

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
                    SelectListTypeCode = "BuidingPlanHUDApprovalTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuidingPlanHUDApprovalTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuildingTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuildingTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "PSIECPhaseTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<PSIECPhaseTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "InspectionTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<InspectionTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ProjectTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ProjectTypeEnum>()
                });

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

                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                var competentPersonList = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");
                genericFormModel.FormModel.CompetentPersonList = competentPersonList.ListData;

                var empaneledArchitectsList = await _iAuthService.GetOfficerDetailsByRoleName("ARCH");
                genericFormModel.FormModel.EmpaneledArchitectsList = empaneledArchitectsList.ListData;

                var empaneledEngineersList = await _iAuthService.GetOfficerDetailsByRoleName("ENGR");
                genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersList.ListData;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_PSIEC, (genericFormModel.FormModel != null ? genericFormModel.FormModel.PsiecBuildingPlanId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        #endregion

        #region Application Form Details
        public async Task<GenericFormModel<Licence_BuildingPlan_PSIECViewModel>> GetBuildingPlan_PSIEC_Detail(long id)
        {
            GenericFormModel<Licence_BuildingPlan_PSIECViewModel> genericFormModel = new GenericFormModel<Licence_BuildingPlan_PSIECViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Licence_BuildingPlan_PSIECViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_BuildingPlan_PSIEC_GeneralDetail();

                var parentWithChildObject = await _iGR_Licence_BuildingPlan_PSIEC_GeneralDetail
                        .GetAsync(x => x.AppRefId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Application)      //Includes
                        .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuidingPlanHUDApprovalTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuidingPlanHUDApprovalTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "IndustryColorCodeByPPCBTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<IndustryColorCodeByPPCBTypeEnum>()
                });

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
               
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_PSIEC, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.PsiecBuildingPlanId : id), "LOCK");
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
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = new CustomeValidationResult();
                genericServiceResultTemplate.CustomeValidationResult.IsValid = true;
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                if (appRefId != 0)
                {

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.BUILDING_PLAN_HUD)!="")
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

        public async Task<GenericResponseTemplateModel<PSIECUserDetailsViewModel>> GetSdoAndEoDetails(string sdo, string eo)
        {
            var genericServiceResultTemplate = new GenericResponseTemplateModel<PSIECUserDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>
                {
                    new StoreProcedureParm() { ParmName = "RoleName", ParmValue = sdo + "," + eo, isNumber = false }
                };

                List<PSIECUserViewModel> allUsers = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PSIECUserViewModel>("dbo.sp_Get_PSIEC_OfficialDetailsByRoleName", storeProcedureParms);

                var result = new PSIECUserDetailsViewModel
                {
                    SdoDetails = allUsers.Where(u => u.Designation == "Sub-Divisional Officer").ToList(),
                    EODetails = allUsers.Where(u => u.Designation == "Executive Officer").ToList()
                };

                genericServiceResultTemplate.ResponseDataModel = result;
                genericServiceResultTemplate.HasError = false;
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
