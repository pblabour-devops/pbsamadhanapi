using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicene_BuildingPlan_PSIECService
    {
        Task<GenericFormModel<Licence_BuildingPlan_PSIEC_GeneralDetail>> GetBuildingPlan_PSIEC_GeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericFormModel<Licence_BuildingPlan_PSIECViewModel>> GetBuildingPlan_PSIEC_Detail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
        Task<GenericResponseTemplateModel<PSIECUserDetailsViewModel>> GetSdoAndEoDetails(string sdo, string eo);
    }
}