using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_ISM_ContractLabourService
    {
        Task<GenericFormModel<Licence_ISM_ContractLabour_GeneralDetail>> GetISMContractLabourGeneralDetail(Int64 id, Int64 projectSiteId);

        Task<GenericFormModel<List<Licence_PE_ISM_Contrator>>> Get_ISM_PE_Contractor_List(Int64 generalDetailRefId);

        Task<GenericResponseTemplateModel<Licence_PE_ISM_Contrator>> Get_ISM_PE_Contractor_List_byId(Int64 id);

        Task<GenericFormModel<Licence_ISM_Contract_LabourViewModel>> Get_Licence_ISM_ContractLabour_Detail(long id);
        Task<GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel>> GetISMPrincipalEmployerDetails(string licencenumber);
    }
}
