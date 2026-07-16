using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class EstablishmentDetailViewModel
    {
        public Establishment_GeneralDetail  GeneralDetail { get; set; }
        public Establishment_EmployerDetail EmployerDetail { get; set; }
        public Establishment_ContractorDetail ContractorDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
}
