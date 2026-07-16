using pbsamadhannetcoreapi.Models;
using System.Threading.Tasks;
using System;
using pbsamadhannetcoreapi.ViewModels;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_MotorTransportService
    {
        Task<GenericFormModel<Licence_MotorTransport>> GetMotorTransportLicenceGeneralDetail(Int64 id, Int64 projectSiteId, Int64 identity);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_MotorTransport formModel, string userName);
        Task<GenericFormModel<Licence_MotorTransportViewModel>> GetMotorTransportDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int appActionType, string remarks);
    }
}
