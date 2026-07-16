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
    public class OSH_Form_30_CommonLicense_Establishment
    {
        [Key]
        public Int64 CommonLicenseEstablishmentId { get; set; }

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

        #region Part-I : Particular of Establishment(Factory/Industrial premises for beedi cigar) for which license required

        [Required(ErrorMessage = "Establishment name is required..!"), StringLength(500, ErrorMessage = "The max length of establishment name is 500 characters..!")]
        public string EstablishmentName { get; set; }

        [Required(ErrorMessage = "Establishment address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of establishment address is 500 characters..!")]
        public string EstablishmentAddress { get; set; }

        [Required(ErrorMessage = "Office address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of office address is 500 characters..!")]
        public string OfficeAddress { get; set; }

        [Required(ErrorMessage = "Office email is required..!"), StringLength(100, ErrorMessage = "The max length of office email is 100 characters..!")]
        public string OfficeEmail { get; set; }

        [Required(ErrorMessage = "Other correspondence address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of other correspondence address is 500 characters..!")]
        public string OtherCorrespondenceAddress { get; set; }

        [Required(ErrorMessage = "Other correspondence email is required..!"), StringLength(100, ErrorMessage = "The max length of other correspondence email is 100 characters..!")]
        public string OtherCorrespondenceEmail { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        [StringLength(10)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Establishment location is required..!")]
        [StringLength(500, ErrorMessage = "The max length of establishment location is 500 characters..!")]
        public string EstablishmentLocation { get; set; }

        #endregion

        #region Part-II : Details of Employer/Occupier

        [Required(ErrorMessage = "Employer or Occupier full name is required..!"), StringLength(200, ErrorMessage = "The max length of Employer Occupier full name is 200 characters..!")]
        public string Employer_Or_Occupier_FullName { get; set; }

        [Required(ErrorMessage = "Employer or Occupier address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Employer or Occupier address is 500 characters..!")]
        public string Employer_Or_Occupier_Address { get; set; }

        [Required(ErrorMessage = "Employer or Occupier email is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Employer or Occupier email is 100 characters..!")]
        public string Employer_Or_Occupier_Email { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number.")]
        [StringLength(10)]
        public string Employer_Or_Occupier_MobileNo { get; set; }

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
    public class OSH_Form_30_CommonLicense_Factory
    {
        [Key]
        public Int64 CommonLicenseFactoryId { get; set; }

        [Required(ErrorMessage = "Factory name is required..!"), StringLength(500, ErrorMessage = "The max length of factory name is 500 characters..!")]
        public string FactoryName { get; set; }

        [Required(ErrorMessage = "Factory address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of factory address is 500 characters..!")]
        public string FactoryAddress { get; set; }

        [Required(ErrorMessage = "Manufacturing process is required..!"), StringLength(500, ErrorMessage = "The max length of manufacturing process is 500 characters..!")]
        public string ManufacturingProcess { get; set; }

        [Required(ErrorMessage = "Factory Applicability date is required..!")]
        public DateTime ApplicabilityDate { get; set; }

        [Required(ErrorMessage = "Max power to be used is required..!")]
        public int MaxPowerToBeUsed { get; set; }

        [Required(ErrorMessage = "Total no workers proposed to be employed is required..!")]
        public int TotalNoWorkersProposedToBeEmployed { get; set; }

        #region Manager Details
        [Required(ErrorMessage = "Manager Name is required..!")]
        [StringLength(200, ErrorMessage = "FieldName max size is 200")]
        public string ManagerName { get; set; }

        [Required(ErrorMessage = "Manager Address is required..!")]
        [StringLength(500, ErrorMessage = "FieldName max size is 500")]
        public string ManagerAddress { get; set; }

        [Required(ErrorMessage = "Manager Email is required..!")]
        public string ManagerEmail { get; set; }

        [Required(ErrorMessage = "Manager Mobile is required..!")]
        [StringLength(10, ErrorMessage = "FieldName max size is 10")]
        public string ManagerMobile { get; set; }
        #endregion

        [Required(ErrorMessage = "Approved building plan detail is required..!"), StringLength(200, ErrorMessage = "The max length of Approved building plan detail is 200 characters..!")]
        public string ApprovedBuildingPlanDetail { get; set; }

        [Required(ErrorMessage = "Submitted stability detail is required..!"), StringLength(200, ErrorMessage = "The max length of Submitted stability detail is 200 characters..!")]
        public string SubmittedStabilityDetail { get; set; }

        [Required(ErrorMessage = "Disposal of trade waste is required..!"), StringLength(200, ErrorMessage = "The max length of Disposal of trade waste is 200 characters..!")]
        public string DisposalOfTradeWaste { get; set; }

        [Required(ErrorMessage = "Fee amount is required..!")]
        public Decimal FeeAmount { get; set; }

        [Required(ErrorMessage = "Fee transaction id is required..!"), StringLength(100, ErrorMessage = "The max length of fee transaction id is 100 characters..!")]
        public string FeeTransactionId { get; set; }

        [Required(ErrorMessage = "Factory type is required..!")]
        public FactoryTypeEnum FactoryType { get; set; }

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
    public class OSH_Form_30_CommonLicense_ContractLabour
    {
        [Key]
        public Int64 CommonLicenseContractorId { get; set; }

        [Required(ErrorMessage = "Contractor name is required..!"), StringLength(200, ErrorMessage = "The max length of contractor name is 200 characters..!")]
        public string ContractorName { get; set; }

        [Required(ErrorMessage = "Contractor address is required..!"),StringLength(500, ErrorMessage = "The max length of Contractor address is 500 characters..!")]
        public string ContractorAddress { get; set; }

        [Required(ErrorMessage = "Nature Of Work is required..!"), StringLength(200, ErrorMessage = "The max length of nature Of Work is 200 characters..!")]
        public string NatureOfWork { get; set; }

        [Required(ErrorMessage = "Maximum no of contract labour to be employed is required..!")]
        public int MaximumNoOfContractLabourToBeEmployed { get; set; }

        [Required(ErrorMessage = "National Industrial Classification Code is required..!")]
        public string NationalIndustrialClassificationCode { get; set; }

        [Required(ErrorMessage = "Date of commencement of work is required..!")]
        public DateTime DateOfCommencementOfWork { get; set; }

        [Required(ErrorMessage = "Date of completion of work is required..!")]
        public DateTime DateOfCompletionOfWork { get; set; }

        [Required(ErrorMessage = "No of inter state migrants is required..!")]
        public int NoOfInterStateMigrants { get; set; }

        [Required(ErrorMessage = "Fee amount is required..!")]
        public Decimal LicenseFeeAmount { get; set; }

        [Required(ErrorMessage = "Transaction id is required..!"), StringLength(100, ErrorMessage = "The max length of Transaction id is 100 characters..!")]
        public string TransactionId { get; set; }

        [Required(ErrorMessage = "Bank guarantee detail is required..!")]
        public string BankGuaranteeDetail { get; set; }

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
