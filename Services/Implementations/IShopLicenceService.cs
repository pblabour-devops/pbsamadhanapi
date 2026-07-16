using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IShopLicenceService
    {
        Task<GenericFormModel<ShopLicence_GeneralDetail>> GetShopLicenceGeneralDetail(Int64 id, Int64 projectSiteId);
        //Task<GenericServiceResultTemplate> AddUpdate_ShopLicenceDetail(ShopLicence_GeneralDetail formModel, string userName);
        Task<GenericFormModel<ShopLicenceViewModel>> GetShopLicenceDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
        Task<GenericFormModel<IntReturn>> FindDuplicateGST(string gstNumber, Int64 shopLicenceId);
        Task<GenericFormModel<IntReturn>> FindDuplicatePanNo(string panOrTanNumber, Int64 shopLicenceId);
        Task<GenericResponseTemplateModel<AdhaarVerifyViewModel>> FindDuplicateAadharNumber(string aadharNumber, Int64 projectSiteRefId);
        Task<GenericFormModel<EmployeeDetailViewModel>> GetShopLicenceEmlpyeeDetail(Int64 id, Int64 projectSiteId);
        //Task<GenericServiceResultTemplate> AddUpdate_ShopLicenceEmployeeDetail(EmployeeDetailViewModel formModel);
        Task<GenericFormModel<List<ShopEmployeeDetailViewModel>>> GetShopLicenceEmlpyeesList(Int64 id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<bool>> RemoveEmployee(Int64 id);
    }
}
