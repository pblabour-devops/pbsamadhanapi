using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.CustomRepositories
{
    public class ProjectSiteRepository : IProjectSiteRepository
    {
        private readonly AppDbContext _context;
        public ProjectSiteRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProjectSite>> Get_CurrentUserAllProjectSites(string userId)
        {
           //_context.ChangeTracker.LazyLoadingEnabled = false;
            return await _context.ProjectSites.Where(x => x.UserRefId == userId).ToListAsync();
        }
    }
}
