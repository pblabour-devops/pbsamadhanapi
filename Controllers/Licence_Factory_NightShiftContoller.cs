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
    public class Licence_Factory_NightShiftController : ControllerBase
    {
        private ILicence_Factory_NightShiftService _iLicence_Factory_NightShiftService;
        private readonly UserManager<User> _userManager;
        public Licence_Factory_NightShiftController(ILicence_Factory_NightShiftService iLicence_Factory_NightShiftService, UserManager<User> userManager)
        {
            _iLicence_Factory_NightShiftService = iLicence_Factory_NightShiftService;
            _userManager = userManager;
        }

        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
            {
            GenericFormModel<Licence_Factory_NightShift_Approval> genericFormModel = await _iLicence_Factory_NightShiftService.GetGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        //[Route("addupdate_generaldetails")]
        //[HttpPost]
        //public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] Licence_Factory_NightShift_Approval requestData)
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_Factory_NightShiftService.AddUpdate_GeneralDetail(requestData, user.Id);
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
        #endregion

        #region Licence Factory Night Shift Detail

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<Licence_Factory_NightShiftViewModel> genericFormModel = await _iLicence_Factory_NightShiftService.GetLicenceFactoryNightShiftDetail(id);
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
            GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_Factory_NightShiftService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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


        [HttpGet, Route("getlastclearances")]
        public async Task<IActionResult> GetLastClearances([FromQuery] string licenceNo , Int64 projectSiteRefId, int projectSiteVersion)
        {
            GenericResponseTemplateModel<List<GetlastClearanceViewModel>> genericFormModel = await _iLicence_Factory_NightShiftService.GetLastClearances(licenceNo, projectSiteRefId, projectSiteVersion);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #endregion 
    }
}