using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.CustomRepositories
{
    public class PaymentManagerRepository : IPaymentManagerRepository
    {
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        public PaymentManagerRepository(AppDbContext context, IGeneric_SP_Repository iGeneric_SP_Repository)
        {
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
        }

        public async Task<FeesHeader> GetFeeHeader(Int64 feeHeaderId)
        {
            return await _context.FeesHeaders.FindAsync(feeHeaderId);
        }

        public async Task<List<AppFeeHeadersAndTreasuryHeadsInfoViewModel>> GetAppFeeHeadersAndTreasuryHeadsByAppId(long appRefId, int paymentPartCounter, int paymentBatchCounter)
        {
            List<AppFeeHeadersAndTreasuryHeadsInfoViewModel> appFeeHeadersAndTreasuryHeads = null;
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appRefid", ParmValue=appRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="paymentPartCounter", ParmValue=paymentPartCounter.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="paymentBatchCounter", ParmValue=paymentBatchCounter.ToString(), isNumber=true},
                };

                appFeeHeadersAndTreasuryHeads = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFeeHeadersAndTreasuryHeadsInfoViewModel>("dbo.sp_Payments_Get_App_FeeHeaders_And_TreasuryHeads_By_AppId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                return appFeeHeadersAndTreasuryHeads;
            }
            return appFeeHeadersAndTreasuryHeads;
        }

        public async Task<AppDDOCodesInfoViewModel> GetAppDDOCodesByAppId(long appRefId, ApplicationTypeEnum applicationType)
        {
            List<AppDDOCodesInfoViewModel> AppDDOCodesInfo = null;
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appRefid", ParmValue=appRefId.ToString(), isNumber=true},
                };

                if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                {
                    AppDDOCodesInfo = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppDDOCodesInfoViewModel>("dbo.sp_Payments_Get_Factory_DDO_Codes_By_AppId", storeProcedureParms);
                }
                else if (applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT)
                {
                    AppDDOCodesInfo = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppDDOCodesInfoViewModel>("dbo.sp_Payments_Get_ALC_DDO_Codes_By_AppId", storeProcedureParms);
                }
                else
                {
                    AppDDOCodesInfo = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppDDOCodesInfoViewModel>("dbo.sp_Payments_Get_App_DDO_Codes_By_AppId", storeProcedureParms);
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
            return AppDDOCodesInfo.FirstOrDefault();
        }
    }
}
