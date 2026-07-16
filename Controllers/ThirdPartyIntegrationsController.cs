using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThirdPartyIntegrationsController : ControllerBase
    {
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private ISystem_O_CommunicationService _system_O_CommunicationService;
        private readonly UserManager<User> _userManager;
        private readonly IAppTimeLineManagerService _AppTimeLineManagerService;
        private IAuthService _iAuthService;
        public ThirdPartyIntegrationsController(IThirdPartyInegrationsService iThirdPartyInegrationsService, UserManager<User> userManager, ISystem_O_CommunicationService system_O_CommunicationService, IAppTimeLineManagerService appTimeLineManagerService, IAuthService iAuthService)
        {
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _system_O_CommunicationService = system_O_CommunicationService;
            _userManager = userManager;
            _AppTimeLineManagerService = appTimeLineManagerService;
            _iAuthService = iAuthService;
        }

        [HttpGet, Route("ServiceGateway")]
        public async Task<IActionResult> ServiceGateway([FromQuery] string msg)
        {
            ServiceGatewayResponseViewModel serviceGatewayResponse = await _iThirdPartyInegrationsService.ServiceGateway(msg, Request);
            if (serviceGatewayResponse.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ExceptionMessage);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);
        }

        [HttpGet, Route("verifyOldLicence")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> VerifyOldLicence([FromQuery] string licenceSnapshot)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = await _iThirdPartyInegrationsService.VerifyOldLicenceNo(licenceSnapshot);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }


        [HttpGet, Route("getInPrincipalApprovalDetails")]
        public async Task<IActionResult> GetInPrincipalApprovalDetails([FromQuery] Int64 appRefId)
        {
            GenericResponseTemplateModel<BuildingPlanHUD_RTB_Mapping> serviceGatewayResponse = await _iThirdPartyInegrationsService.GetInPrincipalApprovalByAppRefId(appRefId);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);

        }

        [HttpGet, Route("verifyLicenseNumber")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> VerifyLicenseNumber([FromQuery] string value)
        {
            GenericResponseTemplateModel<bool> serviceGatewayResponse = await _iThirdPartyInegrationsService.VerifyLicenseNumber(value);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);

        }

        [HttpGet, Route("getRaisedFeePaymentPageCode")]
        public async Task<IActionResult> GetRaisedFeePaymentPageCode([FromQuery] Int64 appRefId)
        {
            GenericResponseTemplateModel<int> serviceGatewayResponse = await _iThirdPartyInegrationsService.GetRaisedFeePaymentPageCode(appRefId);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);

        }

        [HttpPost, Route("deregisterFactoryByFactoryBacklog")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> DeRegisterFactoryByFactoryBacklog([FromBody] DeRegsiterFactoryRequestViewModel deRegsiterFactoryRequest, string fileName)

        {
            GenericResponseTemplateModel<bool> serviceGatewayResponse = await _system_O_CommunicationService.DeRegisterFactoryByFactoryBacklog(deRegsiterFactoryRequest, fileName);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);

        }

        #region BusinessFirst_ApprovedFileSeeding
        [HttpPost, Route("seed_BusinessFirstApprovedFiles")]
        public async Task<IActionResult> Seed_BusinessFirstApprovedFiles([FromBody] BusinessFirst_ApprovedFileSeeding requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iThirdPartyInegrationsService.Seed_BusinessFirstApprovedFiles(requestData);
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
        #endregion


        [HttpGet, Route("getActTypeByLicenceNumber")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> GetActTypeByLicenceNumber([FromQuery] string licenceNumber)
        {
            GenericResponseTemplateModel<List<GetLicenceNumberAndActTypeViewModel>> genericFormModel = await _iThirdPartyInegrationsService.GetActTypeByLicenceNumber(licenceNumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }



        [HttpGet, Route("redirectToOtherPortel")]
        public async Task<IActionResult> RedirecttoOtherPortel([FromQuery] string role, string type)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericFormModel<GetRedirectUrlViewModel> genericFormModel = await _iThirdPartyInegrationsService.GetRedirectToOtherPortel(userClaims.UserId, role, type);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #region Stabilty Card Info
        [HttpGet, Route("getStabiltyAcknoweldgementReceipt")]
        public async Task<IActionResult> GetStabiltyAcknoweldgementReceipt([FromQuery] string msg)
        {
            GenericResponseTemplateModel<StabiltyAcknoweldgementReceiptViewModel> genericFormModel = await _iThirdPartyInegrationsService.GetStabiltyAcknoweldgementSlip(msg);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [HttpGet, Route("getLatestDirtyApplications")]
        public async Task<IActionResult> GetLatestDirtyApplications()
        {
            GenericResponseTemplateModel<List<IPinInfoViewModel>> genericFormModel = await _iThirdPartyInegrationsService.GetLatestDirtyApplicationsDetails();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpPost, Route("setIsDirtyFlagForcefully")]
        public async Task<IActionResult> SetIsDirtyFlagForcefully([FromBody] List<DirtyFlagRequestParamsViewModel> requests)
        {
            GenericResponseTemplateModel<List<SetIsDirtyFlagForcefullyViewModel>> serviceGatewayResponse = await _iThirdPartyInegrationsService.SetIsDirtyFlagForcefully(requests);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);
        }

        [HttpPost, Route("getBFApplicationsLogs")]
        public async Task<IActionResult> GetBFApplicationsLogs([FromBody] List<DirtyFlagRequestParamsViewModel> requests)
        {
            GenericResponseTemplateModel<List<IPinInfoViewModel>> genericFormModel = await _iThirdPartyInegrationsService.GetBFApplicationsLogDetails(requests);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpPost, Route("getAppSubmissionDateByIpinAppId")]
        public async Task<IActionResult> GetAppSubmissionDateByIpinAppId([FromBody] List<DirtyFlagRequestParamsViewModel> requests)
        {
            GenericResponseTemplateModel<List<AppSubmissionResponseViewModel>> serviceGatewayResponse = await _iThirdPartyInegrationsService.GetAppSubmissionDateByIpinAppId(requests);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);
        }

        #region Data Seeding
        [HttpGet, Route("seedApplicationDataByLegacyAppFormId")]
        public async Task<IActionResult> SeedApplicationDataByLegacyAppFormId([FromQuery] int legacyAppFormId)
        {
            GenericResponseTemplateModel<List<IPinInfoViewModel>> genericFormModel = await _iThirdPartyInegrationsService.SeedApplicationDataByLegacyAppFormId(legacyAppFormId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("seedApplicationDataByLegacyAppIdId")]
        public async Task<IActionResult> SeedApplicationDataByLegacyAppIdId([FromQuery] int legacyAppFormId, Int64 appRefId)
        {
            GenericResponseTemplateModel<List<IPinInfoViewModel>> genericFormModel = await _iThirdPartyInegrationsService.SeedApplicationDataByLegacyAppIdId(legacyAppFormId,appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #endregion

        [HttpPost, Route("hrms_GetHRMSCodeAndActWiseData")]
        public async Task<IActionResult> GetHRMSCodeAndActWiseData([FromBody] HRMSRequestParamsViewModel requestData)
        {
            GenericResponseTemplateModel<dynamic> genericFormModel = await _iThirdPartyInegrationsService.GetHRMSCodeAndActWiseData(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel.ResponseDataModel);
        }

        [HttpPost, Route("hrms_GetHRMSCodeAndActWiseData_FactoryWing")]
        public async Task<IActionResult> GetHRMSCodeAndActWiseData_FactoryWing([FromBody] HRMSRequestParamsViewModel requestData)
        {
            GenericResponseTemplateModel<dynamic> genericFormModel = await _iThirdPartyInegrationsService.GetHRMSCodeAndActWiseData_FactoryWing(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel.ResponseDataModel);
        }

        [HttpPost, Route("hrms_GetHRMSCodeAndActWiseData_ALCWing")]
        public async Task<IActionResult> GetHRMSCodeAndActWiseData_ALCWing([FromBody] HRMSRequestParamsViewModel requestData)
        {
            GenericResponseTemplateModel<dynamic> genericFormModel = await _iThirdPartyInegrationsService.GetHRMSCodeAndActWiseData_ALCWing(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel.ResponseDataModel);
        }

        [HttpPost, Route("hrms_GetHRMSCodeAndActWiseData_LabourWing")]
        public async Task<IActionResult> GetHRMSCodeAndActWiseData_LabourWing([FromBody] HRMSRequestParamsViewModel requestData)
        {
            GenericResponseTemplateModel<dynamic> genericFormModel = await _iThirdPartyInegrationsService.GetHRMSCodeAndActWiseData_LabourWing(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel.ResponseDataModel);
        }

        #region Withdraw Application
        [HttpGet, Route("withdrawApplication")]
        public async Task<IActionResult> WithdrawApplication([FromQuery] string requestData)
        {
            GenericResponseTemplateModel<WithdrawApplicationViewModel> genericFormModel = await _iThirdPartyInegrationsService.WithdrawApplication(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Invest Punjab Integration API's
        [HttpPost, Route("getTotalPendingApplications")]
        public async Task<IActionResult> GetTotalPendingApplicationsByDate()
        {
            GenericResponseTemplateModel<List<PendingApplicationDetailsViewModel>> genericFormModel = await _iThirdPartyInegrationsService.GetTotalPendingApplicationsByDate();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpPost, Route("getTotalPendingApplicationsByServiceCode")]
        public async Task<IActionResult> GetTotalPendingApplicationsByServiceCode([FromBody] PendingApplicationsByServiceCodeParamsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>> genericFormModel = await _iThirdPartyInegrationsService.GetTotalPendingApplicationsByServiceCode(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpPost, Route("getApplicationCurrentStatusByIpinAndAppId")]
        public async Task<IActionResult> GetApplicationCurrentStatusByIpinAndAppId([FromBody] ApplicationCurrentStatusParamsViewModel requestData)
        {
            GenericResponseTemplateModel<IpinAndApplicationIdInfoViewModel> genericFormModel = await _iThirdPartyInegrationsService.GetApplicationCurrentStatusByIpinAndAppId(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpPost, Route("getUpdatedApplicationsBetweenDates")]
        public async Task<IActionResult> GetUpdatedApplicationsBetweenDates([FromBody] UpdatedApplicationsParamsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>> genericFormModel = await _iThirdPartyInegrationsService.GetUpdatedApplicationsBetweenDates(requestData);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_ReportByDept")]
        public async Task<IActionResult> Get_ReportByDepartment()
        {
            var result = await _iThirdPartyInegrationsService.GetReportByDepartment();
            if (result.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorDesc);
            }
            return Ok(result.ResponseDataModel);
        }

        [HttpGet, Route("get_ReportByAuthority")]
        public async Task<IActionResult> Get_ReportByAuthority([FromQuery] GetReportByAuthorityViewModel requestData)
        {
            var result = await _iThirdPartyInegrationsService.GetReportByAuthority(requestData);
            if (result.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorDesc);
            }
            return Ok(result.ResponseDataModel);
        }

        [HttpGet, Route("get_ReportByService")]
        public async Task<IActionResult> Get_ReportByService([FromQuery] GetReportByServiceViewModel requestData)
        {
            var result = await _iThirdPartyInegrationsService.GetReportByService(requestData);
            if (result.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorDesc);
            }
            return Ok(result.ResponseDataModel);
        }
        #endregion



        #region Seed TimeLine

        [HttpGet, Route("seedTimelineWiseAction")]
        public async Task<IActionResult> SeedTimeLineWiseAction([FromQuery] Int64 appRefId)
        {
            var result = await _AppTimeLineManagerService.SeedTimeLineData(appRefId,0);
            return Ok(result.HasError);
        }

        #endregion

        [HttpGet, Route("downloadApproval")]
        public async Task<IActionResult> DownloadApproval([FromQuery] string msg)
        {
            GenericResponseTemplateModel<DownloadApprovalViewModel> genericFormModel = await _iThirdPartyInegrationsService.DownloadApproval(msg);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpGet, Route("validateLoginFromPartnerPortal")]
        public async Task<IActionResult> ValidateLoginFromPartnerPortal([FromQuery] string msg)
        {
            string loginResp = await _iThirdPartyInegrationsService.ValidateLoginFromPartnerPortal(msg, Request);
            return StatusCode(StatusCodes.Status200OK, new { EncryptedResp = loginResp });
        }

        [HttpGet]
        [Route("getBOCWTransparencyData")]
        [CustomFillters.AuthorizeAttribute("TRN_RPT")]
        public async Task<IActionResult> GetBOCWTransparencyData([FromQuery] TransperancyReportViewModel requestData)
        {
            GenericResponseTemplateModel<List<BOCWTransparencyViewModel>> genericFormModel = await _iThirdPartyInegrationsService.GetBOCWTransparencyData(requestData);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        
        [HttpGet, Route("prepareTokenForLWB")]
        public async Task<IActionResult> PrepareTokenForLWB([FromQuery] string userId, string licenceNo, string cessId)
        {
            GenericResponseTemplateModel<string> resp = await _iAuthService.PrepareTokenForLWB(userId, licenceNo, cessId);
            if (resp.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }
            return StatusCode(StatusCodes.Status200OK, resp);
        }
    }
}
