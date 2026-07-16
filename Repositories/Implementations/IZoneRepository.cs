using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IZoneRepository
    {
        Task<List<GenericListTemplate>> GetTehsilsByDistrictRefId(Int64 DistrictRefId);
    }
}
