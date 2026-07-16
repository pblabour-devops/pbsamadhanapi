using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class CommonLicenceViewModels
    {
        public CommonLicence_GeneralDetail  GeneralDetail { get; set; }
        public CommonLicence_ContractorDetail ContractorDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
}
