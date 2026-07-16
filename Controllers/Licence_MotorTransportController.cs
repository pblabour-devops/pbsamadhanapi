using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using System.Threading.Tasks;
using System;
using pbsamadhannetcoreapi.ViewModels;
using System.Linq;

namespace pbsamadhannetcoreapi.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        //[Authorize]
        public class Licence_MotorTransportController : ControllerBase
        {
            private ILicence_MotorTransportService _iLicence_MotorTransportService;
            private readonly UserManager<User> _userManager;
            private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
            private IAuthService _iAuthService;
            public Licence_MotorTransportController(ILicence_MotorTransportService iLicence_Motor_TransportService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService authService)
            {
                _iLicence_MotorTransportService = iLicence_Motor_TransportService;
                _userManager = userManager;
                _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
                _iAuthService = authService;
            }
            [HttpGet, Route("getgeneraldetail")]
            [CustomFillters.AuthorizeAttribute("INDL")]
            public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId, Int64 identity)
            {
                GenericFormModel<Licence_MotorTransport> genericFormModel = await _iLicence_MotorTransportService.GetMotorTransportLicenceGeneralDetail(id, projectSiteId, identity);
                if (genericFormModel.HasError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
                }
                return StatusCode(StatusCodes.Status200OK, genericFormModel);
            }

            [Route("addupdate_generaldetails")]
            [HttpPost]
            [CustomFillters.AuthorizeAttribute("INDL")]
            public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] Licence_MotorTransport requestData)
            {
                var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
                GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_MotorTransportService.AddUpdate_GeneralDetail(requestData, userClaims.UserId);
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

            [HttpGet, Route("getMotorTransportDetail")]
            public async Task<IActionResult> Get_MotorTransportDetail([FromQuery] Int64 id, Int64 psid)
            {
                GenericFormModel<Licence_MotorTransportViewModel> genericFormModel = await _iLicence_MotorTransportService.GetMotorTransportDetail(id);
                if (genericFormModel.HasError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
                }
                return StatusCode(StatusCodes.Status200OK, genericFormModel);
            }

            [Route("lockapplication")]
            [HttpPost]
            [CustomFillters.AuthorizeAttribute("INDL")]
            public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
            {
                GenericServiceResultTemplate genericServiceResultTemplate = await _iLicence_MotorTransportService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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
        }
}
