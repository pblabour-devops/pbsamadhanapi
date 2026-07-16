using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.CustomRepositories
{
    public class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly AppDbContext _context;
        public EstablishmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Establishment_Migrantworker>> GetAllEstablishmentMigrants(long EstablishmentRefId)
        {
            throw new NotImplementedException();
        }
        //public async Task<List<Establishment_Migrantworker>> GetAllEstablishmentMigrants(long establishmentRefId)
        //{
        //    return await _context.Establishments_Migrantworkers.Where(x => x.EstablishmentRefId == establishmentRefId).ToListAsync();
        //}
    }
}
