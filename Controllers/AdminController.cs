using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
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
    [CustomFillters.AuthorizeAttribute("LB1N,DEVTEAM,HELPDESK")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _iAdminService;
        public AdminController(IAdminService iAdminService)
        {
            _iAdminService = iAdminService;
        }
        [HttpPost, Route("SearchApplicationByIPin")]
        public async Task<IActionResult> SearchApplicationByIPin([FromBody] StatusManagerRequestParmsViewModel requestData)
        {
            GenericResponseTemplateModel<StatusManagerResponseParmsViewModel> genericServiceResultTemplate = await _iAdminService.SearchApplicationByIPin(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("ShareStaus")]
        public async Task<IActionResult> ShareStaus(Int64 appId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = await _iAdminService.ShareStaus(appId, applicationType, appActionType);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        [HttpGet, Route("unlockedApplication")]
        public async Task<IActionResult> UnlockedApplication(Int64 appId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iAdminService.UnlockedApplication(appId, applicationType, appActionType);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }


        [HttpPost, Route("SearchApplication")]
        public async Task<IActionResult> SearchApplication([FromBody] SearchApplicationParmsViewModel requestData)
        {
            GenericResponseTemplateModel<GetAdminDashboardDetailsViewModel> genericServiceResultTemplate = await _iAdminService.SearchApplication(requestData);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("searchTransactionByAppRefId")]
        public async Task<IActionResult> SearchTransactionByAppRefId([FromQuery] Int64 appRefId)
        {
            GenericResponseTemplateModel<List<AllTransactionsByAppRefIdViewModel>> genericServiceResultTemplate = await _iAdminService.SearchTransactions(appRefId);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("verifyTransactionByAppRefId")]
        public async Task<IActionResult> VerifyTransactionByAppRefId([FromQuery] Int64 appRefId, string uniquePaymentGatewayTransactionId, string paymentTreasuryType)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = await _iAdminService.VerifyTransactionByAppRefId(appRefId, uniquePaymentGatewayTransactionId, paymentTreasuryType);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("updateActionLogs")]
        public async Task<IActionResult> UpdateApplicationActionLogs(Int64 appRefId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = await _iAdminService.UpdateApplicationActionLogs(appRefId);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }


        [Route("registerEmpanelledPerson")]
        [HttpPost]
        public async Task<IActionResult> RegisterEmpanelledPerson([FromBody] UserArchitectAdditionalInfoMapping formModel)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = await _iAdminService.RegisterEmpanelledPerson(formModel);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("getDepartmentOfficialDetailsByRoleName")]
        [HttpGet]
        public async Task<IActionResult> GetDepartmentOfficialDetailsByRoleName([FromQuery] string roleName)
        {
            GenericListModel<DepartmentOfficialDetailsViewModel> genericListModel = await _iAdminService.GetDepartmentOfficialDetailsByRoleName(roleName);
            if (genericListModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericListModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericListModel);
        }

        [Route("getDepartmentOfficialList")]
        [HttpGet]
        public async Task<IActionResult> GetDepartmentOfficialList([FromQuery] string roleName)
        {
            GenericListModel<DepartmentOfficialListViewModel> genericListModel = await _iAdminService.GetDepartmentOfficialList(roleName);
            if (genericListModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericListModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericListModel);
        }

        [Route("officerTransfer")]
        [HttpPost]
        public async Task<IActionResult> OfficerTransfer([FromBody] UpdateOfficerTransferViewModel updateOfficerTransfer)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iAdminService.UpdateOfficerTransfer(updateOfficerTransfer);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        //[Route("resetPasswordbyAdmin")]
        //[HttpPost]
        //public async Task<IActionResult> ResetPasswordbyAdmin([FromBody] ResetPasswordViewModel resetPassword)
        //{
        //    GenericServiceResultTemplate genericServiceResultTemplate = await _iAdminService.ResetPasswordbyAdmin(resetPassword);
        //    if (genericServiceResultTemplate.HasException)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
        //    }

        //    return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        //}

        [Route("updateEmpanelledPersonProfileDetails")]
        [HttpPost]
        public async Task<IActionResult> UpdateEmpanelledPersonProfileDetails([FromBody] UpdateEmpanelledPersonProfileDetailsViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iAdminService.UpdateEmpanelledPersonProfileDetails(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("updateOfficerProfileDetails")]
        [HttpPost]
        public async Task<IActionResult> UpdateOfficerProfileDetails([FromBody] UpdateOfficerProfileDetailsViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iAdminService.UpdateOfficerProfileDetails(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }


        [HttpGet, Route("searchAppPaymentPartByAppRefId")]
        public async Task<IActionResult> SearchAppPaymentPartByAppRefId([FromQuery] Int64 appRefId)
        {
            GenericResponseTemplateModel<List<AppPaymentPart>> genericServiceResultTemplate = await _iAdminService.SearchAppPaymentPartByAppRefId(appRefId);
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [Route("unlockedRTBFeeDetails")]
        [HttpGet]
        public async Task<IActionResult> UnlockedRTBFeeDetails([FromQuery] Int64 appRefId)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iAdminService.UnlockedRTBFeeDetails(appRefId);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

    }
}
