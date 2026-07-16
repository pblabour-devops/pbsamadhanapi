using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IApplicationManagementRepository<T> where T : class
    {
        Task<ApplicationInitiateResponseViewModel> InitiateApplication(T entityType, Int64 projectSiteRefId, User user, Int64 iPin, Int64 Legacy_AppFormId, string legacy_NAR, bool legacy_IsMigrated, Int64 investPunjab_AppId, string legacy_LicenceNo, Int64 legacy_AppId, int applicationType, int applicationPurposeType, int projectSiteVersion, bool isFeeApplicable);
        Task<Int64> RemoveApplication(Int64 appId);
        Task<bool> UpdateAppLastModifiedDate(Int64 AppId);
        Task<string> LockApplication(Int64 AppId, int AppActionType, string remarks);
        Task<bool> SwitchApplicationLifeCycleType(Int64 AppId, ApplicationLifeCycleStatusTypeEnum appLifeCycleStatusType);
        Task<GenerateLicenceNoViewModel> GenerateLicenceNo(Int64 appId, ApplicationTypeEnum applicationType);
    }
}
