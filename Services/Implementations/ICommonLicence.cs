using Microsoft.AspNetCore.Http;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ICommonLicenceService
    {
        Task<GenericFormModel<CommonLicence_GeneralDetail>> GetCommonLicenceGeneralDetail(Int64 id, Int64 projectSiteRefId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(CommonLicence_GeneralDetail formModel, string userName);
        Task<GenericFormModel<CommonLicence_ContractorDetail>> GetCommonLicenceContractorDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_ContractorDetail(CommonLicence_ContractorDetail formModel);
        Task<GenericFormModel<CommonLicenceViewModels>> GetCommonLicenceDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);

        Task<GenericResponseTemplateModel<CaptchaResultViewModel>> GetCaptchaImage();
    }
}
