using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IDashboardService
    {
        Task<GenericFormModel<List<DashboardViewModel>>> GetAllProjectSiteInProcessApplications(Int64 ProjectSiteRefId);
        Task<GenericFormModel<ClearenceFileInfoViewModel>> GetAllProjectSiteAlreadyClearancesApplications(Int64 ProjectSiteRefId);
        Task<GenericFormModel<OfficialDashboardContentViewModel>> LoadRecordsTypesMenusCounts(string id, DashboardRecordsHeaderTypeEnum dashboardRecordsHeaderType, ApplicationTypeEnum applicationType, bool applicationListAlso, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericListModel<ApplicationLogsViewModel>> GetApplicationsLogsData(Int64 appRefId, string userId);
        Task<GenericResponseTemplateModel<List<PivotColDetailViewModel>>> Dashboard_PSL_MajorCount_BP_HUD(PSLDashboardMajorCountRequestParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> Dashboard_PSL_Apps_BP_HUD(string colCode, string roleId, int applicationLifeCycleStatusType, int AppActionType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<Deemed_ProcessFilesLog>> GetDeemedCalculationDetail(Int64 id);
        Task<GenericListModel<Deemed_ProcessFilesLogViewModel>> GetDeemedAllActCalculationDetail(Int64 id);
        Task<GenericResponseTemplateModel<List<PaymentDetailsListViewModel>>> GetPaymentDetails(int applicationType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<PivotColDetailViewModel>>> Dashboard_PSL_MajorCount_Factory(PSLDashboardMajorCountRequestParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> Dashboard_PSL_Apps_Factory(string colCode, string roleId, int applicationLifeCycleStatusType, int AppActionType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<PivotColDetailViewModel>>> Dashboard_PSL_MajorCount_Shop(PSLDashboardMajorCountRequestParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> Dashboard_PSL_Apps_Shop(string colCode, string roleId, int applicationLifeCycleStatusType, int AppActionType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<OtherActClearancesDataViewModel>>> GetOtherActClearances(string UserRefId, int applicationType);
        
        Task<GenericResponseTemplateModel<List<PSLDashboardActWiseCountViewModel>>> GetPSLDashboard_ActWiseCount(PSLDashboardMajorCountRequestParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<PSLDashboardCircleWiseCountViewModel>>> GetPSLDashboard_CircleWiseCount(string fromDate, string toDate, int applicationType);
        Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> GetPSLDashboard_OfficerWiseCount(string colCode, Int64 circleRefId, Int64 applicationType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);

        Task<GenericFormModel<List<FactoryBacklogExcelDataViewModel>>> FactoryBacklogData(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericServiceResultTemplate> AddUpdate_FactoryBacklogDetail(FactoryBacklogDataRequestViewModel requestData);
        Task<GenericServiceResultTemplate> DeRegister_FactoryByBacklog(DeRegsiterFactoryRequestViewModel deRegsiterFactoryRequest);
        Task<GenericResponseTemplateModel<SysNApplicationDetailsViewModel>> GetApplicationData(Int64 projectSiteRefId, Int64 pbLabour_AppId, Int64 pbLabour_AppFormId, string pbLabour_NAR, Int32 IsLegacy, string userId);
        Task<GenericResponseTemplateModel<List<GetBuildingPlanDetailsViewModel>>> GetBuildingPlanData(string licenceNumber);
        Task<GenericResponseTemplateModel<List<GetApplicationNotingLogsViewModel>>> GetApplicationLogsByAppId(Int64 appRefId, Int64 appFormId, string nar, Int32 isLegacy);
        Task<GenericResponseTemplateModel<List<ApplicationFeeDetailsViewModel>>> GetFeeDetailsByAppId(Int64 appRefId, Int64 appFormId, string nar, Int32 isLegacy);
        Task<GenericServiceResultTemplate> MapBuildingPlan(MapBuildingPLanRequestViewModel mapBuildingPLanRequestViewModel);
        Task<GenericFormModel<List<FactoryBacklogCountViewModel>>> FactoryBacklogCount(string id);
        Task<GenericFormModel<List<FactoryBacklogDataViewModel>>> FactoryBacklogCountwiseData(string id, int recordType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> GetDeclarationStabilityApplications(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<MPR_Factory_DashboardViewModel>>> Load_Mpr_Factory_Data(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year);
        Task<GenericResponseTemplateModel<List<RecordsTypeListViewModel>>> GetDeclarationStabilityApplicationsExcel(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<ActAndApplicationPurposeTypeCountsViewModel>>> GetActAndApplicationPurposeTypeCounts(PSLDashboardMajorCountRequestParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<CircleAndApplicationPurposeTypeCountsViewModel>>> GetCircleAndApplicationPurposeTypeCounts(CircleAndApplicationPurposeTypeCountsParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<DesignationAndApplicationPurposeTypeCountsViewModel>>> GetDesignationAndApplicationPurposeTypeCounts(CircleAndApplicationPurposeTypeCountsParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<FileWiseDataViewModel>>> GetFileWiseData(int applicationType, int statusType, int circleRefId, int applicationPurposeType, string fromDate, string toDate, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericFormModel<List<OfficialsRoleViewModel>>> GetAllDesignatedOfficialsRoleList();
        Task<GenericResponseTemplateModel<List<OfficerMISDashboardDetailsByRoleNameViewModel>>> GetOfficerMISDashboardDetailsByRoleName(string roleName);
        Task<GenericResponseTemplateModel<List<OfficerProfileAndActWiseDashboardDetailsViewModel>>> GetProfileAndActWiseDashboardDetails(Int64 officerProfileRefId);
        Task<GenericResponseTemplateModel<List<OfficerProfileAndCircleWiseDashboardDetailsViewModel>>> GetProfileAndCircleWiseDashboardDetails(Int64 officerProfileRefId, Int64 applicationType);
        Task<GenericResponseTemplateModel<List<ProfileAndApplicationPurposeTypeCountsViewModel>>> GetProfileAndApplicationPurposeTypeCounts(int officerProfileRefId);
        Task<GenericResponseTemplateModel<List<CurrentlyDesignatedOfficerDetailsViewModel>>> GetCurrentlyDesignatedOfficerDetails(Int64 officerProfileRefId);
        Task<GenericResponseTemplateModel<List<FileWiseDataViewModel>>> GetFileWiseData_AppSearch(int applicationType, int statusType, int circleRefId, int applicationPurposeType, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<LabourWelfareSchemeApplicationCountsViewModel>>> GetLabourWelfareSchemeCounts(LabourWelfareSchemeApplicationParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<MisInspectionDashboardDataViewModel>>> GetMis_Inspection(string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userId, string roleName);
        Task<GenericResponseTemplateModel<List<InspectionApplicationinfoViewModel>>> Get_inspectionsActionWiseData(int inspectionStatus, int randomizationRefId, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userId, string roleName);
        Task<GenericResponseTemplateModel<List<LWFMonthWiseContributionViewModel>>> GetLWFMonthWiseData(Int64 pwbCessCollectionId);
        Task<GenericResponseTemplateModel<List<LWFEmployeeContributionViewModel>>> GetLWFEmployeeData(string monthlyContributionId);
        Task<GenericResponseTemplateModel<bool>> CheckServiceAlreadyApply(Int64 projectSiteId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType);
        Task<GenericResponseTemplateModel<List<GetAppActionDocumentsViewModel>>> GetAppActionDocuments(string nar, Int64 appFormId, Int64 appId, Int32 isLegacy);

        Task<GenericResponseTemplateModel<List<EsamikshaCountsViewModel>>> GetEsamikshaData(EsamikshaViewModel requestData);
        Task<GenericResponseTemplateModel<List<GetPendencyCountViewModel>>> GetPendencyCountServiceWise();
        Task<GenericResponseTemplateModel<List<GetPendingAppCountsViewModel>>> GetPendingAppData(GetPendingAppViewModel requestData);


        Task<GenericResponseTemplateModel<List<AppActionTimeLineDefination>>> GetTimeLinesDetail(Int64 applicationActionLogId);

        Task<GenericFormModel<List<SearchApplictionDataViewModel>>> SearchApplicationResult( string searchCode);

        Task<GenericFormModel<List<ServiceListViewModel>>> GetAllServiceList(Int64 ProjectSiteRefId);

        Task<GenericFormModel<List<GetApplciationLogsViewModel>>> TrackApplicationLogs(Int64 appId, Int64 investPunjabAppid, string punblicrefno);
        Task<GenericFormModel<List<GetApplciationReActivatedData>>> ReActivateApplication(Int64 appId);
        Task<GenericFormModel<List<GetDeemedApplciationsData>>> GetDeemedReport(string deemedDate);
        Task<GenericListModel<EmpanelledPersonDetailsByRoleNameViewModel>> GetEmpanelledPersonDetailsByRoleName(string roleName);
        Task<GenericListModel<bool>> UpdateEmpanelledPersonStatus(EmpanelledPersonDetailsByRoleNameViewModel requestData);

        Task<GenericResponseTemplateModel<WelfareFundAndWagesDataViewModel>> GetPWBContributionDetails(string startDate, string endDate, int type, string userId);
        Task<GenericResponseTemplateModel<List<MonthWiseContributionDetailsViewModel>>> GetMonthlyContributionDetailsById(Int64 pwbCessCollectionId);
        Task<GenericResponseTemplateModel<List<MonthWiseEmployeeDetailsViewModel>>> GetMonthlyEmployeeDetailsById(Int64 monthlyContributionId);

        Task<GenericResponseTemplateModel<List<UnpaidWagesEmployeeDetailsViewModel>>> GetUnpaidWagesEmployeeDetailsById(Int64 unpaidWagesId);
        Task<GenericResponseTemplateModel<List<FactoriesListViewModel>>> GetRegsiteredFactoriesDetails(int factoryType);
        Task<GenericResponseTemplateModel<GetOtherClearanceDetailsViewModel>> GetOtherClearanceDetails(int projectSiteRefId, Int64 applicationType);

        Task<GenericResponseTemplateModel<List<MPR_Labour_DashboardViewModel>>> Load_Mpr_Labour_Data(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year);

        Task<GenericResponseTemplateModel<List<MPR_Alc_DashboardViewModel>>> Load_Mpr_Alc_Data(string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, int month, int year);

        Task<GenericResponseTemplateModel<List<AnnualReturnDashboardViewModel>>> LoadAnnuaReturnData(string userId, Int64 projectSiteRefId);

        Task<GenericResponseTemplateModel<List<ShopBacklogDataViewModel>>> GetShopBacklogData(string Id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<LegacyShopFormDataViewModel>>> GetLegacyShopFormData(Int64 appId, Int64 appFormId, string nar);
        Task<GenericResponseTemplateModel<List<PendencyReportViewModel>>> GetPendencyReport();
        Task<GenericResponseTemplateModel<EstablishmentAndUserDetailViewModel>> GetEstablishmentAndUserDetailsByLicenceNumber(string licenceNumber);
        Task<GenericListModel<bool>> MergeLicenceWithExistingUser(MergeLicenceWithUserRequestViewModel requestData);
        Task<GenericResponseTemplateModel<string>> SendOTPToUser(string userId, string userName, string licenceNumber);

        Task<GenericResponseTemplateModel<List<ApprovedDataViewModel>>> GetApprovedData(string id, int ServiceCode);
      
        Task<GenericResponseTemplateModel<List<OfflineReportLogs>>> GetOfflineReportLogs(string userRefId);
        Task<GenericListModel<bool>> AddOfflineReport(OfficerReportLogsRequestViewModel requestData);
        Task<FileDownloadViewModel> DownloadOfflineReport(string reportId);
        Task<GenericResponseTemplateModel<List<TransperancyReportCountsViewModel>>> GetTransperancyData(TransperancyReportViewModel requestData);

        Task<FileDownloadViewModel> DownloadRegisteredFactoryCircleWise(string userRefId);
        Task<GenericFormModel<WelfareFundReceiptDetailsViewModel>> GetLWBPaymentReceipt(int pwbCessCollectionId);
        Task<GenericResponseTemplateModel<List<LWFTransperancyReportViewModel>>> GetLWFTransperancyData(TransperancyReportViewModel requestData);

    }
}