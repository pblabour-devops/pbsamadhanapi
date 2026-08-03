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
        public async Task<IActionResult> Get_EmployerOrContractorDetails(long id)
        {
            GenericFormModel<List<Complaint_EmployerORContractorDetail>> genericFormModel =
                await _iComplaintService.Get_EmployerOrContractorDetails(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Get WorkPlace
        [HttpGet, Route("getComplaintWorkplaceDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_WorkPlaceDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_WorkplaceDetail> genericFormModel = await _iComplaintService.Get_WorkPlaceDetail(id);
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
            GenericFormModel<List<Complaint_MinimumWagesPeriodAmt>> genericFormModel = await _iComplaintService.Get_MinimumWagesNotPaidPeriodAmountDetails(id);
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
            GenericFormModel<List<Complaint_Wages_WkDay_PeriodAmt>> genericFormModel = await _iComplaintService.Get_WagesNotPaidWeekDayPeriodAmountDetails(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region  Claim wages not paid for working overtime
        [HttpGet, Route("getWagesWorkingOvertimeDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_WagesWorkingOvertimeDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_Wages_OT> genericFormModel = await _iComplaintService.Get_WagesWorkingOvertimeDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getWagesWorkingOvertimePerAmtDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_WagesWorkingOvertimePerAmtDetail([FromQuery] Int64 id)
        {
            GenericFormModel<List<Complaint_Wages_OT_PeriodAmt>> genericFormModel = await _iComplaintService.Get_WagesWorkingOvertimePerAmtDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region  Claim wages not paid for all
        [HttpGet, Route("getWagesNotPaidDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_WagesNotPaidDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_Wages_Not_Paid> genericFormModel = await _iComplaintService.Get_WagesNotPaidDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getWagesNotPaidPerAmtDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_WagesNotPaidPerAmtDetail([FromQuery] Int64 id)
        {
            GenericFormModel<List<Complaint_Wages_Not_Paid_PeriodAmt>> genericFormModel = await _iComplaintService.Get_WagesNotPaidPerAmtDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region  Claim wages unauthorised deduction
        [HttpGet, Route("getUnauthDeductWagesDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_UnauthDeductWagesDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_Wages_Unauth_Deduct> genericFormModel = await _iComplaintService.Get_UnauthDeductWagesDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getUnauthDeductWagesPerAmtDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_UnauthDeductWagesPerAmtDetail([FromQuery] Int64 id)
        {
            GenericFormModel<List<Complaint_Wages_Unauth_Deduct_PeriodAmt>> genericFormModel = await _iComplaintService.Get_UnauthDeductWagesPerAmtDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region  Non Payment of Bonus
        [HttpGet, Route("getNonPayBonusDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_NonPayBonusDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_Non_Pay_Bonus> genericFormModel = await _iComplaintService.Get_NonPayBonusDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getNonPayBonusPerAmtDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_NonPayBonusPerAmtDetail([FromQuery] Int64 id)
        {
            GenericFormModel<List<Complaint_Non_Pay_Bonus_PeriodAmt>> genericFormModel = await _iComplaintService.Get_NonPayBonusPerAmtDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Recovery of code

        [HttpGet, Route("getComplaintRecOfMonGeneralDetail")]
        public async Task<IActionResult> GetComplaintRecOfMonGeneralDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_RecOfMon_GeneralDetail> genericFormModel = await _iComplaintService.GetComplaintRecOfMonGeneralDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getComplaintRecOfMonMoneyDueDetail")]
        public async Task<IActionResult> GetComplaintRecOfMonMoneyDueDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_RecOfMon_MoneyDueDetail> genericFormModel = await _iComplaintService.GetComplaintRecOfMonMoneyDueDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getComplaintRecOfMonSettlementDetail")]
        public async Task<IActionResult> GetComplaintRecOfMonSettlementDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_RecOfMon_SettlementDetail> genericFormModel = await _iComplaintService.GetComplaintRecOfMonSettlementDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getComplaintRecOfMonAwardDetail")]
        public async Task<IActionResult> GetComplaintRecOfMonAwardDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_RecOfMon_AwardDetail> genericFormModel = await _iComplaintService.GetComplaintRecOfMonAwardDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getComplaintRecOfMonNoticePayDetail")]
        public async Task<IActionResult> GetComplaintRecOfMonNoticePayDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_RecOfMon_NoticePayDetail> genericFormModel = await _iComplaintService.GetComplaintRecOfMonNoticePayDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getComplaintRecOfMonRetrenchmentCompDetail")]
        public async Task<IActionResult> GetComplaintRecOfMonRetrenchmentCompDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_RecOfMon_RetrenchmentCompDetail> genericFormModel = await _iComplaintService.GetComplaintRecOfMonRetrenchmentCompDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getComplaintRecOfMonLayOffDetail")]
        public async Task<IActionResult> GetComplaintRecOfMonLayOffDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Complaint_RecOfMon_LayOffDetail> genericFormModel = await _iComplaintService.GetComplaintRecOfMonLayOffDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getComplaintRecOfMonLayOffCompDetail")]
        public async Task<IActionResult> GetComplaintRecOfMonLayOffCompDetail([FromQuery] Int64 id)
        {
            GenericFormModel<List<Complaint_RecOfMon_LayOffCompDetail>> genericFormModel = await _iComplaintService.GetComplaintRecOfMonLayOffCompDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region GET DETAILS COMPLAINTS
        [HttpGet, Route("getComplaintDetail")]
        public async Task<IActionResult> Get_ComplaintDetail([FromQuery] long id)
        {
            GenericFormModel<ComplaintDetailViewModel> genericFormModel = await _iComplaintService.Get_ComplaintDetail(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Review of dismisaal
        [HttpGet, Route("getReviewofDismissalDetail")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_ReviewofDismissalDetail([FromQuery] long id)
        {
            GenericFormModel<Complaint_Review_OfDismissal> genericFormModel = await _iComplaintService.Get_ReviewofDismissalDetail(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Get Draft application
        [HttpGet, Route("getComplaintsDraftApplication")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_ComplaintsDraftApplication()
        {
            GenericResponseTemplateModel<List<Application>> genericFormModel = await _iComplaintService.Get_ComplaintsDraftApplication();

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Lock Application
        [HttpGet, Route("lockComplaintsApplication")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> LockComplaintsApplication([FromQuery] long id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iComplaintService.LockComplaintsApplication(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Get All application
        [HttpGet, Route("getAllApplications")]
        //[CustomFillters.AuthorizeAttribute("worker_INDL")]
        public async Task<IActionResult> Get_AllApplication()
        {
            GenericResponseTemplateModel<List<Application>> genericFormModel = await _iComplaintService.Get_AllApplication();

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }

            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

    }
}
