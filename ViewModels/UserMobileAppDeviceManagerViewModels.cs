using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class MobileAppDeviceInfoViewModel
    {
        public string UserRefId { get; set; }
        public string DeviceUniqueId { get; set; }
        public string DeviceBrandName { get; set; }
        public string DeviceModelInfo { get; set; }
        public string pushNotificationDeviceToken { get; set; }
    }

    public class MobileAppRegisterByMobileNumViewModel
    {
        public string MobileNo { get; set; }
        public string MobileAppId { get; set; }
    }

    public class MobileAppDeviceResponseViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Int64 UserProfileRefId{ get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string OTP { get; set; }
        public string RoleName { get; set; }
        public int IsMobileRegistered { get; set; }
    }

    public class RegisteredMobileNumberHintRespViewModel
    {
        public string MobileNo { get; set; }
        public bool IsFound { get; set; }
        public string Msg { get; set; }
    }

    public class DeviceRegistrationQRCodeViewModel
    {
        public string MobileNo { get; set; }
        public string UserRefId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public Int64 UserProfileRefId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
    }

    public class DeviceUniqueIdAndUserRefIdViewModel
    {
        public string UserRefId { get; set; }
        public string DeviceUniqueId { get; set; }
        public string Username { get; set; }
       
    }

    public class UserDeviceFoundRespViewModel
    {
        public bool IsFound { get; set; }
        public string TokenNumber { get; set; }

    }

}
