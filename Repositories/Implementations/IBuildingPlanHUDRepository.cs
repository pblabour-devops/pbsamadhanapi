using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IBuildingPlanHUDRepository
    {
        Task<GenericResponseTemplateModel<ApplicationActionViewModel>> AddUpdate_RaiseFee(List<BuildingPlanHUDPaymentDetail> requestData, bool isForVerification, string remarks, bool isTimeLineFlow);
        Task<GenericFormModel<List<BuildingPlanHUDPaymentDetail>>> Get_RaisedFeeList(Int64 appRefId, Int64 paymentBatchCounter);
        Task<GenericResponseTemplateModel<bool>> Update_RaisedFeeDetail(BuildingPlanHUDPaymentDetailViewModal requestData);
    }
}
