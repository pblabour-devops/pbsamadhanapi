using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_BuildingPlanService
    {
        Task<GenericFormModel<Licence_Proposed_BuildingPlan_GeneralDetail>> GetProposedBuildingPlanGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_Proposed_BuildingPlan_GeneralDetail formModel, string id);
        Task<GenericFormModel<ProposedBuildingPlanViewModel>> GetProposedBuildingPlanDetail(long id);

        Task<GenericFormModel<Licence_Existing_BuildingPlan_GeneralDetail>> GetExistingBuildingPlanGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_ExistingBuildingPlanGeneralDetail(Licence_Existing_BuildingPlan_GeneralDetail formModel, string id);
        Task<GenericFormModel<ExistingBuildingPlanViewModel>> GetExistingBuildingPlanDetail(long id);

        Task<GenericFormModel<Licence_Addition_Amendment_BuildingPlan_GeneralDetail>> GetAddition_AmendmentBuildingPlanGeneralDetail(Int64 id, Int64 projectSiteId);
        Task<GenericServiceResultTemplate> AddUpdate_Addition_AmendmentBuildingPlanGeneralDetail(Licence_Addition_Amendment_BuildingPlan_GeneralDetail formModel, string id);
        Task<GenericFormModel<Addition_AmendmentBuildingPlanViewModel>> GetAddition_AmendmentBuildingPlanDetail(long id);

        Task<GenericServiceResultTemplate> LockApplication(Int64 id, int AppActionType,string remarks);

        Task<GenericFormModel<RaiseFeeParmsViewModel>> GetBuildingPlanFactoryRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter);
        Task<GenericResponseTemplateModel<ApplicationActionViewModel>> AddUpdate_BuildingPlanFactoryRaiseFee(RaiseFeeParmsViewModel requestData);
    }
}
