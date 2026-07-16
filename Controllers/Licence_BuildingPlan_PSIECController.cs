using Microsoft.AspNetCore.Authorization;
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
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Licence_BuildingPlan_PSIECController : ControllerBase
    {
        private ILicene_BuildingPlan_PSIECService _iLicene_BuildingPlan_PSIECService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        public Licence_BuildingPlan_PSIECController(ILicene_BuildingPlan_PSIECService iLicene_BuildingPlan_PSIECService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService)
        {
            _iLicene_BuildingPlan_PSIECService = iLicene_BuildingPlan_PSIECService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
        }

        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_BuildingPlan_PSIEC_GeneralDetail> genericFormModel = await _iLicene_BuildingPlan_PSIECService.GetBuildingPlan_PSIEC_GeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Application Form Details

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<Licence_BuildingPlan_PSIECViewModel> genericFormModel = await _iLicene_BuildingPlan_PSIECService.GetBuildingPlan_PSIEC_Detail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion Application Form Details

        #region Lock Application

        [Route("lockapplication")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iLicene_BuildingPlan_PSIECService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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

        #region SDO & EO List
        [HttpGet, Route("getSdoAndEoDetails")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GetSdoAndEoDetails([FromQuery] string sdo, string eo)
        {
            GenericResponseTemplateModel<PSIECUserDetailsViewModel> genericServiceResultTemplate = await _iLicene_BuildingPlan_PSIECService.GetSdoAndEoDetails(sdo, eo);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion
    }
}