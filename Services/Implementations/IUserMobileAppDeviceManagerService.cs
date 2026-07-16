using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IUserMobileAppDeviceManagerService
    {
        Task<GenericFormModel<string>> GenerateAndSendQRCodeByUserName(string userName);
        Task<GenericFormModel<bool>> RegisterMobileAppDevice(string mobileAppDeviceInfoText);

        Task<string> GetUserIdByRegiteredDeviceId(string deviceUniqueId);
        Task<string> VerifyMobileAndSendOTP(string encryptedMobileAndMobileAppId);
        Task<GenericResponseTemplateModel<RegisteredMobileNumberHintRespViewModel>> GetRegisteredMobileNumberHintByUsername(string userName);
        Task<GenericResponseTemplateModel<string>> GenerateDeviceRegistrationQRCode(string userRefId);
        //Task<GenericResponseTemplateModel<UserDeviceFoundRespViewModel>> CheckUsernameHasSameDeviceId(string encryptedUserRefIdAndDeviceId);
    }
}
