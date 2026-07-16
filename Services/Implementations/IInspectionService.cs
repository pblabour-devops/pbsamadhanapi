using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IInspectionService
    {
        Task<GenericResponseTemplateModel<List<Inspection_RandomizeDashboardDataViewModel>>> GetInspection_Randomization( string Id, Int64 id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray);
        Task<GenericResponseTemplateModel<List<InspectioninfoViewModel>>> Get_InspectionsByRandomization(Int64 id, Int64 factoryRefId,string roleName, string userRefId);
        Task<GenericFormModel<LabourCircleUserDetailsViewModel>> GetInspection_LabourCircleUserDetails(Int64 circleIds, Int64 labourWingProfileRefId);
        Task<GenericFormModel<Inspection_Form_Factory_Part_I_General>> GetIncpectionFormFactoryPartIGeneralDetail(Int64 id);
        Task<GenericFormModel<Inspection_Form_Factory_Part_II_FactoryDetail>> GetIncpectionFormFactoryPartIIFactoryDetail(Int64 id);
        Task<GenericFormModel<Inspection_Form_Factory_Part_III_InspectionReport>> GetIncpectionFormFactoryPartIIIInspectionReport(Int64 id);
        Task<GenericFormModel<Inspection_Form_Factory_Part_III_DangerousOperation>> GetForm_Factory_Part_III_Dangerousoperation(Int64 id);
        Task<GenericResponseTemplateModel<List<Inspection_Form_Factory_Part_III_MusterRoll>>> GetIncpectionFormFactoryPartIIIMusterRoll(Int64 id);
        Task<GenericFormModel<Inspection_Form_Factory_Part_III_Health>> GetIncpectionFormFactoryPartIIIHealth(Int64 id);
        Task<GenericFormModel<Inspection_Form_Factory_Part_III_Safety>> GetIncpectionFormFactoryPartIIISafety(Int64 id);
        Task<GenericFormModel<Inspection_Form_Factory_Part_III_Welfare>> GetIncpectionGetFormFactoryPartIIIWelfare(Int64 id);
        Task<GenericFormModel<Inspection_Form_Factory_Part_III_General>> GetIncpectionGetFormFactoryPartIIIGeneral(Int64 id);
        Task<GenericFormModel<Inspection_Form_Factory_Part_III_MajorAccidentHazard>> GetIncpectionGetFormFactoryPartIIIMajorAccidentHazard(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIGeneralDetail(Inspection_Form_Factory_Part_I_General formModel);
        Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIFactoryDetail(Inspection_Form_Factory_Part_II_FactoryDetail formModel);
        Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIInspectionReport(Inspection_Form_Factory_Part_III_InspectionReport formModel);
        Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIMusterRoll(Inspection_Form_Factory_Part_III_MusterRoll formModel);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormFactoryPartIIIHealth(Inspection_Form_Factory_Part_III_Health formModel);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormFactoryPartIIISafety(Inspection_Form_Factory_Part_III_Safety formModel);
        Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIWelfare(Inspection_Form_Factory_Part_III_Welfare formModel);
        Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIGeneral(Inspection_Form_Factory_Part_III_General formModel);
        Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIMajorAccidentHazard(Inspection_Form_Factory_Part_III_MajorAccidentHazard formModel);
        Task<GenericServiceResultTemplate> AddUpdateForm_Factory_Part_III_Dangerousoperation(Inspection_Form_Factory_Part_III_DangerousOperation formModel);
        Task<GenericResponseTemplateModel<bool>> Update_InspectionMasterUsingInpectionId(Int64 inspectionId, Boolean hasAssignedLabourCircle, Int64 labourCircleRefId, Int64 labourWingProfileRefId);
        Task<GenericResponseTemplateModel<bool>> Transfer_Inspection_MasterUsingInspectionId(Int64 inspectionId, Boolean hasAssignedLabourCircle, Int64 circleRefId, string senderUserId, string senderRoleId, string senderrRoleName, string receiverUserId, string receiverRoleId, Int64 receiverProfileId, string remarks, int inspectionType);
        //Task<GenericResponseTemplateModel<bool>> Lock_InspectionInfo(Int64 factoryCircleRefId, Int64 randomizationRefId, string userId);
        Task<GenericServiceResultTemplate> Lock_InspectionInfo(Inspection_LockInfo formModel);
        Task<GenericResponseTemplateModel<bool>> RemoveFormFactoryPartIIIMusterRollByMusterId(Int64 id);

        Task<GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>>> Get_Inspections_FactoryPerformaStepStatus(Int64 inspectionRefId);
        Task<GenericFormModel<List<Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel>>> GetInspection_MasterDataByRandomizationIdWithStabblishmentNameAddress(Int64 id, string userId);


        Task<GenericFormModel<Inspection_Form_Labour_Part_I_General>> GetInspectionFormLabourPartIGeneralDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIGeneralDetail(Inspection_Form_Labour_Part_I_General formModel);
        Task<GenericFormModel<Inspection_Form_Labour_Part_II_FactoryDetail>> GetInspectionFormLabourPartIIFactoryDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIFactoryDetail(Inspection_Form_Labour_Part_II_FactoryDetail formModel);
        Task<GenericResponseTemplateModel<List<Inspection_Form_Labour_Part_III_MusterRoll>>> GetInspectionFormLabourPartIIIMusterRoll(Int64 id);
        Task<GenericResponseTemplateModel<bool>> RemoveFormLabourPartIIIMusterRollByMusterId(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIMusterRoll(Inspection_Form_Labour_Part_III_MusterRoll formModel);
        Task<GenericFormModel<Inspection_Form_Labour_Part_III_EqualEnumerationAct>> GetInspectionFormLabourPartIIIEqualEnumerationAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIEqualEnumerationAct(Inspection_Form_Labour_Part_III_EqualEnumerationAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_MinimumWageAct>> GetInspectionFormLabourPartIIIMinimumWagesAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIMinimumWagesAct(Inspection_Form_Labour_III_MinimumWageAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_PaymentWagesAct>> GetInspectionFormLabourPartIIIPaymentWagesAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIPaymentWagesAct(Inspection_Form_Labour_III_PaymentWagesAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>> GetInspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport formModel);
        Task<GenericFormModel<List<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment>>> GetInspectionFormLabourPartIIIPaymentBonusAct_Part_B_Attachment(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIPaymentBonusAct_Part_B_Attachment(Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>> GetInspectionFormLabourPartIIIChildAndAdolescentLabourAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIChildAndAdolescentLabourAct(Inspection_Form_Labour_III_ChildAndAdolescentLabourAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_NationalAndFestivalHolidays>> GetInspectionFormLabourPartIIINationalAndFestivalHolidays(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIINationalAndFestivalHolidays(Inspection_Form_Labour_III_NationalAndFestivalHolidays formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_MaternityBenefitAct>> GetInspectionFormLabourPartIIIMaternityBenefitAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIMaternityBenefitAct(Inspection_Form_Labour_III_MaternityBenefitAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_ContractLabourAct>> GetInspectionFormLabourPartIIIContractLabourAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIContractLabourAct(Inspection_Form_Labour_III_ContractLabourAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>> GetInspectionFormLabourPartIIIInterStateMigrantWorkmenAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIInterStateMigrantWorkmenAct(Inspection_Form_Labour_III_InterStateMigrantWorkmenAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_LabourWelfareFund_Act>> GetInspectionFormLabourPartIIILabourWelfareFund_Act(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIILabourWelfareFund_Act(Inspection_Form_Labour_III_LabourWelfareFund_Act formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_GratuityAct>> GetInspectionFormLabourPartIIIGratuityAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIGratuityAct(Inspection_Form_Labour_III_GratuityAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_IndustrialEmploymentAct>> GetInspectionFormLabourPartIIIIndustrialEmploymentAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIIndustrialEmploymentAct(Inspection_Form_Labour_III_IndustrialEmploymentAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_BOCW_Act>> GetInspectionFormLabourPartIIIBOCW_Act(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIBOCW_Act(Inspection_Form_Labour_III_BOCW_Act formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_ShopAct>> GetInspectionFormLabourPartIIIShopAct(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIShopAct(Inspection_Form_Labour_III_ShopAct formModel);
        Task<GenericFormModel<Inspection_Form_Labour_III_Observations>> GetInspectionFormLabourPartIIIObservations(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIObservations(Inspection_Form_Labour_III_Observations formModel);
        Task<GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>>> Get_Inspections_LabourPerformaStepStatus(Int64 inspectionRefId);
        Task<GenericServiceResultTemplate> Lock_Inspection(Inpection_LockViewModel formModel);
        Task<GenericResponseTemplateModel<InspectionEstablishmentBasicDetailsViewModel>> GetInspectionEstablishmentBasicDetails(string licenceNo);

        Task<GenericResponseTemplateModel<Int64>> GetDistrictRefIdByUserId(string userId);

        Task<GenericResponseTemplateModel<InspectionFactoryDetailsViewModel>> GetInspectionFactoryDetailsByLicenceNo(string licenceNo);
        Task<GenericResponseTemplateModel<List<InspectionTransferInfoViewModel>>> GetLabourCircleOfficersByInspectionRefId(string userRefId, string roleName);
        Task<GenericResponseTemplateModel<GetFactoryCirlceViewModel>> GetFactoryCircleIdByLabourCircleId(Int64 labourCircleRefId);
        Task<GenericResponseTemplateModel<Inpection_LockViewModel>> Get_Inspection_OperationalStatus(Int64 inspectionRefId, string roleName);
        Task<GenericFormModel<List<InspectionLabourCircleViewModel>>> GetInspection_LabourCircleByDistricRefId(int districtRefId);
        Task<GenericServiceResultTemplate> Update_ContactDetails(UpdateAlternateContactDetailsViewModel formModel);
        Task<GenericResponseTemplateModel<AlternateContactDetailsViewModel>> Get_ContactDetails(Int64 appId, bool isLegacy);
        Task<GenericResponseTemplateModel<IntReturn>> VerifyLicenceNumber(string licenceNumber);
        Task<GenericResponseTemplateModel<LicenceNumberDetailsViewModel>> VerifyLicenceNumberWithUser(string licenceNumber, string userRefId);
        Task<GenericResponseTemplateModel<List<InspectionComplianceinfoViewModel>>> Get_inspectionsDataByUserId(string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userRefId, int investpunjab_ipin, string roleName);
        Task<GenericResponseTemplateModel<InspectionSectionWiseKeyValuePair>> GetInspectionViolationData(Int64 inspectionRefId, int inspectionType);
        Task<GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>> GetProcessInspectionDetail(string userId, int currentActionCode, int inspectionType);
        Task<GenericFormModel<List<ProcessApplicationUsersViewModel>>> GetUserByInspectionActionCode(int actionCode, string userId, Int64 inspectionRefId, int inspectionType);
        Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordInspectionAction(InspectionActionViewModel formModel, string userId);
        Task<GenericResponseTemplateModel<List<GetInspectionNotingLogsViewModel>>> GetInspectionLogsByInspectionId(Int64 inspectionRefId, int inspectionType);
        Task<List<AppFileUploadInfoViewModel>> InitiateInspectionFileInfo(Int64 InspectionId, bool deleteTempFiles, int inspectionType);
        Task<List<AppFileUploadInfoViewModel>> InitiateInspectionFileInfoWithRoleId(Int64 inspectionId, bool deleteTempFiles, string userId, int currentActionCode, int allowedActionCode, Int64 inspectionType);
        Task<GenericResponseTemplateModel<FileUploadResponse>> LogFileInfo(Int64 inspectionId, Int64 DocId, Int64 appDocId, string fileName, int inspectionType);

        Task<bool> LockAppDocFiles(Int64 inspectionId, Int64[] appDocIds);
        Task<GenericServiceResultTemplate> Insert_complianceLogs(int inspectionType);
        Task<GenericResponseTemplateModel<string>> SendOTPToUser(string mobileNumber, string userId, string licenceNumber, string newUserName);
        Task<GenericResponseTemplateModel<bool>> VerifyOTP(int otp, string encryptedData, string userId, string licenceNumber);
        Task<GenericResponseTemplateModel<List<InspectionTransferLogsViewModel>>> GetInspectionTransferLogs(Int64 inspectionRefId, int inspectionType);
        Task<GenericFormModel<List<RandomizationInitializationViewModel>>> RadomizationInitialization(int month, int year, string userRefId);
        Task<GenericFormModel<OldInspectionMapping>> GetOldInspectionId(Int64 inspectionRefId);
        Task<GenericResponseTemplateModel<InspectionFactoryAllotmentDetailsViewModel>> GetInspectionFactoryDetailsByLicenceNo_Allotment(string licenceNo);
        Task<GenericServiceResultTemplate> Insert_AllotmentInspections(InspectionAllotmentRequestViewModel requestData);
        Task<GenericResponseTemplateModel<IntReturn>> VerifyCurrentMonthYear(int month ,int year );
    }
}
