using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_ContractLabourService
    {
        Task<GenericFormModel<Licence_ContractLabour_GeneralDetail>> GetContractLabourGeneralDetail(Int64 id, Int64 projectSiteId);

        Task<GenericFormModel<List<Licence_CL_PE_Contrator>>> Get_PE_Contractor_List(Int64 generalDetailRefId);

        Task<GenericResponseTemplateModel<Licence_CL_PE_Contrator>> Get_PE_Contractor_List_byId(Int64 id);

        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_ContractLabour_GeneralDetail formModel, string userName);

        //Task<GenericServiceResultTemplate> AddUpdate_Licence_ContractLabourContractorDetail(Licence_CL_PE_ContractorDetailViewModel formModel);
        //Task<GenericFormModel<Licence_CL_PE_ContractorDetailViewModel>> Get_ContractLabour_Contractor_General_Detail(Int64 id, Int64 projectSiteId);
        //Task<GenericFormModel<List<Licence_CL_PE_Contrator>>> Get_ContractLabour_Contractor_List(Int64 appRefId);
        //Task<GenericResponseTemplateModel<bool>> RemoveContractor(Int64 id, int totalWorker);
        Task<GenericFormModel<Licence_Contract_LabourViewModel>> Get_Licence_ContractLabour_Detail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
        Task<GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel>> GetPrincipalEmployerDetails(string licencenumber);
        Task<GenericResponseTemplateModel<List<ContractLabourLicenceValidityViewModel>>> GetLicenceValidity(string licencenumber);

    }
}
