using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IContractLabourService
    {
        Task<GenericFormModel<Contractor_GeneralDetail>> GetContractLabourGeneralDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Contractor_GeneralDetail formModel, string userName);
        Task<GenericFormModel<Contractor_PrincipalEmployer>> GetPricipalEmployerDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_PricipalEmployerDetail(Contractor_PrincipalEmployer formModel);
        Task<GenericFormModel<ContractLabourViewModels>> GetContractLabourDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
    }
}
