using pbsamadhannetcoreapi.Models;
using System.Collections.Generic;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class ComplaintDetailViewModel
    {
        public WorkerDetail WorkerDetail { get; set; }

        public ICollection<Complaint_EmployerORContractorDetail> Complaint_EmployerORContractorDetails { get; set; }

        public Complaint_WorkplaceDetail Complaint_WorkplaceDetails { get; set; }

        public Complaint_EstablishmentDetail Complaint_EstablishmentDetails { get; set; }

        public Complaint_GratuityClaim Complaint_GratuityClaims { get; set; }

        public Complaint_Claim_CodeOnWage Complaint_Claim_CodeOnWages { get; set; }

        public Complaint_MinimumWage Complaint_MinimumWage { get; set; }

        public ICollection<Complaint_MinimumWagesPeriodAmt> Complaint_MinimumWagesPeriodAmts { get; set; }

        public Complaint_Wages_WkDay Complaint_Wages_WkDay { get; set; }

        public ICollection<Complaint_Wages_WkDay_PeriodAmt> Complaint_Wages_WkDay_PeriodAmts { get; set; }
        public Complaint_Wages_OT Complaint_Wages_OT { get; set; }

        public ICollection<Complaint_Wages_OT_PeriodAmt> Complaint_Wages_OT_PeriodAmts { get; set; }

        public Complaint_Wages_Not_Paid Complaint_Wages_Not_Paid { get; set; }

        public ICollection<Complaint_Wages_Not_Paid_PeriodAmt> Complaint_Wages_Not_Paid_PeriodAmts { get; set; }

        public Complaint_Wages_Unauth_Deduct Complaint_Wages_Unauth_Deduct { get; set; }

        public ICollection<Complaint_Wages_Unauth_Deduct_PeriodAmt> Complaint_Wages_Unauth_Deduct_PeriodAmts { get; set; }

        public Complaint_Non_Pay_Bonus Complaint_Non_Pay_Bonus { get; set; }

        public ICollection<Complaint_Non_Pay_Bonus_PeriodAmt> Complaint_Non_Pay_Bonus_PeriodAmts { get; set; }

        public Complaint_MaternityBenefitComplaint Complaint_MaternityBenefitComplaints { get; set; }
    }
}
