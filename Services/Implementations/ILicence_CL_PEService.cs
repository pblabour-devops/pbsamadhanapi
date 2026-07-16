using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_CL_PEService
    {
        Task<GenericFormModel<Licence_CL_PE_GeneralDetail>> GetPEGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_CL_PE_GeneralDetail formModel, string userName);
        Task<GenericServiceResultTemplate> AddUpdate_Licence_CL_PEContractorDetail(Licence_CL_PE_ContractorDetailViewModel formModel);
        Task<GenericFormModel<Licence_CL_PE_ContractorDetailViewModel>> Get_CL_PE_Contractor_General_Detail(Int64 id, Int64 projectSiteId);
        Task<GenericFormModel<List<Licence_CL_PE_Contrator>>> Get_CL_PE_Contractor_List(Int64 appRefId);
        Task<GenericResponseTemplateModel<bool>> RemoveContractor(Int64 id);
        Task<GenericFormModel<Licence_CL_PEViewModel>> Get_Licence_CL_PE_Detail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
    }
}