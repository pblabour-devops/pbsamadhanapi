using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.Services;
using System.Collections.Generic;
using pbsamadhannetcoreapi.ViewModels;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class Licence_TradeUnionController : ControllerBase
    {
        private ILicence_TradeUnionService _iLicence_TradeUnionService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;
        public Licence_TradeUnionController(ILicence_TradeUnionService iLicence_Trade_UnionService,

        UserManager<User> userManager,IThirdPartyInegrationsService iThirdPartyInegrationsService,IAuthService authService)

        {
            _iLicence_TradeUnionService = iLicence_Trade_UnionService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = authService;
        }


        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 oldAppRefId, Int64 projectSiteId)
        {
            GenericFormModel<Licence_TradeUnion> genericFormModel = await _iLicence_TradeUnionService.GetTradeUnionLicenceGeneralDetail(id, oldAppRefId, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }



        [HttpGet, Route("getofficerdetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_OfficerDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_Trade_Union_ViewModels> genericFormModel = await _iLicence_TradeUnionService.GetOfficerDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        #region Get Officer List
        [HttpGet, Route("getTradeUnionOfficerList")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_TradeUnion_Officer_List([FromQuery] Int64 appRefId, [FromQuery] Int64 oldAppRefId, [FromQuery] Int64 applicationPurposeType)
        {
            GenericFormModel<List<Licence_TradeUnion_Officer>> genericFormModel = await _iLicence_TradeUnionService.Get_TradeUnion_Officer_List(appRefId, oldAppRefId, applicationPurposeType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Remove Officer
        [HttpGet, Route("removeOfficer")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> RemoveOfficer([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iLicence_TradeUnionService.RemoveOfficer(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region TradeUnion Details

        [HttpGet, Route("gettradeuniondetail")]
        public async Task<IActionResult> Get_Tade_Union_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<Licence_Trade_Union_ViewModels> genericFormModel = await _iLicence_TradeUnionService.Get_Tade_Union_Detail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion TradeUnion Details

        [HttpGet, Route("ValidateLicenceNumber")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> ValidateLicenceNumber([FromQuery] string licenceNo)
        {
            GenericResponseTemplateModel<List<TradeUnionLicenceValidateViewModel>> genericResponseTemplate = await _iLicence_TradeUnionService.ValidateLicenceNumber(licenceNo);

            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }
    }
}