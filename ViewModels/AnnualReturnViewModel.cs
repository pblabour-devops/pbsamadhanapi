using System.Collections.Generic;
using System;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class InitiateAnnualReturnViewModel
    {
        public string SubmittedBy_UserRefId { get; set; }
        public List<string> StepCodes { get; set; }
        public Int64 SubmittedBy_ProfileRefId { get; set; }
        public long AppId { get; set; }
        public string ReturnYear { get; set; }
        public string ActIds { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public string LicenceNumber { get; set; }

    }

    public class SaveStepsReturnViewModel
    {
        public string StepCodes { get; set; }
        public string SubmittedBy_UserRefId { get; set; }
        public long AppId { get; set; }
        public string ReturnYear { get; set; }
        public string JsonData { get; set; }
        public bool IsLocked { get; set; }
        public string ActIds { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public string LicenceNumber { get; set; }

    }

    public class AnnualReturnDashboardViewModel
    {
        public long AppId { get; set; }
        public string ReturnYear { get; set; }
        public string ActIds { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string AcknowledgementNo { get; set; }
        public bool IsLocked { get; set; }
        public string LicenceNumber { get; set; }
    }

    public class EstablishmentDataViewModel
    {

        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string ContactNumber { get; set; }

    }

}
