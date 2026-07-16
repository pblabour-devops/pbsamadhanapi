using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ICommonApisService
    {
        Task<List<GenericListTemplate>> GetTehsilsByDistrictRefId(Int64 DistrictRefId);
        Task<bool> SaveAppFileInfo(Int64 AppId, Int64 DocId, string fileName);
        Task<bool> LockAppDocFiles(Int64 AppId, Int64[] appDocIds);
        Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfo(Int64 AppId, bool deleteTempFiles);
        Task<GenericFormModel<dynamic>> GetApplicationStepper(Int64 entityParentKeyId, Int64 appId, ApplicationTypeEnum ApplicationType);
        Task<GenericFormModel<List<DistrictViewModel>>> GetAllDistrict();
        Task<GenericFormModel<List<NICCode>>> GetAllNICCodes();
        Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfoWithRoleId(Int64 AppId, bool deleteTempFiles, string userId, int currentActionCode, int allowedActionCode, Int64 applicationaType, bool isTimeLineFlow, Int64 applicationActionLogId);
        Task<GenericResponseTemplateModel<FileUploadResponse>> LogFileInfo(Int64 AppId, Int64 DocId, Int64 appDocId, string fileName);
        Task<GenericResponseTemplateModel<EstablishmentCardInfoViewModel>> GetEstablishmentCardDetails(Int64 AppRefId);
        Task<GenericResponseTemplateModel<bool>> LogApplicationAddendumDocument(ApplicationAddendumDocument obj);
        Task<GenericResponseTemplateModel<List<ApplicationAddendumDocument>>> GetApplicationAddendumDocuments(Int64 appRefId);
        Task<GenericResponseTemplateModel<bool>> DetermineAddendumUploadOptionShown(Int64 appRefId);
        Task<GenericResponseTemplateModel<UploadCsvResponseViewModel>> ValidateAndSeedCsvData<T>(string data, string entityKey, Int64 entityKeyValue) where T : class;
        Task<GenericResponseTemplateModel<IntReturn>> FindDuplicateGST(string gstNumber, Int64 projectSiteRefId, int applicationType);
        Task<GenericResponseTemplateModel<IntReturn>> FindDuplicatePanNo(string gstNumber, Int64 projectSiteRefId, int applicationType);
        Task<GenericResponseTemplateModel<DateTime>> GetReportUpdatedDate();
        Task<GenericServiceResultTemplate> CreateActivityLogs(UserActivityLog formModel);
        Task<GenericResponseTemplateModel<InformationViewModel>> GetInformationByLicenceNo(string licenceNo);
        Task<GenericResponseTemplateModel<List<WhatsNewInPortal>>> GetWhatsNewInPortal( string roleName);
        Task<GenericServiceResultTemplate> AddUpdateWhatsNewInPortal(WhatsNewInPortal requestData);
        Task<GenericResponseTemplateModel<ValidateGstResponseViewModel>> ValidateGst(string gstNumber);
    }
}
