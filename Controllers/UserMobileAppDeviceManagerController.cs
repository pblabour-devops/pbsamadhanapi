using Microsoft.AspNetCore.Http;
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
    public class UserMobileAppDeviceManagerController : ControllerBase
    {
        private IUserMobileAppDeviceManagerService _iUserMobileAppDeviceManagerService;
        public UserMobileAppDeviceManagerController(IUserMobileAppDeviceManagerService iUserMobileAppDeviceManagerService)
        {
            _iUserMobileAppDeviceManagerService = iUserMobileAppDeviceManagerService;
        }
        [HttpGet, Route("GenerateAndSendQRCodeByUserName")]
        public async Task<IActionResult> GenerateAndSendQRCodeByUserName([FromQuery] string userName)
        {
            GenericFormModel<string> genericFormModel = await _iUserMobileAppDeviceManagerService.GenerateAndSendQRCodeByUserName(userName);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("RegisterMobileAppDevice")]
        public async Task<IActionResult> RegisterMobileAppDevice([FromQuery] string mobileAppDeviceInfoText)
        {
            
            GenericFormModel<bool> genericFormModel = await _iUserMobileAppDeviceManagerService.RegisterMobileAppDevice(mobileAppDeviceInfoText);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("VerifyMobileAndSendOTP")]
        public async Task<IActionResult> VerifyMobileAndSendOTP([FromQuery] string encryptedMobileAndMobileAppId)
        {
            string genericFormModel = await _iUserMobileAppDeviceManagerService.VerifyMobileAndSendOTP(encryptedMobileAndMobileAppId);
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetRegisteredMobileNumberHintByUsername")]
        public async Task<IActionResult> GetRegisteredMobileNumberHintByUsername([FromQuery] string userName)
        {
            GenericResponseTemplateModel<RegisteredMobileNumberHintRespViewModel> GenericServiceResultTemplate = await _iUserMobileAppDeviceManagerService.GetRegisteredMobileNumberHintByUsername(userName);
            if (GenericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, GenericServiceResultTemplate);
        }

        [HttpGet, Route("GenerateDeviceRegistrationQRCode")]
        public async Task<IActionResult> GenerateDeviceRegistrationQRCode([FromQuery] string userRefId)
        {
            GenericResponseTemplateModel<string> GenericServiceResultTemplate = await _iUserMobileAppDeviceManagerService.GenerateDeviceRegistrationQRCode(userRefId);
            if (GenericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, GenericServiceResultTemplate);
        }

        //[HttpGet, Route("CheckUsernameHasSameDeviceId")]
        //public async Task<IActionResult> CheckUsernameHasSameDeviceId([FromQuery] string reqData)
        //{
        //    GenericResponseTemplateModel<UserDeviceFoundRespViewModel> GenericServiceResultTemplate = await _iUserMobileAppDeviceManagerService.CheckUsernameHasSameDeviceId(reqData);
        //    if (GenericServiceResultTemplate.HasError)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, GenericServiceResultTemplate.ErrorDesc);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, GenericServiceResultTemplate);
        //}
    }
}
