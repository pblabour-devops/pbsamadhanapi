using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
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
    public class Licence_ContractLabourController : ControllerBase
    {
        private ILicence_ContractLabourService _iLicence_ContractLabourService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;
        public Licence_ContractLabourController(ILicence_ContractLabourService iLicence_ContractLabourService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService authService)
        {
            _iLicence_ContractLabourService = iLicence_ContractLabourService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = authService;
        }

        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)

        {
            GenericFormModel<Licence_ContractLabour_GeneralDetail> genericFormModel = await _iLicence_ContractLabourService.GetContractLabourGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Get Principal Employer Details
        [HttpGet, Route("getPrincipalEmployerDetails")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> GetPrincipalEmployerDetails([FromQuery] string licencenumber)
        {
            GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel> genericFormModel = await _iLicence_ContractLabourService.GetPrincipalEmployerDetails(licencenumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Get PE Contractor List
        [HttpGet, Route("getpeContractorlist")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_PE_Contractor_List([FromQuery] Int64 generalDetailRefId)
        {
            GenericFormModel<List<Licence_CL_PE_Contrator>> genericFormModel = await _iLicence_ContractLabourService.Get_PE_Contractor_List(generalDetailRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getpeContractorlistbyid")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_PE_Contractor_List_ById([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<Licence_CL_PE_Contrator> genericFormModel = await _iLicence_ContractLabourService.Get_PE_Contractor_List_byId(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getLicenceValidity")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> GetLicenceValidity([FromQuery] string licenceNumber)
        {
            GenericResponseTemplateModel<List<ContractLabourLicenceValidityViewModel>> genericFormModel = await _iLicence_ContractLabourService.GetLicenceValidity(licenceNumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Add Update General Details
        [Route("addUpdate_GeneralDetail")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] Licence_ContractLabour_GeneralDetail requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_ContractLabourService.AddUpdate_GeneralDetail(requestData, userClaims.UserId);
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


        #region Get Licence Contractor Labour Detail
        [HttpGet, Route("getLicenceContractLabourdetail")]
        public async Task<IActionResult> Get_Licence_Contract_Labour_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<Licence_Contract_LabourViewModel> genericFormModel = await _iLicence_ContractLabourService.Get_Licence_ContractLabour_Detail(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Lock Application
        [Route("lockapplication")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_ContractLabourService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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

    }
}
