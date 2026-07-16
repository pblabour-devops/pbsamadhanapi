using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PaymentManagerController : ControllerBase
    {
        private IPaymentManagerService _iPaymentManagerService;
        private readonly IApplicationManagementService<ApplicationAction> _iApplicationMamnagementService;
        private readonly UserManager<User> _userManager;
        private readonly INotificationManagerService _iNotificationManagerService;
        private readonly AppDbContext _context;
        private readonly IAuthService _iAuthService;
        public IConfiguration Configuration { get; }
        public PaymentManagerController(IPaymentManagerService iPaymentManagerService, 
            IApplicationManagementService<ApplicationAction> iapplicationManagementService,
            UserManager<User> userManager,
            IConfiguration configuration,
            INotificationManagerService iNotificationManagerService,
            AppDbContext context,
            IAuthService iAuthService)
        {
            _iPaymentManagerService = iPaymentManagerService;
            _iApplicationMamnagementService = iapplicationManagementService;
            _userManager = userManager;
            Configuration = configuration;
            _iNotificationManagerService = iNotificationManagerService;
            _context = context;
            _iAuthService = iAuthService;
        }

        [HttpGet, Route("fee_calculator")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> FeeCalculator([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType, Int64 entityKeyId)
        {
            GenericFormModel<List<FeeCalculatorInfoParmsViewModel>> genericFormModel = await _iPaymentManagerService.ApplicationFeeCalculator(appRefId, applicationType, entityKeyId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("prepare_app_fee_payment_initiate_terminal_info")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> PrepareAppFeePaymentInitiateTerminalInfo([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType, decimal netFeeCalculated, int paymentPartCounter, int paymentBatchCounter)
        {
            GenericFormModel<AppFeePaymentInitiateTerminalInfoViewModel> genericFormModel = await _iPaymentManagerService.PrepareAppFeePaymentInitiateTerminalInfo(appRefId, applicationType, netFeeCalculated, paymentPartCounter, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("logAppFeeTransaction")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LogAppFeeTransaction([FromBody] AppFeePaymentInitiateTerminalInfoViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iPaymentManagerService.LogAppFeeTransaction(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("sbi_response_terminal")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> SbiResponseTerminal([FromForm] ResponseData_SBI_ViewModel requestData)
        {
            var ttt = await _iPaymentManagerService.HandlePaymentGatewayResponse_SBI(requestData);
            
            
            return Redirect("http://localhost:4200/payments/appfeepayment_completeterminal");
            //var response = Decrypt(requestData.encData);

            //return StatusCode(StatusCodes.Status200OK, null);
        }

        [HttpGet, Route("getAllAppFeeTransactions")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> GetAllAppFeeTransactions([FromQuery] Int64 appRefId)
        {
            GenericFormModel<List<dynamic>> genericFormModel = await _iPaymentManagerService.GetAllAppFeeTransactions(appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("verifyAlreadyMadePayments")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> VerifyAlreadyMadePayments([FromQuery] Int64[] appFeeTransactionIds)
        {
            GenericFormModel<bool> genericFormModel = await _iPaymentManagerService.VerifyAlreadyMadePayments(appFeeTransactionIds);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("logApplicationFeeHeaders")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> LogApplicationFeeHeaders([FromBody] List<FeeCalculatorInfoParmsViewModel> requestData)
        {
            GenericResponseTemplateModel<int> genericServiceResultTemplate = await _iPaymentManagerService.LogApplicationFeeHeaders(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            //else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            //}
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("responseTerminal_IFMS")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResponseTerminal_IFMS([FromForm] ResponseDataViewModel_IFMS responseData)
        {
            GenericFormModel<PaymentGatewayResponseToUiViewModel> genericFormModel = await _iPaymentManagerService.HandlePaymentGatewayResponse(responseData, PaymentGatewayTypeEnum.IFMS);
            string url = "";
            if (genericFormModel.FormModel.TransactionFinalStatus == TransactionFinalStatusTypeEnum.SUCCEED)
            {
                // Sent Email Notification
                var receiver_UserRefId = _context.ApplicationActions.Where(x => x.ApplicationRefId == genericFormModel.FormModel.AppRefId).Select(x => x.Receiver_UserRefId).FirstOrDefault();
                var receiverInfo = await _iAuthService.GetUserProfileByUserId(receiver_UserRefId);
                TemplateNewCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateNewCaseLendedToOfficerNotificationViewModel()
                {
                    OfficerName = receiverInfo.FirstName + " " + receiverInfo.LastName,
                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(genericFormModel.FormModel.ApplicationPurposeType.ToString()),
                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(genericFormModel.FormModel.ApplicationType.ToString()),
                    EstablishmentName = genericFormModel.FormModel.EstablishmentName,
                    PublicApplicationRefNo = genericFormModel.FormModel.PublicAppRefNum
                };
                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateNewCaseLendedToOfficerNotification", mailTemplate);
                //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, receiverInfo.Email, "Department of Labour: New application received (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);
                await _iNotificationManagerService.InitiateNotification(receiver_UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: New application received (" + mailTemplate.PublicApplicationRefNo + ")"}
                });


                url = Configuration.GetSection("PaymentGatewayConfigs").GetSection("PaymentSuccessUiUrl").Value;
                return Redirect(url + Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(genericFormModel.FormModel))));
            }
            else
            {
                url = Configuration.GetSection("PaymentGatewayConfigs").GetSection("PaymentFailedUiUrl").Value;
                return Redirect(url + JsonConvert.SerializeObject(genericFormModel.FormModel));
            }
        }

        [Route("responseTerminal_IFMS_NON_TREASURY")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResponseTerminal_IFMS_NON_TREASURY([FromForm] ResponseDataViewModel_IFMS responseData)
        {
            GenericFormModel<PaymentGatewayResponseToUiViewModel> genericFormModel = await _iPaymentManagerService.HandlePaymentGatewayResponse(responseData, PaymentGatewayTypeEnum.IFMS_NON_TREASURY);
            string url = "";
            if (genericFormModel.FormModel.TransactionFinalStatus == TransactionFinalStatusTypeEnum.SUCCEED)
            {
                // Sent Email Notification
                var receiver_UserRefId = _context.ApplicationActions.Where(x => x.ApplicationRefId == genericFormModel.FormModel.AppRefId).Select(x => x.Receiver_UserRefId).FirstOrDefault();
                var receiverInfo = await _iAuthService.GetUserProfileByUserId(receiver_UserRefId);
                TemplateNewCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateNewCaseLendedToOfficerNotificationViewModel()
                {
                    OfficerName = receiverInfo.FirstName + " " + receiverInfo.LastName,
                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(genericFormModel.FormModel.ApplicationPurposeType.ToString()),
                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(genericFormModel.FormModel.ApplicationType.ToString()),
                    EstablishmentName = genericFormModel.FormModel.EstablishmentName,
                    PublicApplicationRefNo = genericFormModel.FormModel.PublicAppRefNum
                };
                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateNewCaseLendedToOfficerNotification", mailTemplate);
                //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, receiverInfo.Email, "Department of Labour: New application received (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);

                await _iNotificationManagerService.InitiateNotification(receiver_UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: New application received (" + mailTemplate.PublicApplicationRefNo + ")"}
                });

                url = Configuration.GetSection("PaymentGatewayConfigs").GetSection("PaymentSuccessUiUrl").Value;
                return Redirect(url + Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(genericFormModel.FormModel))));
            }
            else
            {
                url = Configuration.GetSection("PaymentGatewayConfigs").GetSection("PaymentFailedUiUrl").Value;
                return Redirect(url + JsonConvert.SerializeObject(genericFormModel.FormModel));
            }
        }

        #region Application Payment Details
        [HttpGet, Route("payment_details")]
        public async Task<IActionResult> PaymentDetails([FromQuery] Int64 appRefId)
        {
            GenericResponseTemplateModel<PaymentDetailViewModel> genericFormModel = await _iPaymentManagerService.GetApplicationPaymentDetails(appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion Application Payment Details

        #region Building Plan HUD Raised Fee
        [HttpGet, Route("buildingplanfee_raised")]
        public async Task<IActionResult> BuildingPlanHUDFeeRaised([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType, Int64 entityKeyId)
        {
            GenericFormModel<List<FeeCalculatorInfoParmsViewModel>> genericFormModel = await _iPaymentManagerService.BuildingPlanHUDFeeRaised(appRefId, applicationType, entityKeyId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("logBuildingPlanHUDFee")]
        [HttpPost]
        public async Task<IActionResult> LogBuildingPlanHUDFee([FromBody] List<FeeCalculatorInfoParmsViewModel> requestData)
        {
            GenericResponseTemplateModel<int> genericServiceResultTemplate = await _iPaymentManagerService.LogApplicationFeeHeaders(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            //else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            //}
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("logApplicationActionDetails")]
        [HttpPost]
        public async Task<IActionResult> LogApplicationActionDetail([FromBody] ApplicationActionViewModel requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            var res = await _iApplicationMamnagementService.RecordApplicationAction(requestData, userClaims.UserId);
            if (res.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, res.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, res);
        }
        #endregion

        [HttpGet, Route("getPaymentParts")]
        public async Task<IActionResult> GetPaymentParts([FromQuery] Int64 appRefId, int paymentBatchCounter)
        {
            GenericResponseTemplateModel<AppPaymentPartsDetailViewModel> genericFormModel = await _iPaymentManagerService.GetPaymentPart(appRefId, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getPaymentEDCAuthorities")]
        public async Task<IActionResult> GetPaymentEDCAuthorities()
        {
            GenericResponseTemplateModel<List<AppPaymentEDCAuthority>> genericFormModel = await _iPaymentManagerService.GetPaymentEDCAuthorities();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GenerateFeeReceipt")]
        public async Task<IActionResult> GenerateFeeReceipt([FromQuery] Int64 investPunjabIPin, Int64 investPunjabAppId, Int64 appRefId)
        {
            GenericResponseTemplateModel<AppFeePaymentReceiptViewModel> genericFormModel = await _iPaymentManagerService.GenerateFeeReceipt(investPunjabIPin, investPunjabAppId,appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #region Application Raise Fee
        [HttpGet, Route("getApplicationRaiseFee")]
        public async Task<IActionResult> GetApplicationRaiseFee([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter)
        {
            GenericFormModel<ApplicationRaiseFeeParmsViewModel> genericFormModel = await _iPaymentManagerService.GetApplicationRaiseFee(appRefId, applicationType, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getRaisedFeeList")]
        public async Task<IActionResult> GetRaisedFeeList([FromQuery] Int64 appRefId, Int64 paymentBatchCounter)
        {
            GenericFormModel<List<Payments_RaisedFee>> genericFormModel = await _iPaymentManagerService.GetRaisedFeeList(appRefId, paymentBatchCounter);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [HttpGet, Route("getAppFeeDetails")]
        public async Task<IActionResult> GetAppFeeDetails([FromQuery] Int64 appRefId, int paymentBatchCounter)
        {
            GenericResponseTemplateModel<List<AppFeeDetail>> genericFormModel = await _iPaymentManagerService.GetAppFeeDetails(appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getEstablishmentWisePaymentDetails")]
        public async Task<IActionResult> GetEstablishmentWisePaymentDetails([FromQuery] EstablishmentWisePaymentDetailsRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<List<EstablishmentWisePaymentDetailsViewModel>> genericFormModel = await _iPaymentManagerService.GetEstablishmentWisePaymentDetails(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("responseTerminal_HDFC")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResponseTerminal_HDFC([FromForm] ResponseDataViewModel_HDFC responseData)
        {
            GenericFormModel<PaymentGatewayResponseToUiViewModel> genericFormModel = await _iPaymentManagerService.HandlePaymentGatewayResponse(responseData, PaymentGatewayTypeEnum.HDFC);
            string url = "";
            if (genericFormModel.FormModel.TransactionFinalStatus == TransactionFinalStatusTypeEnum.SUCCEED)
            {
                url = Configuration.GetSection("PaymentGatewayConfigs").GetSection("PaymentSuccessUiUrl").Value;
                return Redirect(url + Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(genericFormModel.FormModel))));
            }
            else
            {
                url = Configuration.GetSection("PaymentGatewayConfigs").GetSection("PaymentFailedUiUrl").Value;
                return Redirect(url + JsonConvert.SerializeObject(genericFormModel.FormModel));
            }
        }

    }
}
