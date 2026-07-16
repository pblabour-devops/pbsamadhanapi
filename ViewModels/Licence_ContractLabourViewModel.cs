using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class Licence_ContractLabourViewModel
    {
        public Licence_ContractLabour_GeneralDetail GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
    public class ContractLabourLicenceValidityViewModel
    {
        public string LicenceNumber { get; set; }
        public Int64 LicenceForYear { get; set; }
    }
}
