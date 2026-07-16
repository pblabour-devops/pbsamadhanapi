using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IBocwLicenceService
    {
        Task<GenericFormModel<Licence_BocwAct_GeneralDetail>> GetBocwLicenceGeneralDetail(Int64 id, Int64 projectSiteId);

        Task<GenericServiceResultTemplate> AddUpdate_OccupierAndManagerDetail(Licence_BocwAct_GeneralDetail formModel, string userName);

        Task<GenericFormModel<BocwContractorDetailViewModel>> GetBocwContractorCommanDetail(Int64 id, Int64 projectSiteId);

        Task<GenericFormModel<List<Licence_BocwAct_ContractorDetail>>> GetBocwLicenceContractorList(Int64 id);

        Task<GenericServiceResultTemplate> AddUpdateBocwContractorDetail(BocwContractorDetailViewModel formModel);

        Task<GenericFormModel<BocwLicenceViewModel>> GetBocwLicenceDetail(long id);

        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);

        Task<GenericResponseTemplateModel<bool>> RemoveContractor(Int64 id, int totalWorkers);

    }
}