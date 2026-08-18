using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static pbsamadhannetcoreapi.Models.LWB_LIdAndUnpaidWages;
using static pbsamadhannetcoreapi.ViewModels.UserLoginDetailsViewModel;

namespace pbsamadhannetcoreapi.Models
{
    public class AppDbContext : IdentityDbContext<User, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        //public DbSet<User> Users { get; set; }
        public DbSet<DistrictLgd> Districts { get; set; }
        public DbSet<TehsilLgd> Tehsils { get; set; }
        public DbSet<Establishment_GeneralDetail> Establishments_GeneralDetail { get; set; }
        public DbSet<Establishment_EmployerDetail> Establishments_EmployerDetail { get; set; }
        public DbSet<Establishment_ContractorDetail> Establishments_ContractorDetail { get; set; }
        public DbSet<Contractor_GeneralDetail> Contractors_GeneralDetail { get; set; }
        public DbSet<Contractor_PrincipalEmployer> Contractors_PrincipalEmployer { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationAction> ApplicationActions { get; set; }
        public DbSet<ApplicationActionLog> ApplicationActionLogs { get; set; }
        //public DbSet<BusinessEntity> BusinessEntities { get; set; }
        public DbSet<ProjectSite> ProjectSites { get; set; }
        public DbSet<DigitalSignature> DigitalSignatures { get; set; }
        
        public DbSet<Document> Documents { get; set; }
        public DbSet<ApplicationDocument> ApplicationDocuments { get; set; }
        public DbSet<ApplicationDocument_Log> ApplicationDocument_Logs { get; set; }

        public DbSet<DigitalSignature_Log> DigitalSignature_Logs { get; set; }

        [NotMapped]
        public DbSet<DashboardViewModel> DashboardViewModel { get; set; }

        [NotMapped]
        public DbSet<DashboardAlreadyClearenceViewModel> DashboardAlreadyClearenceViewModel { get; set; }
        public DbSet<AppTypeAllowedDocument> AppTypeAllowedDocuments { get; set; }

        [NotMapped]
        public DbSet<AppFileUploadInfoViewModel> AppFileUploadInfoViewModel { get; set; }

        public DbSet<BP_GeneralDetail> BP_GeneralDetails { get; set; }
        public DbSet<FeesHeader> FeesHeaders { get; set; }
        public DbSet<AppFeeDetail> AppFeeDetails { get; set; }

        public DbSet<UserRegisteredMobileAppDevice> UserRegisteredMobileAppDevices { get; set; }
        public DbSet<FactoryCircle> FactoryCircles { get; set; }
        public DbSet<FactoryCircleTreasuryMapping> FactoryCircleTreasuryMappings { get; set; }
        public DbSet<ALCCircle> ALCCircles { get; set; }
        public DbSet<LabourCircle> LabourCircles { get; set; }
        public DbSet<ALCCircleTreasuryMapping> ALCCircleTreasuryMappings { get; set; }
        public DbSet<LabourCircleTreasuryMapping> LabourCircleTreasuryMappings { get; set; }
        public DbSet<AppFeeTransaction> AppFeeTransactions { get; set; }
        public DbSet<ApplicationCircleMapping> ApplicationCircleMappings { get; set; }
        public DbSet<Establishment_EPFO_Logs> Establishment_EPFO_Logs { get; set; }

        [NotMapped]
        public DbSet<MajorDashboardCountViewModel> MajorDashboardCount { get; set; }

        [NotMapped]
        public DbSet<RecordTypeCountViewModel> RecordTypeCount { get; set; }
        [NotMapped]
        public DbSet<RecordTypeMenuCountViewModel> RecordTypeMenuCount { get; set; }
        [NotMapped]
        public DbSet<RecordsTypeListViewModel> RecordsTypeList { get; set; }

        [NotMapped]
        public DbSet<ProjectProfileViewModel> ProjectProfiles { get; set; }


        [NotMapped]
        public DbSet<GetApplciationLogsViewModel> GetApplciationLogsViewModel { get; set; }

        [NotMapped]
        public DbSet<Establishment_EPFO_Report> Establishment_EPFO_Reports { get; set; }

        [NotMapped]
        public DbSet<CircleManagerViewModel> CircleManager { get; set; }

        [NotMapped]
        public DbSet<LabourCircleViewModel> LabourCircle { get; set; }
        [NotMapped]
        public DbSet<DistrictViewModel> DistrictViewModels { get; set; }

        [NotMapped]
        public DbSet<ServiceListViewModel> ServiceListViewModel { get; set; }

        [NotMapped]
        public DbSet<ProcessApplicationViewModels> ProcessApplication{ get; set; }
        [NotMapped]
        public DbSet<MobileAppDeviceResponseViewModel> MobileAppDeviceResponse { get; set; }
        public DbSet<RoleRelationMapping> RoleRelationMapping { get; set; }
        public DbSet<ApplicationActionCode> ApplicationActionCodes { get; set; }

        [NotMapped]
        public DbSet<ProcessApplicationUsersViewModel> ProcessApplicationUsers { get; set; }

        [NotMapped]
        public DbSet<NotingLogsViewModel> NotingLogs { get; set; }
        
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserCircleMapping> UserCircleMappings { get; set; }

        [NotMapped]
        public DbSet<ApplicationLogsViewModel> ApplicationLogs { get; set; }
        [NotMapped]
        public DbSet<LendingOfficerDetailsViewModel> LendingOfficerDetails { get; set; }

        [NotMapped]
        public DbSet<CertificateGenerateServiceResultTemplate> CertificateGenerateServiceResultTemplate { get; set; }

        [NotMapped]
        public DbSet<ProjectSitesViewModel> ProjectSitesViewModel { get; set; }

        public DbSet<CommonLicence_GeneralDetail> CommonLicence_GeneralDetails { get; set; }
        public DbSet<CommonLicence_ContractorDetail> CommonLicence_ContractorDetails { get; set; }
        public DbSet<CommonLicences_SelectedLicenceMapping> CommonLicences_SelectedLicenceMappings { get; set; }
        [NotMapped]
        public DbSet<AppFeeHeadersAndTreasuryHeadsInfoViewModel> AppFeeHeadersAndTreasuryHeadsInfo { get; set; }

        public DbSet<RegistrationDetail> RegistrationDetails { get; set; }
        [NotMapped]
        public DbSet<AppDDOCodesInfoViewModel> AppDDOCodesInfo { get; set; }
        public DbSet<AppPaymentSuccessTransactionMapping> AppPaymentSuccessTransactionMappings { get; set; }
        public DbSet<ContractLabourAndEstablishmentMapping> ContractLabourAndEstablishmentMappings { get; set; }
        public DbSet<TransactionalStoreProcedureResponseViewModel> TransactionalStoreProcedureResponse { get; set; }
        public DbSet<NICCode> NICCodes { get; set; }

        public DbSet<BuildingPlan> BuildingPlans { get; set; }
        public DbSet<BuildingPlan_AreaDetail> BuildingPlan_AreaDetails { get; set; }

        [NotMapped]
        public DbSet<DisplayPaymentViewModel> DisplayPaymentViewModel { get; set; }
        [NotMapped]
        public DbSet<UserFullDetailsViewModel> UserFullDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<ApplicationFullDetailsViewModel> ApplicationFullDetails { get; set; }

        [NotMapped]
        public DbSet<ContractLabourPdfDetailsViewModel> ContractLabourPdfDetails { get; set; }

        [NotMapped]
        public DbSet<RoleWiseAllowedActionCodeViewModel> RoleWiseAllowedActionCode { get; set; }

        [NotMapped]
        public DbSet<BuildingPlanHudPdfDetailsViewModel> BuildingPlanHudPdfDetails { get; set; }

        [NotMapped]
        public DbSet<ShopLicencePdfDetailsViewModel> ShopLicencePdfDetails { get; set; }

        [NotMapped]
        public DbSet<ProjectSiteViewModel> ProjectSiteViewModel { get; set; }

        [NotMapped]
        public DbSet<EstablishmentCardInfoViewModel> EstablishmentCardInfo { get; set; }

        [NotMapped]
        public DbSet<InvestPunjabShareStatusParmsViewModel> InvestPunjabShareStatusParms { get; set; }

        [NotMapped]
        public DbSet<DataTableParamsViewModel> DataTableParams { get; set; }

        [NotMapped]
        public DbSet<PrincipalApproval_RBA_DetailsViewModel> PrincipalApproval_RBA_DetailsViewModel { get; set; }
        
        [NotMapped]
        public DbSet<PrincipalApprovalData> PrincipalApprovalData { get; set; }

        [NotMapped]
        public DbSet<TransferUserInfoViewModel> TransferUserInfoViewModel { get; set; }

        [NotMapped]
        public DbSet<GenerateLicenceNoViewModel> GenerateLicenceNo { get; set; }
        public DbSet<BuildingPlanHUD_GeneralDetail> BuildingPlanHUD_GeneralDetails { get; set; }
        public DbSet<BusinessFirst_RequestLog> BusinessFirst_RequestLogs { get; set; }
        public DbSet<DistrictLevelUserMapping> DistrictLevelUserMappings { get; set; }
        public DbSet<BuildingPlanHUDPaymentDetail> BuildingPlanHUDPaymentDetails { get; set; }
        public DbSet<RoleWiseAllowedActionCode> RoleWiseAllowedActionCodes { get; set; }
        public DbSet<BusinessFirstShareStatusLog> BusinessFirstShareStatusLogs { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProfileMapping> UserProfileMapping { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        // public DbSet<Circle> Circles { get; set; }


        public DbSet<Licence_TradeUnion> Licence_TradeUnion { get; set; }

        public DbSet<Licence_TradeUnion_AmendmentDataHistories> Licence_TradeUnion_AmendmentDataHistories { get; set; }

        public DbSet<Licence_TradeUnion_Officer> Licence_TradeUnion_Officer { get; set; }

        public DbSet<ShopLicence_GeneralDetail> ShopLicence_GeneralDetails { get; set; }
        public DbSet<ShopLicence_EmployeeDetail> ShopLicence_EmployeeDetails { get; set; }

        public DbSet<Licence_BocwAct_GeneralDetail> Licence_BocwAct_GeneralDetails { get; set; }

        public DbSet<Licence_BocwAct_ContractorDetail> Licence_BocwAct_ContractorDetails { get; set; }
        public DbSet<ApplicationLicenceNoMapping> ApplicationLicenceNoMapping { get; set; }

        public DbSet<LegacyApprovedClearenceMapping> LegacyApprovedClearenceMappings { get; set; }
        public DbSet<ProjectSite_IpinMapping> ProjectSite_IpinMappings { get; set; }

        public DbSet<BuildingPlanHUD_RTB_Mapping> BuildingPlanHUD_RTB_Mappings { get; set; }

        public DbSet<BuildingPlanFactory_GeneralDetail> BuildingPlanFactory_GeneralDetails { get; set; }

        public DbSet<ApplicationAddendumDocument> ApplicationAddendumDocuments { get; set; }

        [NotMapped]
        public DbSet<SenderReceiverDetailViewModal> SenderReceiverDetail { get; set; }

        [NotMapped]
        public DbSet<OfficerDetailsByRoleNameViewModel> OfficerDetailsByRoleName { get; set; }

        [NotMapped]
        public DbSet<GetlastClearanceViewModel> GetlastClearance { get; set; }

        public DbSet<StatusManagerResponseParmsViewModel> StatusManagerResponseParms { get; set; }
        [NotMapped]
        public DbSet<UsersIdWithAppIdParmsViewModel> UsersIdWithAppIdParmsViewModel { get; set; }
        public DbSet<AppPaymentPart> AppPaymentParts { get; set; }

        public DbSet<Deemed_ProcessEngineLog> Deemed_ProcessEngineLogs { get; set; }
        public DbSet<Deemed_ProcessFilesLog> Deemed_ProcessFilesLogs { get; set; }

        public DbSet<All_Act_Deemed_ProcessEngineLogs> All_Act_Deemed_ProcessEngineLogs { get; set; }
        public DbSet<All_Act_Deemed_ProcessFilesLogs> All_Act_Deemed_ProcessFilesLogs { get; set; }

        public DbSet<AutoApprove_ProcessEngineLog> AutoApprove_ProcessEngineLogs { get; set; }
        public DbSet<AutoApprove_ProcessFilesLog> AutoApprove_ProcessFilesLogs { get; set; }

        [NotMapped]
        public DbSet<PSLDashboardMajorCountBP_HUD_ViewModel> PSLDashboardMajorCountBP_HUD { get; set; }
        [NotMapped]
        public DbSet<ShopEmployeeDetailViewModel> ShopEmployeeDetailViewModel { get; set; }
        [NotMapped]
        public DbSet<RegisteredMobileNumberHintRespViewModel> RegisteredMobileNumberHintByUsername { get; set; }
        [NotMapped]

        public DbSet<BocwContractorListViewModel> BocwContractorListViewModel { get; set; }
        [NotMapped]
        public DbSet<DeviceRegistrationQRCodeViewModel> DeviceRegistrationQRCode { get; set; }

        public DbSet<LWB_Contribution> LWB_Contributions { get; set; }
        public DbSet<LWB_Contribution_Employee> LWB_Contribution_Employees { get; set; }
        public DbSet<LWB_Lid> LWB_Lids { get; set; }
        public DbSet<LWB_LIdAndFund> LWB_LIdAndFunds { get; set; }

        public DbSet<Licence_Shop_NightShift_ChecklistPoint> Licence_Shop_NightShift_ChecklistPoints { get; set; }
        public DbSet<Licence_Shop_NightShift_Approval> Licence_Shop_NightShift_Approvals { get; set; }
        public DbSet<Licence_Factory_NightShift_Approval> Licence_Factory_NightShift_Approvals { get; set; }
        [NotMapped]
        public DbSet<GetTotalTakenTimeByDepartmentViewModel> GetTotalTakenTimeByDepartment { get; set; }
        public DbSet<AppPaymentEDCAuthority> AppPaymentEDCAuthorities { get; set; }
        public DbSet<BuildingPlanHUDPaymentDetail_log> BuildingPlanHUDPaymentDetail_logs { get; set; }
        public DbSet<Licence_Factory_GeneralDetail> Licence_Factory_GeneralDetails { get; set; }
        public DbSet<Licence_Factory_OccupierAndManagerDetail> Licence_Factory_OccupierAndManagerDetails { get; set; }
        public DbSet<Licence_Factory_AmendmentDataHistory> Licence_Factory_AmendmentDataHistories { get; set; }
        public DbSet<Licence_Factory_QuestionnaireDetail> Licence_Factory_QuestionnaireDetails { get; set; }
        public DbSet<Licence_Factory_AdditionalDetail> Licence_Factory_AdditionalDetails { get; set; }
        public DbSet<AuditLog_Factory_Backlog> AuditLog_Factory_Backlogs { get; set; }

        [NotMapped]
        public DbSet<LicenceNoViewModel> LicenceNo { get; set; }
        public DbSet<DofLicenceDetailsViewModel> DofLicenceDetails { get; set; }
        public DbSet<AadharVerificationReqLog> AadharVerificationReqLogs { get; set; }
        public DbSet<CircleUpgradationHistoryLog> CircleUpgradationHistoryLogs { get; set; }
        [NotMapped]
        public DbSet<LatestCircleInfoViewModel> LatestCircleInfoViewModel { get; set; }
        public DbSet<Licence_ContractLabour_GeneralDetail> Licence_ContractLabour_GeneralDetails { get; set; }
        public DbSet<Licence_ContractLabour_AmendmentDataHistory> Licence_ContractLabour_AmendmentDataHistories { get; set; }
        [NotMapped]
        public DbSet<DeviceUniqueIdAndUserRefIdViewModel> DeviceUniqueIdAndUserRefIdViewModel { get; set; }
        [NotMapped]
        public DbSet<UserDeviceFoundRespViewModel> UserDeviceFoundRespViewModel { get; set; }
        [NotMapped]
        public DbSet<FormHViewModel> FormHViewModel { get; set; }

        [NotMapped]
        public DbSet<AppFeePaymentReceiptDataViewModel> AppFeePaymentReceiptDataViewModel { get; set; }

        [NotMapped]
        public DbSet<TransferFactoryCircleUserInfoViewModel> TransferFactoryCircleUserInfoViewModel { get; set; }

        [NotMapped]
        public DbSet<TransferALCCircleUserInfoViewModel> TransferALCCircleUserInfoViewModel { get; set; }

        [NotMapped]
        public DbSet<PaymentDetailsListViewModel> PaymentDetailsListViewModel { get; set; }

        [NotMapped]
        public DbSet<FindUserOfSameCircleViewModel> FindUserOfSameCircleViewModel { get; set; }
        [NotMapped]
        public DbSet<ApplicationAdditionalDetailsViewModel> ApplicationAdditionalDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<WelfareFundDetailsViewModel> WelfareFundDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<AnnualReturnDetailsViewModel> AnnualReturnDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<OtherActClearancesDataViewModel> OtherActClearancesDataViewModel { get; set; }


        [NotMapped]

        public DbSet<FactoryBacklogDataViewModel> FactoryBacklogDataViewModel { get; set; }
        [NotMapped]
        public DbSet<FactoryBacklogDataRequestViewModel> FactoryBacklogDataRequestViewModel { get; set; }

        [NotMapped]
        public DbSet<SysNApplicationDetailsViewModel> SysNApplicationDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<GetSysNCurrentStatusViewModel> GetSysNCurrentStatusViewModel { get; set; }
        [NotMapped]

        public DbSet<GetSysNClearancesIssuedsViewModel> GetSysNClearancesIssuedsViewModel { get; set; }
        
        [NotMapped]
        public DbSet<PSLDashboardCircleWiseCountViewModel> PSLDashboardCircleWiseCountViewModel { get; set; }

        [NotMapped]
        public DbSet<Inspection_RandomizeDashboardDataViewModel> Inspection_RandomizationViewModal { get; set; }

        [NotMapped]
        public DbSet<InspectioninfoViewModel> InspectioninfoViewModel { get; set; }

        [NotMapped]
        public DbSet<LabourCircleUserDetailsViewModel> LabourCircleUserDetailsViewModel { get; set; }

        public DbSet<MappedBuildingPlanDetailsViewModel> MappedBuildingPlanDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<FactoryInspectionDetailsViewModel> FactoryInspectionDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<GetBuildingPlanDetailsViewModel> GetBuildingPlanDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<GetApplicationNotingLogsViewModel> GetApplicationNotingLogsViewModel { get; set; }
        [NotMapped]
        public DbSet<ApplicationFeeDetailsViewModel> ApplicationFeeDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<GetApplicationDetailsViewModel> GetApplicationDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<FactoryBacklogCountViewModel> FactoryBacklogCountViewModel { get; set; }

        [NotMapped]
        public DbSet<FactoryBacklogExcelDataViewModel> FactoryBacklogExcelDataViewModel { get; set; }
        [NotMapped]
        public DbSet<GetLicenceNumberAndActTypeViewModel> GetLicenceNumberAndActTypeViewModel { get; set; }
        public DbSet<SyncStatus_ProcessEngineLog> SyncStatus_ProcessEngineLogs { get; set; }
        public DbSet<Licence_Factory_NightShift_ChecklistPoint> Licence_Factory_NightShift_ChecklistPoints { get; set; }
        public DbSet<BusinessFirst_ApprovedFileSeeding> BusinessFirst_ApprovedFileSeedings { get; set; } 
        public DbSet<ShopLicenceDetailsForNightShiftPdfViewModel> ShopLicenceDetailsForNightShiftPdfViewModel { get; set; }
        public DbSet<Inspection_Randomization> Inspection_Randomizations { get; set; }
        public DbSet<Inspection_Master> Inspection_Master { get; set; }
        public DbSet<Inspection_Form_Factory_Part_I_General> Inspection_Form_Factory_Part_I_General { get; set; }
        public DbSet<Inspection_Form_Factory_Part_II_FactoryDetail> Inspection_Form_Factory_Part_II_FactoryDetail { get; set; }
        public DbSet<Inspection_Form_Factory_Part_III_Safety> Inspection_Form_Factory_Part_III_Safety { get; set; }
        public DbSet<Inspection_Form_Factory_Part_III_InspectionReport> Inspection_Form_Factory_Part_III_InspectionReports { get; set; }
        public DbSet<Inspection_Form_Factory_Part_III_MusterRoll> Inspection_Form_Factory_Part_III_MusterRolls { get; set; }
        public DbSet<Inspection_Form_Factory_Part_III_Health> Inspection_Form_Factory_Part_III_Health { get; set; }
        public DbSet<Inspection_Form_Factory_Part_III_Welfare> Inspection_Form_Factory_Part_III_Welfare { get; set; }
        public DbSet<Inspection_Form_Factory_Part_III_General> Inspection_Form_Factory_Part_III_General { get; set; }
        public DbSet<Inspection_Form_Factory_Part_III_MajorAccidentHazard> Inspection_Form_Factory_Part_III_MajorAccidentHazard { get; set; }
        public DbSet<Inspection_Form_Factory_Part_III_DangerousOperation> Inspection_Form_Factory_Part_III_DangerousOperation { get; set; }
        public DbSet<FactoryLicenceDetailsForNightShiftPdfViewModel> FactoryLicenceDetailsForNightShiftPdfViewModel { get; set; }

        [NotMapped]
        public DbSet<GetInspections_FactoryPerformaStepStatusViewModel> GetInspections_FactoryPerformaStepStatusViewModel { get; set; }
        [NotMapped]
        public DbSet<Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel> Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel { get; set; }


        public DbSet<Inspection_Form_Labour_Part_I_General> Inspection_Form_Labour_Part_I_General { get; set; }
        public DbSet<Inspection_Form_Labour_Part_II_FactoryDetail> Inspection_Form_Labour_Part_II_FactoryDetail { get; set; }
        public DbSet<Inspection_Form_Labour_Part_III_MusterRoll> Inspection_Form_Labour_Part_III_MusterRoll { get; set; }
        public DbSet<Inspection_Form_Labour_Part_III_EqualEnumerationAct> Inspection_Form_Labour_Part_III_EqualEnumerationAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_MinimumWageAct> Inspection_Form_Labour_III_MinimumWageAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_PaymentWagesAct> Inspection_Form_Labour_III_PaymentWagesAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport> Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport { get; set; }
        public DbSet<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment> Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment { get; set; }
        public DbSet<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct> Inspection_Form_Labour_III_ChildAndAdolescentLabourAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_NationalAndFestivalHolidays> Inspection_Form_Labour_III_NationalAndFestivalHolidays { get; set; }
        public DbSet<Inspection_Form_Labour_III_MaternityBenefitAct> Inspection_Form_Labour_III_MaternityBenefitAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_ContractLabourAct> Inspection_Form_Labour_III_ContractLabourAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct> Inspection_Form_Labour_III_InterStateMigrantWorkmenAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_LabourWelfareFund_Act> Inspection_Form_Labour_III_LabourWelfareFund_Act { get; set; }
        public DbSet<Inspection_Form_Labour_III_GratuityAct> Inspection_Form_Labour_III_GratuityAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_IndustrialEmploymentAct> Inspection_Form_Labour_III_IndustrialEmploymentAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_BOCW_Act> Inspection_Form_Labour_III_BOCW_Act { get; set; }
        public DbSet<Inspection_Form_Labour_III_ShopAct> Inspection_Form_Labour_III_ShopAct { get; set; }
        public DbSet<Inspection_Form_Labour_III_Observations> Inspection_Form_Labour_III_Observations { get; set; }
        public DbSet<Inspection_ViolationComplianceReminder> Inspection_ViolationComplianceReminders { get; set; }

        [NotMapped]
        public DbSet<InspectionEstablishmentBasicDetailsViewModel> InspectionEstablishmentBasicDetailsViewModel { get; set; }

        //public DbSet<Inspection_LockInfo> Inspection_LockInfo { get; set; }

        //public DbSet<SyncStatus_ProcessEngineLog> SyncStatus_ProcessEngineLogs { get; set; }

        public DbSet<MPR_Factory> MPR_Factories { get; set; }
        
        [NotMapped]
        public DbSet<MPR_Factory_DashboardViewModel> MPR_Factory_DashboardViewModel { get; set; }
        
        public DbSet<BuildingPlanFactory_Declaration_Stability_Certificate> BuildingPlanFactory_Declaration_Stability_Certificates { get; set; }

        [NotMapped]
        public DbSet<ActAndApplicationPurposeTypeCountsViewModel> ActAndApplicationPurposeTypeCountsViewModel { get; set; }

        [NotMapped]
        public DbSet<CircleAndApplicationPurposeTypeCountsViewModel> CircleAndApplicationPurposeTypeCountsViewModel { get; set; }

        [NotMapped]
        public DbSet<DesignationAndApplicationPurposeTypeCountsViewModel> DesignationAndApplicationPurposeTypeCountsViewModel { get; set; }

        [NotMapped]
        public DbSet<FileWiseDataViewModel> FileWiseDataViewModel { get; set; }

        [NotMapped]
        public DbSet<ProfileAndApplicationPurposeTypeCountsViewModel> ProfileAndApplicationPurposeTypeCountsViewModel { get; set; }

        [NotMapped]
        public DbSet<OfficerMISDashboardDetailsByRoleNameViewModel> OfficerMISDashboardDetailsByRoleNameViewModel { get; set; }

        [NotMapped]
        public DbSet<OfficialsRoleViewModel> OfficialsRoleViewModel { get; set; }

        [NotMapped]
        public DbSet<OfficerProfileAndActWiseDashboardDetailsViewModel> OfficerProfileAndActWiseDashboardDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<OfficerProfileAndCircleWiseDashboardDetailsViewModel> OfficerProfileAndCircleWiseDashboardDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<CurrentlyDesignatedOfficerDetailsViewModel> CurrentlyDesignatedOfficerDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<MISDashboard_FileWiseData_AppSearchParamsViewModel> MISDashboard_FileWiseData_AppSearchParamsViewModel { get; set; }

        [NotMapped]
        public DbSet<LabourWelfareSchemeApplicationCountsViewModel> LabourWelfareSchemeApplicationCountsViewModel { get; set; }

        [NotMapped]
        public DbSet<LabourWelfareSchemeApplicationParmsViewModel> LabourWelfareSchemeApplicationParmsViewModel { get; set; }

        public DbSet<MISDashboardEngineStatus> MISDashboardEngineStatus { get; set; }
        public DbSet<Establishment_EPFO> Establishment_EPFO { get; set; }

        [NotMapped]
        public DbSet<CheckAlreadyInProcessViewTypeViewModel> CheckAlreadyInProcessViewTypeViewModel { get; set; }

        [NotMapped]
        public DbSet<GetRedirectUrlViewModel> GetRedirectUrlViewModel { get; set; }

        [NotMapped]
        public DbSet<GenerateDisputeNoViewModel> GenerateDisputeNo { get; set; }


        [NotMapped]
        public DbSet<StabiltyAcknoweldgementReceiptViewModel> StabiltyAcknoweldgementReceiptViewModel { get; set; }

        public DbSet<ApplicationDisputeNoMapping> ApplicationDisputeNoMapping { get; set; }

        public DbSet<UserActivityLog> UserActivityLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public DbSet<RawTokenSerializedDataViewModel> RawTokenSerializedDataViewModel { get; set; }
        [NotMapped]
        public DbSet<AuthTokenSerializedDataViewModel> AuthTokenSerializedDataViewModel { get; set; }
        
        [NotMapped]
        public DbSet<LWBApplicationDetailsViewModel> LWBApplicationDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<InspectionFactoryDetailsViewModel> InspectionFactoryDetailsViewModel { get; set; }
        
        [NotMapped]
        public DbSet<InformationViewModel> InformationViewModel { get; set; }

        [NotMapped]
        public DbSet<PaymentDetailsByLicenceNoViewModel> PaymentDetailsByLicenceNoViewModel { get; set; }

        [NotMapped]
        public DbSet<GetShareStatusRequestDetailsViewModel> GetShareStatusRequestDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<GetFactoryCirlceViewModel> GetFactoryCirlceViewModel { get; set; }
        
        [NotMapped]
        public DbSet<InspectionTransferInfoViewModel> InspectionTransferInfoViewModel { get; set; }

        public DbSet<InspectionTransferLogs> InspectionTransferLogs { get; set; }

        [NotMapped]
        public DbSet<MisInspectionDashboardDataViewModel> MisInspectionDashboardDataViewModel { get; set; }
        [NotMapped]
        public DbSet<InspectionApplicationinfoViewModel> InspectionApplicationinfoViewModel { get; set; }

        public DbSet<ApplicationDirtyStatusMapping> ApplicationDirtyStatusMappings { get; set; }

        [NotMapped]
        public DbSet<GetAllDirtyApplicationsViewModel> GetAllDirtyApplicationsViewModel { get; set; }


        [NotMapped]
        public DbSet<AppSubmissionResponseViewModel> AppSubmissionResponseViewModel { get; set; }


        [NotMapped]
        public DbSet<LegacyUserDetailsViewModel> LegacyUserDetailsViewModel { get; set; }
        public DbSet<SharedIsDirtyFlagRecordLog> SharedIsDirtyFlagRecordLogs { get; set; }
        public DbSet<MISDashboardData> MISDashboardData { get; set; }
        public DbSet<Licence_MotorTransport> Licence_MotorTransport { get; set; }

        public DbSet<Licence_CL_PE_GeneralDetail> Licence_CL_PE_GeneralDetail { get; set; }
        public DbSet<Licence_CL_PE_Contrator> Licence_CL_PE_Contrator { get; set; }

        public DbSet<Licence_PE_AmendmentDataHistories> Licence_PE_AmendmentDataHistories { get; set; }
        [NotMapped]
        public DbSet<AdhaarVerifyViewModel> AdhaarVerifyViewModel { get; set; }

        public DbSet<DesignatedOfficerDetailViewModel> DesignatedOfficerDetailViewModel { get; set; }

        public DbSet<Licence_Proposed_BuildingPlan_GeneralDetail> Licence_Proposed_BuildingPlan_GeneralDetails { get; set; }
        public DbSet<Licence_Existing_BuildingPlan_GeneralDetail> Licence_Existing_BuildingPlan_GeneralDetails { get; set; }
        public DbSet<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> Licence_Addition_Amendment_BuildingPlan_GeneralDetails { get; set; }
        public DbSet<LegacyDocumentMapping> LegacyDocumentMappings { get; set; }
        public DbSet<AppActionTypeMapping> AppActionTypeMappings { get; set; }
        public DbSet<ApplicationSeedingLog> ApplicationSeedingLogs { get; set; }

        [NotMapped]
        public DbSet<LWFMonthWiseContributionViewModel> LWFMonthWiseContributionViewModel { get; set; }

        public DbSet<Licence_CL_PE_GeneralDetail_ViewModel> Licence_CL_PE_GeneralDetail_ViewModel { get; set; }
        
        public DbSet<GetlastTerminationDateViewModel> GetlastTerminationDate { get; set; }

        [NotMapped]
        public DbSet<LWFEmployeeContributionViewModel> LWFEmployeeContributionViewModel { get; set; }

        public DbSet<ToDoApplicationActivityMaping> ToDoApplicationActivityMapings { get; set; }
        public DbSet<ToDoActivityLog> ToDoActivityLogs { get; set; }
        public DbSet<TestMaster> TestMasters { get; set; }

        [NotMapped]
        public DbSet<ToDoActivityLogViewModel> ToDoActivityLogViewModel { get; set; }

        [NotMapped]
        public DbSet<ToDoUserWiseActivityViewModel> ToDoUserWiseActivityViewModel { get; set; }

        [NotMapped]
        public DbSet<ToDoActivityWiseStepViewModel> ToDoActivityWiseStepViewModel { get; set; }

        [NotMapped]
        public DbSet<ToDoTicketDetailViewModel> ToDoTicketDetailViewModel { get; set; }

        public DbSet<ToDoApplicationWiseManpowerMapping> ToDoApplicationWiseManpowerMappings { get; set; }
        
        public DbSet<ToDoTicket> ToDoTickets { get; set; }
        
        [NotMapped]
        public DbSet<CaptchaCodeViewModel> CaptchaCodeViewModel { get; set; }

        [NotMapped]
        public DbSet<CaptchaResultViewModel> CaptchaResultViewModel { get; set; }

        public DbSet<Licence_Motor_Transport_AmendmentDataHistories> Licence_Motor_Transport_AmendmentDataHistories { get; set; }

        [NotMapped]
        public DbSet<ALCCircleManagerViewModel> AlcCircleManager { get; set; }
        public DbSet<GetAppActionDocumentsViewModel> GetAppActionDocumentsViewModel { get; set; }

        [NotMapped]
        public DbSet<LicenceNumberDetailsViewModel> LicenceNumberDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<GetInspectionNotingLogsViewModel> GetInspectionNotingLogsViewModel { get; set; }

        public DbSet<InspectionUserMapping> InspectionUserMappings { get; set; }
        public DbSet<InspectionComplianceLog> InspectionComplianceLogs { get; set; }

        [NotMapped]
        public DbSet<UpdateAlternateContactDetailsViewModel> UpdateAlternateContactDetailsViewModel { get; set; }
        
        [NotMapped]
        public DbSet<AlternateContactDetailsViewModel> AlternateContactDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<InspectionComplianceinfoViewModel> InspectionComplianceinfoViewModel { get; set; }

        [NotMapped]
        public DbSet<UserLoginDetailsViewModel> UserLoginDetailsViewModel { get; set; }
        public DbSet<InspectionDocument> InspectionDocuments { get; set; }
        public DbSet<InspectionDocument_Log> InspectionDocument_Logs { get; set; }

        public DbSet<Licence_PE_ISM_GeneralDetail> Licence_PE_ISM_GeneralDetails { get; set; }
        public DbSet<Licence_PE_ISM_Contrator> Licence_PE_ISM_Contrators { get; set; }
        public DbSet<Licence_PE_ISM_AmendmentDataHistories> Licence_PE_ISM_AmendmentDataHistories { get; set; }
        [NotMapped]
        public DbSet<GetAllClearanceDateSlabsViewModel> GetAllClearanceDateSlabsViewModel { get; set; }

        public DbSet<ProjectSiteLog> ProjectSiteLogs { get; set; }

        [NotMapped]
        public DbSet<ProjectSiteEstablishmentBasicDetailsViewModel> ProjectSiteEstablishmentBasicDetailsViewModel { get; set; }

        public DbSet<InspectionActionCode> InspectionActionCodes { get; set; }
        public DbSet<InspectionRoleWiseAllowedActionCode> InspectionRoleWiseAllowedActionCodes { get; set; }

        public DbSet<ToDoTableRootActivityMapping> ToDoTableRootActivityMappings { get; set; }
        public DbSet<Inspection_UserMapping_Logs> Inspection_UserMapping_Logs { get; set; }

        [NotMapped]
        public DbSet<GetUserRoleAndProfileDetailViewModel> GetUserRoleAndProfileDetailViewModel { get; set; }
        public DbSet<UserProfile_HRMS_CodeMapping> UserProfile_HRMS_CodeMappings { get; set; }

        [NotMapped]
        public DbSet<UserRefIdsAsPerIpinViewModel> UserRefIdsAsPerIpinViewModel { get; set; }

        public DbSet<Payments_RaisedFee> Payments_RaisedFee { get; set; }
        public DbSet<Payments_RaisedFee_Log> Payments_RaisedFee_Log { get; set; }
        public DbSet<ApplicationAndFeeRaiseAllowedMapping> ApplicationAndFeeRaiseAllowedMappings { get; set; }

        [NotMapped]
        public DbSet<ApplicableRaiseFeeHeadsViewModel> ApplicableRaiseFeeHeadsViewModel { get; set; }

        public DbSet<AppProcessLoadBalanceLog> AppProcessLoadBalanceLogs { get; set; }

        [NotMapped]
        public DbSet<InspectionResultViewModel> InspectionResultViewModel { get; set; }
        [NotMapped]
        public DbSet<InspectionTransferLogsViewModel> InspectionTransferLogsViewModel { get; set; }
        public DbSet<BuildingPlanFactory_Declaration_Stability_Randomization> BuildingPlanFactory_Declaration_Stability_Randomizations { get; set; }

        [NotMapped]
        public DbSet<HRMSDataViewModel> HRMSDataViewModel { get; set; }
        
        [NotMapped]
        public DbSet<HRMS_OfficerDetailsViewModel> HRMS_OfficerDetailsViewModel { get; set; }

        public DbSet<RoleWiseAllowedHRMSServiceCodes> RoleWiseAllowedHRMSServiceCodes { get; set; }

        [NotMapped]
        public DbSet<HRMS_RoleIdByHRMSServiceCodeViewModel> HRMS_RoleIdByHRMSServiceCodeViewModel { get; set; }

        public DbSet<Licence_Bocw_AmendmentDataHistories> Licence_Bocw_AmendmentDataHistories { get; set; }
        public DbSet<BusinessFirst_Withdraw_RequestLog> BusinessFirst_Withdraw_RequestLogs { get; set; }

        [NotMapped]
        public DbSet<LegacyAppPaymentDetailsViewModel> LegacyAppPaymentDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<PendingApplicationDetailsViewModel> PendingApplicationDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<PendingApplicationDetailsByServiceCodeViewModel> PendingApplicationDetailsByServiceCodeViewModel { get; set; }

        [NotMapped]
        public DbSet<ApplicationCurrentStatusViewModel> ApplicationCurrentStatusViewModel { get; set; }
        public DbSet<Dormant_ProcessEngineLog> Dormant_ProcessEngineLogs { get; set; }
        public DbSet<Dormant_ProcessFilesLog> Dormant_ProcessFilesLogs { get; set; }
        public DbSet<Dormant_FileNotification> Dormant_FileNotifications { get; set; }
        [NotMapped]
        public DbSet<ElapsedApplicationViewModel> ElapsedApplicationViewModel { get; set; }

        public DbSet<AppActionTimeLine> AppActionTimeLines { get; set; }
        public DbSet<AppActionTimeLineDefination> AppActionTimeLineDefinations { get; set; }
        public DbSet<ApplicationProcessPhaseLog> ApplicationProcessPhaseLogs { get; set; }
        public DbSet<ApplicationProcessPhaseAndObjectionLog> ApplicationProcessPhaseAndObjectionLogs { get; set; }

        public DbSet<Licence_BuildingPlan_PSIEC_GeneralDetail> Licence_BuildingPlan_PSIEC_GeneralDetails { get; set; }

        public DbSet<ApplicationActionExtension> ApplicationActionExtensions { get; set; }
        public DbSet<ApplicationActionExtensionLog> ApplicationActionExtensionLogs { get; set; }
        public DbSet<AppActionTime_MutualProcessFlag> AppActionTime_MutualProcessFlags { get; set; }

        [NotMapped]
        public DbSet<AppActionTime_MutualProcessFlagViewModel> AppActionTime_MutualProcessFlagViewModel { get; set; }

        public DbSet<TimeLine_DepartmentWiseFinalAction> TimeLine_DepartmentWiseFinalActions { get; set; }

        [NotMapped]
        public DbSet<PreCheckViewModel> PreCheckViewModel { get; set; }

        [NotMapped]
        public DbSet<ApplicationToBeElapsedViewModel> ApplicationToBeElapsedViewModel { get; set; }

        [NotMapped]
        public DbSet<AllowedReceiverDetailsViewModel> AllowedReceiverDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<GetApplciationReActivatedData> GetApplciationReActivatedData { get; set; }

        [NotMapped]
        public DbSet<ApplicationInitiateResponseViewModel> ApplicationInitiateResponseViewModel { get; set; }
        
        [NotMapped]
        public DbSet<GetDeemedApplciationsData> GetDeemedApplciationsData { get; set; }

        [NotMapped]
        public DbSet<AnnualReturnWelfareFundViewModel> AnnualReturnWelfareFundViewModel { get; set; }

        public DbSet<EmpanelledPersonDetailsByRoleNameViewModel> EmpanelledPersonDetailsByRoleNameViewModel { get; set; }

        public DbSet<Licence_ISM_ContractLabour_GeneralDetail> Licence_ISM_ContractLabour_GeneralDetails { get; set; }

        public DbSet<Licence_ISM_ContractLabour_AmendmentDataHistory> Licence_ISM_ContractLabour_AmendmentDataHistories { get; set; }
        public DbSet<Randomization_Initialization> Randomization_Initialization { get; set; }

        //[NotMapped]
        //public DbSet<GetAdminDashboardDetailsViewModel> GetAdminDashboardDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<AdminDashboardUserDetailsViewModel> AdminDashboardUserDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<AdminDashboardEstablishmentDetailsViewModel> AdminDashboardEstablishmentDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<AdminDashboardApplicationDetailsViewModel> AdminDashboardApplicationDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<AdminDashboardActionLogViewModel> AdminDashboardActionLogViewModel { get; set; }

        [NotMapped]
        public DbSet<AdminDashboardPaymentDetailsViewModel> AdminDashboardPaymentDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<AdminDashboardApprovalDetailsViewModel> AdminDashboardApprovalDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<ApplicationToBeEscalatedViewModel> ApplicationToBeEscalatedViewModel { get; set; }

        [NotMapped]
        public DbSet<All_Act_Deemed_ProcessEngineLogsViewModel> All_Act_Deemed_ProcessEngineLogsViewModel { get; set; }


        [NotMapped]
        public DbSet<DeemedApplicationTimeLineViewModel> DeemedApplicationTimeLineViewModel { get; set; }

        [NotMapped]
        public DbSet<ApplicationEscalationSlabViewModel> ApplicationEscalationSlabViewModel { get; set; }

        public DbSet<Esclations_ProcessEngine> Esclations_ProcessEngine { get; set; }
        public DbSet<AppEscalationMappings> AppEscalationMappings { get; set; }
        public DbSet<AppEscalations> AppEscalations { get; set; }

        [NotMapped]
        public DbSet<EscalatedApplicationViewModel> EscalatedApplicationViewModel { get; set; }
        [NotMapped]
        public DbSet<OfficerUserDetailsViewModel> OfficerUserDetailsViewModel { get; set; }
        [NotMapped]
        public DbSet<EscalationOffcerViewModel> EscalationOffcerViewModel { get; set; }

        public DbSet<AppActivationLogs> AppActivationLogs { get; set; }

        [NotMapped]
        public DbSet<AllTransactionsByAppRefIdViewModel> AllTransactionsByAppRefIdViewModel { get; set; }

        [NotMapped]
        public DbSet<PSIECUserViewModel> PSIECUserViewModel { get; set; }

        public DbSet<UserArchitectAdditionalInfoMapping> UserArchitectAdditionalInfoMappings { get; set; }

        [NotMapped]
        public DbSet<FundAndUnpaidWagesViewModel> FundAndUnpaidWagesViewModel { get; set; }

        [NotMapped]
        public DbSet<MonthWiseContributionDetailsViewModel> MonthWiseContributionDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<MonthWiseEmployeeDetailsViewModel> MonthWiseEmployeeDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<FundAndWagesCircleWiseDataViewModel> FundAndWagesCircleWiseDataViewModel { get; set; }

        [NotMapped]
        public DbSet<WelfareFundAndWagesDataViewModel> WelfareFundAndWagesDataViewModel { get; set; }

        [NotMapped]
        public DbSet<FactoriesListViewModel> FactoriesListViewModel { get; set; }

        public DbSet<DepartmentOfficialDetailsViewModel> DepartmentOfficialDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<DepartmentOfficialListViewModel> DepartmentOfficialListViewModel { get; set; }

        [NotMapped]

        public DbSet<InspectionLabourCircleViewModel> InspectionLabourCircleViewModel { get; set; }

        public DbSet<Deemed_ProcessFilesLogViewModel> Deemed_ProcessFilesLogViewModel { get; set; }
        public DbSet<WhatsNewInPortal> WhatsNewInPortal { get; set; }

        [NotMapped]
        public DbSet<PWBContributionDetailsViewModel> PWBContributionDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<EstablishmentWisePaymentDetailsViewModel> EstablishmentWisePaymentDetailsViewModel { get; set; }
        public DbSet<UserMyOfficeMapping> UserMyOfficeMappings { get; set; }
        public DbSet<ApplicationAction_ParallelProcess> ApplicationAction_ParallelProcesses { get; set; }

        [NotMapped]

        public DbSet<MPR_Labour_DashboardViewModel> MPR_Labour_DashboardViewModel { get; set; }
        public DbSet<MPR_Labour> MPR_Labours { get; set; }

        [NotMapped]
        public DbSet<MPR_Alc_DashboardViewModel> MPR_Alc_DashboardViewModel { get; set; }

        public DbSet<AnnualReturnDashboardViewModel> AnnualReturnDashboardViewModel { get; set; }

        public DbSet<Annual_Return> AnnualReturns { get; set; }

        [NotMapped]
        public DbSet<EstablishmentDataViewModel> EstablishmentDataViewModel { get; set; }

        public DbSet<UserPasswordResetLog> UserPasswordResetLogs { get; set; }



        public DbSet<MPR_Alc> MPR_Alcs { get; set; }

        public DbSet<GolferRegistration> GolferRegistrations { get; set; }

        [NotMapped]
        public DbSet<MprDataViewModel> MprDataViewModel { get; set; }

        public DbSet<EmpUniqueId> EmpUniqueId { get; set; }

        [NotMapped]
        public DbSet<ErrorLogParamsViewModel> ErrorLogParamsViewModel { get; set; }

        [NotMapped]
        public DbSet<ElapsedPhaseApplicationViewModel> ElapsedPhaseApplicationViewModel { get; set; }

        public DbSet<DepartmentRoleMapping> DepartmentRoleMappings { get; set; }
        public DbSet<UserPortalSwitchLog> UserPortalSwitchLogs { get; set; }

        [NotMapped]

        public DbSet<ShopBacklogDataViewModel> ShopBacklogDataViewModel { get; set; }

        [NotMapped]
        public DbSet<LegacyShopFormDataViewModel> LegacyShopFormDataViewModel { get; set; }

        [NotMapped]
        public DbSet<WorkingHourSlabViewModel> WorkingHourSlabViewModel { get; set; }

        [NotMapped]
        public DbSet<MyOfficeUserProfileWiseDataViewModel> MyOfficeUserProfileWiseDataViewModel { get; set; }

        [NotMapped]
        public DbSet<UserDetailsByRoleNameViewModel> UserDetailsByRoleNameViewModel { get; set; }

        [NotMapped]
        public DbSet<PendencyReportViewModel> PendencyReportViewModel { get; set; }

        public DbSet<UserApiActivityLog> UserApiActivityLogs { get; set; }

        public DbSet<OSH_Form_1_Registration> OSH_Form_1_Registrations { get; set; }
        public DbSet<OSH_Form_1_Registration_Factory> OSH_Form_1_Registration_Factories { get; set; }
        public DbSet<OSH_Form_1_Registration_BOCW> OSH_Form_1_Registration_BOCW { get; set; }
        public DbSet<OSH_Form_1_Registration_EmployeeDetail> OSH_Form_1_Registration_EmployeeDetails { get; set; }
        public DbSet<OSH_Form_1_Registration_EstablishmentOtherType> OSH_Form_1_Registration_EstablishmentOtherType { get; set; }
        public DbSet<OSH_Form_1_Registration_EPFO_ESIC_Detail> OSH_Form_1_Registration_EPFO_ESIC_Details { get; set; }
        public DbSet<OSH_Form_1_Registration_EmployerDetail> OSH_Form_1_Registration_EmployerDetails { get; set; }
        public DbSet<OSH_Form_1_Registration_PrincipalEmployerDetail> OSH_Form_1_Registration_PrincipalEmployerDetails { get; set; }
        public DbSet<OSH_Form_1_Registration_ContractorDetail> OSH_Form_1_Registration_ContractorDetails { get; set; }
        public DbSet<OSH_Form_1_Registration_MotorTransportDetail> OSH_Form_1_Registration_MotorTransportDetails { get; set; }

        [NotMapped]
        public DbSet<ContractLabourLicenceValidityViewModel> ContractLabourLicenceValidityViewModel { get; set; }

        public DbSet<BocwMobileAppUserDataViewModel> BocwMobileAppUserDataViewModel { get; set; }

        public DbSet<OSH_Form_30_CommonLicense_Establishment> OSH_Form_30_CommonLicense_Establishments { get; set; }
        public DbSet<OSH_Form_30_CommonLicense_Factory> OSH_Form_30_CommonLicense_Factories { get; set; }
        public DbSet<OSH_Form_30_CommonLicense_ContractLabour> OSH_Form_30_CommonLicense_ContractLabours { get; set; }

        public DbSet<OSH_Form_21_ContractLabour_General_Detail> OSH_Form_21_ContractLabour_General_Details { get; set; }
        public DbSet<OSH_Form_21_ContractLabour_Employee_Detail> OSH_Form_21_ContractLabour_Employee_Details { get; set; }
        public DbSet<OSH_Form_21_ContractLabour_Establishment_Detail> OSH_Form_21_ContractLabour_Establishment_Details { get; set; }
        public DbSet<OSH_Form_21_ContractLabour_MigrantWorker> OSH_Form_21_ContractLabour_MigrantWorkers { get; set; }

        [NotMapped]
        public DbSet<MyOfficeUserValidationViewModel> MyOfficeUserValidationViewModel { get; set; }

        [NotMapped]
        public DbSet<TradeUnionLicenceValidateViewModel> TradeUnionLicenceValidateViewModel { get; set; }
        
        [NotMapped]
        public DbSet<EstablishmentAndUserDetailViewModel> EstablishmentAndUserDetailViewModel { get; set; }
        public DbSet<OfflineReportLogs> OfflineReportLogs { get; set; }
        [NotMapped]
        public DbSet<ApprovedDataViewModel> ApprovedDataViewModel { get; set; }

        [NotMapped]
        public DbSet<WelfareFundStatusDetailsViewModel> WelfareFundStatusDetailsViewModel { get; set; }

        [NotMapped]
        public DbSet<WelfareSchemesDetailsViewModel> WelfareSchemesDetailsViewModel { get; set; }

        public DbSet<LicenceWiseRelationshipMapping> LicenceWiseRelationshipMappings { get; set; }

        [NotMapped]
        public DbSet<InspectionDetailsBylicenceViewModel> InspectionDetailsBylicenceViewModel { get; set; }

        [NotMapped]
        public DbSet<WelfareFundReceiptDetailsViewModel> WelfareFundReceiptDetailsViewModel { get; set; }

        public DbSet<UserPasswordChangeLog> UserPasswordChangeLogs { get; set; }

        [NotMapped]
        public DbSet<RegisteredFactoryCircleWiseViewModel> RegisteredFactoryCircleWiseViewModel { get; set; }
        public DbSet<OldInspectionMapping> OldInspectionMappings { get; set; }

        public DbSet<LWB_FundMaster> LWB_FundMaster { get; set; }
        public DbSet<LWB_Employees_Fund> LWBFundEmployees { get; set; }

        [NotMapped]
        public DbSet<UnpaidWagesDetailsBylicenceViewModel> UnpaidWagesDetailsBylicenceViewModel { get; set; }

        public DbSet<LWB_Employees_UnpaidWages> LWB_Employees_UnpaidWages { get; set; }
        public DbSet<LWB_LIdAndUnpaidWages> LWB_LIdAndUnpaidWages { get; set; }

        public DbSet<InspectionFactoryAllotmentDetailsViewModel> InspectionFactoryAllotmentDetailsViewModel { get; set; }
        public DbSet<Inspection_AllotmentLogs> Inspection_AllotmentLogs { get; set; }
        public DbSet<User2FactorVerification> User2FactorVerifications { get; set; }

        #region For samadhan
        public DbSet<ComplaintsCategory> ComplaintsCategories { get; set; }

        public DbSet<WorkerDetail> WorkerDetails { get; set; }
        public DbSet<AppComplaintTypeMapping> AppComplaintTypeMappings { get; set; }
        public DbSet<ComplainantTypeComplaintTypeMapping> ComplainantTypeComplaintTypeMappings { get; set; }

        public DbSet<Complaint_EmployerORContractorDetail> Complaint_EmployerORContractorDetails { get; set; }
        public DbSet<Complaint_WorkplaceDetail> Complaint_WorkplaceDetails { get; set; }
        public DbSet<Complaint_EstablishmentDetail> Complaint_EstablishmentDetails { get; set; }
        public DbSet<Complaint_GratuityClaim> Complaint_GratuityClaims { get; set; }
        public DbSet<Complaint_MaternityBenefitComplaint> Complaint_MaternityBenefitComplaints { get; set; }
        public DbSet<Complaint_Claim_CodeOnWage> Complaint_Claim_CodeOnWages { get; set; }
        public DbSet<Complaint_MinimumWage> Complaint_MinimumWages { get; set; }
        public DbSet<Complaint_MinimumWagesPeriodAmt> Complaint_MinimumWagesPeriodAmts { get; set; }
        public DbSet<Complaint_Wages_WkDay> Complaint_Wages_WkDays { get; set; }
        public DbSet<Complaint_Wages_WkDay_PeriodAmt> Complaint_Wages_WkDay_PeriodAmts { get; set; }
        public DbSet<Complaint_Wages_OT> Complaint_Wages_OTs { get; set; }
        public DbSet<Complaint_Wages_OT_PeriodAmt> Complaint_Wages_OT_PeriodAmts { get; set; }
        public DbSet<Complaint_Wages_Not_Paid> Complaint_Wages_Not_Paids { get; set; }
        public DbSet<Complaint_Wages_Not_Paid_PeriodAmt> Complaint_Wages_Not_Paid_PeriodAmts { get; set; }
        public DbSet<Complaint_Wages_Unauth_Deduct> Complaint_Wages_Unauth_Deducts { get; set; }
        public DbSet<Complaint_Wages_Unauth_Deduct_PeriodAmt> Complaint_Wages_Unauth_Deduct_PeriodAmts { get; set; }
        public DbSet<Complaint_Non_Pay_Bonus> Complaint_Non_Pay_Bonuses { get; set; }
        public DbSet<Complaint_Non_Pay_Bonus_PeriodAmt> Complaint_Non_Pay_Bonus_PeriodAmts { get; set; }

        public DbSet<Complaint_RecOfMon_GeneralDetail> Complaint_RecOfMon_GeneralDetails { get; set; }
        public DbSet<Complaint_RecOfMon_MoneyDueDetail> Complaint_RecOfMon_MoneyDueDetails { get; set; }
        public DbSet<Complaint_RecOfMon_SettlementDetail> Complaint_RecOfMon_SettlementDetails { get; set; }
        public DbSet<Complaint_RecOfMon_AwardDetail> Complaint_RecOfMon_AwardDetails { get; set; }
        public DbSet<Complaint_RecOfMon_NoticePayDetail> Complaint_RecOfMon_NoticePayDetails { get; set; }
        public DbSet<Complaint_RecOfMon_RetrenchmentCompDetail> Complaint_RecOfMon_RetrenchmentCompDetails { get; set; }
        public DbSet<Complaint_RecOfMon_LayOffDetail> Complaint_RecOfMon_LayOffDetails { get; set; }
        public DbSet<Complaint_RecOfMon_LayOffCompDetail> Complaint_RecOfMon_LayOffCompDetails { get; set; }
        public DbSet<Complaint_Review_OfDismissal> Complaint_Review_OfDismissals { get; set; }
        public DbSet<Complaint_Appeal> Complaint_Appeals { get; set; }
        public DbSet<Complaint_IndustrialDispute> Complaint_IndustrialDisputes { get; set; }
        public DbSet<Complaint_IndustrialDisputeReasonMapping> Complaint_IndustrialDisputeReasonMappings { get; set; }
        public DbSet<Complaint_IndustrialDisputeReliefSoughtMapping> Complaint_IndustrialDisputeReliefSoughtMappings { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //The OnDelete method takes a DeleteBehavior enum as a parameter:
            //Cascade - dependents should be deleted
            //Restrict - dependents are unaffected
            //SetNull - the foreign key values in dependent rows should update to NULL
            //modelBuilder.Entity<User>().HasKey(x => x.UserId);
            //modelBuilder.Entity<UserRole>().HasKey(x => x.RoleId);
            modelBuilder.Entity<DistrictLgd>()
                .HasMany(estb => estb.TehsilLgds)
                .WithOne(dist => dist.DistrictLgd)
                .HasForeignKey(fk => fk.DistrictRefId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<DistrictLgd>()
            //    .HasMany(estb => estb.Estb_DistrictLgds)
            //    .WithOne(dist => dist.Estb_DistrictLgd)
            //    .HasForeignKey(fk => fk.Estb_DistrictRefId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
                .HasMany(estb => estb.Comm_DistrictLgds)
                .WithOne(dist => dist.Comm_DistrictLgd)
                .HasForeignKey(fk => fk.Comm_DistrictRefId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<TehsilLgd>()
            //    .HasMany(pt => pt.Estb_TehsilLgds)
            //    .WithOne(ct => ct.Estb_TehsilLgd)
            //    .HasForeignKey(fk => fk.Estb_TehsilRefId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
                .HasMany(pt => pt.Comm_TehsilLgds)
                .WithOne(ct => ct.Comm_TehsilLgd)
                .HasForeignKey(fk => fk.Comm_TehsilRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
                .HasMany(pt => pt.Employer_TehsilLgds)
                .WithOne(ct => ct.Employer_TehsilLgd)
                .HasForeignKey(fk => fk.Employer_TehsilRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
                .HasMany(pt => pt.Employer_TehsilLgds)
                .WithOne(ct => ct.Employer_TehsilLgd)
                .HasForeignKey(fk => fk.Employer_TehsilRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
                .HasMany(pt => pt.Contractor_TehsilLgds)
                .WithOne(ct => ct.Contractor_TehsilLgd)
                .HasForeignKey(fk => fk.Contractor_TehsilRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
                .HasMany(pt => pt.Contractor_TehsilLgds)
                .WithOne(ct => ct.Contractor_TehsilLgd)
                .HasForeignKey(fk => fk.Contractor_TehsilRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Establishment_GeneralDetail>()
                .HasMany(pt => pt.Establishment_ContractorsDetail)
                .WithOne(ct => ct.Establishment_GeneralDetail)
                .HasForeignKey(fk => fk.EstablishmentRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Establishment_GeneralDetail>()
                .HasOne<Establishment_EmployerDetail>(s => s.Establishment_EmployerDetail)
                .WithOne(ad => ad.Establishment_GeneralDetail)
                .HasForeignKey<Establishment_EmployerDetail>(ad => ad.EstablishmentRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                 .HasOne<Establishment_GeneralDetail>(s => s.Establishment_GeneralDetail)
                 .WithOne(ad => ad.Application)
                 .HasForeignKey<Establishment_GeneralDetail>(ad => ad.AppRefId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<ApplicationAction>(s => s.ApplicationAction)
                .WithOne(ad => ad.Application)
                .HasForeignKey<ApplicationAction>(ad => ad.ApplicationRefId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<ApplicationAction>()
            //   .HasMany<ApplicationActionLog>(pt => pt.ApplicationActionLogs)
            //   .WithOne(ct => ct.ApplicationAction)
            //   .HasForeignKey(fk => fk.AppActionRefId)
            //   .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Establishment_GeneralDetail>()
              .HasMany<Establishment_Migrantworker>(s => s.Establishment_Migrantworkers)
               .WithOne(ad => ad.Establishment_GeneralDetail)
               .HasForeignKey(ad => ad.EstablishmentRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Contractor_GeneralDetail>(s => s.Contractor_GeneralDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Contractor_GeneralDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relations Define for User & UserProfile
            modelBuilder.Entity<User>()
               .HasOne<UserProfileMapping>(s => s.UserProfileMapping)
               .WithOne(ad => ad.User)
               .HasForeignKey<UserProfileMapping>(ad => ad.UserRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProfile>()
               .HasOne<UserProfileMapping>(s => s.UserProfileMapping)
               .WithOne(ad => ad.UserProfile)
               .HasForeignKey<UserProfileMapping>(ad => ad.UserProfileRefId)
               .OnDelete(DeleteBehavior.Restrict);

            // Relation Define For User & Role Table

            //modelBuilder.Entity<User>()
            //   .HasOne<UserRoleMapping>(s => s.UserRoleMapping)
            //   .WithOne(ad => ad.User)
            //   .HasForeignKey<UserRoleMapping>(ad => ad.UserRoleRefId)
            //   .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UserRole>()
            //   .HasOne<UserRoleMapping>(s => s.UserRoleMapping)
            //   .WithOne(ad => ad.UserRole)
            //   .HasForeignKey<UserRoleMapping>(ad => ad.UserRoleRefId)
            //   .OnDelete(DeleteBehavior.Restrict);

            // Relation Define for Department Mapping

            modelBuilder.Entity<Department>()
               .HasMany<UserDepartmentMapping>(s => s.UserDepartmentMappings)
               .WithOne(ad => ad.Department)
               .HasForeignKey(ad => ad.DepartmentRefID)
               .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Department>()
               .HasMany<UserDepartmentMapping>(s => s.UserDepartmentMappings)
               .WithOne(ad => ad.Department)
               .HasForeignKey(ad => ad.DepartmentRefID)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
               .HasOne<UserDepartmentMapping>(s => s.UserDepartmentMapping)
               .WithOne(ad => ad.User)
               .HasForeignKey<UserDepartmentMapping>(ad => ad.UserRefId)
               .OnDelete(DeleteBehavior.Restrict);

            // Define Relation for Project Site 

            modelBuilder.Entity<User>()
                .HasMany<ProjectSite>(s => s.ProjectSites)
                .WithOne(ad => ad.User)
                .HasForeignKey(ad => ad.UserRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectSite>()
               .HasMany<Application>(s => s.Applications)
               .WithOne(ad => ad.ProjectSites)
               .HasForeignKey(ad => ad.ProjectSiteRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<UserProfile>()
              .HasOne<DigitalSignature>(s => s.DigitalSignature)
              .WithOne(ad => ad.UserProfile)
              .HasForeignKey<DigitalSignature>(ad => ad.UserProfileRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DigitalSignature>()
              .HasMany<Application>(s => s.Applications)
              .WithOne(ad => ad.DigitalSignature)
              .HasForeignKey(ad => ad.DigitalSignatureRefId)
              .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<DigitalSignature>()
              .HasMany<DigitalSignature_Log>(s => s.DigitalSignature_Logs)
              .WithOne(ad => ad.DigitalSignature)
              .HasForeignKey(ad => ad.DigitalSignatureRefId)
              .OnDelete(DeleteBehavior.Restrict);

            #region Contractor

            modelBuilder.Entity<Contractor_GeneralDetail>()
               .HasOne<Contractor_PrincipalEmployer>(s => s.Contractor_PrincipalEmployer)
                .WithOne(ad => ad.Contractor_GeneralDetail)
                .HasForeignKey<Contractor_PrincipalEmployer>(ad => ad.ContractorRefId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Contractor_GeneralDetail>()
            //   .HasOne<Contractor_ContractLabour>(s => s.Contractor_ContractLabour)
            //    .WithOne(ad => ad.Contractor_GeneralDetail)
            //    .HasForeignKey<Contractor_ContractLabour>(ad => ad.ContractorRefId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Contractor_GeneralDetail>(s => s.Contractor_GeneralDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Contractor_GeneralDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
                .HasMany(pt => pt.Contractor_GeneralDetail_TehsilLgds)
                .WithOne(ct => ct.Contractor_TehsilLgd)
                .HasForeignKey(fk => fk.TehsilRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
               .HasMany<Contractor_PrincipalEmployer>(pt => pt.Contractor_PE_ESTB_TehsilLgds)
               .WithOne(ct => ct.Contractor_PE_ESTB_TehsilLgd)
               .HasForeignKey(fk => fk.TehsilRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
              .HasMany<Contractor_PrincipalEmployer>(pt => pt.Contractor_PE_TehsilLgds)
              .WithOne(ct => ct.Contractor_PE_TehsilLgd)
              .HasForeignKey(fk => fk.PrincipalEmployer_TehsilRefId)
              .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<TehsilLgd>()
            //   .HasMany(pt => pt.Contractor_ContractLabour_TehsilLgds)
            //   .WithOne(ct => ct.Contractor_CL_TehsilLgd)
            //   .HasForeignKey(fk => fk.TehsilRefId)
            //   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
               .HasMany(estb => estb.Contractor_GeneralDetail_DistrictLgds)
               .WithOne(dist => dist.Contractor_DistrictLgd)
               .HasForeignKey(fk => fk.DistrictRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
              .HasMany<Contractor_PrincipalEmployer>(estb => estb.Contractor_PE_ESTB_DistrictLgds)
              .WithOne(dist => dist.Contractor_PE_ESTB_DistrictLgd)
              .HasForeignKey(fk => fk.DistrictRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
              .HasMany<Contractor_PrincipalEmployer>(estb => estb.Contractor_PE_DistrictLgds)
              .WithOne(dist => dist.Contractor_PE_DistrictLgd)
              .HasForeignKey(fk => fk.PrincipalEmployer_DistrictRefId)
              .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<DistrictLgd>()
            //  .HasMany(estb => estb.Contractor_ContractLabour_DistrictLgds)
            //  .WithOne(dist => dist.Contractor_CL_DistrictLgd)
            //  .HasForeignKey(fk => fk.DistrictRefId)
            //  .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Application Action & Log
            modelBuilder.Entity<User>()
              .HasMany<ApplicationAction>(s => s.ApplicationActions_User_Receivers)
              .WithOne(ad => ad.ApplicationActions_User_Receiver)
              .HasForeignKey(ad => ad.Receiver_UserRefId)
              .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<User>()
              .HasMany<ApplicationAction>(s => s.ApplicationActions_User_Senders)
              .WithOne(ad => ad.ApplicationActions_User_Sender)
              .HasForeignKey(ad => ad.Sender_UserRefId)
              .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<UserProfile>()
             .HasMany<ApplicationAction>(s => s.ApplicationActions_Profile_Receivers)
             .WithOne(ad => ad.ApplicationActions_Profile_Receiver)
             .HasForeignKey(ad => ad.Receiver_ProfileRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProfile>()
              .HasMany<ApplicationAction>(s => s.ApplicationActions_Profile_Senders)
              .WithOne(ad => ad.ApplicationActions_Profile_Sender)
              .HasForeignKey(ad => ad.Sender_ProfileRefId)
              .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<ApplicationAction>()
            //   .HasMany<ApplicationActionLog>(pt => pt.ApplicationActionLogs)
            //   .WithOne(ct => ct.ApplicationAction)
            //   .HasForeignKey(fk => fk.AppActionRefId)
            //   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            modelBuilder.Entity<DashboardViewModel>().HasNoKey();

            modelBuilder.Entity<Department>()
              .HasMany<Application>(s => s.Applications)
              .WithOne(ad => ad.Department)
              .HasForeignKey(ad => ad.DepartmentRefID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DashboardAlreadyClearenceViewModel>().HasNoKey();

            #region Document

            modelBuilder.Entity<Document>()
            .HasMany<ApplicationDocument>(s => s.ApplicationDocuments)
            .WithOne(ad => ad.Document)
            .HasForeignKey(ad => ad.DocumentRefId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
           .HasMany<ApplicationDocument>(s => s.ApplicationDocuments)
           .WithOne(ad => ad.Application)
           .HasForeignKey(ad => ad.AppRefId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationDocument>()
            .HasMany<ApplicationDocument_Log>(s => s.ApplicationDocument_Log)
            .WithOne(ad => ad.ApplicationDocument)
            .HasForeignKey(ad => ad.AppDocId)
            .OnDelete(DeleteBehavior.Restrict);

            #endregion Document

            #region Building Plan, FeeHeader & Details

            modelBuilder.Entity<Application>()
              .HasMany<AppFeeDetail>(s => s.AppFeeDetails)
              .WithOne(ad => ad.Application)
              .HasForeignKey(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeesHeader>()
              .HasMany<AppFeeDetail>(s => s.AppFeeDetails)
              .WithOne(ad => ad.FeesHeader)
              .HasForeignKey(ad => ad.FeeHeaderRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<BP_GeneralDetail>(s => s.BP_GeneralDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<BP_GeneralDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
              .HasOne<UserRegisteredMobileAppDevice>(s => s.UserRegisteredMobileAppDevice)
              .WithOne(ad => ad.User)
              .HasForeignKey<UserRegisteredMobileAppDevice>(ad => ad.UserRefId)
              .OnDelete(DeleteBehavior.Restrict);
            #endregion

            modelBuilder.Entity<AppFileUploadInfoViewModel>().HasNoKey();

            #region Circle & Treasury Mapping

            modelBuilder.Entity<DistrictLgd>()
                .HasMany<FactoryCircle>(s => s.FactoryCircles)
                .WithOne(ad => ad.DistrictLgd)
                .HasForeignKey(ad => ad.DistrictLgdRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FactoryCircle>()
                .HasOne<FactoryCircleTreasuryMapping>(s => s.FactoryCircleTreasuryMappings)
                .WithOne(ad => ad.FactoryCircle)
                .HasForeignKey<FactoryCircleTreasuryMapping>(ad => ad.FactoryCircleRefId)
                .OnDelete(DeleteBehavior.Restrict);

            // ALC Circle Mapping
            modelBuilder.Entity<DistrictLgd>()
               .HasMany<ALCCircle>(s => s.ALCCircles)
               .WithOne(ad => ad.DistrictLgd)
               .HasForeignKey(ad => ad.DistrictLgdRefId)
               .OnDelete(DeleteBehavior.Restrict);

            // Labour Circle Mapping

            modelBuilder.Entity<ALCCircle>()
               .HasMany<LabourCircle>(s => s.LabourCircles)
               .WithOne(ad => ad.ALCCircle)
               .HasForeignKey(ad => ad.ALCCircleRefId)
               .OnDelete(DeleteBehavior.Restrict);

            // AlC Circle & Labour Circle  Mapping With FactoryCircleTreasuryMapping

            modelBuilder.Entity<ALCCircle>()
                .HasOne<ALCCircleTreasuryMapping>(s => s.ALCCircleTreasuryMapping)
                .WithOne(ad => ad.ALCCircle)
                .HasForeignKey<ALCCircleTreasuryMapping>(ad => ad.ALCCircleRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabourCircle>()
                .HasOne<LabourCircleTreasuryMapping>(s => s.LabourCircleTreasuryMapping)
                .WithOne(ad => ad.LabourCircle)
                .HasForeignKey<LabourCircleTreasuryMapping>(ad => ad.LabourCircleRefId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion Circle & Treasury Mapping

            modelBuilder.Entity<Application>()
              .HasMany<AppFeeTransaction>(s => s.AppFeeTransactions)
              .WithOne(ad => ad.Application)
              .HasForeignKey(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            // Application To ApplicationCircleMapping Relations 

            modelBuilder.Entity<Application>()
                .HasOne<ApplicationCircleMapping>(s => s.ApplicationCircleMapping)
                .WithOne(ad => ad.Application)
                .HasForeignKey<ApplicationCircleMapping>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FactoryCircle>()
              .HasMany<ApplicationCircleMapping>(s => s.ApplicationCircleMappings)
              .WithOne(ad => ad.FactoryCircle)
              .HasForeignKey(ad => ad.FactoryCircleRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ALCCircle>()
              .HasMany<ApplicationCircleMapping>(s => s.ApplicationCircleMappings)
              .WithOne(ad => ad.ALCCircle)
              .HasForeignKey(ad => ad.ALCCircleRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabourCircle>()
              .HasMany<ApplicationCircleMapping>(s => s.ApplicationCircleMappings)
              .WithOne(ad => ad.LabourCircle)
              .HasForeignKey(ad => ad.LabourCircleRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MajorDashboardCountViewModel>().HasNoKey();

            modelBuilder.Entity<RecordTypeCountViewModel>().HasNoKey();

            modelBuilder.Entity<RecordTypeMenuCountViewModel>().HasNoKey();
            modelBuilder.Entity<RecordsTypeListViewModel>().HasNoKey();
            modelBuilder.Entity<StatusManagerResponseParmsViewModel>().HasNoKey();

            modelBuilder.Entity<DistrictLgd>()
             .HasMany<ProjectSite>(s => s.ProjectSites)
             .WithOne(ad => ad.DistrictLgd)
             .HasForeignKey(ad => ad.DistrictRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TehsilLgd>()
             .HasMany<ProjectSite>(s => s.ProjectSites)
             .WithOne(ad => ad.TehsilLgd)
             .HasForeignKey(ad => ad.TehsilRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectProfileViewModel>().HasNoKey();
            modelBuilder.Entity<Establishment_EPFO_Report>().HasNoKey();
            modelBuilder.Entity<CircleManagerViewModel>().HasNoKey();
            modelBuilder.Entity<LabourCircleViewModel>().HasNoKey();
            modelBuilder.Entity<DistrictViewModel>().HasNoKey();
            modelBuilder.Entity<ProcessApplicationViewModels>().HasNoKey();
            //modelBuilder.Entity<LicenceValidDates>().HasNoKey();

            modelBuilder.Entity<MobileAppDeviceResponseViewModel>().HasNoKey();
            modelBuilder.Entity<IntReturn>().HasNoKey();
            modelBuilder.Entity<ProcessApplicationUsersViewModel>().HasNoKey();
            modelBuilder.Entity<NotingLogsViewModel>().HasNoKey();
            modelBuilder.Entity<HRMSDataViewModel>().HasNoKey();
            // Mapping Of users with Factory & Labour circles
            modelBuilder.Entity<FactoryCircle>()
             .HasMany<UserCircleMapping>(s => s.UserCircleMapping)
             .WithOne(ad => ad.FactoryCircle)
             .HasForeignKey(ad => ad.FactoryCircleRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabourCircle>()
             .HasMany<UserCircleMapping>(s => s.UserCircleMapping)
             .WithOne(ad => ad.LabourCircle)
             .HasForeignKey(ad => ad.LabourCircleRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ALCCircle>()
             .HasMany<UserCircleMapping>(s => s.UserCircleMapping)
             .WithOne(ad => ad.ALCCircle)
             .HasForeignKey(ad => ad.AlcCircleRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationLogsViewModel>().HasNoKey();
            modelBuilder.Entity<LendingOfficerDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<CertificateGenerateServiceResultTemplate>().HasNoKey();
            modelBuilder.Entity<ProjectSitesViewModel>().HasNoKey();


            // Common Licence Relation Define Here

            modelBuilder.Entity<Application>()
                .HasOne<CommonLicence_GeneralDetail>(s => s.CommonLicence_GeneralDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<CommonLicence_GeneralDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CommonLicence_GeneralDetail>()
                 .HasOne<CommonLicence_ContractorDetail>(s => s.CommonLicence_ContractorDetail)
                 .WithOne(ad => ad.CommonLicence_GeneralDetail)
                 .HasForeignKey<CommonLicence_ContractorDetail>(ad => ad.CommonLicenceRefId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CommonLicence_GeneralDetail>()
               .HasOne<CommonLicences_SelectedLicenceMapping>(s => s.CommonLicences_SelectedLicenceMapping)
               .WithOne(ad => ad.CommonLicence_GeneralDetail)
               .HasForeignKey<CommonLicences_SelectedLicenceMapping>(ad => ad.CommonLicenceRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Establishment_GeneralDetail>()
             .HasMany<CommonLicence_GeneralDetail>(s => s.CommonLicence_GeneralDetail)
              .WithOne(ad => ad.Establishment_GeneralDetail)
              .HasForeignKey(ad => ad.EstablishmentRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppFeeHeadersAndTreasuryHeadsInfoViewModel>().HasNoKey();

            //RegistrationDetail - This Table hold record of issueds registrations. Relation define below

            modelBuilder.Entity<Establishment_GeneralDetail>()
            .HasMany<RegistrationDetail>(s => s.RegistrationDetail)
             .WithOne(ad => ad.Establishment_GeneralDetail)
             .HasForeignKey(ad => ad.EstablishmentRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppDDOCodesInfoViewModel>().HasNoKey();

            modelBuilder.Entity<Application>()
            .HasMany<AppPaymentSuccessTransactionMapping>(s => s.AppPaymentSuccessTransactionMappings)
             .WithOne(ad => ad.Application)
             .HasForeignKey(ad => ad.AppRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppFeeTransaction>()
               .HasOne<AppPaymentSuccessTransactionMapping>(s => s.AppPaymentSuccessTransactionMapping)
               .WithOne(ad => ad.AppFeeTransaction)
               .HasForeignKey<AppPaymentSuccessTransactionMapping>(ad => ad.AppFeeTransactionRefId)
               .OnDelete(DeleteBehavior.Restrict);

            // Contract Labour And Establishment Mapping Relation Define here
            modelBuilder.Entity<Contractor_GeneralDetail>()
               .HasOne<ContractLabourAndEstablishmentMapping>(s => s.ContractLabourAndEstablishmentMapping)
               .WithOne(ad => ad.Contractor_GeneralDetail)
               .HasForeignKey<ContractLabourAndEstablishmentMapping>(ad => ad.ContractLabourRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Establishment_GeneralDetail>()
               .HasMany<ContractLabourAndEstablishmentMapping>(s => s.ContractLabourAndEstablishmentMapping)
                .WithOne(ad => ad.Establishment_GeneralDetail)
                .HasForeignKey(ad => ad.EstablishmentRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                 .HasOne<BuildingPlanHUD_GeneralDetail>(s => s.BuildingPlanHUD_GeneralDetail)
                 .WithOne(ad => ad.Application)
                 .HasForeignKey<BuildingPlanHUD_GeneralDetail>(ad => ad.AppRefId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<BuildingPlanFactory_GeneralDetail>(s => s.BuildingPlanFactory_GeneralDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<BuildingPlanFactory_GeneralDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Application>()
                .HasMany<ApplicationAction_ParallelProcess>(s => s.ApplicationAction_ParallelProcesses)
                .WithOne(ad => ad.Application)
                .HasForeignKey(ad => ad.ApplicationRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
              .HasMany<ApplicationAction_ParallelProcess>(s => s.ApplicationActions_ParallelProcess_User_Receivers)
              .WithOne(ad => ad.ApplicationActions_ParallelProcess_User_Receivers)
              .HasForeignKey(ad => ad.Receiver_UserRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
             .HasMany<ApplicationAction_ParallelProcess>(s => s.ApplicationActions_ParallelProcess_User_Senders)
             .WithOne(ad => ad.ApplicationActions_ParallelProcess_User_Sender)
             .HasForeignKey(ad => ad.Sender_UserRefId)
             .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TransactionalStoreProcedureResponseViewModel>().HasNoKey();

            modelBuilder.Entity<UserProfile>()
             .HasMany<ApplicationAction_ParallelProcess>(s => s.ApplicationActions_ParallelProcess_Profile_Receivers)
             .WithOne(ad => ad.ApplicationActions_ParallelProcess_Profile_Receiver)
             .HasForeignKey(ad => ad.Receiver_ProfileRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProfile>()
              .HasMany<ApplicationAction_ParallelProcess>(s => s.ApplicationActions_ParallelProcess_Profile_Senders)
              .WithOne(ad => ad.ApplicationActions_ParallelProcess_Profile_Senders)
              .HasForeignKey(ad => ad.Sender_ProfileRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
              .HasMany<UserPasswordResetLog>(s => s.UserPasswordResetLogs)
              .WithOne(ad => ad.User)
              .HasForeignKey(ad => ad.UserRefId)
              .OnDelete(DeleteBehavior.Restrict);





            #region Building Plan Relation

            modelBuilder.Entity<Application>()
              .HasOne<BuildingPlan>(s => s.BuildingPlan)
              .WithOne(ad => ad.Application)
              .HasForeignKey<BuildingPlan>(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Establishment_GeneralDetail>()
            .HasMany<BuildingPlan>(s => s.BuildingPlan)
             .WithOne(ad => ad.Establishment_GeneralDetail)
             .HasForeignKey(ad => ad.EstablishmentRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BuildingPlan>()
            .HasMany<BuildingPlan_AreaDetail>(s => s.BuildingPlan_AreaDetail)
             .WithOne(ad => ad.BuildingPlan)
             .HasForeignKey(ad => ad.BuildingPlanRefId)
             .OnDelete(DeleteBehavior.Restrict);

            #endregion Building Plan Relation

            modelBuilder.Entity<DisplayPaymentViewModel>().HasNoKey();
            modelBuilder.Entity<UserFullDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicationFullDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<ContractLabourPdfDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<RoleWiseAllowedActionCodeViewModel>().HasNoKey();
            modelBuilder.Entity<BuildingPlanHudPdfDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<ShopLicencePdfDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<ProjectSiteViewModel>().HasNoKey();
            modelBuilder.Entity<EstablishmentCardInfoViewModel>().HasNoKey();
            modelBuilder.Entity<InvestPunjabShareStatusParmsViewModel>().HasNoKey();
            modelBuilder.Entity<DataTableParamsViewModel>().HasNoKey();
            modelBuilder.Entity<GenerateLicenceNoViewModel>().HasNoKey();
            modelBuilder.Entity<PrincipalApproval_RBA_DetailsViewModel>().HasNoKey();
            modelBuilder.Entity<PrincipalApprovalData>().HasNoKey();
            modelBuilder.Entity<TransferUserInfoViewModel>().HasNoKey();
            modelBuilder.Entity<SenderReceiverDetailViewModal>().HasNoKey();
            modelBuilder.Entity<UsersIdWithAppIdParmsViewModel>().HasNoKey();
            modelBuilder.Entity<OfficerDetailsByRoleNameViewModel>().HasNoKey();
            modelBuilder.Entity<GetlastClearanceViewModel>().HasNoKey();
            modelBuilder.Entity<PSLDashboardMajorCountBP_HUD_ViewModel>().HasNoKey();
            modelBuilder.Entity<ShopEmployeeDetailViewModel>().HasNoKey();
            modelBuilder.Entity<BocwContractorListViewModel>().HasNoKey();
            modelBuilder.Entity<RegisteredMobileNumberHintRespViewModel>().HasNoKey();
            modelBuilder.Entity<DeviceRegistrationQRCodeViewModel>().HasNoKey();
            modelBuilder.Entity<GetTotalTakenTimeByDepartmentViewModel>().HasNoKey();
            modelBuilder.Entity<LicenceNoViewModel>().HasNoKey();
            modelBuilder.Entity<DofLicenceDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<LatestCircleInfoViewModel>().HasNoKey();
            modelBuilder.Entity<DeviceUniqueIdAndUserRefIdViewModel>().HasNoKey();
            modelBuilder.Entity<UserDeviceFoundRespViewModel>().HasNoKey();
            modelBuilder.Entity<AppFeePaymentReceiptDataViewModel>().HasNoKey();
            modelBuilder.Entity<TransferFactoryCircleUserInfoViewModel>().HasNoKey();
            modelBuilder.Entity<TransferALCCircleUserInfoViewModel>().HasNoKey();
            modelBuilder.Entity<PaymentDetailsListViewModel>().HasNoKey();
            modelBuilder.Entity<FindUserOfSameCircleViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicationAdditionalDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<WelfareFundDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<AnnualReturnDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<OtherActClearancesDataViewModel>().HasNoKey();
            modelBuilder.Entity<FormHViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryBacklogDataViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryBacklogDataRequestViewModel>().HasNoKey();
            modelBuilder.Entity<SysNApplicationDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetSysNCurrentStatusViewModel>().HasNoKey();
            modelBuilder.Entity<GetSysNClearancesIssuedsViewModel>().HasNoKey();
            modelBuilder.Entity<MappedBuildingPlanDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryInspectionDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetBuildingPlanDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetApplicationNotingLogsViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicationFeeDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetApplicationDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryBacklogCountViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryBacklogDataViewModel>().HasNoKey();
            modelBuilder.Entity<ShopLicenceDetailsForNightShiftPdfViewModel>().HasNoKey();
            modelBuilder.Entity<Inspection_RandomizeDashboardDataViewModel>().HasNoKey();
            modelBuilder.Entity<InspectioninfoViewModel>().HasNoKey();
            modelBuilder.Entity<LabourCircleUserDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetInspections_FactoryPerformaStepStatusViewModel>().HasNoKey();
            modelBuilder.Entity<Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionEstablishmentBasicDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryBacklogCountViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryBacklogDataViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryBacklogDataRequestViewModel>().HasNoKey();
            modelBuilder.Entity<SysNApplicationDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetSysNCurrentStatusViewModel>().HasNoKey();
            modelBuilder.Entity<GetSysNClearancesIssuedsViewModel>().HasNoKey();
            modelBuilder.Entity<MappedBuildingPlanDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryInspectionDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetBuildingPlanDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetApplicationNotingLogsViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicationFeeDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetApplicationDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryBacklogExcelDataViewModel>().HasNoKey();
            modelBuilder.Entity<GetLicenceNumberAndActTypeViewModel>().HasNoKey();
            modelBuilder.Entity<MPR_Factory_DashboardViewModel>().HasNoKey();
            modelBuilder.Entity<ActAndApplicationPurposeTypeCountsViewModel>().HasNoKey();
            modelBuilder.Entity<CircleAndApplicationPurposeTypeCountsViewModel>().HasNoKey();
            modelBuilder.Entity<DesignationAndApplicationPurposeTypeCountsViewModel>().HasNoKey();
            modelBuilder.Entity<FileWiseDataViewModel>().HasNoKey();
            modelBuilder.Entity<ProfileAndApplicationPurposeTypeCountsViewModel>().HasNoKey();
            modelBuilder.Entity<OfficerMISDashboardDetailsByRoleNameViewModel>().HasNoKey();
            modelBuilder.Entity<OfficialsRoleViewModel>().HasNoKey();
            modelBuilder.Entity<OfficerProfileAndActWiseDashboardDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<OfficerProfileAndCircleWiseDashboardDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<CurrentlyDesignatedOfficerDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<MISDashboard_FileWiseData_AppSearchParamsViewModel>().HasNoKey();
            modelBuilder.Entity<LabourWelfareSchemeApplicationParmsViewModel>().HasNoKey();
            modelBuilder.Entity<LabourWelfareSchemeApplicationCountsViewModel>().HasNoKey();
            modelBuilder.Entity<Establishment_EPFO>().HasNoKey();
            modelBuilder.Entity<CheckAlreadyInProcessViewTypeViewModel>().HasNoKey();
            modelBuilder.Entity<PSLDashboardCircleWiseCountViewModel>().HasNoKey();
            modelBuilder.Entity<GetRedirectUrlViewModel>().HasNoKey();
            modelBuilder.Entity<StabiltyAcknoweldgementReceiptViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionFactoryDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GenerateDisputeNoViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryLicenceDetailsForNightShiftPdfViewModel>().HasNoKey();
            modelBuilder.Entity<RawTokenSerializedDataViewModel>().HasNoKey();
            modelBuilder.Entity<AuthTokenSerializedDataViewModel>().HasNoKey();
            modelBuilder.Entity<LWBApplicationDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GenerateDisputeNoViewModel>().HasNoKey();
            modelBuilder.Entity<FactoryLicenceDetailsForNightShiftPdfViewModel>().HasNoKey();
            modelBuilder.Entity<InformationViewModel>().HasNoKey();
            modelBuilder.Entity<PaymentDetailsByLicenceNoViewModel>().HasNoKey();
            modelBuilder.Entity<GetShareStatusRequestDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetFactoryCirlceViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionTransferInfoViewModel>().HasNoKey();
            modelBuilder.Entity<MisInspectionDashboardDataViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionApplicationinfoViewModel>().HasNoKey();
            modelBuilder.Entity<GetAllDirtyApplicationsViewModel>().HasNoKey();
            modelBuilder.Entity<Licence_CL_PE_Contractor_ViewModel>().HasNoKey();
            modelBuilder.Entity<AdhaarVerifyViewModel>().HasNoKey();
            modelBuilder.Entity<DesignatedOfficerDetailViewModel>().HasNoKey();
            modelBuilder.Entity<AppSubmissionResponseViewModel>().HasNoKey();
            modelBuilder.Entity<LWFMonthWiseContributionViewModel>().HasNoKey();
            modelBuilder.Entity<LWFEmployeeContributionViewModel>().HasNoKey();
            modelBuilder.Entity<LegacyUserDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<ToDoActivityLogViewModel>().HasNoKey();
            modelBuilder.Entity<ToDoUserWiseActivityViewModel>().HasNoKey();
            modelBuilder.Entity<ToDoActivityWiseStepViewModel>().HasNoKey();
            modelBuilder.Entity<ToDoTicketDetailViewModel>().HasNoKey();

            modelBuilder.Entity<GetAppActionDocumentsViewModel>().HasNoKey();
            modelBuilder.Entity<LicenceNumberDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetInspectionNotingLogsViewModel>().HasNoKey();
            modelBuilder.Entity<UpdateAlternateContactDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<AlternateContactDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionComplianceinfoViewModel>().HasNoKey();
            modelBuilder.Entity<UserLoginDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetlastTerminationDateViewModel>().HasNoKey();
            modelBuilder.Entity<CaptchaCodeViewModel>().HasNoKey();
            modelBuilder.Entity<CaptchaResultViewModel>().HasNoKey();
            modelBuilder.Entity<ALCCircleManagerViewModel>().HasNoKey();
            modelBuilder.Entity<GetAllClearanceDateSlabsViewModel>().HasNoKey();
            modelBuilder.Entity<ProjectSiteEstablishmentBasicDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetUserRoleAndProfileDetailViewModel>().HasNoKey();
            modelBuilder.Entity<UserRefIdsAsPerIpinViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicableRaiseFeeHeadsViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionResultViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionTransferLogsViewModel>().HasNoKey();
            modelBuilder.Entity<HRMS_OfficerDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<HRMS_RoleIdByHRMSServiceCodeViewModel>().HasNoKey();
            modelBuilder.Entity<LegacyAppPaymentDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<PendingApplicationDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<PendingApplicationDetailsByServiceCodeViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicationCurrentStatusViewModel>().HasNoKey();
            modelBuilder.Entity<ServiceListViewModel>().HasNoKey();
            modelBuilder.Entity<GetPendencyCountViewModel>().HasNoKey();
            modelBuilder.Entity<GetPendingAppCountsViewModel>().HasNoKey();
            modelBuilder.Entity<EsamikshaCountsViewModel>().HasNoKey();
            modelBuilder.Entity<DepartmentOfficialDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<DepartmentOfficialListViewModel>().HasNoKey();
            modelBuilder.Entity<ElapsedApplicationViewModel>().HasNoKey();

            modelBuilder.Entity<GetReportByDepartmentViewModel>().HasNoKey();
            modelBuilder.Entity<GetReportByAuthorityCountsViewModel>().HasNoKey();
            modelBuilder.Entity<GetReportByServiceCountsViewModel>().HasNoKey();


            modelBuilder.Entity<AppActionTime_MutualProcessFlagViewModel>().HasNoKey();
            modelBuilder.Entity<PreCheckViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicationToBeElapsedViewModel>().HasNoKey();
            modelBuilder.Entity<AllowedReceiverDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<GetApplciationLogsViewModel>().HasNoKey();
            modelBuilder.Entity<GetApplciationReActivatedData>().HasNoKey();
            modelBuilder.Entity<ApplicationInitiateResponseViewModel>().HasNoKey();
            modelBuilder.Entity<GetDeemedApplciationsData>().HasNoKey();
            modelBuilder.Entity<AnnualReturnWelfareFundViewModel>().HasNoKey();
            modelBuilder.Entity<EmpanelledPersonDetailsByRoleNameViewModel>().HasNoKey();

            //modelBuilder.Entity<GetAdminDashboardDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<AdminDashboardUserDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<AdminDashboardEstablishmentDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<AdminDashboardApplicationDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<AdminDashboardActionLogViewModel>().HasNoKey();
            modelBuilder.Entity<AdminDashboardPaymentDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<AdminDashboardApprovalDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicationEscalationSlabViewModel>().HasNoKey();
            modelBuilder.Entity<ApplicationToBeEscalatedViewModel>().HasNoKey();
            modelBuilder.Entity<EscalatedApplicationViewModel>().HasNoKey();
            modelBuilder.Entity<OfficerUserDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<EscalationOffcerViewModel>().HasNoKey();
            modelBuilder.Entity<AllTransactionsByAppRefIdViewModel>().HasNoKey();

            modelBuilder.Entity<FundAndUnpaidWagesViewModel>().HasNoKey();
            modelBuilder.Entity<MonthWiseContributionDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<MonthWiseEmployeeDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<WelfareFundAndWagesDataViewModel>().HasNoKey();
            modelBuilder.Entity<FundAndWagesCircleWiseDataViewModel>().HasNoKey();
            modelBuilder.Entity<FactoriesListViewModel>().HasNoKey();

            modelBuilder.Entity<All_Act_Deemed_ProcessEngineLogsViewModel>().HasNoKey();
            modelBuilder.Entity<DeemedApplicationTimeLineViewModel>().HasNoKey();
            modelBuilder.Entity<PSIECUserViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionLabourCircleViewModel>().HasNoKey();
            modelBuilder.Entity<Deemed_ProcessFilesLogViewModel>().HasNoKey();
            modelBuilder.Entity<PWBContributionDetailsViewModel>().HasNoKey();

            modelBuilder.Entity<EstablishmentWisePaymentDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<MPR_Labour_DashboardViewModel>().HasNoKey();
            modelBuilder.Entity<MprDataViewModel>().HasNoKey();
            modelBuilder.Entity<AnnualReturnDashboardViewModel>().HasNoKey();
            modelBuilder.Entity<EstablishmentDataViewModel>().HasNoKey();
            modelBuilder.Entity<ErrorLogParamsViewModel>().HasNoKey();
            modelBuilder.Entity<ElapsedPhaseApplicationViewModel>().HasNoKey();
            modelBuilder.Entity<ShopBacklogDataViewModel>().HasNoKey();
            modelBuilder.Entity<LegacyShopFormDataViewModel>().HasNoKey();
            modelBuilder.Entity<WorkingHourSlabViewModel>().HasNoKey();
            modelBuilder.Entity<MyOfficeUserProfileWiseDataViewModel>().HasNoKey();
            modelBuilder.Entity<UserDetailsByRoleNameViewModel>().HasNoKey();
            modelBuilder.Entity<PendencyReportViewModel>().HasNoKey();
            modelBuilder.Entity<ContractLabourLicenceValidityViewModel>().HasNoKey();
            modelBuilder.Entity<BocwMobileAppUserDataViewModel>().HasNoKey();
            modelBuilder.Entity<MyOfficeUserValidationViewModel>().HasNoKey();
            modelBuilder.Entity<TradeUnionLicenceValidateViewModel>().HasNoKey();
            modelBuilder.Entity<EstablishmentAndUserDetailViewModel>().HasNoKey();
            modelBuilder.Entity<ApprovedDataViewModel>().HasNoKey();
            modelBuilder.Entity<RegisteredFactoryCircleWiseViewModel>().HasNoKey();

            modelBuilder.Entity<WelfareFundStatusDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<WelfareSchemesDetailsViewModel>().HasNoKey();

            modelBuilder.Entity<WelfareFundReceiptDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionDetailsBylicenceViewModel>().HasNoKey();
            modelBuilder.Entity<RegisteredFactoryCircleWiseViewModel>().HasNoKey();
            modelBuilder.Entity<UnpaidWagesDetailsBylicenceViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionDetailsBylicenceViewModel>().HasNoKey();
            modelBuilder.Entity<WelfareFundReceiptDetailsViewModel>().HasNoKey();
            modelBuilder.Entity<TransperancyReportCountsViewModel>().HasNoKey();
            modelBuilder.Entity<LWFTransperancyReportViewModel>().HasNoKey();
            modelBuilder.Entity<InspectionFactoryAllotmentDetailsViewModel>().HasNoKey();


            modelBuilder.Entity<DistrictLgd>()
              .HasMany<DistrictLevelUserMapping>(estb => estb.DistrictLevelUserMappings)
              .WithOne(dist => dist.Application_DistrictLgd)
              .HasForeignKey(fk => fk.DistrictLgdRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
              .HasMany<BuildingPlanHUDPaymentDetail>(s => s.BuildingPlanHUDPaymentDetails)
              .WithOne(ad => ad.Application)
              .HasForeignKey(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeesHeader>()
              .HasMany<BuildingPlanHUDPaymentDetail>(s => s.BuildingPlanHUDPaymentDetails)
              .WithOne(ad => ad.FeesHeader)
              .HasForeignKey(ad => ad.FeeHeaderRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
             .HasMany<RoleWiseAllowedActionCode>(s => s.RoleWiseAllowedActionCodes)
             .WithOne(ad => ad.Document)
             .HasForeignKey(ad => ad.DocRefId)
             .OnDelete(DeleteBehavior.Restrict);

            // Shop Licence Relation
            modelBuilder.Entity<Application>()
                 .HasOne<ShopLicence_GeneralDetail>(s => s.ShopLicence_GeneralDetail)
                 .WithOne(ad => ad.Application)
                 .HasForeignKey<ShopLicence_GeneralDetail>(ad => ad.AppRefId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShopLicence_GeneralDetail>()
             .HasMany<ShopLicence_EmployeeDetail>(s => s.ShopLicence_EmployeeDetails)
             .WithOne(ad => ad.ShopLicence_GeneralDetail)
             .HasForeignKey(ad => ad.ShopLicenceRefId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
               .HasOne<ApplicationLicenceNoMapping>(s => s.ApplicationLicenceNoMapping)
               .WithOne(ad => ad.Application)
               .HasForeignKey<ApplicationLicenceNoMapping>(ad => ad.AppRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LWB_Contribution>()
             .HasMany<LWB_Contribution_Employee>(s => s.LWB_Contribution_Employees)
             .WithOne(ad => ad.LWB_Contributions)
             .HasForeignKey(ad => ad.LWB_FundContributionRefId)
             .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Application>()
              .HasOne<Licence_TradeUnion>(s => s.Licence_TradeUnion)
              .WithOne(ad => ad.Application)
              .HasForeignKey<Licence_TradeUnion>(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Licence_TradeUnion>()
            .HasMany<Licence_TradeUnion_Officer>(s => s.Licence_TradeUnion_Officer)
            .WithOne(ad => ad.Licence_TradeUnion)
            .HasForeignKey(ad => ad.TradeUnionRefId)
            .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Application>()
               .HasOne<Licence_Factory_GeneralDetail>(s => s.Licence_Factory_GeneralDetails)
               .WithOne(ad => ad.Application)
               .HasForeignKey<Licence_Factory_GeneralDetail>(ad => ad.AppRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Licence_Factory_GeneralDetail>()
               .HasOne<Licence_Factory_OccupierAndManagerDetail>(s => s.Licence_Factory_OccupierAndManagerDetail)
               .WithOne(ad => ad.Licence_Factory_GeneralDetail)
               .HasForeignKey<Licence_Factory_OccupierAndManagerDetail>(ad => ad.FactoryLicenceRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
              .HasOne<Licence_ContractLabour_GeneralDetail>(s => s.Licence_ContractLabour_GeneralDetails)
              .WithOne(ad => ad.Application)
              .HasForeignKey<Licence_ContractLabour_GeneralDetail>(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
               .HasOne<BuildingPlanFactory_Declaration_Stability_Certificate>(s => s.BuildingPlanFactory_Declaration_Stability_Certificate)
                .WithOne(ad => ad.Application)
                .HasForeignKey<BuildingPlanFactory_Declaration_Stability_Certificate>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Licence_BocwAct_GeneralDetail>()
               .HasMany<Licence_BocwAct_ContractorDetail>(s => s.Licence_BocwAct_ContractorDetail)
               .WithOne(ad => ad.Licence_BocwAct_GeneralDetail)
               .HasForeignKey(ad => ad.BocwEstablishmentRegistrationRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                  .HasOne<Licence_MotorTransport>(s => s.Licence_MotorTransport)
                  .WithOne(ad => ad.Application)
                  .HasForeignKey<Licence_MotorTransport>(ad => ad.AppRefId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProfile>()
                 .HasOne<UserProfile_HRMS_CodeMapping>(s => s.UserProfile_HRMS_CodeMapping)
                 .WithOne(ad => ad.UserProfile)
                 .HasForeignKey<UserProfile_HRMS_CodeMapping>(ad => ad.UserProfileRefId)
                 .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Application>()
              .HasMany<Payments_RaisedFee>(s => s.Payments_RaisedFee)
              .WithOne(ad => ad.Application)
              .HasForeignKey(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeesHeader>()
              .HasMany<Payments_RaisedFee>(s => s.Payments_RaisedFee)
              .WithOne(ad => ad.FeesHeader)
              .HasForeignKey(ad => ad.FeeHeaderRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Licence_BuildingPlan_PSIEC_GeneralDetail>(s => s.Licence_BuildingPlan_PSIEC_GeneralDetails)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Licence_BuildingPlan_PSIEC_GeneralDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Licence_ISM_ContractLabour_GeneralDetail>(s => s.Licence_ISM_ContractLabour_GeneralDetails)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Licence_ISM_ContractLabour_GeneralDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Department>()
              .HasMany<DepartmentRoleMapping>(s => s.DepartmentRoleMappings)
              .WithOne(ad => ad.Department)
              .HasForeignKey(ad => ad.DepartmentRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_1_Registration>(s => s.OSH_Form_1_Registration)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_1_Registration>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_1_Registration_Factory>(s => s.OSH_Form_1_Registration_Factory)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_1_Registration_Factory>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
             .HasMany<OSH_Form_1_Registration_Factory>(s => s.OSH_Form_1_Registration_Factories)
             .WithOne(ad => ad.DistrictLgd)
             .HasForeignKey(ad => ad.DistrictRefId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TehsilLgd>()
             .HasMany<OSH_Form_1_Registration_Factory>(s => s.OSH_Form_1_Registration_Factories)
             .WithOne(ad => ad.TehsilLgd)
             .HasForeignKey(ad => ad.TehsilRefId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_1_Registration_BOCW>(s => s.OSH_Form_1_Registration_BOCW)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_1_Registration_BOCW>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_1_Registration_EmployeeDetail>(s => s.OSH_Form_1_Registration_EmployeeDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_1_Registration_EmployeeDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_1_Registration_EPFO_ESIC_Detail>(s => s.OSH_Form_1_Registration_EPFO_ESIC_Detail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_1_Registration_EPFO_ESIC_Detail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_1_Registration_EmployerDetail>(s => s.OSH_Form_1_Registration_EmployerDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_1_Registration_EmployerDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_1_Registration_PrincipalEmployerDetail>(s => s.OSH_Form_1_Registration_PrincipalEmployerDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_1_Registration_PrincipalEmployerDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_1_Registration_ContractorDetail>(s => s.OSH_Form_1_Registration_ContractorDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_1_Registration_ContractorDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
              .HasMany<OSH_Form_1_Registration_EmployerDetail>(s => s.OSH_Form_1_Registration_EmployerDetail)
              .WithOne(ad => ad.DistrictLgd)
              .HasForeignKey(ad => ad.DistrictRefId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TehsilLgd>()
             .HasMany<OSH_Form_1_Registration_EmployerDetail>(s => s.OSH_Form_1_Registration_EmployerDetail)
             .WithOne(ad => ad.TehsilLgd)
             .HasForeignKey(ad => ad.TehsilRefId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DistrictLgd>()
             .HasMany<OSH_Form_1_Registration_PrincipalEmployerDetail>(s => s.OSH_Form_1_Registration_PrincipalEmployerDetail)
             .WithOne(ad => ad.DistrictLgd)
             .HasForeignKey(ad => ad.DistrictRefId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TehsilLgd>()
             .HasMany<OSH_Form_1_Registration_PrincipalEmployerDetail>(s => s.OSH_Form_1_Registration_PrincipalEmployerDetail)
             .WithOne(ad => ad.TehsilLgd)
             .HasForeignKey(ad => ad.TehsilRefId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DistrictLgd>()
            .HasMany<OSH_Form_1_Registration_ContractorDetail>(s => s.OSH_Form_1_Registration_ContractorDetail)
            .WithOne(ad => ad.DistrictLgd)
            .HasForeignKey(ad => ad.DistrictRefId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TehsilLgd>()
             .HasMany<OSH_Form_1_Registration_ContractorDetail>(s => s.OSH_Form_1_Registration_ContractorDetail)
             .WithOne(ad => ad.TehsilLgd)
             .HasForeignKey(ad => ad.TehsilRefId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
               .HasOne<OSH_Form_1_Registration_MotorTransportDetail>(s => s.OSH_Form_1_Registration_MotorTransportDetail)
               .WithOne(ad => ad.Application)
               .HasForeignKey<OSH_Form_1_Registration_MotorTransportDetail>(ad => ad.AppRefId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_30_CommonLicense_Establishment>(s => s.OSH_Form_30_CommonLicense_Establishment)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_30_CommonLicense_Establishment>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_30_CommonLicense_Factory>(s => s.OSH_Form_30_CommonLicense_Factory)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_30_CommonLicense_Factory>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_30_CommonLicense_ContractLabour>(s => s.OSH_Form_30_CommonLicense_ContractLabour)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_30_CommonLicense_ContractLabour>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_21_ContractLabour_General_Detail>(s => s.OSH_Form_21_ContractLabour_General_Detail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_21_ContractLabour_General_Detail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_21_ContractLabour_Employee_Detail>(s => s.OSH_Form_21_ContractLabour_Employee_Detail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_21_ContractLabour_Employee_Detail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_21_ContractLabour_Establishment_Detail>(s => s.OSH_Form_21_ContractLabour_Establishment_Detail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_21_ContractLabour_Establishment_Detail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<OSH_Form_21_ContractLabour_MigrantWorker>(s => s.OSH_Form_21_ContractLabour_MigrantWorker)
                .WithOne(ad => ad.Application)
                .HasForeignKey<OSH_Form_21_ContractLabour_MigrantWorker>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
              .HasOne<Complaint_Review_OfDismissal>(s => s.Complaint_Review_OfDismissal)
              .WithOne(ad => ad.Application)
              .HasForeignKey<Complaint_Review_OfDismissal>(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);



            #region Samadhan portal

            modelBuilder.Entity<Application>()
                .HasOne<WorkerDetail>(s => s.WorkerDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<WorkerDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
                .HasMany(d => d.PermanentWorkerDetails)
                .WithOne(w => w.PermanentDistrictLgd)
                .HasForeignKey(w => w.PermanentDistrictRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
                .HasMany(d => d.CorrespondenceWorkerDetails)
                .WithOne(w => w.CorrespondenceDistrictLgd)
                .HasForeignKey(w => w.CorrespondenceDistrictRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppComplaintTypeMapping>()
                .HasOne(m => m.ComplaintsCategory)
                .WithMany(c => c.AppComplaintTypeMappings)
                .HasForeignKey(m => m.ComplaintsCategoryRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppComplaintTypeMapping>()
                .HasOne(m => m.Application)
                .WithMany(a => a.AppComplaintTypeMappings)
                .HasForeignKey(m => m.AppRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasMany<Complaint_EmployerORContractorDetail>(s => s.Complaint_EmployerORContractorDetails)
                .WithOne(ad => ad.Application)
                .HasForeignKey(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
                .HasMany<Complaint_EmployerORContractorDetail>(s => s.Complaint_EmployerORContractorDetails)
                .WithOne(ad => ad.DistrictLgd)
                .HasForeignKey(ad => ad.DistrictRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
              .HasOne<Complaint_WorkplaceDetail>(s => s.Complaint_WorkplaceDetails)
              .WithOne(ad => ad.Application)
              .HasForeignKey<Complaint_WorkplaceDetail>(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
               .HasMany<Complaint_WorkplaceDetail>(s => s.Complaint_WorkplaceDetails)
               .WithOne(ad => ad.DistrictLgd)
               .HasForeignKey(ad => ad.DistrictRefId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_EstablishmentDetail>(s => s.Complaint_EstablishmentDetails)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_EstablishmentDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DistrictLgd>()
                .HasMany<Complaint_EstablishmentDetail>(s => s.Complaint_EstablishmentDetails)
                .WithOne(ad => ad.DistrictLgd)
                .HasForeignKey(ad => ad.DistrictRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_GratuityClaim>(s => s.Complaint_GratuityClaims)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_GratuityClaim>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_MaternityBenefitComplaint>(s => s.Complaint_MaternityBenefitComplaints)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_MaternityBenefitComplaint>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_Claim_CodeOnWage>(s => s.Complaint_Claim_CodeOnWages)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_Claim_CodeOnWage>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_MinimumWage>(s => s.Complaint_MinimumWage)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_MinimumWage>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_MinimumWage>(s => s.Complaint_MinimumWage)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_MinimumWage>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasMany<Complaint_MinimumWagesPeriodAmt>(x => x.Complaint_MinimumWagesPeriodAmts)
                .WithOne(x => x.Application)
                .HasForeignKey(x => x.AppRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_Wages_WkDay>(s => s.Complaint_Wages_WkDay)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_Wages_WkDay>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasMany<Complaint_Wages_WkDay_PeriodAmt>(x => x.Complaint_Wages_WkDay_PeriodAmts)
                .WithOne(x => x.Application)
                .HasForeignKey(x => x.AppRefId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Application>()
                .HasOne<Complaint_Wages_OT>(s => s.Complaint_Wages_OT)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_Wages_OT>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasMany<Complaint_Wages_OT_PeriodAmt>(x => x.Complaint_Wages_OT_PeriodAmts)
                .WithOne(x => x.Application)
                .HasForeignKey(x => x.AppRefId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Application>()
                .HasOne<Complaint_Wages_Not_Paid>(s => s.Complaint_Wages_Not_Paid)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_Wages_Not_Paid>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasMany<Complaint_Wages_Not_Paid_PeriodAmt>(x => x.Complaint_Wages_Not_Paid_PeriodAmts)
                .WithOne(x => x.Application)
                .HasForeignKey(x => x.AppRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_Wages_Unauth_Deduct>(s => s.Complaint_Wages_Unauth_Deduct)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_Wages_Unauth_Deduct>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasMany<Complaint_Wages_Unauth_Deduct_PeriodAmt>(x => x.Complaint_Wages_Unauth_Deduct_PeriodAmts)
                .WithOne(x => x.Application)
                .HasForeignKey(x => x.AppRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_Non_Pay_Bonus>(s => s.Complaint_Non_Pay_Bonus)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_Non_Pay_Bonus>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasMany<Complaint_Non_Pay_Bonus_PeriodAmt>(x => x.Complaint_Non_Pay_Bonus_PeriodAmts)
                .WithOne(x => x.Application)
                .HasForeignKey(x => x.AppRefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
              .HasOne<Complaint_Appeal>(s => s.Complaint_Appeal)
              .WithOne(ad => ad.Application)
              .HasForeignKey<Complaint_Appeal>(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);


            #region Recovery of money
            modelBuilder.Entity<Application>()
                .HasOne<Complaint_RecOfMon_GeneralDetail>(s => s.Complaint_RecOfMon_GeneralDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_RecOfMon_GeneralDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_RecOfMon_MoneyDueDetail>(s => s.Complaint_RecOfMon_MoneyDueDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_RecOfMon_MoneyDueDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_RecOfMon_SettlementDetail>(s => s.Complaint_RecOfMon_SettlementDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_RecOfMon_SettlementDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_RecOfMon_AwardDetail>(s => s.Complaint_RecOfMon_AwardDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_RecOfMon_AwardDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_RecOfMon_NoticePayDetail>(s => s.Complaint_RecOfMon_NoticePayDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_RecOfMon_NoticePayDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_RecOfMon_RetrenchmentCompDetail>(s => s.Complaint_RecOfMon_RetrenchmentCompDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_RecOfMon_RetrenchmentCompDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasOne<Complaint_RecOfMon_LayOffDetail>(s => s.Complaint_RecOfMon_LayOffDetail)
                .WithOne(ad => ad.Application)
                .HasForeignKey<Complaint_RecOfMon_LayOffDetail>(ad => ad.AppRefId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
                .HasMany<Complaint_RecOfMon_LayOffCompDetail>(x => x.Complaint_RecOfMon_LayOffCompDetails)
                .WithOne(x => x.Application)
                .HasForeignKey(x => x.AppRefId)
                .OnDelete(DeleteBehavior.Cascade);


            #endregion
            #endregion

            #region Industrial Disputes

            modelBuilder.Entity<Application>()
              .HasOne<Complaint_IndustrialDispute>(s => s.Complaint_IndustrialDispute)
              .WithOne(ad => ad.Application)
              .HasForeignKey<Complaint_IndustrialDispute>(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
              .HasOne<Complaint_IndustrialDisputeReasonMapping>(s => s.Complaint_IndustrialDisputeReasonMapping)
              .WithOne(ad => ad.Application)
              .HasForeignKey<Complaint_IndustrialDisputeReasonMapping>(ad => ad.AppRefId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Application>()
            .HasOne<Complaint_IndustrialDisputeReliefSoughtMapping>(s => s.Complaint_IndustrialDisputeReliefSoughtMapping)
            .WithOne(ad => ad.Application)
            .HasForeignKey<Complaint_IndustrialDisputeReliefSoughtMapping>(ad => ad.AppRefId)
            .OnDelete(DeleteBehavior.Restrict);
            #endregion

        }
    }
}