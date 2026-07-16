using DocumentFormat.OpenXml.VariantTypes;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_Factory_NightShiftService
    {
        Task<GenericFormModel<Licence_Factory_NightShift_Approval>> GetGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_Factory_NightShift_Approval formModel, string id);
        Task<GenericFormModel<Licence_Factory_NightShiftViewModel>> GetLicenceFactoryNightShiftDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
        Task<GenericResponseTemplateModel<List<GetlastClearanceViewModel>>> GetLastClearances(string licenceNo , Int64 projectSiteRefId, int projectSiteVersion);
    }
}