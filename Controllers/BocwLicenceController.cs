using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using System.Threading.Tasks;
using System;
using pbsamadhannetcoreapi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using pbsamadhannetcoreapi.CommonUtiliteis;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BocwLicenceController : Controller
    {
        private IBocwLicenceService _iBocwLicenceService;
        private readonly UserManager<User> _userManager;
        private IAuthService _iAuthService;
        public BocwLicenceController(IBocwLicenceService iBocwLicenceService, UserManager<User> userManager, IAuthService iAuthService)
        {
            _iBocwLicenceService = iBocwLicenceService;
            _userManager = userManager;
            _iAuthService = iAuthService;
        }

        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)

        {
            GenericFormModel<Licence_BocwAct_GeneralDetail> genericFormModel = await _iBocwLicenceService.GetBocwLicenceGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Add Update General Details
        [Route("addUpdate_OccupierAndManagerDetail")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_OccupierAndManagerDetail([FromBody] Licence_BocwAct_GeneralDetail requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBocwLicenceService.AddUpdate_OccupierAndManagerDetail(requestData, userClaims.UserId);
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

        #region Bocw Contractor
        [HttpGet, Route("getbocwcontractorcommendetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_BocwContractorCommanDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<BocwContractorDetailViewModel> genericFormModel = await _iBocwLicenceService.GetBocwContractorCommanDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getbocwLicencecontractorList")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_BocwLicenceContractorList([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericFormModel<List<Licence_BocwAct_ContractorDetail>> genericFormModel = await _iBocwLicenceService.GetBocwLicenceContractorList(dataTableParams.AppRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #endregion


        #region Add Contcator General Details
        [Route("addupdate_bocwcontractordetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdateBocwContractorDetail([FromBody] BocwContractorDetailViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBocwLicenceService.AddUpdateBocwContractorDetail(requestData);
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


        #region BOCW Licence Details

        [HttpGet, Route("getbocwdetail")]
        public async Task<IActionResult> Get_BocwDetail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<BocwLicenceViewModel> genericFormModel = await _iBocwLicenceService.GetBocwLicenceDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion  BOCW Licence Details


        #region Remove Contractor

        [HttpGet, Route("removeContractor")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> RemoveContractor([FromQuery] Int64 id, int totalWorkers)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iBocwLicenceService.RemoveContractor(id, totalWorkers);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion Remove Contractor



        #region Lock Application
        [Route("lockapplication")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBocwLicenceService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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