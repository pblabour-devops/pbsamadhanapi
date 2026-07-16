using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IPaymentManagerService
    {
        public Task<GenericFormModel<List<FeeCalculatorInfoParmsViewModel>>> ApplicationFeeCalculator(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 entityKeyId);
        public Task<FeesHeader> GetFeeHeader(Int64 feeHeaderId);

        public Task<CalculatedFeeInfoViewModel> CalculateFeeAmountHeaderwise(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 feeHeaderId, ApplicationPurposeTypeEnum applicationPurposeType);

        public Task<GenericFormModel<AppFeePaymentInitiateTerminalInfoViewModel>> PrepareAppFeePaymentInitiateTerminalInfo(Int64 appRefId, ApplicationTypeEnum applicationType, decimal netFeeCalculated, int paymentPartCounter, int paymentBatchCounter);
        public Task<PaymentGatewayClientFormInputDetailsViewModel> PreparePaymentGatewayClientFormInputs(ApplicationTypeEnum applicationType, Int64 appRefId, AppFeePaymentInitiateTerminalInfoViewModel AppFeePaymentInitiateTerminalInfo, int paymentPartCounter, int paymentBatchCounter);

        public Task<decimal> HandlePaymentGatewayResponse_SBI(ResponseData_SBI_ViewModel requestData);
        public Task<GenericServiceResultTemplate> LogAppFeeTransaction(AppFeePaymentInitiateTerminalInfoViewModel formModel);
        public Task<GenericFormModel<List<dynamic>>> GetAllAppFeeTransactions(Int64 appRefId);
        public Task<GenericFormModel<bool>> VerifyAlreadyMadePayments(Int64[] appFeeTransactionIds);
        public Task<GenericResponseTemplateModel<int>> LogApplicationFeeHeaders(List<FeeCalculatorInfoParmsViewModel> requestData);
        //public Task<PaymentGatewayResponseToUiViewModel> HandlePaymentGatewayResponse(dynamic responseObject, PaymentGatewayTypeEnum paymentGatewayType);
        public Task<GenericFormModel<PaymentGatewayResponseToUiViewModel>> HandlePaymentGatewayResponse(dynamic responseObject, PaymentGatewayTypeEnum paymentGatewayType);
        Task<GenericResponseTemplateModel<PaymentDetailViewModel>> GetApplicationPaymentDetails(Int64 appRefId);
        public Task<GenericFormModel<List<FeeCalculatorInfoParmsViewModel>>> BuildingPlanHUDFeeRaised(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 entityKeyId);
        public Task<GenericResponseTemplateModel<AppPaymentPartsDetailViewModel>> GetPaymentPart(Int64 appRefId, int paymentBatchCounter);
        public Task<GenericResponseTemplateModel<List<AppPaymentEDCAuthority>>> GetPaymentEDCAuthorities();
        Task<GenericResponseTemplateModel<AppFeePaymentReceiptViewModel>> GenerateFeeReceipt(Int64 investPunjabIPin, Int64 investPunjabAppId, Int64 appRefId);
        Task<GenericFormModel<ApplicationRaiseFeeParmsViewModel>> GetApplicationRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter);
        Task<GenericFormModel<List<Payments_RaisedFee>>> GetRaisedFeeList(Int64 appRefId, Int64 paymentBatchCounter);
        Task<GenericResponseTemplateModel<List<AppFeeDetail>>> GetAppFeeDetails(Int64 appRefId);
        Task<GenericResponseTemplateModel<List<EstablishmentWisePaymentDetailsViewModel>>> GetEstablishmentWisePaymentDetails(EstablishmentWisePaymentDetailsRequestParmsViewModel requestData);
    }
}
