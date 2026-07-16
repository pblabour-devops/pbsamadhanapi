using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class OSH_Form_21_ContractLabour_General_Detail
    {
        [Key]
        public Int64 ContractLabourId { get; set; }

        #region Registration Details

        [Required(ErrorMessage = "registration no is required..!")]
        [StringLength(100, ErrorMessage = "The max length of registration no is 100 characters..!")]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "Registration date is required..!")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Acknowledgement no is required..!")]
        public string AcknowledgementNumber { get; set; }

        [Required(ErrorMessage = "Application date is required..!")]
        public DateTime ApplicationDate { get; set; }

        #endregion

        #region Particulars of the Contractor

        [Required(ErrorMessage = "Contractor name is required..!"), StringLength(200, ErrorMessage = "The max length of contractor name is 200 characters..!")]
        public string ContractorName { get; set; }

        [Required(ErrorMessage = "Contractor address is required..!"), StringLength(500, ErrorMessage = "The max length of Contractor address is 500 characters..!")]
        public string ContractorAddress { get; set; }

        [Required(ErrorMessage = "Establishment constitution type is required..!")]
        public EstablishmentConstitutionTypeEnum EstablishmentConstitutionType { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email is required..!"), StringLength(100, ErrorMessage = "The max length of email is 100 characters..!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contractor Father name is required..!")]
        public string ContractorFatherName { get; set; }

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

        #endregion
    }

    public class OSH_Form_21_ContractLabour_Employee_Detail
    {
        [Key]
        public Int64 EmployeeDetailId { get; set; }

        #region Particulars of the Contract Labour is employed/proposed to be employed

        [Required(ErrorMessage = "Site location and address is required..!"), StringLength(500, ErrorMessage = "The max length of site location and address is 500 characters..!")]
        public string SiteLocationAndAddress { get; set; }

        [Required(ErrorMessage = "Nature Of Work is required..!"), StringLength(200, ErrorMessage = "The max length of nature Of Work is 200 characters..!")]
        public string NatureOfWork { get; set; }

        [Required(ErrorMessage = "National Industrial Classification Code is required..!")]
        public string NationalIndustrialClassificationCode { get; set; }

        [Required(ErrorMessage = "Date of commencement of work is required..!")]
        public DateTime DateOfCommencementOfWork { get; set; }

        [Required(ErrorMessage = "Date of completion of work is required..!")]
        public DateTime DateOfCompletionOfWork { get; set; }

        [Required(ErrorMessage = "Establishment name is required..!"), StringLength(500, ErrorMessage = "The max length of establishment name is 500 characters..!")]
        public string EstablishmentName { get; set; }

        [Required(ErrorMessage = "Name of principal Employer is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of Name of principal Employer is 100 characters..!")]
        public string PrincipalEmployer_Name { get; set; }

        [Required(ErrorMessage = "Principal Employer address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Principal Employer address is 500 characters..!")]
        public string PrincipalEmployer_Address { get; set; }

        [Required(ErrorMessage = "Name of site incharge is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of Name of site incharge is 100 characters..!")]
        public string SiteIncharge_Name { get; set; }

        [Required(ErrorMessage = "Site incharge address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of site incharge address is 500 characters..!")]
        public string SiteIncharge_Address { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        [StringLength(10)]
        public string SiteIncharge_MobileNo { get; set; }

        [Required(ErrorMessage = "Site incharge email is required..!"), StringLength(100, ErrorMessage = "The max length of site incharge email is 100 characters..!")]
        public string SiteIncharge_Email { get; set; }

        [Required(ErrorMessage = "License fee amount is required..!")]
        public Decimal LicenseFeeAmount { get; set; }

        [Required(ErrorMessage = "Transaction id is required..!"), StringLength(100, ErrorMessage = "The max length of Transaction id is 100 characters..!")]
        public string TransactionId { get; set; }

        [Required(ErrorMessage = "Bank guarantee detail is required..!")]
        public string BankGuaranteeDetail { get; set; }

        [Required(ErrorMessage = "Bank guarantee date is required..!")]
        public string BankGuaranteeDate { get; set; }
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

        #endregion
    }

    public class OSH_Form_21_ContractLabour_Establishment_Detail
    {
        [Key]
        public Int64 EstablishmentDetailId { get; set; }

        [Required(ErrorMessage = "Establishment type is Required..!")]
        public EstablishmentTypeEnum EstablishmentType { get; set; }

        [Required(ErrorMessage = "Establishment name is required..!"), StringLength(500, ErrorMessage = "The max length of establishment name is 500 characters..!")]
        public string EstablishmentName { get; set; }

        [Required(ErrorMessage = "Establishment address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of establishment address is 500 characters..!")]
        public string EstablishmentAddress { get; set; }

        [Required(ErrorMessage = "Max no contract labour proposed to be supplied is required..!")]
        public int MaxNoContractLabourProposedToBeSupplied { get; set; }

        [Required(ErrorMessage = "Start date is required..!")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required..!")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Nature Of Work is required..!"), StringLength(200, ErrorMessage = "The max length of nature Of Work is 200 characters..!")]
        public string NatureOfWork { get; set; }

        [Required(ErrorMessage = "National Industrial Classification Code is required..!")]
        public string NationalIndustrialClassificationCode { get; set; }

        [Required(ErrorMessage = "Name of principal Employer is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of Name of principal Employer is 100 characters..!")]
        public string PrincipalEmployer_Name { get; set; }

        [Required(ErrorMessage = "Principal Employer address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Principal Employer address is 500 characters..!")]
        public string PrincipalEmployer_Address { get; set; }

        [Required(ErrorMessage = "Authorized representative name is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of authorized representative name is 100 characters..!")]
        public string AuthorizedRepresentative_Name { get; set; }

        [Required(ErrorMessage = "Remarks is required..!"), StringLength(500, ErrorMessage = "The max length of remarks is 500 characters..!")]
        public string Remarks { get; set; }

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

    public class OSH_Form_21_ContractLabour_MigrantWorker
    {
        [Key]
        public Int64 MigrantworkerId { get; set; }

        [Required(ErrorMessage = "Name of worker is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of Name of contractor is 100 characters..!")]
        public string Worker_Name { get; set; }

        [Required(ErrorMessage = "Father name of contractor is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of father name of contractor is 100 characters..!")]
        public string Worker_Father_Husband_Name { get; set; }

        [Required(ErrorMessage = "Worker address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of worker address is 500 characters..!")]
        public string Worker_Permanent_Address { get; set; }

        [Required(ErrorMessage = "Worker Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of worker Village or Town is 100 characters..!")]
        public string Worker_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Worker Tehsil is required..!")]
        public string Worker_Tehsil { get; set; }

        [Required(ErrorMessage = "Worker District is required..!")]
        public string Worker_District { get; set; }

        [Required(ErrorMessage = "Worker_State is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of state is 100 characters..!")]
        public string Worker_State { get; set; }

        [Required(ErrorMessage = "Name of contractor is Required..!")]
        [StringLength(12, ErrorMessage = "Invalid worker aadhar number..!")]
        public string Worker_Aadhar_Number { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        [StringLength(10)]
        public string Worker_Mobile_Number { get; set; }

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