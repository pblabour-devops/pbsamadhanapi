using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IBuildingPlanService
    {
        Task<GenericFormModel<BuildingPlan>> GetBuildingPlanDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_BuildingPlanDetail(BuildingPlan formModel, string userName);
        Task<GenericFormModel<List<BuildingPlan_AreaDetail>>> GetBuildingPlanAreaDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdateBuildingPlanAreaDetail(BuildingPlan_AreaDetail formModel);
        Task<GenericFormModel<BuildingPlanViewModels>> GetBuildingPlanDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType,string remarks);
    }
}
