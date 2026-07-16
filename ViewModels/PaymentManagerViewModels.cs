using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class FeeCalculatorInfoParmsViewModel
    {
        public Int64 FeeHeaderId { get; set; }
        public string FeeHeaderTitle { get; set; }
        public decimal AmountCalculated { get; set; }
        public bool IsDeduductible { get; set; }
        public Int64 AppRefId { get; set; }
        public string Description { get; set; }
        public int PaymentBatchCounter { get; set; }
        public Int64 PaymentDetailId { get; set; }
        public bool HasDedicatedTreasuryCode { get; set; }
        public string DedicatedTreasurCode { get; set; }
        public string DedicatedDDOCode { get; set; }
        public bool IsTreasuryPayment { get; set; }
        public string NonTreasuryCode { get; set; }
    }

    public class AppFeePaymentInitiateTerminalInfoViewModel
    {
        public Int64 AppRefId { get; set; }
        public PaymentGatewayTypeEnum PaymentGatewayType { get; set; }
        public PaymentModeTypeEnum PaymentModeType { get; set; }
        public PaymentTreasuryTypeEnum PaymentTreasuryType { get; set; }
        //public string PaymentGatewayTargetUrl { get; set; }
        public PaymentGatewayApiMethodTypeEnum PaymentGatewayApiMethodType { get; set; }
        public string UniquePaymentGatewayTransactionRefId { get; set; }
        public decimal AmountCalculated { get; set; }
        public string PaymentGatewayTargetUrl { get; set; }
        //public List<PaymentGatewayClientFormInputViewModel> PaymentGatewayClientFormInputs { get; set; }
        public PaymentGatewayClientFormInputDetailsViewModel PaymentGatewayClientFormInputDetail { get; set; }
        public string ApplicationTypeName { get; set; }
        public string ApplicationPurposeTypeName { get; set; }
        public int PaymentBatchCounter { get; set; }
        public int PaymentPartCounter { get; set; }
    }

    public class PaymentGatewayClientFormInputDetailsViewModel
    {
        public string PaymentGatewayTargetUrl { get; set; }
        public List<PaymentGatewayClientFormInputViewModel> paymentGatewayClientFormInputList { get; set; }
    }

    public class PaymentGatewayClientFormInputViewModel
    {
        public string FormInputName { get; set; }
        public string FormInputValue { get; set; }
        public bool IsHidden { get; set; }
    }


    public class PaymentGatewaySpecificInputPropsViewModel
    {
        public string PaymentGatewayTargetUrl { get; set; }
        public PaymentGatewayInputProps_SBI FormInputs_SBI { get; set; }
        public PaymentGatewayInputProps_HDFC FormInputs_HDFC { get; set; }
        public PaymentGatewayInputProps_IFMS FormInputs_IFMS { get; set; }
        public PaymentGatewayInputProps_IFMS_NonTreasury FormInputs_IFMS_NonTreasury { get; set; }
    }
    public class PaymentGatewayInputProps_SBI
    {
        public string EncryptTrans { get; set; }
        public string MerchIdVal { get; set; }
    }
    public class PaymentGatewayInputProps_HDFC
    {
        public string Service { get; set; }
        public string deptRefNo { get; set; }
        public string challanDate { get; set; }
        public string expiryDate { get; set; }
        public string hodMstId { get; set; }
        public string ddoCode { get; set; }
        public decimal totalAmt { get; set; }
        public string noOfTrans { get; set; }
        public string HDFCKey { get; set; }
        public string HDFCSalt { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public string sURL { get; set; }
        public string fURL { get; set; }
        public string name { get; set; }
        public string teleNumber { get; set; }
        public string mobNumber { get; set; }
        public string emailId { get; set; }
        public string addLine1 { get; set; }
        public string addLine2 { get; set; }
        public string addPincode { get; set; }
        public string district { get; set; }
        public string tehsil { get; set; }
    }
    public class PaymentGatewayInputProps_IFMS
    {
        public string encData { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string integratingAgency { get; set; }
        public string ipAddress { get; set; }
    }

    public class PaymentGatewayInputProps_IFMS_NonTreasury
    {
        public string encData { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string Service { get; set; }
        public string ipAddress { get; set; }
    }

    public class ResponseData_SBI_ViewModel
    {
        public string encData { get; set; }
        public string Bank_Code { get; set; }
        public string merchIdVal { get; set; }
    }

    public class IFMS_RequestDataViewModel
    {
        public string chcksum { get; set; }
        public IFMS_ChallanDataTemplateViewModel challandata { get; set; }
    }
    public class IFMS_RequestDataNonTreasuryViewModel
    {
        public string chcksum { get; set; }
        public IFMS_NonTreasuryChallanDataTemplateViewModel challandata { get; set; }
    }
    public class IFMS_ChallanDataTemplateViewModel
    {
        public string deptRefNo { get; set; }
        public string receiptNo { get; set; }
        public string clientId { get; set; }
        public string challanDate { get; set; }
        public string expiryDate { get; set; }
        public string companyName { get; set; }
        public string deptCode { get; set; }
        public string totalAmt { get; set; }
        public string trsyAmt { get; set; }
        //public string grandtotalamt { get; set; }
        public string nonTrsyAmt { get; set; }
        public string noOfTrans { get; set; }
        public string ddoCode { get; set; }
        public string payLocCode { get; set; }
        //public string deptRefNo { get; set; }  
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public string add4 { get; set; }
        public string add5 { get; set; }
        public string sURL { get; set; }
        public string fURL { get; set; }
        // public Int64 ID { get; set; }
        public List<IFMS_TresuryPaymentViewModel> trsyPayments { get; set; }
        public List<IFMS_NonTresuryPaymentViewModel> nonTrsyPayments { get; set; }
        public IFMS_PayeeInfo payee_info { get; set; }

    }

    public class IFMS_NonTreasuryChallanDataTemplateViewModel
    {
        //public string AgencyCode { get; set; }
        public string Service { get; set; }
        public string deptRefNo { get; set; }
        public string challanDate { get; set; }
        public string expiryDate { get; set; }
        public string hodMstId { get; set; }
        public string ddoCode { get; set; }
        public string totalAmt { get; set; }
        public string noOfTrans { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public string add4 { get; set; }
        public string add5 { get; set; }
        public string sURL { get; set; }
        public string fURL { get; set; }
        public List<IFMS_NonTrsyPaymentsViewModel> ntPayments { get; set; }
        //public IFMS_NonTreasuryPayeeInfo payee_info { get; set; }

        public IFMS_NonTreasuryPayeeInfo depositor_info { get; set; }
    }

    public class IFMS_TresuryPaymentViewModel
    {
        //public Int64 FeeHeaderId { get; set; }
        public string Head { get; set; }
        public string amt { get; set; }
    }
    public class IFMS_NonTresuryPaymentViewModel
    {
        public string NonTrsy { get; set; }
        public string ntAmt { get; set; }
    }

    //public class IFMS_NonTreasuryPayeeInfo
    //{
    //    public string payerName { get; set; }
    //    public string mobNo { get; set; }
    //    public string email { get; set; }
    //    public string addLine1 { get; set; }
    //    public string addLine2 { get; set; }
    //    public string addPincode { get; set; }
    //    public string lgdDistID { get; set; }
    //    public string lgdTehID { get; set; }
    //}

    public class IFMS_NonTreasuryPayeeInfo
    {
        public string name { get; set; }
        public string mobNo { get; set; }
        public string email { get; set; }
        public string addLine1 { get; set; }
        public string addLine2 { get; set; }
        public string addPincode { get; set; }
        public string identity { get; set; }
        public string identityNumber { get; set; }
        public Int64 lgdDistCode { get; set; }
        public Int64 lgdTehCode { get; set; }
    }
    public class IFMS_PayeeInfo
    {
        public string payerName { get; set; }
        public string teleNumber { get; set; }
        public string mobNumber { get; set; }
        public string emailId { get; set; }
        public string addLine1 { get; set; }
        public string addLine2 { get; set; }
        public string addPincode { get; set; }
        public string district { get; set; }
        public string tehsil { get; set; }
    }
    public class AppFeeSlabRangeParmsViewModel
    {
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
    }
    public class AppFeeSlabAmountInfoViewModel
    {
        public decimal FeeAmount { get; set; }
        public bool IsSelected { get; set; }
    }
    public class EstablishmentRegistrationFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel NumberOfEmployeesToBeEmployedSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }
    public class CalculatedFeeInfoViewModel
    {
        public string Description { get; set; }
        public decimal CalculatedAmount { get; set; }
        public int PaymentBatchCounter { get; set; }
        public List<EstablishmentRegistrationFeeSlabViewModel> establishmentRegistrationFeeSlabs { get; set; }
        public List<ContractLabourRegistrationFeeSlabViewModel> contractLabourRegistrationFeeSlab { get; set; }
        public List<CommonLicenceRegistrationFeeSlabViewModel> commonLicenceRegistrationFeeSlab { get; set; }
        public List<BuildingPlanRegistrationFeeSlabViewModel> buildingPlanRegistrationFeeSlab { get; set; }
        public List<BuildingPlanHUDFeeSlabViewModel> BuildingPlanHUDFeeSlabs { get; set; }
        public List<FactoryLicenceFeeSlabViewModel> factoryLicenceFeeSlab { get; set; }
        public List<ISMPrincipalEmployerRegistrationFeeSlabViewModel> ISMEmployerRegistrationFeeSlab { get; set; }
        public List<MotorTransportRegistrationFeeSlabViewModel> motorTransportRegistrationFeeSlabs { get; set; }
        public List<ContractLabourPrincipalEmployerRegistrationFeeSlabViewModel> ContractLabourPrincipalEmployerRegistrationFeeSlab { get; set; }
        public List<ApplicableRaiseFeeHeadsViewModel> ApplicableRaiseFeeHeads { get; set; }
        public List<BocwRegistrationFeeSlabViewModel> bocwRegistrationFeeSlabs { get; set; }

    }

    public class AppFeeHeadersAndTreasuryHeadsInfoViewModel
    {
        public Int64 FeeHeaderRefId { get; set; }
        public decimal Amount { get; set; }
        public string MajorHead_MajorSubHead_MinorHead_MinorSubHead_00 { get; set; }
        public int PaymentPartCounter { get; set; }
        public int PaymentBatchCounter { get; set; }
        public bool HasDedicatedTreasuryCode { get; set; }
        public string DedicatedTreasurCode { get; set; }
        public string DedicatedDDOCode { get; set; }
    }

    public class AppDDOCodesInfoViewModel
    {
        public string FC_Treasury_DDOCode { get; set; }
        public string FC_Treasury_Abbre { get; set; }
        public string FC_Treasury_Code { get; set; }
        public string LC_Treasury_DDOCode { get; set; }
        public string LC_Treasury_Abbre { get; set; }
        public string LC_Treasury_Code { get; set; }

        public string ALC_Treasury_DDOCode { get; set; }
        public string ALC_Treasury_Abbre { get; set; }
        public string ALC_Treasury_Code { get; set; }

    }
    public class ResponseDataViewModel_IFMS
    {
        public string encData { get; set; }
        public string statusCode { get; set; }
        public string msg { get; set; }
        public string integratingAgency { get; set; }
        public string DeptCode { get; set; }
        public string deptRefNo { get; set; }
    }

    public class NonTreasuryResponseDataViewModel_IFMS
    {
        public string encData { get; set; }
        public string statusCode { get; set; }
        public string msg { get; set; }

    }
    public class PaymentGatewayResponseViewModel
    {
        public bool IsWebRequestCycleCompleted { get; set; }
        public string ResponseBodyData { get; set; }
        public DateTime ResponseReceivedOn { get; set; }
        public string ResponseMessage { get; set; }
        public TransactionFinalStatusTypeEnum TransactionFinalStatusType { get; set; }
        public string UniquePaymentGatewayTransactionId { get; set; }
        public Int64 AppRefId { get; set; }
        public int PaymentBatchCounter { get; set; }
        public int PaymentPartCounter { get; set; }
        public PaymentTreasuryTypeEnum PaymentTreasuryType { get; set; }
    }

    public class PaymentGatewayResponseToUiViewModel
    {
        public Int64 AppRefId { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string PublicAppRefNum { get; set; }
        public ApplicationTypeEnum? ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum? ApplicationPurposeType { get; set; }
        public string BankRefNumber { get; set; }
        public BankTransactionReferenceTypeEnum BankTransactionRefNumberType { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentGatewatResponseText { get; set; }
        public TransactionFinalStatusTypeEnum TransactionFinalStatus { get; set; }
        public PaymentGatewayTypeEnum PaymentGatewayType { get; set; }
    }

    public class PaymentGatewayResponseTemplateViewModel_IFMS
    {
        public string chcksum { get; set; }
        public ClientChalanData_IFMS challandata { get; set; }

    }
    public class ClientChalanData_IFMS
    {
        public string receiptNo { get; set; }
        public string deptRefNo { get; set; }
        public string clientId { get; set; }
        public string challanDate { get; set; }
        public string expiryDate { get; set; }
        public string companyName { get; set; }
        public string deptCode { get; set; }
        public string totalAmt { get; set; }
        public string trsyAmt { get; set; }
        public string nonTrsyAmt { get; set; }
        public string noOfTrans { get; set; }
        public string ddoCode { get; set; }
        public string payLocCode { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public string add4 { get; set; }
        public string add5 { get; set; }
        public BankResponse_IFMS bank_Res { get; set; }
    }
    public class BankResponse_IFMS
    {
        public string BankRefNo { get; set; }
        public string CIN { get; set; }
        public string dateOfPay { get; set; }
        public string status { get; set; }
        public string desc { get; set; }
    }

    public class VerifyChallanInfo_IFMS
    {
        public string chcksum { get; set; }
        public ChallanData_IFMS challandata { get; set; }
    }
    public class ChallanData_IFMS
    {
        public string deptRefNo { get; set; }
        public string clientId { get; set; }
        public string deptCode { get; set; }
        public DateTime challanDate { get; set; }
    }

    #region Contract Labour View Model
    public class ContractLabourRegistrationFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel NumberOfEmployeesToBeEmployedSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }

    public class MotorTransportRegistrationFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel NumberOfEmployeesToBeEmployedSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }

    #endregion Contract Labour View Model

    #region CommonLicence View Model
    public class CommonLicenceRegistrationFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel NumberOfEmployeesToBeEmployedSlab { get; set; }
        public AppFeeSlabRangeParmsViewModel ElectricityKWSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }
    #endregion CommonLicence View Model

    #region Building Plan View Model
    public class BuildingPlanRegistrationFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel AreaSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }
    #endregion Building Plan View Model

    #region Payment Details View Model
    public class DisplayPaymentViewModel
    {
        public string FeeHeaderTitle { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DepartmentTransactionId { get; set; }
        public string BankTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string VillageOrTown { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string PinCode { get; set; }
    }
    #endregion

    #region Building Plan HUD View-Model
    public class BuildingPlanHUDFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel AreaSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }

    public class HUDFeeCalculatorInfoParmsViewModel
    {
        public Int64 FeeHeaderRefId { get; set; }
        public string FeeHeaderTitle { get; set; }
        public decimal AmountRaised { get; set; }
        public decimal AmountAlreadyPaid { get; set; }
        public bool IsDeduductible { get; set; }
        public Int64 PaymentBatchCounter { get; set; }
        public Int64 AppRefId { get; set; }
        public string Description { get; set; }
    }

    public class RaiseFeeParmsViewModel
    {
        public bool IsForVerification { get; set; }
        public List<FeeCalculatorInfoParmsViewModel> FeeCalculatorInfoParms { get; set; }
        public string Remarks { get; set; }
        public string NonTreasuryCode { get; set; }
        public bool IsTimeLineFlow { get; set; }
    }
    #endregion

    public class AppPaymentPartsDetailViewModel
    {
        public List<AppFeeDetail> AppFeeDetails { get; set; }
        public List<AppPaymentPart> AppPaymentParts { get; set; }
    }

    public class IFMS_NonTrsyPaymentsViewModel
    {
        //public string serviceID { get; set; }
        public string subServiceID { get; set; }
        public string Amt { get; set; }
        public string desc { get; set; }
    }
    #region Factory Licence View Model
    public class FactoryLicenceFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel NumberOfEmployeesToBeEmployedSlab { get; set; }
        public AppFeeSlabRangeParmsViewModel ElectricityKWSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }
    #endregion 

    public class AppFeePaymentReceiptViewModel
    {
        public Int64 AppRefId { get; set; }
        public string ApplicationTypeDesc { get; set; }
        public string ApplicationPurposeTypeDesc { get; set; }
        public string EstablishmentName { get; set; }
        public string PublicAppRefNum { get; set; }
        public decimal GrandTotal { get; set; }
        public List<AppFeePaymentReceiptDataViewModel> AppFeePaymentReceiptData { get; set; }
    }

    public class AppFeePaymentReceiptDataViewModel
    {
        public Int64 AppRefId { get; set; }
        public int PaymentBatchCounter { get; set; }
        public int PaymentPartCounter { get; set; }
        public PaymentGatewayTypeEnum PaymentGatewayType { get; set; }
        public string UniquePaymentGatewayTransactionId { get; set; }
        public decimal AmountCalculated { get; set; }
        public DateTime BankSettlementOn { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public decimal Amount { get; set; }
        public string FeeHeaderTitle { get; set; }
        public string EstablishmentName { get; set; }
        public string PublicAppRefNum { get; set; }
    }
    public class ISMPrincipalEmployerRegistrationFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel NumberOfEmployeesToBeEmployedSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }

    #region Principal Employeer View Model
    public class ContractLabourPrincipalEmployerRegistrationFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel NumberOfEmployeesToBeEmployedSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }

    #endregion Principal Employeer View Model

    #region Application Raise Fee
    public class ApplicationRaiseFeeParmsViewModel
    {
        public bool IsForVerification { get; set; }
        public List<ApplicationRaiseFeeCalculatorInfoParmsViewModel> FeeCalculatorInfoParms { get; set; }
        public string Remarks { get; set; }
        public string NonTreasuryCode { get; set; }
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }
        public string RootActivityRefId { get; set; }
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public int ProjectSiteVersion { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public Int64 IPin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public Int64 AppRefId { get; set; }
    }

    public class ApplicationRaiseFeeCalculatorInfoParmsViewModel
    {
        public Int64 FeeHeaderId { get; set; }
        public string FeeHeaderTitle { get; set; }
        public decimal AmountCalculated { get; set; }
        public bool IsDeduductible { get; set; }
        public Int64 AppRefId { get; set; }
        public string Description { get; set; }
        public int PaymentBatchCounter { get; set; }
        public Int64 PaymentDetailId { get; set; }
        public bool HasDedicatedTreasuryCode { get; set; }
        public string DedicatedTreasurCode { get; set; }
        public string DedicatedDDOCode { get; set; }
        public bool IsTreasuryPayment { get; set; }
        public string NonTreasuryCode { get; set; }

    }

    public class ApplicableRaiseFeeHeadsViewModel
    {
        public Int64 FeeHeaderId { get; set; }
        public string FeeHeaderTitle { get; set; }
        public bool IsPrePayable { get; set; }
        public string Description { get; set; }
    }

    public class ApplicationPaymentDetailViewModal
    {
        public List<Payments_RaisedFee> ApplicationPaymentDetailList { get; set; }
        public Int64 AppDocRefId { get; set; }
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }
        public string RootActivityRefId { get; set; }
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public int ProjectSiteVersion { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public Int64 IPin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public Int64 AppRefId { get; set; }
    }
    #endregion

    public class BocwRegistrationFeeSlabViewModel
    {
        public AppFeeSlabRangeParmsViewModel NumberOfEmployeesToBeEmployedSlab { get; set; }
        public AppFeeSlabAmountInfoViewModel AppFeeSlabAmountInfo { get; set; }
    }

    public class EstablishmentWisePaymentDetailsRequestParmsViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ServiceType { get; set; }
    }

    public class EstablishmentWisePaymentDetailsViewModel
    {
        public Int64 AppId { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public int ApplicationType { get; set; }
        public decimal AmountCalculated { get; set; }
        public DateTime TransactionInitializationDate { get; set; }
        public string UniquePaymentGatewayTransactionId { get; set; }
        public string PaymentTreasuryType { get; set; }
    }

    public class ResponseDataViewModel_HDFC
    {
        public string razorpay_order_id { get; set; }
        public string razorpay_payment_id { get; set; }
        public string razorpay_signature { get; set; }
        public string response { get; set; }
        public ErrorDetails error { get; set; }

    }
    public class ErrorDetails
    {
        public string code { get; set; }
        public string description { get; set; }
        public string source { get; set; }
        public string step { get; set; }
        public string reason { get; set; }
        public string metadata { get; set; }
    }

    public class ResponseDataSendViewModel_HDFC
    {
        public string order_id { get; set; }
        public int statusCode { get; set; }


    }
    public class ErrorMetadata
    {
        public string payment_id { get; set; }
        public string order_id { get; set; }
    }
}

