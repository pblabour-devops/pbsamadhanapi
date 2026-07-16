using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IAnnualReturnService
    {
        Task<GenericServiceResultTemplate> InitiateAnnualReturn(InitiateAnnualReturnViewModel formModel);

        Task<GenericServiceResultTemplate> SaveStepDataReturn(SaveStepsReturnViewModel formModel);
        Task<GenericResponseTemplateModel<List<Annual_Return>>> GetAnnualReturnList(string userId, long projectSiteRefId);

        Task<GenericResponseTemplateModel<List<EstablishmentDataViewModel>>> GetEstablishmentDetails(string LicenceNo);

    }

}
