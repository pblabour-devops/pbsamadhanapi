using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class NotificationViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string AnonymusEmailOrMobile { get; set; }
        public NotificationModeTypeEnum NotificationMode { get; set; }
        public NotificationPurposeTypeEnum NotificationPurpose { get; set; }
    }

    public class ReturnResponseInBodyViewModel
    {
        public string Body { get; set; }
        public string DeviceId { get; set; }
        public string TokenData { get; set; }
        public bool IsConfirmationRequired { get; set; }
        public string ActionType { get; set; }
    }

    public class DormantFileNotificationViewModel
    {
        public string ApplicationTypeTitle { get; set; }
        public string ApplicationPurposeTitle { get; set; }
        public string EstablishmentName { get; set; }
        public string PublicApplicationRefNo { get; set; }
        public string StatusDescription { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public string UserRefId { get; set; }
        public Int64 IterationCount { get; set; }
        public Int64 DormantInTime { get; set; }
    }
    public class OTPVerifyRespViewModel
    {
        public bool IsOtpMatched { get; set; }
        public string Mobile { get; set; }
        public string EnteredOtp { get; set; }
        public long TimeStemp { get; set; }
    }
    public class ResetPasswordEmailVarificationNotificationViewModel
    {
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string ValidUpto { get; set; }
        public string VerificationLink { get; set; }

    }
    public class OTPRegistrationVerifyRespViewModel
    {
        public bool IsMobileOtpMatched { get; set; }
        public string Mobile { get; set; }
        public string EnteredMobileOtp { get; set; }
        public bool IsEmailOtpMatched { get; set; }
        public string Email { get; set; }
        public string EnteredEmailOtp { get; set; }
        public long TimeStemp { get; set; }
        public bool IsUserCreated { get; set; }
        public bool HasError { get; set; }
        public string ErrorDesc { get; set; }
        public string ErrorCode { get; set; }
    }
    public class NewRegistrationNotificationViewModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class EmailVarificationOTPNotificationViewModel
    {
        public string FullName { get; set; }
    }
}
