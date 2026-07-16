using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class OSH_Form_1_RegistrationViewModel
    {
        public OSH_Form_1_Registration RegistrationDetail { get; set; }
        public OSH_Form_1_Registration_EmployeeDetail EmployeeDetail { get; set; }
        public OSH_Form_1_Registration_Factory FactoryDetail { get; set; }
        public OSH_Form_1_Registration_BOCW BOCWDetail { get; set; }
        public OSH_Form_1_Registration_MotorTransportDetail MotorTransportDetail { get; set; }
        public OSH_Form_1_Registration_EPFO_ESIC_Detail EPFOEsiDetail { get; set; }
        public OSH_Form_1_Registration_EmployerDetail EmployerDetail { get; set; }
        public OSH_Form_1_Registration_PrincipalEmployerDetail PrincipalEmployerDetail { get; set; }
        public OSH_Form_1_Registration_ContractorDetail ContractorsDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
}
