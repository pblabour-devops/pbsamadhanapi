using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class BuildingPlanHUDViewModel
    {
        public BuildingPlanHUD_GeneralDetail GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }

    public class BuildingPlanHUDPaymentDetailViewModal
    {
        public List<BuildingPlanHUDPaymentDetail> BuildingPlanHUDPaymentDetailList{ get; set; }
        public Int64 AppDocRefId { get; set; }
    }

    public class OfficerDetailsByRoleNameViewModel
    {
        public string OfficerFullName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime RegistrationIssuedOn { get; set; }
        public DateTime RegistrationValidUpto { get; set; }
        public bool IsActive { get; set; }
    }
}
