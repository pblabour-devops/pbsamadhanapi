using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using pbsamadhannetcoreapi.CustomFillters;
using Microsoft.AspNetCore.Authorization;
namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private IDashboardService _iDashboardService;
        private readonly UserManager<User> _userManager;
        private IAuthService _iAuthService;
        public DashboardController(IDashboardService iDashboardService, UserManager<User> userManager, IAuthService authService)
        {
            _iDashboardService = iDashboardService;
            _userManager = userManager;
            _iAuthService = authService;
        }

        #region In-Process Application Data
        [HttpGet, Route("getinprocessapplications")]
        public async Task<IActionResult> GetAllProjectSiteInProcessApplications([FromQuery] Int64 ProjectSiteRefId)
        {
            GenericFormModel<List<DashboardViewModel>> genericFormModel = await _iDashboardService.GetAllProjectSiteInProcessApplications(ProjectSiteRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Application Logs Data
        [HttpGet, Route("getapplicationslogs")]
        //[CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetApplicationsLogs([FromQuery] Int64 appRefId)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericListModel<ApplicationLogsViewModel> genericFormModel = await _iDashboardService.GetApplicationsLogsData(appRefId, userClaims.UserId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Already Clearances Application Data
        [HttpGet, Route("getalreadyclearanceapplications")]
        public async Task<IActionResult> GetAllProjectSiteAlreadyClearancesApplications([FromQuery] Int64 ProjectSiteRefId)
        {
            GenericFormModel<ClearenceFileInfoViewModel> genericFormModel = await _iDashboardService.GetAllProjectSiteAlreadyClearancesApplications(ProjectSiteRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [HttpGet, Route("loadOfficialsDashboard")]
        //[CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> LoadOfficialsDashboard([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            GenericFormModel<OfficialDashboardContentViewModel> genericFormModel = await _iDashboardService.LoadRecordsTypesMenusCounts(dataTableParams.Id, (DashboardRecordsHeaderTypeEnum)dataTableParams.DashboardRecordsHeaderType, (ApplicationTypeEnum)dataTableParams.ApplicationType, dataTableParams.ApplicationListAlso, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        //[HttpGet, Route("loadAcwiseRegister")]
        //public async Task<IActionResult> LoadActWiseRegister([FromQuery] int ApplicationType)
        //{
        //    //GenericFormModel<OfficialDashboardContentViewModel> genericFormModel = await _iDashboardService.LoadRecordsTypesMenusCounts((DashboardRecordsHeaderTypeEnum)DashboardRecordsHeaderType, (ApplicationTypeEnum)ApplicationType, user.Id, user.UserName);
        //    //if (genericFormModel.HasError)
        //    //{
        //    //    return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
        //    //}
        //    List<Application> applications = new List<Application>()
        //    {
        //        new Application (){AppId=100, PublicAppRefNum="1236547856" },
        //        new Application (){AppId=101, PublicAppRefNum="9856596569" },
        //        new Application (){AppId=102, PublicAppRefNum="1231431355" },

        //        new Application (){AppId=103, PublicAppRefNum="5681382341" },
        //        new Application (){AppId=104, PublicAppRefNum="3535484354" }
        //    };

        //    return StatusCode(StatusCodes.Status200OK, applications);
        //}

        #region PSL Report Building Plan HUD
        [HttpGet, Route("getDashboard_PSL_MajorCount_BP_HUD")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDashboard_PSL_MajorCount_BP_HUD([FromQuery] PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PivotColDetailViewModel>> genericFormModel = await _iDashboardService.Dashboard_PSL_MajorCount_BP_HUD(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getDashboard_PSL_Apps_BP_HUD")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDashboard_PSL_Apps_BP_HUD([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericFormModel = await _iDashboardService.Dashboard_PSL_Apps_BP_HUD(dataTableParams.ColCode, dataTableParams.RoleId, dataTableParams.ApplicationLifeCycleStatusType, dataTableParams.AppActionType, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getDeemedCalculationDetail")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDeemedCalculationDetail([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<Deemed_ProcessFilesLog> genericFormModel = await _iDashboardService.GetDeemedCalculationDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getDeemedAllActCalculationDetail")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDeemedAllActCalculationDetail([FromQuery] Int64 id)
        {
            GenericListModel<Deemed_ProcessFilesLogViewModel> genericFormModel = await _iDashboardService.GetDeemedAllActCalculationDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [HttpGet, Route("getPaymentDetails")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetPaymentDetails([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<PaymentDetailsListViewModel>> genericFormModel = await _iDashboardService.GetPaymentDetails(Convert.ToInt32(EnumOps.GetEnumValue<ApplicationTypeEnum>(dataTableParams.ApplicationType.ToString())), dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #region PSL Report Factory

        [HttpGet, Route("getDashboard_PSL_MajorCount_Factory")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDashboard_PSL_MajorCount_Factory([FromQuery] PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PivotColDetailViewModel>> genericFormModel = await _iDashboardService.Dashboard_PSL_MajorCount_Factory(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getDashboard_PSL_Apps_Factory")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDashboard_PSL_Apps_Factory([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericFormModel = await _iDashboardService.Dashboard_PSL_Apps_Factory(dataTableParams.ColCode, dataTableParams.RoleId, dataTableParams.ApplicationLifeCycleStatusType, dataTableParams.AppActionType, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #endregion

        #region PSL Report Shop

        [HttpGet, Route("getDashboard_PSL_MajorCount_Shop")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDashboard_PSL_MajorCount_Shop([FromQuery] PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PivotColDetailViewModel>> genericFormModel = await _iDashboardService.Dashboard_PSL_MajorCount_Shop(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getDashboard_PSL_Apps_Shop")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDashboard_PSL_Apps_Shop([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericFormModel = await _iDashboardService.Dashboard_PSL_Apps_Shop(dataTableParams.ColCode, dataTableParams.RoleId, dataTableParams.ApplicationLifeCycleStatusType, dataTableParams.AppActionType, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #endregion

        #region Other Act Clearances
        [HttpGet, Route("getOtherActClearances")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetOtherActClearances([FromQuery] string userRefId, int applicationType)
        {
            GenericResponseTemplateModel<List<OtherActClearancesDataViewModel>> genericFormModel = await _iDashboardService.GetOtherActClearances(userRefId, applicationType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region PSL Dashboard Act-Wise
        [HttpGet, Route("get_PSLDashboard_ActWiseCount")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_PSLDashboard_ActWiseCount([FromQuery] PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PSLDashboardActWiseCountViewModel>> genericFormModel = await _iDashboardService.GetPSLDashboard_ActWiseCount(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region PSL Dashboard Circle-Wise
        [HttpGet, Route("get_PSLDashboard_CircleWiseCount")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_PSLDashboard_CircleWiseCount([FromQuery] string fromDate, string toDate, int applicationType)
        {
            GenericResponseTemplateModel<List<PSLDashboardCircleWiseCountViewModel>> genericFormModel = await _iDashboardService.GetPSLDashboard_CircleWiseCount(fromDate, toDate, applicationType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_PSLDashboard_OfficerWiseCount")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_PSLDashboard_OfficerWiseCount([FromQuery] OfficerWiseDataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericFormModel = await _iDashboardService.GetPSLDashboard_OfficerWiseCount(dataTableParams.ColCode, dataTableParams.CircleRefId, dataTableParams.ApplicationType, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Factory Backlog

        [HttpGet, Route("factoryBacklogCount")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> FactoryBacklogCount([FromQuery] string Id)
        {
            GenericFormModel<List<FactoryBacklogCountViewModel>> genericFormModel = await _iDashboardService.FactoryBacklogCount(Id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("factoryBacklog")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> FactoryBacklogData([FromQuery] string Id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericFormModel<List<FactoryBacklogExcelDataViewModel>> genericFormModel = await _iDashboardService.FactoryBacklogData(Id, searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_factoryBacklogdetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> AddUpdate_FactoryBacklogDetail([FromBody] FactoryBacklogDataRequestViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iDashboardService.AddUpdate_FactoryBacklogDetail(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("deregister_factoryByBacklog")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> DeRegister_FactoryByBacklog([FromBody] DeRegsiterFactoryRequestViewModel deRegsiterFactoryRequest)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iDashboardService.DeRegister_FactoryByBacklog(deRegsiterFactoryRequest);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("getApplicationData")]
        [HttpGet]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetApplicationData([FromQuery] Int64 projectSiteRefId, Int64 pbLabour_AppId, Int64 pbLabour_AppFormId, string pbLabour_NAR, Int32 IsLegacy, string userId)
        {
            GenericResponseTemplateModel<SysNApplicationDetailsViewModel> genericFormModel = await _iDashboardService.GetApplicationData(projectSiteRefId, pbLabour_AppId, pbLabour_AppFormId, pbLabour_NAR, IsLegacy, userId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("getBuildingPlanData")]
        [HttpGet]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetBuildingPlanData([FromQuery] string licenceNumber)
        {
            GenericResponseTemplateModel<List<GetBuildingPlanDetailsViewModel>> genericFormModel = await _iDashboardService.GetBuildingPlanData(licenceNumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("mapBuildingPlan")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> MapBuildingPlan([FromBody] MapBuildingPLanRequestViewModel mapBuildingPLanRequestViewModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iDashboardService.MapBuildingPlan(mapBuildingPLanRequestViewModel);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("getApplicationLogsByAppId")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetApplicationLogsByAppId(Int64 appRefId, Int64 appFormId, string nar, Int32 isLegacy)
        {
            GenericResponseTemplateModel<List<GetApplicationNotingLogsViewModel>> genericFormModel = await _iDashboardService.GetApplicationLogsByAppId(appRefId, appFormId, nar, isLegacy);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getFeeDetailsByAppId")]
        //[CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetFeeDetailsByAppId(Int64 appRefId, Int64 appFormId, string nar, Int32 isLegacy)
        {
            GenericResponseTemplateModel<List<ApplicationFeeDetailsViewModel>> genericFormModel = await _iDashboardService.GetFeeDetailsByAppId(appRefId, appFormId, nar, isLegacy);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("factoryBacklogCountwiseData")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> FactoryBacklogCountwiseData([FromQuery] string Id, int recordType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericFormModel<List<FactoryBacklogDataViewModel>> genericFormModel = await _iDashboardService.FactoryBacklogCountwiseData(Id, recordType, searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getLWFmonthwisecontribution")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetLWFMonthWiseData([FromQuery] Int64 pwbCessCollectionId)
        {
            GenericResponseTemplateModel<List<LWFMonthWiseContributionViewModel>> genericResponseTemplate = await _iDashboardService.GetLWFMonthWiseData(pwbCessCollectionId);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        [HttpGet, Route("getLWFemployeecontribution")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetLWFEmployeeData([FromQuery] string monthlyContributionId)
        {
            GenericResponseTemplateModel<List<LWFEmployeeContributionViewModel>> genericResponseTemplate = await _iDashboardService.GetLWFEmployeeData(monthlyContributionId);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        [HttpGet, Route("getAppActionDocuments")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetAppActionDocuments([FromQuery] string nar, Int64 appFormId, Int64 appId, Int32 isLegacy)
        {
            GenericResponseTemplateModel<List<GetAppActionDocumentsViewModel>> genericResponseTemplate = await _iDashboardService.GetAppActionDocuments(nar, appFormId, appId, isLegacy);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        #endregion

        #region Declaration Stability Certificate
        [HttpGet, Route("getDeclarationStabilityApplications")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDeclarationStabilityApplications([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericFormModel = await _iDashboardService.GetDeclarationStabilityApplications(dataTableParams.Id, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [HttpGet, Route("Load_Mpr_Factory_Data")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Load_Mpr_Factory_Data([FromQuery] string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year)
        {
            GenericResponseTemplateModel<List<MPR_Factory_DashboardViewModel>> genericResponseTemplate = await _iDashboardService.Load_Mpr_Factory_Data(id, searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray, month, year);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        #region Declaration Stability Certificate Excel
        [HttpGet, Route("getDeclarationStabilityApplicationsExcel")]
        public async Task<IActionResult> GetDeclarationStabilityApplicationsExcel([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<RecordsTypeListViewModel>> genericFormModel = await _iDashboardService.GetDeclarationStabilityApplicationsExcel(dataTableParams.Id, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region MIS_Dashboard
        [HttpGet, Route("get_ActAndApplicationPurposeTypeCounts")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_ActAndApplicationPurposeTypeCounts([FromQuery] PSLDashboardMajorCountRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<ActAndApplicationPurposeTypeCountsViewModel>> genericFormModel = await _iDashboardService.GetActAndApplicationPurposeTypeCounts(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_CircleAndApplicationPurposeTypeCounts")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_CircleAndApplicationPurposeTypeCounts([FromQuery] CircleAndApplicationPurposeTypeCountsParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<CircleAndApplicationPurposeTypeCountsViewModel>> genericFormModel = await _iDashboardService.GetCircleAndApplicationPurposeTypeCounts(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_DesignationAndApplicationPurposeTypeCounts")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_DesignationAndApplicationPurposeTypeCounts([FromQuery] CircleAndApplicationPurposeTypeCountsParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<DesignationAndApplicationPurposeTypeCountsViewModel>> genericFormModel = await _iDashboardService.GetDesignationAndApplicationPurposeTypeCounts(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_FileWiseData")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_FileWiseData([FromQuery] int applicationType, int statusType, int circleRefId, int applicationPurposeType, string fromDate, string toDate, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<FileWiseDataViewModel>> genericFormModel = await _iDashboardService.GetFileWiseData(applicationType, statusType, circleRefId, applicationPurposeType, fromDate, toDate, searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_FileWiseData_AppSearch")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDashboard_FileWiseData_AppSearch([FromQuery] MISDashboard_FileWiseData_AppSearchParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<FileWiseDataViewModel>> genericFormModel = await _iDashboardService.GetFileWiseData_AppSearch(dataTableParams.ApplicationType, dataTableParams.StatusType, dataTableParams.CircleRefId, dataTableParams.ApplicationPurposeType, dataTableParams.SearchCode, dataTableParams.PageNo, dataTableParams.PageSize, dataTableParams.SortColumn, dataTableParams.SortOrder, dataTableParams.FilterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_LabourWelfareSchemeCounts")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_LabourWelfareSchemeCounts([FromQuery] LabourWelfareSchemeApplicationParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<LabourWelfareSchemeApplicationCountsViewModel>> genericFormModel = await _iDashboardService.GetLabourWelfareSchemeCounts(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }



        [HttpGet, Route("getMis_Inspection")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetMis_Inspection([FromQuery] string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userId, string roleName)
        {
            GenericResponseTemplateModel<List<MisInspectionDashboardDataViewModel>> genericFormModel = await _iDashboardService.GetMis_Inspection(  searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray, userId,roleName);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpGet, Route("get_inspectionsActionWiseData")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_inspectionsActionWiseData([FromQuery] int inspectionStatus,int randomizationRefId, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userId, string roleName)
        {
            GenericResponseTemplateModel<List<InspectionApplicationinfoViewModel>> genericFormModel = await _iDashboardService.Get_inspectionsActionWiseData(inspectionStatus, randomizationRefId,searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray, userId, roleName);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #region Profile Wise Data

        [HttpGet, Route("get_AllDesignatedOfficialsRoleList")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_AllDesignatedOfficialsRoleList()
        {
            GenericFormModel<List<OfficialsRoleViewModel>> genericFormModel = await _iDashboardService.GetAllDesignatedOfficialsRoleList();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_OfficerMISDashboardDetailsByRoleName")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_OfficerMISDashboardDetailsByRoleName([FromQuery] string roleName)
        {
            GenericResponseTemplateModel<List<OfficerMISDashboardDetailsByRoleNameViewModel>> genericFormModel = await _iDashboardService.GetOfficerMISDashboardDetailsByRoleName(roleName);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_MIS_ProfileAndActWiseDashboardDetails")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_MIS_ProfileAndActWiseDashboardDetails([FromQuery] Int64 officerProfileRefId)
        {
            GenericResponseTemplateModel<List<OfficerProfileAndActWiseDashboardDetailsViewModel>> genericFormModel = await _iDashboardService.GetProfileAndActWiseDashboardDetails(officerProfileRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_MIS_ProfileAndCircleWiseDashboardDetails")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_MIS_ProfileAndCircleWiseDashboardDetails([FromQuery] Int64 officerProfileRefId, Int64 applicationType)
        {
            GenericResponseTemplateModel<List<OfficerProfileAndCircleWiseDashboardDetailsViewModel>> genericFormModel = await _iDashboardService.GetProfileAndCircleWiseDashboardDetails(officerProfileRefId, applicationType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_ProfileAndApplicationPurposeTypeCounts")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_ProfileAndApplicationPurposeTypeCounts([FromQuery] int officerProfileRefId)
        {
            GenericResponseTemplateModel<List<ProfileAndApplicationPurposeTypeCountsViewModel>> genericFormModel = await _iDashboardService.GetProfileAndApplicationPurposeTypeCounts(officerProfileRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_MIS_CurrentlyDesignatedOfficerDetails")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_MIS_CurrentlyDesignatedOfficerDetails([FromQuery] Int64 officerProfileRefId)
        {
            GenericResponseTemplateModel<List<CurrentlyDesignatedOfficerDetailsViewModel>> genericFormModel = await _iDashboardService.GetCurrentlyDesignatedOfficerDetails(officerProfileRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #endregion Profile Wise Data

        #endregion MIS_Dashboard

        #region Project Profile Dashboard

        [HttpGet, Route("checkservicealreadyapply")]
        public async Task<IActionResult> CheckServiceAlreadyApply([FromQuery] Int64 projectSiteId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            GenericResponseTemplateModel<bool> serviceGatewayResponse = await _iDashboardService.CheckServiceAlreadyApply(projectSiteId, applicationType, applicationPurposeType);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);

        }

        #endregion Project Profile Dashboard


        [HttpGet, Route("get_EsamikshaData")]
        public async Task<IActionResult> Get_EsamikshaData([FromQuery] EsamikshaViewModel requestData)
        {
            GenericResponseTemplateModel<List<EsamikshaCountsViewModel>> genericFormModel = await _iDashboardService.GetEsamikshaData(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_PendencyCountServiceWise")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_PendencyCountServiceWise()
         {
             var genericFormModel = await _iDashboardService.GetPendencyCountServiceWise();
             if (genericFormModel.HasError)
             {
                 return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
             }
             return StatusCode(StatusCodes.Status200OK, genericFormModel);
         }

        [HttpGet, Route("getTimeLinesDetail")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetTimeLinesDetail([FromQuery] Int64 applicationActionLogId)
        {
            GenericResponseTemplateModel<List<AppActionTimeLineDefination>> genericFormModel = await _iDashboardService.GetTimeLinesDetail(applicationActionLogId);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpGet, Route("get_PendingAppByServiceCode")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_PendingAppByServiceCode([FromQuery] GetPendingAppViewModel requestData)
         {
             GenericResponseTemplateModel<List<GetPendingAppCountsViewModel>> genericFormModel = await _iDashboardService.GetPendingAppData(requestData);
             if (genericFormModel.HasError)
             {
                 return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
             }
             return StatusCode(StatusCodes.Status200OK, genericFormModel);
         }
        
        [HttpGet, Route("searchApplication")]
        public async Task<IActionResult> SearchApplicationResult([FromQuery]  string searchCode)
        {
            GenericFormModel<List<SearchApplictionDataViewModel>> genericFormModel = await _iDashboardService.SearchApplicationResult( searchCode);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }



        [HttpGet, Route("getservicelist")]
        public async Task<IActionResult> GetAllServiceList([FromQuery] Int64 ProjectSiteId)
        {
            GenericFormModel<List<ServiceListViewModel>> genericFormModel = await _iDashboardService.GetAllServiceList(ProjectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }



        [HttpGet, Route("trackApplicationlogs")]
        public async Task<IActionResult> TrackApplicationLogs([FromQuery] GetApplciationLogsRequestParms requestParms)
        {
            GenericFormModel<List<GetApplciationLogsViewModel>> genericFormModel = await _iDashboardService.TrackApplicationLogs(requestParms.Appid, requestParms.InvestPunjabAppid, requestParms.PublicRefrenceNo);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpGet, Route("reactivateApplication")]
        [CustomFillters.AuthorizeAttribute("LB1N,DEVTEAM,HELPDESK")]
        public async Task<IActionResult> ReActivateApplication([FromQuery] Int64 appId)
        {
            GenericFormModel<List<GetApplciationReActivatedData>> genericFormModel = await _iDashboardService.ReActivateApplication(appId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getDeemedReport")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetDeemedReport([FromQuery] string deemedDate)
        {
            GenericFormModel<List<GetDeemedApplciationsData>> genericFormModel = await _iDashboardService.GetDeemedReport(deemedDate);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [Route("getEmpanelledPersonDetailsByRoleName")]
        [HttpGet]
        public async Task<IActionResult> GetEmpanelledPersonDetailsByRoleName([FromQuery] string roleName)
        {
            GenericListModel<EmpanelledPersonDetailsByRoleNameViewModel> genericListModel = await _iDashboardService.GetEmpanelledPersonDetailsByRoleName(roleName);
            if (genericListModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericListModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericListModel);
        }

        [Route("updateEmpanelledPersonStatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateEmpanelledPersonStatus([FromBody] EmpanelledPersonDetailsByRoleNameViewModel requestData)
        {
            GenericListModel<bool> genericListModel = await _iDashboardService.UpdateEmpanelledPersonStatus(requestData);
            if (genericListModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericListModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericListModel);
        }


        [HttpGet, Route("getPWBContributionDetails")]
        public async Task<IActionResult> GetPWBContributionDetails([FromQuery] string startDate, string endDate, int type , string userId)
        {
            GenericResponseTemplateModel<WelfareFundAndWagesDataViewModel> genericFormModel = await _iDashboardService.GetPWBContributionDetails(startDate, endDate , type , userId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getMonthlyContributionDetailsById")]
        public async Task<IActionResult> GetMonthlyContributionDetailsById([FromQuery] Int64 pwbCessCollectionId)
        {
            GenericResponseTemplateModel<List<MonthWiseContributionDetailsViewModel>> genericFormModel = await _iDashboardService.GetMonthlyContributionDetailsById(pwbCessCollectionId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getMonthlyEmployeeDetailsById")]
        public async Task<IActionResult> GetMonthlyEmployeeDetailsById([FromQuery] Int64 monthlyContributionId)
        {
            GenericResponseTemplateModel<List<MonthWiseEmployeeDetailsViewModel>> genericFormModel = await _iDashboardService.GetMonthlyEmployeeDetailsById(monthlyContributionId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getUnpaidWagesEmployeeDetailsById")]
        public async Task<IActionResult> GetUnpaidWagesEmployeeDetailsById([FromQuery] Int64 unpaidWagesId)
        {
            GenericResponseTemplateModel<List<UnpaidWagesEmployeeDetailsViewModel>> genericFormModel = await _iDashboardService.GetUnpaidWagesEmployeeDetailsById(unpaidWagesId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getRegisteredFactoriesList")]
        public async Task<IActionResult> GetRegsiteredFactoriesDetails(int factoryType)
        {
            GenericResponseTemplateModel<List<FactoriesListViewModel>> genericFormModel = await _iDashboardService.GetRegsiteredFactoriesDetails(factoryType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }



        [HttpGet, Route("getOtherClearanceDetails")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetOtherClearanceDetails(int projectSiteRefId , Int64 applicationType)
        {
            GenericResponseTemplateModel<GetOtherClearanceDetailsViewModel> genericServiceResultTemplate = await _iDashboardService.GetOtherClearanceDetails(projectSiteRefId, applicationType);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }


        [HttpGet, Route("Load_Mpr_Labour_Data")]
        public async Task<IActionResult> Load_Mpr_Labour_Data([FromQuery] string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year)
        {
            GenericResponseTemplateModel<List<MPR_Labour_DashboardViewModel>> genericResponseTemplate = await _iDashboardService.Load_Mpr_Labour_Data(id, searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray, month, year);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }
        [HttpGet, Route("Load_AnnualReturnData")]   
        public async Task<IActionResult> LoadAnnuaReturnData([FromQuery] string userId, [FromQuery] Int64 projectSiteRefId)
        {
            GenericResponseTemplateModel<List<AnnualReturnDashboardViewModel>> genericResponseTemplate = await _iDashboardService.LoadAnnuaReturnData(userId, projectSiteRefId);

            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        [HttpGet, Route("Load_Mpr_Alc_Data")]
        public async Task<IActionResult> Load_Mpr_Alc_Data([FromQuery] string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year)
        {
            GenericResponseTemplateModel<List<MPR_Alc_DashboardViewModel>> genericResponseTemplate = await _iDashboardService.Load_Mpr_Alc_Data(id, searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray, month, year);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }



            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        [Route("getShopBacklogData")]
        [HttpGet]
        public async Task<IActionResult> GetShopBacklogData([FromQuery] string Id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<ShopBacklogDataViewModel>> genericFormModel = await _iDashboardService.GetShopBacklogData(Id,  searchCode,  pageNo,  pageSize,  sortColumn,  sortOrder, filterArray);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("getLegacyShopFormData")]
        [HttpGet]
        public async Task<IActionResult> GetLegacyShopFormData([FromQuery] Int64 appId, Int64 appFormId, string nar)
        {
            GenericResponseTemplateModel<List<LegacyShopFormDataViewModel>> genericFormModel = await _iDashboardService.GetLegacyShopFormData(appId, appFormId, nar);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("getPendencyReport")]
        [HttpGet]
        public async Task<IActionResult> GetPendencyReport()
        {
            GenericResponseTemplateModel<List<PendencyReportViewModel>> genericFormModel = await _iDashboardService.GetPendencyReport();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("mergeLicenceWithExistingUser")]
        [HttpPost]
        public async Task<IActionResult> MergeLicenceWithExistingUser([FromBody] MergeLicenceWithUserRequestViewModel requestData)
        {
            GenericListModel<bool> genericListModel = await _iDashboardService.MergeLicenceWithExistingUser(requestData);
            if (genericListModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericListModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericListModel);
        }

        [HttpGet, Route("send_otp")]
        public async Task<IActionResult> SendOTPToUser(string userId, string userName, string licenceNumber)
        {
            GenericResponseTemplateModel<string> serviceGatewayResponse = await _iDashboardService.SendOTPToUser(userId, userName, licenceNumber);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);
        }

        [Route("getEstablishmentAndUserDetailsByLicenceNumber")]
        [HttpGet]
        public async Task<IActionResult> GetEstablishmentAndUserDetailsByLicenceNumber(string licenceNumber)
        {
            GenericResponseTemplateModel<EstablishmentAndUserDetailViewModel> genericFormModel = await _iDashboardService.GetEstablishmentAndUserDetailsByLicenceNumber(licenceNumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_ApprovedData")]
        public async Task<IActionResult> Get_ApprovedData([FromQuery] string id, int ServiceCode)
        {
            GenericResponseTemplateModel<List<ApprovedDataViewModel>> genericFormModel = await _iDashboardService.GetApprovedData(id, ServiceCode);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        
        
        [Route("getOfflineReportLogs")]
        [HttpGet]
        public async Task<IActionResult> GetOfflineReportLogs(string userRefId)
        {
            GenericResponseTemplateModel<List<OfflineReportLogs>> genericFormModel = await _iDashboardService.GetOfflineReportLogs(userRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addOfflineReport")]
        [HttpPost]
        public async Task<IActionResult> AddOfflineReport([FromBody] OfficerReportLogsRequestViewModel requestData)
        {
            GenericListModel<bool> genericListModel = await _iDashboardService.AddOfflineReport(requestData);
            if (genericListModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericListModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericListModel);
        }

        [HttpGet]
        [Route("downloadOfflineReport")]
        public async Task<IActionResult> DownloadOfflineReport(string reportId)
        {
            var result = await _iDashboardService.DownloadOfflineReport(reportId);

            if (result.HasError)
                return BadRequest(result.ErrorDesc);

            return Ok(result);
        }

        [HttpGet, Route("getTransperancyData")]
        [CustomFillters.AuthorizeAttribute("TRN_RPT")]
        public async Task<IActionResult> GetTransperancyData([FromQuery] TransperancyReportViewModel requestData)
        {
            GenericResponseTemplateModel<List<TransperancyReportCountsViewModel>> genericFormModel = await _iDashboardService.GetTransperancyData(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet]
        [Route("downloadRegisteredFactoryCircleWise")]
        public async Task<IActionResult> DownloadRegisteredFactoryCircleWise(string userRefId)
        {
            var result = await _iDashboardService.DownloadRegisteredFactoryCircleWise(userRefId);

            if (result.HasError)
                return BadRequest(result.ErrorDesc);

            return Ok(result);
        }

        [Route("getLWBPaymentReceipt")]
        [HttpGet]
        public async Task<IActionResult> GetLWBPaymentReceipt(int pwbCessCollectionId)
        {
            GenericFormModel<WelfareFundReceiptDetailsViewModel> genericFormModel = await _iDashboardService.GetLWBPaymentReceipt(pwbCessCollectionId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getLWFTransperancyData")]
        [CustomFillters.AuthorizeAttribute("TRN_RPT")]
        public async Task<IActionResult> GetLWFTransperancyData([FromQuery] TransperancyReportViewModel requestData)
        {
            GenericResponseTemplateModel<List<LWFTransperancyReportViewModel>> genericFormModel = await _iDashboardService.GetLWFTransperancyData(requestData);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
    }
}