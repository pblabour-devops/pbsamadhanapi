using Microsoft.AspNetCore.Http;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IThirdPartyInegrationsService
    {
        Task<ServiceGatewayResponseViewModel> ServiceGateway(string msg, HttpRequest httpRequest);
        Task<GenericResponseTemplateModel<BusinessFirstRequestLogResponseViewModel>> LogBusinessFirstRequest(BusinessFirst_RequestLog requestLog);
        Task<GenericResponseTemplateModel<BusinessFirstSCAFViewModel>> GetBusinessFirstCafDataByIPin(Int64 iPin);
        Task<GenericResponseTemplateModel<bool>> UpdateBusinessFirstRequestNativeUserNativeByIPin(Int64 iPin, string nativeUserId);
        Task<GenericResponseTemplateModel<bool>> UpdateBusinessFirstRequestNativeAppIdByIPin(Int64 iPin, Int64 appId, Int64 nativeAppId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType);
        Task<GenericResponseTemplateModel<string>> ShareStatusToBusinessFirst(Int64 AppRefId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType);
        Task<GenericResponseTemplateModel<Application>> GetIPinByAppRefId(Int64 appRefId);
        Task<GenericResponseTemplateModel<Int32>> GetShareStatusLogCountByAppRefId(Int64 appRefId);
        Task<GenericResponseTemplateModel<ImportAndSeedDataResponseViewModel>> ImportAndSeedData(Int64 sys_O_AppId, string iPin, Int64 investPunjab_AppId, int serviceCode, string sys_o_token, string licenceNo, bool logsAlsoToBeSeeded);
        Task<GenericResponseTemplateModel<string>> VerifyOldLicenceNo(string licenceSnapshot);
        Task<GenericResponseTemplateModel<PrincipalApproval_RBA_DetailsViewModel>> GetPrincipalApprovalUnderRBAByIPin(string iPin, string appId);
        Task<GenericResponseTemplateModel<BuildingPlanHUD_RTB_Mapping>> GetInPrincipalApprovalByAppRefId(Int64 appRefId);
        Task<GenericResponseTemplateModel<bool>> HasShareStatusToBusinessFirst(Int64 nativeAppId);
        Task<GenericResponseTemplateModel<bool>> VerifyLicenseNumber(string value);
        Task<GenericResponseTemplateModel<int>> GetRaisedFeePaymentPageCode(Int64 appRefId);
        Task<GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel>> GetTemporaryLicenseDetails(string tempRegistrationNumber);
        Task<GenericResponseTemplateModel<string>> GetDOFLicenseDetails(string dofNumber);
        Task<GenericServiceResultTemplate> Seed_BusinessFirstApprovedFiles(BusinessFirst_ApprovedFileSeeding requestData);
        Task<GenericResponseTemplateModel<List<GetLicenceNumberAndActTypeViewModel>>> GetActTypeByLicenceNumber(string licenceNumber);
        Task<GenericFormModel<GetRedirectUrlViewModel>> GetRedirectToOtherPortel(string UserId, string role, string type);
        Task<GenericResponseTemplateModel<StabiltyAcknoweldgementReceiptViewModel>> GetStabiltyAcknoweldgementSlip(string msg);
        Task<GenericResponseTemplateModel<List<IPinInfoViewModel>>> GetLatestDirtyApplicationsDetails();
        Task<GenericResponseTemplateModel<List<SetIsDirtyFlagForcefullyViewModel>>> SetIsDirtyFlagForcefully(List<DirtyFlagRequestParamsViewModel> requestData);
        Task<GenericResponseTemplateModel<List<IPinInfoViewModel>>> SeedApplicationDataByLegacyAppFormId(int legacyAppFormId);
        Task<GenericResponseTemplateModel<List<AppSubmissionResponseViewModel>>> GetAppSubmissionDateByIpinAppId(List<DirtyFlagRequestParamsViewModel> requestData);
        Task<GenericResponseTemplateModel<List<IPinInfoViewModel>>> GetBFApplicationsLogDetails(List<DirtyFlagRequestParamsViewModel> requestData);
        Task<GenericResponseTemplateModel<dynamic>> GetHRMSCodeAndActWiseData(HRMSRequestParamsViewModel requestData);
        Task<GenericResponseTemplateModel<dynamic>> GetHRMSCodeAndActWiseData_FactoryWing(HRMSRequestParamsViewModel requestData);
        Task<GenericResponseTemplateModel<dynamic>> GetHRMSCodeAndActWiseData_ALCWing(HRMSRequestParamsViewModel requestData);
        Task<GenericResponseTemplateModel<dynamic>> GetHRMSCodeAndActWiseData_LabourWing(HRMSRequestParamsViewModel requestData);
        Task<GenericResponseTemplateModel<WithdrawApplicationViewModel>> WithdrawApplication(string requestData);
        Task<GenericResponseTemplateModel<List<PendingApplicationDetailsViewModel>>> GetTotalPendingApplicationsByDate();
        Task<GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>>> GetTotalPendingApplicationsByServiceCode(PendingApplicationsByServiceCodeParamsViewModel requestData);
        Task<GenericResponseTemplateModel<IpinAndApplicationIdInfoViewModel>> GetApplicationCurrentStatusByIpinAndAppId(ApplicationCurrentStatusParamsViewModel requestData);
        Task<GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>>> GetUpdatedApplicationsBetweenDates(UpdatedApplicationsParamsViewModel requestData);
        Task<GenericResponseTemplateModel<List<GetReportByDepartmentViewModel>>> GetReportByDepartment();

        Task<GenericResponseTemplateModel<List<GetReportByAuthorityCountsViewModel>>> GetReportByAuthority(GetReportByAuthorityViewModel requestData);

        Task<GenericResponseTemplateModel<List<GetReportByServiceCountsViewModel>>> GetReportByService(GetReportByServiceViewModel requestData);

        Task<GenericResponseTemplateModel<List<IPinInfoViewModel>>> SeedApplicationDataByLegacyAppIdId(int legacyAppFormId, Int64 appRefId);

        Task<GenericResponseTemplateModel<DownloadApprovalViewModel>> DownloadApproval(string msg);

        Task<string> ValidateLoginFromPartnerPortal(string msg, HttpRequest httpRequest);
        Task<GenericResponseTemplateModel<List<BOCWTransparencyViewModel>>> GetBOCWTransparencyData(TransperancyReportViewModel requestData);
    }
}
