using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Middlewares;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _iAuthService;
        private ICommonLicenceService _icommonLicenceService;
        private readonly INotificationManagerService _iNotificationManagerService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        public AuthController(IAuthService authService, 
            INotificationManagerService iNotificationManagerService,
            ICommonLicenceService commonLicenceService
            //,IThirdPartyInegrationsService iThirdPartyInegrationsService

            )
        {
            _iAuthService = authService;
            _iNotificationManagerService = iNotificationManagerService;
            _icommonLicenceService = commonLicenceService;     

        }

        //[HttpPost, Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginViewModel requestData)
        //{
        //    //await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(280191, ApplicationTypeEnum.BUILDING_PLAN_HUD, AppActionTypeEnum.APP_SUBMITTED);

        //    if (requestData == null)
        //        return BadRequest("Invalid login request. No credentials are provided");

        //    //var loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(JsonConvert.SerializeObject(requestData));
        //    LoggedUserInfoViewModel loggedUserInfoViewModel = await _iAuthService.GetUserByUserNameAndPassword(requestData, false);
        //    if (loggedUserInfoViewModel != null)
        //    {
        //        var tokenString = await _iAuthService.GenerateNewJwtToken(loggedUserInfoViewModel);

        //        // Call notification service here
        //        ReturnResponseInBodyViewModel returnBodyViewModel = new ReturnResponseInBodyViewModel();
        //        returnBodyViewModel.Body = "Application Logged-In Successfully..!";
        //        returnBodyViewModel.DeviceId = "aaaaaa";
        //        returnBodyViewModel.TokenData = requestData.tokenConnectionId;
        //        returnBodyViewModel.IsConfirmationRequired = true;
        //        returnBodyViewModel.ActionType = "login";
        //        string notificationReturnData = JsonConvert.SerializeObject(returnBodyViewModel);
        //        //List<NotificationViewModel> notifications = new List<NotificationViewModel>()
        //        //{
        //        //   new NotificationViewModel (){Body= notificationReturnData, Title="User ("+loggedUserInfoViewModel.UserName+") is logged in successfully.!", NotificationMode=NotificationModeTypeEnum.MOBILE_APP, NotificationPurpose=NotificationPurposeTypeEnum.INFO }
        //        //};
        //        //await _iNotificationManagerService.DraftLoginAuthenticationNotification(loggedUserInfoViewModel.UserId, notifications, tokenString);

        //        return StatusCode(StatusCodes.Status200OK, new { Token = tokenString, IsAvailable = true });
        //    }


        //    return StatusCode(StatusCodes.Status200OK, new { Token = "", IsAvailable = false });
        //}

        //[HttpPost, Route("5810BA8C-E5FE-4BF5-AABE-CF354CD10F81")]
        [HttpPost, Route("EF61B022-EADF-4921-AA51-9F7AF16A1F7B")]
        public async Task<IActionResult> DirectLogin([FromBody] LoginViewModel requestData)
        {
            if (requestData == null)
                return BadRequest("Invalid login request. No credentials are provided");
            LoggedUserInfoViewModel loggedUserInfoViewModel = await _iAuthService.GetUserByUserNameAndPassword(requestData, true);
            if (loggedUserInfoViewModel != null)
            {
                var tokenString = await _iAuthService.GenerateNewJwtToken(loggedUserInfoViewModel, Request);

                ReturnResponseInBodyViewModel returnBodyViewModel = new ReturnResponseInBodyViewModel();
                returnBodyViewModel.Body = "Application Logged-In Successfully..!";
                returnBodyViewModel.DeviceId = "aaaaaa";
                returnBodyViewModel.TokenData = requestData.tokenConnectionId;
                returnBodyViewModel.IsConfirmationRequired = true;
                returnBodyViewModel.ActionType = "login";
                string notificationReturnData = JsonConvert.SerializeObject(returnBodyViewModel);
                //List<NotificationViewModel> notifications = new List<NotificationViewModel>()
                //{
                //   new NotificationViewModel (){Body= notificationReturnData, Title="User ("+loggedUserInfoViewModel.UserName+") is logged in successfully.!", NotificationMode=NotificationModeTypeEnum.MOBILE_APP, NotificationPurpose=NotificationPurposeTypeEnum.INFO }
                //};
                //await _iNotificationManagerService.DraftLoginAuthenticationNotification(loggedUserInfoViewModel.UserId, notifications, tokenString);

                return StatusCode(StatusCodes.Status200OK, new { Token = tokenString, IsAvailable = true });
            }
            return StatusCode(StatusCodes.Status200OK, new { Token = "", IsAvailable = false });
        }

        [HttpGet, Route("get_clientid_from_QrCode")]
        public async Task<IActionResult> Get_clientid_from_QrCode(string clientId, string deviceUniqueId, Int64 userProfileRefId)
        {
            LoggedUserInfoViewModel loggedUserInfoViewModel = await _iAuthService.GetUserByRegisteredDeviceId(deviceUniqueId, userProfileRefId);
            if (loggedUserInfoViewModel.IsUserRegistered && loggedUserInfoViewModel.IsSameProfileId)
            {
                //var tokenString = await _iAuthService.GenerateNewJwtToken(loggedUserInfoViewModel, Request);
                //await _iAuthService.SendClientQuickLoginResponse(clientId, tokenString);
                //return StatusCode(StatusCodes.Status200OK, new { Token = tokenString, IsAvailable = true });
            }
            return StatusCode(StatusCodes.Status200OK, new { IsSameProfileId = loggedUserInfoViewModel.IsSameProfileId, IsUserRegistered = loggedUserInfoViewModel.IsUserRegistered, ErrorMessage = loggedUserInfoViewModel.ErrorMessage });
        }

        [HttpGet, HttpGet("inspection")]
        public async Task<IActionResult> GetAuthToken([FromQuery] string rawToken)
        {
            GenericResponseTemplateModel<InspectionResultViewModel> genericFormModel = await _iAuthService.GetAuthToken(rawToken, Request);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getCaptchaImage")]
        public async Task<IActionResult> GetCaptchaImage()
        {
            GenericResponseTemplateModel<CaptchaResultViewModel> genericFormModel = await _icommonLicenceService.GetCaptchaImage();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        public class MemoryStreamJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(MemoryStream).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var bytes = serializer.Deserialize<byte[]>(reader);
                return bytes != null ? new MemoryStream(bytes) : new MemoryStream();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var bytes = ((MemoryStream)value).ToArray();
                serializer.Serialize(writer, bytes);
            }
        }


        [HttpGet, Route("getAllOfficialRoles")]
        public async Task<IActionResult> GetAllOfficialRoles()
        {
            GenericResponseTemplateModel<List<ApplicationRole>> genericFormModel = await _iAuthService.GetAllOfficialRoles();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> LoginByUsernameAndPassword([FromBody] DynamicFormViewModel requestData)
        {
            var loginResponse = await _iAuthService.ValidateLoginInputForm(requestData, Request);
            return StatusCode(StatusCodes.Status200OK, new { EncryptedResp = loginResponse });
        }
        [HttpPost, Route("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] DynamicFormViewModel requestData)
        {
            var loginResponse = await _iAuthService.ResetPassword(requestData);
            return StatusCode(StatusCodes.Status200OK, new { EncryptedResp = loginResponse });
        }
        [HttpPost, Route("setNewPassword")]
        public async Task<IActionResult> SetNewPassword([FromBody] DynamicFormViewModel requestData)
        {
            var loginResponse = await _iAuthService.SetNewPassword(requestData);
            return StatusCode(StatusCodes.Status200OK, new { EncryptedResp = loginResponse });
        }

        [HttpGet, Route("ValidateViaMyOffice")]
        public async Task<IActionResult> ValidateViaMyOffice([FromQuery] string requestData)
        {
            var loginResponse = await _iAuthService.ValidateViaMyOffice(requestData, Request);
            return StatusCode(StatusCodes.Status200OK, new { EncryptedResp = loginResponse });
        }

        [Route("userInfo2050")]
        [HttpGet]
        [CustomFillters.AuthorizeAttribute("LB1N,DEVTEAM,HELPDESK")]
        public async Task<IActionResult> GetUserDetailsByRoleName([FromQuery] string roleName, string username)
        {
            GenericListModel<UserDetailsByRoleNameViewModel> genericListModel = await _iAuthService.GetUserDetailsByRoleName(roleName, username);
            if (genericListModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericListModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericListModel);
        }

        [Route("updateProfileDetails")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("LB1N,DEVTEAM,HELPDESK")]
        public async Task<IActionResult> UpdateProfileDetails([FromBody] UpdateProfileDetailsViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iAuthService.UpdateProfileDetails(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("ValidateViaBocwMobileApp")]
        public async Task<IActionResult> ValidateViaBocwMobileApp([FromQuery] string requestData)
        {
            var loginResponse = await _iAuthService.ValidateViaBocwMobileApp(requestData, Request);
            return StatusCode(StatusCodes.Status200OK, loginResponse);
        }

        [HttpPost, Route("userRegistration")]
        public async Task<IActionResult> UserRegistration([FromBody] DynamicFormViewModel requestData)
        {
            var loginResponse = await _iAuthService.ValidateUserRegistrationForm(requestData, Request);
            return StatusCode(StatusCodes.Status200OK, new { EncryptedResp = loginResponse });
        }
        [HttpPost, Route("validateRegistrationOTP")]
        public async Task<IActionResult> ValidateRegistrationOTP([FromBody] DynamicFormViewModel requestData)
        {
            //string firstName, string middleName, string lastName, string mobile, string enteredMobileOTP, string email, string enteredEmailOTP
            string otpVerifyResp = await _iAuthService.ValidateRegistrationOTP(requestData, Request);
            return StatusCode(StatusCodes.Status200OK, new { OtpVerifyResp = otpVerifyResp });
        }
    }
}
