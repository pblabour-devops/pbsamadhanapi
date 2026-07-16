using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_Shop_NightShiftService
    {
        //Task<GenericResponseTemplateModel<List<Licence_Shop_NightShift_ChecklistPoint>>> GetCheckListPoints(Int64 id);
        Task<GenericFormModel<Licence_Shop_NightShift_Approval>> GetGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_Shop_NightShift_Approval formModel, string id);
        Task<GenericFormModel<Licence_Shop_NightShiftViewModel>> GetLicenceShopNightShiftDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
        Task<GenericResponseTemplateModel<List<GetlastClearanceViewModel>>> GetLastClearancesNTS(string licenceNo, Int64 projectSiteRefId, int projectSiteVersion);
    }
}
