using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Recaptcha { get; set; }
        public string tokenConnectionId { get; set; }
    }
    public class LoggedUserInfoViewModel
    {
        public string UserId { get; set; }
        public string UserPublicId { get; set; }
        public string UserName { get; set; }

        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public Int64 UserProfileId { get; set; }
        public string FullName { get; set; }
        public bool IsUserRegistered { get; set; }
        public bool IsSameProfileId { get; set; }
        public string ErrorMessage { get; set; }
        public long TimeInTicks { get; set; }
        public string LoginResponseId { get; set; }
    }

    public class GetUserRoleAndProfileDetailViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public Int64 UserProfileId { get; set; }
        public string FullName { get; set; }
    }


    public class UserDetailsViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int InvestPunjab_Ipin { get; set; }
        public int LoginType { get; set; }
    }

    public class UserLoginDetailsViewModel
    {
        public string AppUserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int DesigID { get; set; }
        public int DeptID { get; set; }
        public string Email { get; set; }
        //public int LGDDistrictId { get; set; }
        //public int LGDTehsilId { get; set; }
        public string PhoneNumber { get; set; }
        public int SiteLGDDistId { get; set; }
        public int SiteLGDTehId { get; set; }
        public string SitePin { get; set; }
        public string ComPin { get; set; }
    }

    public class CaptchaCodeViewModel
    {
        public string CaptchaCode { get; set; }
        public byte[] CaptchaByteData { get; set; }
        public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);
        public DateTime Timestamp { get; set; }
    }
    public class CaptchaResultViewModel
    {
        public string CaptchaCode { get; set; }
        public string CaptchaImg { get; set; }
        public string CaptchaToken { get; set; }
        public DateTime Timestamp { get; set; }

    }
    public class InspectionResultViewModel
    {
        public string Token { get; set; }
        public string EncryptionKey { get; set; }
        public string IVKey { get; set; }
        public int Ipin { get; set; }

    }

    public class JWT_ClaimsViewModel
    {
        public string ClaimName { get; set; }
        public string Name { get; set; }

    }
    public class DynamicFormViewModel
    {
        public string FormCreatedOn { get; set; }
        public List<DynamicFormFieldsViewModel> DynamicFormFields { get; set; }
        public string ClientId_OnCreation { get; set; }
        public string ClientId_OnSubmission { get; set; }
    }
    public class DynamicFormFieldsViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string KeyCode { get; set; }
        public bool IsClientSideEncryption { get; set; }
        public bool IsHidden { get; set; }
        public string CaptionText { get; set; }
        public string Type { get; set; }
    }

    public class LoginResponseViewModel
    {
        public string? Token { get; set; }
        public string? EncryptionKey { get; set; }
        public string? IVKey { get; set; }
        public bool HasError { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public bool IsMobileOtpVerificationReq { get; set; }
        public bool IsMobileOtpVerified { get; set; }
        public string MobileNoToBeSentOtp { get; set; }
        public string ResponseId { get; set; }

    }

    public class UserRegistrationResponseViewModel
    {
        public bool HasError { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsMobileOtpVerified { get; set; }
        public string MobileNoToBeSentOtp { get; set; }
        public bool IsEmailOtpVerified { get; set; }
        public string EmailToBeSentOtp { get; set; }

    }

    public class ResetPasswordResponseViewModel
    {
        public string? UserId { get; set; }
        public bool HasError { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public bool IsMobileOtpVerificationReq { get; set; }
        public bool IsMobileOtpVerified { get; set; }
        public string MobileNoToBeSentOtp { get; set; }
        public string ResponseId { get; set; }
    }

    public class SetNewPasswordResponseViewModel
    {
        public bool HasError { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        //public List<string> PasswordValidationErrors { get; set; }
    }

    public class GenerateTokenRespViewModel
    {
        public string Token { get; set; }
        public string EncryptionKey { get; set; }
        public string IVKey { get; set; }
    }

    public class MyOfficeValidationResponseViewModel
    {
        public bool HasError { get; set; }
        public string ErrorDesc { get; set; }
        public MyOfficeUserValidationViewModel UserData { get; set; }
    }

    public class MyOfficeUserValidationViewModel
    {
        public Int64 ProfileId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public Int64 UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public Int64? ParentUserId1 { get; set; }
        public Int64? ParentUserId2 { get; set; }
    }
    public class MyOfficeUserProfileWiseDataViewModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Int64 ProfileId { get; set; }
        public string MobileNo { get; set; }
        public Int64 UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public Int64? ParentUserId1 { get; set; }
        public Int64? ParentUserId2 { get; set; }
    }

    public class MyOfficeRequestParmsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class UserDetailsByRoleNameViewModel
    {
        public string Username { get; set; }
        public string CircleName { get; set; }
        public Int64 UserProfileId { get; set; }
        public string UserProfileName { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string UserRefId { get; set; }
        public string Signature { get; set; }
    }

    public class UpdateProfileDetailsViewModel
    {
        public string Username { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
    }

    public class BocwMobileAppUserValidationResponseViewModel
    {
        public bool IsSuccess { get; set; }
        public string UserWB { get; set; }
    }
    public class BocwMobileAppUserViewModel
    {
        public string Path { get; set; }
        public string Role { get; set; }
        public Int64 OfficerProfileId { get; set; }
        public string MobileNo { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public Int64? CircleId { get; set; }
    }
    public class BocwMobileAppUserDataViewModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public Int64 ProfileId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string ClientPasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Role { get; set; }
    }

}
