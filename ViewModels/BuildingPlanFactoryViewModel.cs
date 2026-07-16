using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class BuildingPlanFactoryViewModel
    {
        public BuildingPlanFactory_GeneralDetail GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
    public class BuildingPlanFactoryPaymentDetailViewModal
    {
        public List<BuildingPlanHUDPaymentDetail> BuildingPlanHUDPaymentDetailList { get; set; }
        public Int64 AppDocRefId { get; set; }
    }

    public class BuildingPlanFactory_Declaration_Stability_CertificateViewModel
    {
        public BuildingPlanFactory_Declaration_Stability_Certificate GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
}
