using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pbsamadhannetcoreapi.Services
{
    public class NotificationManagerService : INotificationManagerService
    {
        private readonly IGenericRepository<Notification> _iGR_Notification;
        public IConfiguration _iConfiguration { get; }
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IAuthService _iAuthService;
        private readonly ICircleManager _iCircleManager;
        public NotificationManagerService(IGenericRepository<Notification> iGR_Notification,
                IConfiguration iConfiguration,
                IRazorViewEngine razorViewEngine,
                ITempDataProvider tempDataProvider,
                IServiceProvider serviceProvider,
                AppDbContext context,
                IGeneric_SP_Repository iGeneric_SP_Repository,
                IAuthService iAuthService,
                ICircleManager circleManager)
        {
            _iGR_Notification = iGR_Notification;
            _iConfiguration = iConfiguration;
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iAuthService = iAuthService;
            _iCircleManager = circleManager;
        }
        public async Task<bool> InitiateNotification(string sendToUserRefId, List<NotificationViewModel> notifications)
        {
            //notifications = notifications.Select(x => x.NotificationMode).Distinct();
            Notification notificationInfo = null;
            foreach (var notification in notifications)
            {
                notificationInfo = new Notification()
                {
                    Body = notification.Body,
                    CreatedOn = DateTime.Now,
                    ExpireInMinutes = 10,
                    IsExpirable = (notification.NotificationPurpose == NotificationPurposeTypeEnum.OTP),
                    NotificationMode = notification.NotificationMode,
                    NotificationPurpose = notification.NotificationPurpose,
                    NotificationStatus = NotificationStatusTypeEnum.NEW,
                    ResponseOn = DateTime.Now,
                    ResponseText = "",
                    SendToUserRefId = sendToUserRefId,
                    Title = notification.Title,
                    AnonymusEmailOrMobile = notification.AnonymusEmailOrMobile

                };
                _iGR_Notification.Insert(notificationInfo);
                await _iGR_Notification.SavechangeAsync();
            }
            return true;
        }

        public async Task<bool> DraftLoginAuthenticationNotification(string userRefId, List<NotificationViewModel> notifications, string tokenString)
        {
            Notification notificationInfo = null;
            foreach (var notification in notifications)
            {
                notificationInfo = new Notification()
                {
                    Body = notification.Body,
                    CreatedOn = DateTime.Now,
                    ExpireInMinutes = 10,
                    IsExpirable = (notification.NotificationPurpose == NotificationPurposeTypeEnum.OTP),
                    NotificationMode = notification.NotificationMode,
                    NotificationPurpose = notification.NotificationPurpose,
                    NotificationStatus = NotificationStatusTypeEnum.NEW,
                    ResponseOn = DateTime.Now,
                    ResponseText = "",
                    SendToUserRefId = userRefId,
                    Title = notification.Title
                };
                _iGR_Notification.Insert(notificationInfo);
                await _iGR_Notification.SavechangeAsync();
            }
            return true;
        }

        public async Task<bool> SendSmsViaBOCWBoard(string mobileNo, string otpText)
        {
            GenericResponseTemplateModel<BusinessFirstSCAFViewModel> genericResponseTemplateModel = new GenericResponseTemplateModel<BusinessFirstSCAFViewModel>() { HasError = false, ErrorDesc = "" };
            try
            {
                string url = _iConfiguration.GetSection("NotificationConfigs").GetSection("BOCWBoard_OtpServiceURL").Value;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(new { UserMobile = mobileNo, smsText = otpText }), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        genericResponseTemplateModel.ResponseDataModel = JsonConvert.DeserializeObject<BusinessFirstSCAFViewModel>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<GenericResponseTemplateModel<bool>> SendEmailSMTP(string receiverName, string receiverEmail, string subjectEmail, string bodyEmailHtml)
        {
            GenericResponseTemplateModel<bool> genericResponseTemplateModel = new GenericResponseTemplateModel<bool>() { ErrorDesc = null, HasError = false, ResponseDataModel = true };
            Notification notification = new Notification()
            {
                Body = bodyEmailHtml,
                CreatedOn = DateTime.Now,
                ExpireInMinutes = 0,
                IsExpirable = false,
                NotificationMode = NotificationModeTypeEnum.EMAIL,
                NotificationPurpose = NotificationPurposeTypeEnum.INFO,
                NotificationStatus = NotificationStatusTypeEnum.SENT,
                ResponseOn = DateTime.Now,
                ResponseText = "",
                SendToUserRefId = receiverEmail,
                Title = subjectEmail
            };
            try
            {
                var message = new MimeMessage();

                var smtpConfigs = _iConfiguration.GetSection("NotificationConfigs").GetSection("SmtpConfigs");

                message.From.Add(new MailboxAddress(smtpConfigs.GetSection("EmailName").Value, smtpConfigs.GetSection("NoReplyEmail").Value));
                message.To.Add(new MailboxAddress(receiverName, receiverEmail));
                message.Subject = subjectEmail;
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = bodyEmailHtml;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(smtpConfigs.GetSection("SmtpServer").Value, Convert.ToInt32(smtpConfigs.GetSection("SmtpPort").Value), true);
                    client.Authenticate(smtpConfigs.GetSection("NoReplyEmail").Value, smtpConfigs.GetSection("NoReplyEmailPassword").Value);
                    client.Send(message);
                    client.Disconnect(true);
                }
                notification.ResponseText = "Sent";
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.ResponseDataModel = false;
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                notification.ResponseText = ex.Message;
                notification.NotificationStatus = NotificationStatusTypeEnum.Has_Error;
                throw ex;
            }
            _iGR_Notification.Insert(notification);
            await _iGR_Notification.SavechangeAsync();
            return genericResponseTemplateModel;
        }
        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }

        public async Task<GenericResponseTemplateModel<bool>> SendRandomizationNotificationToFactoryByRandomizationId(Int64 randomizationId)
        {
            GenericResponseTemplateModel<bool> genericResponseTemplateModel = new GenericResponseTemplateModel<bool>() { ErrorDesc = null, HasError = false, ResponseDataModel = true };
            try
            {
                var randMasterData = _context.Inspection_Randomizations.Include(x => x.Inspection_Masters).Where(x => x.RandomizationId == randomizationId).FirstOrDefault();
                var distinctFactCircles = randMasterData.Inspection_Masters.Select(x => x.FactoryCircleId).Distinct().ToList();
                TemplateInspectionOfficerUserNotificationViewModel mailTemplate = new TemplateInspectionOfficerUserNotificationViewModel();

                foreach (var factCircle in distinctFactCircles)
                {
                    var userRefId = randMasterData.Inspection_Masters.Where(x => x.FactoryCircleId == factCircle).FirstOrDefault().UserId;
                    var profileRefId = await _context.UserProfileMapping.Where(x => x.UserRefId == userRefId && x.IsActive == true).Select(x => x.UserProfileRefId).FirstOrDefaultAsync();
                    var officerProfile = await _context.UserProfiles.Where(x => x.UserProfileId == profileRefId).FirstOrDefaultAsync();

                    var circleDetails = await _iCircleManager.GetCircleDetailsByCircleId(factCircle, CircleTypeEnum.FACTORY_CIRCLE);
                    mailTemplate = new TemplateInspectionOfficerUserNotificationViewModel()
                    {
                        CircleName = circleDetails.ResponseDataModel,
                        Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(randMasterData.Month),
                        Year = randMasterData.Year,
                        OfficerName = officerProfile.FirstName + ' ' + officerProfile.MiddleName + ' ' + officerProfile.LastName
                    };

                    //mailTemplate
                    mailTemplate.EstablishmentsList = new List<TemplateInspectionEstablishmentDetails>();
                    foreach (var item in randMasterData.Inspection_Masters.Where(x => x.FactoryCircleId == factCircle).ToList())
                    {
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=item.LicenceNumber.ToString(), isNumber=false}
                        };
                        var establishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionFactoryDetailsViewModel>("sp_GetInspectionFactoryDetailsByLicenceNo", storeProcedureParms);

                        mailTemplate.EstablishmentsList.Add(new TemplateInspectionEstablishmentDetails()
                        {
                            LicenseNumber = item.LicenceNumber,
                            EstablishmentAddress = establishmentDetails.FirstOrDefault().FactoryAddress,
                            EstablishmentName = establishmentDetails.FirstOrDefault().FactoryName
                        });
                    }

                    var emailTemplateText = await RenderToStringAsync("EmailTemplates/_templateInspectionFactoryWingNotification", mailTemplate);
                    await InitiateNotification(randMasterData.Inspection_Masters.Where(x => x.FactoryCircleId == factCircle).FirstOrDefault().UserId,
                    new List<NotificationViewModel>()
                    {
                        new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: Inspection randomization (" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(randMasterData.Month) + " - " + randMasterData.Year.ToString() + ") under factory circle :"+ circleDetails.ResponseDataModel}
                    });
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.ResponseDataModel = false;
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> SendInspectionNotificationToLabourWingByFactoryCircleId(Int64 randomizationId, Int64 factoryCircleId)
        {
            GenericResponseTemplateModel<bool> genericResponseTemplateModel = new GenericResponseTemplateModel<bool>() { ErrorDesc = null, HasError = false, ResponseDataModel = true };
            try
            {
                var randMasterData = _context.Inspection_Randomizations.Include(x => x.Inspection_Masters).Where(x => x.RandomizationId == randomizationId).FirstOrDefault();
                var distinctLabCircles = randMasterData.Inspection_Masters.Where(x => x.FactoryCircleId == factoryCircleId).Select(x => x.LabourCircleRefId).Distinct().ToList();

                TemplateInspectionOfficerUserNotificationViewModel mailTemplate = new TemplateInspectionOfficerUserNotificationViewModel();

                foreach (var labCircle in distinctLabCircles)
                {
                    var userRefId = _context.UserCircleMappings.Where(x => x.LabourCircleRefId == labCircle).Select(x => x.UserRefId).FirstOrDefault();
                    var profileRefId = await _context.UserProfileMapping.Where(x => x.UserRefId == userRefId && x.IsActive == true).Select(x => x.UserProfileRefId).FirstOrDefaultAsync();
                    var officerProfile = await _context.UserProfiles.Where(x => x.UserProfileId == profileRefId).FirstOrDefaultAsync();


                    //var officerProfile = await _iAuthService.GetUserProfileByUserId(_context.UserCircleMappings.Where(x => x.LabourCircleRefId == labCircle).Select(x => x.UserRefId).FirstOrDefault());
                    var circleDetails = await _iCircleManager.GetCircleDetailsByCircleId(labCircle, CircleTypeEnum.LABOUR_CIRCLE);
                    mailTemplate = new TemplateInspectionOfficerUserNotificationViewModel()
                    {
                        CircleName = circleDetails.ResponseDataModel,
                        Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(randMasterData.Month),
                        Year = randMasterData.Year,
                        OfficerName = officerProfile.FirstName + ' ' + officerProfile.MiddleName + ' ' + officerProfile.LastName
                    };

                    //mailTemplate
                    mailTemplate.EstablishmentsList = new List<TemplateInspectionEstablishmentDetails>();
                    foreach (var item in randMasterData.Inspection_Masters.Where(x => x.LabourCircleRefId == labCircle).ToList())
                    {
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=item.LicenceNumber.ToString(), isNumber=false}
                        };
                        var establishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionFactoryDetailsViewModel>("sp_GetInspectionFactoryDetailsByLicenceNo", storeProcedureParms);

                        mailTemplate.EstablishmentsList.Add(new TemplateInspectionEstablishmentDetails()
                        {
                            LicenseNumber = item.LicenceNumber,
                            EstablishmentAddress = establishmentDetails.FirstOrDefault().FactoryAddress,
                            EstablishmentName = establishmentDetails.FirstOrDefault().FactoryName
                        });
                    }

                    var emailTemplateText = await RenderToStringAsync("EmailTemplates/_templateInspectionLabourWingNotification", mailTemplate);
                    await InitiateNotification(randMasterData.Inspection_Masters.Where(x => x.LabourCircleRefId == labCircle).FirstOrDefault().UserId,
                    new List<NotificationViewModel>()
                    {
                        new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: Inspection randomization (" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(randMasterData.Month) + " - " + randMasterData.Year.ToString() + ") under labour circle :"+ circleDetails.ResponseDataModel}
                    });
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.ResponseDataModel = false;
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> SendInspectionNotificationToIndustryUserByFactoryCircleId(Int64 randomizationId, Int64 factoryCircleId)
        {
            GenericResponseTemplateModel<bool> genericResponseTemplateModel = new GenericResponseTemplateModel<bool>() { ErrorDesc = null, HasError = false, ResponseDataModel = true };
            try
            {
                var inspectionsFactCircleWise = _context.Inspection_Master.Include(x => x.Inspection_Randomization).Where(x => x.RandomizationRefId == randomizationId && x.FactoryCircleId == factoryCircleId).ToList();
                TemplateInspectionIndustryUserNotificationViewModel mailTemplate = new TemplateInspectionIndustryUserNotificationViewModel();
                var circleName = await _iCircleManager.GetCircleDetailsByCircleId(factoryCircleId, CircleTypeEnum.FACTORY_CIRCLE);

                foreach (var inspection in inspectionsFactCircleWise)
                {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=inspection.LicenceNumber.ToString(), isNumber=false}
                    };
                    var establishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionEstablishmentBasicDetailsViewModel>("sp_GetInspectionEstablishmentBasicDetails", storeProcedureParms);


                    // Get officer Details Factory Wing
                    List<StoreProcedureParm> storeProcedureParms1 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="CircleId", ParmValue=inspection.FactoryCircleId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="CircleType", ParmValue= ((int)CircleTypeEnum.FACTORY_CIRCLE).ToString(), isNumber=true}
                    };
                    var officerDetails_Factory = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DesignatedOfficerDetailViewModel>("sp_GetDesignatedOfficerDetailByCircleId", storeProcedureParms1);


                    // Get officer Details Labour Wing
                    List<StoreProcedureParm> storeProcedureParms2 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="CircleId", ParmValue=inspection.LabourCircleRefId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="CircleType", ParmValue=((int)CircleTypeEnum.LABOUR_CIRCLE).ToString(), isNumber=true}
                    };
                    var officerDetails_Labour = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DesignatedOfficerDetailViewModel>("sp_GetDesignatedOfficerDetailByCircleId", storeProcedureParms2);


                    mailTemplate = new TemplateInspectionIndustryUserNotificationViewModel()
                    {
                        EstablishmentName = establishmentDetails.FirstOrDefault().EstablishmentName,
                        EstablishmentAddress = establishmentDetails.FirstOrDefault().EstablishmentAddress,
                        LicenseNumber = establishmentDetails.FirstOrDefault().LicenceNo,
                        OfficerName_Factory = officerDetails_Factory.Where(x => x.RoleName == "ADRF" || x.RoleName == "DDRF").FirstOrDefault().OfficerName,
                        OfficerName_Labour = officerDetails_Labour.Where(x => x.RoleName == "LBIN").FirstOrDefault().OfficerName,
                        Designation_Factory = officerDetails_Factory.Where(x => x.RoleName == "ADRF" || x.RoleName == "DDRF").FirstOrDefault().Designation,
                        Designation_Labour = officerDetails_Labour.Where(x => x.RoleName == "LBIN").FirstOrDefault().Designation,
                        Mobile_Factory = officerDetails_Factory.Where(x => x.RoleName == "ADRF" || x.RoleName == "DDRF").FirstOrDefault().MobileNo,
                        Mobile_Labour = officerDetails_Labour.Where(x => x.RoleName == "LBIN").FirstOrDefault().MobileNo,
                        Circle_Factory = officerDetails_Factory.Where(x => x.RoleName == "ADRF" || x.RoleName == "DDRF").FirstOrDefault().CircleName,
                        Circle_Labour = officerDetails_Labour.Where(x => x.RoleName == "LBIN").FirstOrDefault().CircleName,
                    };

                    var emailTemplateText = await RenderToStringAsync("EmailTemplates/_templateInspectionIndustryUserNotification", mailTemplate);
                    await InitiateNotification(establishmentDetails.FirstOrDefault().ApplicantUserId,
                    new List<NotificationViewModel>()
                    {
                        new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: Inspection (" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(inspection.Inspection_Randomization.Month) + " - " + inspection.Inspection_Randomization.Year.ToString() + ") under factory circle :"+ circleName.ResponseDataModel}
                    });
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.ResponseDataModel = false;
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> InitiateLegacyAppNotification(string sendToUserRefId, string templateId, string smsText, NotificationPurposeTypeEnum notificationPurposeType, string alternateMobileNo)
        {
            GenericResponseTemplateModel<bool> genericResponseTemplateModel = new GenericResponseTemplateModel<bool>() { ErrorDesc = null, HasError = false, ResponseDataModel = true };
            try
            {
                if (sendToUserRefId != "" && sendToUserRefId != null)
                {
                    await InitiateNotification(sendToUserRefId,
                    new List<NotificationViewModel>()
                    {
                        new NotificationViewModel()
                        {
                            Body=smsText,
                            NotificationMode=NotificationModeTypeEnum.MOBILE,
                            NotificationPurpose=notificationPurposeType,
                            Title=templateId
                        }
                    });
                }


                if (alternateMobileNo != "" && alternateMobileNo != null)
                {
                    Notification notificationInfo = new Notification()
                    {
                        Body = smsText,
                        CreatedOn = DateTime.Now,
                        ExpireInMinutes = 10,
                        IsExpirable = (notificationPurposeType == NotificationPurposeTypeEnum.OTP),
                        NotificationMode = NotificationModeTypeEnum.MOBILE,
                        NotificationPurpose = notificationPurposeType,
                        NotificationStatus = NotificationStatusTypeEnum.NEW,
                        ResponseOn = DateTime.Now,
                        ResponseText = "",
                        SendToUserRefId = "NOUSER~" + alternateMobileNo,
                        Title = templateId
                    };
                    _iGR_Notification.Insert(notificationInfo);
                    await _iGR_Notification.SavechangeAsync();
                }
                 
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.ResponseDataModel = false;
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericResponseTemplateModel;
        }

        public async Task<string> ValidateMobileOTP(string mobile, string enteredOTP, string userName, string emailVerificationType, string loginResponseId)
        {
            OTPVerifyRespViewModel otpVerifyResp = new OTPVerifyRespViewModel()
            {
                EnteredOtp = enteredOTP,
                IsOtpMatched = true,
                Mobile = mobile,
                TimeStemp = DateTime.Now.Ticks
            };

            var userId = await _context.Users.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefaultAsync();
            var twoWayAuth = await _context.User2FactorVerifications.Where(x => x.ResponseId == loginResponseId && x.User2FactorVerificationType == User2FactorVerificationTypeEnum.PENDING && x.UserRefId == userId).FirstOrDefaultAsync();



            var otpMatchResp = PassKey_Reader_Engine.PKGE.VerifyPassKey(mobile, enteredOTP, _iConfiguration.GetConnectionString("SQLServerConnection"));
            if (!otpMatchResp.IsMatched || twoWayAuth == null)
            {
                otpVerifyResp.IsOtpMatched = false;
            }
            else
            {
                twoWayAuth.User2FactorVerificationType = User2FactorVerificationTypeEnum.OTP_VERIFIED;
                _context.Update<User2FactorVerification>(twoWayAuth);
                await _context.SaveChangesAsync();

                if (emailVerificationType == "RST_PSWD")
                {
                    NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
                    queryString.Add("UserName", userName);
                    queryString.Add("RequestToken", Guid.NewGuid().ToString());
                    queryString.Add("PageMode", "RESETPASSWORD");
                    queryString.Add("ScreenType", "NEWPASSWORD");
                    queryString.Add("TimeStemp", DateTime.Now.Ticks.ToString());
                    var encryptedEmailParms = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(queryString.ToString(),
                    _iConfiguration.GetSection("EncryptionConfigs").GetSection("LoginResponseEncryptionKey").Value,
                    _iConfiguration.GetSection("EncryptionConfigs").GetSection("LoginResponseEncryptionIVKey").Value);



                    //ResetPasswordEmailVarificationNotificationViewModel model = new ResetPasswordEmailVarificationNotificationViewModel()
                    //{
                    //    UserFullName = "Bhupinder Singh",
                    //    ValidUpto = "01 January 2026 10:40 PM",
                    //    UserName = "bhupindersingh",
                    //    VerificationLink = "http://localhost:4200?data=" + encryptedEmailParms
                    //};
                    //var emailTemplateText = await RenderToStringAsync("EmailTemplates/_templateResetPasswordVerificationNotification", model);
                    //List<NotificationViewModel> notifications = new List<NotificationViewModel>()
                    //{
                    //   new NotificationViewModel ()
                    //   {

                    //       Body= emailTemplateText,
                    //       Title="PB LABOUR - Reset password verification",
                    //       NotificationMode=NotificationModeTypeEnum.EMAIL,
                    //       NotificationPurpose=NotificationPurposeTypeEnum.INFO }
                    //};
                    //await InitiateNotification("bd1394bb-f1cc-45f1-a640-dc8975977579", notifications);
                }
            }

            return RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(JsonConvert.SerializeObject(otpVerifyResp),
                    _iConfiguration.GetSection("EncryptionConfigs").GetSection("LoginResponseEncryptionKey").Value,
                    _iConfiguration.GetSection("EncryptionConfigs").GetSection("LoginResponseEncryptionIVKey").Value);
        }


        //public async Task<GenericResponseTemplateModel<string>> SentLegacyNotificationByPhoneNo(string phoneNumber, string templateId, string smsText, string userRefId)
        //{
        //    var genericResponseTemplateModel = new GenericResponseTemplateModel<string>
        //    {
        //        ErrorDesc = null,
        //        HasError = false,
        //        ResponseDataModel = null
        //    };
        //    try
        //    {
        //        await InitiateNotification(userRefId, new List<NotificationViewModel>()
        //        {
        //            new NotificationViewModel(){Body=smsText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        //// Log the error to a file
        //        //string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");
        //        //string errorMessage = $"Date: {DateTime.Now}\nError: {ex.Message}\nStackTrace: {JsonConvert.SerializeObject(ex)}\n\n";
        //        //try
        //        //{
        //        //    File.AppendAllText(logFilePath, errorMessage);
        //        //}
        //        //catch (IOException ioEx)
        //        //{
        //        //    Console.WriteLine("Error writing to log file: " + ioEx.Message);
        //        //}
        //        genericResponseTemplateModel.ResponseDataModel = ex.Message;
        //        genericResponseTemplateModel.HasError = true;
        //        throw ex;
        //    }
        //    return genericResponseTemplateModel;
        //}

    }
}