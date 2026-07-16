using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IApplicationManagementService<T> where T : class
    {
        Task<ApplicationInitiateResponseViewModel> InitiateApplication(ApplicationTypeEnum applicationType, T entityType, Int64 projectSiteRefId, ApplicationPurposeTypeEnum applicationPurposeType, string userName, Int64 iPin, Int64 investPunjab_AppId, bool legacy_IsMigrated, Int64 legacy_AppId, Int64 Legacy_AppFormId, string legacy_NAR, string legacy_LicenceNo, int projectSiteVersion);
        Task<bool> UpdateAppLastModifiedDate(Int64 AppId);
        Task<List<AppFormStepsInfo>> GetAppFormStepperInfo(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 entityParentKeyId, string stepCode);
        Task<string> LockApplication(Int64 AppId, int AppActionType, string remarks, ApplicationTypeEnum applicationType);
        Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfo(Int64 AppId);
        Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordApplicationAction(ApplicationActionViewModel formModel, string userId);
        Task<GenericResponseTemplateModel<bool>> UpgradeApplicationCircle(LatestCircleInfoViewModel requestData);
        Task<GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>> GetProcessApplicationDetail(string userId, int currentActionCode, ApplicationTypeEnum applicationType);
        Task<GenericResponseTemplateModel<ProcessApplicationUsersDetailViewModel>> GetUserByActionCode(int actionCode, string userId, Int64 appRefId, ApplicationTypeEnum applicationType, string precheckCode);
        Task<GenericResponseTemplateModel<List<NotingLogsViewModel>>> GetApplicationNotingLogsByAppId(Int64 appRefId, string userId);
        Task<GenericServiceResultTemplate> DeleteTempCreatedLicense(ApplicationActionViewModel formModel);
        Task<GenericServiceResultTemplate> RecordAppFeeSuccessTransactionEntry(Int64 appId, Int64 appFeeTransactionRefId, int paymentPartCounter, int paymentBatchCounter, int hasAllPartsPaid);
        Task<UserProfile> GetApplicantProfileDetailByAppId(Int64 appRefId);
        Task<GenericListModel<ApplicationFullDetailsViewModel>> GetEstablishmentApplicationCertificateDetailsByAppRefId(Int64 appRefId);
        Task<GenericListModel<ApplicationFullDetailsViewModel>> GetCommonLicenceApplicationCertificateDetailsByAppRefId(Int64 appRefId);
        Task<GenericListModel<ContractLabourPdfDetailsViewModel>> GetContractLabourApplicationCertificateDetailsByAppRefId(Int64 appRefId);
        Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfoWithRoleId(Int64 AppId, string userId, int currentActionCode, int allowedActionCode, Int64 applicationaType, bool isTimeLineFlow, Int64 applicationActionLogId);
        Task<GenericListModel<BuildingPlanHudPdfDetailsViewModel>> GetBuildingPlanHUDCertificateDetailsByAppRefId(Int64 appRefId);
        Task<GenericFormModel<ProjectSiteViewModel>> GetProjectSiteByAppRefId(Int64 appRefId);
        Task<GenericResponseTemplateModel<ApplicationAction>> GetCurrentStatusByAppRefId(Int64 appRefId, ApplicationTypeEnum applicationType);
        Task<GenericResponseTemplateModel<ApplicationActionCode>> GetCurrentStatusCodeByAppActionType(Int64 actionCode);
        Task<GenericResponseTemplateModel<int>> IncreasePaymentBatchCounter(Int64 appId);
        Task<GenericResponseTemplateModel<string>> GetIndustryUserDashboardUrl(string userId);
        Task<GenericListModel<ShopLicencePdfDetailsViewModel>> GetShopLicenceCertificateDetailsByAppRefId(Int64 appRefId);
        Task<GenericResponseTemplateModel<ApplicationAction>> GetCurrentStatusByNativeAppId(Int64 nativeAppId);
        Task<GenericServiceResultTemplate> TransferApplicationAction(ApplicationTransferParmsViewModal formModel);
        Task<GenericResponseTemplateModel<string>> GetLicenceNoDetailsByProjectSiteRefId(Int64 appId);
        Task<GenericFormModel<DofLicenceDetailsViewModel>> GetDofDetailsByLicenceNo(string licenceNo);
        Task<GenericFormModel<DofLicenceDetailsViewModel>> GetDofDetailsByDOFNo(string dofNumber);
        Task<GenericResponseTemplateModel<ApplicationAdditionalDetailsViewModel>> GetApplicationAdditionalDetails(Int64 appRefId, ApplicationTypeEnum applicationType);
        Task<GenericResponseTemplateModel<ApplicationSpecificDataViewModel>> GetApplicationSpecificData(Int64 appRefId, ApplicationTypeEnum applicationType);
        Task<GenericResponseTemplateModel<List<ApplicationClearencesCondition>>> GetApplicationClearencesConditions(Int64 appRefId);
        Task<GenericResponseTemplateModel<ShopLicenceDetailsForNightShiftPdfViewModel>> getShopLicenceLegacyDetail(string licenceNo);
        Task<GenericResponseTemplateModel<FactoryLicenceDetailsForNightShiftPdfViewModel>> getFactoryLicenceLegacyDetail(string licenceNo);

        Task<GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>> GetTimeLineWiseAllowedAction(Int64 appActionLogRefId, string userRefId, Int64 appRefId);
        Task<GenericFormModel<List<AppActionTime_MutualProcessFlagViewModel>>> GetTimeLineOpenMutualAction(Int64 appRefId, string userRefId);

        Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordOfflineRaiseFeeAction(ApplicationActionViewModel formModel, string userId);

    }
}
