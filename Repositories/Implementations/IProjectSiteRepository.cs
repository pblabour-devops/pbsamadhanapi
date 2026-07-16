using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IProjectSiteRepository
    {
        Task<List<ProjectSite>> Get_CurrentUserAllProjectSites(string userId);
    }
}
