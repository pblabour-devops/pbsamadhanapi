using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IBuildingPlanHUDService
    {
        Task<GenericFormModel<BuildingPlanHUD_GeneralDetail>> GetBuildingPlanHUDGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(BuildingPlanHUD_GeneralDetail formModel, string id);
        Task<GenericFormModel<BuildingPlanHUDViewModel>> GetBuildingPlanHUDDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType, string remarks);
        Task<GenericFormModel<RaiseFeeParmsViewModel>> GetBuildingPlanHUDRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter);
        Task<GenericFormModel<RaiseFeeParmsViewModel>> VerifyBuildingPlanHUDRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, int paymentBatchCounter);
        Task<GenericResponseTemplateModel<ApplicationActionViewModel>> AddUpdate_BuildingPlanHUDRaiseFee(RaiseFeeParmsViewModel requestData);
        Task<GenericFormModel<List<BuildingPlanHUDPaymentDetail>>> Get_RaisedFeeList(Int64 appRefId, Int64 paymentBatchCounter);
        Task<GenericResponseTemplateModel<bool>> Update_RaisedFeeDetail(BuildingPlanHUDPaymentDetailViewModal requestData);
        Task<GenericResponseTemplateModel<bool>> UpdateActionLogIdInRaisedFee(UpdateActionLogInRaisedFeeParmsViewModel requestData);
    }
}
