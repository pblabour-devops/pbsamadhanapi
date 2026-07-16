using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class Licence_BuildingPlan_PSIECViewModel
    {
        public Licence_BuildingPlan_PSIEC_GeneralDetail GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }

    public class PSIECUserDetailsViewModel
    {
        public List<PSIECUserViewModel> SdoDetails { get; set; }
        public List<PSIECUserViewModel> EODetails { get; set; }
    }

    public class PSIECUserViewModel
    {
        public string UserRefId { get; set; }
        public string UserName { get; set; }
        public string Designation { get; set; }
        public string OfficerName { get; set; }
    }

}
