using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class ProjectProfileViewModel
    {
        public Int64 ProjectSiteId { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string PinCode { get; set; }
        public string FactoryCircleName { get; set; }
        public string LabourCircleName { get; set; }
        public string VillageOrTown { get; set; }
        public string PublicAppRefNum { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProjectSiteVersion { get; set; }
        public string ALCCircleName { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public string LicenceNumber { get; set; }

    }

    public class ProjectSitesViewModel
    {
        public ApplicationTypeEnum? ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum? ApplicationPurposeType { get; set; }
        public string ApplicationStatus { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string PinCode { get; set; }
        public string FactoryCircleName { get; set; }
        public string LabourCircleName { get; set; }
        public string VillageOrTown { get; set; }
        public string PublicAppRefNum { get; set; }
        public Int64 DistrictRefId { get; set; }
        public Int64 TehsilRefId { get; set; }
        public string ApplicantName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ApplicantPanNumber { get; set; }
        public string ProjectPurpose { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }

    }

    public class ProjectSiteViewModel
    {
        public Int64 ProjectSiteId { get; set; }
        public int ProjectSiteVersion { get; set; }
    }

    public class ProjectSiteEstablishmentBasicDetailsViewModel
    {
        public Int64 ProjectSiteRefId { get; set; }
        public int ProjectSiteVersion { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonMiddleName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ProjectPurpose { get; set; }
        public Int64 AppRefId { get; set; }
    }
}
