using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Licence_Factory_GeneralDetail
    {
        [Key]
        public Int64 FactoryLicenceId { get; set; }

        //#region Competent Person Details
        //[Required(ErrorMessage = "Competent Person name is required..!")]
        //public string CompetentPersonName { get; set; }

        //[Required(ErrorMessage = "Competent Person email is required..!")]
        //public string CompetentPersonEmail { get; set; }

        //[Required(ErrorMessage = "Competent Person Mobile is required..!")]
        //public string CompetentPersonMobile { get; set; }
        //#endregion

        [Required(ErrorMessage = "Is Building Constructed Before 29 June 2018 is required..!")]
        public int IsBuildingConstructedBefore29June2018 { get; set; }

        [Required(ErrorMessage = "Have You Made Changes In Building Plan is required..!")]
        public int HaveYouMadeChangesInBuildingPlan { get; set; }

        #region Factory Basic Details

        [Required(ErrorMessage = "Old Licence No is required..!")]
        public string OldLicenceNo { get; set; }

        //[Required(ErrorMessage = "Old Licence Valid UpTo is required..!")]
        public DateTime? OldLicenceValidUpTo { get; set; }

        public int? OldLicenceTotalEmployees { get; set; }

        public decimal? OldLicenceFactoryKiloWatt { get; set; }

        //[Required(ErrorMessage = "Registration Date is required..!")]
        public DateTime? RegistrationDate { get; set; }

        //[Required(ErrorMessage = "Renewal From Date is required..!")]
        public DateTime? RenewalFromDate { get; set; }
        public DateTime? AmmendmentDate { get; set; }

        [Required(ErrorMessage = "No Of Years is required..!")]
        public int NoOfYears { get; set; }

        [Required(ErrorMessage = "Manufacturing Process Last 12 Months is Required..!"), StringLength(200, ErrorMessage = "The max length of Manufacturing Process is 200 characters..!")]
        public string ManufacturingProcess_Last12Months { get; set; }

        [Required(ErrorMessage = "Manufacturing Process Next 12 Months is Required..!"), StringLength(200, ErrorMessage = "The max length of Manufacturing Process is 200 characters..!")]
        public string ManufacturingProcess_Next12Months { get; set; }

        [Required(ErrorMessage = "National Industrial Classification Code is required..!")]
        public string NationalIndustrialClassificationCode { get; set; }

        [Required(ErrorMessage = "Mfg Products Last 12 Month is required..!")]
        public string MfgProducts_Last12Month { get; set; }

        [Required(ErrorMessage = "Workers Max During Year is required..!")]
        public Int64 Workers_MaxDuringYear { get; set; }

        [Required(ErrorMessage = "Workers Max Last 12 Month is required..!")]
        public Int64 Workers_MaxLast12Month { get; set; }

        [Required(ErrorMessage = "Workers Ordinarily Employed is required..!")]
        public Int64 Workers_OrdinarilyEmployed { get; set; }

        [Required(ErrorMessage = "Power KW Installed is required..!")]
        public decimal PowerKW_Installed { get; set; }

        [Required(ErrorMessage = "Power KW Max Proposed is required..!")]
        public decimal PowerKW_MaxProposed { get; set; }
        public int ModifiedCounter { get; set; } = 1;
        #endregion

        #region Questionnaire

        [Required(ErrorMessage = "Is Under Right To Business Act is required..!")]
        public int IsUnderRightToBusinessAct { get; set; }

        [Required(ErrorMessage = "Date Of Principal Approval is required..!")]
        public string DateOfPrincipalApproval { get; set; }

        [Required(ErrorMessage = "Project Identification No is required..!")]
        public string ProjectIdentificationNo { get; set; }

        [Required(ErrorMessage = "AppId Right To Business Act is required..!")]
        public string AppIdRightToBusinessAct { get; set; }

        [Required(ErrorMessage = "Is RBA Verified is required..!")]
        public bool IsRBAVerified { get; set; }

        [Required(ErrorMessage = "Is Temp Registered is required..!")]
        public int IsTempRegistered { get; set; }

        [Required(ErrorMessage = "Temp Registration Number is required..!")]
        public string TempRegistrationNumber { get; set; }

        [Required(ErrorMessage = "Is Temp Registration Verified is required..!")]
        public bool IsTempRegistrationVerified { get; set; }

        [Required(ErrorMessage = "Is Building Plan Approved is required..!")]
        public int IsBuildingPlanApproved { get; set; }

        [Required(ErrorMessage = "Building Plan Dof Number is required..!")]
        public string BuildingPlanDofNumber { get; set; }

        [Required(ErrorMessage = "Is Building Plan Verified is required..!")]
        public bool IsBuildingPlanVerified { get; set; }

        [Required(ErrorMessage = "Is Stability Approved is required..!")]
        public BuildingPlanStabilityAuthorityTypeEnum IsStabilityApproved { get; set; }


        [Required(ErrorMessage = "Stability Dof Number is required..!")]
        public string StabilityPlanDofNumber { get; set; }

        [Required(ErrorMessage = "Is Stability Verified is required..!")]
        public bool IsStabilityPlanVerified { get; set; }
        public string CompetentPersonUserId { get; set; }

        [Required(ErrorMessage = "Competent Person name is required..!")]
        public string CompetentPersonName { get; set; }

        [Required(ErrorMessage = "Competent Person contact no is required..!")]
        public string CompetentPersonContactNo { get; set; }

        [Required(ErrorMessage = "Competent Person email is required..!")]
        public string CompetentPersonEmail { get; set; }

        [Required(ErrorMessage = "Engineer name is required..!")]
        public string EngineerName { get; set; }

        [Required(ErrorMessage = "Engineer contact no is required..!")]
        public string EngineerContactNo { get; set; }

        [Required(ErrorMessage = "Engineer email is required..!")]
        public string EngineerEmail { get; set; }

        [Required(ErrorMessage = "Building Plan Approval Date is required..!")]
        public DateTime BuildingPlanApprovalDate { get; set; }

        [Required(ErrorMessage = "Building Plan Stabaility Approval Date is required..!")]
        public DateTime BuildingPlanStabilityApprovalDate { get; set; }

        [NotMapped]
        public List<OfficerDetailsByRoleNameViewModel> CompetentPersonList { get; set; }

        [NotMapped]
        public List<OfficerDetailsByRoleNameViewModel> EmpaneledEngineersList { get; set; }

        #endregion

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        public virtual Licence_Factory_OccupierAndManagerDetail Licence_Factory_OccupierAndManagerDetail { get; set; }
        #endregion

        #region Not Mapped Column

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [NotMapped]
        public Int64 IPin { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }


        [NotMapped]
        public DateComponentViewModel RenewalFromDate_Json { get; set; }

        [NotMapped]
        public DateComponentViewModel RegistrationDate_Json { get; set; }

        [NotMapped]
        public DateComponentViewModel OldLicenceValidUpTo_Json { get; set; }

        [NotMapped]
        public DateComponentViewModel AmmendmentDate_Json { get; set; }

        [NotMapped]
        public Int64 FactoryCircleRefId { get; set; }

        [NotMapped]
        public Int64 DistrictRefId { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }

        #endregion
    }

    public class Licence_Factory_OccupierAndManagerDetail
    {
        [Key]
        public Int64 OccupierAndManagerDetailId { get; set; }

        #region Manager Details
        [Required(ErrorMessage = "Manager Full Name is required..!")]
        public string ManagerFullName { get; set; }

        [Required(ErrorMessage = "Manager Father Name is required..!")]
        public string ManagerFatherName { get; set; }

        [Required(ErrorMessage = "Manager Full Address is required..!")]
        public string ManagerFullAddress { get; set; }

        [Required(ErrorMessage = "Manager Mobile is required..!")]
        public string ManagerMobile { get; set; }

        [Required(ErrorMessage = "Manager email is required..!")]
        public string ManagerEmail { get; set; }

        [Required(ErrorMessage = "Manager Residential Address is required..!")]
        public string ManagerResidentialAddress { get; set; }
        #endregion

        #region Occupier Details
        [Required(ErrorMessage = "Occupier Full Name is required..!")]
        public string OccupierFullName { get; set; }

        [Required(ErrorMessage = "Occupier father name is required..!")]
        public string OccupierFatherName { get; set; }

        [Required(ErrorMessage = "Occupier Full Address is required..!")]
        public string OccupierFullAddress { get; set; }

        [Required(ErrorMessage = "Occupier Mobile is required..!")]
        public string OccupierMobile { get; set; }

        [Required(ErrorMessage = "Occupier Email is required..!")]
        public string OccupierEmail { get; set; }

        [Required(ErrorMessage = "Occupier Residential Address is required..!")]
        public string OccupierResidentialAddress { get; set; }
        #endregion

        #region Owner Details
        [Required(ErrorMessage = "Owner name is required..!")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Owner Premises Address is required..!")]
        public string OwnerPremisesAddress { get; set; }
        #endregion

        #region Stability Details

        [Required(ErrorMessage = "Stability Certificate Number is required..!")]
        public string StabilityCertificateNumber { get; set; }

        [Required(ErrorMessage = "Stability Certificate Date is required..!")]
        public DateTime StabilityCertificateDate { get; set; }

        [Required(ErrorMessage = "Stability DOF Number is required..!")]
        public string StabilityDOFNumber { get; set; }

        #endregion

        #region CheckList
        [Required(ErrorMessage = "Checklist Is Building Plan Approved is required..!")]
        public string Checklist_IsBuildingPlanApproved { get; set; }

        [Required(ErrorMessage = "Checklist Is Labour Welfare Fund Paid is required..!")]
        public string Checklist_IsLabourWelfareFundPaid { get; set; }

        [Required(ErrorMessage = "Checklist Is Annual Return Filed is required..!")]
        public string Checklist_IsAnnualReturnFiled { get; set; }

        [Required(ErrorMessage = "Checklist Is Stability Certificate Attached is required..!")]
        public string Checklist_IsStabilityCertificateAttached { get; set; }
        public int ModifiedCounter { get; set; } = 1;
        #endregion

        #region Foreign Key References
        [Required(ErrorMessage = "FactoryLicenceRefId is required..!")]
        [ForeignKey("Licence_Factory_GeneralDetail")]
        public Int64 FactoryLicenceRefId { get; set; }
        public virtual Licence_Factory_GeneralDetail Licence_Factory_GeneralDetail { get; set; }

        [NotMapped]
        public Int64 AppRefId { get; set; }
        #endregion
    }

    public class Licence_Factory_AmendmentDataHistory
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "SectionCode is required..!")]
        [StringLength(10, ErrorMessage = "SectionCode max size is 10")]
        public string SectionCode { get; set; }

        [Required(ErrorMessage = "FieldName is required..!")]
        [StringLength(50, ErrorMessage = "FieldName max size is 50")]
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string ModifiedValue { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedCounter { get; set; }
        public bool IsLocked { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }
    }

    public class Licence_Factory_QuestionnaireDetail
    {
        [Key]
        public Int64 QuestionnaireDetailId { get; set; }

        [Required(ErrorMessage = "Is Under Right To Business Act is required..!")]
        public int IsUnderRightToBusinessAct { get; set; }

        [Required(ErrorMessage = "Date Of Principal Approval is required..!")]
        public string DateOfPrincipalApproval { get; set; }

        [Required(ErrorMessage = "Project Identification No is required..!")]
        public string ProjectIdentificationNo { get; set; }

        [Required(ErrorMessage = "AppId Right To Business Act is required..!")]
        public string AppIdRightToBusinessAct { get; set; }

        [Required(ErrorMessage = "Is RBA Verified is required..!")]
        public bool IsRBAVerified { get; set; }

        [Required(ErrorMessage = "Is Temp Registered is required..!")]
        public int IsTempRegistered { get; set; }

        [Required(ErrorMessage = "Temp Registration Number is required..!")]
        public string TempRegistrationNumber { get; set; }

        [Required(ErrorMessage = "Is Temp Registration Verified is required..!")]
        public bool IsTempRegistrationVerified { get; set; }

        [Required(ErrorMessage = "Is Building Plan Approved is required..!")]
        public int IsBuildingPlanApproved { get; set; }

        [Required(ErrorMessage = "Building Plan Dof Number is required..!")]
        public string BuildingPlanDofNumber { get; set; }

        [Required(ErrorMessage = "Is Building Plan Verified is required..!")]
        public bool IsBuildingPlanVerified { get; set; }

        [Required(ErrorMessage = "Is Stability Approved is required..!")]
        public int IsStabilityApproved { get; set; }

        [Required(ErrorMessage = "Stability Dof Number is required..!")]
        public string StabilityPlanDofNumber { get; set; }

        [Required(ErrorMessage = "Is Stability Verified is required..!")]
        public bool IsStabilityPlanVerified { get; set; }

        public string CompetentPersonUserId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public Int64 IPin { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
    }

    public class Licence_Factory_AdditionalDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        public FactoryHazardousCategoryTypeEnum FactoryHazardousCategoryType { get; set; }

        [Required]
        public FactorySectionCategoryTypeEnum FactorySectionCategoryType { get; set; }

        [Required]
        public FactorySessionCategoryTypeEnum FactorySessionCategoryType { get; set; }

        [Required]
        public FactoryCategoryTypeEnum FactoryCategoryType { get; set; }
        public Int64 LabourCircleRefId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

    }

}