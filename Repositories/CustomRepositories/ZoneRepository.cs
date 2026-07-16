using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.CustomRepositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly AppDbContext _context;
        public ZoneRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<GenericListTemplate>> GetTehsilsByDistrictRefId(long DistrictRefId)
        {
            return await _context.Tehsils.Where(x => x.DistrictRefId == DistrictRefId).Select(x => new GenericListTemplate() { ID = x.TehsilLgdId, Text = x.TehsilName }).ToListAsync();
        }
    }
}
