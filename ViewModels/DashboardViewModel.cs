using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class DashboardViewModel
    {
        public Int64 AppRefId { get; set; }

        public int ApplicationPurposeType { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public EstablishmentTypeEnum ApplicationType { get; set; }
        public string ApplicationTypeDesc { get; set; }
        public AppActionTypeEnum AppActionType { get; set; }
        public DateTime ActionDate { get; set; }
        //public Int64 UserRefId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDigitalSignatureRequired { get; set; }
        public bool IsDigitalSignatureVerified { get; set; }
        public bool IsLocked { get; set; }
        public bool IsAllowEdit { get; set; }
        public bool IsFeeApplicable { get; set; }
        public Int64? IdentityKey { get; set; }
        public int ApplicationLifeCycleStatusType { get; set; }
        public string ActionName { get; set; }
        public string ActionPublicName { get; set; }
        public Int64 EstablishmentId { get; set; }

        public int ProjectSiteVersion { get; set; }

        public string CurrentPendingWith { get; set; }

        public string NormalizedName { get; set; }
    }

    public class DashboardAlreadyClearenceViewModel
    {
        public string PublicAppRefNum { get; set; }
        public Int64 AppRefId { get; set; }

        public Int64 ProjectSiteRefId { get; set; }
        public DateTime ActionDate { get; set; }
        public Int64 ActionTakenDaysCount { get; set; }
        public Int64 ActionTakenHoursCount { get; set; }
        public string LicenseIssuedBy { get; set; }
        public string Designation { get; set; }
        public Int64 EstablishmentId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public int ApplicationPurposeType { get; set; }
        public string LicenceNumber { get; set; }
        public DateTime ExpiredOn { get; set; }
        public int ProjectSiteVersion { get; set; }


    }
    public class StoreProcedureParm
    {
        public string ParmName { get; set; }
        public string ParmValue { get; set; }
        public bool isNumber { get; set; }
    }

    public class MajorDashboardCountViewModel
    {
        public Int64 TotalReceived { get; set; }
        public Int64 TotalCompleted { get; set; }
        public Int64 TotalPending { get; set; }
        public Int64 InObjection { get; set; }
        public Int64 Rejected { get; set; }
    }

    public class RecordsTypeCountWithListViewModel
    {
        public int InboxCount { get; set; }
        public int SentCount { get; set; }
        public int InProcessCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public List<RecordsTypeListViewModel> RecordsTypeList { get; set; }
    }

    public class RecordsTypeListViewModel
    {
        public Int64 ApplicationRefId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string ActionTakenBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string Remarks { get; set; }
        public Int64 ActionTakenDaysCount { get; set; }
        public Int64 ActionTakenHoursCount { get; set; }
        public string CurrentPendingWith { get; set; }
        public string ActionName { get; set; }
        public string ActionPublicName { get; set; }
        public int ActionCode { get; set; }
        public Int64 projectSiteRefId { get; set; }
        public int ApplicationType { get; set; }
        public int ApplicationPurposeType { get; set; }
        public Int64? IdentityKey { get; set; }
        public string NormalizedName { get; set; }
        public string ActionTakenByRoleName { get; set; }
        public string ActionTakenByRoleNormalizeName { get; set; }
        public string DistrictName { get; set; }
        public int MaxRows { get; set; }
        public int ProjectSiteVersion { get; set; }
        public string CircleName { get; set; }
        public Int64 ApplicationActionLogId { get; set; }
        public DateTime? ActionCanTakenUpto { get; set; }
        public bool IsTimeLineElapsed { get; set; }
        public ApplicationProcessPhaseLogTypeEnum? AppProcessPhaseType { get; set; }

        public bool IsTimeLineFlow { get; set; }
    }
    public class RecordTypeCountViewModel
    {
        public int ItemCount { get; set; }
    }

    public class RecordTypeMenuCountViewModel
    {
        public string RecordHead { get; set; }
        public int RecordCount { get; set; }
        public string HeadCode { get; set; }
        public int ApplicationType { get; set; }
        public string ApplicationTypeDesc { get; set; }
        public string ApplicationTypeShortDesc { get; set; }
    }
    public class OfficialDashboardContentViewModel
    {
        public ApplicationTypeEnum ApplicationType { get; set; }
        public List<RecordTypeMenuCountViewModel> RecordTypeMenuCount { get; set; }
        public List<RecordsTypeListViewModel> RecordsTypeList { get; set; }
    }
    public class ApplicationLogsViewModel
    {
        public string ApplicationRefNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Remarks { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionPublicName { get; set; }
        public string Checklist_Json { get; set; }
        public bool IsDocumentUploaded { get; set; }
        public Int64 AppDocumentRefId { get; set; }
        public string AttachmentName { get; set; }
        public Int64 ActionTakenHoursCount { get; set; }
        public Int64 ActionTakenDaysCount { get; set; }
        public Int64 ApplicationActionLogId { get; set; }
        public string ReceiverUserName  { get; set; }
        public string SenderUserName { get; set; }
        public int? ApplicationStatus { get; set; }
    }

    public class DataTableParamsViewModel
    {
        public string Id { get; set; }
        public Int64 AppRefId { get; set; }
        public DashboardRecordsHeaderTypeEnum DashboardRecordsHeaderType { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public int ApplicationLifeCycleStatusType { get; set; }
        public int AppActionType { get; set; }
        public bool ApplicationListAlso { get; set; }
        public string ColCode { get; set; }
        public string RoleId { get; set; }
        public string RootActivityRefIds { get; set; }
        public string SearchCode { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string FilterArray { get; set; }
    }

    public class ClearenceFileInfoViewModel
    {
        public List<DashboardAlreadyClearenceViewModel> AlreadyClearancesData { get; set; }
        public List<LegacyApprovedClearenceMapping> LegacyApprovedClearenceMappings { get; set; }
        public List<GetSysNClearancesIssuedsViewModel> GetSysNClearancesIssueds { get; set; }
        public List<WelfareFundStatusDetailsViewModel> WelfareFundDetails { get; set; }
        public List<WelfareSchemesDetailsViewModel> WelfareSchemesDetails { get; set; }
        public List<InspectionDetailsBylicenceViewModel> InspectionsDetails { get; set; }
        public List<UnpaidWagesDetailsBylicenceViewModel> UnpaidWagesDetails { get; set; }
    }

    //public class LongTypeViewModel
    //{
    //    public Int64 Value { get; set; }
    //}

    public class PSLDashboardMajorCountBP_HUD_ViewModel
    {
        public string CountType { get; set; }
        public string ColumnTitle { get; set; }
        public string RoleCode { get; set; }
        public string RoleDesc { get; set; }
        public string RoleId { get; set; }
        public int Counts { get; set; }
        public int ApplicationLifeCycleStatusType { get; set; }
        public int AppActionType { get; set; }
    }

    public class PSLDashboardMajorCountPivotViewModel
    {
        public PivotColDetailViewModel PivotColDetail { get; set; }
    }
    public class PivotColDetailViewModel
    {
        public string ColCode { get; set; }
        public string ColName { get; set; }
        public int ColSpan { get; set; }
        public List<RoleDetailViewModel> RoleDetailList { get; set; }
    }

    public class RoleDetailViewModel
    {
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string RoleId { get; set; }
        public int Counts { get; set; }
        public int ApplicationLifeCycleStatusType { get; set; }
        public int AppActionType { get; set; }
    }

    public class PSLDashboardMajorCountRequestParmsViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RoleName { get; set; }
    }

    public class PaymentDetailsListViewModel
    {
        public int MaxRows { get; set; }
        public Int64 AppId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string ActionName { get; set; }
        public string ApplicantName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public decimal TotalPaymentReceived { get; set; }
    }

    public class FindUserOfSameCircleViewModel
    {
        public string UserRefId { get; set; }
    }

    public class OtherActClearancesDataViewModel
    {
        public string PublicAppRefNum { get; set; }
        public int ApplicationType { get; set; }
        public string ApplicationName { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string ActionTakenBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string Remarks { get; set; }
        public Int64 ActionTakenDaysCount { get; set; }
        public string ActionName { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public int ActionCode { get; set; }
        public string ActionPublicName { get; set; }
        public Int64 ActionTakenHoursCount { get; set; }
        public string CurrentPendingWith { get; set; }
        public string NormalizedName { get; set; }
        public string ActionTakenByRoleName { get; set; }
        public string ActionTakenByRoleNormalizeName { get; set; }
        public string DistrictName { get; set; }
    }

    public class PSLDashboardActWiseCountViewModel
    {
        public int ApplicationType { get; set; }
        public string ApplicationName { get; set; }
        public int TotalReceived { get; set; }
        public int TotalApproved { get; set; }
        public int TotalInObjection { get; set; }
        public int TotalPending { get; set; }
        public int TotalDeemed { get; set; }
        public int TotalRejected { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class PSLDashboardCircleWiseCountViewModel
    {
        public int ApplicationType { get; set; }
        public Int64 CircleRefId { get; set; }
        public string CircleName { get; set; }
        public string OfficerName { get; set; }
        public int TotalReceived { get; set; }
        public int TotalApproved { get; set; }
        public int TotalInObjection { get; set; }
        public int TotalPending { get; set; }
        public int TotalDeemed { get; set; }
        public int TotalRejected { get; set; }
    }

    public class OfficerWiseDataTableParamsViewModel
    {
        public string ColCode { get; set; }
        public Int64 CircleRefId { get; set; }
        public Int64 ApplicationType { get; set; }
        public string RoleId { get; set; }
        public string SearchCode { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string FilterArray { get; set; }
    }

    public class FactoryBacklogCountViewModel
    {
        public int ApprovedCount { get; set; }
        public int InObjectionCount { get; set; }
        public int DeregisteredCount { get; set; }
        public int BacklogCount { get; set; }

    }

    public class FactoryBacklogDataViewModel
    {
        public string PublicAppRefNum { get; set; }
        public Int64 AppId { get; set; }
        public Int64 FactoryLicenceId { get; set; }
        public string establishmentname { get; set; }
        public string address { get; set; }
        public string ProjectPurpose { get; set; }
        public Int64 Workers_MaxDuringYear { get; set; }
        public decimal PowerKW_MaxProposed { get; set; }
        public string OwnerName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public Int64? FactoryHazardousCategoryType { get; set; }
        public Int64 FactorySessionCategoryType { get; set; }
        public string FactorySectionCategoryType_NVarChar { get; set; }
        public Int32 MaxRows { get; set; }
        public Int32 IsLegacy { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public string applicantuserid { get; set; }
        public Int64 applicantprofileid { get; set; }
        public Int64 StatusId { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public string OccupierFullName { get; set; }
        public string OccupierFullAddress { get; set; }
        public string ManagerFullName { get; set; }
        public string ManagerFullAddress { get; set; }
        public string LicenceNumber { get; set; }
        public string ClearanceIssuedOn { get; set; }
        public string ClearanceExpiredOn { get; set; }
        public int FactoryCategoryType { get; set; }
    }

    public class FactoryBacklogDataRequestViewModel
    {
        public string Id { get; set; }
        public Int64 AppId { get; set; }
        public Int64? FactoryHazardousCategoryType { get; set; }
        public Int64 FactorySessionCategoryType { get; set; }
        public string FactorySectionCategoryType_NVarChar { get; set; }
        public decimal PowerKW_MaxProposed_original { get; set; }
        public Int64? FactoryHazardousCategoryType_original { get; set; }
        public Int64 FactorySessionCategoryType_original { get; set; }
        public string FactorySectionCategoryType_NVarChar_original { get; set; }
        public int FactoryCategoryType { get; set; }
        public int FactoryCategoryType_original { get; set; }
        public int FactoryOrganisationCategoryType { get; set; }
        public int FactoryOrganisationCategoryType_original { get; set; }
        public Int32 IsLegacy { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public string remarks { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
    }
    public class SysNApplicationDetailsViewModel
    {
        public List<GetApplicationDetailsViewModel> ApplicationDetails { get; set; }
        public List<GetSysNCurrentStatusViewModel> CurrentStatus { get; set; }
        public List<GetSysNClearancesIssuedsViewModel> Clearances { get; set; }
        public List<WelfareFundDetailsViewModel> WelfareFundDetails { get; set; }
        public List<AnnualReturnDetailsViewModel> AnnualReturnDetails { get; set; }
        public List<FactoryInspectionDetailsViewModel> FactoryInspectionDetails { get; set; }
        public List<MappedBuildingPlanDetailsViewModel> BuildingPlanDetails { get; set; }
    }

    public class GetApplicationDetailsViewModel
    {
        public string estdname { get; set; }
        public string SiteAddress { get; set; }
        public string ProjectPurpose { get; set; }
        public Int64 AppId { get; set; }
        public string FileNumber { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }

    public class GetSysNCurrentStatusViewModel
    {
        public string estdname { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public DateTime StatusDate { get; set; }
        public Int64 StatusId { get; set; }
        public string FileNumber { get; set; }
        public string SiteAddress { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string ProjectPurpose { get; set; }
        public string ActName { get; set; }
        public string StatusDesc { get; set; }
        public Int32 isLegacy { get; set; }
    }

    public class GetSysNClearancesIssuedsViewModel
    {
        public string TokenNumber { get; set; }
        public string ActName { get; set; }
        public DateTime ClearanceIssuedOn { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string LicenceNo { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public Int32 IsLegacy { get; set; }
        public string NAR { get; set; }
        public string LicencePath { get; set; }
        public Int64 ProjectSiteRefId { get; set; }

    }

  
    public class GetlastClearanceViewModel
    {
        public string LicenceNo { get; set; }
        public string LicencePath { get; set; }
        public DateTime ClearanceIssuedOn { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string FileNo { get; set; }
        public bool IsLegacyApplication { get; set; }

    }



    public class MappedBuildingPlanDetailsViewModel
    {
        public Int64 FacAppid { get; set; }
        public string tokennumber { get; set; }
        public Int64 BpAppid { get; set; }
        public string LicenceNo { get; set; }
        public DateTime IssuedOn { get; set; }
        public string licencePath { get; set; }
        public string NAR { get; set; }
        public Int64 AppformId { get; set; }
    }

    public class FactoryInspectionDetailsViewModel
    {
        public string FactoryName { get; set; }
        public string FactoryOwner { get; set; }
        public string FactoryInspectorName { get; set; }
        public string ShiftTiming { get; set; }
        public string MaleEmployeeNumber { get; set; }
        public string FemaleEmployeeNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public string Violation { get; set; }
        public string Remarks { get; set; }
        public DateTime InspectionDate { get; set; }
        public Int64 FIId { get; set; }
        public Int32 RandomizationYear { get; set; }
        public Int32 RandomizationMonth { get; set; }
    }

    public class GetBuildingPlanDetailsViewModel
    {
        public string PublicAppRefNum { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public Int32 IsLegacy { get; set; }
    }

    public class GetApplicationNotingLogsViewModel
    {
        public string StatusDesc { get; set; }
        public DateTime StatusDate { get; set; }
        public Int64 AppPBIPActionLogId { get; set; }
        public string OfficerName { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
    }

    public class ApplicationFeeDetailsViewModel
    {
        public string ActName { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string FeeType { get; set; }

    }

    public class FactoryBacklogExcelDataViewModel
    {
        public string PublicAppRefNum { get; set; }
        public Int64 AppId { get; set; }
        public Int64 FactoryLicenceId { get; set; }
        public string establishmentname { get; set; }
        public string address { get; set; }
        public string ProjectPurpose { get; set; }
        public Int64 Workers_MaxDuringYear { get; set; }
        public decimal PowerKW_MaxProposed { get; set; }
        public string OwnerName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public Int64? FactoryHazardousCategoryType { get; set; }
        public Int64 FactorySessionCategoryType { get; set; }
        public string FactorySectionCategoryType_NVarChar { get; set; }
        public Int32 MaxRows { get; set; }
        public Int32 IsLegacy { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public string applicantuserid { get; set; }
        public Int64 applicantprofileid { get; set; }
        public Int64 StatusId { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public string OccupierFullName { get; set; }
        public string OccupierFullAddress { get; set; }
        public string ManagerFullName { get; set; }
        public string ManagerFullAddress { get; set; }
        public string LicenceNumber { get; set; }
        public string ClearanceIssuedOn { get; set; }
        public string ClearanceExpiredOn { get; set; }
        public int FactoryCategoryType { get; set; }
        public string? FactoryOrganisationType { get; set; }
    }
    public class DeRegsiterFactoryRequestViewModel
    {
        public string id { get; set; }
        public Int64 appId { get; set; }
        public Int64 appFormId { get; set; }
        public string nar { get; set; }
        public string remarks { get; set; }
        public string deregistrationnumber { get; set; }
        public Int32 IsLegacy { get; set; }
        public string applicantuserid { get; set; }
        public Int32 applicantprofileid { get; set; }
        public string base64 { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
    }
    public class MapBuildingPLanRequestViewModel
    {
        public string id { get; set; }
        public string Token1 { get; set; }
        public string Token2 { get; set; }
        public Int32 IsLegacy { get; set; }
    }




    public class ActAndApplicationPurposeTypeCountsViewModel
    {
        public int ApplicationType { get; set; }
        public int ApplicationPurposeType { get; set; }
        public int Received { get; set; }
        public int InObjection { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Deemed { get; set; }
        public decimal Revenue { get; set; }
    }

    public class CircleAndApplicationPurposeTypeCountsViewModel
    {
        public string? CircleName { get; set; }
        public Int64? CircleRefId { get; set; }
        public int ApplicationPurposeType { get; set; }
        public int ApplicationType { get; set; }
        public int Revenue { get; set; }
        public int Received { get; set; }
        public int InObjection { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Deemed { get; set; }
    }

    public class DesignationAndApplicationPurposeTypeCountsViewModel
    {
        public int ApplicationType { get; set; }
        public string OfficerName { get; set; }
        public int OfficerProfileRefId { get; set; }
        public int ApplicationPurposeType { get; set; }
        public int Received { get; set; }
        public int InObjection { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Deemed { get; set; }
        public decimal Revenue { get; set; }
    }

    public class FileWiseDataViewModel
    {
        public Int64 AppId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string ActionTakenBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string Remarks { get; set; }
        public int ActionTakenDaysCount { get; set; }
        public int ActionTakenHoursCount { get; set; }
        public string CurrentPendingWith { get; set; }
        public string ActionPublicName { get; set; }
        public int AppActionType { get; set; }
        public int projectSiteRefId { get; set; }
        public int ApplicationType { get; set; }
        public int ApplicationPurposeType { get; set; }
        public string NormalizedName { get; set; }
        public string ActionTakenByRoleName { get; set; }
        public string ActionTakenByRoleNormalizeName { get; set; }
        public string DistrictName { get; set; }
        public int CircleRefId { get; set; }
        public int? Legacy_AppFormId { get; set; }
        public string? Legacy_NAR { get; set; }
        public int MaxRows { get; set; }
    }

    public class ProfileAndApplicationPurposeTypeCountsViewModel
    {
        public string OfficerName { get; set; }
        public string Designation { get; set; }
        public string CircleName { get; set; }
        public int Received { get; set; }
        public int InObjection { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Deemed { get; set; }
    }

    public class OfficialsRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleNormalizedName { get; set; }
    }

    public class OfficerMISDashboardDetailsByRoleNameViewModel
    {
        public string OfficerName { get; set; }
        public int OfficerProfileRefId { get; set; }
        public string Designation { get; set; }
        public int Received { get; set; }
        public int InObjection { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Deemed { get; set; }
    }


    public class OfficerProfileAndActWiseDashboardDetailsViewModel
    {
        public string OfficerName { get; set; }
        public int OfficerProfileRefId { get; set; }
        public int ApplicationType { get; set; }
        public int Received { get; set; }
        public int InObjection { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Deemed { get; set; }
    }

    public class OfficerProfileAndCircleWiseDashboardDetailsViewModel
    {
        public string OfficerName { get; set; }
        public int OfficerProfileRefId { get; set; }
        public string CircleName { get; set; }
        public int CircleRefId { get; set; }
        public Int64 ApplicationType { get; set; }
        public int Received { get; set; }
        public int InObjection { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Deemed { get; set; }
    }

    public class CircleAndApplicationPurposeTypeCountsParmsViewModel
    {
        public int ApplicationType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class CurrentlyDesignatedOfficerDetailsViewModel
    {
        public Int64 CircleId { get; set; }
        public string CircleName { get; set; }
        public string RoleName { get; set; }
        public string RoleNormalizedName { get; set; }
        public string OfficerName { get; set; }
    }

    public class MISDashboard_FileWiseData_AppSearchParamsViewModel
    {
        public int ApplicationType { get; set; }
        public int StatusType { get; set; }
        public int CircleRefId { get; set; }
        public int ApplicationPurposeType { get; set; }
        public string SearchCode { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string FilterArray { get; set; }
    }


    public class LabourWelfareSchemeApplicationParmsViewModel
    {
        public int ReportType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class LabourWelfareSchemeApplicationCountsViewModel
    {
        public string? Designation { get; set; }
        public string? OfficerName { get; set; }
        public string? CircleName { get; set; }
        public string? LabourCircleGradeName { get; set; }
        public int Received { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Pendency { get; set; }
        public int InObjection { get; set; }
        public int Pendency30Days { get; set; }
        public int Pendency60Days { get; set; }
        public int Pendency90Days { get; set; }
        public int Pendency365Days { get; set; }
        public int PendencyAbove365Days { get; set; }
    }
    
    public class MisInspectionDashboardDataViewModel
    {
        public Int64 RandomizationId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime Timestemp { get; set; }
        public int TotalInspections { get; set; }
        public int TotalSubmittedFactory { get; set; }
        public int TotalPendingFactory { get; set; }
        public int TotalAssigned { get; set; }
        public int TotalNotAssigned { get; set; }
        public int TotalSubmittedLabour { get; set; }
        public int TotalPendingLabour { get; set; }
        public int MaxRows { get; set; }
    }

    public class InspectionApplicationinfoViewModel
    {
        public Int64 InspectionId { get; set; }
        public Int64 AppId { get; set; }
        public string LicenceNumber { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public Int64 DistrictRefId { get; set; }
        public string FactoryCircleName { get; set; }
        public Int64 LabourCircleRefId { get; set; }
        public Int64 RandomizationRefId { get; set; }
        public string OfficerName { get; set; }
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
        public int LatestAction { get; set; }
        public string ReceiverUserId { get; set; }
        //public int IsTransfered { get; set; }
        //public string TransferedReceiverUserId { get; set; }
    }

    public class LWFMonthWiseContributionViewModel
    {
        public Int64 FinancialYearMonthlyContributionID { get; set; }
        public Int64 PWBCessCollectionId { get; set; }
        public string MonthName { get; set; }
        public Int64 NoOfWorkers { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal EmployerShare { get; set; }
        public decimal EmployeeShare { get; set; }
        public int NoOfEmpsOnLeave { get; set; }

    }

    public class LWFEmployeeContributionViewModel
    {
        public Int64 PWBEmployeeId { get; set; }
        public string EmpName { get; set; }
        public string AadharNo { get; set; }
        public string Mobile { get; set; }
        public string ESICNo { get; set; }
        public string PFNo { get; set; }
        public DateTime? EmpDOJ { get; set; }
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string Gender { get; set; }
        public string IFSCCode { get; set; }
        public string SkillLevel { get; set; }
        public DateTime? EmpDOR { get; set; }
        public DateTime? EmpDOB { get; set; }
        public string FatherOrHusbandName { get; set; }
    }

    public class GetAppActionDocumentsViewModel
    {
        public string DocName { get; set; }
        public string FileName { get; set; }
    }

    public class GetlastTerminationDateViewModel
    {
        public DateTime TerminationDate { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
    }

    public class InspectionComplianceinfoViewModel
    {
        public Int64 InspectionId { get; set; }
        public Int64 AppId { get; set; }
        public string LicenceNumber { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public Int64 DistrictRefId { get; set; }
        public string FactoryCircleName { get; set; }
        public Int64 LabourCircleRefId { get; set; }
        public Int64 RandomizationRefId { get; set; }
        public Int32 InspectionType { get; set; }
        public string OfficerName { get; set; }
        public DateTime? InspectionDoneOn { get; set; }
        public string SubmittedByName { get; set; }
        public string SubmittedByRole { get; set; }
        public string EstbDetail { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public int LatestAction { get; set; }
        public string ReceiverUserId { get; set; }
        public int AllowedDays { get; set; }    
        public int GapDays { get; set; }
        public int ReminderStatus { get; set; }
        //public int IsTransfered { get; set; }
        //public string TransferedReceiverUserId { get; set; }
    }
    public class UserRefIdsAsPerIpinViewModel
    {
        public string UserRefId { get; set; }
    }

    public class EsamikshaCountsViewModel
    {
        public int? TotalInspectionsRandomized { get; set; }
        public int? FactoryWingSubmitted { get; set; }
        public int? LabourWingSubmitted { get; set; }
        public int? FactoryWingPending { get; set; }
        public int? LabourWingPending { get; set; }
        public int? LabourCircleAssigned { get; set; }
        public int? FactoryViolationCount { get; set; }
        public int? LabourViolationCount { get; set; }
        public int? RegisteredFactoriesInTheAskedMonth { get; set; }
        public int? TotalRegisteredFactories { get; set; }
        public int ApplicationsReceived { get; set; }
        public int ApplicationsApproved { get; set; }
        public int PendingBeyondTimeline { get; set; }
        public decimal FeeCollected { get; set; }

    }
    public class GetPendencyCountViewModel
    {
        public int ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int PendingAtDepartment { get; set; }
        public int PendingAtInvestor { get; set; }

    }

    public class EsamikshaViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
     

    }

    public class GetPendingAppViewModel
    {
        public Int64 ServiceCode { get; set; }
        public int PendencyType { get; set; }
    

    }

    public class GetPendingAppCountsViewModel
    {
        public Int64 IPin { get; set; }
        public Int64 ApplicationId { get; set; }
        public string IntegrationDept { get; set; }
        public Int64 ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int NoOfActions { get; set; }
        public Int64 LastLogId { get; set; }
     }
   



    public class SearchApplictionDataViewModel
    {
        public string PublicAppRefNum { get; set; }
        public Int64 AppId { get; set; }
        public string establishmentname { get; set; }
        public string address { get; set; }
        public string ProjectPurpose { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public Int32 IsLegacy { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public string applicantuserid { get; set; }
        public Int64 applicantprofileid { get; set; }
        public Int64 StatusId { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public string LicenceNumber { get; set; }
    }

    public class ServiceListViewModel
    {
        public string ServiceName { get; set; }
        public EstablishmentTypeEnum ApplicationType { get; set; }
        public int ApplicationPurposeType { get; set; }
        public Int64 AppRefId { get; set; }

        public ApplicationLifeCycleStatusTypeEnum ApplicationLifeCycleStatusType { get; set; }

    }


    public class GetApplciationLogsViewModel
    {
        public Int64 AppId { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public string PublicAppRefNum { get; set; }
        public int ApplicationType { get; set; }

        public Int64 ActionTakenDaysCount { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string ApplicationTypeDesc { get; set; }
        public int ApplicationPurposeType { get; set; }
        public string ApplicationPurposeTypeDesc { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public Int64 Legacy_AppFormId { get; set; }
        public string Legacy_NAR { get; set; }
        public string Legacy_LicenceNo { get; set; }
        public int AppActionType { get; set; }
        public string ActionName { get; set; }
        public DateTime ActionDate { get; set; }
        public string SenderUserName { get; set; }
        public string SenderName { get; set; }
        public string SenderRoleName { get; set; }
        public string ReceiverUserName { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverRoleName { get; set; }
        public string Remarks { get; set; }

    }



    public class GetApplciationLogsRequestParms
    {
        public Int64 Appid { get; set; }
        public Int64 InvestPunjabAppid { get; set; }
        public string PublicRefrenceNo { get; set; }
    }

    public class GetApplciationReActivatedData
    {
        public AppActionTypeEnum AppActionType { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
    }

    public class GetDeemedApplciationsData
    {
        public string UserName { get; set; }
        public string OfficerName { get; set; }
        public int TotalDeemed { get; set; }
    }

    public class EmpanelledPersonDetailsByRoleNameViewModel
    {
        public Int64 UserProfileId { get; set; }
        public string OfficerFullName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
        public DateTime RegistrationIssuedOn { get; set; }
        public DateTime RegistrationValidUpto { get; set; }
    }

    public class RandomizationInitializationViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int RandomizationStatus { get; set; }
    }

    public class FundAndUnpaidWagesViewModel
    {
        public Int64 Id { get; set; }
        public Int64? FCId { get; set; }
        public string? FCName { get; set; }
        public Int64? ALLCCircleId { get; set; }
        public string? ALLCCircleName { get; set; }
        public Int64? LabourInspCircleGradeId { get; set; }
        public string? LCGName { get; set; }
        public string EstdName { get; set; }
        public string SiteAddress { get; set; }
        public string MobileNo { get; set; }
        public string FinancialYear { get; set; }
        public string TimeSlot { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string PG_TXN_REF_NO { get; set; }
        public string BANK_REFERENCE_NO { get; set; }
        public string ApplicationFor { get; set; }
        public string LicenceNo { get; set; }
    }
    public class Deemed_ProcessFilesLogViewModel
    {
        public Int64 AppRefId { get; set; }
        public Int64 ApplicationType { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string TotalTime { get; set; }
        public string TotalHolidaysTime { get; set; }
        public string TotalWeekEndsTime { get; set; }
        public string TotalObjectionTime { get; set; }
        public string TotalDormantTime { get; set; }
        public string DeemedInTime { get; set; }
        public string MaxDeemedTime { get; set; }
        public string DeemedTimeType { get; set; }
        public DateTime? FileDeemedDate { get; set; }
        public int IsTimeLineFlow { get; set; }
        public string TotalTimeRemains { get; set; }

    }

    public class GetOtherClearanceDetailsViewModel
    {
        public List<ApprovalDetailsViewModel> ApprovalDetails { get; set; }
        public List<AdminDashboardWelfareContributionDetailsViewModel> WelfareContributionDetails { get; set; }
        public List<AdminDashboardAnnualReturnDetailsViewModel> AnnualReturnDetails { get; set; }
        public List<AdminDashboardInspectionDetailsViewModel> InspectionDetails { get; set; }
    }

    public class ApprovalDetailsViewModel
    {
        public int SrNo { get; set; }
        public string ServiceName { get; set; }
        public string PublicAppRefNum { get; set; }
        public int ApplicationPurposeType { get; set; }

        public string LicenceNumber { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string LicencePath { get; set; }
        public Decimal TransactionAmount { get; set; }
    }

    public class PWBContributionDetailsViewModel
    {
        public Int64 ID { get; set; }
        public Int64? FCId { get; set; }
        public string? FCName { get; set; }
        public Int64? ALLCCircleId { get; set; }
        public string? ALLCCircleName     { get; set; }
        public Int64? LabourInspCircleGradeId { get; set; }
        public string? LCGName { get; set; }
        public string EstdName { get; set; }
        public string SiteAddress { get; set; }
        public string MobileNo { get; set; }
        public string FinancialYear { get; set; }
        public string TimeSlot { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string PG_TXN_REF_NO { get; set; }
        public string BANK_REFERENCE_NO { get; set; }
        public string ApplicationFor { get; set; }
        public string LicenceNo { get; set; }
    }

    public class MonthWiseContributionDetailsViewModel
    {
        public Int64 FinancialYearMonthlyContributionID { get; set; }
        public Int64 PWBCessCollectionId { get; set; }
        public string MonthName { get; set; }
        public Int64 NoOfWorkers { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal EmployerShare { get; set; }
        public decimal EmployeeShare { get; set; }
        public int NoOfEmpsOnLeave { get; set; }
    }

    public class MonthWiseEmployeeDetailsViewModel
    {
        public string EmpName { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string AadharNo { get; set; }
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
    }

    public class FundAndWagesCircleWiseDataViewModel
    {
        public Int64 CircleId { get; set; }
        public string CircleName { get; set; }
        public string ServiceName { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class WelfareFundAndWagesDataViewModel
    {
        public List<FundAndWagesCircleWiseDataViewModel> FundAndWagesCircleWiseDataViewModel { get; set; }
        public List<FundAndUnpaidWagesViewModel> FundAndUnpaidWagesViewModel { get; set; }
    }

    public class UnpaidWagesEmployeeDetailsViewModel
    {
        public string EmpName { get; set; }
        public string FatherName { get; set; }
        public string AadharNo { get; set; }
        public string Mobile { get; set; }
        public decimal Amount { get; set; }
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string Remarks { get; set; }
    }

    public class FactoriesListViewModel
    {
        public string PublicAppRefNum { get; set; }
        public  Int64 AppId { get; set; }
        public string Establishmentname { get; set; }
        public string Address { get; set; }
        public string ProjectPurpose { get; set; }
        public string OwnerName { get; set; }
        public string Email { get; set; }
   
        
    }

    public class ShopBacklogDataViewModel
    {
        public Int64 AppId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string ManagerName { get; set; }
        public Int64 Employees { get; set; }
        public string NICDescriptions { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string ContactPersonEmail { get; set; }
        public string LicenceNumber { get; set; }
        public string ActionPublicName { get; set; }
        public string LicencePath { get; set; }
        public int IsLegacy { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public int ProjectSiteVersion { get; set; }
        public int ApplicationPurposeType { get; set; }
        public DateTime ActionDate { get; set; }
        public Int32 MaxRows { get; set; }



    }


    public class LegacyShopFormDataViewModel
    {
        public Int64 AppId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string? ManagerName { get; set; }
        public Int64 Employees { get; set; }
        public string? NICDescriptions { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string ContactPersonEmail { get; set; }
        public string? EmpName { get; set; }
        public string? EmpFHName { get; set; }
        public string? Gender { get; set; }
        public Int16? Age { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? WeekOffday { get; set; }
        public DateTime? WorkingHoursFrom { get; set; }
        public DateTime? WorkingHoursTo { get; set; }
        public DateTime? IntervalFrom { get; set; }
        public DateTime? IntervalTo { get; set; }
        public string EstdType { get; set; }
        public string OwnerType { get; set; }
        public string OHours { get; set; }
        public string CHours { get; set; }
        public string CloseWeekday { get; set; }



    }

    public class PendencyReportViewModel
    {
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string PublicAppRefNum { get; set; }
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }
        public string Ipin { get; set; }
        public string ApplicationId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string AppFormName { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string PStatus { get; set; }
        public Int64 StatusId { get; set; }
        public DateTime StatusDate { get; set; }
    }

    public class EstablishmentAndUserDetailViewModel
    {
        public Int64 AppId { get; set; }
        public string LicenceNumber { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public Int64 FactoryCircleRefId { get; set; }
        public Int64 AlcCircleRefId { get; set; }
        public Int64 LabourCircleRefId { get; set; }
        public string ProjectPurpose { get; set; }
        public string AlternateEmail { get; set; }
        public string AlternateMobileNo { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public Int64 DistrictRefId { get; set; }
        public Int64 TehsilRefId { get; set; }
        public string ApplicantName { get; set; }
        public int IsAlreadyMerged { get; set; }

    }

    public class MergeLicenceWithUserRequestViewModel
    {
        public Int64 OldAppId { get; set; }
        public string OldUserId { get; set; }
        public string OldUserName { get; set; }
        public string NewUserId { get; set; }
        public string NewuserName { get; set; }
        public string NewInvestpunjabIpin { get; set; }
        public string LicenceNumber { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        
    }

    public class OfficerReportLogsRequestViewModel
    {
        public int OfficerProfileId { get; set; }
        public string UserRefId { get; set; }
        public string RoleId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime Todate { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public string Actions { get; set; }

    }

    public class FileDownloadViewModel
    {
        public string Base64Data { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public bool HasError { get; set; }
        public string ErrorDesc { get; set; }
    }


    public class ApprovedDataViewModel
    {
        public Int64 IPin { get; set; }
        public Int64 AppId { get; set; }
        public string LicenceNo { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string ApprovedBy { get; set; }
        public string ServiceName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string FormData { get; set; }
        public string PaymentDetail { get; set; }
        public string LicencePath { get; set; }
    }

    public class WelfareFundStatusDetailsViewModel
    {
        public Int64 PWBCessCollectionId { get; set; }
        public Int64 AppId { get; set; }
        public string FinancialYear { get; set; }
        public string TimeSlot { get; set; }
        public Decimal Amount { get; set; }
        public DateTime Tdate { get; set; }
        public int AppStatus { get; set; }
        public string Licence_Number { get; set; }
    }

    public class WelfareSchemesDetailsViewModel
    {
            public Int64 Said { get; set; }
            public Int64 ALLCId { get; set; }
            public Int64 LCGID { get; set; }
            public int FCID { get; set; }
            public Int64 schemeId { get; set; }
            public string schemename { get; set; }
            public int Status { get; set; }
            public DateTime UpdateDate { get; set; }
            public DateTime AppSubmitDate { get; set; }
            public decimal Amount { get; set; }
            public string sender_id { get; set; }
            public string Receiver_id { get; set; }
            public Int64 schemeref { get; set; }
            public string schemejson { get; set; }
    }

    public class TransperancyReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


    }

    public class TransperancyReportCountsViewModel
    {
        public int ApplicationType { get; set; }

        public string ActName { get; set; }

        public int BeginningPending { get; set; }

        public int ReceivedInPeriod { get; set; }

        public int Approved { get; set; }

        public int Rejected { get; set; }

        public int Objections { get; set; }

        public int CurrentPending { get; set; }

        public int DeemedAndOthers { get; set; }
    }


    public class RegisteredFactoryCircleWiseViewModel
    {
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public Int64 InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public string ApplicationPurposeType { get; set; }
        public string PublicAppRefNum { get; set; }
        public string LicenceNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LicenceValidity { get; set; }
        public decimal AmountCalculated { get; set; }
        public string UniquePaymentGatewayTransactionId { get; set; }
        public string ManufacturingProcesses { get; set; }
        public Int64 Workers_MaxDuringYear { get; set; }
        public decimal PowerKW_Installed { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonMobileNo { get; set; }
        public string ContactPersonEmail { get; set; }
    }

    public class InspectionDetailsBylicenceViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string LicenceNumber { get; set; }
        public DateTime? InspectionDoneOnFactory { get; set; }
        public string SubmittedByNameFactory { get; set; }
        public string SubmittedByRoleFactory { get; set; }
        public DateTime? InspectionDoneOnLabour { get; set; }
        public string SubmittedByNameLabour { get; set; }
        public string SubmittedByRoleLabour { get; set; }
    }

    public class WelfareFundReceiptDetailsViewModel
    {
        public Int64 PWBCessCollectionId { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string ALLCCircleName { get; set; }
        public string LCGName { get; set; }
        public string EstdName { get; set; }
        public string SiteAddress { get; set; }
        public string CustomerId { get; set; }
        public string FinancialYear { get; set; }
        public string TimeSlot { get; set; }
        public decimal Amount { get; set; }
        public string payStatus { get; set; }
        public DateTime TDate { get; set; }
        public string Licence_Number { get; set; }
    }


    public class UnpaidWagesDetailsBylicenceViewModel
    {
        public Int64 AppId { get; set; }
        public string LicenceNo { get; set; }
        public int LWBSlabType { get; set; }
        public string YEAR { get; set; }
        public Int64 Id { get; set; }
        public int EmployeeCount { get; set; }
        public int VerifiedEmployees { get; set; }
        public int LWBAadhaarVerficationStatusType { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class LWFTransperancyReportViewModel
    {
        public string SchemeName { get; set; }
        public Int64 SchemeId { get; set; }
        public Int64 TotalSubmitted { get; set; }
        public Int64 TotalApproved { get; set; }
        public Int64 TotalRejected { get; set; }
        public Int64 Pending { get; set; }
        public Int64 Objections { get; set; }
    }



}
