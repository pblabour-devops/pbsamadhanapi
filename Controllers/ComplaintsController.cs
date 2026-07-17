using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintService _iComplaintService;

        public ComplaintsController(IComplaintService complaintService)
        {
            _iComplaintService = complaintService;
        }


        [HttpGet, Route("getComplaintsCategories")]
        //[CustomFillters.AuthorizeAttribute("Worker_INDL")]
        public async Task<IActionResult> Get_ComplaintsCategories()
        {
            GenericFormModel<object> genericFormModel = await _iComplaintService.Get_ComplaintsCategories();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #region Worker details
        [HttpGet, Route("getWorkerDetails")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<WorkerDetail> genericFormModel = await _iComplaintService.GetWorkerDetails(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region App Complaint Mapping

        [Route("createAppComplaintTypeMapping")]
        [HttpPost]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> CreateAppComplaintTypeMapping([FromBody] AppComplaintTypeMapping requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iComplaintService.CreateAppComplaintTypeMapping(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }

            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion

        #region Get Employer OR Contractor Details
        [HttpGet, Route("getEmployerOrContractorDetails")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_EmployerOrContractorDetails([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_EmployerORContractorDetail> genericFormModel = await _iComplaintService.Get_EmployerOrContractorDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Get Establishment Details
        [HttpGet, Route("getEstablishmentDetails")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_EstablishmentDetails([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_EstablishmentDetail> genericFormModel = await _iComplaintService.Get_EstablishmentDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Get Gratuity details

        [HttpGet, Route("getGratuityClaimDetails")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_GratuityClaimDetails([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_GratuityClaim> genericFormModel = await _iComplaintService.Get_GratuityClaimDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #endregion

        #region Get Maternity Benefits Complaint detail
        [HttpGet, Route("getMaternityBenefitsComplaintDetails")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_MaternityBenefitsComplaintDetails([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_MaternityBenefitComplaint> genericFormModel = await _iComplaintService.Get_MaternityBenefitsComplaintDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        //#region Get Recovery of Money
        //[HttpGet, Route("getComplaintRecoveryOfMoneyUnderIRCode")]
        ////[CustomFillters.AuthorizeAttribute("worker_INDL")]
        //public async Task<IActionResult> Get_GetComplaintRecoveryOfMoneyUnderIRCodeDetails([FromQuery] Int64 id)
        //{
        //    GenericFormModel<Complaint_RecoveryOfMoneyUnderIRCode> genericFormModel = await _iComplaintService.Get_GetComplaintRecoveryOfMoneyUnderIRCodeDetails(id);
        //    if (genericFormModel.HasError)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, genericFormModel);
        //}
        //#endregion

        #region Claim under code on wages
        [HttpGet, Route("getClaimUnderCodeOnWagesDetails")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_ClaimUnderCodeOnWagesDetails([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_Claim_CodeOnWage> genericFormModel = await _iComplaintService.Get_ClaimUnderCodeOnWagesDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Minimum wages not paid
        [HttpGet, Route("getMinimumWagesNotPaidDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_MinimumWagesNotPaidDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_MinimumWage> genericFormModel = await _iComplaintService.Get_MinimumWagesNotPaidDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Minimum wages not paid period amount 
        [HttpGet, Route("getMinimumWagesNotPaidPeriodAmountDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_MinimumWagesNotPaidPeriodAmountDetails([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_MinimumWagesPeriodAmt> genericFormModel = await _iComplaintService.Get_MinimumWagesNotPaidPeriodAmountDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region  Claim wages not paid for on weekly day of rest
        [HttpGet, Route("getWagesNotPaidWeekDayDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_WagesNotPaidWeekDayDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_Wages_WkDay> genericFormModel = await _iComplaintService.Get_WagesNotPaidWeekDayDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getWagesNotPaidWeekDayPeriodAmountDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_WagesNotPaidWeekDayPeriodAmountDetails([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_Wages_WkDay_PeriodAmt> genericFormModel = await _iComplaintService.Get_WagesNotPaidWeekDayPeriodAmountDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion
    }
}
