using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class LWBCommunicationController : ControllerBase
        {
            private ILWBCommunicationService _iLWBCommunicationService;
            private readonly UserManager<User> _userManager;
                private IAuthService _iAuthService;
        public LWBCommunicationController(ILWBCommunicationService iLWBCommunicationService, UserManager<User> userManager,IAuthService iAuthService)
            {
                _iLWBCommunicationService = iLWBCommunicationService;
                _userManager = userManager;
                _iAuthService = iAuthService;
            }

            [HttpGet, HttpGet("getAuthToken")]
            public async Task<IActionResult> GetAuthToken([FromQuery] string rawToken)
            {
                GenericResponseTemplateModel<string> genericResponseTemplate = await _iLWBCommunicationService.GetAuthToken(rawToken);
                if (genericResponseTemplate.HasError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
                }
                return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
            }

            [HttpGet, Route("getApplicationDetails")]
            public async Task<IActionResult> GetApplicationDetails([FromQuery] string licenceNumber, string authToken)
            {
                GenericResponseTemplateModel<List<LWBApplicationDetailsViewModel>> genericResponseTemplate = await _iLWBCommunicationService.GetApplicationDetails(licenceNumber, authToken);
                if (genericResponseTemplate.HasError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate);
                }
                return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
            }

            [HttpPost("LogError")]
            public async Task<IActionResult> LogError([FromBody] ErrorLogParamsViewModel requestData)
            {
                GenericServiceResultTemplate genericServiceResultTemplate = await _iLWBCommunicationService.ErrorLogsParams(requestData);
                if (genericServiceResultTemplate.HasException)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
                }
                return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
            }
            [HttpGet, Route("getredirectTokenWelfareBoard")]
            public async Task<IActionResult> GetRedirectTokenWelfareBoard(string licenceNumber, int pwbCessCollectionId, int type)
            {
                var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
                GenericResponseTemplateModel<string> genericResponseTemplate = await _iLWBCommunicationService.GetRedirectTokenWelfareBoard(userClaims.UserId, licenceNumber, pwbCessCollectionId, type);
                if (genericResponseTemplate.HasError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate.ErrorDesc);
                }
                return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
            }

        [HttpPost("insertLWBMaster")]
        public async Task<IActionResult> InsertLWBMaster()
        {
            var files = Request.Form.Files;

            var requestData =JsonConvert.DeserializeObject<LWBFormRequestViewModel>(Request.Form["requestData"]);

            var result =await _iLWBCommunicationService.InsertLWBMaster(requestData,files);

            if (result.HasException)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    result.Exceptions.Message);
            }

            return Ok(result);
        }

        [HttpGet, Route("getLWBDetails")]
        public async Task<IActionResult> GetLWBDetails([FromQuery]  Int64 id)
        {
            GenericFormModel<LWB_FundMaster> genericResponseTemplate = await _iLWBCommunicationService.GetLWBDetails(id);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        [HttpGet, Route("getLWBEmployeeDetails")]
        public async Task<IActionResult> GetLWBEmployeeDetails([FromQuery] Int64 id)
        {
            GenericFormModel<List<LWB_Employees_Fund>> genericResponseTemplate = await _iLWBCommunicationService.GetLWBEmployeeDetails(id);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        [HttpGet, Route("getUnpaidWagesErrorEmployeeDetails")]
        public async Task<IActionResult> GetUnpaidWagesErrorEmployeeDetails([FromQuery] Int64 id)
        {
            GenericFormModel<List<LWB_Employees_UnpaidWages>> genericResponseTemplate = await _iLWBCommunicationService.GetUnpaidWagesErrorEmployeeDetails(id);

            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        [HttpGet, Route("getLWBUnpaidWagesPaymentEmployeeDetails")]
        public async Task<IActionResult> GetLWBUnpaidWagesPaymentEmployeeDetails([FromQuery] Int64 fundMasterRefId)
        {
            GenericFormModel<List<LWB_LIdAndUnpaidWages>> genericResponseTemplate = await _iLWBCommunicationService.GetLWBUnpaidWagesPaymentEmployeeDetails(fundMasterRefId);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }

        [HttpGet, Route("verifySlotAndLicenceNumber")]
        public async Task<IActionResult> VerifySlotAndLicenceNumber([FromQuery] string financialYear , string timeSlot , string licenceNumber)
        {
            GenericFormModel<int> genericResponseTemplate = await _iLWBCommunicationService.VerifySlotAndLicenceNumber(financialYear, timeSlot, licenceNumber);
            if (genericResponseTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplate);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplate);
        }
    }   
}
