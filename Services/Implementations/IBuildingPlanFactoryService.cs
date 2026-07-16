using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IBuildingPlanFactoryService
    {
        Task<GenericFormModel<BuildingPlanFactory_GeneralDetail>> GetBuildingPlanFactoryGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(BuildingPlanFactory_GeneralDetail formModel, string id);
        Task<GenericFormModel<BuildingPlanFactoryViewModel>> GetBuildingPlanFactoryDetail(long id);
        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType,string remarks);
        Task<GenericFormModel<RaiseFeeParmsViewModel>> GetBuildingPlanFactoryRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter);
        Task<GenericFormModel<RaiseFeeParmsViewModel>> VerifyBuildingPlanFactoryRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, int paymentBatchCounter);
        Task<GenericResponseTemplateModel<ApplicationActionViewModel>> AddUpdate_BuildingPlanFactoryRaiseFee(RaiseFeeParmsViewModel requestData);
        Task<GenericFormModel<List<BuildingPlanHUDPaymentDetail>>> Get_BuildingPlanFactoryRaisedFeeList(Int64 appRefId, Int64 paymentBatchCounter);
        Task<GenericResponseTemplateModel<bool>> Update_BuildingPlanFactoryRaisedFeeDetail(BuildingPlanFactoryPaymentDetailViewModal requestData);
        Task<GenericFormModel<BuildingPlanFactory_Declaration_Stability_Certificate>> GetDeclarationStabilityCertificate(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_DeclarationStabilityCertificate(BuildingPlanFactory_Declaration_Stability_Certificate formModel, string id);
        Task<GenericFormModel<BuildingPlanFactory_Declaration_Stability_CertificateViewModel>> GetDeclarationStabilityCertificateDetail(long id);
    }
}
