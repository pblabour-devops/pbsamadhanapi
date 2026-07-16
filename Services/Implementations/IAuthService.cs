using Microsoft.AspNetCore.Http;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IAuthService
    {
        Task<LoggedUserInfoViewModel> GetUserByUserNameAndPassword(LoginViewModel loginViewModel, bool bypassPasswordCheck);
        Task<GenerateTokenRespViewModel> GenerateNewJwtToken(LoggedUserInfoViewModel loggedUserInfoViewModel, HttpRequest httpRequest);
        Task SendClientQuickLoginResponse(string clientId, string token);
        Task<LoggedUserInfoViewModel> GetUserByRegisteredDeviceId(string deviceUniqueId, Int64 userProfileRefId);
        Task<UserProfile> GetUserProfileByUserId(string userRefId);
        Task<GenericListModel<UserFullDetailsViewModel>> GetUserFullDetailByUserName(string userName);
        Task<LoggedUserInfoViewModel> GetUserLoginDeatilByUserId(string id);
        Task<GenericResponseTemplateModel<string>> CreateNewUser(UserProfile userProfile, string roleId, string defaultPassword);
        Task<GenericResponseTemplateModel<Int64>> CreateNewUserProfile(UserProfile userProfile);
        Task<GenericResponseTemplateModel<Int64>> MapUserWithProfile(string userRefId, Int64 userProfileRefId);
        Task<UserProfile> GetUserProfileByProfileId(Int64 userProfileId);
        Task<ApplicationRole> GetUserRoleByRoleId(string roleId);
        Task<string> GetRoleNameByUserId(string userId);
        Task<GenericListModel<OfficerDetailsByRoleNameViewModel>> GetOfficerDetailsByRoleName(string roleName);
        Task<GetUserRoleAndProfileDetailViewModel> GetUserRoleAndProfileDetailByUserId(string id);
        Task<UserProfileMapping> GetUserProfileByUserRefId(string id);
        Task<GenericResponseTemplateModel<InspectionResultViewModel>> GetAuthToken(string rawToken, HttpRequest httpRequest);
        Task<string> GenerateCaptchaJwtToken(string captcha);
        Task<GenericResponseTemplateModel<List<ApplicationRole>>> GetAllOfficialRoles();
        Task<LoggedUserInfoViewModel> DecryptLoggedInUserClaims(IEnumerable<Claim> Claims);

        //Task<bool> CheckUserName(string userName);
        //Task<LoggedUserInfoViewModel> GetUserIfPasswordMatched(string userName, string password);
        //Task<string> ValidateAndGetOriginalPassword(string password);
        Task<string> ValidateLoginInputForm(DynamicFormViewModel requestData, HttpRequest httpRequest);
        Task<string> ResetPassword(DynamicFormViewModel requestData);
        Task<string> SetNewPassword(DynamicFormViewModel requestData);
        Task<string> ValidateViaMyOffice(string requestData, HttpRequest httpRequest);
        Task<GenericListModel<UserDetailsByRoleNameViewModel>> GetUserDetailsByRoleName(string roleName, string username);
        Task<GenericServiceResultTemplate> UpdateProfileDetails(UpdateProfileDetailsViewModel requestData);
        Task<BocwMobileAppUserValidationResponseViewModel> ValidateViaBocwMobileApp(string requestData, HttpRequest httpRequest);
        Task<string> ValidateUserRegistrationForm(DynamicFormViewModel requestData, HttpRequest httpRequest);
        public string GenerateDefaultPassword(int length = 12);
        Task<string> ValidateRegistrationOTP(DynamicFormViewModel requestData, HttpRequest httpRequest);
        Task<GenericResponseTemplateModel<string>> PrepareTokenForLWB(string userId, string licenceNo, string cessId);
    }
}