using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Office2013.Word;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace pbsamadhannetcoreapi.ViewModels
{
    public class ServiceGatewayResponseViewModel
    {
        public bool CanApply { get; set; }
        public string RequestDeniedReason { get; set; }
        public bool IsOtpVerificationRequired { get; set; }
        public string ContactSnapshot { get; set; }
        public string SymmetricKey { get; set; }
        public bool HasLessParameters { get; set; }
        public bool HasException { get; set; }
        public string ExceptionMessage { get; set; }
        public Int64 IPin { get; set; }
        public Int64 NativeAppId { get; set; }
        public string NativeUserId { get; set; }
        public int ServiceCode { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public int CategoryTypeId { get; set; }
        public bool IsRequestToLegacyApp { get; set; }
        public string LegacyAppUrl { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public bool IsEntityKeysToKeepSame { get; set; }
        public string LicenceNo { get; set; }
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }
        public string RootActivityRefId { get; set; }
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        public bool HasOpenedTickets { get; set; }
        public List<ToDoTicketDetailViewModel> ToDoTickets { get; set; }
        public int ProjectSiteVersion { get; set; }
        public string EncryptionKey { get; set; }
        public string IVKey { get; set; }
    }

    public class ServiceGatewayRequestJsonDataViewModel
    {
        public Int64 iPin { get; set; }
        public string AppId { get; set; }
        public int ActID { get; set; }
        public string ActName { get; set; }
        public int ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int CategoryTypeId { get; set; }
        public string CategoryType { get; set; }
        public string FileNo { get; set; }
        public bool isTrade { get; set; }
    }

    public class BusinessFirstRequestLogResponseViewModel
    {
        public Int64 BusinessFirst_RequestLogId { get; set; }
        public Int64 NativeAppId { get; set; }
        public string NativeUserId { get; set; }
    }

    public class BusinessFirstSCAFViewModel
    {
        public virtual BusinessFirstSCAF_AppUserViewModel AppUser { get; set; }
        public virtual BusinessFirstSCAF_CafInfoViewModel CafInfo { get; set; }
    }
    public class BusinessFirstSCAF_AppUserViewModel
    {
        public string AppUserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int DesigID { get; set; }
        public int DeptID { get; set; }
        public string Email { get; set; }
        //public int LGDDistrictId { get; set; }
        //public int LGDTehsilId { get; set; }
        public string PhoneNumber { get; set; }
        public int LimitOfApplications { get; set; }
    }

    public class BusinessFirstSCAF_CafInfoViewModel
    {
        public Int64 AppId { get; set; }
        public string TokenNo { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string EstbName { get; set; }
        public string BusiType { get; set; }
        public string AadharNum { get; set; }
        public string ID_Type { get; set; }
        public string AppAddress { get; set; }
        public string SiteAddress { get; set; }
        public string SiteVlgName { get; set; }
        public int SiteLGDDistId { get; set; }
        public int SiteLGDTehId { get; set; }
        public string SitePin { get; set; }
        public string ComAddress { get; set; }
        public string ComVlgName { get; set; }
        public string ComPin { get; set; }
        public string ComLGDTehId { get; set; }
        public string ComLGDDistId { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string AltEmail { get; set; }
        public string IsInPb { get; set; }
        public string SC_Sign { get; set; }
        public string SC_Photo { get; set; }
        public string SC_Aadhar { get; set; }
        public string AppUserID { get; set; }
        public decimal? FCI { get; set; }
        public string ProjectPurpose { get; set; }
        public string ApplicantPan { get; set; }
        public string ApplicantPanAttachment { get; set; }
        public string CompanyPan { get; set; }
        public string CompanyPanAttachment { get; set; }
    }

    public class ImportDataViewModel
    {
        public virtual ApplicationAction ApplicationAction { get; set; }
        public virtual ShopLicence_GeneralDetail ShopLicence_GeneralDetail { get; set; }
    }
    public class ServiceCodeApplicationTypeMapperViewModel
    {
        public int ServiceCode { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
    }


    public class ProcessImd_ClearencesDataViewModel
    {
        public ProcessImd_ServiceViewModel ServiceData { get; set; }
        public List<ProcessImd_ClearencesDocumemtViewModel> Documents { get; set; }
        public List<ProcessImd_ClearencesLogViewModel> ServiceLogs { get; set; }
        public List<ProcessImd_ClearencesFeeViewModel> ClearencesFee { get; set; }
        public ProcessImd_ClearencesApplicantDetailsViewModel ApplicantDetails { get; set; }

        //Factory_Master

        public ProcessImd_ClearencesBuildingPlanViewModel BuildingPlan_Master { get; set; }
        public ProcessImd_ClearencesEstablishmentRegistrationViewModel PrincipalEmployer_Master { get; set; }
        public ProcessImd_ClearencesContractLabourViewModel ContractLabour_Master { get; set; }
        public ProcessImd_ClearencesShopLicenceViewModel Shop_Master { get; set; }
        public ProcessImd_ClearencesFactoryLicenceViewModel Factory_Master { get; set; }
        public ProcessImd_ClearencesBocwRegistrationViewModel BOCW_Master { get; set; }
        public ProcessImd_ClearencesTradeUnionViewModel TradeUnion_Master { get; set; }
        public ProcessImd_ClearencesInterstatePrincipalEmployersViewModel InterstatePrincipalEmployer_Master { get; set; }
        public ProcessImd_ClearencesMotorTransportViewModel MotorTransport_Master { get; set; }
        public ProcessImd_ClearencesProposed_BuildingPlanViewModel Proposed_BuildingPlan_Master { get; set; }
    }


    public class ProcessImd_ServiceViewModel
    {
        public ApplicationPurposeTypeEnum ClearenceType { get; set; }
        public string TokenNumber { get; set; }
        public int IsApproved { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string LicenseNo { get; set; }
        public string LicenseFile { get; set; }
        public DateTime ValidUpTo { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
    }

    public class ProcessImd_ClearencesDocumemtViewModel
    {
        public Int64 DocId { get; set; }
        public string DocumentFile { get; set; }
    }
    public class ProcessImd_ClearencesLogViewModel
    {
        public string SenderUserId { get; set; }
        public int SenderProfileID { get; set; }
        public string ReceiverUserId { get; set; }
        public int ReceiverProfileID { get; set; }
        public Int64 StatusId { get; set; }
        public string StatusDesc { get; set; }
        public DateTime StatusDate { get; set; }

        public int AppFormId { get; set; }
        public string NAR { get; set; }
        public Int64 AppId { get; set; }
        public string SenderRoleId { get; set; }
        public string ReceiverRoleId { get; set; }
    }
    public class ProcessImd_ClearencesFeeViewModel
    {
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string FeeType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string BANK_REFERENCE_NO { get; set; }
        public string ME_TXN_REF_NO { get; set; }
        public string ReqMsg { get; set; }
        public string ResMsg { get; set; }
    }

    public class ProcessImd_ClearencesEstablishmentRegistrationViewModel
    {
        public Int64 EstablishmentRegistrationId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalAddress { get; set; }
        public string PrincipalEmpName { get; set; }
        public string PrincipalEmpAddress { get; set; }
        public string PrincipalFatherEmpName { get; set; }
        public string ManagerName { get; set; }
        public string ManagerAddress { get; set; }
        public string WorkCarried { get; set; }
        public string Place { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime PDate { get; set; }
        public DateTime TDate { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public string OLBankChallanProof { get; set; }
        public DateTime OLRCIssuedDate { get; set; }
        public DateTime? OLFirstRCDate { get; set; }
        public DateTime? OLLastAmendDate { get; set; }
        public decimal? OLFirstRCFees { get; set; }
        public decimal? OLAmount { get; set; }
        public Int32? OLEmps { get; set; }
        public Int32? OLConts { get; set; }
        public decimal? RegistrationFee { get; set; }
        public string NAR { get; set; }
        public string ManagerEmailId { get; set; }
        public string ManagerMobileNo { get; set; }
        public string PrincipalEmpEmailId { get; set; }
        public string PrincipalEmpMobileNo { get; set; }
        public Int64? ProcessTime { get; set; }
        public string AmendmentFormChanges { get; set; }

        public int TotalWorker { get; set; }
    }

    public class ProcessImd_ClearencesBuildingPlanViewModel
    {
        public Int64 BuildingPlanId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 CompetentPersonListID { get; set; }
        public Int64 AppFormId { get; set; }
        public decimal BuildingCost { get; set; }
        public DateTime? PDate { get; set; }
        public DateTime? TDate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string BuildingInfo { get; set; }
        public string TypeOfApproval { get; set; }
        public bool IsBuildingConstBefore2008 { get; set; }
        public virtual Application Application { get; set; }
        public Int64 AppForm { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }
    }

    public class ProcessImd_ClearencesContractLabourViewModel
    {
        public Int64 ContractLabourId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorFatherName { get; set; }
        public string ContractorAddress { get; set; }
        public DateTime? DOB { get; set; }
        public bool IsCoopraticSociety { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string TypeOfBusiness { get; set; }
        public string RegistrationCertificateNo { get; set; }
        public DateTime? RegistrationCertificateDate { get; set; }
        public string PrincipalEmployerName { get; set; }
        public string PrincipalEmployerAddress { get; set; }
        public string WorkNatureString { get; set; }
        public DateTime ContractWork_CommencingDate { get; set; }
        public DateTime ContractWork_TerminationDate { get; set; }
        public string AgentOrManagerName { get; set; }
        public string AgentOrManagerAddress { get; set; }
        public Int64 MaximumNumberEmployee { get; set; }
        public decimal? fees { get; set; }
        public bool IsSecurityAdjusdRequested { get; set; }
        public string SecurityAdjusdReceiptNo { get; set; }
        public DateTime? SecurityAdjusdReceiptDate { get; set; }
        public decimal? SecurityAdjustableAmount { get; set; }
        public bool IsSecurityBalanceAfterAdjusd { get; set; }
        public string SecurityBalanceAfterAdjusdReceiptNo { get; set; }
        public DateTime? SecurityBalanceAfterAdjusdReceiptDate { get; set; }
        public decimal? SecurityBalanceAmountAfterAdjusd { get; set; }
        public DateTime? PDate { get; set; }
        public DateTime? TDate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public Int64 AppForm { get; set; }
        public int? Age { get; set; }
        public string LicenseNumber { get; set; }
        public string ContractorId { get; set; }
        public string DispatchNo { get; set; }
        public string UserId { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public DateTime? OLValidUpto { get; set; }
        public Int32? OLEmps { get; set; }
        public string NAR { get; set; }
        public Int64? ProcessTime { get; set; }
        public Int64 RegistrationFrom { get; set; }
        public string AmendmentFormChanges { get; set; }
    }

    public class ProcessImd_ClearencesShopLicenceViewModel
    {
        public Int64 ShopCommRegistrationId { get; set; }

        public Int64 AppId { get; set; }

        public Int64 AppFormId { get; set; }

        public string RegNo { get; set; }

        public string Name { get; set; }

        public string EstdType { get; set; }

        public DateTime? DateOfEstd { get; set; }

        public string NatureNICCode { get; set; }

        public string NICCode { get; set; }

        public Int64 Nature { get; set; }
        public string Address { get; set; }

        public Int64 TehsilId { get; set; }

        public string Pincode { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string OHours { get; set; }

        public string CHours { get; set; }

        public string CloseWeekday { get; set; }

        public string OwnerType { get; set; }

        public string OName { get; set; }

        public string OFHName { get; set; }

        public string OPhone { get; set; }

        public string OAddress { get; set; }

        public string MgrName { get; set; }

        public string Remarks { get; set; }

        public bool HasEmp { get; set; }

        public Int64 NoOfEmp { get; set; }

        public string NoPreReg { get; set; }

        public DateTime? DatePreReg { get; set; }

        public Int16 Status { get; set; }

        public Int64 LCGId { get; set; }

        public string ProofShopEstd { get; set; }

        public string PhotoFront { get; set; }

        public string PhotoInteriorOne { get; set; }

        public string OwnerProof { get; set; }

        public string OtherDocs { get; set; }

        public string AadharNo { get; set; }

        public string LicenseYear { get; set; }

        public string ApplicantSign { get; set; }

        public bool IsAgree { get; set; }

        public string NAR { get; set; }

        public bool IsUserRead { get; set; }
        public DateTime? UserReadDate { get; set; }

        public DateTime? InspectorReadDate { get; set; }
        public bool IsInspectorRead { get; set; }

        public DateTime? DateOfApproval { get; set; }

        public DateTime? tdate { get; set; }
        public DateTime? pdate { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public Int32? NoOfDays { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }

        public DateTime? OLValidUpto { get; set; }
        public DateTime? OLRegDate { get; set; }
        public Int32? OLEmps { get; set; }
    }

    public class ProcessImd_ClearencesFactoryLicenceViewModel
    {
        public Int64 FactoryLicenseId { get; set; }

        public Int64 AppId { get; set; }

        public Int64 AppFormId { get; set; }

        public string FactoryName { get; set; }

        public string FactoryLicenseNumber { get; set; }

        public string FactorySituationAddress { get; set; }

        public string FactoryCommunicationAddress { get; set; }

        public int MaximumNumberEmployeeInYear { get; set; }

        public int MaximumNumberEmployeeLastYear { get; set; }

        public int OrdinarilyEmployed { get; set; }

        public decimal InstalledPower { get; set; }

        public decimal MaximumPowerUsed { get; set; }

        public string ManagerFullName { get; set; }

        public string ManagerFullAddress { get; set; }

        public string ManagerFatherName { get; set; }

        public string OccupierFullName { get; set; }

        public string OccupierFullAddress { get; set; }

        public string OccupierFatherName { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPremisesAddress { get; set; }

        public string NICCode { get; set; }

        public string ReferenceNumberBuildingConst { get; set; }

        public DateTime? ReferenceDateBuildingConst { get; set; }

        public string ReferenceNumberTradeWaste { get; set; }

        public DateTime? ReferenceDateTradeWaste { get; set; }

        public int LicenceForNoOfYear { get; set; }

        public string CategoryOfLicence { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? LastInspectionDate { get; set; }

        public DateTime? PDate { get; set; }
        public DateTime? TDate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string OccupierInformation { get; set; }
        public System.Nullable<bool> FInspection { get; set; }
        public Int64? SeasonID { get; set; }

        public string AadhaarNo { get; set; }

        public string Section { get; set; }

        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public DateTime? OLValidUpto { get; set; }
        public Int32? OLEmps { get; set; }

        public string NAR { get; set; }
        public decimal? OLKiloWatt { get; set; }

        public DateTime? OLStartDate { get; set; }

        public string ManagerMobile { get; set; }

        public string ManagerEmail { get; set; }

        public string OccupierMobile { get; set; }

        public string OccupierEmail { get; set; }

        public Int64? ProcessTime { get; set; }

        public string AuthorityName { get; set; }

        public bool? IsVerified { get; set; }

        public string BusinessEntity { get; set; }
        public bool IsGovernment { get; set; }
        public bool IsMAH { get; set; }

        public int ShramSuvidhaStatus { get; set; }
        public string LIN { get; set; }
        public DateTime? AmendmentDate { get; set; }
        public decimal? AmndChangeAmt { get; set; }
        public string AmendmentInfo { get; set; }
        public string MFProc { get; set; }
        public DateTime? FactorySetupDate { get; set; }
    }

    public class ProcessImd_ClearencesBocwRegistrationViewModel
    {
        public Int64 BOCWEstbRegistrationId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public string EstbEmployerName { get; set; }
        public string EstbEmployerFatherName { get; set; }
        public string EstbEmployerAddress { get; set; }
        public string EstbEmployerMobile { get; set; }
        public string EstbEmployerEmail { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string EstablishmentPostalAddress { get; set; }
        public string PrincipalEmpName { get; set; }
        public string PrincipalEmpAddress { get; set; }
        public string ManagerName { get; set; }
        public string ManagerAddress { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerMobile { get; set; }
        public string NatureOfConstruction { get; set; }
        public int MaximumNumberOfWorkers { get; set; }
        public string WorkersSlab { get; set; }
        public DateTime Commencement_Date { get; set; }
        public DateTime Completion_Date { get; set; }
        public decimal? FeeDeposited { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime TDate { get; set; }
        public DateTime SubmitionDate { get; set; }
        public int? OLEmps { get; set; }
        public string OLProof { get; set; }
        public string OLNo { get; set; }
        public DateTime? OLFirstRCDate { get; set; }
        public DateTime? OLLastAmendDate { get; set; }
        public decimal? OLFirstRCFees { get; set; }
        public string BOCWCircleType { get; set; }
    }

    public class ProcessImd_ClearencesTradeUnionViewModel
    {
        public Int64 TradeUnionId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public string RegNo { get; set; }
        public string TUName { get; set; }
        public DateTime? DateOfOrigin { get; set; }
        public string TUAddress { get; set; }
        public Int64 TehsilId { get; set; }
        public string Pincode { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public string WorkEngaged { get; set; }
        public DateTime? DateOfApproval { get; set; }
        public DateTime? tdate { get; set; }
        public DateTime? pdate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public Int32? NoOfDays { get; set; }
        public string OLNo { get; set; }
        public DateTime? OLIssueDate { get; set; }
        public string OLProof { get; set; }
        public virtual Application Application { get; set; }
    }


    public class ShopTehsilsViewModel
    {
        public Int64 TehsilId { get; set; }
        public string TehsilName { get; set; }
        public Int64 DistrictId { get; set; }
        public Int64 IsSubTehsil { get; set; }
        public DateTime? PDate { get; set; }
        public DateTime? TDate { get; set; }
        public Int64 Enabled { get; set; }
        public Int64 LGDirTehsilCode { get; set; }
        public Int64 LGDistrictCode { get; set; }
       
    }

    public class ProcessImd_ClearencesInterstatePrincipalEmployersViewModel
    {
        public Int64 InterstatePEId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalAddress { get; set; }
        public string NameOfDirector { get; set; }
        public string AddressOfDirector { get; set; }
        public string PrincipalEmpName { get; set; }
        public string PrincipalEmpAddress { get; set; }
        public string PrincipalFatherEmpName { get; set; }
        public string ManagerName { get; set; }
        public string ManagerAddress { get; set; }
        public string WorkCarried { get; set; }
        public string Place { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime PDate { get; set; }
        public DateTime TDate { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public Int32? OLEmps { get; set; }
        public Int32? OLConts { get; set; }
        public decimal? RegistrationFee { get; set; }
        public string NAR { get; set; }
        public string ManagerEmailId { get; set; }
        public string ManagerMobileNo { get; set; }
        public string PrincipalEmpEmailId { get; set; }
        public string PrincipalEmpMobileNo { get; set; }
        public Int64? ProcessTime { get; set; }
        public Int64? NoOfDays { get; set; }
    }

    public class ProcessImd_ClearencesMotorTransportViewModel
    {
        public Int64 MotorTransportId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public string RegNo { get; set; }
        public string NameMotorTrans { get; set; }
        public string CommAddress { get; set; }
        public string CommPincode { get; set; }
        public Int64 CommAddressTehsilId { get; set; }
        public string NameTransService { get; set; }
        public Int64 TotalNoOfRoutes { get; set; }

        public Int64 TotalMileage { get; set; }
        public Int64 TotalNoVehiclesPreceedingYear { get; set; }
        public Int64 MaxWorkersLastYear { get; set; }
        public string WhichAuthorty { get; set; }
        public string AuthorityFullName { get; set; }
        public string AuthorityAddress { get; set; }
        public string AuthorityMobile { get; set; }
        public string AuthorityEmail { get; set; }
        public string ScannedUndertaking { get; set; }
        public bool IsCompany { get; set; }
        public string DirectorName { get; set; }
        public string DirectorAddress { get; set; }
        public string DirectorMobile { get; set; }
        public string DirectorEmail { get; set; }
        public Int32? NoOfDays { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public DateTime? OLValidUpto { get; set; }
        public Int32? OLEmps { get; set; }
        public Decimal RegistrationFee { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime tdate { get; set; }
        public DateTime pdate { get; set; }
        public virtual Application Application { get; set; }
        public int? RegistrationYearFor { get; set; }
        public string LicenceApplyFor { get; set; }
    }

    public class ProcessImd_ClearencesApplicantDetailsViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ClientPasswdHash { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
    }
    public class NarDataViewModel
    {
        public string NAR { get; set; }
        public int AppFormId { get; set; }
        public DateTime TDate { get; set; }
    }
    public class CreateUserProcessResponseViewModel
    {
        public string UserId { get; set; }
        public Int64 ProjectSiteId { get; set; }
        public string ContactSnapshot { get; set; }
        public int ProjectSiteVersion { get; set; }
    }

    public class ProcessImd_LegacyDocumentInfoViewModel
    {
        public Int64 SYS_N_DocId { get; set; }
        public string Legacy_FileName { get; set; }

    }

    public class ImportAndSeedDataResponseViewModel
    {
        public Int64 AppRefId { get; set; }
        public string UserRefId { get; set; }
    }
    public class PrincipalApprovalData
    {
        public int ipin { get; set; }
        public long serviceid { get; set; }
        public string approval_file { get; set; }
        public string servicename { get; set; }
        public string applicantname { get; set; }
        public string idproofnumber { get; set; }
        public string idproofattachment { get; set; }
        public string pannumber { get; set; }
        public string panattachment { get; set; }
        public string applicantphoneno { get; set; }
        public string applicantmobileno { get; set; }
        public string applicantemailid { get; set; }
        public string applicantaddress { get; set; }
        public string applicantstate { get; set; }
        public string applicantdistrict { get; set; }
        public string areaclassification { get; set; }
        public int? applicanttehsil { get; set; }
        public string applicantvillage { get; set; }
        public string applicantpincode { get; set; }
        public string majoractivities { get; set; }
        public string servicesprovided { get; set; }
        public string projectname { get; set; }
        public string projectpurpose { get; set; }
        public string projectaddress { get; set; }
        public string allotmentletter { get; set; }
        public string typeofindustry { get; set; }
        public string landcost { get; set; }
        public string buildingcost { get; set; }
        public string plantcost { get; set; }
        public string othercost { get; set; }
        public string totalinvestmentcost { get; set; }
        public string directempmale { get; set; }
        public string directempfemale { get; set; }
        public string indirectempmale { get; set; }
        public string indirectempfemale { get; set; }
        public string totalempmale { get; set; }
        public string totalempfemale { get; set; }
        public string ownershipdetail { get; set; }
        public string requirementofwater { get; set; }
        public string address2 { get; set; }
        public string be_name { get; set; }
        public string business_entity_type { get; set; }
        public string applicationid { get; set; }
        public string userid { get; set; }
        public string form_id { get; set; }
        public string order_id { get; set; }
        public string status { get; set; }
        public DateTime tdate { get; set; }
        //public string projectvillage { get; set; }
        public DateTime pdate { get; set; }
        public DateTime applicationdate { get; set; }
        public string paytitle { get; set; }
        public string mclimit { get; set; }
        public string landarea { get; set; }
        public string built_up_area { get; set; }
        public string tradecity { get; set; }
        //public string tradearray { get; set; }
        //public string accarray { get; set; }
        public string statename { get; set; }
        public string districtname { get; set; }
        public string tehsilname { get; set; }
        public string classificationzone { get; set; }
        public string landusecategory { get; set; }
        public string landusecategoryname { get; set; }
        public string undertakingfile { get; set; }
        public string designationname { get; set; }
        public string projectstatename { get; set; }
        public string projectdistrictname { get; set; }
        public string projecttehsilname { get; set; }
        public string projectvillagetown { get; set; }
        public string projectpincode { get; set; }
        public string beaddress1 { get; set; }
        public string beaddress2 { get; set; }
        public string be_city_village { get; set; }
        public string be_pincode { get; set; }
        public string bedistrictname { get; set; }
        public string besubdistrictname { get; set; }
        public string buildingapprovalbylg_filename { get; set; }
        public string buildingapprovalbyfactories_filename { get; set; }
        public string fireapproval_filename { get; set; }
        public string tradeapproval_filename { get; set; }
        public string clu_filename { get; set; }
        public string location_filename { get; set; }
        public string sitesituated { get; set; }
        public string cluexemption_filename { get; set; }
        public string runningmeter { get; set; }
        public string approvedsite { get; set; }
        public string ulbName { get; set; }
        public string urban_local_body_classification { get; set; }
        public string authority { get; set; }
        public string edcfilename { get; set; }
        public string total_amount { get; set; }

        //public List<BifurcationInfoViewModel> bifurcation_info { get; set; }
        [NotMapped]
        public string[] bifurcation_info { get; set; }
        public string approvaldate { get; set; }
    }

    public class PrincipalApproval_RBA_DetailsViewModel
    {
        public bool success { get; set; }
        public List<PrincipalApprovalData> data { get; set; }
    }


    public class ProcessImd_ClearencesInterstateContractLabourViewModel
    {
        public Int64 ContractLabourId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorFatherName { get; set; }
        public string ContractorAddress { get; set; }
        public DateTime? DOB { get; set; }
        public bool IsCoopraticSociety { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string TypeOfBusiness { get; set; }
        public string RegistrationCertificateNo { get; set; }
        public DateTime? RegistrationCertificateDate { get; set; }
        public string PrincipalEmployerName { get; set; }
        public string PrincipalEmployerAddress { get; set; }
        public string WorkNatureString { get; set; }
        public DateTime ContractWork_CommencingDate { get; set; }
        public DateTime ContractWork_TerminationDate { get; set; }
        public string AgentOrManagerName { get; set; }
        public string AgentOrManagerAddress { get; set; }
        public Int64 MaximumNumberEmployee { get; set; }
        public decimal? fees { get; set; }
        public bool IsSecurityAdjusdRequested { get; set; }
        public string SecurityAdjusdReceiptNo { get; set; }
        public DateTime? SecurityAdjusdReceiptDate { get; set; }
        public decimal? SecurityAdjustableAmount { get; set; }
        public bool IsSecurityBalanceAfterAdjusd { get; set; }
        public string SecurityBalanceAfterAdjusdReceiptNo { get; set; }
        public DateTime? SecurityBalanceAfterAdjusdReceiptDate { get; set; }
        public decimal? SecurityBalanceAmountAfterAdjusd { get; set; }
        public DateTime? PDate { get; set; }
        public DateTime? TDate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public Int64 AppForm { get; set; }
        public int? Age { get; set; }
        public string LicenseNumber { get; set; }
        public string ContractorId { get; set; }
        public string DispatchNo { get; set; }
        public string UserId { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public DateTime? OLValidUpto { get; set; }
        public Int32? OLEmps { get; set; }
        public string NAR { get; set; }
        public Int64? ProcessTime { get; set; }
        public string RegistrationFrom { get; set; }
        public string AmendmentFormChanges { get; set; }
    }

    public class BifurcationInfoViewModel
    {
        public string[] value { get; set; }
        //public int Id { get; set; }
        //public string BifurcationName { get; set; }
        //public decimal BifurcationAmount { get; set; }
    }

    public class InvestPunjabApiTokenViewModel
    {
        public string Token { get; set; }
    }

    public class Sys_O_AppId_TokenInfoViewModel
    {
        public Int64 AppId { get; set; }
        public string TokenNumber { get; set; }
    }

    public class Sys_O_AppId_OldDataViewModel
    {
        public Int64 AppId { get; set; }
        public string LicenceNo { get; set; }
    }

    public class Factory_TemporaryLicenceDetailsViewModel
    {
        public Int64 FactoryLicenseId { get; set; }

        public Int64 AppId { get; set; }

        public Int64 AppFormId { get; set; }

        public string FactoryName { get; set; }

        public string FactoryLicenseNumber { get; set; }

        public string FactorySituationAddress { get; set; }

        public string FactoryCommunicationAddress { get; set; }

        public Int64 MaximumNumberEmployeeInYear { get; set; }

        public Int64 MaximumNumberEmployeeLastYear { get; set; }

        public Int64 OrdinarilyEmployed { get; set; }

        public decimal InstalledPower { get; set; }

        public decimal MaximumPowerUsed { get; set; }

        public string ManagerFullName { get; set; }

        public string ManagerFullAddress { get; set; }
        public string ManufacturingProcesses { get; set; }

        public string ManagerFatherName { get; set; }

        public string OccupierFullName { get; set; }

        public string OccupierFullAddress { get; set; }

        public string OccupierFatherName { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPremisesAddress { get; set; }

        public string NICCode { get; set; }

        public string ReferenceNumberBuildingConst { get; set; }

        public DateTime? ReferenceDateBuildingConst { get; set; }

        public string ReferenceNumberTradeWaste { get; set; }

        public DateTime? ReferenceDateTradeWaste { get; set; }

        public int LicenceForNoOfYear { get; set; }

        public string CategoryOfLicence { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? LastInspectionDate { get; set; }

        public DateTime? PDate { get; set; }
        public DateTime? TDate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string OccupierInformation { get; set; }
        public System.Nullable<bool> FInspection { get; set; }
        public Int64? SeasonID { get; set; }

        public string AadhaarNo { get; set; }

        public string Section { get; set; }

        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public DateTime? OLValidUpto { get; set; }
        public Int32? OLEmps { get; set; }

        public string NAR { get; set; }
        public decimal? OLKiloWatt { get; set; }

        public DateTime? OLStartDate { get; set; }

        public string ManagerMobile { get; set; }

        public string ManagerEmail { get; set; }

        public string OccupierMobile { get; set; }

        public string OccupierEmail { get; set; }

        public Int64? ProcessTime { get; set; }

        public string AuthorityName { get; set; }

        public bool? IsVerified { get; set; }

        public string BusinessEntity { get; set; }
        public bool IsGovernment { get; set; }
        public bool IsMAH { get; set; }

        public int ShramSuvidhaStatus { get; set; }
        public string LIN { get; set; }
        public DateTime? AmendmentDate { get; set; }
        public decimal? AmndChangeAmt { get; set; }
        public string AmendmentInfo { get; set; }
        public string MFProc { get; set; }
        public DateTime? FactorySetupDate { get; set; }
        public decimal TXN_AMOUNT { get; set; }
        public DateTime PaymentDate { get; set; }
        public string BANK_REFERENCE_NO { get; set; }
        public string ME_TXN_REF_NO { get; set; }
        public string ReqMsg { get; set; }
        public string ResMsg { get; set; }
    }
    public class Factory_DOFLicenceDetailsViewModel
    {
        public bool success { get; set; }
    }

    public class GetLicenceNumberAndActTypeViewModel
    {
        public string LicenceNumber { get; set; }
        public Int64 ApplicationType { get; set; }
    }
    public class CheckAlreadyInProcessViewTypeViewModel
    {
        public string Legacy_LicenceNo { get; set; }
        public Int64 ApplicationType { get; set; }
    }

    public class GetRedirectUrlViewModel
    {
        public string RedirectUrl { get; set; }
    }

    public class ProcessImd_ClearencesProposed_BuildingPlanViewModel
    {
        public decimal BuildingCost { get; set; }
        public bool IsBuildingConstBefore2008 { get; set; }
        public Int64 BOCW_NoOfWorkers { get; set; }
        public DateTime IssueDate { get; set; }
    }

    public class GetAllDirtyApplicationsViewModel
    {
        public string IPin { get; set; }
        public Int64 ApplicationId { get; set; }
        public int StatusId { get; set; }
        public string StatusDesc { get; set; }
        public DateTime StatusDate { get; set; }
        public string SenderName { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderDesignation { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverDesignation { get; set; }
        public string Comments { get; set; }
        public string LicencePath { get; set; }
        public Int64 DaysTaken { get; set; }
        public string IntegrationSource { get; set; }
        public string DeemedApproval { get; set; }
        public Int64 appActionLogId { get; set; }
        public int IsLegacy { get; set; }
        public Int64 AppRefId { get; set; }
        public int AppFormId { get; set; }
        public string ClearanceIssuedOn { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string LicenceNumber { get; set; }
    }

 

        public class IPinInfoViewModel
    {
        public Int64 IPin { get; set; }
        public List<ApplicationIdInfoViewModel> Applications { get; set; }
    }
    public class ApplicationIdInfoViewModel
    {
        public Int64 ApplicationId { get; set; }
        public List<ApplicationLogViewModel> ApplicationLogs { get; set; }
    }

    public class ApplicationLogViewModel
    {
        public Int64 StatusId { get; set; }
        public string StatusDesc { get; set; }
        public DateTime StatusDate { get; set; }
        public string SenderName { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderDesignation { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string ReceiverDesignation { get; set; }
        public string Comments { get; set; }
        public string LicencePath { get; set; }
        public Int64 DaysTaken { get; set; }
        public string IntegrationSource { get; set; }
        public string DeemedApproval { get; set; }
        public Int64 appActionLogId { get; set; }
        public string ClearanceIssuedOn { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string LicenceNumber { get; set; }
    }

    public class DirtyFlagRequestParamsViewModel
    {
        public Int64 IPin { get; set; }
        public Int64 ApplicationId { get; set; }
        public int ServiceCode { get; set; }
    }
    public class SetIsDirtyFlagForcefullyViewModel
    {
        public Int64 IPin { get; set; }
        public Int64 ApplicationId { get; set; }
        public int ServiceCode { get; set; }
        public bool IsSuccess { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }

    public class SharedAppLogDetailVewModel
    {
        public Int64 IPin { get; set; }
        public Int64 ApplicationId { get; set; }
        public string AppLogsIds { get; set; }
        public int IsLegacy { get; set; }
    }

    public class LegacyLogsViewModel
    {
        public Int64 AppPBIPActionLogId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string TokenNumber { get; set; }

        public Int64 DeptId { get; set; }

        public Int64 StatusId { get; set; }

        public string StatusDesc { get; set; }

        public DateTime StatusDate { get; set; }

        public bool Stage1Locked { get; set; }

        public DateTime Stage1LockedDate { get; set; }

        public bool Stage1Cleared { get; set; }

        public DateTime Stage1ClearedDate { get; set; }
        public string Stage1NextPage { get; set; }

        public DateTime PDate { get; set; }

        public DateTime TDate { get; set; }

        public bool IsActive { get; set; }
        public Int64 AppMultipalLicenseId { get; set; }

        public string NAR { get; set; }

        public string PastStatus { get; set; }

        public string PaymentMode { get; set; }
        public int SenderProfileID { get; set; }
        public int ReceiverProfileID { get; set; }
        public string LogsDetails { get; set; }
    }

    public class LegacyApplicationViewModel
    {
        public Int64 AppId { get; set; }

        public String AppFor { get; set; }

        public string TokenNumber { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public Int64 CountryId { get; set; }

        public Int64 StateId { get; set; }

        public Int64 DistrictId { get; set; }

        public string City { get; set; }

        public string SiteAddCity { get; set; }

        public string PinCode { get; set; }

        public string SiteAddPinCode { get; set; }

        public string AddressLine1 { get; set; }


        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string PhoneNo { get; set; }

        public string ISDCode { get; set; }

        public string MobileNo { get; set; }

        public string FaxNo { get; set; }

        public string Email { get; set; }

        public string AlternateEmail { get; set; }

        public string BusinessEntity { get; set; }

        public string ProjectPurpose { get; set; }

        public string SiteAddress { get; set; }

        public System.Nullable<Int64> FCId { get; set; }

        public System.Nullable<Int64> LCId { get; set; }

        public System.Nullable<Int64> LabourInspCircleGradeId { get; set; }

        public System.Nullable<Int64> ALLCCircleId { get; set; }

        public virtual FactoryCircle FactoryCircle { get; set; }

        public System.Nullable<Int64> CompetentPersonId { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public String Place { get; set; }

        public DateTime PDate { get; set; }

        public DateTime TDate { get; set; }

        public string ApplicationUserID { get; set; }

        public bool? IsAssigned { get; set; }
        public bool? LInspection { get; set; }

        public Int64? SiteAddTehsilId { get; set; }

        public Int64? CommAddTehsilId { get; set; }

        public Int64? SevaKendraId { get; set; }

        public string AadhaarNo { get; set; }

        public bool IsVerified { get; set; }
        public string EstdName { get; set; }
        public string ApplyFrom { get; set; }

        public Int64 DistrictLgdId { get; set; }
        public Int64 LGDirTehsilCode { get; set; }
    }

    public class LegacyUserViewModel
    {
        public string UserName { get; set; }
        public string ClientPasswdHash { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }
    public class LegacyBuildingPlanMasterDataViewModel
    {
        public Int64 BuildingPlanId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 CompetentPersonListID { get; set; }
        public Int64 AppFormId { get; set; }
        public decimal BuildingCost { get; set; }
        public DateTime? PDate { get; set; }
        public DateTime? TDate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string BuildingInfo { get; set; }
        public string TypeOfApproval { get; set; }
        public bool IsBuildingConstBefore2008 { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime CompPerVisitDate { get; set; }
        public string NAR { get; set; }
        public int bocw_numberworker { get; set; }
    }


    public class LegacyMotorTransportMasterDataViewModel
    {
        public Int64 MotorTransportId { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public string NAR { get; set; }
        public string RegNo { get; set; }
        public string NameMotorTrans { get; set; }
        public string CommAddress { get; set; }
        public string CommPincode { get; set; }
        public Int64 CommAddressTehsilId { get; set; }
        public string NameTransService { get; set; }
        public Int64 TotalNoOfRoutes { get; set; }

        public Int64 TotalMileage { get; set; }
        public Int64 TotalNoVehiclesPreceedingYear { get; set; }
        public Int64 MaxWorkersLastYear { get; set; }
        public string WhichAuthorty { get; set; }
        public string AuthorityFullName { get; set; }
        public string AuthorityAddress { get; set; }
        public string AuthorityMobile { get; set; }
        public string AuthorityEmail { get; set; }
        public string ScannedUndertaking { get; set; }
        public bool IsCompany { get; set; }
        public string DirectorName { get; set; }
        public string DirectorAddress { get; set; }
        public string DirectorMobile { get; set; }
        public string DirectorEmail { get; set; }
        public Int32? NoOfDays { get; set; }
        public string OLNo { get; set; }
        public string OLProof { get; set; }
        public DateTime? OLValidUpto { get; set; }
        public Int32? OLEmps { get; set; }
        public Decimal RegistrationFee { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime tdate { get; set; }
        public DateTime pdate { get; set; }
        public virtual Application Application { get; set; }
        public int? RegistrationYearFor { get; set; }
        public string LicenceApplyFor { get; set; }
    }
    public class IpinAndAppIdViewModel
    {
        public Int64 Ipin { get; set; }
        public Int64 ApplicationId { get; set; }
    }

    public class AppSubmissionResponseViewModel
    {
        public Int64 Ipin { get; set; }
        public Int64 ApplicationId { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

      public class LegacyUserDetailsViewModel
    {
        public string StepDescription { get; set; }
        public string Status { get; set; }
    }
    public class LegacyAppDocumentViewModel
    {
        public int DocId { get; set; }
        public string FileName { get; set; }
        public DateTime TDate { get; set; }
    }
    public class LegacyAppPaymentDetailsViewModel
    {
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string BankTransactionRefNumber { get; set; }
        public string ReqMsg { get; set; }
        public string ResMsg { get; set; }
    }

    public class LegacyAppClearanceIssuedViewModel
    {
        public Int64 AppClearanceIssuedID { get; set; }
        public Int64 AppId { get; set; }
        public Int64 AppFormId { get; set; }
        public Int64 DocId { get; set; }
        public string TypeOfClearance { get; set; }
        public DateTime ClearanceIssuedOn { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string LicenceNo { get; set; }
        public DateTime PDate { get; set; }
        public DateTime TDate { get; set; }
        public Int64? AppMultipalLicenseId { get; set; }
        public string NAR { get; set; }
        public string LicencePath { get; set; }
        public string LicOrReg { get; set; }
    }

    public class HRMSApplicationPurposeTypeViewModel
    {

        public HRMS_ActAndApplicationPurposewiseDataViewModel Registration { get; set; }
        public HRMS_ActAndApplicationPurposewiseDataViewModel Renewal { get; set; }
        public HRMS_ActAndApplicationPurposewiseDataViewModel Ammendment { get; set; }
    }

    public class HRMSDataViewModel
    {

        public Int64 UserProfileRefId { get; set; }
        public int ApplicationType { get; set; }
        public int applicationPurposeType { get; set; }
        public string CircleName { get; set; }
        public int TotalPendingInBeginning { get; set; }
        public int TotalReceived { get; set; }
        public int TotalApproved { get; set; }
        public int TotalObjectionRaised { get; set; }
        public int TotalRejected { get; set; }
        public int TotalPending { get; set; }
        public int ObjectionsResolved { get; set; }
        public int AutoApproved { get; set; }

    }

    public class HRMSRequestParamsViewModel
    {
        public int hrmsCode { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }

    public class HRMS_ActAndApplicationPurposewiseDataViewModel
    {
        public int NumberOfApplicationsPendingInTheBeginningOfTheYear { get; set; }
        public int ReceivedDuringTheYear { get; set; }
        public int TotalNumberOfCases { get; set; }
        public int NumberOfRegistrationIssuedAndLicenceGrantedDuringTheYear { get; set; }
        public int ObjectionRaisedDuringTheYear { get; set; }
        public int FilesRejectedDuringTheYear { get; set; }
        public int NumberOfApplicationsPendingAtTheEndOfYear { get; set; }

    }

    public class HRMS_ApplicationPurposeTypeCategoryViewModel
    {
        public string OfficerName { get; set; }
        public int HrmsCode { get; set; }
        public List<HRMS_ServiceCodeWiseViewModel> Service { get; set; }
        public List<HRMS_InspectionServiceCodeWiseViewModel> Inspections { get; set; }

    }
    public class HRMS_CircleWiseCountHeadViewModel
    {
        public string CircleName { get; set; }
        public HRMS_ActAndApplicationPurposewiseDataViewModel CountHeads { get; set; }
    }

    public class HRMS_OfficerDetailsViewModel
    {
        public string OfficerName { get; set; }
        //public string OfficerDesignation { get; set; }
    }

    public class HRMS_CircleWiseInspectionsCountHeadViewModel
    {
        public string CircleName { get; set; }
        public HRMS_InspectionwiseDataViewModel CountHeads { get; set; }
    }

    public class HRMS_InspectionwiseDataViewModel
    {
        public int NumberOfInspectionsPendingInTheBeginningOfTheYear { get; set; }
        public int NumberOfInspectionsAssignedInTheYear { get; set; }
        public int TotalNumberOfInspections { get; set; }
        public int NumberOfInspectionsConductedDuringTheYear { get; set; }
        public int NumberOfInspectionsPendingAtTheEndOfYear { get; set; }
    }

    public class HRMS_RoleIdByHRMSServiceCodeViewModel
    {
        public string RoleId { get; set; }
    }

    public class HRMS_ServiceCodeWiseViewModel
    {
        public int ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public List<HRMS_CircleWiseCountHeadViewModel> Data { get; set; }

    }

    public class HRMS_InspectionServiceCodeWiseViewModel
    {
        public int ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public List<HRMS_InspectionCircleWiseCountHeadViewModel> Data { get; set; }

    }

    public class HRMS_InspectionCircleWiseCountHeadViewModel
    {
        public string CircleName { get; set; }
        public HRMS_InspectionwiseDataViewModel CountHeads { get; set; }
    }


    public class WithdrawApplicationReqParamsViewModel
    {
        public Int64 IPin { get; set; }
        public Int64 AppId { get; set; }
        public int ServiceCode { get; set; }
        public string CategoryType { get; set; }
    }

    public class WithdrawApplicationViewModel
    {
        public Int64 AppRefId { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public int ProjectSiteVersion { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }

        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public Int64 IPin { get; set; }
        public Int64 AppId { get; set; }
    }

    public class WithdrawApplicationSubmissionViewModel
    {
        public Int64 AppRefId { get; set; }
        public string Remarks { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public Int64 IPin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public int ProjectSiteVersion { get; set; }
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }
        public string RootActivityRefId { get; set; }
    }

    public class BocwHRMSRequestParamsViewModel
    {
        public int hrmsCode { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int WingType { get; set; }
    }

    public class APIResponse
    {

        public HttpSmsResponseMessage result { get; set; }

    }
    public class HttpSmsResponseMessage
    {

        public int? returnCode { get; set; }

        public string returnMsg { get; set; }

        public string data { get; set; }
    }
    public class ApplicationData
    {
        public int ApplicationType { get; set; }
        public int ApplicationPurposeType { get; set; }
        public string Circle { get; set; }
        public int PendingInTheBeginningOfTheYear { get; set; }
        public int ApplicationsReceived { get; set; }
        public int ObjectionsRaised { get; set; }
        public int TotalNoofApprovedApplications { get; set; }
        public int PendingAtTheEndOfTheYear { get; set; }
    }

    public class SchemeData
    {
        public int ApplicationType { get; set; }
        public int ApplicationPurposeType { get; set; }
        public string Circle { get; set; }
        public int PendingInTheBeginningOfTheYear { get; set; }
        public int ApplicationsReceived { get; set; }
        public int TotalNoofApprovedSchemes { get; set; }
        public int ObjectionsRaised { get; set; }
        public int ObjectionsResolved { get; set; }
        public int PendingAtTheEndOfTheYear { get; set; }
    }

    public class WorkerData
    {
        public int ApplicationType { get; set; }
        public int ApplicationPurposeType { get; set; }
        public string Circle { get; set; }
        public int PendingInTheBeginningOfTheYear { get; set; }
        public int ApplicationsReceived { get; set; }
        public int ObjectionsResolved { get; set; }
        public int ObjectionsRaised { get; set; }
        public int TotalNoofRegisteredBeneficiaries { get; set; }
        public int PendingAtTheEndOfTheYear { get; set; }
    }

    // Main Data ViewModel
    public class DataViewModel
    {
        public List<SchemeData> Scheme_Data { get; set; }
        public List<WorkerData> Worker_Data { get; set; }

    }

    public class APIResponseDataViewModel
    {
        public List<ApplicationData> ALCData { get; set; }
        public DataViewModel LabourData { get; set; }

    }

    public class ApiResponse
    {
        public Result Result { get; set; }
    }

    public class Result
    {
        public int ReturnCode { get; set; }
        public string ReturnMsg { get; set; }
        public string Data { get; set; }
    }


    public class UpdatedApplicationsParamsViewModel
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class PendingApplicationsByServiceCodeParamsViewModel
    {
        //public string fromDate { get; set; }
        //public string toDate { get; set; }
        public int serviceCode { get; set; }
        public PendencyTypeEnum pendencyType { get; set; }
    }

    public class ApplicationCurrentStatusParamsViewModel
    {
        public Int64 ipin { get; set; }
        public Int64 applicationId { get; set; }
        public int serviceCode { get; set; }
    }

    public class PendingApplicationDetailsViewModel
    {
        public int ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int PendingAtDepartment { get; set; }
        public int PendingAtInvestor { get; set; }
    }
    public class PendingApplicationDetailsByServiceCodeViewModel
    {
        public Int64 IPin { get; set; }
        public Int64 ApplicationId { get; set; }
        public string IntegrationDept { get; set; }
        public Int64 ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public DateTime? SubmittedOn { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int NoOfActions { get; set; }
        public Int64 LastLogId { get; set; }
    }


    public class ApplicationCurrentStatusViewModel
    {
        public Int64 IPin { get; set; }
        public Int64 ApplicationId { get; set; }
        public Int64 StatusId { get; set; }
        public string StatusDesc { get; set; }
        public string Comments { get; set; }
        public string SenderName { get; set; }
        public string SenderDesignation { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverDesignation { get; set; }
        public DateTime ClearanceIssuedOn { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string LicenseNo { get; set; }
        public string ClearanceFile { get; set; }
        public DateTime StatusDate { get; set; }
        public string IntegrationSource { get; set; }
        public string DeemedApproval { get; set; }
        public Int64 DepartmentTakenTotalTime { get; set; }
        public string TimeType { get; set; }
        public Int64 AppActionLogId { get; set; }
    }

    public class IpinAndApplicationIdInfoViewModel
    {
        public string IPin { get; set; }
        public Int64 ApplicationId { get; set; }
        public List<ApplicationStatusLogsViewModel> ApplicationLogs { get; set; }
    }

    public class ApplicationStatusLogsViewModel
    {
        public Int64 StatusId { get; set; }
        public string StatusDesc { get; set; }
        public string Comments { get; set; }
        public string SenderName { get; set; }
        public string SenderDesignation { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverDesignation { get; set; }
        public DateTime ClearanceIssuedOn { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string LicenseNo { get; set; }
        public string ClearanceFile { get; set; }
        public DateTime StatusDate { get; set; }
        public string IntegrationSource { get; set; }
        public string DeemedApproval { get; set; }
        public Int64 DepartmentTakenTotalTime { get; set; }
        public string TimeType { get; set; }
        public Int64 AppActionLogId { get; set; }
    }

    public class GetReportByDepartmentViewModel
    {
        public int departmentID { get; set; }

        public string departmentName { get; set; }

        [JsonProperty("TotalApplicationReceived")]
        public int TotalApplicationReceived { get; set; }

        [JsonProperty("PendingBeyondTimelineApplicationCount")]
        public int PendingBeyondTimelineApplicationCount { get; set; }
    }

    public class GetReportByAuthorityViewModel
    {
        public int departmentID { get; set; }
        public int authorityID { get; set; }
      


    }
    public class GetReportByAuthorityCountsViewModel
    {                                                                                                                   
        public int authorityID { get; set; }
        public string authorityName { get; set; }                                                                                                                
        public int departmentID { get; set; }

        [JsonProperty("TotalApplicationReceived")]
        public int TotalApplicationReceived { get; set; }

        [JsonProperty("PendingBeyondTimelineApplicationCount")]
        public int PendingBeyondTimelineApplicationCount { get; set; }
     
    }

    public class GetReportByServiceViewModel
    {
        public int departmentID { get; set; }
        public int authorityID { get; set; }
        public int serviceID { get; set; }


    }
    public class GetReportByServiceCountsViewModel
    {
        public int serviceID { get; set; }
        public string serviceName { get; set; }
        public int authorityID { get; set; }
        public int departmentID { get; set; }

        [JsonProperty("TotalApplicationReceived")]
        public int TotalApplicationReceived { get; set; }

        [JsonProperty("PendingBeyondTimelineApplicationCount")]
        public int PendingBeyondTimelineApplicationCount { get; set; }

    }

    public class DownloadApprovalViewModel
    {
        public bool IsCertificateDownload { get; set; }
        public string MessageText { get; set; }
        public int DaysLeft { get; set; }
        public string CertificatePath { get; set; }    
    }


    public class PartnerPortalLoginResponseViewModel
    {
        public bool HasError { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public DateTime RequestValidUpto { get; set; }
        public string Token { get; set; }
    }

    public class BOCWTransparencyViewModel
    {
        public string SchemeName { get; set; }
        public int SchemeId { get; set; }
        public int Submitted { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Pending { get; set; }
        public int Objections { get; set; }
    }
}
