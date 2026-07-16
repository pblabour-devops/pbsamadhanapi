using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IReportsService
    {
        Task<GenericFormModel<List<FormHViewModel>>> FormHDetails(string id, DateTime fromDate, DateTime toDate, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
    }
}
