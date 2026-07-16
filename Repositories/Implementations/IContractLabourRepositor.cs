using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IContractLabourRepositor
    {
        Task<List<Contractor_GeneralDetail>> GetAllContractLabours(Int64 ContractLabourId);
    }
}
