using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingPlanHUDController : ControllerBase
    {
        private IBuildingPlanHUDService _iBuildingPlanHUDService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;
        public BuildingPlanHUDController(IBuildingPlanHUDService iBuildingPlanHUDService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService iAuthService)
        {
            _iBuildingPlanHUDService = iBuildingPlanHUDService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = iAuthService;
        }
        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<BuildingPlanHUD_GeneralDetail> genericFormModel = await _iBuildingPlanHUDService.GetBuildingPlanHUDGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_generaldetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] BuildingPlanHUD_GeneralDetail requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBuildingPlanHUDService.AddUpdate_GeneralDetail(requestData, userClaims.UserId);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion

        #region BuildingPlanHUD Details

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<BuildingPlanHUDViewModel> genericFormModel = await _iBuildingPlanHUDService.GetBuildingPlanHUDDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion BuildingPlanHUD Details

        #region Lock Application

        [Route("lockapplication")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBuildingPlanHUDService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion Lock Application

        #region Building Plan HUD Raise Fee
       
        [HttpGet, Route("getBuildingPlanHUDRaiseFee")]
        public async Task<IActionResult> Get_BuildingPlanHUDRaiseFee([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = await _iBuildingPlanHUDService.GetBuildingPlanHUDRaiseFee(appRefId, applicationType, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("verifyBuildingPlanHUDRaiseFee")]
        public async Task<IActionResult> VerifyBuildingPlanHUDRaiseFee([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType, int paymentBatchCounter, bool isForVerification)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = await _iBuildingPlanHUDService.VerifyBuildingPlanHUDRaiseFee(appRefId, applicationType, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdate_BuildingPlanHUDRaiseFee")]
        [HttpPost]
        public async Task<IActionResult> AddUpdate_BuildingPlanHUDRaiseFee([FromBody] RaiseFeeParmsViewModel requestData)
        {
            GenericResponseTemplateModel<ApplicationActionViewModel> genericServiceResultTemplate = await _iBuildingPlanHUDService.AddUpdate_BuildingPlanHUDRaiseFee(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }


        [HttpGet, Route("getRaisedFeeList")]
        public async Task<IActionResult> Get_RaisedFeeList([FromQuery] Int64 appRefId, Int64 paymentBatchCounter)
        {
            GenericFormModel<List<BuildingPlanHUDPaymentDetail>> genericFormModel = await _iBuildingPlanHUDService.Get_RaisedFeeList(appRefId, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("update_RaisedFeeDetail")]
        [HttpPost]
        public async Task<IActionResult> Update_RaisedFeeDetail([FromBody] BuildingPlanHUDPaymentDetailViewModal requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iBuildingPlanHUDService.Update_RaisedFeeDetail(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion

        #region Principal Approval-RBA
        [Route("getPrincipalApprovalUnderRBA")]
        [HttpGet]
        public async Task<IActionResult> GetPrincipalApprovalUnderRBA(string iPin, string appId)
        {
            GenericResponseTemplateModel<PrincipalApproval_RBA_DetailsViewModel> genericFormModel = await _iThirdPartyInegrationsService.GetPrincipalApprovalUnderRBAByIPin(iPin, appId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [Route("updateActionLogIdInRaisedFee")]
        [HttpPost]
        public async Task<IActionResult> UpdateActionLogIdInRaisedFee([FromBody] UpdateActionLogInRaisedFeeParmsViewModel requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iBuildingPlanHUDService.UpdateActionLogIdInRaisedFee(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
    }
}
