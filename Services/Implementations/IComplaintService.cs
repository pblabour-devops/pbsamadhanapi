using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IComplaintService
    {
        Task<GenericFormModel<object>> Get_ComplaintsCategories();
        Task<GenericFormModel<List<ComplaintsCategory>>> Get_SelfComplaints();
        Task<GenericFormModel<WorkerDetail>> GetWorkerDetails(long id, long projectSiteId);
        Task<GenericServiceResultTemplate> CreateAppComplaintTypeMapping(AppComplaintTypeMapping requestData);
        Task<GenericFormModel<List<Complaint_EmployerORContractorDetail>>> Get_EmployerOrContractorDetails(long id);
        Task<GenericFormModel<Complaint_WorkplaceDetail>> Get_WorkPlaceDetail(long id);
        Task<GenericFormModel<Complaint_EstablishmentDetail>> Get_EstablishmentDetails(long id);
        Task<GenericFormModel<Complaint_GratuityClaim>> Get_GratuityClaimDetails(long id);
        Task<GenericFormModel<Complaint_MaternityBenefitComplaint>> Get_MaternityBenefitsComplaintDetails(long id);
        Task<GenericFormModel<Complaint_Claim_CodeOnWage>> Get_ClaimUnderCodeOnWagesDetails(long id);
        Task<GenericFormModel<Complaint_MinimumWage>> Get_MinimumWagesNotPaidDetails(long id);
        Task<GenericFormModel<List<Complaint_MinimumWagesPeriodAmt>>> Get_MinimumWagesNotPaidPeriodAmountDetails(long id);
        Task<GenericFormModel<Complaint_Wages_WkDay>> Get_WagesNotPaidWeekDayDetail(long id);
        Task<GenericFormModel<List<Complaint_Wages_WkDay_PeriodAmt>>> Get_WagesNotPaidWeekDayPeriodAmountDetails(long id);
        Task<GenericFormModel<Complaint_Wages_OT>> Get_WagesWorkingOvertimeDetail(long id);
        Task<GenericFormModel<List<Complaint_Wages_OT_PeriodAmt>>> Get_WagesWorkingOvertimePerAmtDetail(long id);
        Task<GenericFormModel<Complaint_Wages_Not_Paid>> Get_WagesNotPaidDetail(long id);
        Task<GenericFormModel<List<Complaint_Wages_Not_Paid_PeriodAmt>>> Get_WagesNotPaidPerAmtDetail(long id);
        Task<GenericFormModel<Complaint_Wages_Unauth_Deduct>> Get_UnauthDeductWagesDetail(long id);
        Task<GenericFormModel<List<Complaint_Wages_Unauth_Deduct_PeriodAmt>>> Get_UnauthDeductWagesPerAmtDetail(long id);
        Task<GenericFormModel<Complaint_Non_Pay_Bonus>> Get_NonPayBonusDetail(long id);
        Task<GenericFormModel<List<Complaint_Non_Pay_Bonus_PeriodAmt>>> Get_NonPayBonusPerAmtDetail(long id);
        Task<GenericFormModel<ComplaintDetailViewModel>> Get_ComplaintDetail(long id);
        Task<GenericFormModel<Complaint_RecOfMon_GeneralDetail>> GetComplaintRecOfMonGeneralDetail(long appRefId);
        Task<GenericFormModel<List<Complaint_RecOfMon_MoneyDueDetail>>> GetComplaintRecOfMonDueDetail(long appRefId);
        //Task<GenericFormModel<Complaint_RecOfMon_MoneyDueDetail>> GetComplaintRecOfMonMoneyDueDetail(long appRefId);
        Task<GenericFormModel<Complaint_RecOfMon_SettlementDetail>> GetComplaintRecOfMonSettlementDetail(long appRefId);
        Task<GenericFormModel<Complaint_RecOfMon_AwardDetail>> GetComplaintRecOfMonAwardDetail(long appRefId);
        Task<GenericFormModel<Complaint_RecOfMon_NoticePayDetail>> GetComplaintRecOfMonNoticePayDetail(long appRefId);
        Task<GenericFormModel<Complaint_RecOfMon_RetrenchmentCompDetail>> GetComplaintRecOfMonRetrenchmentCompDetail(long appRefId);
        Task<GenericFormModel<Complaint_RecOfMon_LayOffDetail>> GetComplaintRecOfMonLayOffDetail(long appRefId);
        Task<GenericFormModel<List<Complaint_RecOfMon_LayOffCompDetail>>> GetComplaintRecOfMonLayOffCompDetail(long appRefId);
        Task<GenericFormModel<Complaint_Review_OfDismissal>> Get_ReviewofDismissalDetail(long id);
        Task<GenericResponseTemplateModel<List<Application>>> Get_ComplaintsDraftApplication();
        Task<GenericResponseTemplateModel<List<Application>>> Get_AllApplication();
        Task<GenericFormModel<Complaint_Appeal>> GetAppealDetail(long appRefId);
    }
}
