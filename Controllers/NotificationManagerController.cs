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
    public class NotificationManagerController : ControllerBase
    {
        private readonly INotificationManagerService _iNotificationManagerService;
        public NotificationManagerController(INotificationManagerService iNotificationManagerService)
        {
            _iNotificationManagerService = iNotificationManagerService;
        }

        [HttpGet, Route("sendRandomizationNotificationToFactoryByRandomizationId")]
        public async Task<IActionResult> SendRandomizationNotificationToFactoryByRandomizationId(Int64 randomizationId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iNotificationManagerService.SendRandomizationNotificationToFactoryByRandomizationId(randomizationId);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("sendInspectionNotificationToLabourWingByFactoryCircleId")]
        public async Task<IActionResult> SendInspectionNotificationToLabourWingByFactoryCircleId(Int64 randomizationId, Int64 factoryCircleId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iNotificationManagerService.SendInspectionNotificationToLabourWingByFactoryCircleId(randomizationId, factoryCircleId);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("sendInspectionNotificationToIndustryUserByFactoryCircleId")]
        public async Task<IActionResult> SendInspectionNotificationToIndustryUserByFactoryCircleId(Int64 randomizationId, Int64 factoryCircleId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iNotificationManagerService.SendInspectionNotificationToIndustryUserByFactoryCircleId(randomizationId, factoryCircleId);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("initiateLegacyAppNotification")]
        public async Task<IActionResult> InitiateLegacyAppNotification(string sendToUserRefId, string templateId, string smsText, NotificationPurposeTypeEnum notificationPurposeType, string alternateMobileNo)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iNotificationManagerService.InitiateLegacyAppNotification(sendToUserRefId, templateId, smsText, notificationPurposeType, alternateMobileNo);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("validateMobileOTP")]
        public async Task<IActionResult> ValidateMobileOTP(string mobile, string enteredOTP, string userName, string emailVerificationType, string loginResponseId)
        {
            string otpVerifyResp  =  await _iNotificationManagerService.ValidateMobileOTP(mobile, enteredOTP, userName, emailVerificationType, loginResponseId);
            return StatusCode(StatusCodes.Status200OK, new { OtpVerifyResp= otpVerifyResp });
        }

        

        //[HttpGet, Route("sentLegacyNotificationByPhoneNo")]
        //public async Task<IActionResult> SentLegacyNotificationByPhoneNo(string phoneNumber, string templateId, string smsText, string userRefId)
        //{
        //    GenericResponseTemplateModel<string> genericServiceResultTemplate = await _iNotificationManagerService.SentLegacyNotificationByPhoneNo(phoneNumber, templateId, smsText, userRefId);
        //    if (genericServiceResultTemplate.HasError)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        //}
    }
}
