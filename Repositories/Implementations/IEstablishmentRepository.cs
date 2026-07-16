using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IEstablishmentRepository
    {
        Task<List<Establishment_Migrantworker>> GetAllEstablishmentMigrants(Int64 EstablishmentRefId);
    }
}
