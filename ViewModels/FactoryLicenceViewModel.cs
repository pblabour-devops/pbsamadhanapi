using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class FactoryLicenceViewModel
    {
        public Licence_Factory_GeneralDetail GeneralDetail { get; set; }
        public List<Licence_Factory_AmendmentDataHistory> Licence_Factory_AmendmentDataHistory { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }

    public class GetAllClearanceDateSlabsViewModel
    {
        public Int64 AppId { get; set; }
        public DateTime ClearanceIssuedOn { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public int LegacyApplication { get; set; }
    }

    public class FactoryHasTempRegistrationFlagViewModel
    {
        public int IsTempRegistered { get; set; }
        public bool isTempRegistrationVerified { get; set; }
    }

    public class AnnualReturnWelfareFundViewModel
    {
        public string LicenceNumber { get; set; }
        public Int64 LegacyAppId { get; set; }
        public string AnnualReturnYear { get; set; }
        public int IsAnnualReturnSubmitted { get; set; }
        public string WelfareFundYear { get; set; }
        public int IsWelfareFundSubmitted { get; set; }
    }
}
