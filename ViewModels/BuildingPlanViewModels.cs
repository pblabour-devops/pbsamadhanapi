using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class BuildingPlanViewModels
    {
        public BuildingPlan BuildingPlan { get; set; }
        public BuildingPlan_AreaDetail BuildingPlan_AreaDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }

    public class StabiltyAcknoweldgementReceiptViewModel
    {
        public string FileNumber { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public int IsStabilityApproved { get; set; }
        public string VillageOrTown { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string PinCode { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public string ApplicantName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string ContactPersonEmail { get; set; }
        public string CompetentPersonName { get; set; }
        public string CompetentPersonEmail { get; set; }
        public string CompetentPersonContactNo { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string EngineerName { get; set; }
        public string EngineerContactNo { get; set; }
        public string EngineerEmail { get; set; }
    }
}
