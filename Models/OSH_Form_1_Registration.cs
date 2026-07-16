using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class OSH_Form_1_Registration
    {
        [Key]
        public Int64 RegistrationId { get; set; }

        //[Required(ErrorMessage = "Establishment name is required..!"), StringLength(500, ErrorMessage = "The max length of establishment name is 500 characters..!")]
        //public string EstablishmentName { get; set; }

        //[Required(ErrorMessage = "Email is required..!"), EmailAddress(ErrorMessage = "Invalid email address..!"), StringLength(100, ErrorMessage = "The max length of email is 100 characters..!")]
        //public string Email { get; set; }

        //[Required]
        //[RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        //[StringLength(10)]
        //public string MobileNo { get; set; }

        //[Required(ErrorMessage = "Establishment address is required..!")]
        //[StringLength(500, ErrorMessage = "The max length of establishment address is 500 characters..!")]
        //public string EstablishmentAddress { get; set; }

        //[Required(ErrorMessage = "Establishment Village or Town name is required..!")]
        //[StringLength(100, ErrorMessage = "The max length of Establishment Village or Town is 100 characters..!")]
        //public string EstablishmentVillageOrTown { get; set; }

        //[Required(ErrorMessage = "Tehsil is required..!")]
        //[ForeignKey("TehsilLgd")]
        //public Int64 TehsilRefId { get; set; }
        //public virtual TehsilLgd TehsilLgd { get; set; }

        //[Required(ErrorMessage = "District is required..!")]
        //[ForeignKey("DistrictLgd")]
        //public Int64 DistrictRefId { get; set; }
        //public virtual DistrictLgd DistrictLgd { get; set; }

        //[Required(ErrorMessage = "Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        //public string PinCode { get; set; }

        [Required(ErrorMessage = "PAN Number is required..!"), StringLength(15, ErrorMessage = "Invalid PAN_Number..!")]
        public string PanNumber { get; set; }

        [Required(ErrorMessage = "Establishment name is required..!")]
        [StringLength(500, ErrorMessage = "The max length of establishment name is 500 characters..!")]
        public string NameOnPan { get; set; }

        [Required(ErrorMessage = "Date of birth is required..!")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Is estb carrying any hazardous occupation is required..!")]
        public bool IsEstbCarryingAnyHazardousOccupation { get; set; }

        [Required(ErrorMessage = "OshEstablishmentType is required..!")]
        public OSH_EstablishmentTypeEnum OSH_EstablishmentType { get; set; }

        public Int64? EstablishmentOtherTypeId { get; set; }
        
        [Required(ErrorMessage = "Maximum no of workers to be employed on any day is required..!")]
        public int MaximumNoOfWorkersToBeEmployedOnAnyDay { get; set; }

        #region Other Details

        [Required(ErrorMessage = "Do you want voluntary coverage for EPFO is required..!")]
        public bool DoYouWantVoluntaryCoverageForEPFO { get; set; }

        [Required(ErrorMessage = "Do you want voluntary coverage for ESIC is required..!")]
        public bool DoYouWantVoluntaryCoverageForESIC { get; set; }

        [Required(ErrorMessage = "National Industrial Classification Code is required..!")]
        public string NationalIndustrialClassificationCode { get; set; }

        #endregion

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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

        [NotMapped]
        public OSH_Form_1_Registration_EmployeeDetail EmpDetailData { get; set; }

        [NotMapped]
        public OSH_Form_1_Registration_Factory FactoryDetailData { get; set; }

        #endregion

    }
    public class OSH_Form_1_Registration_Factory
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Manufacturing process is required..!"), StringLength(500, ErrorMessage = "The max length of manufacturing process is 500 characters..!")]
        public string ManufacturingProcess { get; set; }

        [Required(ErrorMessage = "Premise name is required..!"), StringLength(500, ErrorMessage = "The max length of premise name is 500 characters..!")]
        public string PremiseName { get; set; }

        [Required(ErrorMessage = "Sub locality is required..!"), StringLength(500, ErrorMessage = "The max length of sub locality is 500 characters..!")]
        public string SubLocality_OR_Street_OR_ColonyName { get; set; }

        [Required(ErrorMessage = "Locality is required..!"), StringLength(500, ErrorMessage = "The max length of locality is 500 characters..!")]
        public string Locality_OR_Landmark { get; set; }

        [Required(ErrorMessage = "village or town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of village or town is 100 characters..!")]
        public string VillageOrTown { get; set; }

        [Required(ErrorMessage = "State is Required..!")]
        public string State { get; set; }

        [Required(ErrorMessage = "Tehsil is required..!")]
        [ForeignKey("TehsilLgd")]
        public Int64 TehsilRefId { get; set; }
        public virtual TehsilLgd TehsilLgd { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }


        [Required(ErrorMessage = "Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Date of commencement is required..!")]
        public DateTime DateOfCommencement { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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
        public Int64 FactoryCircleRefId { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }

        [NotMapped]
        public OSH_Form_1_Registration_EmployeeDetail EmpDetailData { get; set; }

        #endregion

    }
    public class OSH_Form_1_Registration_BOCW
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Name of construction work is required..!"), StringLength(200, ErrorMessage = "The max length of name of construction work is 200 characters..!")]
        public string NameOfConstructionWork { get; set; }

        [Required(ErrorMessage = "Name of principal employer work is required..!"), StringLength(200, ErrorMessage = "The max length of principal employer is 200 characters..!")]
        public string NameOfPrincipalEmployer { get; set; }

        [Required(ErrorMessage = "Date of commencement of work is required..!")]
        public DateTime DateOfCommencementOfWork { get; set; }

        [Required(ErrorMessage = "Date of completion of work is required..!")]
        public DateTime DateOfCompletionOfWork { get; set; }

        [Required(ErrorMessage = "Approval details by local authority is required..!"), StringLength(200, ErrorMessage = "The max length of approval details by local authority is 200 characters..!")]
        public string ApprovalDetailsByLocalAuthority { get; set; }

        [Required(ErrorMessage = "Registration no of principal employer is required..!"), StringLength(50, ErrorMessage = "The max length of registration no of principal employer is 50 characters..!")]
        public string RegistrationNoOfPrincipalEmployer { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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

        [NotMapped]
        public OSH_Form_1_Registration_EmployeeDetail EmpDetailData { get; set; }

        #endregion
    }
    public class OSH_Form_1_Registration_EmployeeDetail
    {
        [Key]
        public Int64 Id { get; set; }

        // 1. Employees directly engaged
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Male employees must be 0 or greater.")]
        public int DirectEmployee_Male { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int DirectEmployee_Female { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int DirectEmployee_Others { get; set; }


        // 1(a) ISMW directly engaged
        [Range(0, int.MaxValue)]
        public int ISMWDirect_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int ISMWDirect_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int ISMWDirect_Others { get; set; }


        // 1(b) Casual Workers directly engaged
        [Range(0, int.MaxValue)]
        public int CasualWorker_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int CasualWorker_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int CasualWorker_Others { get; set; }


        // 1(c) Fixed Term Employment
        [Range(0, int.MaxValue)]
        public int FixedTerm_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int FixedTerm_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int FixedTerm_Others { get; set; }


        // 1(d) Building Workers
        [Range(0, int.MaxValue)]
        public int BuildingWorker_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int BuildingWorker_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int BuildingWorker_Others { get; set; }


        // 2. Contract Labour engaged
        [Range(0, int.MaxValue)]
        public int ContractLabour_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int ContractLabour_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int ContractLabour_Others { get; set; }


        // 2(a) ISMW engaged through contractor
        [Range(0, int.MaxValue)]
        public int ISMWContractLabour_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int ISMWContractLabour_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int ISMWContractLabour_Others { get; set; }

        // 2(b) Building Workers contractor
        [Range(0, int.MaxValue)]
        public int Contractor_BuildingWorker_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int Contractor_BuildingWorker_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int Contractor_BuildingWorker_Others { get; set; }


        // 3. Supervisor
        [Range(0, int.MaxValue)]
        public int Supervisor_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int Supervisor_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int Supervisor_Others { get; set; }


        // 4. Employees drawing wages <= 21000
        [Range(0, int.MaxValue)]
        public int EmployeeBelow21000_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int EmployeeBelow21000_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int EmployeeBelow21000_Others { get; set; }


        // 5. Contract Labour drawing wages <= 21000
        [Range(0, int.MaxValue)]
        public int ContractBelow21000_Male { get; set; }

        [Range(0, int.MaxValue)]
        public int ContractBelow21000_Female { get; set; }

        [Range(0, int.MaxValue)]
        public int ContractBelow21000_Others { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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
    public class OSH_Form_1_Registration_EstablishmentOtherType
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Establishment other type is required..!"), StringLength(500, ErrorMessage = "The max length of establishment type is 500 characters..!")]
        public string EstablishmentOtherType { get; set; }

        [Required(ErrorMessage = "IsActive is required..!")]
        public bool IsActive { get; set; }
    }
    public class OSH_Form_1_Registration_EPFO_ESIC_Detail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Date on which 10 or more person employed is required..!")]
        public DateTime DateOnWhich10OrMorePersonEmployed { get; set; }

        [Required(ErrorMessage = "Date on which 20 or more person employed is required..!")]
        public DateTime DateOnWhich20OrMorePersonEmployed { get; set; }

        [Required(ErrorMessage = "Employees voluntary registration date is required..!")]
        public DateTime EmployeesVoluntaryRegistrationDate { get; set; }

        [Required(ErrorMessage = "Employees voluntary registration date is required..!"), StringLength(50, ErrorMessage = "The max length is 50 characters..!")]
        public string DPIIT_StartupRegistrationNumber { get; set; }

        public string DPIIT_StartupRegistrationDate { get; set; }

        [Required(ErrorMessage = "Date of commencement is required..!")]
        public DateTime DateOfCommencement { get; set; }

        [Required(ErrorMessage = "Esic_NatureOfWork is required..!")]
        public string Esic_NatureOfWork { get; set; }

        [Required(ErrorMessage = "Esic_SubCategory_NatureOfWork is required..!")]
        public string Esic_SubCategory_NatureOfWork { get; set; }

        [Required(ErrorMessage = "Esic branch office is required..!")]
        public string Esic_BranchOffice { get; set; }

        [Required(ErrorMessage = "Esic inspection division is required..!")]
        public string Esic_InspectionDivision { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }


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
    public class OSH_Form_1_Registration_EmployerDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Employer type is required..!")]
        public string EmployerType { get; set; }

        [Required(ErrorMessage = "Employer name is required..!"), StringLength(100, ErrorMessage = "The max length of employer name is 100 characters..!")]
        public string EmployerName { get; set; }

        [Required(ErrorMessage = "Employer name is required..!"), StringLength(100, ErrorMessage = "The max length of employer name is 100 characters..!")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Father or husband name is required..!"), StringLength(100, ErrorMessage = "The max length of father or husband name is 100 characters..!")]
        public string FatherOrHusbandName { get; set; }

        [Required(ErrorMessage = "Email is required..!"), StringLength(100, ErrorMessage = "The max length of email is 100 characters..!")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Premise name is required..!"), StringLength(500, ErrorMessage = "The max length of premise name is 500 characters..!")]
        public string PremiseName { get; set; }

        [Required(ErrorMessage = "Sub locality is required..!"), StringLength(500, ErrorMessage = "The max length of sub locality is 500 characters..!")]
        public string SubLocality_OR_Street_OR_ColonyName { get; set; }

        [Required(ErrorMessage = "Locality is required..!"), StringLength(500, ErrorMessage = "The max length of locality is 500 characters..!")]
        public string Locality_OR_Landmark { get; set; }

        [Required(ErrorMessage = "village or town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of village or town is 100 characters..!")]
        public string VillageOrTown { get; set; }

        [Required(ErrorMessage = "State is Required..!")]
        public string State { get; set; }

        [Required(ErrorMessage = "Tehsil is required..!")]
        [ForeignKey("TehsilLgd")]
        public Int64 TehsilRefId { get; set; }
        public virtual TehsilLgd TehsilLgd { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PinCode { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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
        public Int64 FactoryCircleRefId { get; set; }

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
    public class OSH_Form_1_Registration_PrincipalEmployerDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Employer name is required..!"), StringLength(100, ErrorMessage = "The max length of employer name is 100 characters..!")]
        public string PrincipalEmployerName { get; set; }

        [Required(ErrorMessage = "Employer name is required..!"), StringLength(100, ErrorMessage = "The max length of employer name is 100 characters..!")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Department is required..!"), StringLength(100, ErrorMessage = "The max length of department is 100 characters..!")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Email is required..!"), StringLength(100, ErrorMessage = "The max length of email is 100 characters..!")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Premise name is required..!"), StringLength(500, ErrorMessage = "The max length of premise name is 500 characters..!")]
        public string PremiseName { get; set; }

        [Required(ErrorMessage = "Sub locality is required..!"), StringLength(500, ErrorMessage = "The max length of sub locality is 500 characters..!")]
        public string SubLocality_OR_Street_OR_ColonyName { get; set; }

        [Required(ErrorMessage = "Locality is required..!"), StringLength(500, ErrorMessage = "The max length of locality is 500 characters..!")]
        public string Locality_OR_Landmark { get; set; }

        [Required(ErrorMessage = "village or town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of village or town is 100 characters..!")]
        public string VillageOrTown { get; set; }

        [Required(ErrorMessage = "State is Required..!")]
        public string State { get; set; }

        [Required(ErrorMessage = "Tehsil is required..!")]
        [ForeignKey("TehsilLgd")]
        public Int64 TehsilRefId { get; set; }
        public virtual TehsilLgd TehsilLgd { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PinCode { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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
        public Int64 FactoryCircleRefId { get; set; }

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
    public class OSH_Form_1_Registration_ContractorDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Is establishment engaged contractor is required..!")]
        public bool IsEstablishmentEngagedContractor { get; set; }

        [Required(ErrorMessage = "Principal employer name is required..!")]
        public string PrincipalEmployerName { get; set; }

        [Required(ErrorMessage = "Contractor name is required..!"), StringLength(100, ErrorMessage = "The max length of contractor name is 100 characters..!")]
        public string ContractorName { get; set; }

        [Required(ErrorMessage = "Name and location of work is required..!")]
        public string NameAndLocationOfWork { get; set; }

        [Required(ErrorMessage = "Premise name is required..!"), StringLength(500, ErrorMessage = "The max length of premise name is 500 characters..!")]
        public string PremiseName { get; set; }

        [Required(ErrorMessage = "Sub locality is required..!"), StringLength(500, ErrorMessage = "The max length of sub locality is 500 characters..!")]
        public string SubLocality_OR_Street_OR_ColonyName { get; set; }

        [Required(ErrorMessage = "Locality is required..!"), StringLength(500, ErrorMessage = "The max length of locality is 500 characters..!")]
        public string Locality_OR_Landmark { get; set; }

        [Required(ErrorMessage = "village or town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of village or town is 100 characters..!")]
        public string VillageOrTown { get; set; }

        [Required(ErrorMessage = "State is Required..!")]
        public string State { get; set; }

        [Required(ErrorMessage = "Tehsil is required..!")]
        [ForeignKey("TehsilLgd")]
        public Int64 TehsilRefId { get; set; }
        public virtual TehsilLgd TehsilLgd { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Email is required..!"), StringLength(100, ErrorMessage = "The max length of email is 100 characters..!")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "PAN Number is required..!"), StringLength(15, ErrorMessage = "Invalid PAN_Number..!")]
        public string PanNumber { get; set; }

        [Required(ErrorMessage = "Establishment name is required..!")]
        [StringLength(500, ErrorMessage = "The max length of establishment name is 500 characters..!")]
        public string NameOnPan { get; set; }

        [Required(ErrorMessage = "Date of birth is required..!")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Max no of contract labour to be employed is required..!")]
        public int MaxNoOfContractLabourToBeEmployed { get; set; }

        [Required(ErrorMessage = "Date of commencement of work is required..!")]
        public DateTime DateOfCommencementOfWork { get; set; }

        [Required(ErrorMessage = "Date of completion of work is required..!")]
        public DateTime DateOfCompletionOfWork { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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
        public Int64 FactoryCircleRefId { get; set; }

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
    public class OSH_Form_1_Registration_MotorTransportDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "UBIN is required..!")]
        public int UBIN { get; set; }

        [Required(ErrorMessage = "Motor transport name undertaking is required..!"), StringLength(100, ErrorMessage = "The max length of motor transport name undertaking is 100 characters..!")]
        public string MotorTransportNameUndertaking { get; set; }

        [Required(ErrorMessage = "Motor transport service name is required..!"), StringLength(100, ErrorMessage = "The max length of Motor transport service name is 100 characters..!")]
        public string MotorTransportServiceName { get; set; }

        [Required(ErrorMessage = "Mileage is required..!")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "No of vehicle is required..!")]
        public int NoOfVehicle { get; set; }

        [Required(ErrorMessage = "Maximum No of employed is required..!")]
        public int MaxNoOfEmployedOnAnyDay { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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
}
