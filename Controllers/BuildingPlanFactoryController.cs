using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
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
    public class BuildingPlanFactoryController : ControllerBase
    {
        private IBuildingPlanFactoryService _iBuildingPlanFactoryService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;
        public BuildingPlanFactoryController(IBuildingPlanFactoryService iBuildingPlanFactoryService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService iAuthService)
        {
            _iBuildingPlanFactoryService = iBuildingPlanFactoryService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = iAuthService;
        }
        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<BuildingPlanFactory_GeneralDetail> genericFormModel = await _iBuildingPlanFactoryService.GetBuildingPlanFactoryGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_generaldetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] BuildingPlanFactory_GeneralDetail requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBuildingPlanFactoryService.AddUpdate_GeneralDetail(requestData, userClaims.UserId);
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

        #region Details

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<BuildingPlanFactoryViewModel> genericFormModel = await _iBuildingPlanFactoryService.GetBuildingPlanFactoryDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion Details

        #region Lock Application

        [Route("lockapplication")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBuildingPlanFactoryService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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

        [HttpGet, Route("getBuildingPlanFactoryRaiseFee")]
        public async Task<IActionResult> Get_BuildingPlanFactoryRaiseFee([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = await _iBuildingPlanFactoryService.GetBuildingPlanFactoryRaiseFee(appRefId, applicationType, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("verifyBuildingPlanFactoryRaiseFee")]
        public async Task<IActionResult> VerifyBuildingPlanFactoryRaiseFee([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType, int paymentBatchCounter, bool isForVerification)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = await _iBuildingPlanFactoryService.VerifyBuildingPlanFactoryRaiseFee(appRefId, applicationType, paymentBatchCounter);
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
            GenericResponseTemplateModel<ApplicationActionViewModel> genericServiceResultTemplate = await _iBuildingPlanFactoryService.AddUpdate_BuildingPlanFactoryRaiseFee(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }


        [HttpGet, Route("getRaisedFeeList")]
        public async Task<IActionResult> Get_RaisedFeeList([FromQuery] Int64 appRefId, Int64 paymentBatchCounter)
        {
            GenericFormModel<List<BuildingPlanHUDPaymentDetail>> genericFormModel = await _iBuildingPlanFactoryService.Get_BuildingPlanFactoryRaisedFeeList(appRefId, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("update_RaisedFeeDetail")]
        [HttpPost]
        public async Task<IActionResult> Update_RaisedFeeDetail([FromBody] BuildingPlanFactoryPaymentDetailViewModal requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iBuildingPlanFactoryService.Update_BuildingPlanFactoryRaisedFeeDetail(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion

        #region Declaration Stability Certificate

        [HttpGet, Route("getdeclarationstabilitycertificate")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_Declaration_Stability_Certificate([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<BuildingPlanFactory_Declaration_Stability_Certificate> genericFormModel = await _iBuildingPlanFactoryService.GetDeclarationStabilityCertificate(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_declarationstabilitycertificate")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_Declaration_Stability_Certificate([FromBody] BuildingPlanFactory_Declaration_Stability_Certificate requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBuildingPlanFactoryService.AddUpdate_DeclarationStabilityCertificate(requestData, userClaims.UserId);
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

        [HttpGet, Route("getdeclarationstabilitycertificatedetail")]
        public async Task<IActionResult> Get_Declaration_Stability_CertificateDetail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<BuildingPlanFactory_Declaration_Stability_CertificateViewModel> genericFormModel = await _iBuildingPlanFactoryService.GetDeclarationStabilityCertificateDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #endregion
    }
}
