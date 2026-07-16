using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Inspection_Randomization
    {
        [Key]
        public Int64 RandomizationId { get; set; }
        
        [Required(ErrorMessage ="Month is required..!")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required..!")]
        public int Year { get; set; }

        [Required(ErrorMessage = "ProcessInitiatedByType is required..!")]
        public InspectionRandomizationProcessInitiatedByTypeEnum ProcessInitiatedByType { get; set; }

        [Required(ErrorMessage ="Process initiated by user id..!")]
        [StringLength(60)]
        public string UserRefId { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Timestemp is required..!")]
        public DateTime Timestemp { get; set; }
        public virtual ICollection<Inspection_Master> Inspection_Masters { get; set; }
    }

    public class Inspection_Master
    {
        [Key]
        public Int64 InspectionId { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "LicenceNo can have max 50 chars..!")]
        [Required(ErrorMessage = "LicenceNo is required..!")]
        public string LicenceNumber { get; set; }

        [Required(ErrorMessage = "AppId is required..!")]
        public Int64 AppId { get; set; }

        [Required(ErrorMessage = "FactoryCircleId is required..!")]
        public Int64 FactoryCircleId { get; set; }

        [Required(ErrorMessage = "UserId is required..!")]
        [StringLength(maximumLength: 60, ErrorMessage = "UserId can have max 60 chars..!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "UserProfileId is required..!")]
        public Int64 UserProfileId { get; set; }

        [Required(ErrorMessage = "ALCCircleRefId is required..!")]
        public Int64 ALCCircleRefId { get; set; }

        [Required(ErrorMessage = "ALCUserRefId is required..!")]
        [StringLength(maximumLength: 60, ErrorMessage = "ALCUserRefId can have max 60 chars..!")]
        public string ALCUserRefId { get; set; }

        [Required(ErrorMessage = "ALCProfileRefId is required..!")]
        public Int64 ALCProfileRefId { get; set; }

        [Required(ErrorMessage = "FactoryHazardousCategoryType is required..!")]
        public FactoryHazardousCategoryTypeEnum FactoryHazardousCategoryType { get; set; }

        [Required(ErrorMessage = "FactorySessionCategoryType is required..!")]
        public FactorySessionCategoryTypeEnum FactorySessionCategoryType { get; set; }
        
        [Required(ErrorMessage = "FactorySectionCategoryType is required..!")]
        public FactorySectionCategoryTypeEnum FactorySectionCategoryType { get; set; }

        [Required(ErrorMessage = "IsLegacy is required..!")]
        public bool IsLegacy { get; set; }
        
        [Required(ErrorMessage = "ApplicationType is required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        //[Required(ErrorMessage = "InspectionStatusType is required..!")]
        //public InspectionStatusTypeEnum InspectionStatusType { get; set; }

        [Required(ErrorMessage = "HasAssignedLabourCircle is required..!")]
        public bool HasAssignedLabourCircle { get; set; }

        [Required(ErrorMessage = "LabourCircleRefId is required..!")]
        public Int64 LabourCircleRefId { get; set; }

        [Required(ErrorMessage = "LabourWingProfileRefId is required..!")]
        public Int64? LabourWingProfileRefId { get; set; }

        #region Factory Wing
        public bool IsSubmitted_Factory_Wing { get; set; }
        public DateTime? InspectionDoneOn_Factory_Wing { get; set; }
        public DateTime? InspectionSubmitOn_Factory_Wing { get; set; }

        [Required(ErrorMessage = "InspectionSubmited_Factory_Wing_UserId is required..!")]
        public string InspectionSubmited_Factory_Wing_UserId { get; set; }

        [Required(ErrorMessage = "InspectionSubmited_Factory_Wing_ProfileId is required..!")]
        public Int64 InspectionSubmited_Factory_Wing_ProfileId { get; set; }

        [Required(ErrorMessage = "InspectionSubmited_Factory_Wing_RoleId is required..!")]
        public string InspectionSubmited_Factory_Wing_RoleId { get; set; }

        [Required(ErrorMessage = "InspectionSubmited_Factory_Wing_IsOperationalClosed is required..!")]
        public InspectionFactoryExistenceTypeEnum InspectionSubmited_Factory_Wing_FactoryExistenceType { get; set; }
        [Required(ErrorMessage = "InspectionEstablishmentType is required..!")]
        public InspectionEstablishmentTypeEnum InspectionEstablishmentType { get; set; }

        [Required(ErrorMessage = "FactoryDeRegistrationNo is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "FactoryDeRegistrationNo can have max 50 chars..!")]
        public string FactoryDeRegistrationNo { get; set; }

        [Required(ErrorMessage = "FactoryDeRegistrationNo is required..!")]
        public string Remarks { get; set; }

        #endregion

        #region Labour Wing

        public bool IsSubmitted_Labour_Wing { get; set; }
        public DateTime? InspectionDoneOn_Labour_Wing { get; set; }
        public DateTime? InspectionSubmitOn_Labour_Wing { get; set; }

        [Required(ErrorMessage = "InspectionSubmited_Labour_Wing_UserId is required..!")]
        public string InspectionSubmited_Labour_Wing_UserId { get; set; }

        [Required(ErrorMessage = "InspectionSubmited_Labour_Wing_ProfileId is required..!")]
        public Int64 InspectionSubmited_Labour_Wing_ProfileId { get; set; }

        [Required(ErrorMessage = "InspectionSubmited_Labour_Wing_RoleId is required..!")]
        public string InspectionSubmited_Labour_Wing_RoleId { get; set; }

        [Required(ErrorMessage = "InspectionSubmited_Labour_Wing_IsOperationalClosed is required..!")]
        public InspectionFactoryExistenceTypeEnum InspectionSubmited_Labour_Wing_FactoryExistenceType { get; set; }

        #endregion


        [Required(ErrorMessage = "AdfDdfNameOfLegacyRecord is required..!")]
        [StringLength(maximumLength: 60, ErrorMessage = "AdfDdfNameOfLegacyRecord can have max 60 chars..!")]
        public string AdfDdfNameOfLegacyRecord { get; set; }

        public Int64 DistrictRefId { get; set; }

        [Required(ErrorMessage = "RandomizationRefId is required..!")]
        [ForeignKey("Inspection_Randomization")]
        public Int64 RandomizationRefId { get; set; }

        public virtual Inspection_Randomization Inspection_Randomization { get; set; }
        public virtual Inspection_Form_Factory_Part_I_General Inspection_Form_Factory_Part_I_General { get; set; }
        public virtual Inspection_Form_Factory_Part_II_FactoryDetail Inspection_Form_Factory_Part_II_FactoryDetail { get; set; }
        public virtual Inspection_Form_Factory_Part_III_InspectionReport Inspection_Form_Factory_Part_III_InspectionReport { get; set; }
        public virtual ICollection<Inspection_Form_Factory_Part_III_MusterRoll> Inspection_Form_Factory_Part_III_MusterRolls { get; set; }
        public virtual Inspection_Form_Factory_Part_III_Health Inspection_Form_Factory_Part_III_Health { get; set; }
        public virtual Inspection_Form_Factory_Part_III_Safety Inspection_Form_Factory_Part_III_Safety { get; set; }
        public virtual Inspection_Form_Factory_Part_III_Welfare Inspection_Form_Factory_Part_III_Welfare { get; set; }
        public virtual Inspection_Form_Factory_Part_III_General Inspection_Form_Factory_Part_III_General { get; set; }
        public virtual Inspection_Form_Factory_Part_III_MajorAccidentHazard Inspection_Form_Factory_Part_III_MajorAccidentHazard { get; set; }
        public virtual ICollection<Inspection_LockInfo> Inspection_LockInfo { get; set; }
        public virtual Inspection_Form_Factory_Part_III_DangerousOperation Inspection_Form_Factory_Part_III_DangerousOperation { get; set; }
        public virtual ICollection<InspectionComplianceLog> InspectionComplianceLog { get; set; }
        public virtual ICollection<Inspection_ViolationComplianceReminder> Inspection_ViolationComplianceReminders { get; set; }

        #region Labour Inspection Foreign Key
        public virtual Inspection_Form_Labour_Part_I_General Inspection_Form_Labour_Part_I_General { get; set; }
        public virtual Inspection_Form_Labour_Part_II_FactoryDetail Inspection_Form_Labour_Part_II_FactoryDetail { get; set; }
        public virtual ICollection<Inspection_Form_Labour_Part_III_MusterRoll> Inspection_Form_Labour_Part_III_MusterRoll { get; set; }
        public virtual Inspection_Form_Labour_Part_III_EqualEnumerationAct Inspection_Form_Labour_Part_III_EqualEnumerationAct { get; set; }
        public virtual Inspection_Form_Labour_III_MinimumWageAct Inspection_Form_Labour_III_MinimumWageAct { get; set; }
        public virtual Inspection_Form_Labour_III_PaymentWagesAct Inspection_Form_Labour_III_PaymentWagesAct { get; set; }
        public virtual Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport { get; set; }
        public virtual ICollection<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment> Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment { get; set; }
        public virtual Inspection_Form_Labour_III_ChildAndAdolescentLabourAct Inspection_Form_Labour_III_ChildAndAdolescentLabourAct { get; set; }
        public virtual Inspection_Form_Labour_III_NationalAndFestivalHolidays Inspection_Form_Labour_III_NationalAndFestivalHolidays { get; set; }
        public virtual Inspection_Form_Labour_III_MaternityBenefitAct Inspection_Form_Labour_III_MaternityBenefitAct { get; set; }
        public virtual Inspection_Form_Labour_III_ContractLabourAct Inspection_Form_Labour_III_ContractLabourAct { get; set; }
        public virtual Inspection_Form_Labour_III_InterStateMigrantWorkmenAct Inspection_Form_Labour_III_InterStateMigrantWorkmenAct { get; set; }
        public virtual Inspection_Form_Labour_III_LabourWelfareFund_Act Inspection_Form_Labour_III_LabourWelfareFund_Act { get; set; }
        public virtual Inspection_Form_Labour_III_GratuityAct Inspection_Form_Labour_III_GratuityAct { get; set; }
        public virtual Inspection_Form_Labour_III_IndustrialEmploymentAct Inspection_Form_Labour_III_IndustrialEmploymentAct { get; set; }
        public virtual Inspection_Form_Labour_III_BOCW_Act Inspection_Form_Labour_III_BOCW_Act { get; set; }
        public virtual Inspection_Form_Labour_III_ShopAct Inspection_Form_Labour_III_ShopAct { get; set; }
        public virtual Inspection_Form_Labour_III_Observations Inspection_Form_Labour_III_Observations { get; set; }

        #endregion
    }

    public class Inspection_LockInfo
    {
        [Key]
        public Int64 LockId { get; set; }

        [Required(ErrorMessage = "FactoryCircleRefId is required..!")]
        public Int64 FactoryCircleRefId { get; set; }

        [Required(ErrorMessage = "IsLocked is required..!")]
        public bool IsLocked { get; set; }

        [Required(ErrorMessage = "LockedOn is required..!")]
        public DateTime LockedOn { get; set; }

        [Required(ErrorMessage = "LockedBy_UserId is required..!")]
        public string LockedBy_UserId { get; set; }

        [Required(ErrorMessage = "LockedBy_ProfileId is required..!")]
        public Int64 LockedBy_ProfileId { get; set; }

        [Required(ErrorMessage = "LockedBy_RoleId is required..!")]
        public string LockedBy_RoleId { get; set; }

        //[Required(ErrorMessage = "IsOperationalClosed is required..!")]
        //public bool IsOperationalClosed { get; set; }

        //[Required(ErrorMessage = "InspectionEstablishmentType is required..!")]
        //public InspectionEstablishmentTypeEnum InspectionEstablishmentType { get; set; }

        //[Required(ErrorMessage = "InspectionEstablishmentType is required..!")]
        //public InspectionFactoryExistenceTypeEnum InspectionFactoryExistenceType { get; set; }

        //[Required(ErrorMessage = "FactoryDeRegistrationNo is required..!")]
        //[StringLength(maximumLength: 50, ErrorMessage = "FactoryDeRegistrationNo can have max 50 chars..!")]
        //public string FactoryDeRegistrationNo { get; set; }

        //[Required(ErrorMessage = "Remarks is required..!")]
        //public string Remarks { get; set; }

        [Required(ErrorMessage = "RandomizationRefId is required..!")]
        [ForeignKey("Inspection_Randomization")]
        public Int64  RandomizationRefId { get; set; }
        public virtual Inspection_Randomization Inspection_Randomization { get; set; }
    }

    public class Inspection_Form_Factory_Part_I_General
    {
        [Key]

        [Display(Name = "id")]
        public Int64 Id { get; set; }

        [Display(Name = "1.1 Factory Name")]
        [Required(ErrorMessage = "FactoryName is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "FactoryName can have max 100 chars..!")]
        public string FactoryName { get; set; }

        [Display(Name = "1.2 Factory Address")]
        [Required(ErrorMessage = "FactoryAddress is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "FactoryAddress can have max 500 chars..!")]
        public string FactoryAddress { get; set; }

       [Display(Name = "1.3 Date Of Inspection")]
        [Required(ErrorMessage = "DateOfInspection is required..!")]
        public DateTime DateOfInspection { get; set; }

       [Display(Name = "1.4 Date Of Last Inspection")]
        //[Required(ErrorMessage = "InspectionFacrotyExistenceType is required..!")]
        //public InspectionFactoryExistenceTypeEnum InspectionFacrotyExistenceType { get; set; }
        public DateTime DateOfLastInspection { get; set; }

        [Display(Name = "1.5 Occupier Name")]
        [Required(ErrorMessage = "OccupierName is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "OccupierName can have max 100 chars..!")]
        public string OccupierName { get; set; }

        [Display(Name = "1.6 Occupier Address")]
        [Required(ErrorMessage = "OccupierAddress is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "OccupierAddress can have max 500 chars..!")]
        public string OccupierAddress { get; set; }

        [Display(Name = "1.7 Manager Name")]
        [Required(ErrorMessage = "ManagerName is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "ManagerName can have max 100 chars..!")]
        public string ManagerName { get; set; }

        [Display(Name = "1.8 Manager Address")]
        [Required(ErrorMessage = "ManagerAddress is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "ManagerAddress can have max 500 chars..!")]
        public string ManagerAddress { get; set; }

        [Display(Name = "1.9 Present Person Name")]
        [Required(ErrorMessage = "PresentPersonName is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "PresentPersonName can have max 100 chars..!")]
        public string PresentPersonName { get; set; }

        [Display(Name = "1.10 Present Person Address")]
        [Required(ErrorMessage = "PresentPersonAddress is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "PresentPersonAddress can have max 500 chars..!")]
        public string PresentPersonAddress { get; set; }

        [Display(Name = "InspectionId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_II_FactoryDetail
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "2.1 License number under Factories Act 1948")]
        [Required(ErrorMessage = "License number under Factories Act 1948 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "License number under Factories Act 1948 can have max 50 chars..!")]
        public string License_Factory_Act { get; set; }

        [Display(Name = "2.2 Registration number of Principal Employer under The Contract Labour Act 1970")]
        [Required(ErrorMessage = "Registration number of Principal Employer under The Contract Labour (R &amp;A) Act 1970 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "Registration number of Principal Employer under The Contract Labour (R &amp;A) Act 1970 can have max 50 chars..!")]
        public string Registration_PE_Act { get; set; }

        [Display(Name = "2.3 License numbers of contractor(s) under The Contract Labour (R&A) Act 1970")]
        [Required(ErrorMessage = "License numbers of contractor(s) under The Contract Labour (R &amp;A) Act 1970 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "License numbers of contractor(s) under The Contract Labour (R &amp;A) Act 1970 can have max 50 chars..!")]
        public string License_CL_Act { get; set; }


        [Display(Name = "2.4 Registration number of factory under The Inter-State Migrant Workmen(RoECS) Act 1979")]
        [Required(ErrorMessage = "Registration number of factory under The Inter -State Migrant Workmen(RoECS) Act 1979 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "Registration number of factory under The Inter -State Migrant Workmen(RoECS) Act 1979 can have max 50 chars..!")]
        public string Registration_ISMW_Act { get; set; }

        [Display(Name = "2.5 License number of contractor under The Inter-State Migrant Workmen (RoECS) Act 1979")]
        [Required(ErrorMessage = "License number of contractor under The Inter -State Migrant Workmen(RoECS) Act 1979 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "License number of contractor under The Inter -State Migrant Workmen(RoECS) Act 1979 can have max 50 chars..!")]
        public string License_ISMW_Act { get; set; }

        [Display(Name = "2.6 Has the license fee for the current year been deposited? ")]
        [Required(ErrorMessage = "Has the license fee for the current year been deposited is required..!")]
        public GeneralOptionTypeEnum IsFactoryFeeDeposited { get; set; }

        [Display(Name = "Inspection RefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_III_InspectionReport
    {
        [Key]
        [Display(Name = "InspectionId")]
        public Int64 Id { get; set; }

        [Display(Name = "3.1 Section applicable")]
        [Required(ErrorMessage = "Section applicable is required..!")]
        public FactorySectionCategoryTypeEnum FactorySectionCategoryType { get; set; }

        [Display(Name = "3.2 Manufacturing Process/Product")]
        [Required(ErrorMessage = "Manufacturing process (As per factory licence approval) is required..!")]
        public string Mfg_Process_Reported { get; set; }

        [Display(Name = "3.3 Manufacturing process (On the time of inspection)")]
        [Required(ErrorMessage = "Manufacturing process (On the time of inspection) is required..!")]
        public string Mfg_Process_Inspected { get; set; }

        [Display(Name = "3.4 Bye-product/Intermediates/chemical details")]
        [Required(ErrorMessage = "Bye-product/Intermidiates/Chemical detail is required..!")]
        public string ByeProduct_Intermidiates_ChemicalDetail { get; set; }

        [Display(Name = "3.5 Building Plans: (A) Whether PUPD Rules 2021 applicable or not?")]
        [Required(ErrorMessage = "BP_PUPDRules_IsApplicable is required..!")]
        public GeneralOptionTypeEnum BP_PUPDRules_IsApplicable { get; set; }

        [Display(Name = "3.6 Building Plans: Status of approval of plans under these Rules")]
        [Required(ErrorMessage = "BP_PUPDRules_IsApplicable is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "BP_PUPDRules_Status can have max 500 chars..!")]
        public string BP_PUPDRules_Status { get; set; }

        [Display(Name = "3.7 Building Plans: (B) Whether building plans under Factories Act1948 / Punjab Factory Rules 1952 accepted(yes / No)")]
        [Required(ErrorMessage = "BP_FactoriesAct_IsAccepted is required..!")]
        public GeneralOptionTypeEnum BP_FactoriesAct_IsAccepted { get; set; }

        [Display(Name = "3.7.1 (A) If yes, Give the reference number")]
        [Required(ErrorMessage = "BP_FactoriesAct_ApprovalReferenceNum is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "BP_FactoriesAct_ApprovalReferenceNum can have max 100 chars..!")]
        public string BP_FactoriesAct_ApprovalReferenceNum { get; set; }

        [Display(Name = "3.8.1 (A) If yes, Give the date of approval")]
        //[Required(ErrorMessage = "BP_FactoriesAct_ApprovalDate is required..!")]
        public DateTime? BP_FactoriesAct_ApprovalDate { get; set; }

        [Display(Name = "3.8 (B) Are the buildings constructed and plant/Machinery installed according to the plans approved ? ")]
        [Required(ErrorMessage = "BP_Constructed_IsAsPerApprovedPlans is required..!")]
        public GeneralOptionTypeEnum BP_Constructed_IsAsPerApprovedPlans { get; set; }

        [Display(Name = "3.9 Specific note on changes noticed during inspection")]
        [Required(ErrorMessage = "BP_Constructed_ChangesAsPerInspected is required..!")]
        public string BP_Constructed_ChangesAsPerInspected { get; set; }


        [Display(Name = "3.10 Stability Certificate Is accepted")]
        [Required(ErrorMessage = "BP_Stability_IsAccepted is required..!")]
        public GeneralOptionTypeEnum BP_Stability_IsAccepted { get; set; }

        [Display(Name = "3.10.1 Stability Certificate reference of acceptance")]
        [Required(ErrorMessage = "BP_Stability_AcceptanceReferenceNum is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "BP_Stability_AcceptanceReferenceNum can have max 100 chars..!")]
        public string BP_Stability_AcceptanceReferenceNum { get; set; }

        [Display(Name = "3.10.2 Stability Certificate reference of acceptance date")]
        //[Required(ErrorMessage = "BP_FactoriesAct_AcceptanceDate is required..!")]
        public DateTime? BP_FactoriesAct_AcceptanceDate { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_III_MusterRoll
    {
        [Key]
        [Display(Name = "InspectionRefId")]
        public Int64 Id { get; set; }

        [Display(Name = "ShiftTime_From")]
        [Required(ErrorMessage = "ShiftTime_From is required..!")]
        [StringLength(maximumLength: 20, ErrorMessage = "ShiftTime_From can have max 20 chars..!")]
        public string ShiftTime_From { get; set; }

        [Display(Name = "ShiftTime_To")]
        [Required(ErrorMessage = "ShiftTime_To is required..!")]
        [StringLength(maximumLength: 20, ErrorMessage = "ShiftTime_To can have max 20 chars..!")]
        public string ShiftTime_To { get; set; }

        [Display(Name = "Count Adult Male")]
        [Required(ErrorMessage = "Count_Adult_Male is required..!")]
        public int Count_Adult_Male { get; set; }

        [Display(Name = "Count Adult FeMale")]
        [Required(ErrorMessage = "Count_Adult_FeMale is required..!")]
        public int Count_Adult_FeMale { get; set; }


        [Display(Name = "Count Adolescent Male")]
        [Required(ErrorMessage = "Count_Adolescent_Male is required..!")]
        public int Count_Adolescent_Male { get; set; }

        [Display(Name = "Count Adolescent FeMale")]
        [Required(ErrorMessage = "Count_Adolescent_FeMale is required..!")]
        public int Count_Adolescent_FeMale { get; set; }

        [Display(Name = "Count Children Male")]
        [Required(ErrorMessage = "Count_Children_Male is required..!")]
        public int Count_Children_Male { get; set; }

        [Display(Name = "Count Children FeMale")]
        [Required(ErrorMessage = "Count_Children_FeMale is required..!")]
        public int Count_Children_FeMale { get; set; }

        [Display(Name = "Inspection MusterRoll Type")]
        [Required(ErrorMessage = "InspectionMusterRollType is required..!")]
        public InspectionMusterRollTypeEnum InspectionMusterRollType { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_III_Health
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "Section-11: 11.1.1 Cleanliness Is accumulation of dirt and refuse removed daily by sweeping or  by any other effective method from the floors and benches of  workrooms and from staircases and passages, and disposed of in a suitable manner ? ")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_SuitableManner_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_11_Cleanliness_SuitableManner_Selection { get; set; }

        [Display(Name = "11.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_SuitableManner_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_11_Cleanliness_SuitableManner_ViolationExist { get; set; }

        [Display(Name = "11.1.3 Sec_11_Cleanliness_SuitableManner_Remarks")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_SuitableManner_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_Cleanliness_SuitableManner_Remarks can have max 1000 chars..!")]
        public string Sec_11_Cleanliness_SuitableManner_Remarks { get; set; }

        [Display(Name = "Section-11: 11.2.1 Is the floor of every workroom being cleaned at least once in every week by washing, using disinfectant, where necessary, or by some other effective method?")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_Method_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_11_Cleanliness_Method_Selection { get; set; }

        [Display(Name = "11.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_Method_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_11_Cleanliness_Method_ViolationExist { get; set; }

        [Display(Name = "11.2.3 Sec_11_Cleanliness_Method_Remarks")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_Method_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_Cleanliness_Method_Remarks can have max 1000 chars..!")]
        public string Sec_11_Cleanliness_Method_Remarks { get; set; }

        [Display(Name = "11.3.1 Are effective means of drainage being provided and maintained where a floor is liable to become wet in the course of any manufacturing process to such extent as is capable of being drained, effective means of drainage shall be provided andmaintained ? ")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_Drainage_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_11_Cleanliness_Drainage_Selection { get; set; }

        [Display(Name = "11.3.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_11_Cleanliness_Drainage_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_11_Cleanliness_Drainage_ViolationExist { get; set; }

        [Display(Name = "11.3.3 Sec_11_Cleanliness_Drainage_Remarks")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_Drainage_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_Cleanliness_Drainage_Remarks can have max 1000 chars..!")]
        public string Sec_11_Cleanliness_Drainage_Remarks { get; set; }

        [Display(Name = "11.4.1 Are all inside walls and partitions, all ceilings or tops of rooms and all walls, sides and tops of passages, staircases, latrines, canteen etc.lime washed as per provisions of Factories Act 1948 and rules made thereunder")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_LimeWashed_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_11_Cleanliness_LimeWashed_Selection { get; set; }

        [Display(Name = "11.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_LimeWashed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_11_Cleanliness_LimeWashed_ViolationExist { get; set; }

        [Display(Name = "11.4.3 Sec_11_Cleanliness_LimeWashed_Remarks")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_LimeWashed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_Cleanliness_LimeWashed_Remarks can have max 1000 chars..!")]
        public string Sec_11_Cleanliness_LimeWashed_Remarks { get; set; }

        [Display(Name = "11.5.1 Is the record of dates on which whitewashing, color-washing, varnishing, etc., are carried out entered in a register maintained in Form No. 7")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_Whitewashing_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_11_Cleanliness_Whitewashing_Selection { get; set; }

        [Display(Name = "11.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_Whitewashing_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_11_Cleanliness_Whitewashing_ViolationExist { get; set; }

        [Display(Name = "11.5.3 Sec_11_Cleanliness_Whitewashing_Remarks")]
        [Required(ErrorMessage = "Sec_11_Cleanliness_Whitewashing_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_Cleanliness_Whitewashing_Remarks can have max 1000 chars..!")]
        public string Sec_11_Cleanliness_Whitewashing_Remarks { get; set; }

        [Display(Name = "Section-12 :12.1.1 Are the arrangements made in every factory for the disposal of wastes and effluents due to the manufacturing processes carried on therein in accordance with those approved by the Punjab Pollution Control Board and other appropriate authorities")]
        [Required(ErrorMessage = "Sec_12_DisposalWaste_PPCB_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_12_DisposalWaste_PPCB_Selection { get; set; }

        [Display(Name = "12.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_12_DisposalWaste_PPCB_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_12_DisposalWaste_PPCB_ViolationExist { get; set; }

        [Display(Name = "12.1.3 Sec_12_DisposalWaste_PPCB_Remarks")]
        [Required(ErrorMessage = "Sec_12_DisposalWaste_PPCB_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_12_DisposalWaste_PPCB_Remarks can have max 1000 chars..!")]
        public string Sec_12_DisposalWaste_PPCB_Remarks { get; set; }

        [Display(Name = "Section-13 :13.1.1 Are Wet-Bulb/Dry-Bulb/air-movement provided as per Rule-19A(1)")]
        [Required(ErrorMessage = "Sec_13_VantilationTemperature_WetBulb_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_13_VantilationTemperature_WetBulb_Selection { get; set; }

        [Display(Name = "13.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_13_VantilationTemperature_WetBulb_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_13_VantilationTemperature_WetBulb_ViolationExist { get; set; }

        [Display(Name = "13.1.3 Sec_13_VantilationTemperature_WetBulb_Remarks")]
        [Required(ErrorMessage = "Sec_13_VantilationTemperature_WetBulb_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_13_VantilationTemperature_WetBulb_Remarks can have max 1000 chars..!")]
        public string Sec_13_VantilationTemperature_WetBulb_Remarks { get; set; }

        [Display(Name = "13.2.1 Is ventilation in workroom as per Rule-19A(2)")]
        [Required(ErrorMessage = "Sec_13_VantilationTemperature_WorkRoom_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_13_VantilationTemperature_WorkRoom_Selection { get; set; }

        [Display(Name = "13.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_13_VantilationTemperature_WorkRoom_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_13_VantilationTemperature_WorkRoom_ViolationExist { get; set; }

        [Display(Name = "13.2.3 Sec_13_VantilationTemperature_WorkRoom_Remarks")]
        [Required(ErrorMessage = "Sec_13_VantilationTemperature_WorkRoom_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_13_VantilationTemperature_WorkRoom_Remarks can have max 1000 chars..!")]
        public string Sec_13_VantilationTemperature_WorkRoom_Remarks { get; set; }

        [Display(Name = "Section-14 :14.1.1 Are provisions of Section 14 being complied with")]
        [Required(ErrorMessage = "Sec_14_DustFume_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_14_DustFume_Selection { get; set; }

        [Display(Name = "14.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_14_DustFume_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_14_DustFume_ViolationExist { get; set; }

        [Display(Name = "14.1.3 Sec_14_DustFume_Remarks")]
        [Required(ErrorMessage = "Sec_14_DustFume_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_14_DustFume_Remarks can have max 1000 chars..!")]
        public string Sec_14_DustFume_Remarks { get; set; }

        [Display(Name = "Section-16 :16.1.1 Is space of 14.2 cubic meters available for every worker in every room")]
        [Required(ErrorMessage = "Sec_16_Overcrowding_142CM_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_16_Overcrowding_142CM_Selection { get; set; }

        [Display(Name = "16.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Overcrowding_142CM_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Overcrowding_142CM_ViolationExist { get; set; }

        [Display(Name = "16.1.3 Sec_16_Overcrowding_142CM_Remarks")]
        [Required(ErrorMessage = "Sec_16_Overcrowding_142CM_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Overcrowding_142CM_Remarks can have max 1000 chars..!")]
        public string Sec_16_Overcrowding_142CM_Remarks { get; set; }

        [Display(Name = "Section-17 :17.1.1 Are windows/sky-lights being kept clean for lighting?")]
        [Required(ErrorMessage = "Sec_17_Lighting_Windows_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_17_Lighting_Windows_Selection { get; set; }

        [Display(Name = "17.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_17_Lighting_Windows_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_17_Lighting_Windows_ViolationExist { get; set; }

        [Display(Name = "17.1.3 Sec_17_Lighting_Windows_Remarks")]
        [Required(ErrorMessage = "Sec_17_Lighting_Windows_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_17_Lighting_Windows_Remarks can have max 1000 chars..!")]
        public string Sec_17_Lighting_Windows_Remarks { get; set; }

        [Display(Name = "17.2.1 Are effective arrangements in place to prevent glare,eye - strain, etc.? ")]
        [Required(ErrorMessage = "Sec_17_Lighting_EyeStrain_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_17_Lighting_EyeStrain_Selection { get; set; }

        [Display(Name = "17.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_17_Lighting_EyeStrain_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_17_Lighting_EyeStrain_ViolationExist { get; set; }

        [Display(Name = "17.2.3 Sec_17_Lighting_EyeStrain_Remarks")]
        [Required(ErrorMessage = "Sec_17_Lighting_EyeStrain_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_17_Lighting_EyeStrain_Remarks can have max 1000 chars..!")]
        public string Sec_17_Lighting_EyeStrain_Remarks { get; set; }

        [Display(Name = "17.3.1 Are working places and passages sufficiently and suitable lighting ? ")]
        [Required(ErrorMessage = "Sec_17_Lighting_Passages_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_17_Lighting_Passages_Selection { get; set; }

        [Display(Name = "17.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_17_Lighting_Passages_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_17_Lighting_Passages_ViolationExist { get; set; }

        [Display(Name = "17.3.3 Sec_17_Lighting_Passages_Remarks")]
        [Required(ErrorMessage = "Sec_17_Lighting_Passages_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_17_Lighting_Passages_Remarks can have max 1000 chars..!")]
        public string Sec_17_Lighting_Passages_Remarks { get; set; }

        [Display(Name = " Section-18: 18.1.1 Drinking Water Are notices “DRINKING WATER” in language understood by workers displayed ? ")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Language_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_18_DrinkingWater_Language_Selection { get; set; }

        [Display(Name = "18.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Language_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_DrinkingWater_Language_ViolationExist { get; set; }

        [Display(Name = "18.1.3 Sec_18_DrinkingWater_Language_Remarks")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Language_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_DrinkingWater_Language_Remarks can have max 1000 chars..!")]
        public string Sec_18_DrinkingWater_Language_Remarks { get; set; }

        [Display(Name = "18.2.1 Are drinking water points complying with the distance prescribed in section 18(2)")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Distance_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_18_DrinkingWater_Distance_Selection { get; set; }

        [Display(Name = "18.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Distance_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_DrinkingWater_Distance_ViolationExist { get; set; }

        [Display(Name = "18.2.3 Sec_18_DrinkingWater_Distance_Remarks")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Distance_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_DrinkingWater_Distance_Remarks can have max 1000 chars..!")]
        public string Sec_18_DrinkingWater_Distance_Remarks { get; set; }

        [Display(Name = "18.3.1 Is the quantity of drinking water as prescribed in Rule 36 being supplied ? ")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Quality_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_18_DrinkingWater_Quality_Selection { get; set; }

        [Display(Name = "18.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Quality_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_DrinkingWater_Quality_ViolationExist { get; set; }

        [Display(Name = "18.3.3 Sec_18_DrinkingWater_Quantity_Remarks")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Quality_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_DrinkingWater_Quality_Remarks can have max 1000 chars..!")]
        public string Sec_18_DrinkingWater_Quality_Remarks { get; set; }

        [Display(Name = "18.4.1 Is drinking water supplied from the public authority system?")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Authority_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_18_DrinkingWater_Authority_Selection { get; set; }

        [Display(Name = "18.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Authority_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_DrinkingWater_Authority_ViolationExist { get; set; }

        [Display(Name = "18.4.3 Sec_18_DrinkingWater_Authority_Remarks")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Authority_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_DrinkingWater_Authority_Remarks can have max 1000 chars..!")]
        public string Sec_18_DrinkingWater_Authority_Remarks { get; set; }

        [Display(Name = "18.5.1 Does the source of drinking water require approval from Health Officer ? ")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Approval_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_18_DrinkingWater_Approval_Selection { get; set; }

        [Display(Name = "18.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Approval_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_DrinkingWater_Approval_ViolationExist { get; set; }

        [Display(Name = "18.5.3 Sec_18_DrinkingWater_Approval_Remarks")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Approval_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_DrinkingWater_Approval_Remarks can have max 1000 chars..!")]
        public string Sec_18_DrinkingWater_Approval_Remarks { get; set; }

        [Display(Name = "18.6.1 Are points of drinking water kept clean?")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Clean_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_18_DrinkingWater_Clean_Selection { get; set; }

        [Display(Name = "18.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Clean_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_DrinkingWater_Clean_ViolationExist { get; set; }

        [Display(Name = "18.6.3 Sec_18_DrinkingWater_Clean_Remarks")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Clean_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_DrinkingWater_Clean_Remarks can have max 1000 chars..!")]
        public string Sec_18_DrinkingWater_Clean_Remarks { get; set; }

        [Display(Name = "18.7.1 Are drinking water points for every 150 workers provided as per Rule 41 ? ")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Point_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_18_DrinkingWater_Point_Selection { get; set; }

        [Display(Name = "18.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Point_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_DrinkingWater_Point_ViolationExist { get; set; }

        [Display(Name = "18.7.3 Sec_18_DrinkingWater_Point_Remarks")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_Point_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_DrinkingWater_Point_Remarks can have max 1000 chars..!")]
        public string Sec_18_DrinkingWater_Point_Remarks { get; set; }

        [Display(Name = "18.8.1 Are arrangements to supply cool water made as per Rule-41?")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_CoolWater_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_18_DrinkingWater_CoolWater_Selection { get; set; }

        [Display(Name = "18.8.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_CoolWater_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_DrinkingWater_CoolWater_ViolationExist { get; set; }

        [Display(Name = "18.8.3 Sec_18_DrinkingWater_CoolWater_Remarks")]
        [Required(ErrorMessage = "Sec_18_DrinkingWater_CoolWater_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_DrinkingWater_CoolWater_Remarks can have max 1000 chars..!")]
        public string Sec_18_DrinkingWater_CoolWater_Remarks { get; set; }

        [Display(Name = "Section-19: 19.1.1  Latrines and Urinals  Are latrine accommodations provided as per Rule-42")]
        [Required(ErrorMessage = "Sec_19_Urinals_Accomodation_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_19_Urinals_Accomodation_Selection { get; set; }

        [Display(Name = "19.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Urinals_Accomodation_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Urinals_Accomodation_ViolationExist { get; set; }

        [Display(Name = "19.1.3 Sec_19_Urinals_Accomodation_Remarks")]
        [Required(ErrorMessage = "Sec_19_Urinals_Accomodation_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Urinals_Accomodation_Remarks can have max 1000 chars..!")]
        public string Sec_19_Urinals_Accomodation_Remarks { get; set; }

        [Display(Name = " 19.2.1 Are latrines conforming to public health requirements?")]
        [Required(ErrorMessage = "Sec_19_Urinals_PublicHealth_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_19_Urinals_PublicHealth_Selection { get; set; }

        [Display(Name = "19.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Urinals_PublicHealth_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Urinals_PublicHealth_ViolationExist { get; set; }

        [Display(Name = "19.2.3 Sec_19_Urinals_PublicHealth_Remarks")]
        [Required(ErrorMessage = "Sec_19_Urinals_PublicHealth_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Urinals_PublicHealth_Remarks can have max 1000 chars..!")]
        public string Sec_19_Urinals_PublicHealth_Remarks { get; set; }


        [Display(Name = "19.3.1 Is privacy ensured in latrine accommodation?")]
        [Required(ErrorMessage = "Sec_19_Urinals_Privacy_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_19_Urinals_Privacy_Selection { get; set; }

        [Display(Name = "19.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Urinals_Privacy_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Urinals_Privacy_ViolationExist { get; set; }

        [Display(Name = "19.3.3 Sec_19_Urinals_Privacy_Remarks")]
        [Required(ErrorMessage = "Sec_19_Urinals_Privacy_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Urinals_Privacy_Remarks can have max 1000 chars..!")]
        public string Sec_19_Urinals_Privacy_Remarks { get; set; }

        [Display(Name = "19.4.1 Are signboards on latrines in language understood by workers displayed ? ")]
        [Required(ErrorMessage = "Sec_19_Urinals_Signboards_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_19_Urinals_Signboards_Selection { get; set; }

        [Display(Name = "19.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Urinals_Signboards_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Urinals_Signboards_ViolationExist { get; set; }

        [Display(Name = "19.4.3 Sec_19_Urinals_Signboards_Remarks")]
        [Required(ErrorMessage = "Sec_19_Urinals_Signboards_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Urinals_Signboards_Remarks can have max 1000 chars..!")]
        public string Sec_19_Urinals_Signboards_Remarks { get; set; }

        [Display(Name = "19.5.1 Are latrine accommodations provided as per Rule-46")]
        [Required(ErrorMessage = "Sec_19_Urinals_Rule46_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_19_Urinals_Rule46_Selection { get; set; }

        [Display(Name = "19.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Urinals_Rule46_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Urinals_Rule46_ViolationExist { get; set; }

        [Display(Name = "19.5.3 Sec_19_Urinals_Rule46_Remarks")]
        [Required(ErrorMessage = "Sec_19_Urinals_Rule46_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Urinals_Rule46_Remarks can have max 1000 chars..!")]
        public string Sec_19_Urinals_Rule46_Remarks { get; set; }

        [Display(Name = "19.6.1 The walls, ceilings, and partitions of every latrine and urinal shall be whitewashed, and the whitewashing shall be repeated at least once in every period of four months.The dates on which the whitewashing is carried out shall be entered in the prescribed register Form No. 7.")]
        [Required(ErrorMessage = "Sec_19_Urinals_Walls_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_19_Urinals_Walls_Selection { get; set; }

        [Display(Name = "19.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Urinals_Wall_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Urinals_Wall_ViolationExist { get; set; }

        [Display(Name = "19.6.3 Sec_19_Urinals_Walls_Remarks")]
        [Required(ErrorMessage = "Sec_19_Urinals_Walls_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Urinals_Walls_Remarks can have max 1000 chars..!")]
        public string Sec_19_Urinals_Walls_Remarks { get; set; }

        [Display(Name = "Section-20: 20.1.1 Spittoons  Are spittoons kept and maintained as per Rule-53 and 54")]
        [Required(ErrorMessage = "Sec_20_Spittoons_Rule_53_54_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_20_Spittoons_Rule_53_54_Selection { get; set; }

        [Display(Name = "20.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_20_Spittoons_Rule_53_54_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_20_Spittoons_Rule_53_54_ViolationExist { get; set; }

        [Display(Name = "20.1.3  Sec_20_Spittoons_Rule_53_54_Remarks")]
        [Required(ErrorMessage = "Sec_20_Spittoons_Rule_53_54_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_20_Spittoons_Rule_53_54_Remarks can have max 1000 chars..!")]
        public string Sec_20_Spittoons_Rule_53_54_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_III_Safety
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "section-21: Fencing of Machinery :21.1.1 Is any Schedule prescribed under Rule-55 applicable?")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_Rule55_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_21_FancingOfMachinery_Rule55_Selection { get; set; }

        [Display(Name = "21.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_Rule55_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_21_FancingOfMachinery_Rule55_ViolationExist { get; set; }

        [Display(Name = "21.1.3 Sec_21_FancingOfMachinery_Rule55_Remarks")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_Rule55_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_21_FancingOfMachinery_Rule55_Remarks can have max 1000 chars..!")]
        public string Sec_21_FancingOfMachinery_Rule55_Remarks { get; set; }

        [Display(Name = "21.2.1 Report on guarding of as per sec_tion 21?")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_Guarding_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_21_FancingOfMachinery_Guarding_Selection { get; set; }

        [Display(Name = "21.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_Guarding_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_21_FancingOfMachinery_Guarding_ViolationExist { get; set; }

        [Display(Name = "21.2.3 Sec_21_FancingOfMachinery_Guarding_Remarks")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_Guarding_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_21_FancingOfMachinery_Guarding_Remarks can have max 1000 chars..!")]
        public string Sec_21_FancingOfMachinery_Guarding_Remarks { get; set; }

        [Display(Name = "21.3.1 Any other specific report")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_OtherSpecificReport_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_21_FancingOfMachinery_OtherSpecificReport_Selection { get; set; }

        [Display(Name = "21.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_OtherSpecificReport_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_21_FancingOfMachinery_OtherSpecificReport_ViolationExist { get; set; }

        [Display(Name = "21.3.3 Sec_21_FancingOfMachinery_OtherSpecificReport_Remarks")]
        [Required(ErrorMessage = "Sec_21_FancingOfMachinery_OtherSpecificReport_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_21_FancingOfMachinery_OtherSpecificReport_Remarks can have max 1000 chars..!")]
        public string Sec_21_FancingOfMachinery_OtherSpecificReport_Remarks { get; set; }

        [Display(Name = "22.1.1 section-22: Work on or near machinery in motion : Registers of workers attending to machinery as provided in sub - section(1) of section 22 of the Act shall be in Form 7 - A.")]
        [Required(ErrorMessage = "Sec_22_MachineryInMotion_Form7_A_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_22_MachineryInMotion_Form7_A_Selection { get; set; }

        [Display(Name = "22.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_22_MachineryInMotion_Form7_A_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_22_MachineryInMotion_Form7_A_ViolationExist { get; set; }

        [Display(Name = "22.1.3 Sec_22_MachineryInMotion_Form7_A_Remarks")]
        [Required(ErrorMessage = "Sec_22_MachineryInMotion_Form7_A_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_22_MachineryInMotion_Form7_A_Remarks can have max 1000 chars..!")]
        public string Sec_22_MachineryInMotion_Form7_A_Remarks { get; set; }

        [Display(Name = " section-28: Hoists and Lifts :28.1.1  Is record of examination of hoist and lifts is being kept in Form 23")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_Examination_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_28_HoistAndLifts_Examination_Selection { get; set; }

        [Display(Name = "28.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_Examination_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_28_HoistAndLifts_Examination_ViolationExist { get; set; }

        [Display(Name = "28.1.3 Sec_28_HoistAndLifts_Examination_Remarks")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_Examination_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_28_HoistAndLifts_Examination_Remarks can have max 1000 chars..!")]
        public string Sec_28_HoistAndLifts_Examination_Remarks { get; set; }

        [Display(Name = "28.2.1 (c) the maximum safe working load shall be plainly marked")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_SafeWorking_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_28_HoistAndLifts_SafeWorking_Selection { get; set; }

        [Display(Name = "28.2.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_28_HoistAndLifts_SafeWorking_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_28_HoistAndLifts_SafeWorking_ViolationExist { get; set; }

        [Display(Name = "28.2.3 Sec_28_HoistAndLifts_SafeWorking_Remarks")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_SafeWorking_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_28_HoistAndLifts_SafeWorking_Remarks can have max 1000 chars..!")]
        public string Sec_28_HoistAndLifts_SafeWorking_Remarks { get; set; }

        [Display(Name = "28.3.1 Is interlocking or other safety device provided")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_Interlocking_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_28_HoistAndLifts_Interlocking_Selection { get; set; }

        [Display(Name = "28.3.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_28_HoistAndLifts_Interlocking_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_28_HoistAndLifts_Interlocking_ViolationExist { get; set; }

        [Display(Name = "28.3.3 Sec_28_HoistAndLifts_Interlocking_Remarks")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_Interlocking_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_28_HoistAndLifts_Interlocking_Remarks can have max 1000 chars..!")]
        public string Sec_28_HoistAndLifts_Interlocking_Remarks { get; set; }

        [Display(Name = "28.4.1 Any other specific report including annual maintenance contract.")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_OtherSpecificReport_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_28_HoistAndLifts_OtherSpecificReport_Selection { get; set; }

        [Display(Name = "28.4.2 Is Violation Founds")]
        [Required(ErrorMessage = " Sec_28_HoistAndLifts_OtherSpecificReport_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_28_HoistAndLifts_OtherSpecificReport_ViolationExist { get; set; }

        [Display(Name = "28.4.3 Sec_28_HoistAndLifts_OtherSpecificReport_Remarks")]
        [Required(ErrorMessage = "Sec_28_HoistAndLifts_OtherSpecificReport_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_28_HoistAndLifts_OtherSpecificReport_Remarks can have max 1000 chars..!")]
        public string Sec_28_HoistAndLifts_OtherSpecificReport_Remarks { get; set; }

        [Display(Name = "section-29 Lifting machine, chains, ropes and lifting tackles :29.1.1  Are all parts, including the working gear, whether fixed or movable, of every lifting machine and every chain, rope or lifting tackle(i) of good construction, sound material and adequate strength and free from defects;")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_GoodConstruction_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_29_LiftingMachine_GoodConstruction_Selection { get; set; }

        [Display(Name = "29.1.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_29_LiftingMachine_GoodConstruction_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_LiftingMachine_GoodConstruction_ViolationExist { get; set; }

        [Display(Name = "29.1.3 Sec_29_LiftingMachine_GoodConstruction_Remarks")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_GoodConstruction_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_LiftingMachine_GoodConstruction_Remarks can have max 1000 chars..!")]
        public string Sec_29_LiftingMachine_GoodConstruction_Remarks { get; set; }

        [Display(Name = "29.2.1  properly maintained")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_ProperlyMaintained_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_29_LiftingMachine_ProperlyMaintained_Selection { get; set; }

        [Display(Name = "29.2.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_29_LiftingMachine_ProperlyMaintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_LiftingMachine_ProperlyMaintained_ViolationExist { get; set; }

        [Display(Name = "29.2.3 Sec_29_LiftingMachine_ProperlyMaintained_Remarks")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_ProperlyMaintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_LiftingMachine_ProperlyMaintained_Remarks can have max 1000 chars..!")]
        public string Sec_29_LiftingMachine_ProperlyMaintained_Remarks { get; set; }

        [Display(Name = "29.3.1  thoroughly examined by a competent person at least once in every period of twelve months")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_CompetentPerson_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_29_LiftingMachine_CompetentPerson_Selection { get; set; }

        [Display(Name = "29.3.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_29_LiftingMachine_CompetentPerson_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_LiftingMachine_CompetentPerson_ViolationExist { get; set; }

        [Display(Name = "29.3.3 Sec_29_LiftingMachine_CompetentPerson_Remarks")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_CompetentPerson_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_LiftingMachine_CompetentPerson_Remarks can have max 1000 chars..!")]
        public string Sec_29_LiftingMachine_CompetentPerson_Remarks { get; set; }

        [Display(Name = "29.4.1 Is the maximum safe working load and identification number marked ? ")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_MaximumSafeWorking_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_29_LiftingMachine_MaximumSafeWorking_Selection { get; set; }

        [Display(Name = "29.4.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_29_LiftingMachine_MaximumSafeWorking_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_LiftingMachine_MaximumSafeWorking_ViolationExist { get; set; }

        [Display(Name = "29.4.3 Sec_29_LiftingMachine_MaximumSafeWorking_Remarks")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_MaximumSafeWorking_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_LiftingMachine_MaximumSafeWorking_Remarks can have max 1000 chars..!")]
        public string Sec_29_LiftingMachine_MaximumSafeWorking_Remarks { get; set; }

        [Display(Name = "29.5.1 Is register containing the details prescribed in Rule 60A(3) maintained ? ")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_Rule60A_3_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_29_LiftingMachine_Rule60A_3_Selection { get; set; }

        [Display(Name = "29.5.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_29_LiftingMachine_Rule60A_3_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_LiftingMachine_Rule60A_3_ViolationExist { get; set; }

        [Display(Name = "29.5.3 Sec_29_LiftingMachine_Rule60A_3_Remarks")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_Rule60A_3_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_LiftingMachine_Rule60A_3_Remarks can have max 1000 chars..!")]
        public string Sec_29_LiftingMachine_Rule60A_3_Remarks { get; set; }

        [Display(Name = "29.6.1 Are suitable passages for cranes provided?")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_SuitablePassages_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_29_LiftingMachine_SuitablePassages_Selection { get; set; }

        [Display(Name = "29.6.2 Is Violation Found")]
        [Required(ErrorMessage = " Sec_29_LiftingMachine_SuitablePassages_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_LiftingMachine_SuitablePassages_ViolationExist { get; set; }

        [Display(Name = "29.6.3 Sec_29_LiftingMachine_SuitablePassages_Remarks")]
        [Required(ErrorMessage = "Sec_29_LiftingMachine_SuitablePassages_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_LiftingMachine_SuitablePassages_Remarks can have max 1000 chars..!")]
        public string Sec_29_LiftingMachine_SuitablePassages_Remarks { get; set; }

        [Display(Name = "sec_tion-30 Revolving Machine :30.1.1  Are notices of safe speed displayed near revolving machines as per sec_tion - 30")]
        [Required(ErrorMessage = "Sec_30_RevolvingMachine_NoticesOfSafe_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_30_RevolvingMachine_NoticesOfSafe_Selection { get; set; }

        [Display(Name = "30.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_30_RevolvingMachine_NoticesOfSafe_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_30_RevolvingMachine_NoticesOfSafe_ViolationExist { get; set; }

        [Display(Name = "30.1.3 Sec_30_RevolvingMachine_NoticesOfSafe_Remarks")]
        [Required(ErrorMessage = "Sec_30_RevolvingMachine_NoticesOfSafe_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_30_RevolvingMachine_NoticesOfSafe_Remarks can have max 1000 chars..!")]
        public string Sec_30_RevolvingMachine_NoticesOfSafe_Remarks { get; set; }

        [Display(Name = "sec_tion-31 Pressure Plant :31.1.1  Are pressure plants of good construction, sound material, adequate strength, and free from any patent defect and properly maintained in a safe condition ? ")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_GoodConstruction_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_31_PressurePlant_GoodConstruction_Selection { get; set; }

        [Display(Name = "31.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_GoodConstruction_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_31_PressurePlant_GoodConstruction_ViolationExist { get; set; }

        [Display(Name = "31.1.3 Sec_31_PressurePlant_GoodConstruction_Remarks")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_GoodConstruction_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_31_PressurePlant_GoodConstruction_Remarks can have max 1000 chars..!")]
        public string Sec_31_PressurePlant_GoodConstruction_Remarks { get; set; }

        [Display(Name = "31.2.1 Is a suitable safety valve or other effective device provided to ensure the maximum permissible working pressure ? ")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_SafetyValve_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_31_PressurePlant_SafetyValve_Selection { get; set; }

        [Display(Name = "31.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_SafetyValve_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_31_PressurePlant_SafetyValve_ViolationExist { get; set; }

        [Display(Name = "31.2.3 Sec_31_PressurePlant_SafetyValve_Remarks")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_SafetyValve_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_31_PressurePlant_SafetyValve_Remarks can have max 1000 chars..!")]
        public string Sec_31_PressurePlant_SafetyValve_Remarks { get; set; }

        [Display(Name = "31.3.1 Is a suitable pressure gauge provided, easily visible, and designed to show the correct internal pressure in kilogram per square centimeter, marked with a prominent red mark at the safe working pressure of the vessel?")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_Gauge_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_31_PressurePlant_Gauge_Selection { get; set; }

        [Display(Name = "31.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_Gauge_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_31_PressurePlant_Gauge_ViolationExist { get; set; }

        [Display(Name = "31.3.3 Sec_31_PressurePlant_Gauge_Remarks")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_Gauge_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_31_PressurePlant_Gauge_Remarks can have max 1000 chars..!")]
        public string Sec_31_PressurePlant_Gauge_Remarks { get; set; }

        [Display(Name = "31.4.1 Is a suitable stop valve or valves provided to isolate the vessel from other vessels or sources of pressure supply ?")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_StopValve_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_31_PressurePlant_StopValve_Selection { get; set; }

        [Display(Name = "31.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_StopValve_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_31_PressurePlant_StopValve_ViolationExist { get; set; }

        [Display(Name = "31.4.3 Sec_31_PressurePlant_StopValve_Remarks")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_StopValve_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_31_PressurePlant_StopValve_Remarks can have max 1000 chars..!")]
        public string Sec_31_PressurePlant_StopValve_Remarks { get; set; }

        [Display(Name = "31.5.1 Is a suitable drain cock or valve at the lowest part of the vessel for the discharge of connected liquid provided ?")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_DrainCock_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_31_PressurePlant_DrainCock_Selection { get; set; }

        [Display(Name = "31.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_DrainCock_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_31_PressurePlant_DrainCock_ViolationExist { get; set; }

        [Display(Name = "31.5.3 Sec_31_PressurePlant_DrainCock_Remarks")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_DrainCock_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_31_PressurePlant_DrainCock_Remarks can have max 1000 chars..!")]
        public string Sec_31_PressurePlant_DrainCock_Remarks { get; set; }

        [Display(Name = "31.6.1 Is every pressure vessel thoroughly examined by a competent person: (i)externally, once in every period of six months, to ensure the general condition of the vessel and the working of its fittings; (ii)internally, once in every period of twelve months, to ensure the condition of the walls, seams, and ties both inside and outside of the vessel, soundness of the part of the vessel, and the effects of corrosion; (iii)hydraulically tested at intervals of not more than four years ?")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_CompetentPerson_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_31_PressurePlant_CompetentPerson_Selection { get; set; }

        [Display(Name = "31.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_CompetentPerson_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_31_PressurePlant_CompetentPerson_ViolationExist { get; set; }

        [Display(Name = "31.6.3 Sec_31_PressurePlant_CompetentPerson_Remarks")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_CompetentPerson_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_31_PressurePlant_CompetentPerson_Remarks can have max 1000 chars..!")]
        public string Sec_31_PressurePlant_CompetentPerson_Remarks { get; set; }

        [Display(Name = "31.7.1 Any other specific report")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_SpecificReport_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_31_PressurePlant_SpecificReport_Selection { get; set; }

        [Display(Name = "31.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_SpecificReport_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_31_PressurePlant_SpecificReport_ViolationExist { get; set; }

        [Display(Name = "31.7.3 Sec_31_PressurePlant_SpecificReport_Remarks")]
        [Required(ErrorMessage = "Sec_31_PressurePlant_SpecificReport_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_31_PressurePlant_SpecificReport_Remarks can have max 1000 chars..!")]
        public string Sec_31_PressurePlant_SpecificReport_Remarks { get; set; }

        [Display(Name = "sec_tion-32 Floor, Stairs and means of access :32.1.1 all floors, steps, stairs, passages and gangways shall be of sound construction and properly maintained.")]
        [Required(ErrorMessage = "Sec_32_FloorStairs_ProperlyMaintained_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_32_FloorStairs_ProperlyMaintained_Selection { get; set; }

        [Display(Name = "32.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_32_FloorStairs_ProperlyMaintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_32_FloorStairs_ProperlyMaintained_ViolationExist { get; set; }

        [Display(Name = "32.1.3 Sec_32_FloorStairs_ProperlyMaintained_Remarks")]
        [Required(ErrorMessage = "Sec_32_FloorStairs_ProperlyMaintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_32_FloorStairs_ProperlyMaintained_Remarks can have max 1000 chars..!")]
        public string Sec_32_FloorStairs_ProperlyMaintained_Remarks { get; set; }

        [Display(Name = "32.2.1  Are substantial handrails provided?")]
        [Required(ErrorMessage = "Sec_32_FloorStairs_HandrailsProvided_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_32_FloorStairs_HandrailsProvided_Selection { get; set; }

        [Display(Name = "32.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_32_FloorStairs_HandrailsProvided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_32_FloorStairs_HandrailsProvided_ViolationExist { get; set; }

        [Display(Name = "32.2.3 Sec_32_FloorStairs_HandrailsProvided_Remarks")]
        [Required(ErrorMessage = "Sec_32_FloorStairs_HandrailsProvided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_32_FloorStairs_HandrailsProvided_Remarks can have max 1000 chars..!")]
        public string Sec_32_FloorStairs_HandrailsProvided_Remarks { get; set; }

        [Display(Name = "sec_tion-33 Pits, Sumps, opening in floors :33.1.1  Is every fixed vessel, sump, tank, pit or opening in the ground or in a floor which, by reason of its depth, situation, construction or contents, is or may be a source of danger, shall be either securely covered or securely fenced ?")]
        [Required(ErrorMessage = "Sec_33_PitsSumps_SecurelyCovered_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_33_PitsSumps_SecurelyCovered_Selection { get; set; }

        [Display(Name = "33.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_33_PitsSumps_SecurelyCovered_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_33_PitsSumps_SecurelyCovered_ViolationExist { get; set; }

        [Display(Name = "33.1.3 Sec_33_PitsSumps_SecurelyCovered_Remarks")]
        [Required(ErrorMessage = "Sec_33_PitsSumps_SecurelyCovered_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_33_PitsSumps_SecurelyCovered_Remarks can have max 1000 chars..!")]
        public string Sec_33_PitsSumps_SecurelyCovered_Remarks { get; set; }

        [Display(Name = "sec_tion-34 Excessive weights :34.1.1 Are provisions of Rule 62 being complied with?")]
        [Required(ErrorMessage = "Sec_34_ExcessiveWeights_Rule62_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_34_ExcessiveWeights_Rule62_Selection { get; set; }

        [Display(Name = "34.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_34_ExcessiveWeights_Rule62_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_34_ExcessiveWeights_Rule62_ViolationExist { get; set; }

        [Display(Name = "34.1.3 Sec_34_ExcessiveWeights_Rule62_Remarks")]
        [Required(ErrorMessage = "Sec_34_ExcessiveWeights_Rule62_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_34_ExcessiveWeights_Rule62_Remarks can have max 1000 chars..!")]
        public string Sec_34_ExcessiveWeights_Rule62_Remarks { get; set; }

        [Display(Name = "sec_tion-35 Protection of eyes :35.1.1  Are effective screens or suitable goggles shall be provided for the protection of persons employed in or in the immediate vicinity of the processes specified in Schedule I and II")]
        [Required(ErrorMessage = "Sec_35_ProtectionOfEyes_SuitableGoggles_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_35_ProtectionOfEyes_SuitableGoggles_Selection { get; set; }

        [Display(Name = "34.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_35_ProtectionOfEyes_SuitableGoggles_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_35_ProtectionOfEyes_SuitableGoggles_ViolationExist { get; set; }

        [Display(Name = "35.1.3 Sec_35_ProtectionOfEyes_SuitableGoggles_Remarks")]
        [Required(ErrorMessage = "Sec_35_ProtectionOfEyes_SuitableGoggles_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_35_ProtectionOfEyes_SuitableGoggles_Remarks can have max 1000 chars..!")]
        public string Sec_35_ProtectionOfEyes_SuitableGoggles_Remarks { get; set; }

        [Display(Name = "sec_tion-36 Precautions against dangerous fumes :36.1.1  Are manholes have Minimum dimensions as prescribed under Rule-64")]
        [Required(ErrorMessage = "Sec_36_PrecautionsDangerusFumes_Rule64_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_36_PrecautionsDangerusFumes_Rule64_Selection { get; set; }

        [Display(Name = "36.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_36_PrecautionsDangerusFumes_Rule64_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_36_PrecautionsDangerusFumes_Rule64_ViolationExist { get; set; }

        [Display(Name = "36.1.3 Sec_36_PrecautionsDangerusFumes_Rule64_Remarks")]
        [Required(ErrorMessage = "Sec_36_PrecautionsDangerusFumes_Rule64_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_36_PrecautionsDangerusFumes_Rule64_Remarks can have max 1000 chars..!")]
        public string Sec_36_PrecautionsDangerusFumes_Rule64_Remarks { get; set; }

        [Display(Name = "sec_tion-37 Explosive, Inflammable dust, gas etc. :37.1.1  Is manufacturing process produces dust, gas, fume or vapour of such character and to such extent as to be likely to explode on ignition, all practicable measures shall be taken to prevent any such explosion by; (a)effective enclosure of the plant or machinery used in the process; (b)removal or prevention of the accumulation of such dust, gas, fume or vapour; (c)exclusion or effective enclosure of all possible sources of ignition.")]
        [Required(ErrorMessage = "Sec_37_Explosive_ManufaturinProcess_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_37_Explosive_ManufaturinProcess_Selection { get; set; }

        [Display(Name = "37.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_37_Explosive_ManufaturinProcess_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_37_Explosive_ManufaturinProcess_ViolationExist { get; set; }

        [Display(Name = "37.1.3 Sec_37_Explosive_ManufaturinProcess_Remarks")]
        [Required(ErrorMessage = "Sec_37_Explosive_ManufaturinProcess_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_37_Explosive_ManufaturinProcess_Remarks can have max 1000 chars..!")]
        public string Sec_37_Explosive_ManufaturinProcess_Remarks { get; set; }

        [Display(Name = "sec_tion-38 Precaution in case of fire :38.1.1 Is protection against lighting - Protection from lighting provided as per Rule 66(3) ? ")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_3_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_Rule66_3_Selection { get; set; }

        [Display(Name = "38.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_3_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_Rule66_3_ViolationExist { get; set; }

        [Display(Name = "38.1.3 Sec_38_PrecautionOfFire_Rule66_3_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_3_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_3_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_Rule66_3_Remarks { get; set; }

        [Display(Name = "38.2.1 Are protections against ignition taken as per Rule 66(4)?")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_4_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_Rule66_4_Selection { get; set; }

        [Display(Name = "38.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_4_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_Rule66_4_ViolationExist { get; set; }

        [Display(Name = "38.2.3 Sec_38_PrecautionOfFire_Rule66_4_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_4_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_4_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_Rule66_4_Remarks { get; set; }

        [Display(Name = "38.3.1 Are materials susceptible to auto ignition are stored as per Rule 66(5) ? ")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_5_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_Rule66_5_Selection { get; set; }

        [Display(Name = "38.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_5_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_Rule66_5_ViolationExist { get; set; }

        [Display(Name = "38.3.3 Sec_38_PrecautionOfFire_Rule66_5_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_5_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_5_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_Rule66_5_Remarks { get; set; }

        [Display(Name = "38.4.1 Are compressed gas cylinders are stored as per Rule 66(6)?")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_6_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_Rule66_6_Selection { get; set; }

        [Display(Name = "38.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_6_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_Rule66_6_ViolationExist { get; set; }

        [Display(Name = "38.4.3 Sec_38_PrecautionOfFire_Rule66_6_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_6_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_6_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_Rule66_6_Remarks { get; set; }

        [Display(Name = "38.5.1 Are Inflammable liquids are stored as per Rule 66(7)?")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_7_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_Rule66_7_Selection { get; set; }

        [Display(Name = "38.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_7_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_Rule66_7_ViolationExist { get; set; }

        [Display(Name = "38.5.3 Sec_38_PrecautionOfFire_Rule66_7_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_7_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_7_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_Rule66_7_Remarks { get; set; }

        [Display(Name = "38.6.1 Are preventive measures have been taken for accumulation of flammable dust, gas, fume or vapour in air of flammable waste material on the floors ? ")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_PreventiveMeasures_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_PreventiveMeasures_Selection { get; set; }

        [Display(Name = "38.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_PreventiveMeasure_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_PreventiveMeasure_ViolationExist { get; set; }

        [Display(Name = "38.6.3 Sec_38_PrecautionOfFire_PreventiveMeasures_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_PreventiveMeasures_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_PreventiveMeasures_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_PreventiveMeasures_Remarks { get; set; }

        [Display(Name = "38.7.1 Are FIRE EXITS are maintained as per Rule 66(9)?")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_9_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_Rule66_9_Selection { get; set; }

        [Display(Name = "38.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_9_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_Rule66_9_ViolationExist { get; set; }

        [Display(Name = "38.7.3 Sec_38_PrecautionOfFire_Rule66_9_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_9_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_9_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_Rule66_9_Remarks { get; set; }

        [Display(Name = "38.8.1 Are First-aid fire fighting arrangements are maintained adequately and suitably as per Rule 66(10) ? ")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_10_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_Rule66_10_Selection { get; set; }

        [Display(Name = "38.8.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_10_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_Rule66_10_ViolationExist { get; set; }

        [Display(Name = "38.8.3 Sec_38_PrecautionOfFire_Rule66_10_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_10_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_10_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_Rule66_10_Remarks { get; set; }

        [Display(Name = "38.9.1 Are other fire fighting arrangements are provided as per Rule 66(11) ? ")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_11_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_Rule66_11_Selection { get; set; }

        [Display(Name = "39.9.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_11_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_Rule66_11_ViolationExist { get; set; }

        [Display(Name = "38.9.3 Sec_38_PrecautionOfFire_Rule66_11_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_11_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_Rule66_11_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_Rule66_11_Remarks { get; set; }

        [Display(Name = "30.10.1 Are first-Aid and other fire fighting equipment in the charge of a trained responsible person ? ")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_FirstAid_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_FirstAid_Selection { get; set; }

        [Display(Name = "30.10.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_FirstAid_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_FirstAid_ViolationExist { get; set; }

        [Display(Name = "30.10.3 Sec_38_PrecautionOfFire_FirstAid_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_FirstAid_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_FirstAid_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_FirstAid_Remarks { get; set; }

        [Display(Name = "30.11.1 Are sufficient number of persons trained in the proper handling of fire fighting equipment ? ")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_PersonTrained_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_PersonTrained_Selection { get; set; }

        [Display(Name = "30.11.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_PersonTrained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_PersonTrained_ViolationExist { get; set; }

        [Display(Name = "30.11.3 Sec_38_PrecautionOfFire_PersonTrained_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_PersonTrained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_PersonTrained_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_PersonTrained_Remarks { get; set; }

        [Display(Name = "30.12.1 Is fire fighting drill held at least once in every period of two months")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_FightingDrill_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_38_PrecautionOfFire_FightingDrill_Selection { get; set; }

        [Display(Name = "30.12.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_FightingDrill_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_38_PrecautionOfFire_FightingDrill_ViolationExist { get; set; }

        [Display(Name = "30.12.3 Sec_38_PrecautionOfFire_FightingDrill_Remarks")]
        [Required(ErrorMessage = "Sec_38_PrecautionOfFire_FightingDrill_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_38_PrecautionOfFire_FightingDrill_Remarks can have max 1000 chars..!")]
        public string Sec_38_PrecautionOfFire_FightingDrill_Remarks { get; set; }

        [Display(Name = "Section-40/Rule-66A, 66B, 66C, and 66D  Special report :40.1.1 Is any building or structure constructed, situated or maintained in factory in such a manner to cause risk of bodily injury")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_BuildingCauseRiskOfBodilyInjury_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_40_SpecialReport_BuildingCauseRiskOfBodilyInjury_Selection { get; set; }

        [Display(Name = "40.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_BuildingCauseRiskOfBodilyInjury_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_40_SpecialReport_BuildingCauseRiskOfBodilyInjury_ViolationExist { get; set; }

        [Display(Name = "40.1.3 Sec_40_SpecialReport_BuildingCauseRiskOfBodilyInjury_Remarks")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_BuildingCauseRiskOfBodilyInjury_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_40_SpecialReport_BuildingCauseRiskOfBodilyInjury_Remarks can have max 1000 chars..!")]
        public string Sec_40_SpecialReport_BuildingCauseRiskOfBodilyInjury_Remarks { get; set; }

        [Display(Name = "40.2.1 Is any plant or machinery constructed, situated or maintained in factory in such a manner to cause risk of bodily injury")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_PlantCauseRiskOfBodilyInjury_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_40_SpecialReport_PlantCauseRiskOfBodilyInjury_Selection { get; set; }

        [Display(Name = "40.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_PlantCauseRiskOfBodilyInjury_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_40_SpecialReport_PlantCauseRiskOfBodilyInjury_ViolationExist { get; set; }

        [Display(Name = "40.2.3 Sec_40_SpecialReport_PlantCauseRiskOfBodilyInjury_Remarks")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_PlantCauseRiskOfBodilyInjury_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_40_SpecialReport_PlantCauseRiskOfBodilyInjury_Remarks can have max 1000 chars..!")]
        public string Sec_40_SpecialReport_PlantCauseRiskOfBodilyInjury_Remarks { get; set; }

        [Display(Name = "40.3.1 Is any process or method of work carried out in factory in such a manner to cause risk of bodily injury")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Selection { get; set; }

        [Display(Name = "40.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_ViolationExist { get; set; }

        [Display(Name = "40.3.3 Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Remarks")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Remarks can have max 1000 chars..!")]
        public string Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Remarks { get; set; }

        [Display(Name = "40.4.1 Is any material or equipment stacked or stored in factory in such a manner to cause risk of bodily injury")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_40_SpecialReport_MaterialCauseRiskOfBodilyInjury_Selection { get; set; }

        [Display(Name = "40.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_MaterialCauseRiskOfBodilyInjury_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_40_SpecialReport_MaterialCauseRiskOfBodilyInjury_ViolationExist { get; set; }

        [Display(Name = "40.4.3 Sec_40_SpecialReport_MaterialCauseRiskOfBodilyInjury_Remarks")]
        [Required(ErrorMessage = "Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_40_SpecialReport_ProcessCauseRiskOfBodilyInjury_Remarks can have max 1000 chars..!")]
        public string Sec_40_SpecialReport_MaterialCauseRiskOfBodilyInjury_Remarks { get; set; }

        [Display(Name = "Section-40B Safety Officer :40B.1.1  Number of Safety officers required")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_NoOfRequired_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_40B_SafetyOfficer_NoOfRequired_Selection { get; set; }

        [Display(Name = "40B.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_NoOfRequired_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_40B_SafetyOfficer_NoOfRequired_ViolationExist { get; set; }

        [Display(Name = "40B.1.3 Sec_40B_SafetyOfficer_NoOfRequired_Remarks")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_NoOfRequired_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_40B_SafetyOfficer_NoOfRequired_Remarks can have max 1000 chars..!")]
        public string Sec_40B_SafetyOfficer_NoOfRequired_Remarks { get; set; }

        [Display(Name = "40B.2.1 Number of Safety Officer appointed")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_NoOfAppointed_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_40B_SafetyOfficer_NoOfAppointed_Selection { get; set; }

        [Display(Name = "40B.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_NoOfAppointed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_40B_SafetyOfficer_NoOfAppointed_ViolationExist { get; set; }

        [Display(Name = "40B.2.3 Sec_40B_SafetyOfficer_NoOfAppointed_Remarks")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_NoOfAppointed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_40B_SafetyOfficer_NoOfAppointed_Remarks can have max 1000 chars..!")]
        public string Sec_40B_SafetyOfficer_NoOfAppointed_Remarks { get; set; }

        [Display(Name = "40B.3.1 Are appointed Safety officer fulfilling the eligibility criteria as per rules")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_EligibilityCriteria_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_40B_SafetyOfficer_EligibilityCriteria_Selection { get; set; }

        [Display(Name = "40B.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_EligibilityCriteria_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_40B_SafetyOfficer_EligibilityCriteria_ViolationExist { get; set; }

        [Display(Name = "40B.3.3 Sec_40B_SafetyOfficer_EligibilityCriteria_Remarks")]
        [Required(ErrorMessage = "Sec_40B_SafetyOfficer_EligibilityCriteria_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_40B_SafetyOfficer_EligibilityCriteria_Remarks can have max 1000 chars..!")]
        public string Sec_40B_SafetyOfficer_EligibilityCriteria_Remarks { get; set; }

        [Display(Name = "Rule-66F Safety Committee :66F.1.1  Is safety Committee required as per Rule-66F")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeRequired_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_66F_SafetyCommittee_CommitteeRequired_Selection { get; set; }

        [Display(Name = "66F.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeRequired_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_66F_SafetyCommittee_CommitteeRequired_ViolationExist { get; set; }

        [Display(Name = "66F.1.3 Sec_66F_SafetyCommittee_CommitteeRequired_Remarks")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeRequired_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeRequired_Remarks can have max 1000 chars..!")]
        public string Sec_66F_SafetyCommittee_CommitteeRequired_Remarks { get; set; }

        [Display(Name = "66F.2.1 Is safety committee constituted (Attach copy)")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeConstructed_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_66F_SafetyCommittee_CommitteeConstructed_Selection { get; set; }

        [Display(Name = "66F.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeConstructed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_66F_SafetyCommittee_CommitteeConstructed_ViolationExist { get; set; }

        [Display(Name = "66F.2.3 Sec_66F_SafetyCommittee_CommitteeConstructed_Remarks")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeConstructed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeConstructed_Remarks can have max 1000 chars..!")]
        public string Sec_66F_SafetyCommittee_CommitteeConstructed_Remarks { get; set; }

        [Display(Name = "66F.3.1 Is meeting of safety committee held?")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeHeld_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_66F_SafetyCommittee_CommitteeHeld_Selection { get; set; }

        [Display(Name = "66F.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeHeld_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_66F_SafetyCommittee_CommitteeHeld_ViolationExist { get; set; }

        [Display(Name = "66F.3.3 Sec_66F_SafetyCommittee_CommitteeHeld_Remarks")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeHeld_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_66F_SafetyCommittee_CommitteeHeld_Remarks can have max 1000 chars..!")]
        public string Sec_66F_SafetyCommittee_CommitteeHeld_Remarks { get; set; }

        [Display(Name = "66.4.1 Is record of minutes of meetings of safety committee maintained?")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_RecordOfMinutes_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_66F_SafetyCommittee_RecordOfMinutes_Selection { get; set; }

        [Display(Name = "66.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_RecordOfMinutes_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_66F_SafetyCommittee_RecordOfMinutes_ViolationExist { get; set; }

        [Display(Name = "66.4.3 Sec_66F_SafetyCommittee_RecordOfMinutes_Remarks")]
        [Required(ErrorMessage = "Sec_66F_SafetyCommittee_RecordOfMinutes_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_66F_SafetyCommittee_RecordOfMinutes_Remarks can have max 1000 chars..!")]
        public string Sec_66F_SafetyCommittee_RecordOfMinutes_Remarks { get; set; }

        [Display(Name = "Rule-67A   Safety Belts etc.  :67A.1.1  Safety belts and other safety equipment proved as per Rule 67A")]
        [Required(ErrorMessage = "Sec_67A_SafetyBelts_Rule67A_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_67A_SafetyBelts_Rule67A_Selection { get; set; }

        [Display(Name = "67A.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_67A_SafetyBelts_Rule67A_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_67A_SafetyBelts_Rule67A_ViolationExist { get; set; }

        [Display(Name = "67A.1.3 Sec_67A_SafetyBelts_Rule67A_Remarks")]
        [Required(ErrorMessage = "Sec_67A_SafetyBelts_Rule67A_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_67A_SafetyBelts_Rule67A_Remarks can have max 1000 chars..!")]
        public string Sec_67A_SafetyBelts_Rule67A_Remarks { get; set; }

        [Display(Name = "Rule-67C Electrical safety :67C.1.1  Are Provisions of Rule 67C being complied? Submit comments")]
        [Required(ErrorMessage = "Sec_67C_SafetyElecrical_Rule67C_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_67C_SafetyElecrical_Rule67C_Selection { get; set; }

        [Display(Name = "67C.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_67C_SafetyElecrical_Rule67C_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_67C_SafetyElecrical_Rule67C_ViolationExist { get; set; }

        [Display(Name = "67C.1.3 Sec_67C_SafetyElecrical_Rule67C_Remarks")]
        [Required(ErrorMessage = "Sec_67C_SafetyElecrical_Rule67C_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_67C_SafetyElecrical_Rule67C_Remarks can have max 1000 chars..!")]
        public string Sec_67C_SafetyElecrical_Rule67C_Remarks { get; set; }

        [Display(Name = "Rule-67D  Personal protective equipment :67D.1.1 Quality of personal protective equipment certification from Bureau of Indian Standards")]
        [Required(ErrorMessage = "Sec_67D_QualityOfProposal_BoIS_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_67D_QualityOfProposal_BoIS_Selection { get; set; }

        [Display(Name = "67D.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_67D_QualityOfProposal_BoIS_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_67D_QualityOfProposal_BoIS_ViolationExist { get; set; }

        [Display(Name = "67D.1.3 Sec_67D_QualityOfProposal_BoIS_Remarks")]
        [Required(ErrorMessage = "Sec_67D_QualityOfProposal_BoIS_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_67D_QualityOfProposal_BoIS_Remarks can have max 1000 chars..!")]
        public string Sec_67D_QualityOfProposal_BoIS_Remarks { get; set; }

        [Display(Name = "Rule-67F Examination of eyesight of certain workers :67F.1.1  Are eyes of workers, as required under Rule-67F, examined?")]
        [Required(ErrorMessage = "Sec_67F_EyeSight_Rule67F_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_67F_EyeSight_Rule67F_Selection { get; set; }

        [Display(Name = "67F.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_67F_EyeSight_Rule67F_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_67F_EyeSight_Rule67F_ViolationExist { get; set; }

        [Display(Name = "67F.1.3 Sec_67F_EyeSight_Rule67F_Remarks")]
        [Required(ErrorMessage = "Sec_67F_EyeSight_Rule67F_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_67F_EyeSight_Rule67F_Remarks can have max 1000 chars..!")]
        public string Sec_67F_EyeSight_Rule67F_Remarks { get; set; }

        [Display(Name = "67F.2.1 Is record of such examination kept in Form 8-A?")]
        [Required(ErrorMessage = "Sec_67F_EyeSight_Form8A_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_67F_EyeSight_Form8A_Selection { get; set; }

        [Display(Name = "67F.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_67F_EyeSight_Form8A_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_67F_EyeSight_Form8A_ViolationExist { get; set; }

        [Display(Name = "67F.2.3 Sec_67F_EyeSight_Form8A_Remarks")]
        [Required(ErrorMessage = "Sec_67F_EyeSight_Form8A_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_67F_EyeSight_Form8A_Remarks can have max 1000 chars..!")]
        public string Sec_67F_EyeSight_Form8A_Remarks { get; set; }

        [Display(Name = "Rule-67G  67G.1.1 Railways in factory  Are provisions of Rule-67G being complied? Report if applicable")]
        [Required(ErrorMessage = "Sec_67G_RailwaysInFacroty_Rule67G_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_67G_RailwaysInFacroty_Rule67G_Selection { get; set; }

        [Display(Name = "67G.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_67G_RailwaysInFacroty_Rule67G_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_67G_RailwaysInFacroty_Rule67G_ViolationExist { get; set; }

        [Display(Name = "67G.1.3 Sec_67G_RailwaysInFacroty_Rule67G_Remarks")]
        [Required(ErrorMessage = "Sec_67G_RailwaysInFacroty_Rule67G_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_67G_RailwaysInFacroty_Rule67G_Remarks can have max 1000 chars..!")]
        public string Sec_67G_RailwaysInFacroty_Rule67G_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_III_Welfare
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "Section-42 Washing Facilities :42.1.1 Are washing facilities provided?")]
        [Required(ErrorMessage = "Sec_42_WashingFacilities_Provided_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_42_WashingFacilities_Provided_Selection { get; set; }

        [Display(Name = "42.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_42_WashingFacilities_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_42_WashingFacilities_Provided_ViolationExist { get; set; }

        [Display(Name = "42.1.3 Sec_42_WashingFacilities_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_42_WashingFacilities_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_42_WashingFacilities_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_42_WashingFacilities_Provided_Remarks { get; set; }

        [Display(Name = "Section-43 Facilities for storing and drying clothes :43.1.1 Are facilities for storing and drying clothes provided as required under Rule 68A")]
        [Required(ErrorMessage = "Sec_43_StoringAndDrying_Rule68A_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_43_StoringAndDrying_Rule68A_Selection { get; set; }

        [Display(Name = "43.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_43_StoringAndDrying_Rule68A_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_43_StoringAndDrying_Rule68A_ViolationExist { get; set; }

        [Display(Name = "43.1.3 Sec_43_StoringAndDrying_Rule68A_Remarks")]
        [Required(ErrorMessage = "Sec_43_StoringAndDrying_Rule68A_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_43_StoringAndDrying_Rule68A_Remarks can have max 1000 chars..!")]
        public string Sec_43_StoringAndDrying_Rule68A_Remarks { get; set; }

        [Display(Name = "Section-44 Facilities for sitting :44.1.1  Are sitting facilities provided?")]
        [Required(ErrorMessage = "Sec_44_FacilitiesForSitting_Provided_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_44_FacilitiesForSitting_Provided_Selection { get; set; }

        [Display(Name = "44.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_44_FacilitiesForSitting_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_44_FacilitiesForSitting_Provided_ViolationExist { get; set; }

        [Display(Name = "44.1.3 Sec_44_FacilitiesForSitting_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_44_FacilitiesForSitting_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_44_FacilitiesForSitting_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_44_FacilitiesForSitting_Provided_Remarks { get; set; }

        [Display(Name = "Section-45/Rule-69 First-Aid :45.1.1  Are First-Aid boxes maintained as per Rule 69?")]
        [Required(ErrorMessage = "Sec_45_Rule69_FirstAid_Rule69_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_45_Rule69_FirstAid_Rule69_Selection { get; set; }

        [Display(Name = "45.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_45_Rule69_FirstAid_Rule69_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_45_Rule69_FirstAid_Rule69_ViolationExist { get; set; }

        [Display(Name = "45.1.3 Sec_45_Rule69_FirstAid_Rule69_Remarks")]
        [Required(ErrorMessage = "Sec_45_Rule69_FirstAid_Rule69_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_45_Rule69_FirstAid_Rule69_Remarks can have max 1000 chars..!")]
        public string Sec_45_Rule69_FirstAid_Rule69_Remarks { get; set; }

        [Display(Name = "45.2.1 Are notices containing names of persons in charge of First-Aid boxes and name / telephone number of nearest hospital displayed in factory ? ")]
        [Required(ErrorMessage = "Sec_45_Rule69_NoticesContainNameOfPerson_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_45_Rule69_NoticesContainNameOfPerson_Selection { get; set; }

        [Display(Name = "45.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_45_Rule69_NoticesContainNameOfPerson_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_45_Rule69_NoticesContainNameOfPerson_ViolationExist { get; set; }

        [Display(Name = "45.2.3 Sec_45_Rule69_NoticesContainNameOfPerson_Remarks")]
        [Required(ErrorMessage = "Sec_45_Rule69_NoticesContainNameOfPerson_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_45_Rule69_NoticesContainNameOfPerson_Remarks can have max 1000 chars..!")]
        public string Sec_45_Rule69_NoticesContainNameOfPerson_Remarks { get; set; }

        [Display(Name = "45.3.1 Is there any person trained in First-Aid? (attach certificate)")]
        [Required(ErrorMessage = "Sec_45_Rule69_PersonTrainedInFirstAid_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_45_Rule69_PersonTrainedInFirstAid_Selection { get; set; }

        [Display(Name = "45.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_45_Rule69_PersonTrainedInFirstAid_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_45_Rule69_PersonTrainedInFirstAid_ViolationExist { get; set; }

        [Display(Name = "45.3.3 Sec_45_Rule69_PersonTrainedInFirstAid_Remarks")]
        [Required(ErrorMessage = "Sec_45_Rule69_PersonTrainedInFirstAid_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_45_Rule69_PersonTrainedInFirstAid_Remarks can have max 1000 chars..!")]
        public string Sec_45_Rule69_PersonTrainedInFirstAid_Remarks { get; set; }

        [Display(Name = "Section-45/Rule-70 Ambulance Room :45.4.1 Is ambulance room required?")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_Required_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_45_Rule70_AmbulanceRoom_Required_Selection { get; set; }

        [Display(Name = "45.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_Required_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_45_Rule70_AmbulanceRoom_Required_ViolationExist { get; set; }

        [Display(Name = "45.4.3 Sec_45_Rule70_AmbulanceRoom_Required_Remarks")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_Required_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_Required_Remarks can have max 1000 chars..!")]
        public string Sec_45_Rule70_AmbulanceRoom_Required_Remarks { get; set; }

        [Display(Name = "45.5.1 Is ambulance room maintained and equipped as per Rule-70")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_Rule70_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_45_Rule70_AmbulanceRoom_Rule70_Selection { get; set; }

        [Display(Name = "45.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_Rule70_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_45_Rule70_AmbulanceRoom_Rule70_ViolationExist { get; set; }

        [Display(Name = "45.5.3 Sec_45_Rule70_AmbulanceRoom_Rule70_Remarks")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_Rule70_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_Rule70_Remarks can have max 1000 chars..!")]
        public string Sec_45_Rule70_AmbulanceRoom_Rule70_Remarks { get; set; }

        [Display(Name = "45.6.1 Are medical officers appointed as per requirement?")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_MedicalOfficers_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_45_Rule70_AmbulanceRoom_MedicalOfficers_Selection { get; set; }

        [Display(Name = "45.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_MedicalOfficers_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_45_Rule70_AmbulanceRoom_MedicalOfficers_ViolationExist { get; set; }

        [Display(Name = "45.6.3 Sec_45_Rule70_AmbulanceRoom_MedicalOfficers_Remarks")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_MedicalOfficers_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_MedicalOfficers_Remarks can have max 1000 chars..!")]
        public string Sec_45_Rule70_AmbulanceRoom_MedicalOfficers_Remarks { get; set; }

        [Display(Name = "45.7.1 Is para medical staff provided in ambulance room?")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_ParaMedicalStaff_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_45_Rule70_AmbulanceRoom_ParaMedicalStaff_Selection { get; set; }

        [Display(Name = "45.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_ParaMedicalStaff_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_45_Rule70_AmbulanceRoom_ParaMedicalStaff_ViolationExist { get; set; }

        [Display(Name = "45.7.3 Sec_45_Rule70_AmbulanceRoom_ParaMedicalStaff_Remarks")]
        [Required(ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_ParaMedicalStaff_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_45_Rule70_AmbulanceRoom_ParaMedicalStaff_Remarks can have max 1000 chars..!")]
        public string Sec_45_Rule70_AmbulanceRoom_ParaMedicalStaff_Remarks { get; set; }

        [Display(Name = "Section-46/Rules under this (section) Canteen :46.1.1  Is canteen required?")]
        [Required(ErrorMessage = "Sec_46_Canteen_Required_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_46_Canteen_Required_Selection { get; set; }

        [Display(Name = "46.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_46_Canteen_Required_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_46_Canteen_Required_ViolationExist { get; set; }

        [Display(Name = "46.1.3 Sec_46_Canteen_Required_Remarks")]
        [Required(ErrorMessage = "Sec_46_Canteen_Required_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_46_Canteen_Required_Remarks can have max 1000 chars..!")]
        public string Sec_46_Canteen_Required_Remarks { get; set; }

        [Display(Name = "46.2.1 Is canteen maintained and equipped as per Rule-71, 72, 73?")]
        [Required(ErrorMessage = "Sec_46_Canteen_Rule71_72_73_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_46_Canteen_Rule71_72_73_Selection { get; set; }

        [Display(Name = "46.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_46_Canteen_Rule71_72_73_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_46_Canteen_Rule71_72_73_ViolationExist { get; set; }

        [Display(Name = "46.2.3 Sec_46_Canteen_Rule71_72_73_Remarks")]
        [Required(ErrorMessage = "Sec_46_Canteen_Rule71_72_73_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_46_Canteen_Rule71_72_73_Remarks can have max 1000 chars..!")]
        public string Sec_46_Canteen_Rule71_72_73_Remarks { get; set; }

        [Display(Name = "46.3.1 Prices to be charged are approved from canteen committee?")]
        [Required(ErrorMessage = "Sec_46_Canteen_PriceToCharged_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_46_Canteen_PriceToCharged_Selection { get; set; }

        [Display(Name = "46.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_46_Canteen_PriceToCharged_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_46_Canteen_PriceToCharged_ViolationExist { get; set; }

        [Display(Name = "46.3.3 Sec_46_Canteen_PriceToCharged_Remarks")]
        [Required(ErrorMessage = "Sec_46_Canteen_PriceToCharged_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_46_Canteen_PriceToCharged_Remarks can have max 1000 chars..!")]
        public string Sec_46_Canteen_PriceToCharged_Remarks { get; set; }

        [Display(Name = "46.4.1 Are accounts of canteen being maintained?")]
        [Required(ErrorMessage = "Sec_46_Canteen_BeingMaintained_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_46_Canteen_BeingMaintained_Selection { get; set; }

        [Display(Name = "46.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_46_Canteen_BeingMaintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_46_Canteen_BeingMaintained_ViolationExist { get; set; }

        [Display(Name = "46.4.3 Sec_46_Canteen_BeingMaintained_Remarks")]
        [Required(ErrorMessage = "Sec_46_Canteen_BeingMaintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_46_Canteen_BeingMaintained_Remarks can have max 1000 chars..!")]
        public string Sec_46_Canteen_BeingMaintained_Remarks { get; set; }

        [Display(Name = "46.5.1 Is canteen committee constituted? (Attach copy)")]
        [Required(ErrorMessage = "Sec_46_Canteen_CommitteeConstituted_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_46_Canteen_CommitteeConstituted_Selection { get; set; }

        [Display(Name = "46.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_46_Canteen_CommitteeConstituted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_46_Canteen_CommitteeConstituted_ViolationExist { get; set; }

        [Display(Name = "46.5.3 Sec_46_Canteen_CommitteeConstituted_Remarks")]
        [Required(ErrorMessage = "Sec_46_Canteen_CommitteeConstituted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_46_Canteen_CommitteeConstituted_Remarks can have max 1000 chars..!")]
        public string Sec_46_Canteen_CommitteeConstituted_Remarks { get; set; }

        [Display(Name = "46.6.1 Is canteen staff medically examined annually?")]
        [Required(ErrorMessage = "Sec_46_Canteen_ExaminedAnnualy_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_46_Canteen_ExaminedAnnualy_Selection { get; set; }

        [Display(Name = "46.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_46_Canteen_ExaminedAnnualy_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_46_Canteen_ExaminedAnnualy_ViolationExist { get; set; }

        [Display(Name = "46.6.3 Sec_46_Canteen_ExaminedAnnualy_Remarks")]
        [Required(ErrorMessage = "Sec_46_Canteen_ExaminedAnnualy_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_46_Canteen_ExaminedAnnualy_Remarks can have max 1000 chars..!")]
        public string Sec_46_Canteen_ExaminedAnnualy_Remarks { get; set; }

        [Display(Name = "Section-47/Rule-78 Shelter, Rest-Room and Lunch Room :47.1.1 Are shelter/rest-room/lunch room required?")]
        [Required(ErrorMessage = "Sec_47_RestRoom_Required_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_47_RestRoom_Required_Selection { get; set; }

        [Display(Name = "47.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_47_RestRoom_Required_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_47_RestRoom_Required_ViolationExist { get; set; }

        [Display(Name = "47.1.3  Sec_47_RestRoom_Required_Remarks")]
        [Required(ErrorMessage = "Sec_47_RestRoom_Required_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_47_RestRoom_Required_Remarks can have max 1000 chars..!")]
        public string Sec_47_RestRoom_Required_Remarks { get; set; }

        [Display(Name = "47.2.1 Are these as per rule-78?")]
        [Required(ErrorMessage = "Sec_47_RestRoom_Rule78_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_47_RestRoom_Rule78_Selection { get; set; }

        [Display(Name = "47.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_47_RestRoom_Rule78_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_47_RestRoom_Rule78_ViolationExist { get; set; }

        [Display(Name = "47.2.3 Sec_47_RestRoom_Rule78_Remarks")]
        [Required(ErrorMessage = "Sec_47_RestRoom_Rule78_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_47_RestRoom_Rule78_Remarks can have max 1000 chars..!")]
        public string Sec_47_RestRoom_Rule78_Remarks { get; set; }

        [Display(Name = "Section-48/Rule-79 Crech :48.1.1  Is crech required?")]
        [Required(ErrorMessage = "Sec_48_Crech_Required_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_48_Crech_Required_Selection { get; set; }

        [Display(Name = "48.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_48_Crech_Required_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_48_Crech_Required_ViolationExist { get; set; }

        [Display(Name = "48.1.3 Sec_48_Crech_Required_Remarks")]
        [Required(ErrorMessage = "Sec_48_Crech_Required_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_48_Crech_Required_Remarks can have max 1000 chars..!")]
        public string Sec_48_Crech_Required_Remarks { get; set; }

        [Display(Name = "48.2.1 Is crech maintained/equipped as per rule-79?")]
        [Required(ErrorMessage = "Sec_48_Crech_Rule79_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_48_Crech_Rule79_Selection { get; set; }

        [Display(Name = "48.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_48_Crech_Rule79_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_48_Crech_Rule79_ViolationExist { get; set; }

        [Display(Name = "48.2.3 Sec_48_Crech_Rule79_Remarks")]
        [Required(ErrorMessage = "Sec_48_Crech_Rule79_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_48_Crech_Rule79_Remarks can have max 1000 chars..!")]
        public string Sec_48_Crech_Rule79_Remarks { get; set; }

        [Display(Name = "48.3.1 Is washroom provided in crech?")]
        [Required(ErrorMessage = "Sec_48_Crech_WashRoom_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_48_Crech_WashRoom_Selection { get; set; }
        [Display(Name = "48.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_48_Crech_WashRoom_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_48_Crech_WashRoom_ViolationExist { get; set; }

        [Display(Name = "48.3.3 Sec_48_Crech_WashRoom_Remarks")]
        [Required(ErrorMessage = "Sec_48_Crech_WashRoom_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_48_Crech_WashRoom_Remarks can have max 1000 chars..!")]
        public string Sec_48_Crech_WashRoom_Remarks { get; set; }

        [Display(Name = "48.4.1 Is milk and refreshment supplied in crech?")]
        [Required(ErrorMessage = "Sec_48_Crech_MilkAndRefreshment_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_48_Crech_MilkAndRefreshment_Selection { get; set; }

        [Display(Name = "48.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_48_Crech_MilkAndRefreshment_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_48_Crech_MilkAndRefreshment_ViolationExist { get; set; }

        [Display(Name = "48.4.3 Sec_48_Crech_MilkAndRefreshment_Remarks")]
        [Required(ErrorMessage = "Sec_48_Crech_MilkAndRefreshment_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_48_Crech_MilkAndRefreshment_Remarks can have max 1000 chars..!")]
        public string Sec_48_Crech_MilkAndRefreshment_Remarks { get; set; }

        [Display(Name = "48.5.1 Is a break given to the mother for feeding?")]
        [Required(ErrorMessage = "Sec_48_Crech_MotherFeeding_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_48_Crech_MotherFeeding_Selection { get; set; }

        [Display(Name = "48.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_48_Crech_MotherFeeding_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_48_Crech_MotherFeeding_ViolationExist { get; set; }

        [Display(Name = "48.5.3 Sec_48_Crech_MotherFeeding_Remarks")]
        [Required(ErrorMessage = "Sec_48_Crech_MotherFeeding_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_48_Crech_MotherFeeding_Remarks can have max 1000 chars..!")]
        public string Sec_48_Crech_MotherFeeding_Remarks { get; set; }

        [Display(Name = "49.1.1 Number of welfare officer required?")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_Required_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_49_WelfareOfficer_Required_Selection { get; set; }

        [Display(Name = "49.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_Required_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_49_WelfareOfficer_Required_ViolationExist { get; set; }

        [Display(Name = "49.1.3 Sec_49_WelfareOfficer_Required_Remarks")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_Required_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_49_WelfareOfficer_Required_Remarks can have max 1000 chars..!")]
        public string Sec_49_WelfareOfficer_Required_Remarks { get; set; }

        [Display(Name = "49.2.1 Is appointment of welfare officer(s) as per The Punjab WelfareOfficer Rules ? ")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_PWOR_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_49_WelfareOfficer_PWOR_Selection { get; set; }

        [Display(Name = "49.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_PWOR_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_49_WelfareOfficer_PWOR_ViolationExist { get; set; }

        [Display(Name = "49.2.3 Sec_49_WelfareOfficer_PWOR_Remarks")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_PWOR_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_49_WelfareOfficer_PWOR_Remarks can have max 1000 chars..!")]
        public string Sec_49_WelfareOfficer_PWOR_Remarks { get; set; }

        [Display(Name = "49.3.1 Any other remarks ")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_OtherRemarks_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_49_WelfareOfficer_OtherRemarks_Selection { get; set; }

        [Display(Name = "49.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_PWOR_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_49_WelfareOfficer_OtherRemarks_ViolationExist { get; set; }

        [Display(Name = "49.3.3 Remarks by Inspecting Officer")]
        [Required(ErrorMessage = "Sec_49_WelfareOfficer_OtherRemarks_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_49_WelfareOfficer_OtherRemarks_Remarks can have max 1000 chars..!")]
        public string Sec_49_WelfareOfficer_OtherRemarks_Remarks { get; set; }

        [Display(Name = "Id")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_III_General
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "Section-51 Weekly Hours :51.1.1 Are workers working not more than 9 hours in a day or 48 hours in a week?")]
        [Required(ErrorMessage = "Sec_51_WeeklyHours_9HoursInADay_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_51_WeeklyHours_9HoursInADay_Selection { get; set; }

        [Display(Name = "51.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_51_WeeklyHours_9HoursInADay_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_51_WeeklyHours_9HoursInADay_ViolationExist { get; set; }

        [Display(Name = "51.1.3 Sec_51_WeeklyHours_9HoursInADay_Remarks")]
        [Required(ErrorMessage = "Sec_51_WeeklyHours_9HoursInADay_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_51_WeeklyHours_9HoursInADay_Remarks can have max 1000 chars..!")]
        public string Sec_51_WeeklyHours_9HoursInADay_Remarks { get; set; }

        [Display(Name = "Section-52  Weekly Holidays :52.1.1 Is weekly holiday given to worker(s) on first day of week ? ")]
        [Required(ErrorMessage = "Sec_52_WeeklyHoliday_FirstDayOfWeek_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_52_WeeklyHoliday_FirstDayOfWeek_Selection { get; set; }

        [Display(Name = "52.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_52_WeeklyHoliday_FirstDayOfWeek_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_52_WeeklyHoliday_FirstDayOfWeek_ViolationExist { get; set; }

        [Display(Name = "52.1.3 Sec_52_WeeklyHoliday_FirstDayOfWeek_Remarks")]
        [Required(ErrorMessage = "Sec_52_WeeklyHoliday_FirstDayOfWeek_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_52_WeeklyHoliday_FirstDayOfWeek_Remarks can have max 1000 chars..!")]
        public string Sec_52_WeeklyHoliday_FirstDayOfWeek_Remarks { get; set; }

        [Display(Name = "52.2.1 If not Is required notice given by factory manager and such notice is displayed in factory ? ")]
        [Required(ErrorMessage = "Sec_52_WeeklyHoliday_FactoryManager_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_52_WeeklyHoliday_FactoryManager_Selection { get; set; }

        [Display(Name = "52.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_52_WeeklyHoliday_FactoryManager_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_52_WeeklyHoliday_FactoryManager_ViolationExist { get; set; }

        [Display(Name = "52.2.3 Sec_52_WeeklyHoliday_FactoryManager_Remarks")]
        [Required(ErrorMessage = "Sec_52_WeeklyHoliday_FactoryManager_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_52_WeeklyHoliday_FactoryManager_Remarks can have max 1000 chars..!")]
        public string Sec_52_WeeklyHoliday_FactoryManager_Remarks { get; set; }

        [Display(Name = "Section-53 Compensatory Holidays :53.1.1 Is compensatory holidays register maintained?")]
        [Required(ErrorMessage = "Sec_53_CompensatoryHoliday_RegisterMaintained_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_53_CompensatoryHoliday_RegisterMaintained_Selection { get; set; }

        [Display(Name = "53.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_53_CompensatoryHoliday_RegisterMaintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_53_CompensatoryHoliday_RegisterMaintained_ViolationExist { get; set; }

        [Display(Name = "53.1.3 Sec_53_CompensatoryHoliday_RegisterMaintained_Remarks")]
        [Required(ErrorMessage = "Sec_53_CompensatoryHoliday_RegisterMaintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_53_CompensatoryHoliday_RegisterMaintained_Remarks can have max 1000 chars..!")]
        public string Sec_53_CompensatoryHoliday_RegisterMaintained_Remarks { get; set; }

        [Display(Name = "Section-54 :54.1.1 Daily Hours Are provisions of section 54 being compiled ? ")]
        [Required(ErrorMessage = "Sec_54_DailyHours_Sec54_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_54_DailyHours_Sec54_Selection { get; set; }

        [Display(Name = "54.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_54_DailyHours_Sec54_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_54_DailyHours_Sec54_ViolationExist { get; set; }

        [Display(Name = "54.1.3 Sec_54_DailyHours_Sec54_Remarks")]
        [Required(ErrorMessage = "Sec_54_DailyHours_Sec54_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_54_DailyHours_Sec54_Remarks can have max 1000 chars..!")]
        public string Sec_54_DailyHours_Sec54_Remarks { get; set; }

        [Display(Name = "Sectionn-55: 55.1.1  Interval of rest Are provisions of section 55 being compiled ? ")]
        [Required(ErrorMessage = "Sec_55_IntervalOfRest_Sec55_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_55_IntervalOfRest_Sec55_Selection { get; set; }

        [Display(Name = "55.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_55_IntervalOfRest_Sec55_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_55_IntervalOfRest_Sec55_ViolationExist { get; set; }

        [Display(Name = "55.1.3 Sec_55_IntervalOfRest_Sec55_Remarks")]
        [Required(ErrorMessage = "Sec_55_IntervalOfRest_Sec55_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_55_IntervalOfRest_Sec55_Remarks can have max 1000 chars..!")]
        public string Sec_55_IntervalOfRest_Sec55_Remarks { get; set; }

        [Display(Name = "Section-56 :56.1.1 Spread over Are provisions of section 56 being compiled ? ")]
        [Required(ErrorMessage = "Sec_56_SpreadOver_Sec56_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_56_SpreadOver_Sec56_Selection { get; set; }

        [Display(Name = "56.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_56_SpreadOver_Sec56_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_56_SpreadOver_Sec56_ViolationExist { get; set; }

        [Display(Name = "56.1.3 Sec_56_SpreadOver_Sec56_Remarks")]
        [Required(ErrorMessage = "Sec_56_SpreadOver_Sec56_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_56_SpreadOver_Sec56_Remarks can have max 1000 chars..!")]
        public string Sec_56_SpreadOver_Sec56_Remarks { get; set; }

        [Display(Name = "Section-57: 57.1.1 Are provisions of section 57 being compiled ? ")]
        [Required(ErrorMessage = "Sec_57_NightShifts_Sec57_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_57_NightShifts_Sec57_Selection { get; set; }

        [Display(Name = "57.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_57_NightShifts_Sec57_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_57_NightShifts_Sec57_ViolationExist { get; set; }

        [Display(Name = "57.1.3 Sec_57_NightShifts_Sec57_Remarks")]
        [Required(ErrorMessage = "Sec_57_NightShifts_Sec57_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_57_NightShifts_Sec57_Remarks can have max 1000 chars..!")]
        public string Sec_57_NightShifts_Sec57_Remarks { get; set; }

        [Display(Name = "Section-58 58.1.1 Overlapping of shifts Are provisions of section 58 being complied ? ")]
        [Required(ErrorMessage = "Sec_58_OverlappingShifts_Sec58_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_58_OverlappingShifts_Sec58_Selection { get; set; }

        [Display(Name = "58.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_58_OverlappingShifts_Sec58_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_58_OverlappingShifts_Sec58_ViolationExist { get; set; }

        [Display(Name = "58.1.3 Sec_58_OverlappingShifts_Sec58_Remarks")]
        [Required(ErrorMessage = "Sec_58_OverlappingShifts_Sec58_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_58_OverlappingShifts_Sec58_Remarks can have max 1000 chars..!")]
        public string Sec_58_OverlappingShifts_Sec58_Remarks { get; set; }

        [Display(Name = "Section-59 59.1.1 Extra wages for over time Is register prescribed under Rule 85 maintained ? Are payments of over - time being given as per section 59 ? ")]
        [Required(ErrorMessage = "Sec_59_ExtraWages_Sec85_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_59_ExtraWages_Sec85_Selection { get; set; }

        [Display(Name = "59.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_59_ExtraWages_Sec85_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_59_ExtraWages_Sec85_ViolationExist { get; set; }

        [Display(Name = "59.1.3 Sec_59_ExtraWages_Sec85_Remarks")]
        [Required(ErrorMessage = "Sec_59_ExtraWages_Sec85_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_59_ExtraWages_Sec85_Remarks can have max 1000 chars..!")]
        public string Sec_59_ExtraWages_Sec85_Remarks { get; set; }

        [Display(Name = "59.2.1 Are payments of over-time being given as per section 59?")]
        [Required(ErrorMessage = "Sec_59_ExtraWages_Sec59_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_59_ExtraWages_Sec59_Selection { get; set; }

        [Display(Name = "59.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_59_ExtraWages_Sec59_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_59_ExtraWages_Sec59_ViolationExist { get; set; }

        [Display(Name = "59.2.3 IdSec_59_ExtraWages_Sec59_Remarks")]
        [Required(ErrorMessage = "Sec_59_ExtraWages_Sec59_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_59_ExtraWages_Sec59_Remarks can have max 1000 chars..!")]
        public string Sec_59_ExtraWages_Sec59_Remarks { get; set; }

        [Display(Name = "Section-60 Double empoyemnt 60.1.1 Are provisions of section 60 being compiled ? ")]
        [Required(ErrorMessage = "Sec_60_DoubleEmployment_Sec60_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_60_DoubleEmployment_Sec60_Selection { get; set; }

        [Display(Name = "60.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_60_DoubleEmployment_Sec60_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_60_DoubleEmployment_Sec60_ViolationExist { get; set; }

        [Display(Name = "60.1.3 Sec_60_DoubleEmployment_Sec60_Remarks")]
        [Required(ErrorMessage = "Sec_60_DoubleEmployment_Sec60_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_60_DoubleEmployment_Sec60_Remarks can have max 1000 chars..!")]
        public string Sec_60_DoubleEmployment_Sec60_Remarks { get; set; }

        [Display(Name = "Section-61 61.1.1 Notice of period Is notice of period displayed ? ")]
        [Required(ErrorMessage = "Sec_61_NoticeOfPeriod_Displayed_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_61_NoticeOfPeriod_Displayed_Selection { get; set; }

        [Display(Name = "61.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_61_NoticeOfPeriod_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_61_NoticeOfPeriod_Displayed_ViolationExist { get; set; }

        [Display(Name = "61.1.3 Sec_61_NoticeOfPeriod_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_61_NoticeOfPeriod_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_61_NoticeOfPeriod_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_61_NoticeOfPeriod_Displayed_Remarks { get; set; }

        [Display(Name = "61.2.1 Is register of Adult Workers maintained?")]
        [Required(ErrorMessage = "Sec_61_NoticeOfPeriod_RegiterOfAudit_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_61_NoticeOfPeriod_RegiterOfAudit_Selection { get; set; }

        [Display(Name = "61.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_61_NoticeOfPeriod_RegiterOfAudit_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_61_NoticeOfPeriod_RegiterOfAudit_ViolationExist { get; set; }

        [Display(Name = "61.2.3 Sec_61_NoticeOfPeriod_RegiterOfAudit_Remarks")]
        [Required(ErrorMessage = "Sec_61_NoticeOfPeriod_RegiterOfAudit_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_61_NoticeOfPeriod_RegiterOfAudit_Remarks can have max 1000 chars..!")]
        public string Sec_61_NoticeOfPeriod_RegiterOfAudit_Remarks { get; set; }

        [Display(Name = "Section-66  Employment of women in night shifts 66.1.1 Are women employed in night shifts ? ")]
        [Required(ErrorMessage = "Sec_66_WomenInNightShift_Employed_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_66_WomenInNightShift_Employed_Selection { get; set; }

        [Display(Name = "66.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_66_WomenInNightShift_Employed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_66_WomenInNightShift_Employed_ViolationExist { get; set; }

        [Display(Name = "66.1.3 Sec_66_WomenInNightShift_Employed_Remarks")]
        [Required(ErrorMessage = "Sec_66_WomenInNightShift_Employed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_66_WomenInNightShift_Employed_Remarks can have max 1000 chars..!")]
        public string Sec_66_WomenInNightShift_Employed_Remarks { get; set; }

        [Display(Name = "66.2.1 Is approval to employ women in night shifts taken? (Attach copy)")]
        [Required(ErrorMessage = "Sec_66_WomenInNightShift_Approval_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_66_WomenInNightShift_Approval_Selection { get; set; }

        [Display(Name = "66.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_66_WomenInNightShift_Approval_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_66_WomenInNightShift_Approval_ViolationExist { get; set; }

        [Display(Name = "66.2.3 Sec_66_WomenInNightShift_Approval_Remarks")]
        [Required(ErrorMessage = "Sec_66_WomenInNightShift_Approval_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_66_WomenInNightShift_Approval_Remarks can have max 1000 chars..!")]
        public string Sec_66_WomenInNightShift_Approval_Remarks { get; set; }

        [Display(Name = "66.3.1 Employment of Young person Are non - adult workers given token ? ")]
        [Required(ErrorMessage = "YoungPerson_NonAuditWorkers_Selection is required..!")]
        public GeneralOptionTypeEnum YoungPerson_NonAuditWorkers_Selection { get; set; }

        [Display(Name = "66.3.2 Is Violation Found")]
        [Required(ErrorMessage = "YoungPerson_NonAuditWorkers_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum YoungPerson_NonAuditWorkers_ViolationExist { get; set; }

        [Display(Name = "66.3.3 YoungPerson_NonAuditWorkers_Remarks")]
        [Required(ErrorMessage = "YoungPerson_NonAuditWorkers_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "YoungPerson_NonAuditWorkers_Remarks can have max 1000 chars..!")]
        public string YoungPerson_NonAuditWorkers_Remarks { get; set; }

        [Display(Name = "66.4.1  Are young persons employed have certificate of fitness and is it valid?")]
        [Required(ErrorMessage = "YoungPerson_CertificateOfFitness_Selection is required..!")]
        public GeneralOptionTypeEnum YoungPerson_CertificateOfFitness_Selection { get; set; }

        [Display(Name = "66.4.2 Is Violation Found")]
        [Required(ErrorMessage = "YoungPerson_CertificateOfFitness_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum YoungPerson_CertificateOfFitness_ViolationExist { get; set; }

        [Display(Name = "66.4.3 YoungPerson_CertificateOfFitness_Remarks")]
        [Required(ErrorMessage = "YoungPerson_CertificateOfFitness_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "YoungPerson_CertificateOfFitness_Remarks can have max 1000 chars..!")]
        public string YoungPerson_CertificateOfFitness_Remarks { get; set; }

        [Display(Name = "66.5.1  Are provisions of section 71 and Rule 92 being complied?")]
        [Required(ErrorMessage = "YoungPerson_Sec71Rule92_Selection is required..!")]
        public GeneralOptionTypeEnum YoungPerson_Sec71Rule92_Selection { get; set; }

        [Display(Name = "66.5.2 Is Violation Found")]
        [Required(ErrorMessage = "YoungPerson_Sec71Rule92_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum YoungPerson_Sec71Rule92_ViolationExist { get; set; }

        [Display(Name = "66.5.3 YoungPerson_Sec71Rule92_Remarks")]
        [Required(ErrorMessage = "YoungPerson_Sec71Rule92_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "YoungPerson_Sec71Rule92_Remarks can have max 1000 chars..!")]
        public string YoungPerson_Sec71Rule92_Remarks { get; set; }

        [Display(Name = "66.6.1 Is register of child workers maintained as per Rule-93?")]
        [Required(ErrorMessage = "YoungPerson_Rule93_Selection is required..!")]
        public GeneralOptionTypeEnum YoungPerson_Rule93_Selection { get; set; }

        [Display(Name = "66.6.2 Is Violation Found")]
        [Required(ErrorMessage = "YoungPerson_Rule93_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum YoungPerson_Rule93_ViolationExist { get; set; }

        [Display(Name = "66.6.3 YoungPerson_Rule93_Remarks")]
        [Required(ErrorMessage = "YoungPerson_Rule93_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "YoungPerson_Rule93_Remarks can have max 1000 chars..!")]
        public string YoungPerson_Rule93_Remarks { get; set; }

        [Display(Name = "66.7.1 Is there any contravention of The Child Labour And Adolescent Act 1986 as amended in 2016?")]
        [Required(ErrorMessage = "YoungPerson_CLAAct1986_Selection is required..!")]
        public GeneralOptionTypeEnum YoungPerson_CLAAct1986_Selection { get; set; }

        [Display(Name = "66.7.2 Is Violation Found")]
        [Required(ErrorMessage = "YoungPerson_CLAAct1986_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum YoungPerson_CLAAct1986_ViolationExist { get; set; }

        [Display(Name = "66.7.3 YoungPerson_CLAAct1986_Remarks")]
        [Required(ErrorMessage = "YoungPerson_CLAAct1986_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "YoungPerson_CLAAct1986_Remarks can have max 1000 chars..!")]
        public string YoungPerson_CLAAct1986_Remarks { get; set; }

        [Display(Name = "Section-79 Leave with wages 79.1.1 Is Leave With Wages Register being maintained ? ")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_RegisterMaintained_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_79_LeaveAndWages_RegisterMaintained_Selection { get; set; }

        [Display(Name = "79.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_RegisterMaintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_79_LeaveAndWages_RegisterMaintained_ViolationExist { get; set; }

        [Display(Name = "79.1.3 Sec_79_LeaveAndWages_RegisterMaintained_Remarks")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_RegisterMaintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_79_LeaveAndWages_RegisterMaintained_Remarks can have max 1000 chars..!")]
        public string Sec_79_LeaveAndWages_RegisterMaintained_Remarks { get; set; }

        [Display(Name = "79.2.1 Are leave books provided to workers?")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_LeaveBooks_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_79_LeaveAndWages_LeaveBooks_Selection { get; set; }

        [Display(Name = "79.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_LeaveBooks_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_79_LeaveAndWages_LeaveBooks_ViolationExist { get; set; }

        [Display(Name = "79.2.3 Sec_79_LeaveAndWages_LeaveBooks_Remarks")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_LeaveBooks_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_79_LeaveAndWages_LeaveBooks_Remarks can have max 1000 chars..!")]
        public string Sec_79_LeaveAndWages_LeaveBooks_Remarks { get; set; }

        [Display(Name = "79.3.1 Is nomination in Form-D taken?")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_FormD_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_79_LeaveAndWages_FormD_Selection { get; set; }

        [Display(Name = "79.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_FormD_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_79_LeaveAndWages_FormD_ViolationExist { get; set; }

        [Display(Name = "79.3.3 Sec_79_LeaveAndWages_FormD_Remarks")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_FormD_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_79_LeaveAndWages_FormD_Remarks can have max 1000 chars..!")]
        public string Sec_79_LeaveAndWages_FormD_Remarks { get; set; }

        [Display(Name = "79.4.1 Any other report?")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_AnyOtherReport_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_79_LeaveAndWages_AnyOtherReport_Selection { get; set; }

        [Display(Name = "79.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_AnyOtherReport_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_79_LeaveAndWages_AnyOtherReport_ViolationExist { get; set; }

        [Display(Name = "79.4.3 Sec_79_LeaveAndWages_AnyOtherReport_Remarks")]
        [Required(ErrorMessage = "Sec_79_LeaveAndWages_AnyOtherReport_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_79_LeaveAndWages_AnyOtherReport_Remarks can have max 1000 chars..!")]
        public string Sec_79_LeaveAndWages_AnyOtherReport_Remarks { get; set; }

        [Display(Name = "88.1.1 Section-88-89 Accident / Disease Notices  Is Accident and Dangerous Register being maintained ? ")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_RegisterMaintained_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_88_89_AccidentNotice_RegisterMaintained_Selection { get; set; }

        [Display(Name = "88.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_RegisterMaintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_88_89_AccidentNotice_RegisterMaintained_ViolationExist { get; set; }

        [Display(Name = "88.1.3 Sec_88_89_AccidentNotice_RegisterMaintained_Remarks")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_RegisterMaintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_88_89_AccidentNotice_RegisterMaintained_Remarks can have max 1000 chars..!")]
        public string Sec_88_89_AccidentNotice_RegisterMaintained_Remarks { get; set; }

        [Display(Name = "88.2.1 Is accident or dangerous occurrence reported within prescribed time?")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_PrescribedTime_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_88_89_AccidentNotice_PrescribedTime_Selection { get; set; }

        [Display(Name = "88.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_PrescribedTime_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_88_89_AccidentNotice_PrescribedTime_ViolationExist { get; set; }

        [Display(Name = "88.2.3 Sec_88_89_AccidentNotice_PrescribedTime_Remarks")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_PrescribedTime_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_88_89_AccidentNotice_PrescribedTime_Remarks can have max 1000 chars..!")]
        public string Sec_88_89_AccidentNotice_PrescribedTime_Remarks { get; set; }

        [Display(Name = "88.3.1 Is notice of poison or disease sent as per Rule-104?")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_Rule104_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_88_89_AccidentNotice_Rule104_Selection { get; set; }

        [Display(Name = "88.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_Rule104_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_88_89_AccidentNotice_Rule104_ViolationExist { get; set; }

        [Display(Name = "88.3.3 Sec_88_89_AccidentNotice_Rule104_Remarks")]
        [Required(ErrorMessage = "Sec_88_89_AccidentNotice_Rule104_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_88_89_AccidentNotice_Rule104_Remarks can have max 1000 chars..!")]
        public string Sec_88_89_AccidentNotice_Rule104_Remarks { get; set; }

        [Display(Name = "Rule-104A Permissible levels of certain chemicals in work environment 104.1.1 Is level of certain chemicals within limit as prescribed in Rule - 104A")]
        [Required(ErrorMessage = "Rule104A_PermissibleLevels_Rule104A_Selection is required..!")]
        public GeneralOptionTypeEnum Rule104A_PermissibleLevels_Rule104A_Selection { get; set; }

        [Display(Name = "104.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule104A_PermissibleLevels_Rule104A_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule104A_PermissibleLevels_Rule104A_ViolationExist { get; set; }

        [Display(Name = "104.1.3 Rule104A_PermissibleLevels_Rule104A_Remarks")]
        [Required(ErrorMessage = "Rule104A_PermissibleLevels_Rule104A_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule104A_PermissibleLevels_Rule104A_Remarks can have max 1000 chars..!")]
        public string Rule104A_PermissibleLevels_Rule104A_Remarks { get; set; }

        [Display(Name = "Section-108 Notices 108.1.1 Are abstracts of Factories Act 1948 and Punjab Factory Rules 1952 displayed as per Section 108")]
        [Required(ErrorMessage = "Sec_108_Notices_FactoryActRules_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_108_Notices_FactoryActRules_Selection { get; set; }

        [Display(Name = "108.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_108_Notices_FactoryActRules_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_108_Notices_FactoryActRules_ViolationExist { get; set; }

        [Display(Name = "108.1.3 Sec_108_Notices_FactoryActRules_Remarks")]
        [Required(ErrorMessage = "Sec_108_Notices_FactoryActRules_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_108_Notices_FactoryActRules_Remarks can have max 1000 chars..!")]
        public string Sec_108_Notices_FactoryActRules_Remarks { get; set; }

        [Display(Name = "108.2.1 Are names of Inspector and Certifying Surgeon under Factories Act 1948 displayed")]
        [Required(ErrorMessage = "Sec_108_Notices_Surgeon_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_108_Notices_Surgeon_Selection { get; set; }

        [Display(Name = "108.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_108_Notices_Surgeon_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_108_Notices_Surgeon_ViolationExist { get; set; }

        [Display(Name = "108.2.3 Sec_108_Notices_Surgeon_Remarks")]
        [Required(ErrorMessage = "Sec_108_Notices_Surgeon_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_108_Notices_Surgeon_Remarks can have max 1000 chars..!")]
        public string Sec_108_Notices_Surgeon_Remarks { get; set; }

        [Display(Name = "Section-110 Returns 110.1.1 Are Annual Returns up to last calendar year submitted")]
        [Required(ErrorMessage = "Sec_110_Returns_LastYearSubmitted_Selection is required..!")]
        public GeneralOptionTypeEnum Sec_110_Returns_LastYearSubmitted_Selection { get; set; }

        [Display(Name = "110.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_110_Returns_LastYearSubmitted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_110_Returns_LastYearSubmitted_ViolationExist { get; set; }

        [Display(Name = "110.1.3 Sec_110_Returns_LastYearSubmitted_Remarks")]
        [Required(ErrorMessage = "Sec_110_Returns_LastYearSubmitted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_110_Returns_LastYearSubmitted_Remarks can have max 1000 chars..!")]
        public string Sec_110_Returns_LastYearSubmitted_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_III_MajorAccidentHazard
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "Is factory covered under 2(cb) or Major Accident?")]
        [Required(ErrorMessage = "IsFactoryCoverdUnderAccident is required..!")]
        public bool IsFactoryCoverdUnderAccident { get; set; }

        [Display(Name = "A A.1.1 Hazardous process")]
        [Required(ErrorMessage = "HazardousProcess is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "HazardousProcess can have max 1000 chars..!")]
        public string HazardousProcess { get; set; }

        [Display(Name = "A.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "HazardousProcess_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum HazardousProcess_ViolationExist { get; set; }

        [Display(Name = "B B.1.1  Types of hazardous chemical substances Manufactured, Stored, Usedand Handled their quantities with the storage capacity, please specify the names of the chemicals with their technical and commercial names.")]
        [Required(ErrorMessage = "TypesOfHazarousChemical is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "TypesOfHazarousChemical can have max 1000 chars..!")]
        public string TypesOfHazarousChemical { get; set; }

        [Display(Name = "B.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "TypesOfHazarousChemical_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum TypesOfHazarousChemical_ViolationExist { get; set; }

        [Display(Name = "C C.1.1 Method of storage of hazardous chemicals(specify whether in tank form, above ground level or below ground level, in bullets, in spheres, barrels etc.).")]
        [Required(ErrorMessage = "MethodOfStorage is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "MethodOfStorage can have max 1000 chars..!")]
        public string MethodOfStorage { get; set; }

        [Display(Name = "C.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "MethodOfStorage_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum MethodOfStorage_ViolationExist { get; set; }

        [Display(Name = "D D.1.1 Whether the On - site Emergency Plans have been prepared and approved by the Director of Factories.If so, specify the number and date.")]
        [Required(ErrorMessage = "OnSiteEmergencyPlansPrepared is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "OnSiteEmergencyPlansPrepared can have max 1000 chars..!")]
        public string OnSiteEmergencyPlansPrepared { get; set; }

        [Display(Name = "D.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "OnSiteEmergencyPlansPrepared_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OnSiteEmergencyPlansPrepared_ViolationExist { get; set; }

        [Display(Name = "E E.1.1 Whether the On - site Emergency Plans approved have been revised, if so please specify the last date of such revision and approval and specify the reason for such revision and non - revision.")]
        [Required(ErrorMessage = "OnSiteEmergencyPlansApproved is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "OnSiteEmergencyPlansApproved can have max 1000 chars..!")]
        public string OnSiteEmergencyPlansApproved { get; set; }

        [Display(Name = "E.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "OnSiteEmergencyPlansApproved_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OnSiteEmergencyPlansApproved_ViolationExist { get; set; }

        [Display(Name = "F F.1.1 Whether the Health and Safety Policy of the factory drawn up and notified.")]
        [Required(ErrorMessage = "HealthAndSafetyPolicy is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "HealthAndSafetyPolicy can have max 1000 chars..!")]
        public string HealthAndSafetyPolicy { get; set; }

        [Display(Name = "F.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "HealthAndSafetyPolicy_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum HealthAndSafetyPolicy_ViolationExist { get; set; }

        [Display(Name = "G G.1.1 Whether decontamination facility is required, if so details / types of such facilities required / provided in detail.")]
        [Required(ErrorMessage = "DecontaminationFacility is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "DecontaminationFacility can have max 1000 chars..!")]
        public string DecontaminationFacility { get; set; }

        [Display(Name = "G.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "DecontaminationFacility_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum DecontaminationFacility_ViolationExist { get; set; }

        [Display(Name = "H H.1.1 Whether the MSDS are prepared in respect of all the chemical substances, manufactured, stored, used and handled in the industry.If so, collect the sample copy for each chemical.")]
        [Required(ErrorMessage = "MsdsPrepared is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "MsdsPrepared can have max 1000 chars..!")]
        public string MsdsPrepared { get; set; }

        [Display(Name = "H.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "MsdsPrepared_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum MsdsPrepared_ViolationExist { get; set; }

        [Display(Name = "I I.1.1 Is the safety audit, risk analysis and HAZOP studies have been conducted ? If so Indicate: -")]
        [Required(ErrorMessage = "HazopStudies_Selection is required..!")]
        public GeneralOptionTypeEnum HazopStudies_Selection { get; set; }

        [Display(Name = "I.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "HazopStudies_Selection_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum HazopStudies_Selection_ViolationExist { get; set; }

        [Display(Name = "I.2.1 Agency which conducted such studies.")]
        [Required(ErrorMessage = "AgencyConductedStudies_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "AgencyConductedStudies_Remarks can have max 1000 chars..!")]
        public string AgencyConductedStudies_Remarks { get; set; }

        [Display(Name = "I.2.2 Is Violation Found?")]
        [Required(ErrorMessage = "AgencyConductedStudies_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum AgencyConductedStudies_ViolationExist { get; set; }

        [Display(Name = "I.3.1 The frequency at which they are conducted.")]
        [Required(ErrorMessage = "FrequencyConductedStudies_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "FrequencyConductedStudies_Remarks can have max 1000 chars..!")]
        public string FrequencyConductedStudies_Remarks { get; set; }

        [Display(Name = "I.3.2 Is Violation Found?")]
        [Required(ErrorMessage = "AgencyConductedStudies_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum FrequencyConductedStudies_ViolationExist { get; set; }

        [Display(Name = "I.4.1 The date on which the last such study was conducted Important findings of the study.")]
        [Required(ErrorMessage = "DateOfLastStudyConducted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "DateOfLastStudyConducted_Remarks can have max 1000 chars..!")]
        public string DateOfLastStudyConducted_Remarks { get; set; }

        [Display(Name = "I.4.2 Is Violation Found?")]
        [Required(ErrorMessage = "DateOfLastStudyConducted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum DateOfLastStudyConducted_ViolationExist { get; set; }

        [Display(Name = "I.5.1 Whether such reports have been submitted to the authorities.")]
        [Required(ErrorMessage = "ReportSubmittedAuthorities_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "ReportSubmittedAuthorities_Remarks can have max 1000 chars..!")]
        public string ReportSubmittedAuthorities_Remarks { get; set; }

        [Display(Name = "I.5.2 Is Violation Found?")]
        [Required(ErrorMessage = "DateOfLastStudyConducted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum ReportSubmittedAuthorities_ViolationExist { get; set; }

        [Display(Name = "J J.1.1 Do they have a system of in house and external training programmes on safety and health ? ")]
        [Required(ErrorMessage = "InternalExternalTrainingProg is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "InternalExternalTrainingProg can have max 1000 chars..!")]
        public string InternalExternalTrainingProg { get; set; }

        [Display(Name = "J.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "InternalExternalTrainingProg_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum InternalExternalTrainingProg_ViolationExist { get; set; }

        [Display(Name = "K K.1.1 How frequently the on - site emergency mock - drills are conducted ? How many such drills are conducted in the presence of Departmental officers of Factories ? (details thereof)")]
        [Required(ErrorMessage = "OnSiteEmergencyMockDrills is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "OnSiteEmergencyMockDrills can have max 1000 chars..!")]
        public string OnSiteEmergencyMockDrills { get; set; }

        [Display(Name = "K.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "OnSiteEmergencyMockDrills_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OnSiteEmergencyMockDrills_ViolationExist { get; set; }

        [Display(Name = "L L.1.1 Whether all the storage vessels, process, equipment, reaction vessels, kettles, ovens, mountings etc., are tested periodically to ensure their integrity and soundness ? If so, indicate the following details.")]
        [Required(ErrorMessage = "IntegritySoundless_Selection is required..!")]
        public GeneralOptionTypeEnum IntegritySoundless_Selection { get; set; }

        [Display(Name = "L.1.2 The person/agency conducting such tests and examinations.")]
        [Required(ErrorMessage = "IntegritySoundless_AgencyConductedStudies_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "IntegritySoundless_AgencyConductedStudies_Remarks can have max 1000 chars..!")]
        public string IntegritySoundless_AgencyConductedStudies_Remarks { get; set; }

        [Display(Name = "L.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "IntegritySoundless_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum IntegritySoundless_AgencyConductedStudies_ViolationExist { get; set; }

        [Display(Name = "L.2.1 The frequency at which they are conducted.")]
        [Required(ErrorMessage = "IntegritySoundless_FrequencyConductedStudies_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "IntegritySoundless_FrequencyConductedStudies_Remarks can have max 1000 chars..!")]
        public string IntegritySoundless_FrequencyConductedStudies_Remarks { get; set; }

        [Display(Name = "L.2.2 Is Violation Found?")]
        [Required(ErrorMessage = "IntegritySoundless_FrequencyConductedStudies_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum IntegritySoundless_FrequencyConductedStudies_ViolationExist { get; set; }

        [Display(Name = "L.3.1 The data on which the last such study was conducted.")]
        [Required(ErrorMessage = "IntegritySoundless_DateOfLastStudyConducted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "IntegritySoundless_DateOfLastStudyConducted_Remarks can have max 1000 chars..!")]
        public string IntegritySoundless_DateOfLastStudyConducted_Remarks { get; set; }

        [Display(Name = "L.3.2 Is Violation Found?")]
        [Required(ErrorMessage = "IntegritySoundless_DateOfLastStudyConducted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum IntegritySoundless_DateOfLastStudyConducted_ViolationExist { get; set; }

        [Display(Name = "L.4.1 Furnish the critical findings noticed during such tests and examinations and the corrective actions.")]
        [Required(ErrorMessage = "IntegritySoundless_CriticalFindingsNoticed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "IntegritySoundless_CriticalFindingsNoticed_Remarks can have max 1000 chars..!")]
        public string IntegritySoundless_CriticalFindingsNoticed_Remarks { get; set; }

        [Display(Name = "L.4.2 Is Violation Found?")]
        [Required(ErrorMessage = "IntegritySoundless_DateOfLastStudyConducted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum IntegritySoundless_CriticalFindingsNoticed_ViolationExist { get; set; }

        [Display(Name = "M M.1.1 Are there any Major Accidents in the last three years ? If so, the details.")]
        [Required(ErrorMessage = "MajorAccidentsInLast3Years is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "MajorAccidentsInLast3Years can have max 1000 chars..!")]
        public string MajorAccidentsInLast3Years { get; set; }

        [Display(Name = "M.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "MajorAccidentsInLast3Years_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum MajorAccidentsInLast3Years_ViolationExist { get; set; }

        [Display(Name = "N N.1.1 Whether the occupational Health Center is provided ? If so, ")]
        [Required(ErrorMessage = "OccupationalHealthCenterProvided_Selection is required..!")]
        public GeneralOptionTypeEnum OHCProvided_Selection { get; set; }

        [Display(Name = "N.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "OHCProvided_Selection_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OHCProvided_Selection_ViolationExist { get; set; }

        [Display(Name = "N.2.1 The area (size) of the OHC along with details of infrastructure facilities available.Please support it by furnishing a ground plan of the OHC.")]
        [Required(ErrorMessage = "OccupationalHealthCenterArea is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "OccupationalHealthCenterArea can have max 1000 chars..!")]
        public string OHCArea { get; set; }

        [Display(Name = "N.2.2 Is Violation Found?")]
        [Required(ErrorMessage = "OHCArea_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OHCArea_ViolationExist { get; set; }

        [Display(Name = "N.3.1 The date on which factory medical officer was appointed.")]
        [Required(ErrorMessage = "OccupationalHealthCenter_DateOfOfficerAppointed is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "OccupationalHealthCenter_DateOfOfficerAppointed can have max 1000 chars..!")]
        public string OHC_DateOfOfficerAppointed { get; set; }

        [Display(Name = "N.3.2 Is Violation Found?")]
        [Required(ErrorMessage = "OHC_DateOfOfficerAppointed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OHC_DateOfOfficerAppointed_ViolationExist { get; set; }

        [Display(Name = "N.4.1 The details of Qualification, etc, of the medical officer so appointed.")]
        [Required(ErrorMessage = "OHC_OfficerQualification is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "OHC_OfficerQualification can have max 1000 chars..!")]
        public string OHC_OfficerQualification { get; set; }

        [Display(Name = "N.4.2 Is Violation Found?")]
        [Required(ErrorMessage = "OHC_OfficerQualification_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OHC_OfficerQualification_ViolationExist { get; set; }

        [Display(Name = "N.5.1 Whether Para-medical staff are being appointed, if so the details.")]
        [Required(ErrorMessage = "OHC_ParaMedicalStaffAppointed is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "OHC_ParaMedicalStaffAppointed can have max 1000 chars..!")]
        public string OHC_ParaMedicalStaffAppointed { get; set; }

        [Display(Name = "N.5.2 Is Violation Found?")]
        [Required(ErrorMessage = "OHC_ParaMedicalStaffAppointed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OHC_ParaMedicalStaffAppointed_ViolationExist { get; set; }

        [Display(Name = "N.6.1 Whether an Ambulance van is provided and maintained along with a driver, if so furnish the details, if not specify the reason along with the alternative arrangements made")]
        [Required(ErrorMessage = "OHC_AmbulanceVanProvided is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "OHC_AmbulanceVanProvided can have max 1000 chars..!")]
        public string OHC_AmbulanceVanProvided { get; set; }

        [Display(Name = "N.6.2 Is Violation Found?")]
        [Required(ErrorMessage = "OHC_AmbulanceVanProvided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum OHC_AmbulanceVanProvided_ViolationExist { get; set; }

        [Display(Name = "0.1.1 Whether the employees working in Hazardous process have been subjected to pre - employment and periodical medical examinations ? If so, ")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_Selection is required..!")]
        public GeneralOptionTypeEnum WorkingInHazardousProcess_Selection { get; set; }

        [Display(Name = "0.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum WorkingInHazardousProcess_ViolationExist { get; set; }

        [Display(Name = "0.2.1 The name and address of the medical officer conducting such examinations.")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_NameAddressOfficer is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "WorkingInHazardousProcess_NameAddressOfficer can have max 1000 chars..!")]
        public string WorkingInHazardousProcess_NameAddressOfficer { get; set; }

        [Display(Name = "0.2.2 Is Violation Found?")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_NameAddressOfficer_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum WorkingInHazardousProcess_NameAddressOfficer_ViolationExist { get; set; }

        [Display(Name = "0.3.1 No. of employees medically examined.")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_NoOfWorkersExamined is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "WorkingInHazardousProcess_NoOfWorkersExamined can have max 1000 chars..!")]
        public string WorkingInHazardousProcess_NoOfWorkersExamined { get; set; }

        [Display(Name = "0.3.2 Is Violation Found?")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_NoOfWorkersExamined_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum WorkingInHazardousProcess_NoOfWorkersExamined_ViolationExist { get; set; }

        [Display(Name = "0.4.1 The nature of occupational disease identified.")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_NatureOfOccDisease is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "WorkingInHazardousProcess_NatureOfOccDisease can have max 1000 chars..!")]
        public string WorkingInHazardousProcess_NatureOfOccDisease { get; set; }

        [Display(Name = "0.4.2 Is Violation Found?")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_NatureOfOccDisease_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum WorkingInHazardousProcess_NatureOfOccDisease_ViolationExist { get; set; }

        [Display(Name = "0.5.1 Are there any noticeable diseases? If so the details")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_AnyNoticeableDiseased is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "WorkingInHazardousProcess_AnyNoticeableDiseased can have max 1000 chars..!")]
        public string WorkingInHazardousProcess_AnyNoticeableDiseased { get; set; }

        [Display(Name = "0.5.2 Is Violation Found?")]
        [Required(ErrorMessage = "WorkingInHazardousProcess_AnyNoticeableDiseased_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum WorkingInHazardousProcess_AnyNoticeableDiseased_ViolationExist { get; set; }

        [Display(Name = "P P.1.1 Whether, a data - base on Health record are being developed, if so sample copy of the same to be obtained.")]
        [Required(ErrorMessage = "DatabaseOnHealthRecordDeveloped is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "DatabaseOnHealthRecordDeveloped can have max 1000 chars..!")]
        public string DatabaseOnHealthRecordDeveloped { get; set; }

        [Display(Name = "P.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "DatabaseOnHealthRecordDeveloped_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum DatabaseOnHealthRecordDeveloped_ViolationExist { get; set; }

        [Display(Name = "Q Q.1.1 Whether the public awareness programme has been conducted in inform the public outside the site who are likely to be affected by a major accident")]
        [Required(ErrorMessage = "PublicAwarenessProgConducted is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "PublicAwarenessProgConducted can have max 1000 chars..!")]
        public string PublicAwarenessProgConducted { get; set; }

        [Display(Name = "Q.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "PublicAwarenessProgConducted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum PublicAwarenessProgConducted_ViolationExist { get; set; }

        [Display(Name = "R R.1.1 Whether the safety pamphlets have been printed and distributed to the public living nearby")]
        [Required(ErrorMessage = "SafetyPamphletsPrinted is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "SafetyPamphletsPrinted can have max 1000 chars..!")]
        public string SafetyPamphletsPrinted { get; set; }

        [Display(Name = "R.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "SafetyPamphletsPrinted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum SafetyPamphletsPrinted_ViolationExist { get; set; }

        [Display(Name = "S S.1.1 Are persons with suitable qualification and experience are employed as supervisors")]
        [Required(ErrorMessage = "PrsonsWithSuitableSupervisors is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "PrsonsWithSuitableSupervisors can have max 1000 chars..!")]
        public string PrsonsWithSuitableSupervisors { get; set; }

        [Display(Name = "S.1.2 Is Violation Found?")]
        [Required(ErrorMessage = "PrsonsWithSuitableSupervisors_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum PrsonsWithSuitableSupervisors_ViolationExist { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Factory_Part_III_DangerousOperation
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "11.Is factory carrying any Dangerous Operation (as per Rule-102)")]
        [Required(ErrorMessage = "isCarryingAnyDangerousOperation is required..!")]
        public bool isCarryingAnyDangerousOperation { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }


    public class Inspection_FormLockParmViewModel
    {
        public Int64 InspectionRefId { get; set; }
    }

    public class InspectionTransferLogs
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "InspectionRefId is required..!")]
        public Int64 InspectionRefId { get; set; }

        [Required(ErrorMessage = "SenderUserRefId is required..!")]
        public string SenderUserRefId { get; set; }

        [Required(ErrorMessage = "SenderRoleId is required..!")]
        public string SenderRoleId { get; set; }

        [Required(ErrorMessage = "SenderProfileId is required..!")]
        public Int64 SenderProfileId { get; set; }

        [Required(ErrorMessage = "ReceiverUserRefId is required..!")]
        public string ReceiverUserRefId { get; set; }
        [Required(ErrorMessage = "ReceiverRoleId is required..!")]
        public string ReceiverRoleId { get; set; }

        [Required(ErrorMessage = "ReceiverProfileId is required..!")]
        public Int64 ReceiverProfileId { get; set; }

        [Required(ErrorMessage = "LastModifiedOn is required..!")]
        public DateTime LastModifiedOn { get; set; }

        [Required(ErrorMessage = "SenderCircleRefId is required..!")]
        public Int64 SenderCircleRefId { get; set; }

        [Required(ErrorMessage = "ReceiverCircleRefId is required..!")]
        public Int64 ReceiverCircleRefId { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Inspection Type is required..!")]
        public int InspectionType { get; set; }
    }


    public class Inspection_Form_Labour_Part_I_General
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "1.1 Name of factory")]
        [Required(ErrorMessage = "FactoryName is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "FactoryName can have max 100 chars..!")]
        public string FactoryName { get; set; }

        [Display(Name = "1.2 Address of factory")]
        [Required(ErrorMessage = "FactoryAddress is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "FactoryAddress can have max 500 chars..!")]
        public string FactoryAddress { get; set; }

        [Display(Name = "1.3 Date Of Inspection")]
        [Required(ErrorMessage = "DateOfInspection is required..!")]
        public DateTime DateOfInspection { get; set; }

        [Display(Name = "1.4 Date Of Last Inspection")]
        //[Required(ErrorMessage = "InspectionFacrotyExistenceType is required..!")]
        //public InspectionFactoryExistenceTypeEnum InspectionFactoryExistenceType { get; set; }
        public DateTime DateOfLastInspection { get; set; }

        [Display(Name = "1.5 Name of Occupier")]
        [Required(ErrorMessage = "OccupierName is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "OccupierName can have max 100 chars..!")]
        public string OccupierName { get; set; }

        [Display(Name = "1.6 Address of Occupier")]
        [Required(ErrorMessage = "OccupierAddress is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "OccupierAddress can have max 500 chars..!")]
        public string OccupierAddress { get; set; }

        [Display(Name = "1.7 Name of Manager")]
        [Required(ErrorMessage = "ManagerName is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "ManagerName can have max 100 chars..!")]
        public string ManagerName { get; set; }

        [Display(Name = "1.8 Address of Manager")]
        [Required(ErrorMessage = "ManagerAddress is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "ManagerAddress can have max 500 chars..!")]
        public string ManagerAddress { get; set; }

        [Display(Name = "1.9 Name of Person Present")]
        [Required(ErrorMessage = "PresentPersonName is required..!")]
        [StringLength(maximumLength: 100, ErrorMessage = "PresentPersonName can have max 100 chars..!")]
        public string PresentPersonName { get; set; }

        [Display(Name = "1.10 Address of Person Present")]
        [Required(ErrorMessage = "PresentPersonAddress is required..!")]
        [StringLength(maximumLength: 500, ErrorMessage = "PresentPersonAddress can have max 500 chars..!")]
        public string PresentPersonAddress { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_Part_II_FactoryDetail
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "2.1 License number under Factories Act 1948")]
        [Required(ErrorMessage = "License number under Factories Act 1948 is required..!")]

        [StringLength(maximumLength: 50, ErrorMessage = "License number under Factories Act 1948 can have max 50 chars..!")]
  
        public string License_Factory_Act { get; set; }

        [Display(Name = "2.2 Registration number of Principal Employer under The Contract")]
        [Required(ErrorMessage = "Registration number of Principal Employer under The Contract Labour (R &amp;A) Act 1970 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "Registration number of Principal Employer under The Contract Labour (R &amp;A) Act 1970 can have max 50 chars..!")]

        public string Registration_PE_Act { get; set; }

        [Display(Name = "2.3 License numbers of contractor(s) under The Contract Labour (R &amp;A) Act 1970")]
        [Required(ErrorMessage = "License numbers of contractor(s) under The Contract Labour (R &amp;A) Act 1970 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "License numbers of contractor(s) under The Contract Labour (R &amp;A) Act 1970 can have max 50 chars..!")]

        public string License_CL_Act { get; set; }

        [Display(Name = "2.4 Registration number of factory under The Inter-State Migrant Workmen (RoECS) Act 1979")]
        [Required(ErrorMessage = "Registration number of factory under The Inter -State Migrant Workmen(RoECS) Act 1979 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "Registration number of factory under The Inter -State Migrant Workmen(RoECS) Act 1979 can have max 50 chars..!")]

        public string Registration_ISMW_Act { get; set; }

        [Display(Name = "2.5 RLicense number of contractor under The Inter-State Migrant Workmen (RoECS) Act 1979")]
        [Required(ErrorMessage = "License number of contractor under The Inter -State Migrant Workmen(RoECS) Act 1979 is required..!")]
        [StringLength(maximumLength: 50, ErrorMessage = "License number of contractor under The Inter -State Migrant Workmen(RoECS) Act 1979 can have max 50 chars..!")]

        public string License_ISMW_Act { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_Part_III_MusterRoll
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "ShiftTime_From")]
        [Required(ErrorMessage = "ShiftTime_From is required..!")]
        [StringLength(maximumLength: 20, ErrorMessage = "ShiftTime_From can have max 20 chars..!")]
        public string ShiftTime_From { get; set; }

        [Display(Name = "ShiftTime_To")]
        [Required(ErrorMessage = "ShiftTime_To is required..!")]
        [StringLength(maximumLength: 20, ErrorMessage = "ShiftTime_To can have max 20 chars..!")]
        public string ShiftTime_To { get; set; }

        [Display(Name = "Count_Adult_Male")]
        [Required(ErrorMessage = "Count_Adult_Male is required..!")]
        public int Count_Adult_Male { get; set; }

        [Display(Name = "Count_Adult_FeMale")]
        [Required(ErrorMessage = "Count_Adult_FeMale is required..!")]
        public int Count_Adult_FeMale { get; set; }

        [Display(Name = "Count_Adolescent_Male")]
        [Required(ErrorMessage = "Count_Adolescent_Male is required..!")]
        public int Count_Adolescent_Male { get; set; }

        [Display(Name = "Count_Adolescent_FeMale")]
        [Required(ErrorMessage = "Count_Adolescent_FeMale is required..!")]
        public int Count_Adolescent_FeMale { get; set; }

        [Display(Name = "Count_Children_Male")]
        [Required(ErrorMessage = "Count_Children_Male is required..!")]
        public int Count_Children_Male { get; set; }

        [Display(Name = "Count_Children_FeMale")]
        [Required(ErrorMessage = "Count_Children_FeMale is required..!")]
        public int Count_Children_FeMale { get; set; }

        [Display(Name = "Inspection Muster Roll Type")]
        [Required(ErrorMessage = "InspectionMusterRollType is required..!")]
        public InspectionMusterRollTypeEnum InspectionMusterRollType { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Labour_Part_III_EqualEnumerationAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "4.1.1 Section-4 Are enumeration to workers of opposite sexes being paid equally?")]
        [Required(ErrorMessage = "Sec_4_IsEnumerationOfOppositeGenderPaidEqually is required..!")]
        public GeneralOptionTypeEnum Sec_4_IsEnumerationOfOppositeGenderPaidEqually { get; set; }

        [Display(Name = "4.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_4_IsEnumerationOfOppositeGenderPaidEqually_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_4_IsEnumerationOfOppositeGenderPaidEqually_ViolationExist { get; set; }

        [Display(Name = "4.1.3 Sec_4_IsEnumerationOfOppositeGenderPaidEqually_Remarks")]
        [Required(ErrorMessage = "Sec_4_IsEnumerationOfOppositeGenderPaidEqually_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_4_IsEnumerationOfOppositeGenderPaidEqually_Remarks can have max 1000 chars..!")]
        public string Sec_4_IsEnumerationOfOppositeGenderPaidEqually_Remarks { get; set; }

        [Display(Name = "5.1.1 Section-5 Is there any discrimination in recruitment, any condition of service subsequent to recruitment such as promotion, training, transfer?")]
        [Required(ErrorMessage = "Sec_5_AnyDiscriminationIn_Recruitment_Promotion_Training_Transfer is required..!")]
        public GeneralOptionTypeEnum Sec_5_AnyDiscriminationIn_Recruitment_Promotion_Training_Transfer { get; set; }

        [Display(Name = "5.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_5_AnyDiscriminationIn_Recruitment_Promotion_Training_Transfer_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_5_AnyDiscriminationIn_Recruitment_Promotion_Training_Transfer_ViolationExist { get; set; }

        [Display(Name = "5.1.3 Sec_5_AnyDiscriminationIn_Recruitment_Promotion_Training_Transfer_Remarks")]
        [Required(ErrorMessage = "Sec_5_AnyDiscriminationIn_Recruitment_Promotion_Training_Transfer_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_5_AnyDiscriminationIn_Recruitment_Promotion_Training_Transfer_Remarks can have max 1000 chars..!")]
        public string Sec_5_AnyDiscriminationIn_Recruitment_Promotion_Training_Transfer_Remarks { get; set; }

        [Display(Name = "6.1.1 Rule_6_IsRegister_FormD_Maintained")]
        [Required(ErrorMessage = "Rule_6_IsRegister_FormD_Maintained is required..!")]
        public GeneralOptionTypeEnum Rule_6_IsRegister_FormD_Maintained { get; set; }
        
        [Display(Name = "6.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_6_IsRegister_FormD_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_6_IsRegister_FormD_Maintained_ViolationExist { get; set; }

        [Display(Name = "6.1.3 Rule_6_IsRegister_FormD_Maintained_Remarks")]
        [Required(ErrorMessage = "Rule_6_IsRegister_FormD_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_6_IsRegister_FormD_Maintained_Remarks can have max 1000 chars..!")]
        public string Rule_6_IsRegister_FormD_Maintained_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_MinimumWageAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "12.1.1 Section-12 Are wages being paid to all employees as fixed by appropriate government under section-5?")]
        [Required(ErrorMessage = "Sec_12_IsWagesPaid_ToAll_FixedByGovernment_UnderSection_5 is required..!")]
        public GeneralOptionTypeEnum Sec_12_IsWagesPaid_ToAll_FixedByGovernment_UnderSection_5 { get; set; }

        [Display(Name = "12.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_12_IsWagesPaid_ToAll_FixedByGovernment_UnderSection_5_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_12_IsWagesPaid_ToAll_FixedByGovernment_UnderSection_5_ViolationExist { get; set; }

        [Display(Name = "12.1.3 Sec_12_IsWagesPaid_ToAll_FixedByGovernment_UnderSection_5_Remarks")]
        [Required(ErrorMessage = "Sec_12_IsWagesPaid_ToAll_FixedByGovernment_UnderSection_5_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_12_IsWagesPaid_ToAll_FixedByGovernment_UnderSection_5_Remarks can have max 1000 chars..!")]
        public string Sec_12_IsWagesPaid_ToAll_FixedByGovernment_UnderSection_5_Remarks { get; set; }

        [Display(Name = "13.1.1 Section-13 Are all employees working as per hours fixed by appropriate government?")]
        [Required(ErrorMessage = "Sec_13_IsAllEmployeeWorking_AsFixedHoursBy_AppropriateGovernment is required..!")]
            public GeneralOptionTypeEnum Sec_13_IsAllEmployeeWorking_AsFixedHoursBy_AppropriateGovernment { get; set; }

        [Display(Name = "13.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_13_IsAllEmployeeWorking_AsFixedHoursBy_AppropriateGovernment_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_13_IsAllEmployeeWorking_AsFixedHoursBy_AppropriateGovernment_ViolationExist { get; set; }

        [Display(Name = "13.1.3 Sec_13_IsAllEmployeeWorking_AsFixedHoursBy_AppropriateGovernment_Remarks")]
        [Required(ErrorMessage = "Sec_13_IsAllEmployeeWorking_AsFixedHoursBy_AppropriateGovernment_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_13_IsAllEmployeeWorking_AsFixedHoursBy_AppropriateGovernment_Remarks can have max 1000 chars..!")]
        public string Sec_13_IsAllEmployeeWorking_AsFixedHoursBy_AppropriateGovernment_Remarks { get; set; }

        [Display(Name = "13.2.1 Is rest in period of seven days is given to every employee?")]
        [Required(ErrorMessage = "Sec_13_IsRest_Given_ToEvery_Employee is required..!")]
        public GeneralOptionTypeEnum Sec_13_IsRest_Given_ToEvery_Employee { get; set; }

        [Display(Name = "13.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_13_IsRest_Given_ToEvery_Employee_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_13_IsRest_Given_ToEvery_Employee_ViolationExist { get; set; }

        [Display(Name = "13.2.3 Sec_13_IsRest_Given_ToEvery_Employee_Remarks")]
        [Required(ErrorMessage = "Sec_13_IsRest_Given_ToEvery_Employee_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_13_IsRest_Given_ToEvery_Employee_Remarks can have max 1000 chars..!")]
        public string Sec_13_IsRest_Given_ToEvery_Employee_Remarks { get; set; }

        [Display(Name = "13.3.1 Is payment for work done on rest day is given at rate overtime?")]
        [Required(ErrorMessage = "Sec_13_IsPayment_Overtime_Given is required..!")]
        public GeneralOptionTypeEnum Sec_13_IsPayment_Overtime_Given { get; set; }

        [Display(Name = "13.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_13_IsPayment_Overtime_Given_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_13_IsPayment_Overtime_Given_ViolationExist { get; set; }

        [Display(Name = "13.3.3 Sec_13_IsPayment_Overtime_Given_Remarks")]
        [Required(ErrorMessage = "Sec_13_IsPayment_Overtime_Given_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_13_IsPayment_Overtime_Given_Remarks can have max 1000 chars..!")]
        public string Sec_13_IsPayment_Overtime_Given_Remarks { get; set; }

        [Display(Name = "15.1.1 Section-15 Are wages to workers who work for less than normal working day being paid as per provision of section-15?")]
        [Required(ErrorMessage = "Sec_15_IsWagesPaid_ForLessWorkingDays_AsPerSection_15 is required..!")]
        public GeneralOptionTypeEnum Sec_15_IsWagesPaid_ForLessWorkingDays_AsPerSection_15 { get; set; }

        [Display(Name = "15.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_15_IsWagesPaid_ForLessWorkingDays_AsPerSection_15_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_15_IsWagesPaid_ForLessWorkingDays_AsPerSection_15_ViolationExist { get; set; }

        [Display(Name = "15.1.3 Sec_15_IsWagesPaid_ForLessWorkingDays_AsPerSection_15_Remarks")]
        [Required(ErrorMessage = "Sec_15_IsWagesPaid_ForLessWorkingDays_AsPerSection_15_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_15_IsWagesPaid_ForLessWorkingDays_AsPerSection_15_Remarks can have max 1000 chars..!")]
        public string Sec_15_IsWagesPaid_ForLessWorkingDays_AsPerSection_15_Remarks { get; set; }

        [Display(Name = "21.1.1 Section-21 Is time and conditions of payment being complied?")]
        [Required(ErrorMessage = "Rule_21_IsTimeAndConditions_OfPayment_Complied is required..!")]
        public GeneralOptionTypeEnum Rule_21_IsTimeAndConditions_OfPayment_Complied { get; set; }

        [Display(Name = "21.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_21_IsTimeAndConditions_OfPayment_Complied_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_21_IsTimeAndConditions_OfPayment_Complied_ViolationExist { get; set; }

        [Display(Name = "21.1.3 Rule_21_IsTimeAndConditions_OfPayment_Complied_Remarks")]
        [Required(ErrorMessage = "Rule_21_IsTimeAndConditions_OfPayment_Complied_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_21_IsTimeAndConditions_OfPayment_Complied_Remarks can have max 1000 chars..!")]
        public string Rule_21_IsTimeAndConditions_OfPayment_Complied_Remarks { get; set; }

        [Display(Name = "21.2.1 Are deductions made as prescribed?")]
        [Required(ErrorMessage = "Rule_21_IsDeductionMade_AsPrescribed is required..!")]
        public GeneralOptionTypeEnum Rule_21_IsDeductionMade_AsPrescribed { get; set; }

        [Display(Name = "21.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_21_IsDeductionMade_AsPrescribed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_21_IsDeductionMade_AsPrescribed_ViolationExist { get; set; }

        [Display(Name = "21.2.3 Rule_21_IsDeductionMade_AsPrescribed_Remarks")]
        [Required(ErrorMessage = "Rule_21_IsDeductionMade_AsPrescribed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_21_IsDeductionMade_AsPrescribed_Remarks can have max 1000 chars..!")]
        public string Rule_21_IsDeductionMade_AsPrescribed_Remarks { get; set; }

        [Display(Name = "21.3.1 Upon by employer/authorized representative?")]
        [Required(ErrorMessage = "Rule_21_Upon_EmployerOrAuthorized_Representative is required..!")]
        public GeneralOptionTypeEnum Rule_21_Upon_EmployerOrAuthorized_Representative { get; set; }

        [Display(Name = "21.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_21_Upon_EmployerOrAuthorized_Representative_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_21_Upon_EmployerOrAuthorized_Representative_ViolationExist { get; set; }

        [Display(Name = "21.3.3 Rule_21_Upon_EmployerOrAuthorized_Representative_Remarks")]
        [Required(ErrorMessage = "Rule_21_Upon_EmployerOrAuthorized_Representative_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_21_Upon_EmployerOrAuthorized_Representative_Remarks can have max 1000 chars..!")]
        public string Rule_21_Upon_EmployerOrAuthorized_Representative_Remarks { get; set; }

        [Display(Name = "21.4.1 Is Muster roll Form V Maintained?")]
        [Required(ErrorMessage = "Rule_21_IsMusterRoll_Form_V_Maintained is required..!")]
        public GeneralOptionTypeEnum Rule_21_IsMusterRoll_Form_V_Maintained { get; set; }

        [Display(Name = "21.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_21_IsMusterRoll_Form_V_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_21_IsMusterRoll_Form_V_Maintained_ViolationExist { get; set; }

        [Display(Name = "21.4.3 Rule_21_IsMusterRoll_Form_V_Maintained_Remarks")]
        [Required(ErrorMessage = "Rule_21_IsMusterRoll_Form_V_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_21_IsMusterRoll_Form_V_Maintained_Remarks can have max 1000 chars..!")]
        public string Rule_21_IsMusterRoll_Form_V_Maintained_Remarks { get; set; }


        [Display(Name = "26B.1.1 Section-26B Are registered preserved for three years after date of  last entry made in therein?")]
        [Required(ErrorMessage = "Rule_26_B_IsRegistered_Preserved_For_Three_Years is required..!")]
        public GeneralOptionTypeEnum Rule_26_B_IsRegistered_Preserved_For_Three_Years { get; set; }

        [Display(Name = "26B.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_26_B_IsRegistered_Preserved_For_Three_Years_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_26_B_IsRegistered_Preserved_For_Three_Years_ViolationExist { get; set; }

        [Display(Name = "26B.1.3 Rule_26_B_IsRegistered_Preserved_For_Three_Years_Remarks")]
        [Required(ErrorMessage = "Rule_26_B_IsRegistered_Preserved_For_Three_Years_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_26_B_IsRegistered_Preserved_For_Three_Years_Remarks can have max 1000 chars..!")]
        public string Rule_26_B_IsRegistered_Preserved_For_Three_Years_Remarks { get; set; }

        [Display(Name = "26C.1.1 Section-26C Are registered produced at time of inspection?")]
        [Required(ErrorMessage = "Rule_26_C_IsRegistered_Produced_While_Inspection is required..!")]
        public GeneralOptionTypeEnum Rule_26_C_IsRegistered_Produced_While_Inspection { get; set; }

        [Display(Name = "26C.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_26_C_IsRegistered_Produced_While_Inspection_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_26_C_IsRegistered_Produced_While_Inspection_ViolationExist { get; set; }

        [Display(Name = "26C.1.3 Rule_26_C_IsRegistered_Produced_While_Inspection_Remarks")]
        [Required(ErrorMessage = "Rule_26_C_IsRegistered_Produced_While_Inspection_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_26_C_IsRegistered_Produced_While_Inspection_Remarks can have max 1000 chars..!")]
        public string Rule_26_C_IsRegistered_Produced_While_Inspection_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_PaymentWagesAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "4.1.1 Section-4 Is period of wages fixed?")]
        [Required(ErrorMessage = "Sec_4_IsWagesPeriodFixed is required..!")]
        public GeneralOptionTypeEnum Sec_4_IsWagesPeriodFixed { get; set; }

        [Display(Name = "4.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_4_IsWagesPeriodFixed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_4_IsWagesPeriodFixed_ViolationExist { get; set; }

        [Display(Name = "4.1.3 Sec_4_IsWagesPeriodFixed_Remarks")]
        [Required(ErrorMessage = "Sec_4_IsWagesPeriodFixed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_4_IsWagesPeriodFixed_Remarks can have max 1000 chars..!")]
        public string Sec_4_IsWagesPeriodFixed_Remarks { get; set; }

        [Display(Name = "5.1.1 Section-5 Are wages being paid on time as specified in section-5?")]
        [Required(ErrorMessage = "Sec_5_IsWagesPaid_OnTime_As_Per_Section_5 is required..!")]
        public GeneralOptionTypeEnum Sec_5_IsWagesPaid_OnTime_As_Per_Section_5 { get; set; }

        [Display(Name = "5.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_5_IsWagesPaid_OnTime_As_Per_Section_5_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_5_IsWagesPaid_OnTime_As_Per_Section_5_ViolationExist { get; set; }

        [Display(Name = "5.1.3 Sec_5_IsWagesPaid_OnTime_As_Per_Section_5_Remarks")]
        [Required(ErrorMessage = "Sec_5_IsWagesPaid_OnTime_As_Per_Section_5_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_5_IsWagesPaid_OnTime_As_Per_Section_5_Remarks can have max 1000 chars..!")]
        public string Sec_5_IsWagesPaid_OnTime_As_Per_Section_5_Remarks { get; set; }

        [Display(Name = "6.1.1 Section-6 Are wages paid as prescribed in section6 or as notified by appropriate government?")]
        [Required(ErrorMessage = "Sec_6_IsWages_Paid_As_Prescribed is required..!")]
        public GeneralOptionTypeEnum Sec_6_IsWages_Paid_As_Prescribed { get; set; }

        [Display(Name = "6.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_6_IsWages_Paid_As_Prescribed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_6_IsWages_Paid_As_Prescribed_ViolationExist { get; set; }

        [Required(ErrorMessage = "Sec_6_Wages_Paid_As_Prescribed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_6_Wages_Paid_As_Prescribed_Remarks can have max 1000 chars..!")]
        public string Sec_6_IsWages_Paid_As_Prescribed_Remarks { get; set; }

        [Display(Name = "7.1.1 Section-7 Are deductions made only  as specified in section-7")]
        [Required(ErrorMessage = "Sec_7_IsDeductionMade is required..!")]
        public GeneralOptionTypeEnum Sec_7_IsDeductionMade { get; set; }

        [Display(Name = "7.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_7_IsDeductionMade_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_7_IsDeductionMade_ViolationExist { get; set; }

        [Display(Name = "7.1.3 Sec_7_IsDeductionMade_Remarks")]
        [Required(ErrorMessage = "Sec_7_IsDeductionMade_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_7_IsDeductionMade_Remarks can have max 1000 chars..!")]
        public string Sec_7_IsDeductionMade_Remarks { get; set; }

        [Display(Name = "8.1.1 Section-8 Are fines imposed for acts and omissions with previous approval of appropriate government or prescribed authority?")]
        [Required(ErrorMessage = "Sec_8_IsFine_Imposed_For_Acts_And_Omissions is required..!")]
        public GeneralOptionTypeEnum Sec_8_IsFine_Imposed_For_Acts_And_Omissions { get; set; }

        [Display(Name = "8.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_IsFine_Imposed_For_Acts_And_Omissions_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_IsFine_Imposed_For_Acts_And_Omissions_ViolationExist { get; set; }

        [Display(Name = "8.1.3 Sec_8_IsFine_Imposed_For_Acts_And_Omissions_Remarks")]
        [Required(ErrorMessage = "Sec_8_IsFine_Imposed_For_Acts_And_Omissions_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_IsFine_Imposed_For_Acts_And_Omissions_Remarks can have max 1000 chars..!")]
        public string Sec_8_IsFine_Imposed_For_Acts_And_Omissions_Remarks { get; set; }

        [Display(Name = "8.2.1 Is notice of acts and omissions displayed?")]
        [Required(ErrorMessage = "Sec_8_IsNotice_Of_Acts_And_Omissions_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_8_IsNotice_Of_Acts_And_Omissions_Displayed { get; set; }

        [Display(Name = "8.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_IsNotice_Of_Acts_And_Omissions_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_IsNotice_Of_Acts_And_Omissions_Displayed_ViolationExist { get; set; }

        [Display(Name = "8.2.3 Sec_8_IsNotice_Of_Acts_And_Omissions_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_8_IsNotice_Of_Acts_And_Omissions_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_IsNotice_Of_Acts_And_Omissions_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_8_IsNotice_Of_Acts_And_Omissions_Displayed_Remarks { get; set; }

        [Display(Name = "8.3.1 Is fine imposed after giving opportunity?")]
        [Required(ErrorMessage = "Sec_8_IsFine_Imposed_After_Giving_Opportunity is required..!")]
        public GeneralOptionTypeEnum Sec_8_IsFine_Imposed_After_Giving_Opportunity { get; set; }

        [Display(Name = "8.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_IsFine_Imposed_After_Giving_Opportunity_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_IsFine_Imposed_After_Giving_Opportunity_ViolationExist { get; set; }

        [Display(Name = "8.3.3 Sec_8_IsFine_Imposed_After_Giving_Opportunity_Remarks")]
        [Required(ErrorMessage = "Sec_8_IsFine_Imposed_After_Giving_Opportunity_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_IsFine_Imposed_After_Giving_Opportunity_Remarks can have max 1000 chars..!")]
        public string Sec_8_IsFine_Imposed_After_Giving_Opportunity_Remarks { get; set; }


        [Display(Name = "8.4.1 Is fine amount as per sub-section-8(4) and 8(5)?")]
        [Required(ErrorMessage = "Sec_8_Is_Fine_Amount_As_Per_Sub_Sec_8_4_And_8_5 is required..!")]
        public GeneralOptionTypeEnum Sec_8_Is_Fine_Amount_As_Per_Sub_Sec_8_4_And_8_5 { get; set; }

        [Display(Name = "8.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_Is_Fine_Amount_As_Per_Sub_Sec_8_4_And_8_5_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_Is_Fine_Amount_As_Per_Sub_Sec_8_4_And_8_5_ViolationExist { get; set; }

        [Display(Name = "8.4.3 Sec_8_Is_Fine_Amount_As_Per_Sub_Sec_8_4_And_8_5_Remarks")]
        [Required(ErrorMessage = "Sec_8_Is_Fine_Amount_As_Per_Sub_Sec_8_4_And_8_5_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_Is_Fine_Amount_As_Per_Sub_Sec_8_4_And_8_5_Remarks can have max 1000 chars..!")]
        public string Sec_8_Is_Fine_Amount_As_Per_Sub_Sec_8_4_And_8_5_Remarks { get; set; }

        [Display(Name = "8.5.1 Is fine being recovered as per sub-section 8(6)?")]
        [Required(ErrorMessage = "Sec_8_Is_Fine_Recovered_As_Per_Sub_Sec_8_6 is required..!")]
        public GeneralOptionTypeEnum Sec_8_Is_Fine_Recovered_As_Per_Sub_Sec_8_6 { get; set; }
        
        [Display(Name = "8.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_Is_Fine_Recovered_As_Per_Sub_Sec_8_6_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_Is_Fine_Recovered_As_Per_Sub_Sec_8_6_ViolationExist { get; set; }

        [Display(Name = "8.5.3 Sec_8_Is_Fine_Recovered_As_Per_Sub_Sec_8_6_Remarks")]
        [Required(ErrorMessage = "Sec_8_Is_Fine_Recovered_As_Per_Sub_Sec_8_6_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_Is_Fine_Recovered_As_Per_Sub_Sec_8_6_Remarks can have max 1000 chars..!")]
        public string Sec_8_Is_Fine_Recovered_As_Per_Sub_Sec_8_6_Remarks { get; set; }


        [Display(Name = "9.1.1 Section-9,10,11,12,12A,13 Are deductions made as per this section?")]
        [Required(ErrorMessage = "Sec_9_To_13_IsDeductions_Made_AsPerSection is required..!")]
        public GeneralOptionTypeEnum Sec_9_To_13_IsDeductions_Made_AsPerSection { get; set; }

        [Display(Name = "9.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_9_To_13_IsDeductions_Made_AsPerSection_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_9_To_13_IsDeductions_Made_AsPerSection_ViolationExist { get; set; }

        [Display(Name = "9.1.3 Sec_9_To_13_IsDeductions_Made_AsPerSection_Remarks")]
        [Required(ErrorMessage = "Sec_9_To_13_IsDeductions_Made_AsPerSection_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_9_To_13_IsDeductions_Made_AsPerSection_Remarks can have max 1000 chars..!")]
        public string Sec_9_To_13_IsDeductions_Made_AsPerSection_Remarks { get; set; }

        [Display(Name = "13A.1.1 Section-13A Are prescribed register being maintained?")]
        [Required(ErrorMessage = "Sec_13_A_IsPrescribed_Register_Maintained is required..!")]
        public GeneralOptionTypeEnum Sec_13_A_IsPrescribed_Register_Maintained { get; set; }

        [Display(Name = "13A.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_13_A_IsPrescribed_Register_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_13_A_IsPrescribed_Register_Maintained_ViolationExist { get; set; }

        [Display(Name = "13A.1.3 Sec_13_A_IsPrescribed_Register_Maintained_Remarks")]
        [Required(ErrorMessage = "Sec_13_A_IsPrescribed_Register_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_13_A_IsPrescribed_Register_Maintained_Remarks can have max 1000 chars..!")]
        public string Sec_13_A_IsPrescribed_Register_Maintained_Remarks { get; set; }

        [Display(Name = "13A.2.1 Are registered preserved for three years after date of  last entry made in therein?")]
        [Required(ErrorMessage = "Sec_13_A_IsRegister_Preserved_For_Three_Years is required..!")]
        public GeneralOptionTypeEnum Sec_13_A_IsRegister_Preserved_For_Three_Years { get; set; }

        [Display(Name = "13A.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_13_A_IsRegister_Preserved_For_Three_Years_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_13_A_IsRegister_Preserved_For_Three_Years_ViolationExist { get; set; }

        [Display(Name = "13A.2.3 Sec_13_A_IsRegister_Preserved_For_Three_Years_Remarks")]
        [Required(ErrorMessage = "Sec_13_A_IsRegister_Preserved_For_Three_Years_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_13_A_IsRegister_Preserved_For_Three_Years_Remarks can have max 1000 chars..!")]
        public string Sec_13_A_IsRegister_Preserved_For_Three_Years_Remarks { get; set; }

        [Display(Name = "25.1.1 Section-25 Are notices required under this section displayed?")]
        [Required(ErrorMessage = "Sec_25_IsNotices_Under_section_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_25_IsNotices_Under_section_Displayed { get; set; }

       

        [Display(Name = "25.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_25_IsNotices_Under_section_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_25_IsNotices_Under_section_Displayed_ViolationExist { get; set; }

        [Display(Name = "25.1.3 Sec_25_IsNotices_Under_section_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_25_IsNotices_Under_section_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_25_IsNotices_Under_section_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_25_IsNotices_Under_section_Displayed_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "Rule-3.1.1 Rule-3 Is any permission to change accounting year granted by labour Commissioner?")]
        [Required(ErrorMessage = "Rule_3_AnyPermission_ToChange_AccountingYear is required..!")]
        public GeneralOptionTypeEnum Rule_3_AnyPermission_ToChange_AccountingYear { get; set; }

        [Display(Name = "Rule-3.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_3_AnyPermission_ToChange_AccountingYear_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_3_AnyPermission_ToChange_AccountingYear_ViolationExist { get; set; }

        [Display(Name = "Rule-3.1.3 Rule_3_AnyPermission_ToChange_AccountingYear_Remarks")]
        [Required(ErrorMessage = "Rule_3_AnyPermission_ToChange_AccountingYear_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_3_AnyPermission_ToChange_AccountingYear_Remarks can have max 1000 chars..!")]
        public string Rule_3_AnyPermission_ToChange_AccountingYear_Remarks { get; set; }

        [Display(Name = "Rule-4.1.1 Rule-4 Is register in Form-A maintained?")]
        [Required(ErrorMessage = "Rule_4_IsForm_A_Register_Maintained is required..!")]
        public GeneralOptionTypeEnum Rule_4_IsForm_A_Register_Maintained { get; set; }
        
        [Display(Name = "Rule-4.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_4_IsForm_A_Register_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_4_IsForm_A_Register_Maintained_ViolationExist { get; set; }

        [Display(Name = "Rule-4.1.3 Rule_4_IsForm_A_Register_Maintained_Remarks")]
        [Required(ErrorMessage = "Rule_4_IsForm_A_Register_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_4_IsForm_A_Register_Maintained_Remarks can have max 1000 chars..!")]
        public string Rule_4_IsForm_A_Register_Maintained_Remarks { get; set; }

        [Display(Name = "Rule-4.2.1 Is register in Form-B maintained?")]
        [Required(ErrorMessage = "Rule_4_IsForm_B_Register_Maintained is required..!")]
        public GeneralOptionTypeEnum Rule_4_IsForm_B_Register_Maintained { get; set; }

        [Display(Name = "Rule-4.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_4_IsForm_B_Register_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_4_IsForm_B_Register_Maintained_ViolationExist { get; set; }

        [Display(Name = "Rule-4.2.3 Rule_4_IsForm_B_Register_Maintained_Remarks")]
        [Required(ErrorMessage = "Rule_4_IsForm_B_Register_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_4_IsForm_B_Register_Maintained_Remarks can have max 1000 chars..!")]
        public string Rule_4_IsForm_B_Register_Maintained_Remarks { get; set; }

        [Display(Name = "Rule-4.3.1 Is register in Form-C maintained?")]
        [Required(ErrorMessage = "Rule_4_IsForm_B_Register_Maintained is required..!")]
        public GeneralOptionTypeEnum Rule_4_IsForm_C_Register_Maintained { get; set; }

        [Display(Name = "Rule-4.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_4_IsForm_C_Register_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_4_IsForm_C_Register_Maintained_ViolationExist { get; set; }

        [Display(Name = "Rule-4.3.3 Rule_4_IsForm_C_Register_Maintained_Remarks")]
        [Required(ErrorMessage = "Rule_4_IsForm_C_Register_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_4_IsForm_C_Register_Maintained_Remarks can have max 1000 chars..!")]
        public string Rule_4_IsForm_C_Register_Maintained_Remarks { get; set; }

        [Display(Name = "Rule-5.1.1  Rule-5 Is annual return in Form-D submitted?")]
        [Required(ErrorMessage = "Rule_5_IsAnnualReturn_Form_D_Submitted is required..!")]
        public GeneralOptionTypeEnum Rule_5_IsAnnualReturn_Form_D_Submitted { get; set; }

        [Display(Name = "Rule-5.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_5_IsAnnualReturn_Form_D_Submitted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_5_IsAnnualReturn_Form_D_Submitted_ViolationExist { get; set; }

        [Display(Name = "Rule-5.1.3 Rule_5_IsAnnualReturn_Form_D_Submitted_Remarks")]
        [Required(ErrorMessage = "Rule_5_IsAnnualReturn_Form_D_Submitted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_5_IsAnnualReturn_Form_D_Submitted_Remarks can have max 1000 chars..!")]
        public string Rule_5_IsAnnualReturn_Form_D_Submitted_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "AccountingYear")]
        [Required(ErrorMessage = "AccountingYear is required..!")]
        public string AccountingYear { get; set; }

        [Display(Name = "NoOfEmployees")]
        [Required(ErrorMessage = "NoOfEmployees is required..!")]
        public int NoOfEmployees { get; set; }

        [Display(Name = "NoOfEmployeesEligibleForBonus")]
        [Required(ErrorMessage = "NoOfEmployeesEligibleForBonus is required..!")]
        public int NoOfEmployeesEligibleForBonus { get; set; }

        [Display(Name = "RateOfBonus")]
        [Required(ErrorMessage = "RateOfBonus is required..!")]
        public decimal RateOfBonus { get; set; }

        [Display(Name = "NoOfEmployeesWhomeBonusPaid")]
        [Required(ErrorMessage = "NoOfEmployeesWhomeBonusPaid is required..!")]
        public int NoOfEmployeesWhomeBonusPaid { get; set; }

        [Display(Name = "NoOfEmployeesWhomeBonusNotPaid")]
        [Required(ErrorMessage = "NoOfEmployeesWhomeBonusNotPaid is required..!")]
        public int NoOfEmployeesWhomeBonusNotPaid { get; set; }

        [Display(Name = "AnyAgreementOfBonousBetweenEmployerAndEmployee")]
        [Required(ErrorMessage = "AnyAgreementOfBonousBetweenEmployerAndEmployee is required..!")]
        public GeneralOptionTypeEnum AnyAgreementOfBonousBetweenEmployerAndEmployee { get; set; }

        [Display(Name = "TotalAmountUnpaidAsBonus")]
        [Required(ErrorMessage = "TotalAmountUnpaidAsBonus is required..!")]
        public decimal TotalAmountUnpaidAsBonus { get; set; }

        [Display(Name = "IsEmployerSendUnpaidBonusToEmployeesAddress")]
        [Required(ErrorMessage = "IsEmployerSendUnpaidBonusToEmployeesAddress is required..!")]
        public GeneralOptionTypeEnum IsEmployerSendUnpaidBonusToEmployeesAddress { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Labour_III_ChildAndAdolescentLabourAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "3.1.1 Section-3 Is any child found working?")]
        [Required(ErrorMessage = "Sec_3_AnyChild_Found_Working is required..!")]
        public GeneralOptionTypeEnum Sec_3_AnyChild_Found_Working { get; set; }
       
        [Display(Name = "3.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_3_AnyChild_Found_Working_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_3_AnyChild_Found_Working_ViolationExist { get; set; }

        [Display(Name = "3.1.3 Sec_3_AnyChild_Found_Working_Remarks")]
        [Required(ErrorMessage = "Sec_3_AnyChild_Found_Working_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_3_AnyChild_Found_Working_Remarks can have max 1000 chars..!")]
        public string Sec_3_AnyChild_Found_Working_Remarks { get; set; }


        [Display(Name = "3A.1.1 Section-3A Is any adolescent found working in certain hazardous occupations or processes which are prohibited under this section?")]
        [Required(ErrorMessage = "Sec_3_A_AnyAdolescent_Found_Working is required..!")]
        public GeneralOptionTypeEnum Sec_3_A_AnyAdolescent_Found_Working { get; set; }

        [Display(Name = "3A.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_3_A_AnyAdolescent_Found_Working_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_3_A_AnyAdolescent_Found_Working_ViolationExist { get; set; }

        [Display(Name = "3A.1.3 Sec_3_A_AnyAdolescent_Found_Working_Remarks")]
        [Required(ErrorMessage = "Sec_3_A_AnyAdolescent_Found_Working_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_3_A_AnyAdolescent_Found_Working_Remarks can have max 1000 chars..!")]
        public string Sec_3_A_AnyAdolescent_Found_Working_Remarks { get; set; }

        [Display(Name = "7.1.1 Section-7 Is interval or rest of half an hour after every three hours working is being given to adolescent?")]
        [Required(ErrorMessage = "Sec_7_IsRestGiven_To_Adolescent is required..!")]
        public GeneralOptionTypeEnum Sec_7_IsRestGiven_To_Adolescent { get; set; }

        [Display(Name = "7.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_7_IsRestGiven_To_Adolescent_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_7_IsRestGiven_To_Adolescent_ViolationExist { get; set; }

        [Display(Name = "7.1.3 Sec_7_IsRestGiven_To_Adolescent_Remarks")]
        [Required(ErrorMessage = "Sec_7_IsRestGiven_To_Adolescent_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_7_IsRestGiven_To_Adolescent_Remarks can have max 1000 chars..!")]
        public string Sec_7_IsRestGiven_To_Adolescent_Remarks { get; set; }

        [Display(Name = "7.2.1 Is spread over (including interval/rest period) is not more than six hours on any day")]
        [Required(ErrorMessage = "Sec_7_IsRestGiven_To_Adolescent is required..!")]
        public GeneralOptionTypeEnum Sec_7_IsWorkPeriod_NotMoreThen_6_Hours { get; set; }

        [Display(Name = "7.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_7_IsWorkPeriod_NotMoreThen_6_Hours_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_7_IsWorkPeriod_NotMoreThen_6_Hours_ViolationExist { get; set; }

        [Display(Name = "7.2.3 Sec_7_IsWorkPeriod_NotMoreThen_6_Hours_Remarks")]
        [Required(ErrorMessage = "Sec_7_IsWorkPeriod_NotMoreThen_6_Hours_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_7_IsWorkPeriod_NotMoreThen_6_Hours_Remarks can have max 1000 chars..!")]
        public string Sec_7_IsWorkPeriod_NotMoreThen_6_Hours_Remarks { get; set; }

        [Display(Name = "7.3.1 Is any adolescent was working between 7PM and 8 AM")]
        [Required(ErrorMessage = "Sec_7_IsAny_Adolescent_Work_Between_7_PM_To_8_AM is required..!")]
        public GeneralOptionTypeEnum Sec_7_IsAny_Adolescent_Work_Between_7_PM_To_8_AM { get; set; }

        
        [Display(Name = "7.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_7_IsAny_Adolescent_Work_Between_7_PM_To_8_AM_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_7_IsAny_Adolescent_Work_Between_7_PM_To_8_AM_ViolationExist { get; set; }

        [Display(Name = "7.3.3 Sec_7_IsAny_Adolescent_Work_Between_7_PM_To_8_AM_Remarks")]
        [Required(ErrorMessage = "Sec_7_IsAny_Adolescent_Work_Between_7_PM_To_8_AM_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_7_IsAny_Adolescent_Work_Between_7_PM_To_8_AM_Remarks can have max 1000 chars..!")]
        public string Sec_7_IsAny_Adolescent_Work_Between_7_PM_To_8_AM_Remarks { get; set; }


        [Display(Name = "7.4.1 Is any adolescent was working on overtime?")]
        [Required(ErrorMessage = "Sec_7_IsAny_Adolescent_Working_Overtime is required..!")]
        public GeneralOptionTypeEnum Sec_7_IsAny_Adolescent_Working_Overtime { get; set; }

        [Display(Name = "7.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_7_IsAny_Adolescent_Working_Overtime_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_7_IsAny_Adolescent_Working_Overtime_ViolationExist { get; set; }

        [Display(Name = "7.4.3 Sec_7_IsAny_Adolescent_Working_Overtime_Remarks")]
        [Required(ErrorMessage = "Sec_7_IsAny_Adolescent_Working_Overtime_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_7_IsAny_Adolescent_Working_Overtime_Remarks can have max 1000 chars..!")]
        public string Sec_7_IsAny_Adolescent_Working_Overtime_Remarks { get; set; }

        [Display(Name = "8.1.1 Section-8 Is weekly holiday given to adolescent as fixed by employer?")]
        [Required(ErrorMessage = "Sec_8_Is_Weekly_Holidays_Given_To_Adolescent is required..!")]
        public GeneralOptionTypeEnum Sec_8_Is_Weekly_Holidays_Given_To_Adolescent { get; set; }
        
        [Display(Name = "8.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_Is_Weekly_Holidays_Given_To_Adolescent_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_Is_Weekly_Holidays_Given_To_Adolescent_ViolationExist { get; set; }

        [Display(Name = "8.1.3 Sec_8_Is_Weekly_Holidays_Given_To_Adolescent_Remarks")]
        [Required(ErrorMessage = "Sec_8_Is_Weekly_Holidays_Given_To_Adolescent_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_Is_Weekly_Holidays_Given_To_Adolescent_Remarks can have max 1000 chars..!")]
        public string Sec_8_Is_Weekly_Holidays_Given_To_Adolescent_Remarks { get; set; }

        [Display(Name = "8.2.1 Is notice of weekly holiday given to adolescent  is exhibited at conspicuous place?")]
        [Required(ErrorMessage = "Sec_8_Is_Notice_Of_Weekly_Holidays_Given_To_Adolescent is required..!")]
        public GeneralOptionTypeEnum Sec_8_Is_Notice_Of_Weekly_Holidays_Given_To_Adolescent { get; set; }

        [Display(Name = "8.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_Is_Notice_Of_Weekly_Holidays_Given_To_Adolescent_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_Is_Notice_Of_Weekly_Holidays_Given_To_Adolescent_ViolationExist { get; set; }

        [Display(Name = "8.2.3 Sec_8_Is_Notice_Of_Weekly_Holidays_Given_To_Adolescent_Remarks")]
        [Required(ErrorMessage = "Sec_8_Is_Notice_Of_Weekly_Holidays_Given_To_Adolescent_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_Is_Notice_Of_Weekly_Holidays_Given_To_Adolescent_Remarks can have max 1000 chars..!")]
        public string Sec_8_Is_Notice_Of_Weekly_Holidays_Given_To_Adolescent_Remarks { get; set; }

        [Display(Name = "8.3.1 Is notice altered within three months?")]
        [Required(ErrorMessage = "Sec_8_Is_Notice_Altered_Within_3_months is required..!")]
        public GeneralOptionTypeEnum Sec_8_Is_Notice_Altered_Within_3_months { get; set; }

        [Display(Name = "8.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_Is_Notice_Altered_Within_3_months_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_Is_Notice_Altered_Within_3_months_ViolationExist { get; set; }

        [Display(Name = "8.3.3 Sec_8_Is_Notice_Altered_Within_3_months_Remarks")]
        [Required(ErrorMessage = "Sec_8_Is_Notice_Altered_Within_3_months_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_Is_Notice_Altered_Within_3_months_Remarks can have max 1000 chars..!")]
        public string Sec_8_Is_Notice_Altered_Within_3_months_Remarks { get; set; }

        [Display(Name = "9.1.1 Section-9 Is notice sent to inspector?")]
        [Required(ErrorMessage = "Sec_9_Is_Notice_Sent_To_Inspector is required..!")]
        public GeneralOptionTypeEnum Sec_9_Is_Notice_Sent_To_Inspector { get; set; }

        [Display(Name = "9.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_9_Is_Notice_Sent_To_Inspector_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_9_Is_Notice_Sent_To_Inspector_ViolationExist { get; set; }

        [Display(Name = "9.1.3 Sec_9_Is_Notice_Sent_To_Inspector_Remarks")]
        [Required(ErrorMessage = "Sec_9_Is_Notice_Sent_To_Inspector_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_9_Is_Notice_Sent_To_Inspector_Remarks can have max 1000 chars..!")]
        public string Sec_9_Is_Notice_Sent_To_Inspector_Remarks { get; set; }

        [Display(Name = "11.1.1 Section-11 Are prescribed register being maintained?")]
        [Required(ErrorMessage = "Sec_11_Is_Prescribed_Register_Maintained is required..!")]
        public GeneralOptionTypeEnum Sec_11_Is_Prescribed_Register_Maintained { get; set; }

        [Display(Name = "11.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_11_Is_Prescribed_Register_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_11_Is_Prescribed_Register_Maintained_ViolationExist { get; set; }

        [Display(Name = "11.1.3 Sec_11_Is_Prescribed_Register_Maintained_Remarks")]
        [Required(ErrorMessage = "Sec_11_Is_Prescribed_Register_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_Is_Prescribed_Register_Maintained_Remarks can have max 1000 chars..!")]
        public string Sec_11_Is_Prescribed_Register_Maintained_Remarks { get; set; }

        [Display(Name = "12.1.1 Section-12 Is notice of abstract containing section 3A and 14 is displayed at conspicuous place?")]
        [Required(ErrorMessage = "Is_NoticeOfAbstract_Section_3A_And_14_displayed is required..!")]
        public GeneralOptionTypeEnum Sec_12_Is_NoticeOfAbstract_Section_3A_And_14_displayed { get; set; }

        [Display(Name = "12.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_12_Is_NoticeOfAbstract_Section_3A_And_14_displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_12_Is_NoticeOfAbstract_Section_3A_And_14_displayed_ViolationExist { get; set; }

        [Display(Name = "12.1.3 Sec_12_NoticeOfAbstract_Section_3A_And_14_Remarks")]
        [Required(ErrorMessage = "Sec_12_NoticeOfAbstract_Section_3A_And_14_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_Is_Prescribed_Register_Maintained_Remarks can have max 1000 chars..!")]
        public string Sec_12_Is_NoticeOfAbstract_Section_3A_And_14_displayed_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_NationalAndFestivalHolidays
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "Rule-3.1.1 Rule-3 Are festival holidays being decided before  30th  November every year for the ensuing calender year?")]
        [Required(ErrorMessage = "Rule_3_IsFestivalHolidays_Decided_Before_30th_November is required..!")]
        public GeneralOptionTypeEnum Rule_3_IsFestivalHolidays_Decided_Before_30th_November { get; set; }

        [Display(Name = "Rule-3.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_3_IsFestivalHolidays_Decided_Before_30th_November_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_3_IsFestivalHolidays_Decided_Before_30th_November_ViolationExist { get; set; }

        [Display(Name = "Rule-3.1.3 Rule_3_IsFestivalHolidays_Decided_Before_30th_November_Remarks")]
        [Required(ErrorMessage = "Rule_3_IsFestivalHolidays_Decided_Before_30th_November_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_3_IsFestivalHolidays_Decided_Before_30th_November_Remarks can have max 1000 chars..!")]
        public string Rule_3_IsFestivalHolidays_Decided_Before_30th_November_Remarks { get; set; }

        [Display(Name = "Rule-3.2.1 Are workers informed in Form A  before 31st  December about the festival holidays decided by exhibiting copy on notice board?")]
        [Required(ErrorMessage = "Rule_3_IsFestivalHolidays_Notification_Share_Before_31th_Dec is required..!")]
        public GeneralOptionTypeEnum Rule_3_IsFestivalHolidays_Notification_Share_Before_31th_Dec { get; set; }

        [Display(Name = "Rule-3.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_3_IsFestivalHolidays_Notification_Share_Before_31th_Dec_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_3_IsFestivalHolidays_Notification_Share_Before_31th_Dec_ViolationExist { get; set; }

        [Display(Name = "Rule-3.2.3 Rule_3_IsFestivalHolidays_Notification_Share_Before_31th_Dec_Remarks")]
        [Required(ErrorMessage = "Rule_3_IsFestivalHolidays_Notification_Share_Before_31th_Dec_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_3_IsFestivalHolidays_Notification_Share_Before_31th_Dec_Remarks can have max 1000 chars..!")]
        public string Rule_3_IsFestivalHolidays_Notification_Share_Before_31th_Dec_Remarks { get; set; }

        [Display(Name = "Rule-3.3.1 Is copy of festival holidays decided in Form A  is sent to inspector before 31st  December of year?")]
        [Required(ErrorMessage = "Rule_3_Is_Festival_Holidays_Copy_Sent_To_Inspector_Before_31th_Dec is required..!")]
        public GeneralOptionTypeEnum Rule_3_Is_Festival_Holidays_Copy_Sent_To_Inspector_Before_31th_Dec { get; set; }

        [Display(Name = "Rule-3.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_3_Is_Festival_Holidays_Copy_Sent_To_Inspector_Before_31th_Dec_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_3_Is_Festival_Holidays_Copy_Sent_To_Inspector_Before_31th_Dec_ViolationExist { get; set; }

        [Display(Name = "Rule-3.3.3 Rule_3_Festival_Holidays_Copy_Sent_To_Inspector_Remarks")]
        [Required(ErrorMessage = "Rule_3_Festival_Holidays_Copy_Sent_To_Inspector_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_3_Festival_Holidays_Copy_Sent_To_Inspector_Remarks can have max 1000 chars..!")]
        public string Rule_3_Is_Festival_Holidays_Copy_Sent_To_Inspector_Before_31th_Dec_Remarks { get; set; }

        [Display(Name = "Rule-4.1.1 Rule-4 Is election conducted as per provision of this rule?")]
        [Required(ErrorMessage = "Rule_4_Is_Election_Conducted is required..!")]
        public GeneralOptionTypeEnum Rule_4_Is_Election_Conducted { get; set; }

        [Display(Name = "Rule-4.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_4_Is_Election_Conducted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_4_Is_Election_Conducted_ViolationExist { get; set; }

        [Display(Name = "Rule-4.1.3 Rule_4_Is_Election_Conducted_Remarks")]
        [Required(ErrorMessage = "Rule_4_Is_Election_Conducted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_4_Is_Election_Conducted_Remarks can have max 1000 chars..!")]
        public string Rule_4_Is_Election_Conducted_Remarks { get; set; }

        [Display(Name = "Rule-7.1.1 Rule-7 Is register maintained in Form B")]
        [Required(ErrorMessage = "Rule_7_Is_Form_B_Register_Maintained is required..!")]
        public GeneralOptionTypeEnum Rule_7_Is_Form_B_Register_Maintained { get; set; }

        [Display(Name = "Rule-7.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_7_Is_Form_B_Register_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_7_Is_Form_B_Register_Maintained_ViolationExist { get; set; }

        [Display(Name = "Rule-7.1.3 Rule_7_Is_Form_B_Register_Maintained_Remarks")]
        [Required(ErrorMessage = "Rule_7_Is_Form_B_Register_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_7_Is_Form_B_Register_Maintained_Remarks can have max 1000 chars..!")]
        public string Rule_7_Is_Form_B_Register_Maintained_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_MaternityBenefitAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "8.1.1 Section-8 Rule-6(4) Is payment of medical bonus made?")]
        [Required(ErrorMessage = "Sec_8_Rule_6_4_Is_Payment_Of_Medical_Bonus_Paid is required..!")]
        public GeneralOptionTypeEnum Sec_8_Rule_6_4_Is_Payment_Of_Medical_Bonus_Paid { get; set; }

        [Display(Name = "8.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_8_Rule_6_4_Is_Payment_Of_Medical_Bonus_Paid_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_8_Rule_6_4_Is_Payment_Of_Medical_Bonus_Paid_ViolationExist { get; set; }

        [Display(Name = "8.1.3 Sec_8_Rule_6_4_Is_Payment_Of_Medical_Bonus_Paid_Remarks")]
        [Required(ErrorMessage = "Sec_8_Rule_6_4_Is_Payment_Of_Medical_Bonus_Paid_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_8_Rule_6_4_Is_Payment_Of_Medical_Bonus_Paid_Remarks can have max 1000 chars..!")]
        public string Sec_8_Rule_6_4_Is_Payment_Of_Medical_Bonus_Paid_Remarks { get; set; }

        [Display(Name = "9.1.1 Section-9 Is leave for miscarriage granted?")]
        [Required(ErrorMessage = "Sec_9_Is_Leave_For_Miscarriage_Granted is required..!")]
        public GeneralOptionTypeEnum Sec_9_Is_Leave_For_Miscarriage_Granted { get; set; }

        [Display(Name = "9.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_9_Is_Leave_For_Miscarriage_Granted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_9_Is_Leave_For_Miscarriage_Granted_ViolationExist { get; set; }

        [Display(Name = "9.1.3 Sec_9_Is_Leave_For_Miscarriage_Granted_Remarks")]
        [Required(ErrorMessage = "Sec_9_Is_Leave_For_Miscarriage_Granted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_9_Is_Leave_For_Miscarriage_Granted_Remarks can have max 1000 chars..!")]
        public string Sec_9_Is_Leave_For_Miscarriage_Granted_Remarks { get; set; }

        [Display(Name = "9A.1.1 Section-9A Is leave with wages in case of tubectomy granted?")]
        [Required(ErrorMessage = "Sec_9_A_Is_Leave_with_Wages_Granted_In_Case_Of_tubectomy is required..!")]
        public GeneralOptionTypeEnum Sec_9_A_Is_Leave_with_Wages_Granted_In_Case_Of_tubectomy { get; set; }

        [Display(Name = "9A.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_9_A_Is_Leave_with_Wages_Granted_In_Case_Of_tubectomy_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_9_A_Is_Leave_with_Wages_Granted_In_Case_Of_tubectomy_ViolationExist { get; set; }

        [Display(Name = "9A.1.3 Sec_9_A_Is_Leave_with_Wages_Granted_In_Case_Of_tubectomy_Remarks")]
        [Required(ErrorMessage = "Sec_9_A_Is_Leave_with_Wages_Granted_In_Case_Of_tubectomy_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_9_A_Is_Leave_with_Wages_Granted_In_Case_Of_tubectomy_Remarks can have max 1000 chars..!")]
        public string Sec_9_A_Is_Leave_with_Wages_Granted_In_Case_Of_tubectomy_Remarks { get; set; }

        [Display(Name = "10.1.1 Section-10 Rule-6(6) Is leave for illness arising out of pregnancy, delivery, premature birth of child, or miscarriage granted?")]
        [Required(ErrorMessage = "Sec_10_Role_6_Is_Leave_Granted_For_Illness_Arising_Out_Of_Pregnancy is required..!")]
        public GeneralOptionTypeEnum Sec_10_Role_6_Is_Leave_Granted_For_Illness_Arising_Out_Of_Pregnancy { get; set; }

        [Display(Name = "10.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_10_Role_6_Is_Leave_Granted_For_Illness_Arising_Out_Of_Pregnancy_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_10_Role_6_Is_Leave_Granted_For_Illness_Arising_Out_Of_Pregnancy_ViolationExist { get; set; }

        [Display(Name = "10.1.3 Sec_10_Role_6_Is_Leave_Granted_For_Illness_Arising_Out_Of_Pregnancy_Remarks")]
        [Required(ErrorMessage = "Sec_10_Role_6_Is_Leave_Granted_For_Illness_Arising_Out_Of_Pregnancy_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_10_Role_6_Is_Leave_Granted_For_Illness_Arising_Out_Of_Pregnancy_Remarks can have max 1000 chars..!")]
        public string Sec_10_Role_6_Is_Leave_Granted_For_Illness_Arising_Out_Of_Pregnancy_Remarks { get; set; }

        [Display(Name = "11.1.1 Section-11, Rule-7 Are nursing breaks of 20 minutes given?")]
        [Required(ErrorMessage = "Sec_11_Role_7_Is_Nursing_Breaks_Given_Of_20_Minutes is required..!")]
        public GeneralOptionTypeEnum Sec_11_Role_7_Is_Nursing_Breaks_Given_Of_20_Minutes { get; set; }

        [Display(Name = "11.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_11_Role_7_Is_Nursing_Breaks_Given_Of_20_Minutes_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_11_Role_7_Is_Nursing_Breaks_Given_Of_20_Minutes_ViolationExist { get; set; }

        [Display(Name = "11.1.3 Sec_11_Role_7_Is_Nursing_Breaks_Given_Of_20_Minutes_Remarks")]
        [Required(ErrorMessage = "Sec_11_Role_7_Is_Nursing_Breaks_Given_Of_20_Minutes_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_Role_7_Is_Nursing_Breaks_Given_Of_20_Minutes_Remarks can have max 1000 chars..!")]
        public string Sec_11_Role_7_Is_Nursing_Breaks_Given_Of_20_Minutes_Remarks { get; set; }

        [Display(Name = "11A.1.1 Section-11 A Is creche facility provide?")]
        [Required(ErrorMessage = "Sec_11_A_Is_Creche_Facility_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_11_A_Is_Creche_Facility_Provided { get; set; }

        [Display(Name = "11.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_11_A_Is_Creche_Facility_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_11_A_Is_Creche_Facility_Provided_ViolationExist { get; set; }

        [Display(Name = "11A.1.3 Sec_11_A_Is_Creche_Facility_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_11_A_Is_Creche_Facility_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_11_A_Is_Creche_Facility_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_11_A_Is_Creche_Facility_Provided_Remarks { get; set; }

        [Display(Name = "12.1.1 Section-12 Is there any dismissal during absence of pregnancy?")]
        [Required(ErrorMessage = "Sec_12_Is_Any_Dismissal_During_Absence_Of_Pregnancy is required..!")]
        public GeneralOptionTypeEnum Sec_12_Is_Any_Dismissal_During_Absence_Of_Pregnancy { get; set; }

        [Display(Name = "12.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_12_Is_Any_Dismissal_During_Absence_Of_Pregnancy_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_12_Is_Any_Dismissal_During_Absence_Of_Pregnancy_ViolationExist { get; set; }

        [Display(Name = "12.1.3 Sec_12_Is_Any_Dismissal_During_Absence_Of_Pregnancy_Remarks")]
        [Required(ErrorMessage = "Sec_12_Is_Any_Dismissal_During_Absence_Of_Pregnancy_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_12_Is_Any_Dismissal_During_Absence_Of_Pregnancy_Remarks can have max 1000 chars..!")]
        public string Sec_12_Is_Any_Dismissal_During_Absence_Of_Pregnancy_Remarks { get; set; }

        [Display(Name = "19.1.1 Section-19, Rule-15 Is abstract of Act and rules in Form K displayed?")]
        [Required(ErrorMessage = "Sec_19_Role_15_Is_Act_And_Rules_In_Form_K_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_19_Role_15_Is_Act_And_Rules_In_Form_K_Displayed { get; set; }

        [Display(Name = "19.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Role_15_Is_Act_And_Rules_In_Form_K_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Role_15_Is_Act_And_Rules_In_Form_K_Displayed_ViolationExist { get; set; }

        [Display(Name = "19.1.3 Sec_19_Role_15_Is_Act_And_Rules_In_Form_K_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_19_Role_15_Is_Act_And_Rules_In_Form_K_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Role_15_Is_Act_And_Rules_In_Form_K_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_19_Role_15_Is_Act_And_Rules_In_Form_K_Displayed_Remarks { get; set; }

        [Display(Name = "20.1.1 Section-20 Rule-3 Is muster roll maintained in Form A")]
        [Required(ErrorMessage = "Sec_20_Rule_3_IsMusterRoll_Form_A_Maintained is required..!")]
        public GeneralOptionTypeEnum Sec_20_Rule_3_IsMusterRoll_Form_A_Maintained { get; set; }

        [Display(Name = "20.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_20_Rule_3_IsMusterRoll_Form_A_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_20_Rule_3_IsMusterRoll_Form_A_Maintained_ViolationExist { get; set; }

        [Display(Name = "20.1.3 Sec_20_Rule_3_IsMusterRoll_Form_A_Maintained_Remarks")]
        [Required(ErrorMessage = "Sec_20_Rule_3_IsMusterRoll_Form_A_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_20_Rule_3_IsMusterRoll_Form_A_Maintained_Remarks can have max 1000 chars..!")]
        public string Sec_20_Rule_3_IsMusterRoll_Form_A_Maintained_Remarks { get; set; }

        [Display(Name = "Rule-14.1.1 Rule-14 Is record preserved for three years after date of  last entry made in therein?")]
        [Required(ErrorMessage = "Rule_14_Is_Record_Preserved_For_Three_Years is required..!")]
        public GeneralOptionTypeEnum Rule_14_Is_Record_Preserved_For_Three_Years { get; set; }

        [Display(Name = "Rule-14.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_14_Is_Record_Preserved_For_Three_Years_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_14_Is_Record_Preserved_For_Three_Years_ViolationExist { get; set; }

        [Display(Name = "Rule-14.1.3 Rule_14_Is_Record_Preserved_For_Three_Years_Remarks")]
        [Required(ErrorMessage = "Rule_14_Is_Record_Preserved_For_Three_Years_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_14_Is_Record_Preserved_For_Three_Years_Remarks can have max 1000 chars..!")]
        public string Rule_14_Is_Record_Preserved_For_Three_Years_Remarks { get; set; }

        [Display(Name = "Rule-16.1.1 Rule-16 Is annual return in Form L,M,N and O are submitted?")]
        [Required(ErrorMessage = "Rule_16_Is_Annual_Return_Form_L_M_N_O_Submitted is required..!")]
        public GeneralOptionTypeEnum Rule_16_Is_Annual_Return_Form_L_M_N_O_Submitted { get; set; }

        [Display(Name = "16.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_16_Is_Annual_Return_Form_L_M_N_O_Submitted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_16_Is_Annual_Return_Form_L_M_N_O_Submitted_ViolationExist { get; set; }

        [Display(Name = "Rule-16.1.3 Rule_16_Is_Annual_Return_Form_L_M_N_O_Submitted_Remarks")]
        [Required(ErrorMessage = "Rule_16_Is_Annual_Return_Form_L_M_N_O_Submitted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_16_Is_Annual_Return_Form_L_M_N_O_Submitted_Remarks can have max 1000 chars..!")]
        public string Rule_16_Is_Annual_Return_Form_L_M_N_O_Submitted_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_ContractLabourAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "16.42.1.1 Section-16, Rule-42 Is canteen provided and maintained in efficient manner?")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Canteen_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_42_Is_Canteen_Provided { get; set; }

        [Display(Name = "16.42.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Canteen_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_42_Is_Canteen_Provided_ViolationExist { get; set; }

        [Display(Name = "16.42.1.3 Sec_16_Rule_42_Is_Canteen_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Canteen_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_42_Is_Canteen_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_42_Is_Canteen_Provided_Remarks { get; set; }

        [Display(Name = "16.43.1.1 Section-16, Rule-43 Is canteen consist of dinning hall, kitchen, storeroom, pantry and washing place for workers and utensils?")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_43_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace { get; set; }

        [Display(Name = "16.43.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_43_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_ViolationExist { get; set; }
        
        [Display(Name = "16.43.1.3 Sec_16_Rule_43_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_43_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_Remarks { get; set; }

        [Display(Name = "16.43.2.1 Is canteen sufficiently lighted?")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Sufficiently_Lighted is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_43_Is_Canteen_Sufficiently_Lighted { get; set; }

        [Display(Name = "16.43.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Sufficiently_Lighted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_43_Is_Canteen_Sufficiently_Lighted_ViolationExist { get; set; }

        [Display(Name = "16.43.2.3 Sec_16_Rule_43_Is_Canteen_Sufficiently_Lighted_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Sufficiently_Lighted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Sufficiently_Lighted_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_43_Is_Canteen_Sufficiently_Lighted_Remarks { get; set; }

        [Display(Name = "16.43.3.1 Is floor of canteen is of impervious material?")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Floor_Impervious_Material is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_43_Is_Canteen_Floor_Impervious_Material { get; set; }

        [Display(Name = "16.43.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Floor_Impervious_Material_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_43_Is_Canteen_Floor_Impervious_Material_ViolationExist { get; set; }

        [Display(Name = "16.43.3.3 Sec_16_Rule_43_Is_Canteen_Floor_Impervious_Material_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Floor_Impervious_Material_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Floor_Impervious_Material_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_43_Is_Canteen_Floor_Impervious_Material_Remarks { get; set; }

        [Display(Name = "16.43.4.1 Are inside walls of canteen are lime washed once in each year?")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Wall_Lime_Washed_Once_InYear is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_43_Is_Canteen_Wall_Lime_Washed_Once_InYear { get; set; }

        [Display(Name = "16.43.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Wall_Lime_Washed_Once_InYear_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_43_Is_Canteen_Wall_Lime_Washed_Once_InYear_ViolationExist { get; set; }

        [Display(Name = "16.43.4.3 Sec_16_Rule_43_Is_Canteen_Wall_Lime_Washed_Once_InYear_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Wall_Lime_Washed_Once_InYear_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_43_Is_Canteen_Wall_Lime_Washed_Once_InYear_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_43_Is_Canteen_Wall_Lime_Washed_Once_InYear_Remarks { get; set; }

        [Display(Name = "16.43.5.1 Are inside walls of kitchen lime washed every four months?")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_43_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month { get; set; }

        [Display(Name = "16.43.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_43_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_ViolationExist { get; set; }

        [Display(Name = "16.43.5.3 Sec_16_Rule_43_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_43_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_43_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_Remarks { get; set; }

        [Display(Name = "16.43.6.1 Is precincts of canteen in clean and sanitary conditions?")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Precincts_Of_Canteen_In_Clean is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_43_Is_Precincts_Of_Canteen_In_Clean { get; set; }

        [Display(Name = "16.43.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Precincts_Of_Canteen_In_Clean_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_43_Is_Precincts_Of_Canteen_In_Clean_ViolationExist { get; set; }

        [Display(Name = "16.43.6.3 Sec_16_Rule_43_Is_Precincts_Of_Canteen_In_Clean_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Precincts_Of_Canteen_In_Clean_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_43_Is_Precincts_Of_Canteen_In_Clean_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_43_Is_Precincts_Of_Canteen_In_Clean_Remarks { get; set; }

        [Display(Name = "16.43.7.1 Is wastewater and garbage is disposed properly?")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Wastewater_And_Garbage_Disposed is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_43_Is_Wastewater_And_Garbage_Disposed { get; set; }

        [Display(Name = "16.43.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Wastewater_And_Garbage_Disposed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_43_Is_Wastewater_And_Garbage_Disposed_ViolationExist { get; set; }

        [Display(Name = "16.43.7.3 Sec_16_Rule_43_Is_Wastewater_And_Garbage_Disposed_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Wastewater_And_Garbage_Disposed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_43_Is_Wastewater_And_Garbage_Disposed_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_43_Is_Wastewater_And_Garbage_Disposed_Remarks { get; set; }

        [Display(Name = "16.44.1.1 Section-16, Rule-44 Is dining hall accommodating 30% of contract labour working at a time?")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_DiningHall_Accommodates_30Percent_ContractLabour is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_44_Is_DiningHall_Accommodates_30Percent_ContractLabour { get; set; }

        [Display(Name = "16.44.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_DiningHall_Accommodates_30Percent_ContractLabour_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_44_Is_DiningHall_Accommodates_30Percent_ContractLabour_ViolationExist { get; set; }

        [Display(Name = "16.44.1.3 Sec_16_Rule_44_Is_DiningHall_Accommodates_30Percent_ContractLabour_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_DiningHall_Accommodates_30Percent_ContractLabour_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_44_Is_DiningHall_Accommodates_30Percent_ContractLabour_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_44_Is_DiningHall_Accommodates_30Percent_ContractLabour_Remarks { get; set; }

        [Display(Name = "16.44.2.1 Is area of 1 m2 is available every diner?")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Area_Of_1_Mtr_Sqr_Available is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_44_Is_Area_Of_1_Mtr_Sqr_Available { get; set; }

        [Display(Name = "16.44.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Area_Of_1_Mtr_Sqr_Available_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_44_Is_Area_Of_1_Mtr_Sqr_Available_ViolationExist { get; set; }

        [Display(Name = "16.44.2.3 Sec_16_Rule_44_Is_Area_Of_1_Mtr_Sqr_Available_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Area_Of_1_Mtr_Sqr_Available_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_44_Is_Area_Of_1_Mtr_Sqr_Available_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_44_Is_Area_Of_1_Mtr_Sqr_Available_Remarks { get; set; }

        [Display(Name = "16.44.3.1 Is portion of dinning hall and service counter is reserved for women workers?")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Portion_Of_Dinning_Reserved_For_Women is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_44_Is_Portion_Of_Dinning_Reserved_For_Women { get; set; }

        [Display(Name = "16.44.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Portion_Of_Dinning_Reserved_For_Women_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_44_Is_Portion_Of_Dinning_Reserved_For_Women_ViolationExist { get; set; }

        [Display(Name = "16.44.3.3 Sec_16_Rule_44_Is_Portion_Of_Dinning_Reserved_For_Women_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Portion_Of_Dinning_Reserved_For_Women_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_44_Is_Portion_Of_Dinning_Reserved_For_Women_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_44_Is_Portion_Of_Dinning_Reserved_For_Women_Remarks { get; set; }

        [Display(Name = "16.44.4.1 Is separate washing place with privacy is provided?")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Separate_Washing_Place_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_44_Is_Separate_Washing_Place_Provided { get; set; }

        [Display(Name = "16.44.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Separate_Washing_Place_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_44_Is_Separate_Washing_Place_Provided_ViolationExist { get; set; }

        [Display(Name = "16.44.4.3 Sec_16_Rule_44_Is_Separate_Washing_Place_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Separate_Washing_Place_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_44_Is_Separate_Washing_Place_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_44_Is_Separate_Washing_Place_Provided_Remarks { get; set; }

        [Display(Name = "16.45.1.1 Section-16, Rule-45 Are provisions of this rule being complied?")]
        [Required(ErrorMessage = "Sec_16_Rule_45_Is_Provisions_Of_Rule_Complied is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_45_Is_Provisions_Of_Rule_Complied { get; set; }

        [Display(Name = "16.45.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_45_Is_Provisions_Of_Rule_Complied_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_45_Is_Provisions_Of_Rule_Complied_ViolationExist { get; set; }

        [Display(Name = "16.45.1.3 Sec_16_Rule_45_Is_Provisions_Of_Rule_Complied_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_45_Is_Provisions_Of_Rule_Complied_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_45_Is_Provisions_Of_Rule_Complied_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_45_Is_Provisions_Of_Rule_Complied_Remarks { get; set; }

        [Display(Name = "16.46.1.1 Section-16, Rule-46 Are foodstuff meeting the requirements of natural habits of contract labour?")]
        [Required(ErrorMessage = "Sec_16_Rule_46_Is_FoodStuffs_Meets_habits_Of_ContractLabour is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_46_Is_FoodStuffs_Meets_habits_Of_ContractLabour { get; set; }

        [Display(Name = "16.46.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_46_Is_FoodStuffs_Meets_habits_Of_ContractLabour_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_46_Is_FoodStuffs_Meets_habits_Of_ContractLabour_ViolationExist { get; set; }

        [Display(Name = "16.46.1.3 Sec_16_Rule_46_Is_FoodStuffs_Meets_habits_Of_ContractLabour_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_46_Is_FoodStuffs_Meets_habits_Of_ContractLabour_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_46_Is_FoodStuffs_Meets_habits_Of_ContractLabour_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_46_Is_FoodStuffs_Meets_habits_Of_ContractLabour_Remarks { get; set; }

        [Display(Name = "16.47.1.1 Section-16, Rule-47 Is canteen running at “NO PROFIT NO LOSS”?")]
        [Required(ErrorMessage = "Sec_16_Rule_47_Is_Canteen_Running_No_Profit_No_Loss is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_47_Is_Canteen_Running_No_Profit_No_Loss { get; set; }

        [Display(Name = "16.47.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_47_Is_Canteen_Running_No_Profit_No_Loss_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_47_Is_Canteen_Running_No_Profit_No_Loss_ViolationExist { get; set; }

        [Display(Name = "16.47.1.3 Sec_16_Rule_47_Is_Canteen_Running_No_Profit_No_Loss_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_47_Is_Canteen_Running_No_Profit_No_Loss_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_47_Is_Canteen_Running_No_Profit_No_Loss_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_47_Is_Canteen_Running_No_Profit_No_Loss_Remarks { get; set; }

        [Display(Name = "16.49.1.1 Section-16, Rule-49 Are account books and other records produced during inspection?")]
        [Required(ErrorMessage = "Sec_16_Rule_49_Is_AccountBook_Produced_While_Inspection is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_49_Is_AccountBook_Produced_While_Inspection { get; set; }

        [Display(Name = "16.49.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_49_Is_AccountBook_Produced_While_Inspection_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_49_Is_AccountBook_Produced_While_Inspection_ViolationExist { get; set; }

        [Display(Name = "16.49.1.3 Sec_16_Rule_49_Is_AccountBook_Produced_While_Inspection_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_49_Is_AccountBook_Produced_While_Inspection_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_49_Is_AccountBook_Produced_While_Inspection_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_49_Is_AccountBook_Produced_While_Inspection_Remarks { get; set; }

        [Display(Name = "16.50.1.1 Section-16, Rule-50 Are accounts of canteen audit every 12 months by registered accountant and auditors?")]
        [Required(ErrorMessage = "Sec_16_Rule_50_Is_Canteen_Audit_Performed is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_50_Is_Canteen_Audit_Performed { get; set; }

        [Display(Name = "16.49.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_50_Is_Canteen_Audit_Performed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_50_Is_Canteen_Audit_Performed_ViolationExist { get; set; }

        [Display(Name = "16.50.1.3 Sec_16_Rule_50_Is_Canteen_Audit_Performed_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_50_Is_Canteen_Audit_Performed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_50_Is_Canteen_Audit_Performed_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_50_Is_Canteen_Audit_Performed_Remarks { get; set; }

        [Display(Name = "17.41.1.1 Section-17, Rule-41 Are restrooms provide as per provisions under this section and rule?")]
        [Required(ErrorMessage = "Sec_17_Rule_41_Is_Restroom_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_17_Rule_41_Is_Restroom_Provided { get; set; }

        [Display(Name = "17.41.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_50_Is_Canteen_Audit_Performed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_17_Rule_41_Is_Restroom_Provided_ViolationExist { get; set; }

        [Display(Name = "17.41.1.3 Sec_17_Rule_41_Is_Restroom_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_17_Rule_41_Is_Restroom_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_17_Rule_41_Is_Restroom_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_17_Rule_41_Is_Restroom_Provided_Remarks { get; set; }

        [Display(Name = "18.40.1.1 Section-18, Rule-40 Is sufficient supply of wholesome drinking water is available to all workers?")]
        [Required(ErrorMessage = "Sec_18_Rule_40_Is_Sufficient_Drinking_Water_Available is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_40_Is_Sufficient_Drinking_Water_Available { get; set; }

        [Display(Name = "18.40.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_40_Is_Sufficient_Drinking_Water_Available_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_40_Is_Sufficient_Drinking_Water_Available_ViolationExist { get; set; }

        [Display(Name = "18.40.1.3 Sec_18_Rule_40_Is_Sufficient_Drinking_Water_Available_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_40_Is_Sufficient_Drinking_Water_Available_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_40_Is_Sufficient_Drinking_Water_Available_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_40_Is_Sufficient_Drinking_Water_Available_Remarks { get; set; }

        [Display(Name = "18.51.1.1 Section-18, Rule-51,52 Are latrines provided as per provisions of rule-51?")]
        [Required(ErrorMessage = "Sec_18_Rule_51_52_Is_Latrines_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_51_52_Is_Latrines_Provided { get; set; }

        [Display(Name = "18.51.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_51_52_Is_Latrines_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_51_52_Is_Latrines_Provided_ViolationExist { get; set; }

        [Display(Name = "18.51.1.3 Sec_18_Rule_51_52_Is_Latrines_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_51_52_Is_Latrines_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_51_52_Is_Latrines_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_51_52_Is_Latrines_Provided_Remarks { get; set; }

        [Display(Name = "18.53.1.1 Section-18, Rule-53 Are signboards with figures on latrines in language understood by workers displayed?")]
        [Required(ErrorMessage = "Sec_18_Rule_53_Is_Signboard_In_Latrines_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_53_Is_Signboard_In_Latrines_Displayed { get; set; }

        [Display(Name = "18.51.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_53_Is_Signboard_In_Latrines_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_53_Is_Signboard_In_Latrines_Displayed_ViolationExist { get; set; }

        [Display(Name = "18.53.1.3 Sec_18_Rule_53_Is_Signboard_In_Latrines_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_53_Is_Signboard_In_Latrines_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_53_Is_Signboard_In_Latrines_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_53_Is_Signboard_In_Latrines_Displayed_Remarks { get; set; }

        [Display(Name = "18.54.1.1 Section-18, Rule-54 Are Urinals provided as per provisions of rule-54?")]
        [Required(ErrorMessage = "Sec_18_Rule_54_Is_Urinals_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_54_Is_Urinals_Provided { get; set; }

        [Display(Name = "18.51.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_54_Is_Urinals_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_54_Is_Urinals_Provided_ViolationExist { get; set; }

        [Display(Name = "18.54.1.3 Sec_18_Rule_54_Is_Urinals_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_54_Is_Urinals_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_54_Is_Urinals_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_54_Is_Urinals_Provided_Remarks { get; set; }

        [Display(Name = "18.55.1.1 Section-18, Rule-55 Are latrine/Urinals adequately lighted?")]
        [Required(ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Adequately_Lighted is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_55_Is_Latrine_Urinals_Adequately_Lighted { get; set; }

        [Display(Name = "18.55.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_54_Is_Urinals_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_55_Is_Latrine_Urinals_Adequately_Lighted_ViolationExist { get; set; }

        [Display(Name = "18.55.1.3 Sec_18_Rule_55_Is_Latrine_Urinals_Adequately_Lighted_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Adequately_Lighted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Adequately_Lighted_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_55_Is_Latrine_Urinals_Adequately_Lighted_Remarks { get; set; }

        [Display(Name = "18.55.2.1 Are latrines/Urinals in clean and sanitary conditions?")]
        [Required(ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Clean is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_55_Is_Latrine_Urinals_Clean { get; set; }

        [Display(Name = "18.55.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Clean_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_55_Is_Latrine_Urinals_Clean_ViolationExist { get; set; }

        [Display(Name = "18.55.2.3 Sec_18_Rule_55_Is_Latrine_Urinals_Clean_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Clean_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Clean_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_55_Is_Latrine_Urinals_Clean_Remarks { get; set; }

        [Display(Name = "18.55.3.1 Are latrines/Urinals confirming to public health requirements?")]
        [Required(ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Public_Health_Requirements is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_55_Is_Latrine_Urinals_Public_Health_Requirements { get; set; }

        [Display(Name = "18.55.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Public_Health_Requirements_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_55_Is_Latrine_Urinals_Public_Health_Requirements_ViolationExist { get; set; }

        [Display(Name = "18.55.3.3 Sec_18_Rule_55_Is_Latrine_Urinals_Public_Health_Requirements_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Public_Health_Requirements_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_55_Is_Latrine_Urinals_Public_Health_Requirements_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_55_Is_Latrine_Urinals_Public_Health_Requirements_Remarks { get; set; }

        [Display(Name = "18.56.1.1 Section-18, Rule-56 Is water supply in latrine/Urinal is as per this rule?")]
        [Required(ErrorMessage = "Sec_18_Rule_56_Is_Water_Supply_In_Latrine_Urinals is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_56_Is_Water_Supply_In_Latrine_Urinals { get; set; }

        [Display(Name = "18.56.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_56_Is_Water_Supply_In_Latrine_Urinals_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_56_Is_Water_Supply_In_Latrine_Urinals_ViolationExist { get; set; }

        [Display(Name = "18.56.1.3 Sec_18_Rule_56_Water_Supply_In_Latrine_Urinals_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_56_Water_Supply_In_Latrine_Urinals_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_56_Water_Supply_In_Latrine_Urinals_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_56_Is_Water_Supply_In_Latrine_Urinals_Remarks { get; set; }

        [Display(Name = "18.57.1.1 Section-18, Rule-57 Are washing facilities are provided as per rule 57?")]
        [Required(ErrorMessage = "Sec_18_Rule_57_Is_Washing_Facilities_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_18_Rule_57_Is_Washing_Facilities_Provided { get; set; }

        [Display(Name = "18.57.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_18_Rule_57_Is_Washing_Facilities_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_18_Rule_57_Is_Washing_Facilities_Provided_ViolationExist { get; set; }

        [Display(Name = "18.57.1.3 Sec_18_Rule_57_Is_Washing_Facilities_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_18_Rule_57_Is_Washing_Facilities_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_18_Rule_57_Is_Washing_Facilities_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_18_Rule_57_Is_Washing_Facilities_Provided_Remarks { get; set; }

        [Display(Name = "19.58.1.1 Section-19, Rule-58 Are sufficient number of first aid boxes are provided and maintained?")]
        [Required(ErrorMessage = "Sec_19_Rule_58_Is_First_Aid_Boxes_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_19_Rule_58_Is_First_Aid_Boxes_Provided { get; set; }

        [Display(Name = "18.58.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Rule_58_Is_First_Aid_Boxes_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Rule_58_Is_First_Aid_Boxes_Provided_ViolationExist { get; set; }

        [Display(Name = "19.58.1.3 Sec_19_Rule_58_Is_First_Aid_Boxes_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_19_Rule_58_Is_First_Aid_Boxes_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Rule_58_Is_First_Aid_Boxes_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_19_Rule_58_Is_First_Aid_Boxes_Provided_Remarks { get; set; }

        [Display(Name = "19.59.1.1 Section-19, Rule-59,60 Are first aid box es  being maintained as per Rule 59 and 60?")]
        [Required(ErrorMessage = "Sec_19_Rule_58_60_Is_First_Aid_Boxes_Maintained is required..!")]
        public GeneralOptionTypeEnum Sec_19_Rule_58_60_Is_First_Aid_Boxes_Maintained { get; set; }

        [Display(Name = "18.59.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Rule_58_60_Is_First_Aid_Boxes_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Rule_58_60_Is_First_Aid_Boxes_Maintained_ViolationExist { get; set; }

        [Display(Name = "19.59.1.3 Sec_19_Rule_58_60_Is_First_Aid_Boxes_Maintained_Remarks")]
        [Required(ErrorMessage = "Sec_19_Rule_58_60_Is_First_Aid_Boxes_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Rule_58_60_Is_First_Aid_Boxes_Maintained_Remarks can have max 1000 chars..!")]
        public string Sec_19_Rule_58_60_Is_First_Aid_Boxes_Maintained_Remarks { get; set; }

        [Display(Name = "19.61.1.1 Section-19, Rule-61, 62 Are first aid boxes kept in charge of trained person who are readily available during working hours?")]
        [Required(ErrorMessage = "Sec_19_Rule_61_62_Is_First_Aid_Boxes_InCharged_TrainedPerson is required..!")]
        public GeneralOptionTypeEnum Sec_19_Rule_61_62_Is_First_Aid_Boxes_InCharged_TrainedPerson { get; set; }

        [Display(Name = "19.61.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_19_Rule_61_62_Is_First_Aid_Boxes_InCharged_TrainedPerson_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_19_Rule_61_62_Is_First_Aid_Boxes_InCharged_TrainedPerson_ViolationExist { get; set; }

        [Display(Name = "19.61.1.3 Sec_19_Rule_61_62_Is_First_Aid_Boxes_InCharged_TrainedPerson_Remarks")]
        [Required(ErrorMessage = "Sec_19_Rule_61_62_Is_First_Aid_Boxes_InCharged_TrainedPerson_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_19_Rule_61_62_Is_First_Aid_Boxes_InCharged_TrainedPerson_Remarks can have max 1000 chars..!")]
        public string Sec_19_Rule_61_62_Is_First_Aid_Boxes_InCharged_TrainedPerson_Remarks { get; set; }

        [Display(Name = "21.1.1 Section-21 and rules made there under Are wages being paid to workers as per provisions under this section and rules")]
        [Required(ErrorMessage = "Sec_21_Is_Wages_Paid is required..!")]
        public GeneralOptionTypeEnum Sec_21_Is_Wages_Paid { get; set; }

        [Display(Name = "21.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_21_Is_Wages_Paid_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_21_Is_Wages_Paid_ViolationExist { get; set; }

        [Display(Name = "21.1.3 Sec_21_Is_Wages_Paid_Remarks")]
        [Required(ErrorMessage = "Sec_21_Is_Wages_Paid_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_21_Is_Wages_Paid_Remarks can have max 1000 chars..!")]
        public string Sec_21_Is_Wages_Paid_Remarks { get; set; }

        [Display(Name = "29.74.1.1 Section-29, Rule-74 Is register of contractor in Form XII maintained by principal employer?")]
        [Required(ErrorMessage = "Sec_29_Rule_74_Is_Contractor_Registered_In_Form_XII_Maintained_By_PE is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_74_Is_Contractor_Registered_In_Form_XII_Maintained_By_PE { get; set; }

        [Display(Name = "29.74.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_74_Is_Contractor_Registered_In_Form_XII_Maintained_By_PE_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_74_Is_Contractor_Registered_In_Form_XII_Maintained_By_PE_ViolationExist { get; set; }

        [Display(Name = "29.74.1.3 Sec_29_Rule_74_Is_Contractor_Registered_In_Form_XII_Maintained_By_PE_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_74_Is_Contractor_Registered_In_Form_XII_Maintained_By_PE_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_74_Is_Contractor_Registered_In_Form_XII_Maintained_By_PE_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_74_Is_Contractor_Registered_In_Form_XII_Maintained_By_PE_Remarks { get; set; }

        [Display(Name = "29.75.1.1 Section-29, Rule-75 Are contractor(s) maintaining register of persons employed in Form XIII?")]
        [Required(ErrorMessage = "Sec_29_Rule_75_Is_Emp_Register_Maintained_By_Contractor_In_Form_XIII is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_75_Is_Emp_Register_Maintained_By_Contractor_In_Form_XIII { get; set; }

        [Display(Name = "29.75.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_75_Is_Emp_Register_Maintained_By_Contractor_In_Form_XIII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_75_Is_Emp_Register_Maintained_By_Contractor_In_Form_XIII_ViolationExist { get; set; }

        [Display(Name = "29.75.1.3 Sec_29_Rule_75_Is_Emp_Register_Maintained_By_Contractor_In_Form_XIII_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_75_Is_Emp_Register_Maintained_By_Contractor_In_Form_XIII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_75_Is_Emp_Register_Maintained_By_Contractor_In_Form_XIII_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_75_Is_Emp_Register_Maintained_By_Contractor_In_Form_XIII_Remarks { get; set; }

        [Display(Name = "29.76.1.1 Section-29, Rule-76 Are employment cards given to workers in Form-XIV by contractor(s)?")]
        [Required(ErrorMessage = "Sec_29_Rule_76_Is_Employee_Card_Given_In_Form_XIV is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_76_Is_Employee_Card_Given_In_Form_XIV { get; set; }

        [Display(Name = "29.76.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_76_Is_Employee_Card_Given_In_Form_XIV_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_76_Is_Employee_Card_Given_In_Form_XIV_ViolationExist { get; set; }

        [Display(Name = "29.76.1.3 Sec_29_Rule_76_Employee_Card_Given_In_Form_XIV_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_76_Employee_Card_Given_In_Form_XIV_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_76_Employee_Card_Given_In_Form_XIV_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_76_Is_Employee_Card_Given_In_Form_XIV_Remarks { get; set; }

        [Display(Name = "29.77.1.1 Section-29, Rule-77 Are service certificate in Form XV issued to workers on termination?")]
        [Required(ErrorMessage = "Sec_29_Rule_77_Is_Service_Certificate_Form_XV_Issued is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_77_Is_Service_Certificate_Form_XV_Issued { get; set; }

        [Display(Name = "29.76.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_77_Is_Service_Certificate_Form_XV_Issued_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_77_Is_Service_Certificate_Form_XV_Issued_ViolationExist { get; set; }

        [Display(Name = "29.77.1.3 Sec_29_Rule_77_Is_Service_Certificate_Form_XV_Issued_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_77_Is_Service_Certificate_Form_XV_Issued_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_77_Is_Service_Certificate_Form_XV_Issued_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_77_Is_Service_Certificate_Form_XV_Issued_Remarks { get; set; }

        [Display(Name = "29.78.1.1 Section-29, Rule-78 Is muster roll in Form XVI maintained?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Muster_Roll_Form_XVI_Maintained is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Muster_Roll_Form_XVI_Maintained { get; set; }

        [Display(Name = "29.78.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Muster_Roll_Form_XVI_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Muster_Roll_Form_XVI_Maintained_ViolationExist { get; set; }

        [Display(Name = "29.78.1.3 Sec_29_Rule_78_Is_Muster_Roll_Form_XVI_Maintained_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Muster_Roll_Form_XVI_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Muster_Roll_Form_XVI_Maintained_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Muster_Roll_Form_XVI_Maintained_Remarks { get; set; }

        [Display(Name = "29.78.2.1 Is register of wages in Form XVII maintained?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Register_Maintained_In_Form_XVII is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Wages_Register_Maintained_In_Form_XVII { get; set; }

        [Display(Name = "29.78.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Register_Maintained_In_Form_XVII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Wages_Register_Maintained_In_Form_XVII_ViolationExist { get; set; }

        [Display(Name = "29.78.2.3 Sec_29_Rule_78_Is_Wages_Register_Maintained_In_Form_XVII_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Register_Maintained_In_Form_XVII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Wages_Register_Maintained_In_Form_XVII_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Wages_Register_Maintained_In_Form_XVII_Remarks { get; set; }

        [Display(Name = "29.78.3.1 Is register of wages-cum-muster roll  in Form XVIII maintained?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII { get; set; }

        [Display(Name = "29.78.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_ViolationExist { get; set; }

        [Display(Name = "29.78.3.3 Sec_29_Rule_78_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_Remarks { get; set; }

        [Display(Name = "29.78.4.1 Is register of deduction in Form XX maintained?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Deduction_Register_Maintained_In_Form_XX is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Deduction_Register_Maintained_In_Form_XX { get; set; }

        [Display(Name = "29.78.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Deduction_Register_Maintained_In_Form_XX_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Deduction_Register_Maintained_In_Form_XX_ViolationExist { get; set; }

        [Display(Name = "29.78.4.3 Sec_29_Rule_78_Is_Deduction_Register_Maintained_In_Form_XX_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Deduction_Register_Maintained_In_Form_XX_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Deduction_Register_Maintained_In_Form_XX_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Deduction_Register_Maintained_In_Form_XX_Remarks { get; set; }

        [Display(Name = "29.78.5.1 Is register of fines in Form XXI maintained?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Fine_Register_Maintained_In_Form_XXI is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Fine_Register_Maintained_In_Form_XXI { get; set; }

        [Display(Name = "29.78.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Fine_Register_Maintained_In_Form_XXI_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Fine_Register_Maintained_In_Form_XXI_ViolationExist { get; set; }

        [Display(Name = "29.78.5.3 Sec_29_Rule_78_Is_Fine_Register_Maintained_In_Form_XXI_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Fine_Register_Maintained_In_Form_XXI_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Fine_Register_Maintained_In_Form_XXI_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Fine_Register_Maintained_In_Form_XXI_Remarks { get; set; }

        [Display(Name = "29.78.6.1 Is register of advances in Form XXII maintained?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Advances_Register_Maintained_In_Form_XXII is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Advances_Register_Maintained_In_Form_XXII { get; set; }

        [Display(Name = "29.78.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Advances_Register_Maintained_In_Form_XXII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Advances_Register_Maintained_In_Form_XXII_ViolationExist { get; set; }

        [Display(Name = "29.78.6.3 Sec_29_Rule_78_Is_Advances_Register_Maintained_In_Form_XXII_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Advances_Register_Maintained_In_Form_XXII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Advances_Register_Maintained_In_Form_XXII_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Advances_Register_Maintained_In_Form_XXII_Remarks { get; set; }

        [Display(Name = "29.78.7.1 Is register of overtime  in Form XXIII maintained?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Overtime_Register_Maintained_In_Form_XXIII is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Overtime_Register_Maintained_In_Form_XXIII { get; set; }

        [Display(Name = "29.78.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Overtime_Register_Maintained_In_Form_XXIII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Overtime_Register_Maintained_In_Form_XXIII_ViolationExist { get; set; }

        [Display(Name = "29.78.7.3 Sec_29_Rule_78_Is_Overtime_Register_Maintained_In_Form_XXIII_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Overtime_Register_Maintained_In_Form_XXIII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Overtime_Register_Maintained_In_Form_XXIII_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Overtime_Register_Maintained_In_Form_XXIII_Remarks { get; set; }

        [Display(Name = "29.78.8.1 Are wage slips issued to workers in Form XIX?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Slip_Issue_In_Form_XIX is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Wages_Slip_Issue_In_Form_XIX { get; set; }

        [Display(Name = "29.78.8.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Slip_Issue_In_Form_XIX_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Wages_Slip_Issue_In_Form_XIX_ViolationExist { get; set; }

        [Display(Name = "29.78.8.3 Sec_29_Rule_78_Is_Wages_Slip_Issue_In_Form_XIX_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Wages_Slip_Issue_In_Form_XIX_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Wages_Slip_Issue_In_Form_XIX_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Wages_Slip_Issue_In_Form_XIX_Remarks { get; set; }

        [Display(Name = "29.78.9.1 Are signature or thump impression of workers are taken in Register of Wages or Muster Roll_cum_Register of Wages, as the cas emay be, mentioned registered and are authenticated upon by employer/authorized representative")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Biometric_Of_Workers_Taken is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Biometric_Of_Workers_Taken { get; set; }

        [Display(Name = "29.78.9.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Biometric_Of_Workers_Taken_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Biometric_Of_Workers_Taken_ViolationExist { get; set; }

        [Display(Name = "29.78.9.3 Sec_29_Rule_78_Is_Biometric_Of_Workers_Taken_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Biometric_Of_Workers_Taken_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Biometric_Of_Workers_Taken_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Biometric_Of_Workers_Taken_Remarks { get; set; }

        [Display(Name = "29.78.10.1 Are provision of Rule 78(1)(d) applicable?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Provision_Of_Rule_78_1_d_Applicable is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Provision_Of_Rule_78_1_d_Applicable { get; set; }

        [Display(Name = "29.78.10.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Provision_Of_Rule_78_1_d_Applicable_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Provision_Of_Rule_78_1_d_Applicable_ViolationExist { get; set; }

        [Display(Name = "29.78.10.3 Sec_29_Rule_78_Is_Provision_Of_Rule_78_1_d_Applicable_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Provision_Of_Rule_78_1_d_Applicable_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Provision_Of_Rule_78_1_d_Applicable_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Provision_Of_Rule_78_1_d_Applicable_Remarks { get; set; }

        [Display(Name = "29.78.11.1 Are provision of Rule 78(2 ) applicable?")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Provision_Of_Rule_78_2_d_Applicable is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_78_Is_Provision_Of_Rule_78_2_d_Applicable { get; set; }

        [Display(Name = "29.78.11.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Provision_Of_Rule_78_2_d_Applicable_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_78_Is_Provision_Of_Rule_78_2_d_Applicable_ViolationExist { get; set; }

        [Display(Name = "29.78.11.3 Sec_29_Rule_78_Is_Provision_Of_Rule_78_2_d_Applicable_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_78_Is_Provision_Of_Rule_78_2_d_Applicable_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_78_Is_Provision_Of_Rule_78_2_d_Applicable_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_78_Is_Provision_Of_Rule_78_2_d_Applicable_Remarks { get; set; }

        [Display(Name = "29.79.1.1 Sec_29_Rule_79_Is_Notices_Prescribed_In_Rule_79_Displayed")]
        [Required(ErrorMessage = "Sec_29_Rule_79_Is_Notices_Prescribed_In_Rule_79_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_79_Is_Notices_Prescribed_In_Rule_79_Displayed { get; set; }

        [Display(Name = "29.79.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_79_Is_Notices_Prescribed_In_Rule_79_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_79_Is_Notices_Prescribed_In_Rule_79_Displayed_ViolationExist { get; set; }

        [Display(Name = "29.79.1.3 Section-29, Rule-79 Are notices, prescribed in rule 79, displayed?")]
        [Required(ErrorMessage = "sec_29_Rule_79_Notices_Prescribed_In_Rule_79_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_79_Is_Notices_Prescribed_In_Rule_79_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_79_Is_Notices_Prescribed_In_Rule_79_Displayed_Remarks { get; set; }

        [Display(Name = "29.80(3).1.1 Section-29, Rule-80(3) Are registered preserved for three years after date of  last entry made in therein?")]
        [Required(ErrorMessage = "Sec_29_Rule_80_3_IsRegistered_Preserved_For_Three_Years is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_80_3_IsRegistered_Preserved_For_Three_Years { get; set; }

        [Display(Name = "29.80(3).1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_80_3_IsRegistered_Preserved_For_Three_Years_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_80_3_IsRegistered_Preserved_For_Three_Years_ViolationExist { get; set; }

        [Display(Name = "29.80(3).1.3 Sec_29_Rule_80_3_IsRegistered_Preserved_For_Three_Years_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_80_3_IsRegistered_Preserved_For_Three_Years_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_80_3_IsRegistered_Preserved_For_Three_Years_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_80_3_IsRegistered_Preserved_For_Three_Years_Remarks { get; set; }

        [Display(Name = "29.80(4).1.1 Section-29, Rule-80(4) Are registered produced at time of inspection?")]
        [Required(ErrorMessage = "Sec_29_Rule_80_4_IsRegistered_Produced_While_Inspection is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_80_4_IsRegistered_Produced_While_Inspection { get; set; }

        [Display(Name = "29.80(4).1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_80_4_IsRegistered_Produced_While_Inspection_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_80_4_IsRegistered_Produced_While_Inspection_ViolationExist { get; set; }

        [Display(Name = "29.80(4).1.3 Sec_29_Rule_80_4_IsRegistered_Produced_While_Inspection_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_80_4_IsRegistered_Produced_While_Inspection_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_80_4_IsRegistered_Produced_While_Inspection_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_80_4_IsRegistered_Produced_While_Inspection_Remarks { get; set; }

        [Display(Name = "29.81.1.1 Section-29, Rule-81 Are notices, prescribed in rule 81, displayed?")]
        [Required(ErrorMessage = "Sec_29_Rule_81_Is_Notices_Prescribed_In_Rule_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_81_Is_Notices_Prescribed_In_Rule_Displayed { get; set; }

        [Display(Name = "29.81.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_81_Is_Notices_Prescribed_In_Rule_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_81_Is_Notices_Prescribed_In_Rule_Displayed_ViolationExist { get; set; }

        [Display(Name = "29.81.1.3 Sec_29_Rule_81_Is_Notices_Prescribed_In_Rule_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_81_Is_Notices_Prescribed_In_Rule_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_81_Is_Notices_Prescribed_In_Rule_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_81_Is_Notices_Prescribed_In_Rule_Displayed_Remarks { get; set; }

        [Display(Name = "29.82.1.1 Section-29, Rule-82 Is half yearly return in Form XXIV submitted by each contractor?")]
        [Required(ErrorMessage = "Sec_29_Rule_82_Is_HalfYearlyReturn_In_Form_XXIII_Submitted is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_82_Is_HalfYearlyReturn_In_Form_XXIII_Submitted { get; set; }

        [Display(Name = "29.82.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_82_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_82_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_ViolationExist { get; set; }

        [Display(Name = "29.82.1.3 Sec_29_Rule_82_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_82_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_82_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_82_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_Remarks { get; set; }

        [Display(Name = "29.82.2.1 Is annual return in Form XXV is submitted by principal employer?")]
        [Required(ErrorMessage = "Sec_29_Rule_82_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE is required..!")]
        public GeneralOptionTypeEnum Sec_29_Rule_82_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE { get; set; }

        [Display(Name = "29.82.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_29_Rule_82_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_29_Rule_82_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_ViolationExist { get; set; }

        [Display(Name = "29.82.2.3 Sec_29_Rule_82_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_Remarks")]
        [Required(ErrorMessage = "Sec_29_Rule_82_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_29_Rule_82_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_Remarks can have max 1000 chars..!")]
        public string Sec_29_Rule_82_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_InterStateMigrantWorkmenAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "16.41.1.1 Section-16, Rule-41 Is canteen provided and maintained in efficient manner?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Canteen_Provided { get; set; }

        [Display(Name = "16.41.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Canteen_Provided_ViolationExist { get; set; }

        [Display(Name = "16.41.1.3 Sec_16_Rule_41_Is_Canteen_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Canteen_Provided_Remarks { get; set; }

        [Display(Name = "16.41.2.1 Is canteen consist of dinning hall, kitchen, storeroom, pantry and washing place for workers and utensils?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace { get; set; }

        [Display(Name = "16.41.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_ViolationExist { get; set; }

        [Display(Name = "16.41.2.3 Sec_16_Rule_41_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Canteen_Consist_DinningHall_Kitchen_StoreRoom_Pantry_WashingPlace_Remarks { get; set; }

        [Display(Name = "16.41.3.1 Is canteen sufficiently lighted?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted { get; set; }

        [Display(Name = "16.41.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted_ViolationExist { get; set; }

        [Display(Name = "16.41.3.3 Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted_Remarks { get; set; }

        [Display(Name = "16.41.4.1 Is floor of canteen is of impervious material?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Sufficiently_Lighted is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Canteen_Floor_Impervious_Material { get; set; }

        [Display(Name = "16.41.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Floor_Impervious_Material_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Canteen_Floor_Impervious_Material_ViolationExist { get; set; }

        [Display(Name = "16.41.4.3 Sec_16_Rule_41_Is_Canteen_Floor_Impervious_Material_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Floor_Impervious_Material_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Floor_Impervious_Material_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Canteen_Floor_Impervious_Material_Remarks { get; set; }

        [Display(Name = "16.41.5.1 Are inside walls of canteen are lime washed once in each year?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Wall_Lime_Washed_Once_InYear is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Canteen_Wall_Lime_Washed_Once_InYear { get; set; }

        [Display(Name = "16.41.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Wall_Lime_Washed_Once_InYear_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Canteen_Wall_Lime_Washed_Once_InYear_ViolationExist { get; set; }

        [Display(Name = "16.41.5.3 Sec_16_Rule_41_Is_Canteen_Wall_Lime_Washed_Once_InYear_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Wall_Lime_Washed_Once_InYear_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Wall_Lime_Washed_Once_InYear_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Canteen_Wall_Lime_Washed_Once_InYear_Remarks { get; set; }

        [Display(Name = "16.41.6.1 Are inside walls of kitchen lime washed every four months?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month { get; set; }

        [Display(Name = "16.41.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_ViolationExist { get; set; }

        [Display(Name = "16.41.6.2 Sec_16_Rule_41_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Canteen_InSide_Wall_Lime_Washed_Every_Four_Month_Remarks { get; set; }

        [Display(Name = "16.41.7.1 Is precincts of canteen in clean and sanitary conditions?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Precincts_Of_Canteen_In_Clean is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Precincts_Of_Canteen_In_Clean { get; set; }

        [Display(Name = "16.41.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Precincts_Of_Canteen_In_Clean_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Precincts_Of_Canteen_In_Clean_ViolationExist { get; set; }

        [Display(Name = "16.41.7.3 Sec_16_Rule_41_Is_Precincts_Of_Canteen_In_Clean_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Precincts_Of_Canteen_In_Clean_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Precincts_Of_Canteen_In_Clean_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Precincts_Of_Canteen_In_Clean_Remarks { get; set; }

        [Display(Name = "16.41.8.1 Is wastewater and garbage is disposed properly?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Wastewater_And_Garbage_Disposed is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Wastewater_And_Garbage_Disposed { get; set; }

        [Display(Name = "16.41.8.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Wastewater_And_Garbage_Disposed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Wastewater_And_Garbage_Disposed_ViolationExist { get; set; }

        [Display(Name = "16.41.8.3 Sec_16_Rule_41_Is_Wastewater_And_Garbage_Disposed_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Wastewater_And_Garbage_Disposed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Wastewater_And_Garbage_Disposed_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Wastewater_And_Garbage_Disposed_Remarks { get; set; }

        [Display(Name = "16.41.9.1 Is dining hall accommodating 30%  of contract labour working at a time?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_DiningHall_Accommodates_30Percent_ContractLabour is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_DiningHall_Accommodates_30Percent_ContractLabour { get; set; }

        [Display(Name = "16.41.9.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_DiningHall_Accommodates_30Percent_ContractLabour_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_DiningHall_Accommodates_30Percent_ContractLabour_ViolationExist { get; set; }

        [Display(Name = "16.41.9.3 Sec_16_Rule_41_Is_DiningHall_Accommodates_30Percent_ContractLabour_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_DiningHall_Accommodates_30Percent_ContractLabour_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_DiningHall_Accommodates_30Percent_ContractLabour_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_DiningHall_Accommodates_30Percent_ContractLabour_Remarks { get; set; }

        [Display(Name = "16.41.10.1 Is area of 1 m2 is available every diner?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Area_Of_1_Mtr_Sqr_Available is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Area_Of_1_Mtr_Sqr_Available { get; set; }

        [Display(Name = "16.41.10.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Area_Of_1_Mtr_Sqr_Available_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Area_Of_1_Mtr_Sqr_Available_ViolationExist { get; set; }

        [Display(Name = "16.41.10.3 Sec_16_Rule_41_Is_Area_Of_1_Mtr_Sqr_Available_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Area_Of_1_Mtr_Sqr_Available_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Area_Of_1_Mtr_Sqr_Available_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Area_Of_1_Mtr_Sqr_Available_Remarks { get; set; }

        [Display(Name = "16.41.11.1 Is portion of dinning hall and service counter is reserved for women workers?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Portion_Of_Dinning_Reserved_For_Women is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Portion_Of_Dinning_Reserved_For_Women { get; set; }

        [Display(Name = "16.41.11.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Portion_Of_Dinning_Reserved_For_Women_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Portion_Of_Dinning_Reserved_For_Women_ViolationExist { get; set; }

        [Display(Name = "16.41.11.3 Sec_16_Rule_41_Is_Portion_Of_Dinning_Reserved_For_Women_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Portion_Of_Dinning_Reserved_For_Women_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Portion_Of_Dinning_Reserved_For_Women_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Portion_Of_Dinning_Reserved_For_Women_Remarks { get; set; }

        [Display(Name = "16.41.12.1 Is separate washing place with  privacy is provided?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Separate_Washing_Place_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Separate_Washing_Place_Provided { get; set; }

        [Display(Name = "16.41.12.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Separate_Washing_Place_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Separate_Washing_Place_Provided_ViolationExist { get; set; }

        [Display(Name = "16.41.12.3 Sec_16_Rule_41_Is_Separate_Washing_Place_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Separate_Washing_Place_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Separate_Washing_Place_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Separate_Washing_Place_Provided_Remarks { get; set; }

        [Display(Name = "16.41.13.1 Are provisions of this rule being complied?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Provisions_Of_Rule_Complied is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Provisions_Of_Rule_Complied { get; set; }

        [Display(Name = "16.41.13.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Provisions_Of_Rule_Complied_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Provisions_Of_Rule_Complied_ViolationExist { get; set; }

        [Display(Name = "16.41.13.3 Sec_16_Rule_41_Is_Provisions_Of_Rule_Complied_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Provisions_Of_Rule_Complied_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Provisions_Of_Rule_Complied_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Provisions_Of_Rule_Complied_Remarks { get; set; }

        [Display(Name = "16.41.14.1 Are foodstuff meeting the requirements of natural habits of contract labour?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_FoodStuffs_Meets_habits_Of_ContractLabour is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_FoodStuffs_Meets_habits_Of_ContractLabour { get; set; }

        [Display(Name = "16.41.14.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_FoodStuffs_Meets_habits_Of_ContractLabour_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_FoodStuffs_Meets_habits_Of_ContractLabour_ViolationExist { get; set; }

        [Display(Name = "16.41.14.3 Sec_16_Rule_41_Is_FoodStuffs_Meets_habits_Of_ContractLabour_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_FoodStuffs_Meets_habits_Of_ContractLabour_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_FoodStuffs_Meets_habits_Of_ContractLabour_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_FoodStuffs_Meets_habits_Of_ContractLabour_Remarks { get; set; }

        [Display(Name = "16.41.15.1 Is canteen running at “NO PROFIT NO LOSS”?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Running_No_Profit_No_Loss is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Canteen_Running_No_Profit_No_Loss { get; set; }

        [Display(Name = "16.41.15.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Running_No_Profit_No_Loss_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Canteen_Running_No_Profit_No_Loss_ViolationExist { get; set; }

        [Display(Name = "16.41.15.3 Sec_16_Rule_41_Is_Canteen_Running_No_Profit_No_Loss_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Running_No_Profit_No_Loss_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Running_No_Profit_No_Loss_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Canteen_Running_No_Profit_No_Loss_Remarks { get; set; }

        [Display(Name = "16.41.16.1 Are account books and other records produced during inspection?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_AccountBook_Produced_While_Inspection is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_AccountBook_Produced_While_Inspection { get; set; }

        [Display(Name = "16.41.16.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_AccountBook_Produced_While_Inspection_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_AccountBook_Produced_While_Inspection_ViolationExist { get; set; }

        [Display(Name = "16.41.16.3 Sec_16_Rule_41_Is_AccountBook_Produced_While_Inspection_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_AccountBook_Produced_While_Inspection_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_AccountBook_Produced_While_Inspection_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_AccountBook_Produced_While_Inspection_Remarks { get; set; }

        [Display(Name = "16.41.17.1 Are accounts of canteen audit every 12 months by registered accountant and auditors?")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Audit_Performed is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_41_Is_Canteen_Audit_Performed { get; set; }

        [Display(Name = "16.41.17.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Audit_Performed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_41_Is_Canteen_Audit_Performed_ViolationExist { get; set; }

        [Display(Name = "16.41.17.3 Sec_16_Rule_41_Is_Canteen_Audit_Performed_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_41_Canteen_Audit_Performed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_41_Is_Canteen_Audit_Performed_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_41_Is_Canteen_Audit_Performed_Remarks { get; set; }

        [Display(Name = "16.40.1.1 Section-16, Rule-40 Are restrooms provide as per provisions under this section and rule?")]
        [Required(ErrorMessage = "Sec_16_Rule_40_Is_Restroom_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_40_Is_Restroom_Provided { get; set; }

        [Display(Name = "16.40.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_40_Is_Restroom_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_40_Is_Restroom_Provided_ViolationExist { get; set; }

        [Display(Name = "16.40.1.3 Sec_16_Rule_40_Is_Restroom_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_40_Is_Restroom_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_40_Is_Restroom_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_40_Is_Restroom_Provided_Remarks { get; set; }

        [Display(Name = "16.39.1.1 Section-16, Rule-39 Is sufficient supply of wholesome drinking water is available to all workers?")]
        [Required(ErrorMessage = "Sec_16_Rule_39_Is_Sufficient_Drinking_Water_Available is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_39_Is_Sufficient_Drinking_Water_Available { get; set; }

        [Display(Name = "16.39.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_39_Is_Sufficient_Drinking_Water_Available_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_39_Is_Sufficient_Drinking_Water_Available_ViolationExist { get; set; }

        [Display(Name = "16.39.1.3 Sec_16_Rule_39_Is_Sufficient_Drinking_Water_Available_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_39_Is_Sufficient_Drinking_Water_Available_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_39_Is_Sufficient_Drinking_Water_Available_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_39_Is_Sufficient_Drinking_Water_Available_Remarks { get; set; }

        [Display(Name = "16.42.1.1 Are latrines provided as per provisions of this rule?")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrines_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_42_Is_Latrines_Provided { get; set; }

        [Display(Name = "16.42.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrines_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_42_Is_Latrines_Provided_ViolationExist { get; set; }

        [Display(Name = "16.42.1.3 Sec_16_Rule_42_Is_Latrines_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrines_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_42_Is_Latrines_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_42_Is_Latrines_Provided_Remarks { get; set; }

        [Display(Name = "16.42.2.1 Are signboards with figures on latrines in language understood by workers displayed?")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Signboard_In_Latrines_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_42_Is_Signboard_In_Latrines_Displayed { get; set; }

        [Display(Name = "16.42.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Signboard_In_Latrines_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_42_Is_Signboard_In_Latrines_Displayed_ViolationExist { get; set; }

        [Display(Name = "16.42.2.3 Sec_16_Rule_42_Is_Signboard_In_Latrines_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Signboard_In_Latrines_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_42_Is_Signboard_In_Latrines_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_42_Is_Signboard_In_Latrines_Displayed_Remarks { get; set; }

        [Display(Name = "16.42.3.1 Are Urinals provided as per provisions of this rule?")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Urinals_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_42_Is_Urinals_Provided { get; set; }

        [Display(Name = "16.42.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Urinals_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_42_Is_Urinals_Provided_ViolationExist { get; set; }

        [Display(Name = "16.42.3.3 Sec_16_Rule_42_Is_Urinals_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Urinals_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_42_Is_Urinals_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_42_Is_Urinals_Provided_Remarks { get; set; }

        [Display(Name = "16.42.4.1 Are latrine/Urinals adequately lighted?")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Adequately_Lighted is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_42_Is_Latrine_Urinals_Adequately_Lighted { get; set; }

        [Display(Name = "16.42.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Adequately_Lighted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_42_Is_Latrine_Urinals_Adequately_Lighted_ViolationExist { get; set; }

        [Display(Name = "16.42.4.3 Sec_16_Rule_42_Is_Latrine_Urinals_Adequately_Lighted_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Adequately_Lighted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Adequately_Lighted_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_42_Is_Latrine_Urinals_Adequately_Lighted_Remarks { get; set; }

        [Display(Name = "16.42.5.1 Are latrines/Urinals in clean and sanitary conditions?")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Clean is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_42_Is_Latrine_Urinals_Clean { get; set; }

        [Display(Name = "16.42.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Clean_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_42_Is_Latrine_Urinals_Clean_ViolationExist { get; set; }

        [Display(Name = "16.42.5.3 Sec_16_Rule_42_Is_Latrine_Urinals_Clean_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Clean_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Clean_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_42_Is_Latrine_Urinals_Clean_Remarks { get; set; }

        [Display(Name = "16.42.6.1 Are latrines/Urinals confirming to public health requirements?")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Public_Health_Requirements is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_42_Is_Latrine_Urinals_Public_Health_Requirements { get; set; }

        [Display(Name = "16.42.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Public_Health_Requirements_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_42_Is_Latrine_Urinals_Public_Health_Requirements_ViolationExist { get; set; }

        [Display(Name = "16.42.6.3 Sec_16_Rule_42_Is_Latrine_Urinals_Public_Health_Requirements_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Public_Health_Requirements_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_42_Is_Latrine_Urinals_Public_Health_Requirements_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_42_Is_Latrine_Urinals_Public_Health_Requirements_Remarks { get; set; }

        [Display(Name = "16.42.7.1 Is water supply in latrine/Urinal is as per this rule?")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Water_Supply_In_Latrine_Urinals is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_42_Is_Water_Supply_In_Latrine_Urinals { get; set; }

        [Display(Name = "16.42.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Water_Supply_In_Latrine_Urinals_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_42_Is_Water_Supply_In_Latrine_Urinals_ViolationExist { get; set; }

        [Display(Name = "16.42.7.3 Sec_16_Rule_42_Is_Water_Supply_In_Latrine_Urinals_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_42_Is_Water_Supply_In_Latrine_Urinals_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_42_Is_Water_Supply_In_Latrine_Urinals_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_42_Is_Water_Supply_In_Latrine_Urinals_Remarks { get; set; }

        [Display(Name = "16.43.1.1 Section-16, Rule-43 Are washing facilities are provided as per this rule?")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Washing_Facilities_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_43_Is_Washing_Facilities_Provided { get; set; }

        [Display(Name = "16.43.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Washing_Facilities_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_43_Is_Washing_Facilities_Provided_ViolationExist { get; set; }

        [Display(Name = "16.43.1.3 Sec_16_Rule_43_Is_Washing_Facilities_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_43_Is_Washing_Facilities_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_43_Is_Washing_Facilities_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_43_Is_Washing_Facilities_Provided_Remarks { get; set; }

        [Display(Name = "16.37.1.1 Section-16, Rule-37 Is medical facility for outdoor treatment provided to migrant workmen by the contractor?")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Medical_Facility_Provided_By_Contractor is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_37_Is_Medical_Facility_Provided_By_Contractor { get; set; }

        [Display(Name = "16.37.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Medical_Facility_Provided_By_Contractor_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_37_Is_Medical_Facility_Provided_By_Contractor_ViolationExist { get; set; }

        [Display(Name = "16.37.1.3 Sec_16_Rule_37_Is_Medical_Facility_Provided_By_Contractor_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Medical_Facility_Provided_By_Contractor_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_37_Is_Medical_Facility_Provided_By_Contractor_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_37_Is_Medical_Facility_Provided_By_Contractor_Remarks { get; set; }

        [Display(Name = "16.37.2.1 Is contractor reimbursing the cost incurred by migrant workman on purchase of medicines?")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Medicines_Cost_Reimburse_By_Contractor is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_37_Is_Medicines_Cost_Reimburse_By_Contractor { get; set; }

        [Display(Name = "16.37.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Medicines_Cost_Reimburse_By_Contractor_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_37_Is_Medicines_Cost_Reimburse_By_Contractor_ViolationExist { get; set; }

        [Display(Name = "16.37.2.3 Sec_16_Rule_37_Is_Medicines_Cost_Reimburse_By_Contractor_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Medicines_Cost_Reimburse_By_Contractor_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_37_Is_Medicines_Cost_Reimburse_By_Contractor_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_37_Is_Medicines_Cost_Reimburse_By_Contractor_Remarks { get; set; }

        [Display(Name = "16.37.3.1 Is contractor bearing the expenses of hospitalization of migrant workmen or his family")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Family_Hospital_Expenses_Bearing_By_Contractor is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_37_Is_Family_Hospital_Expenses_Bearing_By_Contractor { get; set; }

        [Display(Name = "16.37.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Family_Hospital_Expenses_Bearing_By_Contractor_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_37_Is_Family_Hospital_Expenses_Bearing_By_Contractor_ViolationExist { get; set; }

        [Display(Name = "16.37.3.3 Sec_16_Rule_37_Is_Family_Hospital_Expenses_Bearing_By_Contractor_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_Family_Hospital_Expenses_Bearing_By_Contractor_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_37_Is_Family_Hospital_Expenses_Bearing_By_Contractor_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_37_Is_Family_Hospital_Expenses_Bearing_By_Contractor_Remarks { get; set; }

        [Display(Name = "16.37.4.1 Are sufficient number of first aid boxes are proved and maintained?")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_37_Is_First_Aid_Boxes_Provided { get; set; }

        [Display(Name = "16.37.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_37_Is_First_Aid_Boxes_Provided_ViolationExist { get; set; }

        [Display(Name = "16.37.4.3 Sec_16_Rule_37_Is_First_Aid_Boxes_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_37_Is_First_Aid_Boxes_Provided_Remarks { get; set; }

        [Display(Name = "16.37.5.1 Are first aid boxes being maintained as per this rule ?")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_Maintained is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_37_Is_First_Aid_Boxes_Maintained { get; set; }

        [Display(Name = "16.37.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_37_Is_First_Aid_Boxes_Maintained_ViolationExist { get; set; }

        [Display(Name = "16.37.5.3 Sec_16_Rule_37_Is_First_Aid_Boxes_Maintained_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_Maintained_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_37_Is_First_Aid_Boxes_Maintained_Remarks { get; set; }

        [Display(Name = "16.37.6.1 Are first aid boxes kept in charge of trained person who are readily available during working hours?")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_InCharged_TrainedPerson is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_37_Is_First_Aid_Boxes_InCharged_TrainedPerson { get; set; }

        [Display(Name = "16.37.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_InCharged_TrainedPerson_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_37_Is_First_Aid_Boxes_InCharged_TrainedPerson_ViolationExist { get; set; }

        [Display(Name = "16.37.6.3 Sec_16_Rule_37_Is_First_Aid_Boxes_InCharged_TrainedPerson_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_InCharged_TrainedPerson_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_37_Is_First_Aid_Boxes_InCharged_TrainedPerson_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_37_Is_First_Aid_Boxes_InCharged_TrainedPerson_Remarks { get; set; }

        [Display(Name = "16.38.1.1 Are protective clothes provided to  migrant workmen as per this rule?")]
        [Required(ErrorMessage = "Sec_16_Rule_38_Is_Protective_Clothes_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_38_Is_Protective_Clothes_Provided { get; set; }

        [Display(Name = "16.38.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_38_Is_Protective_Clothes_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_38_Is_Protective_Clothes_Provided_ViolationExist { get; set; }

        [Display(Name = "16.38.1.3 Sec_16_Rule_38_Is_Protective_Clothes_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_38_Is_Protective_Clothes_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_38_Is_Protective_Clothes_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_38_Is_Protective_Clothes_Provided_Remarks { get; set; }

        [Display(Name = "16.44.1.1 Section-16, Rule 44 Is creche provided as this rule?")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Creche_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_44_Is_Creche_Provided { get; set; }

        [Display(Name = "16.44.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Creche_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_44_Is_Creche_Provided_ViolationExist { get; set; }

        [Display(Name = "16.44.1.3 Sec_16_Rule_44_Is_Creche_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_44_Is_Creche_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_44_Is_Creche_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_44_Is_Creche_Provided_Remarks { get; set; }

        [Display(Name = "16.45.1.1 Section-16, Rule 45 Is residential accommodation  provided as this rule?")]
        [Required(ErrorMessage = "Sec_16_Rule_45_Is_Residential_Accommodation_Provided is required..!")]
        public GeneralOptionTypeEnum Sec_16_Rule_45_Is_Residential_Accommodation_Provided { get; set; }

        [Display(Name = "16.15.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_16_Rule_45_Is_Residential_Accommodation_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_16_Rule_45_Is_Residential_Accommodation_Provided_ViolationExist { get; set; }

        [Display(Name = "16.45.1.3 Sec_16_Rule_45_Is_Residential_Accommodation_Provided_Remarks")]
        [Required(ErrorMessage = "Sec_16_Rule_45_Is_Residential_Accommodation_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_16_Rule_45_Is_Residential_Accommodation_Provided_Remarks can have max 1000 chars..!")]
        public string Sec_16_Rule_45_Is_Residential_Accommodation_Provided_Remarks { get; set; }

        [Display(Name = "12.21.1.1< Section-12, Rule-21 Has contractor furnished particulars of migrant workmen in Form-X")]
        [Required(ErrorMessage = "Sec_12_Rule_21_Is_Contractor_Furnished_Particulars_In_Form_X is required..!")]
        public GeneralOptionTypeEnum Sec_12_Rule_21_Is_Contractor_Furnished_Particulars_In_Form_X { get; set; }

        [Display(Name = "12.21.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_12_Rule_21_Is_Contractor_Furnished_Particulars_In_Form_X_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_12_Rule_21_Is_Contractor_Furnished_Particulars_In_Form_X_ViolationExist { get; set; }

        [Display(Name = "12.21.1.3 Sec_12_Rule_21_Is_Contractor_Furnished_Particulars_In_Form_X_Remarks")]
        [Required(ErrorMessage = "Sec_12_Rule_21_Is_Contractor_Furnished_Particulars_In_Form_X_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_12_Rule_21_Is_Contractor_Furnished_Particulars_In_Form_X_Remarks can have max 1000 chars..!")]
        public string Sec_12_Rule_21_Is_Contractor_Furnished_Particulars_In_Form_X_Remarks { get; set; }

        [Display(Name = "12.22.1.1 Has contractor paid return fair to migrant workmen?")]
        [Required(ErrorMessage = "Sec_12_Rule_22_Is_Contractor_Paid_Return_Fair_To_Migrant is required..!")]
        public GeneralOptionTypeEnum Sec_12_Rule_22_Is_Contractor_Paid_Return_Fair_To_Migrant { get; set; }

        [Display(Name = "12.21.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_12_Rule_22_Is_Contractor_Paid_Return_Fair_To_Migrant_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_12_Rule_22_Is_Contractor_Paid_Return_Fair_To_Migrant_ViolationExist { get; set; }

        [Display(Name = "12.21.1.3 Sec_12_Rule_22_Is_Contractor_Paid_Return_Fair_To_Migrant_Remarks")]
        [Required(ErrorMessage = "Sec_12_Rule_22_Is_Contractor_Paid_Return_Fair_To_Migrant_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_12_Rule_22_Is_Contractor_Paid_Return_Fair_To_Migrant_Remarks can have max 1000 chars..!")]
        public string Sec_12_Rule_22_Is_Contractor_Paid_Return_Fair_To_Migrant_Remarks { get; set; }


        [Display(Name = "12.23.1.1 Is pass-book issued to every migrant workman?")]
        [Required(ErrorMessage = "Sec_12_Rule_23_Is_Passbook_Issued_To_Every_Migrant is required..!")]
        public GeneralOptionTypeEnum Sec_12_Rule_23_Is_Passbook_Issued_To_Every_Migrant { get; set; }

        [Display(Name = "12.21.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_12_Rule_23_Is_Passbook_Issued_To_Every_Migrant_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_12_Rule_23_Is_Passbook_Issued_To_Every_Migrant_ViolationExist { get; set; }

        [Display(Name = "12.21.1.3 Sec_12_Rule_23_Is_Passbook_Issued_To_Every_Migrant_Remarks")]
        [Required(ErrorMessage = "Sec_12_Rule_23_Is_Passbook_Issued_To_Every_Migrant_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_12_Rule_23_Is_Passbook_Issued_To_Every_Migrant_Remarks can have max 1000 chars..!")]
        public string Sec_12_Rule_23_Is_Passbook_Issued_To_Every_Migrant_Remarks { get; set; }


        [Display(Name = "12.24.1.1 Section-12, Rule-24 Is return in Form XI submitted by contractor?")]
        [Required(ErrorMessage = "Sec_12_Rule_24_Is_Return_In_Form_XI_Submitted_By_Contractor is required..!")]
        public GeneralOptionTypeEnum Sec_12_Rule_24_Is_Return_In_Form_XI_Submitted_By_Contractor { get; set; }

        [Display(Name = "12.21.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_12_Rule_24_Is_Return_In_Form_XI_Submitted_By_Contractor_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_12_Rule_24_Is_Return_In_Form_XI_Submitted_By_Contractor_ViolationExist { get; set; }

        [Display(Name = "12.21.1.3 Sec_12_Rule_24_Is_Return_In_Form_XI_Submitted_By_Contractor_Remarks")]
        [Required(ErrorMessage = "Sec_12_Rule_24_Is_Return_In_Form_XI_Submitted_By_Contractor_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_12_Rule_24_Is_Return_In_Form_XI_Submitted_By_Contractor_Remarks can have max 1000 chars..!")]
        public string Sec_12_Rule_24_Is_Return_In_Form_XI_Submitted_By_Contractor_Remarks { get; set; }

        [Display(Name = "13.25.1.1 Section-13 Rule 25 Are wages being paid to workers as per provisions under this section and rule")]
        [Required(ErrorMessage = "Sec_13_Rule_25_Is_Wages_Paid_To_Workers is required..!")]
        public GeneralOptionTypeEnum Sec_13_Rule_25_Is_Wages_Paid_To_Workers { get; set; }

        [Display(Name = "13.25.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_13_Rule_25_Is_Wages_Paid_To_Workers_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_13_Rule_25_Is_Wages_Paid_To_Workers_ViolationExist { get; set; }

        [Display(Name = "13.25.1.3 Sec_13_Rule_25_Is_Wages_Paid_To_Workers_Remarks")]
        [Required(ErrorMessage = "Sec_13_Rule_25_Is_Wages_Paid_To_Workers_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_13_Rule_25_Is_Wages_Paid_To_Workers_Remarks can have max 1000 chars..!")]
        public string Sec_13_Rule_25_Is_Wages_Paid_To_Workers_Remarks { get; set; }

        [Display(Name = "17.28.1.1 Section-17, Rule-28 Are payments of wages made on seventh or tenth of every month, as the case may?")]
        [Required(ErrorMessage = "Sec_17_Rule_28_Is_Wages_Payment_Made_On_7_or_10_Every_Month is required..!")]
        public GeneralOptionTypeEnum Sec_17_Rule_28_Is_Wages_Payment_Made_On_7_or_10_Every_Month { get; set; }

        [Display(Name = "17.28.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_17_Rule_28_Is_Wages_Payment_Made_On_7_or_10_Every_Month_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_17_Rule_28_Is_Wages_Payment_Made_On_7_or_10_Every_Month_ViolationExist { get; set; }

        [Display(Name = "17.28.1.3 Sec_17_Rule_28_Is_Wages_Payment_Made_On_7_or_10_Every_Month_Remarks")]
        [Required(ErrorMessage = "Sec_17_Rule_28_Is_Wages_Payment_Made_On_7_or_10_Every_Month_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_17_Rule_28_Is_Wages_Payment_Made_On_7_or_10_Every_Month_Remarks can have max 1000 chars..!")]
        public string Sec_17_Rule_28_Is_Wages_Payment_Made_On_7_or_10_Every_Month_Remarks { get; set; }

        [Display(Name = "33.1.1 Rule-33 Is notice as per this rule displayed?")]
        [Required(ErrorMessage = "Rule_33_Is_Notice_Displayed is required..!")]
        public GeneralOptionTypeEnum Rule_33_Is_Notice_Displayed { get; set; }

        [Display(Name = "33.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_33_Is_Notice_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_33_Is_Notice_Displayed_ViolationExist { get; set; }

        [Display(Name = "33.1.3 Rule_33_Is_Notice_Displayed_Remarks")]
        [Required(ErrorMessage = "Rule_33_Is_Notice_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_33_Is_Notice_Displayed_Remarks can have max 1000 chars..!")]
        public string Rule_33_Is_Notice_Displayed_Remarks { get; set; }

        [Display(Name = "35.1.1 Rule-35 Is signing certificate given in wages register or wages-cum-muster roll register?")]
        [Required(ErrorMessage = "Rule_35_Is_Signing_Certificate_Given_In_Wages_Register is required..!")]
        public GeneralOptionTypeEnum Rule_35_Is_Signing_Certificate_Given_In_Wages_Register { get; set; }

        [Display(Name = "35.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_35_Is_Signing_Certificate_Given_In_Wages_Register_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_35_Is_Signing_Certificate_Given_In_Wages_Register_ViolationExist { get; set; }

        [Display(Name = "35.1.3 Rule_35_Is_Signing_Certificate_Given_In_Wages_Register_Remarks")]
        [Required(ErrorMessage = "Rule_35_Is_Signing_Certificate_Given_In_Wages_Register_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_35_Is_Signing_Certificate_Given_In_Wages_Register_Remarks can have max 1000 chars..!")]
        public string Rule_35_Is_Signing_Certificate_Given_In_Wages_Register_Remarks { get; set; }

        [Display(Name = "33.48.1.1 Section-33, Rule-48 Is register of contractor in Form XII maintained by principal employer?")]
        [Required(ErrorMessage = "Sec_33_Rule_48_Is_Contractor_Register_Maintained_In_Form_XII_By_PE is required..!")]
        public GeneralOptionTypeEnum Sec_33_Rule_48_Is_Contractor_Register_Maintained_In_Form_XII_By_PE { get; set; }

        [Display(Name = "33.48.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_33_Rule_48_Is_Contractor_Register_Maintained_In_Form_XII_By_PE_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_33_Rule_48_Is_Contractor_Register_Maintained_In_Form_XII_By_PE_ViolationExist { get; set; }

        [Display(Name = "33.48.1.3 Sec_33_Rule_48_Is_Contractor_Register_Maintained_In_Form_XII_By_PE_Remarks")]
        [Required(ErrorMessage = "Sec_33_Rule_48_Is_Contractor_Register_Maintained_In_Form_XII_By_PE_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_33_Rule_48_Is_Contractor_Register_Maintained_In_Form_XII_By_PE_Remarks can have max 1000 chars..!")]
        public string Sec_33_Rule_48_Is_Contractor_Register_Maintained_In_Form_XII_By_PE_Remarks { get; set; }

        [Display(Name = "33.49.1.1 Section-33 & 35, Rule-49 Are contractor(s) maintaining register  of persons employed in Form XIII?")]
        [Required(ErrorMessage = "Sec_33_35_Rule_49_Is_Contractor_Maintained_Employee_Register_In_Form_XIII is required..!")]
        public GeneralOptionTypeEnum Sec_33_35_Rule_49_Is_Contractor_Maintained_Employee_Register_In_Form_XIII { get; set; }

        [Display(Name = "33.49.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_33_35_Rule_49_Is_Contractor_Maintained_Employee_Register_In_Form_XIII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_33_35_Rule_49_Is_Contractor_Maintained_Employee_Register_In_Form_XIII_ViolationExist { get; set; }

        [Display(Name = "33.49.1.3 Sec_33_35_Rule_49_Is_Contractor_Maintained_Employee_Register_In_Form_XIII_Remarks")]
        [Required(ErrorMessage = "Sec_33_35_Rule_49_Is_Contractor_Maintained_Employee_Register_In_Form_XIII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_33_35_Rule_49_Is_Contractor_Maintained_Employee_Register_In_Form_XIII_Remarks can have max 1000 chars..!")]
        public string Sec_33_35_Rule_49_Is_Contractor_Maintained_Employee_Register_In_Form_XIII_Remarks { get; set; }

        [Display(Name = "35.50.1.1 Section-35, Rule-50 Are service certificate in Form XIV issued to workers on termination?")]
        [Required(ErrorMessage = "Sec_35_Rule_50_Is_Service_Certificate_Issued_To_Workers is required..!")]
        public GeneralOptionTypeEnum Sec_35_Rule_50_Is_Service_Certificate_Issued_To_Workers { get; set; }

        [Display(Name = "35.50.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_35_Rule_50_Is_Service_Certificate_Issued_To_Workers_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_35_Rule_50_Is_Service_Certificate_Issued_To_Workers_ViolationExist { get; set; }

        [Display(Name = "35.50.1.3 Sec_35_Rule_50_Is_Service_Certificate_Issued_To_Workers_Remarks")]
        [Required(ErrorMessage = "Sec_35_Rule_50_Is_Service_Certificate_Issued_To_Workers_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_35_Rule_50_Is_Service_Certificate_Issued_To_Workers_Remarks can have max 1000 chars..!")]
        public string Sec_35_Rule_50_Is_Service_Certificate_Issued_To_Workers_Remarks { get; set; }

        [Display(Name = "23.51.1.1 Section-23, Rule-51 Is contractor maintaining displacement_cum_outward journey allowance in Form XV")]
        [Required(ErrorMessage = "Sec_23_Rule_51_Is_Contractor_Maintained_Displacement_Cum_Outward_Journey_Allowance is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_51_Is_Contractor_Maintained_Displacement_Cum_Outward_Journey_Allowance { get; set; }

        [Display(Name = "35.50.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_51_Is_Contractor_Maintained_Displacement_Cum_Outward_Journey_Allowance_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_51_Is_Contractor_Maintained_Displacement_Cum_Outward_Journey_Allowance_ViolationExist { get; set; }

        [Display(Name = "23.51.1.3 Sec_23_Rule_51_Is_Contractor_Maintained_Displacement_Cum_Outward_Journey_Allowance_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_51_Is_Contractor_Maintained_Displacement_Cum_Outward_Journey_Allowance_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_51_Is_Contractor_Maintained_Displacement_Cum_Outward_Journey_Allowance_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_51_Is_Contractor_Maintained_Displacement_Cum_Outward_Journey_Allowance_Remarks { get; set; }

        [Display(Name = "35.50.2.1 Is contractor maintaining return journey allowance in Form XVI")]
        [Required(ErrorMessage = "Sec_23_Rule_51_Is_Contractor_Maintained_Return_Journey_Allowance is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_51_Is_Contractor_Maintained_Return_Journey_Allowance { get; set; }

        [Display(Name = "35.50.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_51_Is_Contractor_Maintained_Return_Journey_Allowance_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_51_Is_Contractor_Maintained_Return_Journey_Allowance_ViolationExist { get; set; }

        [Display(Name = "35.50.2.3 Sec_23_Rule_51_Is_Contractor_Maintained_Return_Journey_Allowance_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_51_Is_Contractor_Maintained_Return_Journey_Allowance_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_51_Is_Contractor_Maintained_Return_Journey_Allowance_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_51_Is_Contractor_Maintained_Return_Journey_Allowance_Remarks { get; set; }

        [Display(Name = "23.52.1.1 Section-23, Rule-52 Is rule-52(1) applicable?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Rule_52_1_Applicable is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Rule_52_1_Applicable { get; set; }

        [Display(Name = "23.52.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Rule_52_1_Applicable_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Rule_52_1_Applicable_ViolationExist { get; set; }

        [Display(Name = "23.52.1.3 Sec_23_Rule_52_Is_Rule_52_1_Applicable_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Rule_52_1_Applicable_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Rule_52_1_Applicable_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Rule_52_1_Applicable_Remarks { get; set; }

        [Display(Name = "23.52.2.1 Is muster roll in Form XVII maintained?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Muster_Roll_Form_XVII_Maintained is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Muster_Roll_Form_XVII_Maintained { get; set; }

        [Display(Name = "23.52.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Muster_Roll_Form_XVII_Maintained_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Muster_Roll_Form_XVII_Maintained_ViolationExist { get; set; }

        [Display(Name = "23.52.2.3 Sec_23_Rule_52_Is_Muster_Roll_Form_XVII_Maintained_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Muster_Roll_Form_XVII_Maintained_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Muster_Roll_Form_XVII_Maintained_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Muster_Roll_Form_XVII_Maintained_Remarks { get; set; }

        [Display(Name = "23.52.2.1 Is register of wages in Form XVIII maintained?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Wages_Register_Maintained_In_Form_XVIII is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Wages_Register_Maintained_In_Form_XVIII { get; set; }

        [Display(Name = "23.52.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Wages_Register_Maintained_In_Form_XVIII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Wages_Register_Maintained_In_Form_XVIII_ViolationExist { get; set; }

        [Display(Name = "23.52.2.3 Sec_23_Rule_52_Is_Wages_Register_Maintained_In_Form_XVIII_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Wages_Register_Maintained_In_Form_XVIII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Wages_Register_Maintained_In_Form_XVIII_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Wages_Register_Maintained_In_Form_XVIII_Remarks { get; set; }

        [Display(Name = "23.52.3.1 Is register of wages-cum-muster roll in Form XVIII maintained?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII { get; set; }

        [Display(Name = "23.52.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_ViolationExist { get; set; }

        [Display(Name = "23.52.3.3 Sec_23_Rule_52_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Wages_Cum_Muster_Roll_Register_Maintained_In_Form_XVIII_Remarks { get; set; }

        [Display(Name = "23.52.4.1 Is register of deduction in Form XIX maintained?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Deduction_Register_Maintained_In_Form_XIX is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Deduction_Register_Maintained_In_Form_XIX { get; set; }

        [Display(Name = "23.52.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Deduction_Register_Maintained_In_Form_XIX_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Deduction_Register_Maintained_In_Form_XIX_ViolationExist { get; set; }

        [Display(Name = "23.52.4.3 Sec_23_Rule_52_Is_Deduction_Register_Maintained_In_Form_XIX_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Deduction_Register_Maintained_In_Form_XIX_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Deduction_Register_Maintained_In_Form_XIX_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Deduction_Register_Maintained_In_Form_XIX_Remarks { get; set; }

        [Display(Name = "23.52.5.1 Is register of fines in Form XX maintained?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Fine_Register_Maintained_In_Form_XX is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Fine_Register_Maintained_In_Form_XX { get; set; }

        [Display(Name = "23.52.4.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Fine_Register_Maintained_In_Form_XX_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Fine_Register_Maintained_In_Form_XX_ViolationExist { get; set; }

        [Display(Name = "23.52.4.1 Sec_23_Rule_52_Is_Fine_Register_Maintained_In_Form_XX_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Fine_Register_Maintained_In_Form_XX_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Fine_Register_Maintained_In_Form_XX_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Fine_Register_Maintained_In_Form_XX_Remarks { get; set; }

        [Display(Name = "23.52.5.1 Is register of advances in Form XXI maintained?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Advances_Register_Maintained_In_Form_XXI is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Advances_Register_Maintained_In_Form_XXI { get; set; }

        [Display(Name = "23.52.5.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Advances_Register_Maintained_In_Form_XXI_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Advances_Register_Maintained_In_Form_XXI_ViolationExist { get; set; }

        [Display(Name = "23.52.5.3 Sec_23_Rule_52_Is_Advances_Register_Maintained_In_Form_XXI_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Advances_Register_Maintained_In_Form_XXI_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Advances_Register_Maintained_In_Form_XXI_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Advances_Register_Maintained_In_Form_XXI_Remarks { get; set; }

        [Display(Name = "23.52.6.1 Is register of overtime  in Form XXII maintained?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Overtime_Register_Maintained_In_Form_XXII is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Overtime_Register_Maintained_In_Form_XXII { get; set; }

        [Display(Name = "23.52.6.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Overtime_Register_Maintained_In_Form_XXII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Overtime_Register_Maintained_In_Form_XXII_ViolationExist { get; set; }

        [Display(Name = "23.52.6.3 Sec_23_Rule_52_Is_Overtime_Register_Maintained_In_Form_XXII_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Overtime_Register_Maintained_In_Form_XXII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Overtime_Register_Maintained_In_Form_XXII_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Overtime_Register_Maintained_In_Form_XXII_Remarks { get; set; }

        [Display(Name = "23.52.7.1 Are signature or thump impression of workers are taken in Register of Wages or Muster Roll_cum_Register of Wages, as the case may be, mentioned registered and are authenticated upon by employer/authorized representative?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Biometric_Of_Workers_Taken is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Biometric_Of_Workers_Taken { get; set; }

        [Display(Name = "23.52.7.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Biometric_Of_Workers_Taken_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Biometric_Of_Workers_Taken_ViolationExist { get; set; }

        [Display(Name = "23.52.7.3 Sec_23_Rule_52_Is_Biometric_Of_Workers_Taken_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Biometric_Of_Workers_Taken_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Biometric_Of_Workers_Taken_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Biometric_Of_Workers_Taken_Remarks { get; set; }

        [Display(Name = "23.52.8.1 Are provision of Rule 52(3) applicable?")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Provision_Of_Rule_52_3_Applicable is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_52_Is_Provision_Of_Rule_52_3_Applicable { get; set; }

        [Display(Name = "23.52.8.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Provision_Of_Rule_52_3_Applicable_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_52_Is_Provision_Of_Rule_52_3_Applicable_ViolationExist { get; set; }

        [Display(Name = "23.52.8.3 Sec_23_Rule_52_Is_Provision_Of_Rule_52_3_Applicable_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_52_Is_Provision_Of_Rule_52_3_Applicable_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_52_Is_Provision_Of_Rule_52_3_Applicable_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_52_Is_Provision_Of_Rule_52_3_Applicable_Remarks { get; set; }

        [Display(Name = "23.54.1.1 Section-23, Rule-54 Are notices, prescribed in rule 79, displayed?")]
        [Required(ErrorMessage = "Sec_23_Rule_54_Is_Notices_Prescribed_In_Rule_79_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_54_Is_Notices_Prescribed_In_Rule_79_Displayed { get; set; }
        [Display(Name = "23.54.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_54_Is_Notices_Prescribed_In_Rule_79_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_54_Is_Notices_Prescribed_In_Rule_79_Displayed_ViolationExist { get; set; }

        [Display(Name = "23.54.1.3 Sec_23_Rule_54_Is_Notices_Prescribed_In_Rule_79_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_54_Is_Notices_Prescribed_In_Rule_79_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_54_Is_Notices_Prescribed_In_Rule_79_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_54_Is_Notices_Prescribed_In_Rule_79_Displayed_Remarks { get; set; }

        [Display(Name = "23.53.1.1 Section-23, Rule-53 Are registered preserved for three years after date of  last entry made in therein?")]
        [Required(ErrorMessage = "Sec_23_Rule_53_IsRegistered_Preserved_For_Three_Years is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_53_IsRegistered_Preserved_For_Three_Years { get; set; }

        [Display(Name = "23.53.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_53_IsRegistered_Preserved_For_Three_Years_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_53_IsRegistered_Preserved_For_Three_Years_ViolationExist { get; set; }

        [Display(Name = "23.53.1.3 Sec_23_Rule_53_IsRegistered_Preserved_For_Three_Years_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_53_IsRegistered_Preserved_For_Three_Years_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_53_IsRegistered_Preserved_For_Three_Years_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_53_IsRegistered_Preserved_For_Three_Years_Remarks { get; set; }

        [Display(Name = "23.53.2.1 Are registered produced at time of inspection?")]
        [Required(ErrorMessage = "Sec_23_Rule_53_IsRegistered_Produced_While_Inspection is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_53_IsRegistered_Produced_While_Inspection { get; set; }

        [Display(Name = "23.53.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_53_IsRegistered_Produced_While_Inspection_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_53_IsRegistered_Produced_While_Inspection_ViolationExist { get; set; }

        [Display(Name = "23.53.2.3 Sec_23_Rule_53_IsRegistered_Produced_While_Inspection_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_53_IsRegistered_Produced_While_Inspection_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_53_IsRegistered_Produced_While_Inspection_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_53_IsRegistered_Produced_While_Inspection_Remarks { get; set; }

        [Display(Name = "23.55.1.1 Section-23, Rule-55 Are notices, prescribed in rule 55, displayed?")]
        [Required(ErrorMessage = "Sec_23_Rule_55_Is_Notices_Prescribed_In_Rule_Displayed is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_55_Is_Notices_Prescribed_In_Rule_Displayed { get; set; }

        [Display(Name = "23.55.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_55_Is_Notices_Prescribed_In_Rule_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_55_Is_Notices_Prescribed_In_Rule_Displayed_ViolationExist { get; set; }

        [Display(Name = "23.55.1.3 Sec_23_Rule_55_Is_Notices_Prescribed_In_Rule_Displayed_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_55_Is_Notices_Prescribed_In_Rule_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_55_Is_Notices_Prescribed_In_Rule_Displayed_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_55_Is_Notices_Prescribed_In_Rule_Displayed_Remarks { get; set; }

        [Display(Name = "23.56.1.1 Section-23, Rule-56 Is half yearly return in Form XXIII submitted by each contractor?")]
        [Required(ErrorMessage = "Sec_23_Rule_56_Is_HalfYearlyReturn_In_Form_XXIII_Submitted is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_56_Is_HalfYearlyReturn_In_Form_XXIII_Submitted { get; set; }

        [Display(Name = "23.56.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_56_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_56_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_ViolationExist { get; set; }

        [Display(Name = "23.56.1.3 Sec_23_Rule_56_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_56_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_56_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_56_Is_HalfYearlyReturn_In_Form_XXIII_Submitted_Remarks { get; set; }

        [Display(Name = "23.56.2.1 Is annual return in Form XXIV is submitted by principal employer?")]
        [Required(ErrorMessage = "Sec_23_Rule_56_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE is required..!")]
        public GeneralOptionTypeEnum Sec_23_Rule_56_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE { get; set; }

        [Display(Name = "23.56.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_23_Rule_56_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_23_Rule_56_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_ViolationExist { get; set; }

        [Display(Name = "23.56.2.3 Sec_23_Rule_56_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_Remarks")]
        [Required(ErrorMessage = "Sec_23_Rule_56_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_23_Rule_56_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_Remarks can have max 1000 chars..!")]
        public string Sec_23_Rule_56_Is_AnnualReturn_In_Form_XXIV_Submitted_By_PE_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_LabourWelfareFund_Act
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "Whether fund due on date of Inspection is paid?")]
        [Required(ErrorMessage = "IsLabourWelfareFund_Paid is required..!")]
        public bool IsLabourWelfareFund_Paid { get; set; }

        [Display(Name = "Deposited Date")]
        [Required(ErrorMessage = "DepositedDate is required..!")]
        public DateTime? DepositedDate { get; set; }

        [Display(Name = "Deposited Amount")]
        [Required(ErrorMessage = "DepositedAmount is required..!")]
        public decimal? DepositedAmount { get; set; }

        #region LWF Details
        [Display(Name = "Establishment Name")]
        [Required(ErrorMessage = "EstablishmentName is required..!")]
        public string? EstablishmentName { get; set; }

        [Display(Name = "Period")]
        [Required(ErrorMessage = "Period is required..!")]
        public string? Period { get; set; }

        [Display(Name = "No Of Workers")]
        [Required(ErrorMessage = "NoOfWorkers is required..!")]
        public int? NoOfWorkers { get; set; }

        [Display(Name = "Contribution Amount")]
        [Required(ErrorMessage = "ContributionAmount is required..!")]
        public decimal? ContributionAmount { get; set; }

        [Display(Name = "Unpaid Accumulation still Payable (Default Amount)")]
        [Required(ErrorMessage = "UnpaidAccumulation is required..!")]
        public string? UnpaidAccumulation { get; set; }

        [Display(Name = "Contribution Paid And Period")]
        [Required(ErrorMessage = "ContributionPaidAndPeriod is required..!")]
        public string? ContributionPaidAndPeriod { get; set; }

        [Display(Name = "Unclaimed Paid and Period")]
        [Required(ErrorMessage = "UnclaimedPaidAndPeriod is required..!")]
        public string? UnclaimedPaidAndPeriod { get; set; }

        [Display(Name = "Contribution Amount still payable(Default Amount)")]
        [Required(ErrorMessage = "ContributionAmountStillPayable is required..!")]
        public string? ContributionAmountStillPayable { get; set; }

        [Display(Name = "Unclaimed Paid Or Not")]
        [Required(ErrorMessage = "UnclaimedPaidOrNot is required..!")]
        public string? UnclaimedPaidOrNot { get; set; }

        [Display(Name = "Remarks")]
        [Required(ErrorMessage = "Remarks is required..!")]
        public string? Remarks { get; set; }


        #endregion
        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Labour_III_GratuityAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "3.1.1 Rule-3 Is notice in Form A given?")]
        [Required(ErrorMessage = "Rule_3_Is_Notice_In_Form_A_Given is required..!")]
        public GeneralOptionTypeEnum Rule_3_Is_Notice_In_Form_A_Given { get; set; }

        [Display(Name = "3.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_3_Is_Notice_In_Form_A_Given_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_3_Is_Notice_In_Form_A_Given_ViolationExist { get; set; }

        [Display(Name = "3.1.3 Rule_3_Is_Notice_In_Form_A_Given_Remarks")]
        [Required(ErrorMessage = "Rule_3_Is_Notice_In_Form_A_Given_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_3_Is_Notice_In_Form_A_Given_Remarks can have max 1000 chars..!")]
        public string Rule_3_Is_Notice_In_Form_A_Given_Remarks { get; set; }

        [Display(Name = "4.1.1 Rule-4 Is notice displayed as specified in this rule?")]
        [Required(ErrorMessage = "Rule_4_Is_Notice_Displayed is required..!")]
        public GeneralOptionTypeEnum Rule_4_Is_Notice_Displayed { get; set; }

        [Display(Name = "4.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_4_Is_Notice_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_4_Is_Notice_Displayed_ViolationExist { get; set; }

        [Display(Name = "4.1.3 Rule_4_Is_Notice_Displayed_Remarks")]
        [Required(ErrorMessage = "Rule_4_Is_Notice_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_4_Is_Notice_Displayed_Remarks can have max 1000 chars..!")]
        public string Rule_4_Is_Notice_Displayed_Remarks { get; set; }

        [Display(Name = "5.1.1 Rule-5 Is notice in Form D or E given? ")]
        [Required(ErrorMessage = "Rule_5_Is_Notice_In_Form_D_E_Given is required..!")]
        public GeneralOptionTypeEnum Rule_5_Is_Notice_In_Form_D_E_Given { get; set; }

        [Display(Name = "5.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_5_Is_Notice_In_Form_D_E_Given_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_5_Is_Notice_In_Form_D_E_Given_ViolationExist { get; set; }

        [Display(Name = "5.1.3 Rule_5_Is_Notice_In_Form_D_E_Given_Remarks")]
        [Required(ErrorMessage = "Rule_5_Is_Notice_In_Form_D_E_Given_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_5_Is_Notice_In_Form_D_E_Given_Remarks can have max 1000 chars..!")]
        public string Rule_5_Is_Notice_In_Form_D_E_Given_Remarks { get; set; }

        [Display(Name = "6.1.1 Section-6, Rule-6 Is nomination filed in Form F by employees?")]
        [Required(ErrorMessage = "Rule_6_Rule_6_Is_Nominee_Filled_By_Employee_In_Form_F is required..!")]
        public GeneralOptionTypeEnum Rule_6_Rule_6_Is_Nominee_Filled_By_Employee_In_Form_F { get; set; }

        [Display(Name = "6.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_6_Rule_6_Is_Nominee_Filled_By_Employee_In_Form_F_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_6_Rule_6_Is_Nominee_Filled_By_Employee_In_Form_F_ViolationExist { get; set; }

        [Display(Name = "6.1.3 Rule_6_Rule_6_Is_Nominee_Filled_By_Employee_In_Form_F_Remarks")]
        [Required(ErrorMessage = "Rule_6_Rule_6_Is_Nominee_Filled_By_Employee_In_Form_F_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_6_Rule_6_Is_Nominee_Filled_By_Employee_In_Form_F_Remarks can have max 1000 chars..!")]
        public string Rule_6_Rule_6_Is_Nominee_Filled_By_Employee_In_Form_F_Remarks { get; set; }

        [Display(Name = "8.1.1 Are notices in Form L or M endorsed to controlling authority?Are notices in Form L or M endorsed to controlling authority?")]
        [Required(ErrorMessage = "Rule_8_Is_Notice_In_Form_L_M_Endorsed_To_Authority is required..!")]
        public GeneralOptionTypeEnum Rule_8_Is_Notice_In_Form_L_M_Endorsed_To_Authority { get; set; }

        [Display(Name = "8.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_8_Is_Notice_In_Form_L_M_Endorsed_To_Authority_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_8_Is_Notice_In_Form_L_M_Endorsed_To_Authority_ViolationExist { get; set; }

        [Display(Name = "8.1.3 Rule_8_Is_Notice_In_Form_L_M_Endorsed_To_Authority_Remarks")]
        [Required(ErrorMessage = "Rule_8_Is_Notice_In_Form_L_M_Endorsed_To_Authority_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_8_Is_Notice_In_Form_L_M_Endorsed_To_Authority_Remarks can have max 1000 chars..!")]
        public string Rule_8_Is_Notice_In_Form_L_M_Endorsed_To_Authority_Remarks { get; set; }

        [Display(Name = "20.1.1 Rule-20 Is notice in Form U as specified in this rule displayed?")]
        [Required(ErrorMessage = "Rule_20_Is_Notice_In_Form_U_Displayed is required..!")]
        public GeneralOptionTypeEnum Rule_20_Is_Notice_In_Form_U_Displayed { get; set; }

        [Display(Name = "20.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Rule_20_Is_Notice_In_Form_U_Displayed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Rule_20_Is_Notice_In_Form_U_Displayed_ViolationExist { get; set; }

        [Display(Name = "20.1.3 Rule_20_Is_Notice_In_Form_U_Displayed_Remarks")]
        [Required(ErrorMessage = "Rule_20_Is_Notice_In_Form_U_Displayed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Rule_20_Is_Notice_In_Form_U_Displayed_Remarks can have max 1000 chars..!")]
        public string Rule_20_Is_Notice_In_Form_U_Displayed_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_IndustrialEmploymentAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "1.1.1 Section-1(3) Is this Act applicable on establishment")]
        [Required(ErrorMessage = "Sec_1_3_Is_Act_Applicable_On_Establishment is required..!")]
        public GeneralOptionTypeEnum Sec_1_3_Is_Act_Applicable_On_Establishment { get; set; }

        [Display(Name = "1.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_1_3_Is_Act_Applicable_On_Establishment_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_1_3_Is_Act_Applicable_On_Establishment_ViolationExist { get; set; }

        [Display(Name = "1.1.3 Sec_1_3_Is_Act_Applicable_On_Establishment_Remarks")]
        [Required(ErrorMessage = "Sec_1_3_Is_Act_Applicable_On_Establishment_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_1_3_Is_Act_Applicable_On_Establishment_Remarks can have max 1000 chars..!")]
        public string Sec_1_3_Is_Act_Applicable_On_Establishment_Remarks { get; set; }

        [Display(Name = "5.1.1 Section-5 Is establishment falling in exempted category as per notification No.21/65/2019/-4L/1098 dated 06-08-2020")]
        [Required(ErrorMessage = "Sec_5_Is_Establishment_Fall_In_Exempted_Category is required..!")]
        public GeneralOptionTypeEnum Sec_5_Is_Establishment_Fall_In_Exempted_Category { get; set; }

        [Display(Name = "5.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_5_Is_Establishment_Fall_In_Exempted_Category_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_5_Is_Establishment_Fall_In_Exempted_Category_ViolationExist { get; set; }

        [Display(Name = "5.1.3 Sec_5_Is_Establishment_Fall_In_Exempted_Category_Remarks")]
        [Required(ErrorMessage = "Sec_5_Is_Establishment_Fall_In_Exempted_Category_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_5_Is_Establishment_Fall_In_Exempted_Category_Remarks can have max 1000 chars..!")]
        public string Sec_5_Is_Establishment_Fall_In_Exempted_Category_Remarks { get; set; }

        [Display(Name = "5.2.1 If yes then Is establishment complying the provisions of notification No.21/65/2019/-4L/1098 dated 06-08-2020")]
        [Required(ErrorMessage = "Sec_5_Is_Establishment_Complying_Provission_Of_Notification is required..!")]
        public GeneralOptionTypeEnum Sec_5_Is_Establishment_Complying_Provission_Of_Notification { get; set; }

        [Display(Name = "5.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_5_Is_Establishment_Complying_Provission_Of_Notification_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_5_Is_Establishment_Complying_Provission_Of_Notification_ViolationExist { get; set; }

        [Display(Name = "5.2.3 Sec_5_Is_Establishment_Complying_Provission_Of_Notification_Remarks")]
        [Required(ErrorMessage = "Sec_5_Is_Establishment_Complying_Provission_Of_Notification_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_5_Is_Establishment_Complying_Provission_Of_Notification_Remarks can have max 1000 chars..!")]
        public string Sec_5_Is_Establishment_Complying_Provission_Of_Notification_Remarks { get; set; }

        [Display(Name = "5.3.1 If no then Are standing orders submitted to/certified by certifying officer?")]
        [Required(ErrorMessage = "Sec_5_Is_Standing_Orders_Submitted_By_Certifying_Officer is required..!")]
        public GeneralOptionTypeEnum Sec_5_Is_Standing_Orders_Submitted_By_Certifying_Officer { get; set; }

        [Display(Name = "5.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_5_Is_Standing_Orders_Submitted_By_Certifying_Officer_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_5_Is_Standing_Orders_Submitted_By_Certifying_Officer_ViolationExist { get; set; }

        [Display(Name = "5.3.3 Sec_5_Is_Standing_Orders_Submitted_By_Certifying_Officer_Remarks")]
        [Required(ErrorMessage = "Sec_5_Is_Standing_Orders_Submitted_By_Certifying_Officer_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_5_Is_Standing_Orders_Submitted_By_Certifying_Officer_Remarks can have max 1000 chars..!")]
        public string Sec_5_Is_Standing_Orders_Submitted_By_Certifying_Officer_Remarks { get; set; }

        [Display(Name = "6.1.1 If any appeal under this section pending with Appellate Authority?")]
        [Required(ErrorMessage = "Sec_6_Is_Any_Appeal_Pending_With_Appellate_Authority is required..!")]
        public GeneralOptionTypeEnum Sec_6_Is_Any_Appeal_Pending_With_Appellate_Authority { get; set; }

        [Display(Name = "6.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_6_Is_Any_Appeal_Pending_With_Appellate_Authority_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_6_Is_Any_Appeal_Pending_With_Appellate_Authority_ViolationExist { get; set; }

        [Display(Name = "6.1.3 Sec_6_Is_Any_Appeal_Pending_With_Appellate_Authority_Remarks")]
        [Required(ErrorMessage = "Sec_6_Is_Any_Appeal_Pending_With_Appellate_Authority_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_6_Is_Any_Appeal_Pending_With_Appellate_Authority_Remarks can have max 1000 chars..!")]
        public string Sec_6_Is_Any_Appeal_Pending_With_Appellate_Authority_Remarks { get; set; }

        [Display(Name = "9.1.1 Section-9 Are Standing Orders posted by employer as per provision of this section?")]
        [Required(ErrorMessage = "Sec_9_Is_Standing_Order_Posted_By_Employer is required..!")]
        public GeneralOptionTypeEnum Sec_9_Is_Standing_Order_Posted_By_Employer { get; set; }

        [Display(Name = "9.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_9_Is_Standing_Order_Posted_By_Employer_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_9_Is_Standing_Order_Posted_By_Employer_ViolationExist { get; set; }

        [Display(Name = "9.1.3 Sec_9_Is_Standing_Order_Posted_By_Employer_Remarks")]
        [Required(ErrorMessage = "Sec_9_Is_Standing_Order_Posted_By_Employer_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_9_Is_Standing_Order_Posted_By_Employer_Remarks can have max 1000 chars..!")]
        public string Sec_9_Is_Standing_Order_Posted_By_Employer_Remarks { get; set; }

        [Display(Name = "10A.1.1 Section-10A Is subsistence allowance being given to suspended workmen as per provision of this section?")]
        [Required(ErrorMessage = "Sec_10_A_Is_Subsistence_Allowance_Given_To_Suspended_Workmen is required..!")]
        public GeneralOptionTypeEnum Sec_10_A_Is_Subsistence_Allowance_Given_To_Suspended_Workmen { get; set; }

        [Display(Name = "10A.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Sec_10_A_Is_Subsistence_Allowance_Given_To_Suspended_Workmen_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Sec_10_A_Is_Subsistence_Allowance_Given_To_Suspended_Workmen_ViolationExist { get; set; }

        [Display(Name = "10A.1.3 Sec_10_A_Is_Subsistence_Allowance_Given_To_Suspended_Workmen_Remarks")]
        [Required(ErrorMessage = "Sec_10_A_Is_Subsistence_Allowance_Given_To_Suspended_Workmen_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Sec_10_A_Is_Subsistence_Allowance_Given_To_Suspended_Workmen_Remarks can have max 1000 chars..!")]
        public string Sec_10_A_Is_Subsistence_Allowance_Given_To_Suspended_Workmen_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_BOCW_Act
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "1.1.1 Sr No-1 Whether the construction establishment is duly registered under the Act")]
        [Required(ErrorMessage = "Is_Construction_Establishment_Registered_Under_Act is required..!")]
        public GeneralOptionTypeEnum Is_Construction_Establishment_Registered_Under_Act { get; set; }

        [Display(Name = "1.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Construction_Establishment_Registered_Under_Act_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Construction_Establishment_Registered_Under_Act_ViolationExist { get; set; }

        [Display(Name = "1.1.3 Is_Construction_Establishment_Registered_Under_Act_Remarks")]
        [Required(ErrorMessage = "Is_Construction_Establishment_Registered_Under_Act_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Construction_Establishment_Registered_Under_Act_Remarks can have max 1000 chars..!")]
        public string Is_Construction_Establishment_Registered_Under_Act_Remarks { get; set; }

        [Display(Name = "2.1.1 Sr No-2 Whether the workers engaged therein registered under the Act.  ")]
        [Required(ErrorMessage = "Is_Engaged_Workers_Registered_Under_Act is required..!")]
        public GeneralOptionTypeEnum Is_Engaged_Workers_Registered_Under_Act { get; set; }

        [Display(Name = "2.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Engaged_Workers_Registered_Under_Act_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Engaged_Workers_Registered_Under_Act_ViolationExist { get; set; }

        [Display(Name = "2.1.3 Is_Engaged_Workers_Registered_Under_Act_Remarks")]
        [Required(ErrorMessage = "Is_Engaged_Workers_Registered_Under_Act_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Engaged_Workers_Registered_Under_Act_Remarks can have max 1000 chars..!")]
        public string Is_Engaged_Workers_Registered_Under_Act_Remarks { get; set; }

        [Display(Name = "3.1.1 Sr No-3 Whether the employer of the construction establishment submitted the Notice of commencement and completion of the construction work.")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Commencement_Completion_Notice is required..!")]
        public GeneralOptionTypeEnum Is_Employer_Submitted_Commencement_Completion_Notice { get; set; }

        [Display(Name = "3.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Commencement_Completion_Notice_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Employer_Submitted_Commencement_Completion_Notice_ViolationExist { get; set; }

        [Display(Name = "3.1.3 Is_Employer_Submitted_Commencement_Completion_Notice_Remarks")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Commencement_Completion_Notice_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Employer_Submitted_Commencement_Completion_Notice_Remarks can have max 1000 chars..!")]
        public string Is_Employer_Submitted_Commencement_Completion_Notice_Remarks { get; set; }

        [Display(Name = "4.1.1 Sr No-4 Whether the employer of the construction establishment Submission of information in Form No-I to the Assessing officer within 30 days of commencement of work.")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Form_I_Within_30Days_Commencement_Of_Work is required..!")]
        public GeneralOptionTypeEnum Is_Employer_Submitted_Form_I_Within_30Days_Commencement_Of_Work { get; set; }

        [Display(Name = "4.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Form_I_Within_30Days_Commencement_Of_Work_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Employer_Submitted_Form_I_Within_30Days_Commencement_Of_Work_ViolationExist { get; set; }

        [Display(Name = "4.1.3 Is_Employer_Submitted_Form_I_Within_30Days_Commencement_Of_Work_Remarks")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Form_I_Within_30Days_Commencement_Of_Work_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Employer_Submitted_Form_I_Within_30Days_Commencement_Of_Work_Remarks can have max 1000 chars..!")]
        public string Is_Employer_Submitted_Form_I_Within_30Days_Commencement_Of_Work_Remarks { get; set; }

        [Display(Name = "5.1.1 Sr No-5 Whether the establishment paid 1% Cess on the total cost of construction.")]
        [Required(ErrorMessage = "Is_One_Percent_Cess_Being_Paid_Of_Total_Cost is required..!")]
        public GeneralOptionTypeEnum Is_One_Percent_Cess_Being_Paid_Of_Total_Cost { get; set; }

        [Display(Name = "5.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_One_Percent_Cess_Being_Paid_Of_Total_Cost_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_One_Percent_Cess_Being_Paid_Of_Total_Cost_ViolationExist { get; set; }

        [Display(Name = "5.1.3 Is_One_Percent_Cess_Being_Paid_Of_Total_Cost_Remarks")]
        [Required(ErrorMessage = "Is_One_Percent_Cess_Being_Paid_Of_Total_Cost_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_One_Percent_Cess_Being_Paid_Of_Total_Cost_Remarks can have max 1000 chars..!")]
        public string Is_One_Percent_Cess_Being_Paid_Of_Total_Cost_Remarks { get; set; }

        [Display(Name = "6.1.1 Sr No-6 Whether the establishment maintained the Muster Roll in Form No.XVI.")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Muster_Roll_In_Form_XVI is required..!")]
        public GeneralOptionTypeEnum Is_Establishment_Maintained_Muster_Roll_In_Form_XVI { get; set; }

        [Display(Name = "6.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Muster_Roll_In_Form_XVI_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Establishment_Maintained_Muster_Roll_In_Form_XVI_ViolationExist { get; set; }

        [Display(Name = "6.1.3 Is_Establishment_Maintained_Muster_Roll_In_Form_XVI_Remarks")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Muster_Roll_In_Form_XVI_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Establishment_Maintained_Muster_Roll_In_Form_XVI_Remarks can have max 1000 chars..!")]
        public string Is_Establishment_Maintained_Muster_Roll_In_Form_XVI_Remarks { get; set; }

        [Display(Name = "6.2.1 Whether the establishment maintained the Register of Wages in Form No. XVII")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Wages_Register_In_Form_XVII is required..!")]
        public GeneralOptionTypeEnum Is_Establishment_Maintained_Wages_Register_In_Form_XVII { get; set; }

        [Display(Name = "6.2.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Wages_Register_In_Form_XVII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Establishment_Maintained_Wages_Register_In_Form_XVII_ViolationExist { get; set; }

        [Display(Name = "6.2.3 Is_Establishment_Maintained_Wages_Register_In_Form_XVII_Remarks")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Wages_Register_In_Form_XVII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Establishment_Maintained_Wages_Register_In_Form_XVII_Remarks can have max 1000 chars..!")]
        public string Is_Establishment_Maintained_Wages_Register_In_Form_XVII_Remarks { get; set; }

        [Display(Name = "6.3.1 Whether the establishment maintained the Register of Overtime in Form No. XXII")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Overtime_Register_In_Form_XXII is required..!")]
        public GeneralOptionTypeEnum Is_Establishment_Maintained_Overtime_Register_In_Form_XXII { get; set; }

        [Display(Name = "6.3.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Overtime_Register_In_Form_XXII_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Establishment_Maintained_Overtime_Register_In_Form_XXII_ViolationExist { get; set; }

        [Display(Name = "6.3.3 Is_Establishment_Maintained_Overtime_Register_In_Form_XXII_Remarks")]
        [Required(ErrorMessage = "Is_Establishment_Maintained_Overtime_Register_In_Form_XXII_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Establishment_Maintained_Overtime_Register_In_Form_XXII_Remarks can have max 1000 chars..!")]
        public string Is_Establishment_Maintained_Overtime_Register_In_Form_XXII_Remarks { get; set; }

        [Display(Name = "7.1.1 Sr No-7 Whether employer of the construction establishment Provided the working conditions to the workers such as working hours, drinking water, Washrooms, free accommodation, crèches and first aid facilities.")]
        [Required(ErrorMessage = "Is_Employer_Provide_Working_Conditions_To_Workers is required..!")]
        public GeneralOptionTypeEnum Is_Employer_Provide_Working_Conditions_To_Workers { get; set; }

        [Display(Name = "7.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Employer_Provide_Working_Conditions_To_Workers_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Employer_Provide_Working_Conditions_To_Workers_ViolationExist { get; set; }

        [Display(Name = "7.1.3 Is_Employer_Provide_Working_Conditions_To_Workers_Remarks")]
        [Required(ErrorMessage = "Is_Employer_Provide_Working_Conditions_To_Workers_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Employer_Provide_Working_Conditions_To_Workers_Remarks can have max 1000 chars..!")]
        public string Is_Employer_Provide_Working_Conditions_To_Workers_Remarks { get; set; }

        [Display(Name = "8.1.1 Sr No-8 Whether the employer submitted Annual Return.")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Annual_Return is required..!")]
        public GeneralOptionTypeEnum Is_Employer_Submitted_Annual_Return { get; set; }

        [Display(Name = "8.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Annual_Return_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Employer_Submitted_Annual_Return_ViolationExist { get; set; }

        [Display(Name = "8.1.3 Is_Employer_Submitted_Annual_Return_Remarks")]
        [Required(ErrorMessage = "Is_Employer_Submitted_Annual_Return_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Employer_Submitted_Annual_Return_Remarks can have max 1000 chars..!")]
        public string Is_Employer_Submitted_Annual_Return_Remarks { get; set; }

        [Display(Name = "9.1.1 Sr No-9 Whether the employer of the construction establishment ensured the safety measures such as Fire protection equipments, Fencing of Motors, Eye protection, safety helmets and shoes etc.")]
        [Required(ErrorMessage = "Is_Employer_Ensured_Safety_Measures is required..!")]
        public GeneralOptionTypeEnum Is_Employer_Ensured_Safety_Measures { get; set; }

        [Display(Name = "9.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Employer_Ensured_Safety_Measures_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Employer_Ensured_Safety_Measures_ViolationExist { get; set; }

        [Display(Name = "9.1.3 Is_Employer_Ensured_Safety_Measures_Remarks")]
        [Required(ErrorMessage = "Is_Employer_Ensured_Safety_Measures_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Employer_Ensured_Safety_Measures_Remarks can have max 1000 chars..!")]
        public string Is_Employer_Ensured_Safety_Measures_Remarks { get; set; }

        [Display(Name = "10.1.1 Sr No-10 Nature of violation, if any found during the inspection")]
        [Required(ErrorMessage = "Is_Employer_Ensured_Safety_Measures is required..!")]
        public GeneralOptionTypeEnum Is_Any_Nature_Of_Violation_Found { get; set; }

        [Display(Name = "10.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Any_Nature_Of_Violation_Found_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Any_Nature_Of_Violation_Found_ViolationExist { get; set; }

        [Display(Name = "10.1.3 Is_Any_Nature_Of_Violation_Found_Remarks")]
        [Required(ErrorMessage = "Is_Any_Nature_Of_Violation_Found_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Any_Nature_Of_Violation_Found_Remarks can have max 1000 chars..!")]
        public string Is_Any_Nature_Of_Violation_Found_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }
    public class Inspection_Form_Labour_III_ShopAct
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "Nature Of Business Type")]
        [Required(ErrorMessage = "NatureOfBusinessType is required..!")]
        public NatureOfBusinessTypeEnum NatureOfBusinessType { get; set; }

        [Display(Name = "Number of workers employed Male")]
        [Required(ErrorMessage = "NoOfWorkerMale is required..!")]
        public Int64 NoOfWorkerMale { get; set; }

        [Display(Name = "Number of workers employed Female")]
        [Required(ErrorMessage = "NoOfWorkerFemale is required..!")]
        public Int64 NoOfWorkerFemale { get; set; }

        [Display(Name = "Number of workers employed Young Person")]
        [Required(ErrorMessage = "NoOfWorkerYoungPerson is required..!")]
        public Int64 NoOfWorkerYoungPerson { get; set; }

        [Display(Name = "Number of workers employed Child")]
        [Required(ErrorMessage = "NoOfWorkerChild is required..!")]
        public Int64 NoOfWorkerChild { get; set; }

        [Display(Name = "Whether Registration Certificate Obtained")]
        [Required(ErrorMessage = "Is_Registration_Certificate_Obtained is required..!")]
        public string Is_Registration_Certificate_Obtained { get; set; }

        [Display(Name = "Whether Registration Certificate is valid")]
        [Required(ErrorMessage = "Is_Registration_Certificate_Valid is required..!")]
        public string Is_Registration_Certificate_Valid { get; set; }

        [Display(Name = "Whether Shop or establishment has obtained any exemption under the Act (under Section 4)")]
        [Required(ErrorMessage = "Is_Shop_Or_Esablishment_Obtained_Exemption is required..!")]
        public string Is_Shop_Or_Esablishment_Obtained_Exemption { get; set; }

        [Display(Name = "Wage Period")]
        [Required(ErrorMessage = "WagesPeriod is required..!")]
        public string WagesPeriod { get; set; }

        [Display(Name = "Date of Payment of Wages")]
        [Required(ErrorMessage = "WagesPaymentDate is required..!")]
        public DateTime WagesPaymentDate { get; set; }

        [Display(Name = "Mode of Payment")]
        [Required(ErrorMessage = "WagesPaymentModeType is required..!")]
        public WagesPaymentModeTypeEnum WagesPaymentModeType { get; set; }

        [Display(Name = "Violation Found or Not")]
        [Required(ErrorMessage = "IsViolationFound is required..!")]
        public bool IsViolationFound { get; set; }

        [Display(Name = "Remarks")]
        [Required(ErrorMessage = "Remarks is required..!")]
        public string Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }

    public class Inspection_Form_Labour_III_Observations
    {
        [Key]
        [Display(Name = "Id")]
        public Int64 Id { get; set; }

        [Display(Name = "1.1.1 Sr No-1 Whether spread over is being observed as prescribed under the Act and during the period of rest, the workers are free to leave the place")]
        [Required(ErrorMessage = "Is_Spread_Over_Being_Observed_During_Rest is required..!")]
        public GeneralOptionTypeEnum Is_Spread_Over_Being_Observed_During_Rest { get; set; }

        [Display(Name = "1.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Spread_Over_Being_Observed_During_Rest_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Spread_Over_Being_Observed_During_Rest_ViolationExist { get; set; }

        [Display(Name = "1.1.3 Is_Spread_Over_Being_Observed_During_Rest_Remarks")]
        [Required(ErrorMessage = "Is_Spread_Over_Being_Observed_During_Rest_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Spread_Over_Being_Observed_During_Rest_Remarks can have max 1000 chars..!")]
        public string Is_Spread_Over_Being_Observed_During_Rest_Remarks { get; set; }

        [Display(Name = "2.1.1 Sr No-2 Whether any child was found employed")]
        [Required(ErrorMessage = "Is_Any_Child_Employee_Found is required..!")]
        public GeneralOptionTypeEnum Is_Any_Child_Employee_Found { get; set; }

        [Display(Name = "2.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Any_Child_Employee_Found_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Any_Child_Employee_Found_ViolationExist { get; set; }

        [Display(Name = "2.1.3 Is_Any_Child_Employee_Found_Remarks")]
        [Required(ErrorMessage = "Is_Any_Child_Employee_Found_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Any_Child_Employee_Found_Remarks can have max 1000 chars..!")]
        public string Is_Any_Child_Employee_Found_Remarks { get; set; }

        [Display(Name = "3.1.1 Sr No-3 Whether the working hours and timings in case of young persons, women are being adhered to")]
        [Required(ErrorMessage = "Is_Working_Hours_Adhered_For_Women_Young_Person is required..!")]
        public GeneralOptionTypeEnum Is_Working_Hours_Adhered_For_Women_Young_Person { get; set; }

        [Display(Name = "3.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Working_Hours_Adhered_For_Women_Young_Person_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Working_Hours_Adhered_For_Women_Young_Person_ViolationExist { get; set; }

        [Display(Name = "3.1.3 Is_Working_Hours_Adhered_For_Women_Young_Person_Remarks")]
        [Required(ErrorMessage = "Is_Working_Hours_Adhered_For_Women_Young_Person_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Working_Hours_Adhered_For_Women_Young_Person_Remarks can have max 1000 chars..!")]
        public string Is_Working_Hours_Adhered_For_Women_Young_Person_Remarks { get; set; }

        [Display(Name = "4.1.1 Sr No-4 Whether opening & closing hours are being observed.")]
        [Required(ErrorMessage = "Is_Opening_Closing_Hours_Observed is required..!")]
        public GeneralOptionTypeEnum Is_Opening_Closing_Hours_Observed { get; set; }

        [Display(Name = "4.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Opening_Closing_Hours_Observed_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Opening_Closing_Hours_Observed_ViolationExist { get; set; }

        [Display(Name = "4.1.3 Is_Opening_Closing_Hours_Observed_Remarks")]
        [Required(ErrorMessage = "Is_Opening_Closing_Hours_Observed_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Opening_Closing_Hours_Observed_Remarks can have max 1000 chars..!")]
        public string Is_Opening_Closing_Hours_Observed_Remarks { get; set; }

        [Display(Name = "5.1.1 Sr No-5 Whether close day is being observed, if not, whether the employees are being provided weekly holiday.")]
        [Required(ErrorMessage = "Is_Weekly_Holiday_Provided is required..!")]
        public GeneralOptionTypeEnum Is_Weekly_Holiday_Provided { get; set; }

        [Display(Name = "5.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Weekly_Holiday_Provided_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Weekly_Holiday_Provided_ViolationExist { get; set; }

        [Display(Name = "5.1.3 Is_Weekly_Holiday_Provided_Remarks")]
        [Required(ErrorMessage = "Is_Weekly_Holiday_Provided_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Weekly_Holiday_Provided_Remarks can have max 1000 chars..!")]
        public string Is_Weekly_Holiday_Provided_Remarks { get; set; }

        [Display(Name = "6.1.1 Sr No-6 Whether the employees are called for duty on National Holidays. If yes, whether they are being paid overtime wage and a compensatory holiday in lieu thereof.")]
        [Required(ErrorMessage = "Is_Overtime_Paid_For_Holiday_Work is required..!")]
        public GeneralOptionTypeEnum Is_Overtime_Paid_For_Holiday_Work { get; set; }

        [Display(Name = "6.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Overtime_Paid_For_Holiday_Work_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Overtime_Paid_For_Holiday_Work_ViolationExist { get; set; }

        [Display(Name = "6.1.3 Is_Overtime_Paid_For_Holiday_Work_Remarks")]
        [Required(ErrorMessage = "Is_Overtime_Paid_For_Holiday_Work_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Overtime_Paid_For_Holiday_Work_Remarks can have max 1000 chars..!")]
        public string Is_Overtime_Paid_For_Holiday_Work_Remarks { get; set; }

        [Display(Name = "7.1.1 Sr No-7 Whether any deduction from wage is being made other than specified in section 20 (2)")]
        [Required(ErrorMessage = "Any_Deduction_Of_Wages is required..!")]
        public GeneralOptionTypeEnum Any_Deduction_Of_Wages { get; set; }

        [Display(Name = "7.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Any_Deduction_Of_Wages_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Any_Deduction_Of_Wages_ViolationExist { get; set; }

        [Display(Name = "7.1.3 Any_Deduction_Of_Wages_Remarks")]
        [Required(ErrorMessage = "Any_Deduction_Of_Wages_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Any_Deduction_Of_Wages_Remarks can have max 1000 chars..!")]
        public string Any_Deduction_Of_Wages_Remarks { get; set; }

        [Display(Name = "8.1.1 Sr No-8 Whether any fine imposed or deduction made on account of damage or lose to the employer caused by employee has been explained to him personally and also in writing.")]
        [Required(ErrorMessage = "Any_Fine_Imposed_For_Damage_Loss is required..!")]
        public GeneralOptionTypeEnum Any_Fine_Imposed_For_Damage_Loss { get; set; }

        [Display(Name = "8.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Any_Fine_Imposed_For_Damage_Loss_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Any_Fine_Imposed_For_Damage_Loss_ViolationExist { get; set; }

        [Display(Name = "8.1.3 Any_Fine_Imposed_For_Damage_Loss_Remarks")]
        [Required(ErrorMessage = "Any_Fine_Imposed_For_Damage_Loss_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Any_Fine_Imposed_For_Damage_Loss_Remarks can have max 1000 chars..!")]
        public string Any_Fine_Imposed_For_Damage_Loss_Remarks { get; set; }

        [Display(Name = "9.1.1 Sr No-9 Whether fine realized is being utilized in accordance with the directions of the government.")]
        [Required(ErrorMessage = "Is_Fine_Realized_Utilized_As_Per_Gov_Guidline is required..!")]
        public GeneralOptionTypeEnum Is_Fine_Realized_Utilized_As_Per_Gov_Guidline { get; set; }

        [Display(Name = "9.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Fine_Realized_Utilized_As_Per_Gov_Guidline_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Fine_Realized_Utilized_As_Per_Gov_Guidline_ViolationExist { get; set; }

        [Display(Name = "9.1.3 Is_Fine_Realized_Utilized_As_Per_Gov_Guidline_Remarks")]
        [Required(ErrorMessage = "Is_Fine_Realized_Utilized_As_Per_Gov_Guidline_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Fine_Realized_Utilized_As_Per_Gov_Guidline_Remarks can have max 1000 chars..!")]
        public string Is_Fine_Realized_Utilized_As_Per_Gov_Guidline_Remarks { get; set; }

        [Display(Name = "10.1.1 Sr No-10 Whether the employees are being allowed casual, sick leave, Festival and Gazetted Holidays (21 in a year).")]
        [Required(ErrorMessage = "Is_Leaves_Given_To_Employees is required..!")]
        public GeneralOptionTypeEnum Is_Leaves_Given_To_Employees { get; set; }

        [Display(Name = "10.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Leaves_Given_To_Employees_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Leaves_Given_To_Employees_ViolationExist { get; set; }

        [Display(Name = "10.1.3 Is_Leaves_Given_To_Employees_Remarks")]
        [Required(ErrorMessage = "Is_Leaves_Given_To_Employees_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Leaves_Given_To_Employees_Remarks can have max 1000 chars..!")]
        public string Is_Leaves_Given_To_Employees_Remarks { get; set; }

        [Display(Name = "11.1.1 Sr No-11 Cleanliness")]
        [Required(ErrorMessage = "Is_Cleanliness is required..!")]
        public GeneralOptionTypeEnum Is_Cleanliness { get; set; }


        [Display(Name = "11.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_Cleanliness_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_Cleanliness_ViolationExist { get; set; }

        [Display(Name = "11.1.3 Is_Cleanliness_Remarks")]
        [Required(ErrorMessage = "Is_Cleanliness_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_Cleanliness_Remarks can have max 1000 chars..!")]
        public string Is_Cleanliness_Remarks { get; set; }

        [Display(Name = "12.1.1 Sr No-12 Ventilation and Lighting")]
        [Required(ErrorMessage = "Is_There_Ventilation_And_Lighting is required..!")]
        public GeneralOptionTypeEnum Is_There_Ventilation_And_Lighting { get; set; }

        [Display(Name = "12.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_There_Ventilation_And_Lighting_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_There_Ventilation_And_Lighting_ViolationExist { get; set; }

        [Display(Name = "12.1.3 Is_There_Ventilation_And_Lighting_Remarks")]
        [Required(ErrorMessage = "Is_There_Ventilation_And_Lighting_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_There_Ventilation_And_Lighting_Remarks can have max 1000 chars..!")]
        public string Is_There_Ventilation_And_Lighting_Remarks { get; set; }

        [Display(Name = "13.1.1 Sr No-13 Drinking Water facility.")]
        [Required(ErrorMessage = "Is_There_Drinking_Water_Facility is required..!")]
        public GeneralOptionTypeEnum Is_There_Drinking_Water_Facility { get; set; }

        [Display(Name = "13.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Is_There_Drinking_Water_Facility_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Is_There_Drinking_Water_Facility_ViolationExist { get; set; }

        [Display(Name = "13.1.3 Is_There_Drinking_Water_Facility_Remarks")]
        [Required(ErrorMessage = "Is_There_Drinking_Water_Facility_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Is_There_Drinking_Water_Facility_Remarks can have max 1000 chars..!")]
        public string Is_There_Drinking_Water_Facility_Remarks { get; set; }

        [Display(Name = "14.1.1 Sr No-14 Precaution against Fire")]
        [Required(ErrorMessage = "Any_Precaution_Against_Fire is required..!")]
        public GeneralOptionTypeEnum Any_Precaution_Against_Fire { get; set; }

        [Display(Name = "14.1.2 Is Violation Found")]
        [Required(ErrorMessage = "Any_Precaution_Against_Fire_ViolationExist is required..!")]
        public InspectionViolationExistTypeEnum Any_Precaution_Against_Fire_ViolationExist { get; set; }

        [Display(Name = "14.1.3 Any_Precaution_Against_Fire_Remarks")]
        [Required(ErrorMessage = "Any_Precaution_Against_Fire_Remarks is required..!")]
        [StringLength(maximumLength: 1000, ErrorMessage = "Any_Precaution_Against_Fire_Remarks can have max 1000 chars..!")]
        public string Any_Precaution_Against_Fire_Remarks { get; set; }

        [Display(Name = "InspectionRefId")]
        [Required(ErrorMessage = "InspectionRefId is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }
    }


    public class InspectionUserMapping
    {
        [Key]
        public Int64 InspectionUserMappingId { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "LicenceNumber is required..!")]
        public string LicenceNumber { get; set; }

        [Required(ErrorMessage = "InspectionRefId is required..!")]
        public string InspectionRefId { get; set; }

    }
    public class InspectionComplianceLog
    {
        [Key]
        public Int64 InspectionComplianceLogId { get; set; }

        [Required(ErrorMessage = "Application action is required..!")]
        public int AppActionType { get; set; }

        [Required(ErrorMessage = "Sender user ref id is required..!")]
        [StringLength(450)]
        public string Sender_UserRefId { get; set; }

        [Required(ErrorMessage = "Sender profile ref id is required..!")]
        public Int64 Sender_ProfileRefId { get; set; }


        [Required(ErrorMessage = "Receiver user ref id is required..!")]
        [StringLength(450)]
        public string Receiver_UserRefId { get; set; }

        [Required(ErrorMessage = "Receiver profile ref id is required..!")]
        public Int64 Receiver_ProfileRefId { get; set; }


        [Required(ErrorMessage = "Action date is required..!")]
        public DateTime ActionDate { get; set; }

        [Required(ErrorMessage = "Action taken days count is required..!")]
        public Int64 ActionTakenDaysCount { get; set; }

        [Required(ErrorMessage = "Action taken hours count is required..!")]
        public Int64 ActionTakenHoursCount { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        //[StringLength(500, ErrorMessage = "The max length of remarks is 500 characters..!")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Sender role id is required..!")]
        [StringLength(450)]
        public string SenderRoleId { get; set; }

        [Required(ErrorMessage = "Receiver role id is required..!")]
        [StringLength(450)]
        public string ReceiverRoleId { get; set; }

        [Required]
        public bool IsDocumentUploaded { get; set; }

        [Required]
        public Int64 AppDocumentRefId { get; set; }

        [Required(ErrorMessage = "IP address is required..!")]
        public string IpAddress { get; set; }

        [Required(ErrorMessage = "Latitude is required..!")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required..!")]
        public string Longitude { get; set; }

        [Required(ErrorMessage = "InspectionType is required..!")]

        public int InspectionType { get; set; }

        #region ForeignKeyReferences

        [Required(ErrorMessage = "Inspection ref id is required..!")]
        [ForeignKey("Inspection_Master")]

        public Int64 InspectionRefId { get; set; }

        //[Required(ErrorMessage = "Application ref id is required..!")]
        //[ForeignKey("ApplicationAction")]
        //public Int64 AppActionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }

        #endregion ForeignKeyReferences
    }
    public class InspectionActionCode
    {
        [Required, Key]
        public int InspectionActionCodeId { get; set; }
        public int ActionCode { get; set; }
        public string ActionName { get; set; }
        public string ActionPublicName { get; set; }
    }
    public class InspectionRoleWiseAllowedActionCode
    {
        [Required, Key]
        public Int64 InspectionRoleWiseAllowedActionCodeId { get; set; }

        [Required(ErrorMessage = "RoleId is required..!")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Cuurent Action Code is required..!")]
        public Int64 CurrentActionCode { get; set; }

        [Required(ErrorMessage = "IsDocumentUploadOption is required..!")]
        public bool IsDocumentUploadOption { get; set; }

        [Required(ErrorMessage = "IsDocumentUploadedRequired is required..!")]
        public bool IsOptional { get; set; }

        [Required]
        [ForeignKey("Document")]
        public Int64? DocRefId { get; set; }
        public virtual Document Document { get; set; }

        [Required(ErrorMessage = "AllowedActionCode is required..!")]
        public Int64 AllowedActionCode { get; set; }

        [Required(ErrorMessage = "Inspection Type is required..!")]
        public InspectionTypeEnum InspectionType { get; set; }

        [NotMapped]
        public string ActionName { get; set; }

        [Required(ErrorMessage = "IsEnabled is required..!")]
        public bool IsEnabled { get; set; }
    }

    public class Inspection_ViolationComplianceReminder
    {
        [Key]
        public Int64 Inspection_ViolationComplianceReminderId { get; set; }

        [Required(ErrorMessage = "Time In Days is required..!")]
        public int TimeInDays { get; set; }

        [Required(ErrorMessage = "Start Date is required..!")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required..!")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Total Reminders To Be Send is required..!")]
        public int TotalRemindersToBeSend { get; set; }

        [Required(ErrorMessage = "Total Reminders Sent is required..!")]
        public int TotalRemindersSent { get; set; }

        [Required(ErrorMessage = "Reminder Status is required..!")]
        public int ReminderStatus { get; set; }

        [Required(ErrorMessage = "Inspection Type is required..!")]

        public int InspectionType { get; set; }

        #region ForeignKeyReferences

        [Required(ErrorMessage = "Inspection ref id is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }

        public virtual Inspection_Master Inspection_Master { get; set; }

        #endregion ForeignKeyReferences
    }
    public class InspectionDocument
    {
        [Key]
        [Display(Name = "AppDocId")]
        public Int64 AppDocId { get; set; }

        [Display(Name = "AttachmentName")]
        [StringLength(60, ErrorMessage = "Attachment name must be 7-60 characters long !", MinimumLength = 7)]
        public string AttachmentName { get; set; }

        [Display(Name = "Is Uploaded")]
        [Required]
        public bool IsUploaded { get; set; }

        [Required(ErrorMessage = "Uploaded Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Uploaded date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Uploadeddate { get; set; }

        public bool IsLocked { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "Inspection Type is required..!")]
        public int InspectionType { get; set; }

        [Required(ErrorMessage = "DocumentRefId is required..!")]
        [ForeignKey("Documents")]
        public Int64 DocumentRefId { get; set; }
        public virtual Document Document { get; set; }

        [Required(ErrorMessage = "Inspection Ref Id is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }

        public virtual ICollection<InspectionDocument_Log> InspectionDocument_Log { get; set; }
    }
    public class InspectionDocument_Log
    {
        [Key]
        [Display(Name = "DocumentLogId")]
        public Int64 DocumentLogId { get; set; }

        [Display(Name = "AttachmentName")]
        [StringLength(60, ErrorMessage = "Attachment name must be 7-60 characters long !", MinimumLength = 7)]
        public string AttachmentName { get; set; }

        [Display(Name = "Is Uploaded")]
        [Required]
        public bool IsUploaded { get; set; }

        [Required(ErrorMessage = "Uploaded Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Uploaded date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Uploadeddate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "DocumentId is required..!")]
        [ForeignKey("Documents")]
        public Int64 DocumentId { get; set; }
        public virtual Document Documents { get; set; }

        [Required(ErrorMessage = "Inspection Ref Id is required..!")]
        [ForeignKey("Inspection_Master")]
        public Int64 InspectionRefId { get; set; }
        public virtual Inspection_Master Inspection_Master { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("ApplicationDocument")]
        public Int64 AppDocId { get; set; }
        public virtual InspectionDocument InspectionDocument { get; set; }
    }
    public class Inspection_UserMapping_Logs
    {
        [Key]
        public Int64 InspectionUserMappingLogId { get; set; }

        [Required(ErrorMessage = "LicenceNumber is required..!")]
        public string LicenceNumber { get; set; }

        [Required(ErrorMessage = "OldUserRefId is required..!")]
        public string OldUserRefId { get; set; }

        [Required(ErrorMessage = "NewUserRefId is required..!")]
        public string NewUserRefId { get; set; }

        [Required(ErrorMessage = "TimeStemp is required..!")]
        public DateTime TimeStemp { get; set; }

    }

    public class Randomization_Initialization
    {
        [Key]
        public Int64 RandomizationInitializationId { get; set; }

        [Required(ErrorMessage = "Month is required..!")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required..!")]
        public int Year { get; set; }

        [Required(ErrorMessage = "RandomizationInitializationStatus is required..!")]
        public RandomizationInitilaztionProcessStatusTypeEnum RandomizationInitializationStatus { get; set; }

        [Required(ErrorMessage = "Process initiated by user id..!")]
        [StringLength(60)]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "Timestemp is required..!")]
        public DateTime Timestemp { get; set; }
    }

    public class OldInspectionMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "InspectionId is required..!")]
        public Int64 InspectionId { get; set; }

        [Required(ErrorMessage = "FactoryInspectionId is required..!")]
        public Int64 FactoryInspectionId { get; set; }

        [Required(ErrorMessage = "LabourInspectionId is required..!")]
        public Int64 LabourInspectionId { get; set; }

        [Required(ErrorMessage = "Month is required..!")]
        public Int64 Month { get; set; }

        [Required(ErrorMessage = "Year is required..!")]
        public Int64 Year { get; set; }
    }

    public class Inspection_AllotmentLogs
    {
        [Key]
        public Int64 Id { get; set; }   

        public string LicenceNumber { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string UserRefId { get; set; }

        public Int64 ProfileId { get; set; }   

        public string RoleId { get; set; }

        public DateTime TimeStamp { get; set; } 
    }

}

