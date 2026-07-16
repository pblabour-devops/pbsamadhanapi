using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface INotificationManagerService
    {
        Task<bool> InitiateNotification(string sendToUserRefId, List<NotificationViewModel> notifications);
        Task<bool> DraftLoginAuthenticationNotification(string sendToUserRefId, List<NotificationViewModel> notifications, string tokenString);
        Task<bool> SendSmsViaBOCWBoard(string mobileNo, string otpText);
        Task<GenericResponseTemplateModel<bool>> SendEmailSMTP(string receiverName, string receiverEmail, string subjectEmail, string bodyEmailHtml);
        Task<string> RenderToStringAsync(string viewName, object model);
        Task<GenericResponseTemplateModel<bool>> SendRandomizationNotificationToFactoryByRandomizationId(Int64 randomizationId);
        Task<GenericResponseTemplateModel<bool>> SendInspectionNotificationToLabourWingByFactoryCircleId(Int64 randomizationId, Int64 factoryCircleId);
        Task<GenericResponseTemplateModel<bool>> SendInspectionNotificationToIndustryUserByFactoryCircleId(Int64 randomizationId, Int64 factoryCircleId);
        Task<GenericResponseTemplateModel<bool>> InitiateLegacyAppNotification(string sendToUserRefId, string templateId, string smsText, NotificationPurposeTypeEnum notificationPurposeType, string alternateMobileNo);
        //Task<GenericResponseTemplateModel<string>> SentLegacyNotificationByPhoneNo(string phoneNumber, string templateId, string smsText, string userRefId);
        Task<string> ValidateMobileOTP(string mobile, string enteredOTP, string userName, string emailVerificationType, string loginResponseId);
    }
}
