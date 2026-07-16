using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace pbsamadhannetcoreapi.Services
{
    public class UserMobileAppDeviceManagerService : IUserMobileAppDeviceManagerService
    {
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepository<UserRegisteredMobileAppDevice> _iGR_UserRegisteredMobileAppDevice;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IGenericRepository<User> _iGR_User;
        private readonly IOptions<CustomAppConfigsModel> _appSettings;
        // private IAuthService _iAuthService;
        private readonly AppDbContext _context;
        public IConfiguration Configuration { get; }
        public UserMobileAppDeviceManagerService(UserManager<User> userManager,
            IGenericRepository<UserRegisteredMobileAppDevice> iGR_UserRegisteredMobileAppDevice,
            IConfiguration configuration,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            IGenericRepository<User> iGR_User,
            //IAuthService authService,
            AppDbContext context,
            IOptions<CustomAppConfigsModel> appSettings)
        {
            _userManager = userManager;
            _iGR_UserRegisteredMobileAppDevice = iGR_UserRegisteredMobileAppDevice;
            Configuration = configuration;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iGR_User = iGR_User;
            // _iAuthService = authService;
            _context = context;
            _appSettings = appSettings;
        }
        public async Task<GenericFormModel<string>> GenerateAndSendQRCodeByUserName(string userName)
        {
            GenericFormModel<string> genericFormModel = new GenericFormModel<string>();
            try
            {
                var user = await _userManager.Users.Include(x => x.UserRoles).Include(x => x.UserProfileMapping).Include(x => x.UserProfileMapping.UserProfile).FirstOrDefaultAsync(x => x.UserName == userName);
                if (user != null)
                {
                    var rolenames = await _userManager.GetRolesAsync(user);
                    if (rolenames.Count > 0)
                    {
                        var userDetailPayload = new
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            UserProfileRefId = user.UserProfileMapping.UserProfileRefId,
                            FirstName = user.UserProfileMapping.UserProfile.FirstName,
                            Email = user.UserProfileMapping.UserProfile.Email,
                            MobileNo = user.UserProfileMapping.UserProfile.MobileNo,
                            OTP = "1234",
                            RoleName = rolenames.FirstOrDefault()
                        };
                        string userDetailPayloadJsonText = JsonConvert.SerializeObject(userDetailPayload);

                        var encryptedPayload = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(userDetailPayloadJsonText, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataEncryptionKey").Value, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataIVKey").Value);
                        genericFormModel.FormModel = encryptedPayload;
                    }
                    else
                    {
                        genericFormModel.HasError = true;
                        genericFormModel.ErrorDesc = "UserName does not have any role...!";
                    }
                }
                else
                {
                    genericFormModel.HasError = true;
                    genericFormModel.ErrorDesc = "UserName does not exists...!";
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<bool>> RegisterMobileAppDevice(string mobileAppDeviceInfoEncrytedText)
        {
            GenericFormModel<bool> genericFormModel = new GenericFormModel<bool>();
            genericFormModel.FormModel = false;
            try
            {
                var decryptedInfo = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(mobileAppDeviceInfoEncrytedText, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataEncryptionKey").Value, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataIVKey").Value);
                MobileAppDeviceInfoViewModel mobileAppDeviceInfo = JsonConvert.DeserializeObject<MobileAppDeviceInfoViewModel>(decryptedInfo);
                var alreadyRegisteredDevices = _iGR_UserRegisteredMobileAppDevice.GetAsync(x => x.UserRefId == mobileAppDeviceInfo.UserRefId || x.DeviceUniqueId == mobileAppDeviceInfo.DeviceUniqueId).Result.ToList();

                if (alreadyRegisteredDevices.Count > 0)
                {
                    _iGR_UserRegisteredMobileAppDevice.DeleteRange(alreadyRegisteredDevices);
                    await _iGR_UserRegisteredMobileAppDevice.SavechangeAsync();
                }

                UserRegisteredMobileAppDevice userRegisteredMobileAppDevice = new UserRegisteredMobileAppDevice()
                {
                    DeviceBrandName = mobileAppDeviceInfo.DeviceBrandName,
                    DeviceModelInfo = mobileAppDeviceInfo.DeviceModelInfo,
                    DeviceRegisterdOn = DateTime.Now,
                    DeviceUniqueId = mobileAppDeviceInfo.DeviceUniqueId,
                    LoginTokenKey = "AZXY",
                    UserRefId = mobileAppDeviceInfo.UserRefId,
                    PushNotificationDeviceToken = mobileAppDeviceInfo.pushNotificationDeviceToken
                };

                _iGR_UserRegisteredMobileAppDevice.Insert(userRegisteredMobileAppDevice);
                await _iGR_UserRegisteredMobileAppDevice.SavechangeAsync();

                genericFormModel.FormModel = true;

            }
            catch (Exception ex)
            {
                genericFormModel.FormModel = false;
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<string> GetUserIdByRegiteredDeviceId(string deviceUniqueId)
        {
            string userId = null;
            try
            {
                var userRegisteredMobileAppDevices = _iGR_UserRegisteredMobileAppDevice.GetAsync(x => x.DeviceUniqueId == deviceUniqueId).Result.ToList();
                if (userRegisteredMobileAppDevices.Count() != 1)
                {
                    return string.Empty;
                }
                userId = userRegisteredMobileAppDevices.First().UserRefId;
            }
            catch (Exception ex)
            {
                userId = string.Empty;
            }
            return userId;
        }

        public async Task<string> VerifyMobileAndSendOTP(string encryptedMobileNoAndMobileAppId)
        {
            MobileAppDeviceResponseViewModel mobileAppDeviceResponse = new MobileAppDeviceResponseViewModel();

            try
            {

                var decryptedPayload = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(encryptedMobileNoAndMobileAppId, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataEncryptionKey").Value, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataIVKey").Value);
                MobileAppRegisterByMobileNumViewModel clientInfo = JsonConvert.DeserializeObject<MobileAppRegisterByMobileNumViewModel>(decryptedPayload);

                if (clientInfo != null)
                {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="MobileNo", ParmValue= clientInfo.MobileNo, isNumber=false}
                    };
                    var userInfo = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<MobileAppDeviceResponseViewModel>("dbo.sp_MobileDevice_VerifyByMobile", storeProcedureParms);
                    if (userInfo.Count() == 1)
                    {
                        mobileAppDeviceResponse.IsMobileRegistered = 1;

                        mobileAppDeviceResponse.UserId = userInfo.FirstOrDefault().UserId;
                        mobileAppDeviceResponse.UserName = userInfo.FirstOrDefault().UserName;
                        mobileAppDeviceResponse.UserProfileRefId = userInfo.FirstOrDefault().UserProfileRefId;
                        mobileAppDeviceResponse.FirstName = userInfo.FirstOrDefault().FirstName;
                        mobileAppDeviceResponse.Email = userInfo.FirstOrDefault().Email;
                        mobileAppDeviceResponse.MobileNo = userInfo.FirstOrDefault().MobileNo;
                        mobileAppDeviceResponse.RoleName = userInfo.FirstOrDefault().RoleName;

                        Random _rdm = new Random();
                        mobileAppDeviceResponse.OTP = _rdm.Next(1234, 9876).ToString();
                    }
                    else if (userInfo.Count() == 0)
                    {
                        mobileAppDeviceResponse.IsMobileRegistered = 0;

                    }
                    else
                    {
                        mobileAppDeviceResponse.IsMobileRegistered = 0;
                    }

                }
                else
                {
                    mobileAppDeviceResponse.IsMobileRegistered = 0;
                }
                return RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(JsonConvert.SerializeObject(mobileAppDeviceResponse), Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataEncryptionKey").Value, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataIVKey").Value);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<GenericResponseTemplateModel<RegisteredMobileNumberHintRespViewModel>> GetRegisteredMobileNumberHintByUsername(string userName)
        {
            GenericResponseTemplateModel<RegisteredMobileNumberHintRespViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<RegisteredMobileNumberHintRespViewModel>() { ErrorDesc = null, HasError = false };
            try
            {
                var user = await _iGR_User.GetAsync(x => x.UserName.ToLower() == userName.ToLower(), null, x => x.UserProfileMapping.UserProfile).ConfigureAwait(false);
                if (user.Count() > 0)
                {

                    if (user.FirstOrDefault().UserProfileMapping.UserProfile.MobileNo != null)
                    {
                        string maskedMobileNo = CommonMethods.MaskMobileNumber(user.FirstOrDefault().UserProfileMapping.UserProfile.MobileNo);
                        genericServiceResultTemplate.ResponseDataModel = new RegisteredMobileNumberHintRespViewModel()
                        {
                            MobileNo = maskedMobileNo,
                            Msg = null,
                            IsFound = true
                        };
                    }
                    else
                    {
                        genericServiceResultTemplate.ResponseDataModel = new RegisteredMobileNumberHintRespViewModel()
                        {
                            MobileNo = null,
                            Msg = "Invalid mobile number mapped",
                            IsFound = false
                        };
                    }
                }
                else
                {
                    genericServiceResultTemplate.ResponseDataModel = new RegisteredMobileNumberHintRespViewModel()
                    {
                        MobileNo = null,
                        Msg = "Username not found",
                        IsFound = false
                    };
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }

            return genericServiceResultTemplate;
        }


        public async Task<GenericResponseTemplateModel<string>> GenerateDeviceRegistrationQRCode(string userRefId)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = new GenericResponseTemplateModel<string>();
            try
            {
                //var user = await _iGR_User.GetAsync(x => x.UserProfileMapping.UserRefId.ToLower() == userRefId.ToLower(), null, x => x.UserProfileMapping.UserProfile,).ConfigureAwait(false);
                var user = await _context.Users.Where(x => x.Id == userRefId).Include(x => x.UserRoles).ThenInclude(x => x.Role).Include(x => x.UserProfileMapping).ThenInclude(x => x.UserProfile).FirstOrDefaultAsync();
                if (user != null)
                {
                    //var userMobileNo = user.FirstOrDefault().UserProfileMapping.UserProfile.MobileNo;
                    var userData = new DeviceRegistrationQRCodeViewModel()
                    {
                        MobileNo = user.UserProfileMapping.UserProfile.MobileNo,
                        UserRefId = userRefId,
                        Email = user.UserProfileMapping.UserProfile.Email,
                        FirstName = user.UserProfileMapping.UserProfile.FirstName,
                        UserProfileRefId = user.UserProfileMapping.UserProfileRefId,
                        Username = user.UserName,
                        TimeStamp = DateTime.Now,
                        RoleName = user.UserRoles.FirstOrDefault().Role.NormalizedName,
                    };


                    genericServiceResultTemplate.ResponseDataModel = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(
                        JsonConvert.SerializeObject(userData),
                        Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataEncryptionKey").Value,
                        Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataIVKey").Value);
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }

            return genericServiceResultTemplate;
        }

        //public async Task<GenericResponseTemplateModel<UserDeviceFoundRespViewModel>> CheckUsernameHasSameDeviceId(string reqData)
        //{
        //    //DeviceUniqueIdAndUserRefIdViewModel mobileAppDeviceResponse = new DeviceUniqueIdAndUserRefIdViewModel();
        //    GenericResponseTemplateModel<UserDeviceFoundRespViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<UserDeviceFoundRespViewModel>() { HasError = false, ErrorDesc = null };
        //    try
        //    {
        //        var decryptedPayload = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(reqData, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataEncryptionKey").Value, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataIVKey").Value);
        //        DeviceUniqueIdAndUserRefIdViewModel deviceInfo = JsonConvert.DeserializeObject<DeviceUniqueIdAndUserRefIdViewModel>(decryptedPayload);

        //        var user = await _iGR_User.GetAsync(x => x.UserRegisteredMobileAppDevice.UserRefId.ToLower() == deviceInfo.UserRefId.ToLower() && x.UserRegisteredMobileAppDevice.DeviceUniqueId.ToLower() == deviceInfo.DeviceUniqueId.ToLower(),
        //            null,
        //            x => x.UserRoles,
        //            x => x.UserProfileMapping.UserProfile
        //            ).ConfigureAwait(false);


        //        if (user.Count()>0)
        //        {
        //            LoggedUserInfoViewModel loggedUserInfoViewModel = new LoggedUserInfoViewModel();
        //            if (user.FirstOrDefault().UserRoles.Count > 0)
        //            {
        //                var role = _context.Roles.Where(x => x.Id == user.FirstOrDefault().UserRoles.FirstOrDefault().RoleId).FirstOrDefault();

        //                loggedUserInfoViewModel.FullName = string.Concat(user.FirstOrDefault().UserProfileMapping.UserProfile.FirstName, " ", user.FirstOrDefault().UserProfileMapping.UserProfile.LastName);
        //                loggedUserInfoViewModel.UserProfileId = user.FirstOrDefault().UserProfileMapping.UserProfileRefId;
        //                loggedUserInfoViewModel.RoleCode = role.Name;
        //                loggedUserInfoViewModel.RoleName = role.Name;
        //                loggedUserInfoViewModel.UserId = user.FirstOrDefault().Id;
        //                loggedUserInfoViewModel.UserName = user.FirstOrDefault().UserName;
        //                loggedUserInfoViewModel.UserPublicId = user.FirstOrDefault().Id;
        //                loggedUserInfoViewModel.RoleId = role.Id;

        //                var secrateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.JwtConfigs.SecurityKey));
        //                var signingCredentials = new SigningCredentials(secrateKey, SecurityAlgorithms.HmacSha256);
        //                var claims = new[] {
        //                new Claim(ClaimTypes.Name, loggedUserInfoViewModel.UserName),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                new Claim("UserPublicId", loggedUserInfoViewModel.UserPublicId),
        //                new Claim("UserId", loggedUserInfoViewModel.UserId.ToString()),
            

        //                //new Claim("RoleId", loggedUserInfoViewModel.RoleId.ToString()),
        //                new Claim("RoleCode", loggedUserInfoViewModel.RoleCode),
        //                new Claim("RoleName", loggedUserInfoViewModel.RoleName),
        //                new Claim(ClaimTypes.Role, loggedUserInfoViewModel.RoleCode),
        //                new Claim("UserProfileId", loggedUserInfoViewModel.UserProfileId.ToString()),
        //                new Claim("FullName", loggedUserInfoViewModel.FullName),
        //                };

        //                var tokenOptions = new JwtSecurityToken(
        //                        issuer: _appSettings.Value.JwtConfigs.Issuer,
        //                        audience: _appSettings.Value.JwtConfigs.Audience,
        //                        claims: claims,

        //                        expires: DateTime.Now.AddHours(5),
        //                        signingCredentials: signingCredentials
        //                    );
        //                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        //                genericServiceResultTemplate.ResponseDataModel = new UserDeviceFoundRespViewModel()
        //                {
        //                    IsFound = true,
        //                    TokenNumber = tokenString,
        //                };
        //            }
        //            else
        //            {
        //                genericServiceResultTemplate.ResponseDataModel = new UserDeviceFoundRespViewModel()
        //                {
        //                    IsFound = false,
        //                    TokenNumber = null,
        //                };
        //            }
        //        }
        //        else
        //        {
        //            genericServiceResultTemplate.ResponseDataModel = new UserDeviceFoundRespViewModel()
        //            {
        //                IsFound = false,
        //                TokenNumber = null,
        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        genericServiceResultTemplate.HasError = true;
        //        genericServiceResultTemplate.ErrorDesc = ex.Message;
        //    }
        //    return genericServiceResultTemplate;
        //}

    }
}
