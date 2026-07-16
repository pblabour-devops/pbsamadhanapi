using Microsoft.AspNetCore.Http;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IEstablishmentService
    {
        Task<GenericFormModel<Establishment_GeneralDetail>> GetEstablishmentGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Establishment_GeneralDetail formModel, string userName);

        Task<GenericFormModel<Establishment_EmployerDetail>> GetEstablishmentEmployerDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_EmployerDetail(Establishment_EmployerDetail formModel);

        Task<GenericFormModel<List<Establishment_ContractorDetail>>> GetEstablishmentContractorDetail(Int64 id);

        Task<GenericServiceResultTemplate> DeleteContractorDetail(Int64 establishment_ContractorDetailId);
        Task<GenericServiceResultTemplate> AddUpdate_ContractorDetail(Establishment_ContractorDetail formModel);

        Task<GenericFormModel<List<Establishment_Migrantworker>>> GetMigrantWorkerDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_MigrantWorkerDetail(Establishment_Migrantworker formModel);
        Task<GenericServiceResultTemplate> DeleteMigrantWorkerDetail(Int64 establishmentMigrantworkerId);

        Task<GenericFormModel<EstablishmentDetailViewModel>> GetEstablishmentDetail(long id);

        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
        Task<GenericFormModel<IntReturn>> FindDuplicateGST(string gstNumber, Int64 establishmentId);
    }
}
