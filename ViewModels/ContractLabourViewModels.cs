using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class ContractLabourViewModels
    {
        public Contractor_GeneralDetail  GeneralDetail { get; set; }
        public Contractor_PrincipalEmployer PrincipalEmployerDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
}
