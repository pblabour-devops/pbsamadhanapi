using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IInvestPunjabShareStatusService
    {
        Task<GenericResponseTemplateModel<bool>> ShareStatusToBusinessFirst(InvestPunjabShareStatusParmsViewModel investPunjabShareStatusParms);
    }
}
