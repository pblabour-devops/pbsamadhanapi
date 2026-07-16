using pbsamadhannetcoreapi.Models;
using System.Threading.Tasks;
using System;
using pbsamadhannetcoreapi.ViewModels;
using System.Collections.Generic;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IMiscellaneousService
    {
        Task<GenericResponseTemplateModel<Establishment_EPFO>> GetEstablishmentEpfoDetails(string establishmentId);

        Task<GenericServiceResultTemplate> UpdateEstablishmentDataLogs(Establishment_EPFO_Logs formModel);

        Task<GenericServiceResultTemplate> UpdateEstablishmentReamrks(Establishment_EPFO_Logs formModel);

        Task<GenericResponseTemplateModel<List<Establishment_EPFO_Logs>>> GetEstablishmentEpfoLogDetailbyId(string establishmentRefId);
        Task<GenericResponseTemplateModel<List<Establishment_EPFO_Report>>> GetEstablishmentEPFOReport(string fromdate, string todate, string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);

        
    }
}