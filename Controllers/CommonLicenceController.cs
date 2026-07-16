using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CommonLicenceController : ControllerBase
    {
        private ICommonLicenceService _icommonLicenceService;
        private IAuthService _iAuthService;
        public CommonLicenceController(ICommonLicenceService commonLicenceService, IAuthService iAuthService)
        {
            _icommonLicenceService = commonLicenceService;
            _iAuthService = iAuthService;
        }

        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteRefId)
        {
            GenericFormModel<CommonLicence_GeneralDetail> genericFormModel = await _icommonLicenceService.GetCommonLicenceGeneralDetail(id, projectSiteRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_generaldetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] CommonLicence_GeneralDetail requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _icommonLicenceService.AddUpdate_GeneralDetail(requestData, userClaims.UserName);
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

        #region Contractor Details
        [HttpGet, Route("getcontractordetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_ContractorDetail([FromQuery] Int64 id)
        {
            GenericFormModel<CommonLicence_ContractorDetail> genericFormModel = await _icommonLicenceService.GetCommonLicenceContractorDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_contractordetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_ContractorDetail([FromBody] CommonLicence_ContractorDetail requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _icommonLicenceService.AddUpdate_ContractorDetail(requestData);
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

        #region Common Licence Details

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<CommonLicenceViewModels> genericFormModel = await _icommonLicenceService.GetCommonLicenceDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion Common Licence Details 

        #region Lock Application

        [Route("lockapplication")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _icommonLicenceService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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
    }
}
