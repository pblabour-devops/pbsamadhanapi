using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Application
    {
        [Key]
        public Int64 AppId { get; set; }

        [Required(ErrorMessage = "Application type is required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [Required(ErrorMessage = "Application purpose type is required..!")]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [Required(ErrorMessage = "Public application reference number is required..!")]
        [StringLength(20, ErrorMessage = "The max length of public application reference number is 100 characters..!")]

        //********Unique constrain is pending******
        public string PublicAppRefNum { get; set; }

        [Required(ErrorMessage = "Iteration count is required..!")]
        public Int64 IterationCount { get; set; }

        [Required(ErrorMessage = "Created date is required..!")]
        public DateTime CreatedOnDate { get; set; }

        [Required(ErrorMessage = "Last modified on date is required..!")]
        public DateTime LastModifiedOnDate { get; set; }

        [Required(ErrorMessage = "IsEnabled is required..!")]
        public bool IsEnabled { get; set; }

        [Required(ErrorMessage = "IsDeleted is required..!")]
        public bool IsDeleted { get; set; }

        [Required]
        public bool IsDigitalSignatureRequired { get; set; }

        [Required]
        public bool IsDigitalSignatureVerified { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [Required]
        public bool IsAllowEdit { get; set; }

        [Required]
        public bool IsFeeApplicable { get; set; }

        [Required]
        public ApplicationLifeCycleStatusTypeEnum ApplicationLifeCycleStatusType { get; set; } = 0;

        [Required]
        public DateTime ApplicationLifeCycleLastStatusOn { get; set; }

        [Required]
        public int PaymentBatchCounter { get; set; } = 1;

        public bool Legacy_IsMigrated { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public Int64 Legacy_AppFormId { get; set; }
        public string Legacy_NAR { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public string Legacy_LicenceNo { get; set; }
        [Required(ErrorMessage = "ProjectSiteVersion is required..!")]
        public int ProjectSiteVersion { get; set; } = 1;


        //[Required(ErrorMessage = "RootActivityRefId is required..!")]
        //[StringLength(100, ErrorMessage = "RootActivityRefId lenght is 100")]
        //public string RootActivityRefId { get; set; }

        /*[Required(ErrorMessage = "RootActivityRefId is required..!")]
        [StringLength(100, ErrorMessage = "RootActivityRefId lenght is 100")]
        public string RootActivityRefId { get; set; }*/

        [Required]
        public bool IsIPIntegrated { get; set; }

        [Required]
        public bool IsTimeLineFlow { get; set; }

        #region ForeignKeyReferences

        public virtual Establishment_GeneralDetail Establishment_GeneralDetail { get; set; }
        public virtual ApplicationAction ApplicationAction { get; set; }
        public virtual Contractor_GeneralDetail Contractor_GeneralDetail { get; set; }

        [ForeignKey("ProjectSites")]
        public Int64 ProjectSiteRefId { get; set; }
        public virtual ProjectSite ProjectSites { get; set; }

        //[Required(ErrorMessage = "DigitalSignatureRefId id is required..!")]
        [ForeignKey("DigitalSignature")]
        public Int64? DigitalSignatureRefId { get; set; }
        public virtual DigitalSignature DigitalSignature { get; set; }

        [Required(ErrorMessage = "DepartmentRefId id is required..!")]
        [ForeignKey("DepartmentRefID")]
        public Int64 DepartmentRefID { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<ApplicationDocument> ApplicationDocuments { get; set; }
        public virtual BP_GeneralDetail BP_GeneralDetail { get; set; }
        public virtual ICollection<AppFeeDetail> AppFeeDetails { get; set; }
        public virtual ICollection<AppFeeTransaction> AppFeeTransactions { get; set; }
        public virtual ApplicationCircleMapping ApplicationCircleMapping { get; set; }
        public virtual CommonLicence_GeneralDetail CommonLicence_GeneralDetail { get; set; }
        public virtual ICollection<AppPaymentSuccessTransactionMapping> AppPaymentSuccessTransactionMappings { get; set; }
        public virtual BuildingPlan BuildingPlan { get; set; }
        public virtual BuildingPlanHUD_GeneralDetail BuildingPlanHUD_GeneralDetail { get; set; }
        public virtual ICollection<BuildingPlanHUDPaymentDetail> BuildingPlanHUDPaymentDetails { get; set; }
        public virtual ShopLicence_GeneralDetail ShopLicence_GeneralDetail { get; set; }
        public virtual ApplicationLicenceNoMapping ApplicationLicenceNoMapping { get; set; }
        public virtual ICollection<LegacyApprovedClearenceMapping> LegacyApprovedClearenceMappings { get; set; }
        public virtual BuildingPlanFactory_GeneralDetail BuildingPlanFactory_GeneralDetail { get; set; }
        public virtual ICollection<ApplicationAddendumDocument> ApplicationAddendumDocuments { get; set; }
        public virtual ICollection<AppPaymentPart> AppPaymentParts { get; set; }
        public virtual Deemed_ProcessFilesLog Deemed_ProcessFilesLog { get; set; }
        public virtual LWB_Contribution LWB_Contributions { get; set; }
        public virtual Licence_Factory_GeneralDetail Licence_Factory_GeneralDetails { get; set; }
        public virtual ICollection<Licence_Factory_AmendmentDataHistory> Licence_Factory_AmendmentDataHistories { get; set; }
        //public virtual ICollection<Licence_Factory_QuestionnaireDetail> Licence_Factory_QuestionnaireDetails { get; set; }
        public virtual ICollection<Licence_Shop_NightShift_Approval> Licence_Shop_NightShift_Approvals { get; set; }
        public virtual Licence_Factory_AdditionalDetail Licence_Factory_AdditionalDetail { get; set; }
        public virtual Licence_ContractLabour_GeneralDetail Licence_ContractLabour_GeneralDetails { get; set; }
        public virtual ICollection<Licence_ContractLabour_AmendmentDataHistory> Licence_ContractLabour_AmendmentDataHistories { get; set; }
        public virtual BuildingPlanFactory_Declaration_Stability_Certificate BuildingPlanFactory_Declaration_Stability_Certificate { get; set; }
        public virtual ICollection<Licence_Factory_NightShift_Approval> Licence_Factory_NightShift_Approvals { get; set; }
        public virtual Licence_BocwAct_GeneralDetail Licence_BocwAct_GeneralDetail { get; set; }
        public virtual ApplicationDirtyStatusMapping ApplicationDirtyStatusMappings { get; set; }
        public virtual Licence_MotorTransport Licence_MotorTransport { get; set; }
        public virtual Licence_TradeUnion Licence_TradeUnion { get; set; }
        public virtual Licence_Proposed_BuildingPlan_GeneralDetail Licence_Proposed_BuildingPlan_GeneralDetail { get; set; }
        public virtual Licence_Existing_BuildingPlan_GeneralDetail Licence_Existing_BuildingPlan_GeneralDetail { get; set; }
        public virtual Licence_Addition_Amendment_BuildingPlan_GeneralDetail Licence_Addition_Amendment_BuildingPlan_GeneralDetail { get; set; }
        public virtual Licence_PE_ISM_GeneralDetail Licence_PE_ISM_GeneralDetails { get; set; }
        public virtual Licence_PE_ISM_AmendmentDataHistories Licence_PE_ISM_AmendmentDataHistories { get; set; }
        public virtual Licence_CL_PE_GeneralDetail Licence_CL_PE_GeneralDetail { get; set; }
        public virtual Licence_PE_AmendmentDataHistories Licence_PE_AmendmentDataHistories { get; set; }
        public virtual TestMaster TestMaster { get; set; }
        public virtual ICollection<BuildingPlanFactory_Declaration_Stability_Randomization> BuildingPlanFactory_Declaration_Stability_Randomizations { get; set; }
        public virtual ICollection<Payments_RaisedFee> Payments_RaisedFee { get; set; }
        public virtual AppProcessLoadBalanceLog AppProcessLoadBalanceLogs { get; set; }
        public virtual ICollection<AppActionTimeLine> AppActionTimeLines { get; set; }
        public virtual ICollection<AppActionTimeLineDefination> AppActionTimeLineDefinations { get; set; }
        public virtual ICollection<ApplicationProcessPhaseLog> ApplicationProcessPhaseLogs { get; set; }
        public virtual Licence_ISM_ContractLabour_GeneralDetail Licence_ISM_ContractLabour_GeneralDetails { get; set; }

        public virtual ICollection<Licence_ISM_ContractLabour_AmendmentDataHistory> Licence_ISM_ContractLabour_AmendmentDataHistories { get; set; }
        public virtual Licence_BuildingPlan_PSIEC_GeneralDetail Licence_BuildingPlan_PSIEC_GeneralDetails { get; set; }
        public virtual ICollection<ApplicationAction_ParallelProcess> ApplicationAction_ParallelProcesses { get; set; }
        public virtual OSH_Form_1_Registration OSH_Form_1_Registration { get; set; }
        public virtual OSH_Form_1_Registration_Factory OSH_Form_1_Registration_Factory { get; set; }
        public virtual OSH_Form_1_Registration_BOCW OSH_Form_1_Registration_BOCW { get; set; }
        public virtual OSH_Form_1_Registration_EmployeeDetail OSH_Form_1_Registration_EmployeeDetail { get; set; }

        public virtual OSH_Form_1_Registration_EPFO_ESIC_Detail OSH_Form_1_Registration_EPFO_ESIC_Detail { get; set; }
        public virtual OSH_Form_1_Registration_EmployerDetail OSH_Form_1_Registration_EmployerDetail { get; set; }
        public virtual OSH_Form_1_Registration_PrincipalEmployerDetail OSH_Form_1_Registration_PrincipalEmployerDetail { get; set; }
        public virtual OSH_Form_1_Registration_ContractorDetail OSH_Form_1_Registration_ContractorDetail { get; set; }
        public virtual OSH_Form_1_Registration_MotorTransportDetail OSH_Form_1_Registration_MotorTransportDetail { get; set; }

        public virtual OSH_Form_30_CommonLicense_Establishment OSH_Form_30_CommonLicense_Establishment { get; set; }
        public virtual OSH_Form_30_CommonLicense_Factory OSH_Form_30_CommonLicense_Factory { get; set; }
        public virtual OSH_Form_30_CommonLicense_ContractLabour OSH_Form_30_CommonLicense_ContractLabour { get; set; }

        public virtual OSH_Form_21_ContractLabour_General_Detail OSH_Form_21_ContractLabour_General_Detail { get; set; }
        public virtual OSH_Form_21_ContractLabour_Employee_Detail OSH_Form_21_ContractLabour_Employee_Detail { get; set; }
        public virtual OSH_Form_21_ContractLabour_Establishment_Detail OSH_Form_21_ContractLabour_Establishment_Detail { get; set; }
        public virtual OSH_Form_21_ContractLabour_MigrantWorker OSH_Form_21_ContractLabour_MigrantWorker { get; set; }

        #endregion ForeignKeyReferences

        #region for samadhaan portal
        public virtual WorkerDetail WorkerDetail { get; set; }
        public virtual ICollection<AppComplaintTypeMapping> AppComplaintTypeMappings { get; set; }

        public virtual ICollection<Complaint_EmployerORContractorDetail> Complaint_EmployerORContractorDetails { get; set; }
        public virtual Complaint_WorkplaceDetail Complaint_WorkplaceDetails { get; set; }
        public virtual Complaint_EstablishmentDetail Complaint_EstablishmentDetails { get; set; }
        public virtual Complaint_GratuityClaim Complaint_GratuityClaims { get; set; }
        public virtual Complaint_MaternityBenefitComplaint Complaint_MaternityBenefitComplaints { get; set; }
        public virtual Complaint_Claim_CodeOnWage Complaint_Claim_CodeOnWages { get; set; }
        public virtual Complaint_MinimumWagesNotPaid Complaint_MinimumWagesNotPaid { get; set; }
        public virtual ICollection<Complaint_MinimumWagesNotPaidPeriodAmount> Complaint_MinimumWagesNotPaidPeriodAmounts { get; set; }



        #endregion]

    }
    public class ApplicationAction
    {
        [Key]
        public Int64 AppActionId { get; set; }

        [Required(ErrorMessage = "Application action is required..!")]
        public int AppActionType { get; set; }

        [Required(ErrorMessage = "Sender user ref id is required..!")]
        [StringLength(450)]
        [ForeignKey("User_Sender")]
        public string Sender_UserRefId { get; set; }
        public virtual User ApplicationActions_User_Sender { get; set; }

        [Required(ErrorMessage = "Sender profile ref id is required..!")]
        [ForeignKey("Profile_Sender")]
        public Int64 Sender_ProfileRefId { get; set; }
        public virtual UserProfile ApplicationActions_Profile_Sender { get; set; }
        [Required(ErrorMessage = "Receiver user ref id is required..!")]
        [StringLength(450)]
        [ForeignKey("User_Receiver")]
        public string Receiver_UserRefId { get; set; }
        public virtual User ApplicationActions_User_Receiver { get; set; }

        [Required(ErrorMessage = "Receiver profile ref id is required..!")]
        [ForeignKey("Profile_Receiver")]
        public Int64 Receiver_ProfileRefId { get; set; }
        public virtual UserProfile ApplicationActions_Profile_Receiver { get; set; }

        [Required(ErrorMessage = "Action date is required..!")]
        public DateTime ActionDate { get; set; }

        [Required(ErrorMessage = "Action taken days count is required..!")]
        public Int64 ActionTakenDaysCount { get; set; }

        [Required(ErrorMessage = "Action taken hours count is required..!")]
        public Int64 ActionTakenHoursCount { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        //[StringLength(500, ErrorMessage = "The max length of remarks is 500 characters..!")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Sender role id is required..!")]
        [StringLength(450)]
        public string SenderRoleId { get; set; }

        [Required(ErrorMessage = "Receiver role id is required..!")]
        [StringLength(450)]
        public string ReceiverRoleId { get; set; }

        public string Checklist_Json { get; set; }

        [Required]
        public bool Checklist_IsAllAgreed { get; set; }

        [Required]
        public int Checklist_FieldObjections { get; set; }

        [Required]
        public int Checklist_DocObjections { get; set; }

        [Required]
        public bool IsDocumentUploaded { get; set; }

        [Required]
        public Int64 AppDocumentRefId { get; set; }

        [Required(ErrorMessage = "IP address is required..!")]
        public string IpAddress { get; set; }

        public bool Legacy_IsMigrated { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public Int64 Legacy_AppFormId { get; set; }
        public string Legacy_NAR { get; set; }
        public int Legacy_StatusId { get; set; }

        [NotMapped]
        public string ActionPublicName { get; set; }

        [Required(ErrorMessage = "Latitude is required..!")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required..!")]
        public string Longitude { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType { get; set; }

        [Required(ErrorMessage = "IsDeleted is required..!")]
        public bool IsDeleted { get; set; } = false;

        #region ForeignKeyReferences
        //public virtual ICollection<ApplicationActionLog> ApplicationActionLogs { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 ApplicationRefId { get; set; }
        public virtual Application Application { get; set; }

        #endregion ForeignKeyReferences
    }
    public class ApplicationActionLog
    {
        [Key]
        public Int64 ApplicationActionLogId { get; set; }

        [Required(ErrorMessage = "Application action is required..!")]
        public int AppActionType { get; set; }

        [Required(ErrorMessage = "Sender user ref id is required..!")]
        [StringLength(450)]
        public string Sender_UserRefId { get; set; }

        [Required(ErrorMessage = "Sender profile ref id is required..!")]
        public Int64 Sender_ProfileRefId { get; set; }


        [Required(ErrorMessage = "Receiver user ref id is required..!")]
        [StringLength(450)]
        public string Receiver_UserRefId { get; set; }

        [Required(ErrorMessage = "Receiver profile ref id is required..!")]
        public Int64 Receiver_ProfileRefId { get; set; }


        [Required(ErrorMessage = "Action date is required..!")]
        public DateTime ActionDate { get; set; }

        [Required(ErrorMessage = "Action taken days count is required..!")]
        public Int64 ActionTakenDaysCount { get; set; }

        [Required(ErrorMessage = "Action taken hours count is required..!")]
        public Int64 ActionTakenHoursCount { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        //[StringLength(500, ErrorMessage = "The max length of remarks is 500 characters..!")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Sender role id is required..!")]
        [StringLength(450)]
        public string SenderRoleId { get; set; }

        [Required(ErrorMessage = "Receiver role id is required..!")]
        [StringLength(450)]
        public string ReceiverRoleId { get; set; }

        public string Checklist_Json { get; set; }

        [Required]
        public bool Checklist_IsAllAgreed { get; set; }

        [Required]
        public int Checklist_FieldObjections { get; set; }

        [Required]
        public int Checklist_DocObjections { get; set; }

        [Required]
        public bool IsDocumentUploaded { get; set; }

        [Required]
        public Int64 AppDocumentRefId { get; set; }

        [Required(ErrorMessage = "IP address is required..!")]
        public string IpAddress { get; set; }

        public bool Legacy_IsMigrated { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public Int64 Legacy_AppFormId { get; set; }
        public string Legacy_NAR { get; set; }
        public int Legacy_StatusId { get; set; }

        [Required(ErrorMessage = "Latitude is required..!")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required..!")]
        public string Longitude { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType { get; set; }

        [Required(ErrorMessage = "IsDeleted is required..!")]
        public bool IsDeleted { get; set; } = false;

        #region ForeignKeyReferences

        [Required(ErrorMessage = "Application ref id is required..!")]
        public Int64 ApplicationRefId { get; set; }

        //[Required(ErrorMessage = "Application ref id is required..!")]
        //[ForeignKey("ApplicationAction")]
        //public Int64 AppActionRefId { get; set; }
        public virtual ApplicationAction ApplicationAction { get; set; }

        public virtual ICollection<AppActionTimeLine> AppActionTimeLines { get; set; }
        public virtual ICollection<AppActionTimeLineDefination> AppActionTimeLineDefinations { get; set; }

        #endregion ForeignKeyReferences
    }
    public class ApplicationMamnagement
    {
        public static Int64 InitiateApplication()
        {
            return 0;
        }
    }
    public class AppTypeAllowedDocument
    {
        [Key]
        public Int64 AppTypeAllowedDocumentId { get; set; }

        [Required]
        [ForeignKey("Document")]
        public Int64 DocRefId { get; set; }
        public virtual Document Document { get; set; }

        [Required]
        public Int64 AppTypeId { get; set; }

        [Required]
        public bool IsOptional { get; set; }
    }
    public class ApplicationActionCode
    {
        [Required, Key]
        public int AppActionCodeId { get; set; }
        public int ActionCode { get; set; }
        public string ActionName { get; set; }
        public string ActionPublicName { get; set; }
    }
    public class RoleWiseAllowedActionCode
    {
        [Required, Key]
        public Int64 RoleWiseAllowedActionCodeId { get; set; }

        [Required(ErrorMessage = "RoleId is required..!")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Cuurent Action Code is required..!")]
        public Int64 CurrentActionCode { get; set; }

        [Required(ErrorMessage = "IsDocumentUploadOption is required..!")]
        public bool IsDocumentUploadOption { get; set; }

        [Required(ErrorMessage = "IsDocumentUploadedRequired is required..!")]
        public bool IsOptional { get; set; }

        [Required]
        [ForeignKey("Document")]
        public Int64? DocRefId { get; set; }
        public virtual Document Document { get; set; }

        [Required(ErrorMessage = "AllowedActionCode is required..!")]
        public Int64 AllowedActionCode { get; set; }

        [Required(ErrorMessage = "Application type is required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [NotMapped]
        public string ActionName { get; set; }

        [Required(ErrorMessage = "IsEnabled is required..!")]
        public bool IsEnabled { get; set; }
    }
    public class ApplicationClearencesCondition
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required]
        public string ConditionDescription { get; set; }
    }
    public class ApplicationDirtyStatusMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "IsDirty is required..!")]
        public bool IsDirty { get; set; }
        public DateTime? LastIsDirtyTrueOn { get; set; }
        public DateTime? LastIsDirtyFalseOn { get; set; }
    }
    public class AppActionTypeMapping
    {
        [Key]
        public int Id { get; set; }
        public int OldAppActionType { get; set; }

        [StringLength(200, ErrorMessage = "The max length of OldAppActionTypeDescription is 200 characters..!")]
        public string OldAppActionTypeDescription { get; set; }
        public int NewAppActionType { get; set; }

        [StringLength(200, ErrorMessage = "The max length of NewAppActionTypeDescription is 200 characters..!")]
        public string NewAppActionTypeDescription { get; set; }
        public int ApplicationLifeCycleStatusType { get; set; }
    }

    public class AppProcessLoadBalanceLog
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "UserProfileRefId is required..!")]
        public string UserProfileRefId { get; set; }

        [Required(ErrorMessage = "Action date is required..!")]
        public string UserRoleId { get; set; }

        [Required(ErrorMessage = "Action date is required..!")]
        public CircleTypeEnum CircleType { get; set; }

        [Required(ErrorMessage = "Action date is required..!")]
        public Int64 CircleRefId { get; set; }

        [Required(ErrorMessage = "Action date is required..!")]
        public DateTime AssignmentDate { get; set; }
    }

    public class AppActionTimeLine
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "AppActionLogRefId is required..!")]
        [ForeignKey("ApplicationActionLog")]
        public Int64 AppActionLogRefId { get; set; }
        public virtual ApplicationActionLog ApplicationActionLog { get; set; }

        [Required(ErrorMessage = "AllowedAppActionType is required..!")]
        public AppActionTypeEnum AllowedAppActionType { get; set; }

        [Required(ErrorMessage = "ActionCanTakenUpto is required..!")]
        public DateTime ActionCanTakenUpto { get; set; }

        [Required(ErrorMessage = "IsProcessed is required..!")]
        public bool IsProcessed { get; set; }

        [Required(ErrorMessage = "IsActionSuspended is required..!")]
        public bool IsActionSuspended { get; set; }

        [Required(ErrorMessage = "MaxHoursAllowed is required..!")]
        public int MaxHoursAllowed { get; set; }

        [Required(ErrorMessage = "IsDocumentUploadOption is required..!")]
        public bool IsDocumentUploadOption { get; set; }

        [Required(ErrorMessage = "IsOptional is required..!")]
        public bool IsOptional { get; set; }

        [Required(ErrorMessage = "DocRefId is required..!")]
        public Int64 DocRefId { get; set; }

        [Required(ErrorMessage = "IsAutoAction is required..!")]
        public bool IsAutoAction { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        public string UserRefId { get; set; }

        public String PrecheckCode { get; set; }


        [Required(ErrorMessage = "IsExpiredByAutoProcess is required..!")]
        public bool IsExpiredByAutoProcess { get; set; }

        [Required(ErrorMessage = "IsHidden is required..!")]
        public bool IsHidden { get; set; }

        [Required(ErrorMessage = "AppActionParallelProcessRefId is required..!")]
        [ForeignKey("ApplicationAction_ParallelProcess")]
        public Int64 AppActionParallelProcessRefId { get; set; }
        public virtual ApplicationAction_ParallelProcess ApplicationAction_ParallelProcess { get; set; }

    }

    public class AppActionTimeLineDefination
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "AppActionLogRefId is required..!")]
        [ForeignKey("ApplicationActionLog")]
        public Int64 AppActionLogRefId { get; set; }
        public virtual ApplicationActionLog ApplicationActionLog { get; set; }

        [Required(ErrorMessage = "SlabDate is required..!")]
        public DateTime SlabDate { get; set; }

        [Required(ErrorMessage = "TotalWorkingTime is required..!")]
        public double TotalWorkingTime { get; set; }

        [Required(ErrorMessage = "SlabDayType is required..!")]
        public SlabDayTypeEnum SlabDayType { get; set; }

        [Required(ErrorMessage = "SlabDayDesc is required..!")]
        public string SlabDayDesc { get; set; }

        [Required(ErrorMessage = "WorkStartsTime is required..!")]
        public DateTime WorkStartsTime { get; set; }

        [Required(ErrorMessage = "WorkEndsTime is required..!")]
        public DateTime WorkEndsTime { get; set; }

        [Required(ErrorMessage = "OrderNo is required..!")]
        public int OrderNo { get; set; } = 1;

        [Required(ErrorMessage = "AllowedAppActionType is required..!")]
        public AppActionTypeEnum AllowedAppActionType { get; set; }

        [NotMapped]
        public int Hours { get; set; }

        [NotMapped]
        public int Minutes { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        public string UserRefId { get; set; }

        public string PrecheckCode { get; set; }
    }

    public class ApplicationProcessPhaseLog
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "ApplicationProcessPhaseLogType id is required..!")]
        public ApplicationProcessPhaseLogTypeEnum ApplicationProcessPhaseLogType { get; set; }

        [Required(ErrorMessage = "LastModifiedOn id is required..!")]
        public DateTime LastModifiedOn { get; set; }

        [Required(ErrorMessage = "PhaseCounter is required..!")]
        public int PhaseCounter { get; set; } = 1;
    }

    public class ApplicationProcessPhaseAndObjectionLog
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "AppRefId id is required..!")]
        public Int64 AppRefId { get; set; }

        [Required(ErrorMessage = "ApplicationProcessPhaseLogType id is required..!")]
        public ApplicationProcessPhaseLogTypeEnum ApplicationProcessPhaseLogType { get; set; }

        [Required(ErrorMessage = "RoleRefId is required..!")]
        public string RoleRefId { get; set; }

        [Required(ErrorMessage = "AppActionLogRefId is required..!")]
        public Int64 AppActionLogRefId { get; set; }
    }

    public class ApplicationActionExtension
    {
        [Key]
        public Int64 ActionExtensionId { get; set; }

        [StringLength(450)]
        public string? Receiver_UserRefId_1 { get; set; }
        public Int64 Receiver_ProfileRefId_1 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_1 { get; set; }
        public DateTime? Receiver_ActionOn_1 { get; set; }
        public string Receiver_Remarks_1 { get; set; }

        public string? Sender_UserRefId_1 { get; set; }

        public Int64 Sender_ProfileRefId_1 { get; set; }

        public string SenderRoleId_1 { get; set; }

        public AppActionTypeEnum AppActionType_1 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_1 { get; set; }

        [StringLength(450)]
        public string Receiver_UserRefId_2 { get; set; }
        public Int64 Receiver_ProfileRefId_2 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_2 { get; set; }

        public DateTime? Receiver_ActionOn_2 { get; set; }
        public string Receiver_Remarks_2 { get; set; }

        public string? Sender_UserRefId_2 { get; set; }

        public Int64 Sender_ProfileRefId_2 { get; set; }

        public string SenderRoleId_2 { get; set; }
        public AppActionTypeEnum AppActionType_2 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_2 { get; set; }

        [StringLength(450)]
        public string Receiver_UserRefId_3 { get; set; }
        public Int64 Receiver_ProfileRefId_3 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_3 { get; set; }

        public DateTime? Receiver_ActionOn_3 { get; set; }
        public string Receiver_Remarks_3 { get; set; }

        public string? Sender_UserRefId_3 { get; set; }

        public Int64 Sender_ProfileRefId_3 { get; set; }

        public string SenderRoleId_3 { get; set; }
        public AppActionTypeEnum AppActionType_3 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_3 { get; set; }

        [StringLength(450)]
        public string Receiver_UserRefId_4 { get; set; }
        public Int64 Receiver_ProfileRefId_4 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_4 { get; set; }

        public DateTime? Receiver_ActionOn_4 { get; set; }
        public string Receiver_Remarks_4 { get; set; }

        public string? Sender_UserRefId_4 { get; set; }

        public Int64 Sender_ProfileRefId_4 { get; set; }

        public string SenderRoleId_4 { get; set; }
        public AppActionTypeEnum AppActionType_4 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_4 { get; set; }

        [StringLength(450)]
        public string Receiver_UserRefId_5 { get; set; }
        public Int64 Receiver_ProfileRefId_5 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_5 { get; set; }

        public DateTime? Receiver_ActionOn_5 { get; set; }
        public string Receiver_Remarks_5 { get; set; }

        public string? Sender_UserRefId_5 { get; set; }

        public Int64 Sender_ProfileRefId_5 { get; set; }

        public string SenderRoleId_5 { get; set; }
        public AppActionTypeEnum AppActionType_5 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_5 { get; set; }

        public Int64 AppActionRefId { get; set; }

        public string CurrentActionTakenUserRefId { get; set; }

        public AppActionTypeEnum CurrentActionTakenCode { get; set; }


        public Int64 AppDocumentRefId_1 { get; set; }
        public Int64 AppDocumentRefId_2 { get; set; }
        public Int64 AppDocumentRefId_3 { get; set; }
        public Int64 AppDocumentRefId_4 { get; set; }
        public Int64 AppDocumentRefId_5 { get; set; }

        public bool IsDocumentUploaded_1 { get; set; }
        public bool IsDocumentUploaded_2 { get; set; }
        public bool IsDocumentUploaded_3 { get; set; }
        public bool IsDocumentUploaded_4 { get; set; }
        public bool IsDocumentUploaded_5 { get; set; }

        public Int64 ActionTakenHoursCount_1 { get; set; }
        public Int64 ActionTakenHoursCount_2 { get; set; }
        public Int64 ActionTakenHoursCount_3 { get; set; }
        public Int64 ActionTakenHoursCount_4 { get; set; }
        public Int64 ActionTakenHoursCount_5 { get; set; }

        public Int64 ActionTakenDaysCount_1 { get; set; }
        public Int64 ActionTakenDaysCount_2 { get; set; }
        public Int64 ActionTakenDaysCount_3 { get; set; }
        public Int64 ActionTakenDaysCount_4 { get; set; }
        public Int64 ActionTakenDaysCount_5 { get; set; }
    }
    public class ApplicationActionExtensionLog
    {
        [Key]
        public Int64 ActionExtensionLogId { get; set; }
       
        [StringLength(450)]
        public string Receiver_UserRefId_1 { get; set; }

        public Int64 Receiver_ProfileRefId_1 { get; set; }
       
        [StringLength(450)]
        public string ReceiverRoleId_1 { get; set; }

        public DateTime? Receiver_ActionOn_1 { get; set; }
        public string Receiver_Remarks_1 { get; set; }

        public string? Sender_UserRefId_1 { get; set; }

        public Int64 Sender_ProfileRefId_1 { get; set; }

        public string SenderRoleId_1 { get; set; }

        public AppActionTypeEnum AppActionType_1 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_1 { get; set; }

        [StringLength(450)]
        public string Receiver_UserRefId_2 { get; set; }

        public Int64 Receiver_ProfileRefId_2 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_2 { get; set; }
        public DateTime? Receiver_ActionOn_2 { get; set; }
        public string Receiver_Remarks_2 { get; set; }
        public string? Sender_UserRefId_2 { get; set; }
        public Int64 Sender_ProfileRefId_2 { get; set; }
        public string SenderRoleId_2 { get; set; }
        public AppActionTypeEnum AppActionType_2 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_2 { get; set; }

        [StringLength(450)]
        public string Receiver_UserRefId_3 { get; set; }
        public Int64 Receiver_ProfileRefId_3 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_3 { get; set; }

        public DateTime? Receiver_ActionOn_3 { get; set; }
        public string Receiver_Remarks_3 { get; set; }

        public string? Sender_UserRefId_3 { get; set; }

        public Int64 Sender_ProfileRefId_3 { get; set; }

        public string SenderRoleId_3 { get; set; }
        public AppActionTypeEnum AppActionType_3 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_3 { get; set; }

        [StringLength(450)]
        public string Receiver_UserRefId_4 { get; set; }
        public Int64 Receiver_ProfileRefId_4 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_4 { get; set; }

        public DateTime? Receiver_ActionOn_4 { get; set; }
        public string Receiver_Remarks_4 { get; set; }

        public string? Sender_UserRefId_4 { get; set; }

        public Int64 Sender_ProfileRefId_4 { get; set; }

        public string SenderRoleId_4 { get; set; }
        public AppActionTypeEnum AppActionType_4 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_4 { get; set; }

        [StringLength(450)]
        public string Receiver_UserRefId_5 { get; set; }
        public Int64 Receiver_ProfileRefId_5 { get; set; }

        [StringLength(450)]
        public string ReceiverRoleId_5 { get; set; }

        public DateTime? Receiver_ActionOn_5 { get; set; }
        public string Receiver_Remarks_5 { get; set; }

        public string? Sender_UserRefId_5 { get; set; }

        public Int64 Sender_ProfileRefId_5 { get; set; }

        public string SenderRoleId_5 { get; set; }
        public AppActionTypeEnum AppActionType_5 { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType_5 { get; set; }
        public string CurrentActionTakenUserRefId { get; set; }

        public AppActionTypeEnum CurrentActionTakenCode { get; set; }

        public Int64 CurrentAppActionLogRefId { get; set; }

        public Int64 AppActionLogRefId_1 { get; set; }
        public Int64 AppActionLogRefId_2 { get; set; }
        public Int64 AppActionLogRefId_3 { get; set; }
        public Int64 AppActionLogRefId_4 { get; set; }
        public Int64 AppActionLogRefId_5 { get; set; }


        public Int64 ActionTakenHoursCount_1 { get; set; }
        public Int64 ActionTakenHoursCount_2 { get; set; }
        public Int64 ActionTakenHoursCount_3 { get; set; }
        public Int64 ActionTakenHoursCount_4 { get; set; }
        public Int64 ActionTakenHoursCount_5 { get; set; }

        public Int64 ActionTakenDaysCount_1 { get; set; }
        public Int64 ActionTakenDaysCount_2 { get; set; }
        public Int64 ActionTakenDaysCount_3 { get; set; }
        public Int64 ActionTakenDaysCount_4 { get; set; }
        public Int64 ActionTakenDaysCount_5 { get; set; }

        public bool IsDocumentUploaded_1 { get; set; }
        public bool IsDocumentUploaded_2 { get; set; }
        public bool IsDocumentUploaded_3 { get; set; }
        public bool IsDocumentUploaded_4 { get; set; }
        public bool IsDocumentUploaded_5 { get; set; }

        public Int64 AppDocumentRefId_1 { get; set; }
        public Int64 AppDocumentRefId_2 { get; set; }
        public Int64 AppDocumentRefId_3 { get; set; }
        public Int64 AppDocumentRefId_4 { get; set; }
        public Int64 AppDocumentRefId_5 { get; set; }
    }

    public class AppActionTime_MutualProcessFlag
    {
        [Key]
        public Int64 Id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "SenderUserRefId is required..!")]
        public string SenderUserRefId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "ReceiverUserRefId is required..!")]
        public string ReceiverUserRefId { get; set; }

        [Required(ErrorMessage = "ReceiverRoleId is required..!")]
        public string ReceiverRoleId { get; set; }

        [Required(ErrorMessage = "ReceiverProfileRefId is required..!")]
        public Int64 ReceiverProfileRefId { get; set; }

        [Required(ErrorMessage = "AppActionLogRefId is required..!")]
        public Int64 AppActionLogRefId { get; set; }

        [Required(ErrorMessage = "HasClosed is required..!")]
        public bool HasClosed { get; set; }

        public Int64 AppRefId { get; set; }

        //[NotMapped]
        //public string OfficerName { get; set; }

        //[NotMapped]
        //public string OfficerDesignation { get; set; }
    }

    public class TimeLine_DepartmentWiseFinalAction
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "AppRefId is required..!")]
        public Int64 AppRefId { get; set; }

        [Required(ErrorMessage = "DeptCodeType is required..!")]
        public DepartmentCodeTypeEnum DeptCodeType { get; set; }

        [Required(ErrorMessage = "AppActionType is required..!")]
        public AppActionTypeEnum AppActionType { get; set; }

        [Required(ErrorMessage = "AppActionLogRefId is required..!")]
        public Int64 AppActionLogRefId { get; set; }
    }


    public class AppEscalationMappings
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "ApplicationType is required..!")]
        public Int64 ApplicationType { get; set; }

        [Required(ErrorMessage = "PercentageSlab is required..!")]
        public int PercentageSlab { get; set; }

        [Required(ErrorMessage = "RoleIds is required..!")]
        public string RoleIds { get; set; }

    }

    public class Esclations_ProcessEngine
    {
        [Key]
        public Int64 Esclations_ProcessEngineId { get; set; }

        [Required(ErrorMessage = "TimeStemp is required..!")]
        public DateTime TimeStemp { get; set; }

        //public virtual List<AppEscalations> AppEscalations { get; set; }

    }

    public class AppEscalations
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "AppRefId is required..!")]
        public Int64 AppRefId { get; set; }

        [Required(ErrorMessage = "ApplicationType is required..!")]
        public Int32 ApplicationType { get; set; }

        [Required(ErrorMessage = "SubmissionDate is required..!")]
        public DateTime SubmissionDate { get; set; }

        [Required(ErrorMessage = "TotalDaysCount is required..!")]
        public int TotalDaysCount { get; set; }

        [Required(ErrorMessage = "TotalDaysCount is required..!")]
        public int TotalWorkingDaysCount { get; set; }

        [Required(ErrorMessage = "TotalDaysCount is required..!")]
        public int TotalHolidaysCount { get; set; }

        [Required(ErrorMessage = "TotalDaysCount is required..!")]
        public int TotalObjectionDaysCount { get; set; }

        [Required(ErrorMessage = "NetDaysCount is required..!")]
        public int NetDaysCount { get; set; }

        [Required(ErrorMessage = "ActualPercentage is required..!")]
        public decimal ActualPercentage { get; set; }

        [Required(ErrorMessage = "PercentageSlab is required..!")]
        public int PercentageSlab { get; set; }

        [Required(ErrorMessage = "MaxTATDays is required..!")]
        public int MaxTATDays { get; set; }

        [Required(ErrorMessage = "PendingWithUserId is required..!")]
        public string PendingWithUserId { get; set; }

        [Required(ErrorMessage = "IsMailNotificationSent is required..!")]
        public int IsMailNotificationSent { get; set; }

        [Required(ErrorMessage = "IsSMSNotificationSent is required..!")]
        public int IsSMSNotificationSent { get; set; }

        [Required(ErrorMessage = "IsMailNotificationSent_Self is required..!")]
        public int IsMailNotificationSent_Self { get; set; }

        [Required(ErrorMessage = "IsSMSNotificationSent_Self is required..!")]
        public int IsSMSNotificationSent_Self { get; set; }

        [Required(ErrorMessage = "FactoryCircleRefId is required..!")]
        public Int64 FactoryCircleRefId { get; set; }

        [Required(ErrorMessage = "ALCCircleRefId is required..!")]
        public Int64 ALCCircleRefId { get; set; }

        [Required(ErrorMessage = "LabourCircleRefId is required..!")]
        public Int64 LabourCircleRefId { get; set; }

        [Required(ErrorMessage = "EscalatedUsers is required..!")]
        public string EscalatedUsers { get; set; }

        [Required(ErrorMessage = "EscalationProcessEngineRefId is required..!")]
        //[ForeignKey("Esclations_ProcessEngine")]
        public Int64 EscalationProcessEngineRefId { get; set; }

    }

    public class AppActivationLogs
    {
        [Key]
        public Int64 AppActivationLogId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public ApplicationActivationStatusTypeEnum ApplicationStatus { get; set; }

        public DateTime ActionDate { get; set; }

        [Required(ErrorMessage = "AppActionLogRefId is required..!")]
        public Int64 AppActionLogRefId { get; set; }
    }


    public class ApplicationAction_ParallelProcess
    {
        [Key]
        public Int64 AppActionParallelProcessId { get; set; }

        [Required(ErrorMessage = "Application action is required..!")]
        public int AppActionType { get; set; }

        [Required(ErrorMessage = "Sender user ref id is required..!")]
        [StringLength(450)]
        [ForeignKey("User_Sender")]
        public string Sender_UserRefId { get; set; }
        public virtual User ApplicationActions_ParallelProcess_User_Sender { get; set; }

        [Required(ErrorMessage = "Sender profile ref id is required..!")]
        [ForeignKey("Profile_Sender")]
        public Int64 Sender_ProfileRefId { get; set; }
        public virtual UserProfile ApplicationActions_ParallelProcess_Profile_Senders { get; set; }
        [Required(ErrorMessage = "Receiver user ref id is required..!")]
        [StringLength(450)]
        [ForeignKey("User_Receiver")]
        public string Receiver_UserRefId { get; set; }
        public virtual User ApplicationActions_ParallelProcess_User_Receivers { get; set; }

        [Required(ErrorMessage = "Receiver profile ref id is required..!")]
        [ForeignKey("Profile_Receiver")]
        public Int64 Receiver_ProfileRefId { get; set; }
        public virtual UserProfile ApplicationActions_ParallelProcess_Profile_Receiver { get; set; }

        [Required(ErrorMessage = "Action date is required..!")]
        public DateTime ActionDate { get; set; }

        [Required(ErrorMessage = "Action taken days count is required..!")]
        public Int64 ActionTakenDaysCount { get; set; }

        [Required(ErrorMessage = "Action taken hours count is required..!")]
        public Int64 ActionTakenHoursCount { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        //[StringLength(500, ErrorMessage = "The max length of remarks is 500 characters..!")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Sender role id is required..!")]
        [StringLength(450)]
        public string SenderRoleId { get; set; }

        [Required(ErrorMessage = "Receiver role id is required..!")]
        [StringLength(450)]
        public string ReceiverRoleId { get; set; }

        public string Checklist_Json { get; set; }

        [Required]
        public bool Checklist_IsAllAgreed { get; set; }

        [Required]
        public int Checklist_FieldObjections { get; set; }

        [Required]
        public int Checklist_DocObjections { get; set; }

        [Required]
        public bool IsDocumentUploaded { get; set; }

        [Required]
        public Int64 AppDocumentRefId { get; set; }

        [Required(ErrorMessage = "IP address is required..!")]
        public string IpAddress { get; set; }

        //public bool Legacy_IsMigrated { get; set; }
        //public Int64 Legacy_AppId { get; set; }
        //public Int64 Legacy_AppFormId { get; set; }
        //public string Legacy_NAR { get; set; }
        //public int Legacy_StatusId { get; set; }

        [NotMapped]
        public string ActionPublicName { get; set; }

        [Required(ErrorMessage = "Latitude is required..!")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required..!")]
        public string Longitude { get; set; }
        public ActionTakenModeTypeEnum ActionTakenModeType { get; set; }

        [Required(ErrorMessage = "IsDeleted is required..!")]
        public bool IsDeleted { get; set; } = false;

        [Required(ErrorMessage = "IsAlive is required..!")]
        public bool IsAlive { get; set; }

        #region ForeignKeyReferences

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 ApplicationRefId { get; set; }
        public virtual Application Application { get; set; }

        public virtual ICollection<AppActionTimeLine> AppActionTimeLines { get; set; }
        #endregion ForeignKeyReferences
    }
}
