using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class InspectionViewModal
    {

    }
    public class Inspection_RandomizeDashboardDataViewModel
    {
        public Int64 RandomizationId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime Timestemp { get; set; }
        public int TotalInspections { get; set; }
        public int TotalTransfered { get; set; }
        public int TotalSubmitted { get; set; }
        public int TotalPending { get; set; }
        public int TotalAssigned { get; set; }
        public int? IsLocked { get; set; }
        public DateTime? LockedOn { get; set; }
        public string? LockedBy_Name { get; set; }
        public string? LockedBy_Role { get; set; }
        public int? TotalTimeTaken_Hours { get; set; }
        public int? TotalHolidays { get; set; }
        public int? TotalWeekends { get; set; }
        public Int64 FactoryRefId { get; set; }
        public int MaxRows { get; set; }


    }
    public class InspectioninfoViewModel
    {
        public Int64 InspectionId { get; set; }
        public Int64 AppId { get; set; }
        public string LicenceNumber { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public Int64 DistrictRefId { get; set; }
        public Int64 FactoryCircleId { get; set; }
        public Int64 LabourCircleRefId { get; set; }
        public Int64 LabourWingProfileRefId { get; set; }
        public Int64 RandomizationRefId { get; set; }

        public bool IsSubmitted_Factory_Wing { get; set; }
        public DateTime? InspectionDoneOn_Factory_Wing { get; set; }

        public DateTime? InspectionSubmitOn_Factory_Wing { get; set; }
        public string SubmittedByName_FactoryWing { get; set; }
        public string SubmittedByRole_FactoryWing { get; set; }

        public bool IsSubmitted_Labour_Wing { get; set; }
        public DateTime? InspectionDoneOn_Labour_Wing { get; set; }
        public DateTime? InspectionSubmitOn_Labour_Wing { get; set; }
        public string SubmittedByName_LabourWing { get; set; }
        public string SubmittedByRole_LabourWing { get; set; }

        public string EstbDetail { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string LabourCircleName { get; set; }    
        public int TotalTimeTaken_Hours_Factory { get; set; }
        public int TotalTimeTaken_Hours_Labour { get; set; }
        public int TotalHolidays_Factory { get; set; }
        public int TotalHolidays_Labour { get; set; }
        public int TotalWeekends_Factory { get; set; }
        public int TotalWeekends_Labour { get; set; }
        public int IsTransfered { get; set; }
        public string TransferedReceiverUserId { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public bool IsLegacy { get; set; }
        public int FactoryCurrentStatus { get; set; }
    }

    public class LabourCircleUserDetailsViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public Int64 LabourCircleRefId { get; set; }
    }

    public class GetInspections_FactoryPerformaStepStatusViewModel
    {
        public int IsCompleted { get; set; }
        public string Code { get; set; }
    }

    public class Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel
    {
        public Int64 InspectionId { get; set; }
        public bool IsLegacy { get; set; }
        public Int64 LabourCircleRefId { get; set; }
        public Int64 RandomizationRefId { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string LabourCircleName { get; set; }
        public Int64 DistrictRefId { get; set; }
        public bool HasAssignedLabourCircle { get; set; }
        public int IsTransfered { get; set; }
    }

    public class Inpection_LockViewModel
    {
        public string FactoryDeRegistrationNo { get; set; }
        public DateTime? InspectionDate { get; set; }
        public InspectionEstablishmentTypeEnum InspectionEstablishmentType { get; set; }
        public InspectionFactoryExistenceTypeEnum InspectionFactoryExistenceType { get; set; }
        public Int64 InspectionRefId { get; set; }
        public string Remarks { get; set; }
        public string UserRefId { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public Int64 AppId { get; set; }
        public bool IsLegacy { get; set; }

    }

    public class InspectionEstablishmentBasicDetailsViewModel
    {
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string LicenceNo { get; set; }
        public Int64 WorkerDuringYear { get; set; }
        public Int64 WorkerPastYear { get; set; }
        public decimal InstalledPower { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ApplicantUserId { get; set; }
    }

    public class InspectionFactoryDetailsViewModel
    {
        public string? FactoryName { get; set; }
        public string? FactoryAddress { get; set; }
        public string? OccupierName { get; set; }
        public string? OccupierAddress { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerAddress { get; set; }
        public string? ManufacturingProcess { get; set; }
        public DateTime? LastInspectionDate { get; set; }
    }

    public class GetFactoryCirlceViewModel
    {
        public Int64 FactoryCircleId { get; set; }
        public Int64 DistrictId { get; set; }
    }

    public class InspectionTransferInfoViewModel
    {
        public string UserRefId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Int64 UserProfileRefId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public Int64 CircleId { get; set; }
        public string CircleName { get; set; }
        public string JuridictionArea { get; set; }

    }

    public class PropertyTitleValuePair
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string PropertyName { get; set; }

    }

    public class DesignatedOfficerDetailViewModel
    {
        public string OfficerName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string Designation { get; set; }
        public string CircleName { get; set; }
    }
    public class UpdateAlternateContactDetailsViewModel
    {
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public Int64 AppId { get; set; }
        public bool IsLegacy { get; set; }
    }

    public class LicenceNumberDetailsViewModel
    {
        public string LicenceNumber { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public int IsMatched { get; set; }
        public int IsLegacy { get; set; }
        public string UserId { get; set; }
    }
    public class InspectionSectionWiseKeyValuePair
    {
        public List<PropertyTitleValuePair> pairs_GeneralDetails { get; set; }
        public List<PropertyTitleValuePair> pairs_FactoryDetails { get; set; }
        public List<PropertyTitleValuePair> pairs_Health { get; set; }
        public List<PropertyTitleValuePair> pairs_Safety { get; set; }
        public List<PropertyTitleValuePair> pairs_Welfare { get; set; }
        public List<PropertyTitleValuePair> pairs_General { get; set; }
        public List<PropertyTitleValuePair> pairs_Accident { get; set; }
        public List<PropertyTitleValuePair> pairs_EnumerationAct { get; set; }
        public List<PropertyTitleValuePair> pairs_MinimumWageAct { get; set; }
        public List<PropertyTitleValuePair> pairs_PaymentWagesAct { get; set; }
        public List<PropertyTitleValuePair> pairs_StatutoryReport { get; set; }
        public List<PropertyTitleValuePair> pairs_AdolescentLabourAct { get; set; }
        public List<PropertyTitleValuePair> pairs_FestivalHolidays { get; set; }
        public List<PropertyTitleValuePair> pairs_MaternityBenefitAct { get; set; }
        public List<PropertyTitleValuePair> pairs_ContractLabourAct { get; set; }
        public List<PropertyTitleValuePair> pairs_MigrantWorkmenAct { get; set; }
        public List<PropertyTitleValuePair> pairs_WelfareFundAct { get; set; }
        public List<PropertyTitleValuePair> pairs_GratuityAct { get; set; }
        public List<PropertyTitleValuePair> pairs_IndustrialEmploymentAct { get; set; }
        public List<PropertyTitleValuePair> pairs_BOCWAct { get; set; }
        public List<PropertyTitleValuePair> pairs_ObservationsAct { get; set; }

    }

    public class InspectionActionViewModel
    {
        public string UserId { get; set; }

        public int AppActionType { get; set; }

        public string Receiver_UserRefId { get; set; }
        public Int64 Receiver_ProfileRefId { get; set; }
        public string Remarks { get; set; }
        public Int64 InspectionRefId { get; set; }
        public string PdfNameGUID { get; set; }
        public string PublicAppRefNum { get; set; }
        public bool IsDocumentUploaded { get; set; }
        public Int64 AppDocumentRefId { get; set; }
        public string IpAddress { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Int32 AllowedDays { get; set; }
        public Int32 InspectionType { get; set; }
    }

    public class GetInspectionNotingLogsViewModel
    {
        public string StatusDesc { get; set; }
        public DateTime StatusDate { get; set; }
        public Int64 AppPBIPActionLogId { get; set; }
        public string OfficerName { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public Int64 ActionTakenDaysCount { get; set; }
        public bool IsDocumentUploaded { get; set; }
        public Int64 AppDocumentRefId { get; set; }
        public string AttachmentName { get; set; }
    }

    public class AlternateContactDetailsViewModel
    {
        public string AlternateMobileNo { get; set; }
        public string AlternateEmail { get; set; }
    }
    public class EncryptedOTPViewModel
    {
        public int OTP { get; set; }
        public string MobileNumber { get; set; }
    }

    public class InspectionTransferLogsViewModel
    {
        
        public string OfficerName { get; set; }
        public string NormalizedName { get; set; }
        public string UserName { get; set; }
        public DateTime StatusDate { get; set; }
        public string Remarks { get; set; }
        public string ActionName { get; set; }

    }
    public class InspectionLabourCircleViewModel
    {
        public Int64 LabourCircleId { get; set; }
        public string LabourCircleName { get; set; }
        public string JuridcitionArea { get; set; }
        public string ALCCircleName { get; set; }
        public Int64 ALCCircleRefId { get; set; }
        public string OfficerName { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public Int64 UserProfileId { get; set; }
    }

    public class InspectionFactoryAllotmentDetailsViewModel
    {
        public string? FactoryName { get; set; }
        public string? FactoryAddress { get; set; }
        public string? OccupierName { get; set; }
        public string? OccupierAddress { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerAddress { get; set; }
        public string? ManufacturingProcess { get; set; }
        public DateTime? LastInspectionDate { get; set; }
        public Int64 FactoryHazardousCategoryType { get; set; }
        public Int64 FactorySessionCategoryType { get; set; }
        public string FactorySectionCategoryType { get; set; }
        public int FactoryCategoryType { get; set; }
        public Int64 AppRefId { get; set; }
        public Int64 FactoryCircleRefId { get; set; }
        public string FactoryCircleName { get; set; }
        public int IsLegacy { get; set; }
        public string UserId { get; set; }
        public Int64 DistrictRefId { get; set; }
    }

    public class InspectionAllotmentRequestViewModel
    {
        public Int64 FactoryHazardousCategoryType { get; set; }
        public Int64 FactorySessionCategoryType { get; set; }
        public string FactorySectionCategoryType { get; set; }
        public Int64 AppRefId { get; set; }
        public Int64 FactoryCircleRefId { get; set; }
        public int IsLegacy { get; set; }
        public string LicenceNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string UserId { get; set; }
        public int DistrictId { get; set; }
        public string SubmittedUserId { get; set; }
        public string SubmittedRoleId { get; set; }
        public Int64 SubmittedProfileId { get; set; }
    }


}
