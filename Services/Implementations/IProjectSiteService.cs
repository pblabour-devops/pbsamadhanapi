using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IProjectSiteService
    {
        Task<GenericFormModel<ProjectSite>> GetProjectSiteDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_ProjectSiteDetail(ProjectSite requestData, string userId);
        Task<GenericFormModel<List<ProjectProfileViewModel>>> Get_CurrentUserAllProjectSites(string UserId);
        Task<GenericResponseTemplateModel<ProjectSitesViewModel>> Get_ProjectSiteByProjectSiteId(Int64 projectSiteId, Int64 appRefId, int projectSiteVersion);
        Task<GenericResponseTemplateModel<ProjectSiteViewModel>> CreateProjectSite(ProjectSite requestData, bool isIpinMapping, string iPin);
        Task<GenericResponseTemplateModel<Int64>> GetProjectSiteRefIdByAppId(Int64 appId);
        Task<GenericResponseTemplateModel<Int64>> GetProjectSiteRefIdByIpin(string iPin);
        Task<GenericServiceResultTemplate> SetProjectSiteCircle(Int64 projectSiteRefId, Int64 factoryCircleId, Int64 labourCircleId, Int64 alcCircleId);
        Task<GenericServiceResultTemplate> Update_ProjectSiteLogDetails(ProjectSiteEstablishmentBasicDetailsViewModel requestData);
    }
}
