using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IFactoryLicenceService
    {
        Task<GenericFormModel<Licence_Factory_GeneralDetail>> GetFactoryLicenceGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_Factory_GeneralDetail formModel, string id);
        Task<GenericFormModel<Licence_Factory_OccupierAndManagerDetail>> GetOccupierAndManagerDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_OccupierAndManagerDetail(Licence_Factory_OccupierAndManagerDetail formModel);
        Task<GenericFormModel<FactoryLicenceViewModel>> GetFactoryLicenceDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
        Task<GenericServiceResultTemplate> AddUpdate_Questionnairedetails(Licence_Factory_QuestionnaireDetail formModel, string id);
        Task<GenericFormModel<AnnualReturnWelfareFundViewModel>> VerifyWelfareFundAndReturn(string licenceNumber, Int64 appRefId);
        Task<GeneratePdfServiceResultTemplate> GenerateApplicationFormPdf(Int64 appRefId);
    }
}
