using Microsoft.EntityFrameworkCore.Storage;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class ApplicationInitiateResponseViewModel
    {
        public bool IsApplicationCreated { get; set; }
        public Int64 AppId { get; set; }
        public Int64 EntityKeyId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string ErrorDescription { get; set; }
    }
    public class ApplicationLockParmsViewModel
    {
        public Int64 AppId { get; set; }
        public int AppActionType { get; set; }
        public string Remarks { get; set; }
    }



    public class AppFileUploadInfoViewModel
    {
        public Int64 DocumentId { get; set; }
        public int AllowedMaxMB { get; set; }
        public int AllowedMinMB { get; set; }
        public string DocumentExtensionType { get; set; }
        public string DocumentName { get; set; }
        public bool IsOptional { get; set; }
        public string AlreadyUploaded { get; set; }
        public bool IsDocumentUploadOption { get; set; }
        public bool IsSampleDoc { get; set; }
        public string SampleDocPath { get; set; }
        public List<AppFileAlreadyFileUploadedViewModel> AlreadyUploadedInfo { get; set; }
    }
    public class AppFileAlreadyFileUploadedViewModel
    {
        public string FileName { get; set; }
        public DateTime FileUploadOn { get; set; }
        public bool IsLocked { get; set; }
    }

    public class ApplicationActionViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Application action is required..!")]
        public int AppActionType { get; set; }

        [Required(ErrorMessage = "Receiver_UserRefId is required..!")]
        public string Receiver_UserRefId { get; set; }

        [Required(ErrorMessage = "Receiver profile ref id is required..!")]
        public Int64 Receiver_ProfileRefId { get; set; }
        public string Remarks { get; set; }

        [Required(ErrorMessage = "AppRefId is required..!")]
        public Int64 AppRefId { get; set; }
        public string PdfNameGUID { get; set; }
        public string PublicAppRefNum { get; set; }
        public string CheckListFormJson { get; set; }

        [Required]
        public bool IsDocumentUploaded { get; set; }

        [Required]
        public Int64 AppDocumentRefId { get; set; }

        public int PaymentBatchCounter { get; set; }
        [Required]
        public FactoryHazardousCategoryTypeEnum FactoryHazardousCategoryType { get; set; }

        [Required]
        public FactorySectionCategoryTypeEnum FactorySectionCategoryType { get; set; }

        [Required]
        public FactorySessionCategoryTypeEnum FactorySessionCategoryType { get; set; }
        [Required]
        public FactoryCategoryTypeEnum FactoryCategoryType { get; set; }

        [Required]
        public Int64 LabourCircleRefId { get; set; }

        [Required]
        public Int64 DistrictLgdRefId { get; set; }

        [Required]
        public string RaisedFeeReason { get; set; }

        [Required]
        public decimal RaisedFeeAmount { get; set; }

        [NotMapped]
        public decimal SecurityRaisedFeeAmount { get; set; }

        [Required]
        public Int64 Workers_MaxDuringYear { get; set; }

        [Required]
        public decimal PowerKW_Installed { get; set; }

        [Required]
        public Int64 ExistingWorkers_MaxDuringYear { get; set; }

        [Required]
        public decimal ExistingPowerKW_Installed { get; set; }



        [Required]
        public decimal PsiecCessAmount { get; set; }

        [Required]
        public decimal PsiecProcessingFeeAmount { get; set; }


        [Required(ErrorMessage = "IpAddress is required..!")]
        public string IpAddress { get; set; }

        [Required(ErrorMessage = "Latitude is required..!")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required..!")]
        public string Longitude { get; set; }


        [Required(ErrorMessage = "ApplicationActionLogId is required..!")]
        public Int64 ApplicationActionLogId { get; set; }

        public Int64 DormantId { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType { get; set; }

        public AppActionTypeEnum PreviousActionType { get; set; }
    }
    public class CheckListDataViewModel
    {
        public List<CheckListNodeTeplateViewModel> CheckListNodes { get; set; }
    }

    public class CheckListNodeTeplateViewModel
    {
        public string FieldName { get; set; }
        public bool IsVerified { get; set; }
        public string Remarks { get; set; }
        public bool IsDocument { get; set; }
    }

    public class ApplicationFullDetailsViewModel
    {
        public string OccupierName { get; set; }
        public string EstablishmentName { get; set; }
        public string NatureOfWorkContractLabour { get; set; }
        public int TotalNoWorkersToBeEmployedInLicence { get; set; }
        public int ElectricLoadConnectedInKilowatts { get; set; }
        public DateTime DateOfTerminationOfEmployementUnderEachContractor { get; set; }
        public Decimal AmountCalculated { get; set; }
        public DateTime DepositDate { get; set; }
        public int NumberOfContractLabourToBeEmployed { get; set; }
        public string ManufacturingProcess { get; set; }
    }

    public class ContractLabourPdfDetailsViewModel
    {
        public decimal SlabFees { get; set; }
        public decimal TotalSecurityFee { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ContractorName { get; set; }
        public string NatureOfWork { get; set; }
        public string EstablishmentName { get; set; }
        public int MaxContractLabourToBeEmployed { get; set; }
        public DateTime DurationOfProposedContractWorkEnd { get; set; }
    }

    public class BuildingPlanHudPdfDetailsViewModel
    {
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string ProjectPurpose { get; set; }
        public decimal PlotAreaSqFt { get; set; }
        public decimal PlotAreaAcres { get; set; }
        public decimal BuildingCost { get; set; }
    }

    public class ShopLicencePdfDetailsViewModel
    {
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string OwnerName { get; set; }
        public string OwnerFatherOrHusbandName { get; set; }
        public string PANCard { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeFatherName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
    }

    public class EstablishmentCardInfoViewModel
    {
        public string FileNumber { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string VillageOrTown { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string PinCode { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public string ApplicantName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string ContactPersonEmail { get; set; }
    }

    public class GenerateLicenceNoViewModel
    {
        public string LicenceNo { get; set; }
    }

    public class GenerateDisputeNoViewModel
    {
        public string DisputeNo { get; set; }

        public Int64 EstablishmentEPFOLogsRefId { get; set; }
    }

    public class PrincipalApprovalsDetailsParmsViewModel
    {
        public Int64 IPin { get; set; }
        public Int64 AppId { get; set; }
    }

    public class ApplicationTransferParmsViewModal
    {

        public string Remarks { get; set; }
        public Int64 AppRefId { get; set; }

        public string Receiver_UserRefId { get; set; }
        public Int64 Receiver_UserProfileRefId { get; set; }
        public string Receiver_RoleRefId { get; set; }

        public string Sender_UserRefId { get; set; }
        public Int64 Sender_UserProfileRefId { get; set; }
        public Int64 LabourCircleId { get; set; }

        public Int64 AlcCircleId { get; set; }

        public Int64 FactoryCircleId { get; set; }

        [Required(ErrorMessage = "IpAddress is required..!")]
        public string IpAddress { get; set; }

        [Required(ErrorMessage = "Latitude is required..!")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required..!")]
        public string Longitude { get; set; }
    }

    public class SenderReceiverDetailViewModal
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public Int64 UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NormalizedName { get; set; }
        public string Email { get; set; }
    }
    public class DeemedActionParmsViewModel
    {
        //public Int64 DeemedId { get; set; }
        //public Int64 AppRefId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
    }

    public class AutoApproveActionParmsViewModel
    {
        //public Int64 DeemedId { get; set; }
        //public Int64 AppRefId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
    }
    public class UsersIdWithAppIdParmsViewModel
    {
        public string UserRefId { get; set; }
        public Int64 AppId { get; set; }
    }
    public class LicenceNoViewModel
    {
        public string LicenceNo { get; set; }
    }

    public class DofLicenceDetailsViewModel
    {
        public string DofNumber { get; set; }
        public DateTime DofApprovalDate { get; set; }
    }
  
    public class ApplicationAdditionalDetailsViewModel
    {
        public List<WelfareFundDetailsViewModel> WelfareFundDetails { get; set; }
        public List<AnnualReturnDetailsViewModel> AnnualReturnDetails { get; set; }
    }
    public class WelfareFundDetailsViewModel
    {
        public Int64 PWBCessCollectionId { get; set; }
        public Int64 AppId { get; set; }
        public string FinancialYear { get; set; }
        public string TimeSlot { get; set; }
        public Decimal Amount { get; set; }
    }

    public class AnnualReturnDetailsViewModel
    {
        public Int64 AppId { get; set; }
        public string AccNo { get; set; }
        public string ReturnYear { get; set; }
        public string ReturnType { get; set; }
        public Int64 ReturnID { get; set; }
    }

    public class ApplicationSpecificDataViewModel
    {
        public FactoryLicenceSpecificDataViewModel FactoryLicenceSpecificData { get; set; }
    }

    public class FactoryLicenceSpecificDataViewModel
    {
        public Int64 ExistingWorkers_MaxDuringYear { get; set; }
        public decimal ExistingPowerKW_Installed { get; set; }
    }

    public class ShopLicenceDetailsForNightShiftPdfViewModel
    {
        public string OName { get; set; }
        public string OFHName { get; set; }
        public string MgrName { get; set; }
        public string EstdName { get; set; }
        public string SiteAddress { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public int PinCode { get; set; }
        public string ProjectPurpose { get; set; }
        public string ModifiedTokenNumber { get; set; }
        public string NationalIndustrialClassificationCode { get; set; }
        public DateTime ClearanceIssuedOn { get; set; }
        public string ApplicantSign { get; set; }
        public Int64 TotalEmployees { get; set; }
    }

    public class FactoryLicenceDetailsForNightShiftPdfViewModel
    {
        public string RegNo { get; set; }
        public DateTime IssueDate { get; set; }
        public string FileNo { get; set; }
        public string EName { get; set; }
        public string EFHName { get; set; }
        public string NameOftheEstablishment { get; set; }
        public string ManagerName { get; set; }
        public string PostalAddressEstablishment { get; set; }
        public string BusinessNature { get; set; }
        public string ProjectPurpose { get; set; }
        public Int64 TotalEmployees { get; set; }
        public DateTime DateOfPreviousRegistration { get; set; }
        public DateTime PermissionGrantedForWomenWorkingNightShift { get; set; }
        public DateTime ValidPermissionGrantedForWomenWorkingNightShift { get; set; }
    }



    public class BocwLicenceDetailsPdfViewModel
    {
        public string PricipalEmployerName { get; set; }
    }

    public class ReceiverBackupViewModel
    {
        public Int64 Receiver_ProfileRefId { get; set; }
        public string Receiver_UserRefId { get; set; }
        public string ReceiverRoleId { get; set; }
        public DateTime? Receiver_ActionOn { get; set; }
        public string Receiver_Remarks { get; set; }

        public string Sender_UserRefId { get; set; }

        public Int64 Sender_ProfileRefId { get; set; }

        public string SenderRoleId { get; set; }
        public AppActionTypeEnum AppActionType { get; set; }

        public bool IsCurrentReceiver { get; set; }

        public ActionTakenModeTypeEnum ActionTakenModeType { get; set; }

        public Int64 AppActionLogRefId { get; set; }
        public Int64 ActionTakenHoursCount { get; set; }
        public Int64 ActionTakenDaysCount { get; set; }
        public bool IsDocumentUploaded { get; set; }
        public Int64 AppDocumentRefId { get; set; }
        public int ReceiverRepeatedCount { get; set; }

    }
    public class AppActionTime_MutualProcessFlagViewModel
    {
        public bool HasClosed { get; set; }
        public string OfficerName { get; set; }
        public string OfficerDesignation { get; set; }
    }

    public class DormantParmsViewModel
    {
        //public Int64 DeemedId { get; set; }
        //public Int64 AppRefId { get; set; }
        public Int64 DormantProcessEngineRefId { get; set; }
    }

    public class ApplicationToBeEscalatedViewModel
    {
        public Int64 AppRefId { get; set; }
        public Int32 ApplicationType { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string PendingWithUserId { get; set; }
        public int TotalDaysCount { get; set; }
        public int NetDaysCount { get; set; }
        public decimal ActualPercentage { get; set; }
        public int PercentageSlab { get; set; }
        public int MaxTATDays { get; set; }
        public int TotalWorkingDaysCount { get; set; }
        public int TotalHolidaysCount { get; set; }
        public int TotalObjectionDaysCount { get; set; }
        public Int64 FactoryCircleRefId { get; set; }
        public Int64 AlcCircleRefId { get; set; }
        public Int64 LabourCircleRefId { get; set; }
        public bool IsTimeLineFlow { get; set; }


    }
    public class ApplicationEscalationSlabViewModel
    {
        public int SlabRangeFrom { get; set; }
        public int SlabRangeTo { get; set; }
        public int SlabValue { get; set; }

    }

    public class EscalatedApplicationViewModel
    {
        public Int32 ApplicationType { get; set; }
        public string ServiceName { get; set; }
        public string PublicAppRefNum { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string OfficerName { get; set; }
        public string Designation { get; set; }
        public string FactoryCircleName { get; set; }
        public string ALCCircleName { get; set; }
        public string LabourCircleName { get; set; }
        public int MaxDays_TAT { get; set; }
        public int TotalDays_Pending { get; set; }
        public int PercentageSlab { get; set; }
    }


    public class EscalatedApplicationSlabWiseViewModel
    {
        public string ApplicationTypeDesc { get; set; }
        public int MaxDays_TAT { get; set; }
        public int Slab50 { get; set; }
        public int Slab65 { get; set; }
        public int Slab80 { get; set; }
    }

    public class EscalationFileAndActWiseDataViewModel
    {
        public List<EscalatedApplicationViewModel> EscalatedApplication { get; set; }
        public List<EscalatedApplicationSlabWiseViewModel> EscalatedApplicationSlabWise { get; set; }
        
    }

    public class OfficerUserDetailsViewModel
    {
        public string OfficerName { get; set; }
        public string Designation { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }

    public class EscalationNotificatonDataViewModel
    {
        public OfficerUserDetailsViewModel OfficerUserDetails { get; set; }
        public List<EscalatedApplicationSlabWiseViewModel> EscalatedApplicationSlabWise { get; set; }
        public string EncryptedParms { get; set; }
        public DateTime EscalationDate { get; set; }

    }

    public class EncryptedEscalationSerializedDataViewModel
    {
        public Int64 EscalationProcessEngineRefId { get; set; }
        public string userId { get; set; }
    }

    public class EscalationOffcerViewModel
    {
        public string Users { get; set; }
    }


    public class All_Act_Deemed_ProcessEngineLogsViewModel
    {
        public Int64 DeemedId { get; set; }
        public Int64 AppRefId { get; set; }
        public Int64 AllActDeemedProcessEngineRefId { get; set; }
        public Int32 ApplicationType { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime InitiateDeemedDate { get; set; }
        public DateTime DeemedDate { get; set; }
        public string TotalTime { get; set; }
        public string TotalHolidaysTime { get; set; }
        public string TotalWeekEndsTime { get; set; }
        public string TotalObjectionTime { get; set; }
        public string DeemedInTime { get; set; }
        public string MaxDeemedTime { get; set; }
        public string DeemedTimeType { get; set; }
        public int DeemedProcessStatusType { get; set; }
        public string DeemedProcessRemarks { get; set; }
        public string CertificateFileName { get; set; }
        public DateTime? FileDeemedDate { get; set; }
        public string Officer_UserRefId { get; set; }
        public string Officer_RoleRefId { get; set; }
        public Int64 Officer_ProfileRefId { get; set; }
        public string ServiceName { get; set; }
        public string CurrentPendingWith { get; set; }
        public string EstablishmentName { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string PublicAppRefNum { get; set; }
        public bool IsFeeApplicable { get; set; }
        public string InvetPunjabiPin { get; set; }
        public Int64 InvetPunjabiAppId { get; set; }

    }

    public class DeemedApplicationTimeLineViewModel
    {
        public Int64 AppRefId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string ServiceName { get; set; }
        public Int64 ApplicationType { get; set; }
        public string EstablishmentName { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public int TotalTime { get; set; }
        public int TotalHolidaysTime { get; set; }
        public int TotalWeekEndsTime { get; set; }
        public int TotalObjectionTime { get; set; }
        public int DeemedInTime { get; set; }
        public int MaxDeemedTime { get; set; }
        public string DeemedTimeType { get; set; }
        public DateTime? LastDeemedDate { get; set; }
        public string UserName { get; set; }

    }

    public class DepartmentTimeTakenViewModel
    {
        public Int64 ApplicationActionLogId { get; set; }
        public int ApplicationRefId { get; set; }

        public Int64 ApplicationType { get; set; }
        public string ReceiverUserRoleId { get; set; }
        public int AppActionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate_Actual { get; set; }
        public DateTime EndDate_Actual { get; set; }
        public int TotalTimeInMintue { get; set; }
        public int TimeGapInMintue { get; set; }
        public int TotalHolidaysTime { get; set; }
        public int TotalWeekEndsTime { get; set; }
        public int MaxDeemedTime { get; set; }
        public int ActionPendingWithType { get; set; }


    }

    //public class CurrentStatusViewModel
    //{
    //    public ApplicationAction ApplicationAction { get; set; }
    //    public ApplicationAction_ParallelProcess ApplicationAction_ParallelProcess { get; set; }
    //    public bool IsParallelProcess { get; set; }
    //}

}


