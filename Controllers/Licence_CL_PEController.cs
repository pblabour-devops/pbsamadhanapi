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
using System.Threading.Tasks;
namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Licence_CL_PEController : ControllerBase
    {
        private ILicence_CL_PEService _iLicence_CL_PEService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;

        public Licence_CL_PEController(ILicence_CL_PEService iLicence_CL_PEService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService authService)
        {
            _iLicence_CL_PEService = iLicence_CL_PEService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = authService;
        }


        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)

        {
            GenericFormModel<Licence_CL_PE_GeneralDetail> genericFormModel = await _iLicence_CL_PEService.GetPEGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        //#region Add Update General Details
        //[Route("addUpdate_GeneralDetail")]
        //[HttpPost]
        //public async Task<IActionResult> AddUpdate_OccupierAndManagerDetail([FromBody] Licence_CL_PE_GeneralDetail requestData)
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_CL_PEService.AddUpdate_GeneralDetail(requestData, user.Id);
        //    if (genericServiceResultTemplate.HasException)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
        //    }
        //    else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        //}
        //#endregion

        #region Get Contractor General Detail
        [HttpGet, Route("getContractorgeneralDetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_CL_PE_Contractor_General_Detail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_CL_PE_ContractorDetailViewModel> genericFormModel = await _iLicence_CL_PEService.Get_CL_PE_Contractor_General_Detail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        //#region Add Update Contractor Detail
        //[Route("addupdate_Licencepecontractordetails")]
        //[HttpPost]
        //public async Task<IActionResult> AddUpdate_Licence_CL_PEContractorDetail([FromBody] Licence_CL_PE_ContractorDetailViewModel requestData)
        //{
        //    GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_CL_PEService.AddUpdate_Licence_CL_PEContractorDetail(requestData);
        //    if (genericServiceResultTemplate.HasException)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        //}
        //#endregion

        #region Get Contractor List
        [HttpGet, Route("getContractorlist")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_CL_PE_Contractor_List([FromQuery] Int64 appRefId)
        {
            GenericFormModel<List<Licence_CL_PE_Contrator>> genericFormModel = await _iLicence_CL_PEService.Get_CL_PE_Contractor_List(appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Remove Contractor
        [HttpGet, Route("removeContractor")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> RemoveContractor([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iLicence_CL_PEService.RemoveContractor(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Get Licence CL PE Detail
        [HttpGet, Route("getLicenceCLPEdetail")]
        public async Task<IActionResult> Get_Licence_CL_PE_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<Licence_CL_PEViewModel> genericFormModel = await _iLicence_CL_PEService.Get_Licence_CL_PE_Detail(id);

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
            GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_CL_PEService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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
