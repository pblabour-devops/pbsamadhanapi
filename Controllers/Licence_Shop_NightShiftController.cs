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
    public class Licence_Shop_NightShiftController : ControllerBase
    {
        private ILicence_Shop_NightShiftService _iLicence_Shop_NightShift;
        private readonly UserManager<User> _userManager;
        private IAuthService _iAuthService;
        public Licence_Shop_NightShiftController(ILicence_Shop_NightShiftService iLicence_Shop_NightShift, UserManager<User> userManager, IAuthService iAuthService) 
        {
            _iLicence_Shop_NightShift = iLicence_Shop_NightShift;
            _userManager = userManager;
            _iAuthService = iAuthService;
        }

        //[Route("getCheckListPoints")]
        //[HttpGet]
        //public async Task<IActionResult> GetCheckListPoints([FromQuery] Int64 id)
        //{
        //    GenericResponseTemplateModel<List<Licence_Shop_NightShift_ChecklistPoint>> genericServiceResultTemplate = await _iLicence_Shop_NightShift.GetCheckListPoints(id);
        //    if (genericServiceResultTemplate.HasError)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        //}

        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_Shop_NightShift_Approval> genericFormModel = await _iLicence_Shop_NightShift.GetGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_generaldetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] Licence_Shop_NightShift_Approval requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_Shop_NightShift.AddUpdate_GeneralDetail(requestData, userClaims.UserId);
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

        #region Licence Shop Night Shift Detail

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<Licence_Shop_NightShiftViewModel> genericFormModel = await _iLicence_Shop_NightShift.GetLicenceShopNightShiftDetail(id);
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
            GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_Shop_NightShift.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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


        [HttpGet, Route("getlastclearancesnts")]
        public async Task<IActionResult> GetLastClearancesNTS([FromQuery] string licenceNo, Int64 projectSiteRefId, int projectSiteVersion)
        {
            GenericResponseTemplateModel<List<GetlastClearanceViewModel>> genericFormModel = await _iLicence_Shop_NightShift.GetLastClearancesNTS(licenceNo, projectSiteRefId, projectSiteVersion);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

    }
}
