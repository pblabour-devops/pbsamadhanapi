using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProcessApplicationController : ControllerBase
    {
        private readonly IApplicationManagementService<ApplicationAction> _iApplicationMamnagementService;
        private readonly UserManager<User> _userManager;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;
        public ProcessApplicationController(UserManager<User> userManager,
            IApplicationManagementService<ApplicationAction> iapplicationManagementService,
            IThirdPartyInegrationsService iThirdPartyInegrationsService
            ,IAuthService iAuthService)
        {
            _userManager = userManager;
            _iApplicationMamnagementService = iapplicationManagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = iAuthService;
        }

        #region Process Application Details
        [HttpGet, Route("getprocessapplicationdetail")]
        public async Task<IActionResult> Get_ProcessApplicationDetail(string id, int currentActionCode, ApplicationTypeEnum applicationType)
        {
            GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>> genericFormModel = await _iApplicationMamnagementService.GetProcessApplicationDetail(id, currentActionCode, applicationType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getuserbyactioncode")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_UserByActionCode(string id, int actionCode, Int64 appRefId, ApplicationTypeEnum applicationType, string precheckCode)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);

            GenericResponseTemplateModel<ProcessApplicationUsersDetailViewModel> genericFormModel = await _iApplicationMamnagementService.GetUserByActionCode(actionCode, id, appRefId, applicationType, precheckCode);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addprocessapplicationdetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> AddUpdate_ProcessApplicationDetail([FromBody] ApplicationActionViewModel requestData)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var resp = await _iApplicationMamnagementService.RecordApplicationAction(requestData, requestData.UserId);
            if (resp.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resp.ErrorDesc);
            }
            else
            {
                // Share status to Invest Punjab Portal
                //await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_HUD, (AppActionTypeEnum)requestData.AppActionType);
            }

            return StatusCode(StatusCodes.Status200OK, resp);
        }

        [Route("upgradeApplicationCircle")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> UpgradeApplicationCircle([FromBody] LatestCircleInfoViewModel requestData)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var resp = await _iApplicationMamnagementService.UpgradeApplicationCircle(requestData);
            if (resp.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resp.ErrorDesc);
            }
            else
            {
                // Share status to Invest Punjab Portal
                //await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_HUD, (AppActionTypeEnum)requestData.AppActionType);
            }

            return StatusCode(StatusCodes.Status200OK, resp);
        }

        [Route("TransferApplication")]
        [HttpPost]
        public async Task<IActionResult> TransferApplication([FromBody] ApplicationTransferParmsViewModal requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iApplicationMamnagementService.TransferApplicationAction(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else
            {
                // Share status to Invest Punjab Portal
                //await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_HUD, (AppActionTypeEnum)requestData.AppActionType);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("getApplicationNotingLogs")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> Get_ApplicationNotingLogs(string id, Int64 appRefId)
        {
            GenericResponseTemplateModel<List<NotingLogsViewModel>> genericFormModel = await _iApplicationMamnagementService.GetApplicationNotingLogsByAppId(appRefId, id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("deleteTempCreatedLicense")]
        [HttpPost]
        public async Task<IActionResult> DeleteTempCreatedLicense([FromBody] ApplicationActionViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iApplicationMamnagementService.DeleteTempCreatedLicense(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        [Route("markDeemedApplications")]
        [HttpPost]
        public async Task<IActionResult> MarkDeemedApplications([FromBody] List<DeemedActionParmsViewModel> requestData)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //GenericServiceResultTemplate genericServiceResultTemplate = await _iApplicationMamnagementService.DeleteTempCreatedLicense(requestData);
            //if (genericServiceResultTemplate.HasException)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            //}

            return StatusCode(StatusCodes.Status200OK, requestData);
        }

        [HttpGet, Route("GetAppAdditionalDetails")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetApplicationAdditionalDetails([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<ApplicationAdditionalDetailsViewModel> genericFormModel = await _iApplicationMamnagementService.GetApplicationAdditionalDetails(appRefId, applicationType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getApplicationSpecificData")]
        public async Task<IActionResult> GetApplicationSpecificData([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<ApplicationSpecificDataViewModel> genericFormModel = await _iApplicationMamnagementService.GetApplicationSpecificData(appRefId, applicationType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getApplicationClearencesConditions")]
        public async Task<IActionResult> GetApplicationClearencesConditions([FromQuery] Int64 appRefId)
        {
            GenericResponseTemplateModel<List<ApplicationClearencesCondition>> genericFormModel = await _iApplicationMamnagementService.GetApplicationClearencesConditions(appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        [HttpGet, Route("getTimeLineWiseAllowedAction")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetTimeLineWiseAllowedAction(Int64 appActionLogRefId, string userRefId, Int64 appRefId)
        {
            GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>> genericFormModel = await _iApplicationMamnagementService.GetTimeLineWiseAllowedAction(appActionLogRefId, userRefId, appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getTimeLineOpenMutualAction")]
        public async Task<IActionResult> GetTimeLineOpenMutualAction(Int64 appRefId, string userRefId)
        {
            GenericFormModel<List<AppActionTime_MutualProcessFlagViewModel>> genericFormModel = await _iApplicationMamnagementService.GetTimeLineOpenMutualAction(appRefId, userRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [Route("recordOfflineRaiseFeeAction")]
        [HttpPost]
        public async Task<IActionResult> RecordOfflineRaiseFeeAction([FromBody] ApplicationActionViewModel requestData)
        {
            var resp = await _iApplicationMamnagementService.RecordOfflineRaiseFeeAction(requestData, requestData.UserId);
            if (resp.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resp.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, resp);
        }
    }
}
