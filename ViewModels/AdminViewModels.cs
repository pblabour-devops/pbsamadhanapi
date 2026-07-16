using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class StatusManagerRequestParmsViewModel
    {
        public string IPin { get; set; }
        public string ApplicationId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
    }

    public class StatusManagerResponseParmsViewModel
    {
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string ActionPublicName { get; set; }
        public int AppActionType { get; set; }
        public string ActionTakenBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionTakenByRoleName { get; set; }
        //public string ActionTakenByRoleNormalizeName { get; set; }
        public Int64 AppId { get; set; }
        public string PublicAppRefNum { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public string Remarks { get; set; }
        public Int64 ActionTakenDaysCount { get; set; }
        public Int64 ActionTakenHoursCount { get; set; }
        public string CurrentPendingWith { get; set; }
        public string NormalizedName { get; set; }
    }

    public class SearchApplicationParmsViewModel
    {
        public string SearchParams { get; set; }
    }
    public class GetAdminDashboardDetailsViewModel
    {
        public List<AdminDashboardUserDetailsViewModel> UserDetails { get; set; }
        public List<AdminDashboardEstablishmentDetailsViewModel> EstablishmentDetails { get; set; }
        public List<AdminDashboardApplicationDetailsViewModel> ApplicationDetails { get; set; }
        public List<AdminDashboardActionLogViewModel> ApplicationActions { get; set; }
        public List<AdminDashboardActionLogViewModel> ApplicationActionLogs { get; set; }
        public List<AdminDashboardPaymentDetailsViewModel> PaymentDetails { get; set; }
        public List<AdminDashboardApprovalDetailsViewModel> ApprovalDetails { get; set; }
        public List<AdminDashboardWelfareContributionDetailsViewModel> WelfareContributionDetails { get; set; }
        public List<AdminDashboardAnnualReturnDetailsViewModel> AnnualReturnDetails { get; set; }
        public List<AdminDashboardInspectionDetailsViewModel> InspectionDetails { get; set; }
    }

    public class AdminDashboardUserDetailsViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }


    public class AdminDashboardEstablishmentDetailsViewModel
    {
        public string EstablishmentName { get; set; }
        public string Address { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string CircleName { get; set; }
        public string ProjectPurpose { get; set; }
    }


    public class AdminDashboardApplicationDetailsViewModel
    {
        public int AppId { get; set; }
        public string PublicAppRefNum { get; set; }
        public string ServiceName { get; set; }
        public string InvestPunjab_Ipin { get; set; }
        public string InvestPunjab_AppId { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public string ApplicationLifeCycleStatusType { get; set; }
        public string FormStatus { get; set; }
    }

    public class AdminDashboardActionLogViewModel
    {
        public Int64 AppId { get; set; }
        public int AppActionType { get; set; }
        public string ActionName { get; set; }
        public string SenderUserName { get; set; }
        public string SenderName { get; set; }
        public string ReceiverUserName { get; set; }
        public string ReceiverName { get; set; }
        public DateTime ActionDate { get; set; }
        public string Comment { get; set; }
    }


    public class AdminDashboardPaymentDetailsViewModel
    {
        public int AppId { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string UniquePaymentGatewayTransactionId { get; set; }
        public string TransactionStatus { get; set; }
    }


    public class AdminDashboardApprovalDetailsViewModel
    {
        public int SrNo { get; set; }
        public Int64 AppId { get; set; }
        public string ServiceName { get; set; }
        public string LicenceNumber { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string LicencePath { get; set; }
    }

    public class AdminDashboardWelfareContributionDetailsViewModel
    {
        public Int64 AppId { get; set; }
        public string FinancialYear { get; set; }
        public string TimeSlot { get; set; }
        public decimal Amount { get; set; }
        public Int64 PWBCessCollectionId { get; set; }
    }

    public class AdminDashboardAnnualReturnDetailsViewModel
    {
        public Int64 AppId { get; set; }
        public string AccNo { get; set; }
        public string ReturnYear { get; set; }
        public string ReturnType { get; set; }
        public Int64 ReturnID { get; set; }
    }

    public class AdminDashboardInspectionDetailsViewModel
    {
        public Int64 AppId { get; set; }
        public string EstablishmentName { get; set; }
        public string LicenceNumber { get; set; }
        public string FactoryHazardousCategoryType { get; set; }
        public DateTime InspectionDoneOn_Factory_Wing { get; set; }
        public string InspectionSubmittedFactoryWing { get; set; }
        public DateTime InspectionDoneOn_Labour_Wing { get; set; }
        public string InspectionSubmittedLabourWing { get; set; }
    }

    public class AllTransactionsByAppRefIdViewModel
    {
        public Int64 AppfeeTransactionId { get; set; }
        public Int64 AppRefId { get; set; }  
        public decimal AmountCalculated { get; set; }  
        public string FeeHeaderRefId { get; set; }
        public string UniquePaymentGatewayTransactionId { get; set; }
        public int PaymentTreasuryType { get; set; }
        public string BankTransactionRefNumber1 { get; set; }
        public DateTime TransactionInitializationDate { get; set; }
        public int TransactionFinalStatusType { get; set; }
        public Int64 FeeTransaction { get; set; }
        public int ApplicationAction { get; set; }
        public Int64 TimeLineSeed { get; set; }
        public bool IsTimeLineFlow { get; set; }
        public string ActionDesc { get; set; }

    }

    // Models For verify Payments-
    public class ifms_data
    {
        public string chcksum { get; set; }
        public Challandata challandata { get; set; }
    }
    public class Challandata
    {
        public string deptRefNo { get; set; }
        public string clientId { get; set; }
        public string deptCode { get; set; }
        public DateTime challanDate { get; set; }
    }
    public class RequestData
    {
        public string encData { get; set; }
        public string statusCode { get; set; }
        public string msg { get; set; }
    }
    public class checkdata
    {
        public string encData { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string integratingAgency { get; set; }
        public string ipAddress { get; set; }
        public string transactionID { get; set; }

    }
    public class IFMSHeader
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        public string service { get; set; }
        public string ipAddress { get; set; }
        public string transactionID { get; set; }
        public string dateTime { get; set; }
    }

    public class PaymentId
    {
        public string paymentId { get; set; }
    }
    public class IFMS_Auth_Keys
    {
        public HeaderKeys Header { get; set; }
        public JWTKeys JWTKeys { get; set; }
        public DeptCodes DeptCodes { get; set; }

    }
    public class HeaderKeys
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IPAllow { get; set; }
        public string IntegratingAgency { get; set; }

    }
    public class DeptCodes
    {
        public string ClientId { get; set; }
        public string DeptCode { get; set; }
    }
    public class JWTKeys
    {
        public string ChecksumKey { get; set; }
        public string SecretKey { get; set; }
        public string SecretIV { get; set; }
    }

    public class PaymnetVerificationResponseViewModel
    {
        public string encData { get; set; }
        public string statusCode { get; set; }
        public string msg { get; set; }
        public string integratingAgency { get; set; }
        public string deptCode { get; set; }
        public string deptRefNo { get; set; }
    }

    public class ChallandataViewModel
    {
        public string receiptNo { get; set; }
        public string deptRefNo { get; set; }
        public string clientId { get; set; }
        public DateTime challanDate { get; set; }
        public DateTime expiryDate { get; set; }
        public string companyName { get; set; }
        public string deptCode { get; set; }
        public decimal totalAmt { get; set; }
        public decimal trsyAmt { get; set; }
        public dynamic nonTrsyAmt { get; set; }
        public dynamic noOfTrans { get; set; }
        public string ddoCode { get; set; }
        public string payLocCode { get; set; }
        public string add1 { get; set; }
        public string add2 { get; set; }
        public string add3 { get; set; }
        public string add4 { get; set; }
        public string add5 { get; set; }
        public bank_ResViewMode bank_Res { get; set; }
    }
    public class bank_ResViewMode
    {
        public string BankRefNo { get; set; }
        public string CIN { get; set; }
        public DateTime dateOfPay { get; set; }
        public string status { get; set; }
        public string desc { get; set; }
    }

    #region Non_treasury Verify Payment View Models
    public class NonTreasuryVerifyRequest
    {
        public string Checksum { get; set; }
        public NonTreasuryChallanDetails ChallanDetails { get; set; }
    }

    public class NonTreasuryChallanDetails
    {
        public string DeptRefNo { get; set; }
        public string ClientId { get; set; }
        public string Service { get; set; }
        public DateTime ChallanDate { get; set; }
    }

    public class NonTreasuryHeader
    {
        public string EncData { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Service { get; set; }
        public string IPAddress { get; set; }
        public string TransactionID { get; set; }
        public string DeptRefNo { get; set; }
    }

    public class NonTreasuryVerifyResponse
    {
        public string Checksum { get; set; }
        public NonTreasuryChallanDetails ChallanDetails { get; set; }
    }
    #endregion



    public class IFMS_NonTreasury_Auth_Keys
    {
        public NonTreasury_HeaderKeys Header { get; set; }
        public NonTreasury_JWTKeys JWTKeys { get; set; }
        public NonTreasury_DeptCodes DeptCodes { get; set; }

    }
    public class NonTreasury_HeaderKeys
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Service { get; set; }
    }

    public class NonTreasury_JWTKeys
    {
        public string ChecksumKey { get; set; }
        public string SecretKey { get; set; }
        public string SecretIV { get; set; }
    }

    public class NonTreasury_DeptCodes
    {
        public string ClientId { get; set; }
        public string DeptCode { get; set; }
    }

    public class DepartmentOfficialDetailsViewModel
    {
        public string Username { get; set; }
        public string CircleName { get; set; }
        public Int64 UserProfileId { get; set; }
        public string UserProfileName { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string UserRefId { get; set; }
        public string Signature { get; set; }
    }

    public class DepartmentOfficialListViewModel
    {
        public string OfficerName { get; set; }
        public Int64 UserProfileId { get; set; }
        public string MobileNo { get; set; }

        public string Email { get; set; }

        public string Signature { get; set; }
    }


    public class UpdateOfficerTransferViewModel
    {
        public string Username { get; set; }
        public Int64 CurrentProfileId { get; set; }
        public string CurrentofficerName { get; set; }
        public string CurrentofficerMobileNo { get; set; }
        public Int64 tranferofficerProfileId { get; set; }
        public string transferofficerName { get; set; }
        public string transferofficerMobileNo { get; set; }
        public DateTime? trasnferDate { get; set; }
        public string role { get; set; }
        public string remarks { get; set; }
        public string base64 { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }

    public class UpdateEmpanelledPersonProfileDetailsViewModel
    {
        public string ContactNo { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string OfficerFullName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Status { get; set; }
        public Int64 UserProfileId { get; set; }
        public DateTime RegistrationIssuedOn { get; set; }
        public DateTime RegistrationValidUpto { get; set; }
    }

    public class UpdateOfficerProfileDetailsViewModel
    {
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string UserRefId { get; set; }
        public string UserProfileName { get; set; }
        public Int64 UserProfileId { get; set; }
    }
}


