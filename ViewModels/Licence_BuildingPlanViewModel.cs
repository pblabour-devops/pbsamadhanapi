using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class ProposedBuildingPlanViewModel
    {
        public Licence_Proposed_BuildingPlan_GeneralDetail GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }

    public class ExistingBuildingPlanViewModel
    {
        public Licence_Existing_BuildingPlan_GeneralDetail GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }

    public class Addition_AmendmentBuildingPlanViewModel
    {
        public Licence_Addition_Amendment_BuildingPlan_GeneralDetail GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
}
