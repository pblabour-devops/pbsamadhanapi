using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class TemplateNewCaseLendedToOfficerNotificationViewModel
    {
        public string OfficerName { get; set; }
        public string PublicApplicationRefNo { get; set; }
        public string EstablishmentName { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public string ApplicationPurposeTitle { get; set; }
    }

    public class TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel
    {
        public string OfficerName { get; set; }
        public string PublicApplicationRefNo { get; set; }
        public string EstablishmentName { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public string ApplicationPurposeTitle { get; set; }
        public string StatusDescription { get; set; }
        public string InvestPunjab_Ipin { get; set; }
    }

    public class TemplateInspectionIndustryUserNotificationViewModel
    {
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string LicenseNumber { get; set; }
        public string OfficerName_Factory { get; set; }
        public string OfficerName_Labour { get; set; }
        public string Designation_Factory { get; set; }
        public string Designation_Labour { get; set; }
        public string Mobile_Factory { get; set; }
        public string Mobile_Labour { get; set; }
        public string Circle_Factory { get; set; }
        public string Circle_Labour { get; set; }
    }
    public class TemplateInspectionOfficerUserNotificationViewModel
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public string OfficerName { get; set; }
        public string CircleName { get; set; }
        public List<TemplateInspectionEstablishmentDetails> EstablishmentsList { get; set; }

    }
    public class TemplateInspectionEstablishmentDetails
    {
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string LicenseNumber { get; set; }
    }

    public class TemplateTicketCreationToApplicantNotificationViewModel
    {
        public string RootActivityRefId { get; set; }
    }

    public class TemplateTicketCreationToDevTeamNotificationViewModel
    {
        public string RootActivityRefId { get; set; }
        public string EstablishmentName { get; set; }
        public string UserName { get; set; }
        public string ApplicantName { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public string InvestPunjab_AppId { get; set; }
        public Int64 AppRefId { get; set; }
        public string ApplicationTypeDesc { get; set; }
        public string ApplicationPurposeTypeDesc { get; set; }
        public string ToDoActivityCategoryTypeDesc { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DeveloperName { get; set; }

        public List<ToDoActivityWiseStepViewModel> ToDoActivityWiseSteps { get; set; }
    }
  
  public class TemplateInspectionSubmittedNotificationViewModel
    {
        public int InspectionType { get; set; }
        public string OfficerName { get; set; }
        public string RoleName { get; set; }
        public int IsViolationFound { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string LicenseNumber { get; set; }
        public string FactoryStatus { get; set; }
        public string FactoryCircle { get; set; }

    }

    public class TemplateEscalatedApplicationNotificationViewModel
    {
        public string OfficerName { get; set; }
        public string Designation { get; set; }
        public int IsViolationFound { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string LicenseNumber { get; set; }
        public string FactoryStatus { get; set; }
        public string FactoryCircle { get; set; }

    }
}

