    
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FactoryLicenceController : ControllerBase
    {
        private IFactoryLicenceService _iFactoryLicenceService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;
        public FactoryLicenceController(IFactoryLicenceService iFactoryLicenceService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService authService)
        {
            _iFactoryLicenceService = iFactoryLicenceService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = authService;
        }
        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_Factory_GeneralDetail> genericFormModel = await _iFactoryLicenceService.GetFactoryLicenceGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_generaldetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] Licence_Factory_GeneralDetail requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            //"f0b192d6-2c3d-4895-9fe7-aff1ad214cd0"
            GenericServiceResultTemplate genericServiceResultTemplate = await _iFactoryLicenceService.AddUpdate_GeneralDetail(requestData, userClaims.UserId);
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

        #region Occupier & Manager Details
        [HttpGet, Route("getOccupierAndManagerDetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_OccupierAndManagerDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Licence_Factory_OccupierAndManagerDetail> genericFormModel = await _iFactoryLicenceService.GetOccupierAndManagerDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdate_OccupierAndManagerDetail")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_OccupierAndManagerDetail([FromBody] Licence_Factory_OccupierAndManagerDetail requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iFactoryLicenceService.AddUpdate_OccupierAndManagerDetail(requestData);
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

        #region Factory Licence Details

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<FactoryLicenceViewModel> genericFormModel = await _iFactoryLicenceService.GetFactoryLicenceDetail(id);
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
            GenericServiceResultTemplate genericServiceResultTemplate = await _iFactoryLicenceService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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

        #region Temporary License Details
        [Route("getFactoryTemporaryLicenseDetails")]
        [HttpGet]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> GetFactoryTemporaryLicenseDetails(string tempRegistrationNumber)
        {
            GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel> genericFormModel = await _iThirdPartyInegrationsService.GetTemporaryLicenseDetails(tempRegistrationNumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region BuidingPlanDetails
        [Route("getBuidingPlanDetails")]
        [HttpGet]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> GetFactoryDOFLicenseDetails(string dofNumber)
        {
            GenericResponseTemplateModel<string> genericFormModel = await _iThirdPartyInegrationsService.GetDOFLicenseDetails(dofNumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Competent Person
        [Route("getCompetentPersonDetails")]
        [HttpGet]
        public async Task<IActionResult> GetCompetentPersonDetails()
        {
            GenericListModel<OfficerDetailsByRoleNameViewModel> genericFormModel = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [Route("addupdate_questionnairedetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_Questionnairedetails([FromBody] Licence_Factory_QuestionnaireDetail requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iFactoryLicenceService.AddUpdate_Questionnairedetails(requestData, userClaims.UserId);
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


        #region Renewal Check Condition

        [HttpGet, Route("verifyWelfareFundAndReturn")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> VerifyWelfareFundAndReturn([FromQuery] string licenceNumber, Int64 appRefId)
        {
            GenericFormModel<AnnualReturnWelfareFundViewModel> genericFormModel = await _iFactoryLicenceService.VerifyWelfareFundAndReturn(licenceNumber, appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [HttpGet, Route("generateApplicationFormPdf")]
        public async Task<IActionResult> GenerateApplicationFormPdf([FromQuery] Int64 appRefId)
        {
            GeneratePdfServiceResultTemplate genericServiceResultTemplate = await _iFactoryLicenceService.GenerateApplicationFormPdf(appRefId);

            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
    }
}
