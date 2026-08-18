using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbsamadhannetcoreapi.Models
{
    #region ComplaintsCategory
    public class ComplaintsCategory
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Complaint Title"), StringLength(500, ErrorMessage = "Complaint Title is required..!")]
        public string ComplaintTitle { get; set; }
        public string Info { get; set; }

        public bool HasInfo { get; set; }

        [Required(ErrorMessage = "Complaint Category Type is required..!")]
        public ComplaintCategoryTypeEnum ComplaintCategoryType { get; set; }

        public virtual ICollection<AppComplaintTypeMapping> AppComplaintTypeMappings { get; set; }

    }
    #endregion

    #region APP COMPLAINT MAPPING
    public class AppComplaintTypeMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Complaint ref id is required..!")]
        [ForeignKey("ComplaintsCategory")]
        public Int64 ComplaintsCategoryRefId { get; set; }
        public virtual ComplaintsCategory ComplaintsCategory { get; set; }

    }
    #endregion

    #region Complaintent Type Complaint Type Mapping
    public class ComplainantTypeComplaintTypeMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Complainant Type is required..!")]
        public ComplainantTypeEnum ComplainantType { get; set; }

        public int ComplaintType { get; set; }
    }
    #endregion
    #region WORKER DETAILS
    public class WorkerDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Name is required..!")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is required..!")]
        [StringLength(20, ErrorMessage = "Gender cannot exceed 20 characters.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Designation is required..!")]
        [StringLength(200, ErrorMessage = "Designation cannot exceed 200 characters.")]
        public string Designation { get; set; }

        [StringLength(50, ErrorMessage = "Marital Status cannot exceed 50 characters.")]
        public string MaritalStatus { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, ErrorMessage = "Mobile Number cannot exceed 10 characters.")]
        public string MobileNumber { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid Email Address.")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Permanent Address is required..!")]
        [StringLength(500, ErrorMessage = "Permanent Address cannot exceed 500 characters.")]
        public string PermanentAddress { get; set; }

        [Required(ErrorMessage = "Permanent Country is required..!")]
        [StringLength(100, ErrorMessage = "Permanent Country cannot exceed 100 characters.")]
        public string PermanentCountry { get; set; }

        [Required(ErrorMessage = "Permanent State is required..!")]
        [StringLength(100, ErrorMessage = "Permanent State cannot exceed 100 characters.")]
        public string PermanentState { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey(nameof(PermanentDistrictLgd))]
        public Int64 PermanentDistrictRefId { get; set; }
        public virtual DistrictLgd PermanentDistrictLgd { get; set; }

        [Required(ErrorMessage = "Permanent Pincode is required..!")]
        [StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PermanentPincode { get; set; }

        [Required(ErrorMessage = "Please specify whether the correspondence address is same as the permanent address.")]
        public bool SameAsAbove { get; set; }

        [StringLength(500, ErrorMessage = "Correspondence Address cannot exceed 500 characters.")]
        public string CorrespondenceAddress { get; set; }

        [Required(ErrorMessage = "Correspondence Country is required..!")]
        [StringLength(100, ErrorMessage = "Correspondence Country cannot exceed 100 characters.")]
        public string CorrespondenceCountry { get; set; }

        [Required(ErrorMessage = "Correspondence State is required..!")]
        [StringLength(100, ErrorMessage = "Correspondence State cannot exceed 100 characters.")]
        public string CorrespondenceState { get; set; }

        [Required(ErrorMessage = "Correspondence District is required..!")]
        [ForeignKey(nameof(CorrespondenceDistrictLgd))]
        public Int64 CorrespondenceDistrictRefId { get; set; }
        public virtual DistrictLgd CorrespondenceDistrictLgd { get; set; }

        [Required(ErrorMessage = "Correspondence Pincode is required..!")]
        [StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string CorrespondencePincode { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        #region Not Mapped Column For application


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }


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
    #endregion

    #region COMPLAINT EMPLOYER DETAIL
    public class Complaint_EmployerORContractorDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Please specify the Establishment Type.")]
        public bool IsEstablishmentCentralGovernment { get; set; }

        [Required(ErrorMessage = "Establishment Type is required..!")]
        public SamadhaanEstablishmentTypeEnum EstablishmentType { get; set; }

        [Required(ErrorMessage = "Please specify whether you are engaged through contractor.")]
        public bool IsEngagedThroughContractor { get; set; }

        [Required(ErrorMessage = "Employer Name and Designation is required..!")]
        [StringLength(300, ErrorMessage = "Employer Name and Designation cannot exceed 300 characters.")]
        public string EmployerORContractorNameAndDesignation { get; set; }

        [Required(ErrorMessage = "Employer Address is required..!")]
        [StringLength(500, ErrorMessage = "Employer Address cannot exceed 500 characters.")]
        public string EmployerORContractorAddress { get; set; }

        [Required(ErrorMessage = "State is required..!")]
        public string State { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "PIN Code is required..!")]
        [StringLength(10, ErrorMessage = "PIN Code cannot exceed 10 characters.")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, ErrorMessage = "Mobile Number cannot exceed 10 digits.")]
        public string MobileNumber { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid Email Address.")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        public string Email { get; set; }


        #region Not Mapped Column For application


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region COMPLAINT WORKPLACE DETAIL
    public class Complaint_WorkplaceDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Workplace Address is required..!")]
        [StringLength(500, ErrorMessage = "Workplace Address cannot exceed 500 characters.")]
        public string WorkplaceAddress { get; set; }

        [Required(ErrorMessage = "State is required..!")]
        public string State { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "PIN Code is required..!")]
        [StringLength(10, ErrorMessage = "PIN Code cannot exceed 10 characters.")]
        public string PinCode { get; set; }

        #region NotMapped


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region COMPLAINT ESTABLISHMENT DETAIL
    public class Complaint_EstablishmentDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Name of Establishment is required..!")]
        [StringLength(300, ErrorMessage = "Name of Establishment cannot exceed 300 characters.")]
        public string EstablishmentName { get; set; }

        [Required(ErrorMessage = "Address of Establishment is required..!")]
        [StringLength(500, ErrorMessage = "Address of Establishment cannot exceed 500 characters.")]
        public string EstablishmentAddress { get; set; }

        [Required(ErrorMessage = "State is required..!")]
        public string State { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }


        [Required(ErrorMessage = "PIN Code is required..!")]
        [StringLength(10, ErrorMessage = "PIN Code cannot exceed 10 characters.")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, ErrorMessage = "Mobile Number must be 10 digits.")]
        public string MobileNumber { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid Email Address.")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "Nature of work cannot exceed 500 characters.")]
        public string NatureOfWorkPerformed { get; set; }

        [Required(ErrorMessage = "Please specify whether you are still working for the same employer/contractor.")]
        public bool IsStillWorking { get; set; }

        [Required(ErrorMessage = "Worker category type is required..!")]
        public WorkerCategoryTypeEnum WorkerCategoryType { get; set; }

        [Required(ErrorMessage = "Date of start of employment is required..!")]
        public DateTime EmploymentStartDate { get; set; }

        [Required(ErrorMessage = "Date of end of employment is required..!")]
        public DateTime EmploymentEndDate { get; set; }

        [Required(ErrorMessage = "Wage Period Type is required..!")]
        public WagePeriodtypeEnum WagePeriodType { get; set; }

        [Required(ErrorMessage = "Rate of wages is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal WageRate { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region COMPLAINT MINIMUM WAGES
    public class Complaint_MinimumWageClaimDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required..!")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Total relief sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(2000)]
        public string ClaimDetails { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region COMPLAINT WEEKLY DAY OF REST CLAIM
    public class Complaint_WeeklyDayOfRestClaim
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required..!")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Total relief sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(2000)]
        public string ClaimDetails { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region COMPLAINT OVERTIME CLAIM
    public class Complaint_OvertimeClaim
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required..!")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Total relief sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(2000)]
        public string ClaimDetails { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region WAGES NOT PAID AT ALL
    public class Complaint_WagesNotPaidClaim
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required..!")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Total relief sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(2000)]
        public string ClaimDetails { get; set; }

        #region NotMapped


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }

        #endregion
    }

    #endregion

    #region COMPLAINT UNAUTHORISED DEDUCTION CLAIM
    public class Complaint_UnauthorizedDeductionClaim
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required..!")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Total relief sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(2000)]
        public string ReasonForUnauthorizedDeduction { get; set; }

        #region NotMapped       

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

    #endregion

    #region COMPLAINT BONUS CLAIM
    public class Complaint_BonusClaim
    {


    }
    #endregion

    #region COMPLAINT GRATUITY CLAIM

    public class Complaint_GratuityClaim
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        #region Claim Information

        [Required(ErrorMessage = "Basis of claim type is required..!")]
        public GratuityClaimBasisTypeEnum BasisOfClaimType { get; set; }

        [Required(ErrorMessage = "Date of start of employment is required..!")]
        public DateTime EmploymentStartDate { get; set; }

        [Required(ErrorMessage = "Date of end of employment is required..!")]
        public DateTime EmploymentEndDate { get; set; }

        [Required(ErrorMessage = "Years of continuous service is required..!")]
        public int YearsOfContinuousService { get; set; }

        [Required(ErrorMessage = "Please specify whether application was made to employer.")]
        public bool IsApplicationMadeToEmployer { get; set; }

        [StringLength(2000, ErrorMessage = "Dispute details cannot exceed 2000 characters.")]
        public string DisputeDetails { get; set; }

        //[StringLength(500)]
        //public string AdditionalDocuments { get; set; }

        #endregion

        #region Annexure

        [Required(ErrorMessage = "Applicant name and address is required..!")]
        [StringLength(500, ErrorMessage = "Applicant name and address cannot exceed 500 characters.")]
        public string ApplicantNameAndAddress { get; set; }

        [Required(ErrorMessage = "Claim basis description is required..!")]
        [StringLength(500, ErrorMessage = "Claim basis description cannot exceed 500 characters.")]
        public string ClaimBasisDescription { get; set; }

        [Required(ErrorMessage = "Employee name and address is required..!")]
        [StringLength(500, ErrorMessage = "Employee name and address cannot exceed 500 characters.")]
        public string EmployeeNameAndAddress { get; set; }

        [Required(ErrorMessage = "Marital status is required..!")]
        public MaritalStatusTypeEnum MaritalStatusType { get; set; }

        [Required(ErrorMessage = "Employer name and address is required..!")]
        [StringLength(500, ErrorMessage = "Employer name and address cannot exceed 500 characters.")]
        public string EmployerNameAndAddress { get; set; }

        [StringLength(300, ErrorMessage = "Department cannot exceed 300 characters.")]
        public string Department { get; set; }

        [StringLength(300, ErrorMessage = "Employee post cannot exceed 300 characters.")]
        public string EmployeePost { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        [StringLength(500, ErrorMessage = "Termination reason cannot exceed 500 characters.")]
        public string TerminationReason { get; set; }

        public string TotalServicePeriod { get; set; }

        [Required(ErrorMessage = "Last drawn wages is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDrawnWages { get; set; }

        [StringLength(200, ErrorMessage = "Nomination number cannot exceed 200 characters.")]
        public string NominationNumber { get; set; }

        public DateTime? NominationRecordingDate { get; set; }

        //[StringLength(500, ErrorMessage = "Legal heir evidence document cannot exceed 500 characters.")]
        //public string LegalHeirEvidenceDocument { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalGratuityPayable { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? GratuityPercentagePayable { get; set; }

        [Required(ErrorMessage = "Gratuity amount claimed is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GratuityAmountClaimed { get; set; }

        [Required(ErrorMessage = "Claim date is required..!")]
        public DateTime ClaimDate { get; set; }

        [Required(ErrorMessage = "Place is required..!")]
        [StringLength(300, ErrorMessage = "Place cannot exceed 300 characters.")]
        public string Place { get; set; }

        #endregion

        #region NotMapped


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    #endregion

    #region COMPLAINT MATERNITY BENEFIT (MB) COMPLAINT

    public class Complaint_MaternityBenefitComplaint
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Please specify whether you have been discharged or dismissed/conditions of services have been changed on account of absence from work.")]
        public bool IsDischargedOrDismissedDueToAbsence { get; set; }

        [Required(ErrorMessage = "Maternity discharge type is required..!")]
        public MaternityDischargeTypeEnum? MaternityDischargeType { get; set; }

        [Required(ErrorMessage = "Maternity Benefit amount due is required..!")]
        [Range(0, double.MaxValue, ErrorMessage = "Maternity Benefit amount cannot be negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaternityBenefitAmountDue { get; set; }

        [Required(ErrorMessage = "Medical Bonus (Maternity) amount due is required..!")]
        [Range(0, double.MaxValue, ErrorMessage = "Medical Bonus amount cannot be negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MedicalBonusMaternityAmountDue { get; set; }

        [Required(ErrorMessage = "Wages for Maternity Leave amount due is required..!")]
        [Range(0, double.MaxValue, ErrorMessage = "Wages for Maternity Leave amount cannot be negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal WagesForMaternityLeaveAmountDue { get; set; }

        #region Not Mapped Column For application

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    #endregion

    #region COMPLAINT CLAIM UNDER CODE ON WAGES

    public class Complaint_Claim_CodeOnWage
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Allowance Type required..!")]
        public AllowanceTypeEnum AllowanceType { get; set; }

        public PlaceOfWorkTypeEnum? PlaceOfWorkTypeA { get; set; }

        public PlaceOfWorkTypeEnum? PlaceOfWorkTypeB { get; set; }

        public string PlaceofWorkNameC { get; set; }

        #region Not Mapped Column For application


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    #endregion

    #region COMPLAINT MINIMUM WAGES NOT PAID

    public class Complaint_MinimumWage
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Total Relief Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(200, ErrorMessage = "The max length of claim details is 200 characters..!")]
        public string DetailAboutTheClaim { get; set; }

        #region Not Mapped Column For application

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_MinimumWagesPeriodAmt
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        #region Not Mapped Column For application

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    #endregion

    #region Claim wages not paid for on weekly day of rest
    public class Complaint_Wages_WkDay
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Total Relief Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(200, ErrorMessage = "The max length of claim details is 200 characters..!")]
        public string DetailAboutTheClaim { get; set; }

        #region Not Mapped Column For application


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_Wages_WkDay_PeriodAmt
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        #region Not Mapped Column

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region Wages not paid for working overtime: 
    public class Complaint_Wages_OT
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Total Relief Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(200, ErrorMessage = "The max length of claim details is 200 characters..!")]
        public string DetailAboutTheClaim { get; set; }

        #region Not Mapped Column For application


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_Wages_OT_PeriodAmt
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Overtime is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OverTimeHours { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        #region Not Mapped Column

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region Wages not paid at all
    public class Complaint_Wages_Not_Paid
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Total Relief Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(200, ErrorMessage = "The max length of Reason is 200 characters..!")]
        public string Reason { get; set; }

        #region Not Mapped Column For application


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_Wages_Not_Paid_PeriodAmt
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        #region Not Mapped Column

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region Wages Unauthorised deduction
    public class Complaint_Wages_Unauth_Deduct
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Total Relief Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(200, ErrorMessage = "The max length of Reason is 200 characters..!")]
        public string Reason { get; set; }

        #region Not Mapped Column For application


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_Wages_Unauth_Deduct_PeriodAmt
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "From Date is required..!")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        public DateTime ToDate { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        #region Not Mapped Column

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region Non Payment of Bonus
    public class Complaint_Non_Pay_Bonus
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Total Relief Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalReliefSought { get; set; }

        [Required(ErrorMessage = "Compensation Sought is required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationSought { get; set; }

        [StringLength(200, ErrorMessage = "The max length of Detail is 200 characters..!")]
        public string Details { get; set; }

        #region Not Mapped Column For application


        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_Non_Pay_Bonus_PeriodAmt
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }


        [Required(ErrorMessage = "Accounting year is required..!")]
        [StringLength(200, ErrorMessage = "Accounting Year cannot exceed 200 characters..!.")]
        public string AccountingYear { get; set; }

        [Required(ErrorMessage = "Amount required..!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Bonus Claim Type is required..!")]
        public BonusClaimTypeEnum BonusClaimType { get; set; }

        #region Not Mapped Column

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region Recovery of money under section of IR Code

    public class Complaint_RecOfMon_GeneralDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required]
        public DateTime DemandNoticeServedDate { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_RecOfMon_MoneyDueDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Money due reason type is required..!")]
        public MoneyDueReasonTypeEnum MoneyDueReasonType { get; set; }


        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    public class Complaint_RecOfMon_SettlementDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Name of the parties to the settlement is required..!")]
        [StringLength(300, ErrorMessage = "Name of the parties to the settlement cannot exceed 300 characters.")]
        public string PartiesName { get; set; }

        [Required(ErrorMessage = "Settlement date is required..!")]
        public DateTime SettlementDate { get; set; }

        [Required(ErrorMessage = "Settlement type is required..!")]
        public SettlementTypeEnum SettlementType { get; set; }

        [StringLength(300, ErrorMessage = "Name and designation of the conciliation officer cannot exceed 300 characters.")]
        public string ConciliationOfficerNameAndDesignation { get; set; }

        [StringLength(500, ErrorMessage = "Address of the conciliation officer cannot exceed 500 characters.")]
        public string ConciliationOfficerAddress { get; set; }

        [Required(ErrorMessage = "Money due from which terms of settlement is required..!")]
        [StringLength(500, ErrorMessage = "Money due terms cannot exceed 500 characters.")]
        public string MoneyDueTerms { get; set; }

        [Required(ErrorMessage = "Amount of money due is required..!")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount of money due cannot be negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountDue { get; set; }

        [Required(ErrorMessage = "Date from which settlement money became due is required..!")]
        public DateTime AmountDueFromDate { get; set; }


        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_RecOfMon_AwardDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Name of the parties to the award is required..!")]
        [StringLength(300, ErrorMessage = "Name of the parties to the award cannot exceed 300 characters.")]
        public string PartiesName { get; set; }

        [Required(ErrorMessage = "Name of the CGIT/Arbitrator who gave the award is required..!")]
        [StringLength(300, ErrorMessage = "Name of the CGIT/Arbitrator cannot exceed 300 characters.")]
        public string CGITOrArbitratorName { get; set; }

        [Required(ErrorMessage = "Award number is required..!")]
        [StringLength(100, ErrorMessage = "Award number cannot exceed 100 characters.")]
        public string AwardNumber { get; set; }

        [Required(ErrorMessage = "Award date is required..!")]
        public DateTime AwardDate { get; set; }

        [Required(ErrorMessage = "Money due from which terms of award is required..!")]
        [StringLength(500, ErrorMessage = "Money due terms of award cannot exceed 500 characters.")]
        public string AwardTerms { get; set; }

        [Required(ErrorMessage = "Date from which award money became due is required..!")]
        public DateTime AmountDueFromDate { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_RecOfMon_NoticePayDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Date of joining is required..!")]
        public DateTime DateOfJoining { get; set; }

        [Required(ErrorMessage = "Date of termination is required..!")]
        public DateTime DateOfTermination { get; set; }

        [Required(ErrorMessage = "Period of notice pay is required..!")]
        public NoticePayPeriodTypeEnum NoticePayPeriodType { get; set; }

        [Required(ErrorMessage = "Amount of money due for non-payment of notice pay is required..!")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount of money due cannot be negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountDue { get; set; }

        [Required(ErrorMessage = "Date from which payment became due in lieu of notice is required..!")]
        public DateTime AmountDueFromDate { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_RecOfMon_RetrenchmentCompDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Date of joining is required..!")]
        public DateTime DateOfJoining { get; set; }

        [Required(ErrorMessage = "Date of retrenchment/closure of establishment is required..!")]
        public DateTime DateOfRetrenchmentOrClosure { get; set; }

        [Required(ErrorMessage = "Total length of service is required..!")]
        [Range(0, int.MaxValue, ErrorMessage = "Total length of service cannot be negative.")]
        public int TotalLengthOfServiceDays { get; set; }

        [Required(ErrorMessage = "Amount due on account of non-payment of retrenchment/closure compensation is required..!")]
        [Range(0, double.MaxValue, ErrorMessage = "Compensation amount cannot be negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationAmountDue { get; set; }

        [Required(ErrorMessage = "Date from which retrenchment/closure compensation became due is required..!")]
        public DateTime CompensationDueFromDate { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_RecOfMon_LayOffDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Date of joining is required..!")]
        public DateTime DateOfJoining { get; set; }

        [Required(ErrorMessage = "Date of lay-off is required..!")]
        public DateTime DateOfLayOff { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    public class Complaint_RecOfMon_LayOffCompDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Lay-off from date is required..!")]
        public DateTime LayOffFromDate { get; set; }

        [Required(ErrorMessage = "Lay-off to date is required..!")]
        public DateTime LayOffToDate { get; set; }

        [Required(ErrorMessage = "Amount of compensation is required..!")]
        [Range(0, double.MaxValue, ErrorMessage = "Compensation amount cannot be negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CompensationAmount { get; set; }

        [Required(ErrorMessage = "Date from which lay-off compensation became due is required..!")]
        public DateTime CompensationDueFromDate { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    #endregion


    #region Review
    public class Complaint_Review_OfDismissal
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }


        [Required(ErrorMessage = "Order number against which review is being filed is required..!")]
        public int OrderNumber { get; set; }

        [Required(ErrorMessage = "Date of the order is required..!")]
        public DateTime OrderDate { get; set; }

        [StringLength(1000, ErrorMessage = "Remarks cannot exceed 1000 characters.")]
        public string Remarks { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion

    #region Appeal Form

    public class Complaint_Appeal
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Order num type is required..!")]
        public OrderNumTypeEnum OrderNumType { get; set; }

        [Required(ErrorMessage = "Date of the order is required..!")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Name of authority is required..!")]
        [StringLength(100, ErrorMessage = "Name of authority cannot exceed 100 characters.")]
        public string NameOfAuthority { get; set; }

        [Required(ErrorMessage = "Address of authority is required..!")]
        [StringLength(500, ErrorMessage = "Address of authority cannot exceed 500 characters.")]
        public string AddressOfAuthority { get; set; }

        [Required(ErrorMessage = "Name of appellant is required..!")]
        [StringLength(100, ErrorMessage = "Name of appellant cannot exceed 100 characters.")]
        public string NameOfAppellant { get; set; }

        [Required(ErrorMessage = "Address of appellant is required..!")]
        [StringLength(500, ErrorMessage = "Address of appellant cannot exceed 500 characters.")]
        public string AddressOfAppellant { get; set; }

        [Required(ErrorMessage = "Name of respondent is required..!")]
        [StringLength(100, ErrorMessage = "Name of respondent cannot exceed 100 characters.")]
        public string NameOfRespondent { get; set; }

        [Required(ErrorMessage = "Address of respondent is required..!")]
        [StringLength(500, ErrorMessage = "Address of respondent cannot exceed 500 characters.")]
        public string AddressOfRespondent { get; set; }

        [Required(ErrorMessage = "Facts of case is required..!")]
        [StringLength(500, ErrorMessage = "Facts of case cannot exceed 500 characters.")]
        public string FactsOfCase { get; set; }

        [Required(ErrorMessage = "Ground of appeal is required..!")]
        [StringLength(500, ErrorMessage = "Ground of appeal cannot exceed 500 characters.")]
        public string GroundOfAppeal { get; set; }

        [Required(ErrorMessage = "Relief sought is required..!")]
        [StringLength(500, ErrorMessage = "Relief sought cannot exceed 500 characters.")]
        public string Reliefsought { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters.")]
        public string Remarks { get; set; }

        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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
    #endregion


    #region Industrial Disputes
    public class Complaint_IndustrialDispute
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application reference is required.")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        public virtual Application Application { get; set; }


        [Required(ErrorMessage = "Industrial dispute type is required.")]
        public IndustrialDisputeTypeEnum IndustrialDisputeType { get; set; }

        public DateTime? DateOfAppointment { get; set; }

        public DateTime? DateOfAction { get; set; }


        [StringLength(2000, ErrorMessage = "Industrial dispute details cannot exceed 2000 characters.")]
        public string IndustrialDisputeDetails { get; set; }


        [StringLength(1000, ErrorMessage = "Other reason cannot exceed 1000 characters.")]
        public string OtherReason { get; set; }


        [StringLength(1000, ErrorMessage = "Other relief cannot exceed 1000 characters.")]
        public string OtherRelief { get; set; }

        [StringLength(2000, ErrorMessage = "Relief sought cannot exceed 2000 characters.")]
        public string ReliefSought { get; set; }


        [StringLength(2000, ErrorMessage = "Remarks cannot exceed 2000 characters.")]
        public string Remarks { get; set; }


        #region NotMapped

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

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

    #region Industrial Disputes Reason Mapping
    public class Complaint_IndustrialDisputeReasonMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Money due reason type is required..!")]
        public IndustrialDisputesReasonTypeEnum IndustrialDisputesReasonType { get; set; }
    }

    #endregion

    #region

    public class Complaint_IndustrialDisputeReliefSoughtMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }


        [Required(ErrorMessage = "Money due reason type is required..!")]
        public IndustrialDisputesReliefSoughtTypeEnum IndustrialDisputesReliefSoughtType { get; set; }
    }
    #endregion
    #endregion

}
