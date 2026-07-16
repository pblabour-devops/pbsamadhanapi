using pbsamadhannetcoreapi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using pbsamadhannetcoreapi.ViewModels;



namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_PE_ISMService
    {
        Task<GenericFormModel<Licence_PE_ISM_GeneralDetail>> GetISMGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericFormModel<Licence_PE_ISM_ContractorDetailViewModel>> Get_ISM_Contractor_General_Detail(Int64 id, Int64 projectSiteId);
        Task<GenericFormModel<List<Licence_PE_ISM_Contrator>>> Get_ISM_Contractor_List(Int64 appRefId);
        Task<GenericResponseTemplateModel<bool>> RemoveContractor(Int64 id);
        Task<GenericFormModel<Licence_PE_ISMViewModel>> Get_Licence_ISM_Detail(long id);

    }
}
