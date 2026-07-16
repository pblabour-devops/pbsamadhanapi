using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IPaymentManagerRepository
    {
        public Task<FeesHeader> GetFeeHeader(Int64 feeHeaderId);
        public Task<List<AppFeeHeadersAndTreasuryHeadsInfoViewModel>> GetAppFeeHeadersAndTreasuryHeadsByAppId(Int64 appRefId, int paymentPartCounter, int paymentBatchCounter);
        public Task<AppDDOCodesInfoViewModel> GetAppDDOCodesByAppId(Int64 appRefId, ApplicationTypeEnum applicationType);
    }
}
