using pbsamadhannetcoreapi.CommonUtiliteis.CustomeAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class LWB_Contribution
    {
        [Key]
        public Int64 LWB_FundContributionId { get; set; }

        [Required(ErrorMessage = "Financial year is Required..!"), StringLength(15, ErrorMessage = "The max length of financial year is 15 characters..!")]
        public string FinancialYear { get; set; }

        [Required(ErrorMessage = "Month slot is Required..!"), StringLength(50, ErrorMessage = "The max length of Month slot is 15 characters..!")]
        public string MonthSlot { get; set; }

        [Required(ErrorMessage = "Fund Contribution Area Type is required..!")]
        public LWB_FundContributionAreaTypeEnum FundContributionAreaType { get; set; }

        [Required(ErrorMessage = "Fund Contribution Payment Type is required..!")]
        public LWB_FundContributionPaymentTypeEnum FundContributionPaymentType { get; set; }

        [StringLength(500, ErrorMessage = "The max length of remarks is 500 characters..!")]
        public string Remarks { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        public DateTime Createddate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        public DateTime LastModifiedDate { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        public virtual ICollection<LWB_Contribution_Employee> LWB_Contribution_Employees { get; set; }

        #endregion
    }

    public class LWB_Contribution_Employee
    {
        [Key]
        public Int64 LWB_EmployeeId { get; set; }

        [Required(ErrorMessage = "Name of employee is Required..!"), StringLength(100, ErrorMessage = "The max length of Name of employee is 100 characters..!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Father Or Husband Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Father Or Husband Name is 100 characters..!")]
        public string FatherOrHusbandName { get; set; }

        [Required(ErrorMessage = "Gender is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Gender is 100 characters..!")]
        [RegularExpression("MALE|FEMALE|OTHER", ErrorMessage = "Invalid gender type..!")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Aadhar Number is required..!")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Invalid AadharNumber length..!")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Invalid AadharNumber..!")]
        [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet: false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Aadhar number..!")]
        public string AadharNumber { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid mobile number length..!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid mobile number..!")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Account number is required..!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid mobile number..!")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Bank name is required..!")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Ifsc code is required..!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid ifsc code length..!")]
        public string IFSCCode { get; set; }

        [StringLength(17, MinimumLength = 17, ErrorMessage = "Invalid ESIC number length..!")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Invalid ESIC number..!")]
        public string ESIC_Number { get; set; }

        public string PF_Number { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid DateOfBirth..!")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Joining Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid JoiningDate..!")]
        public DateTime JoiningDate { get; set; }

        [Required(ErrorMessage = "Relieving Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Relieving date..!")]
        public DateTime RelievingDate { get; set; }

        [Required(ErrorMessage = "Location is required..!")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Nationality Type is required..!")]
        public NationalityTypeEnum NationalityType { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "LWB_FundContributionRefId is required..!")]
        [ForeignKey("LWB_Contributions")]
        public Int64 LWB_FundContributionRefId { get; set; }
        public virtual LWB_Contribution LWB_Contributions { get; set; }

        [Required(ErrorMessage = "LidRefId is required..!")]
        [ForeignKey("Lid")]
        public Int64 LidRefId { get; set; }
        public virtual LWB_Lid LWB_Lid { get; set; }

        #endregion
    }

    public class LWB_Lid
    {
        [Key]
        public Int64 LId { get; set; }
        [Required(ErrorMessage = "Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Name of employee is 100 characters..!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Aadhar Number is required..!")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Invalid AadharNumber length..!")]
        public string AadharNumber { get; set; }

        [Required(ErrorMessage = "Gender is Required..!"), StringLength(10, ErrorMessage = "The max length of Employee Gender is 10 characters..!")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required..!")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "PublicLId is Required..!"), StringLength(50, ErrorMessage = "The max length of PublicLId is 50 characters..!")]
        public string PublicLId { get; set; }

        public virtual ICollection<LWB_Contribution_Employee> LWB_Contribution_Employees { get; set; }

    }

    public class LWB_FundMaster
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Licence number is required.")]
        public string LicenceNo { get; set; }

        [Required(ErrorMessage = "Slab type is required.")]
        public LWBSlabTypeEnum LWBSlabType { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public string Year { get; set; }

        public LWBAadhaarverificationStatusTypeEnum LWBAadhaarVerficationStatusType { get; set; }

        [Required(ErrorMessage = "File Name is required")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public int Type { get; set; }

        [Required(ErrorMessage = "Created On is required")]
        public DateTime CreatedOn { get; set; }
        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        public virtual ICollection<LWB_Employees_Fund> LWBFundEmployees { get; set; }


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
        public int ProjectSiteVersion { get; set; }

        #endregion
    }

    #region LWB FUND EMPLOYEES
    //public class LWBFundEmployees
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required(ErrorMessage = "Fund master reference is required.")]
    //    public int FundMasterRefId { get; set; }

    //    [Required(ErrorMessage = "Employee name is required.")]
    //    public string Name { get; set; }

    //    [Required(ErrorMessage = "Aadhaar number is required.")]
    //    public string Aadhaar { get; set; }

    //    public AadharVerificationFormatTypeEnum VerificationFormatType { get; set; }
    //    public AadharVerificationDemographicTypeEnum VerificationDemographicType { get; set; }

    //    #region Foreign Key References

    //    [ForeignKey("FundMasterRefId")]
    //    public virtual LWBFundMaster LWBFundMaster { get; set; }



    //    #endregion


    //}

    #endregion

    public class LWB_Employees_Fund
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Fund master reference is required.")]
        public Int64 FundMasterRefId { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceeds 50 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can contain only alphabets and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Aadhar Number is required..!")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Invalid AadharNumber length..!")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Invalid AadharNumber..!")]
        [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet: false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Aadhar number..!")]
        public string Aadhaar { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        public AadharVerificationFormatTypeEnum VerificationFormatType { get; set; }
        public AadharVerificationDemographicTypeEnum VerificationDemographicType { get; set; }

        [Required(ErrorMessage = "Father Or Husband Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Father Or Husband Name is 100 characters..!")]
        public string FatherOrHusbandName { get; set; }

        [Required(ErrorMessage = "Gender is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Gender is 100 characters..!")]
        [RegularExpression("MALE|FEMALE|OTHER", ErrorMessage = "Invalid gender type..!")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid mobile number length..!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid mobile number..!")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Bank Account No is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Account No is 100 characters..!")]
        [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet: false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Bank Account No..!")]
        public string BankAccountNo { get; set; }

        [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC Code..!")]
        public string IFSCCode { get; set; }

        [Required(ErrorMessage = "ESIC No is Required..!"), StringLength(100, ErrorMessage = "The max length of ESIC No is 100 characters..!")]
        public string ESICNo { get; set; }

        [Required(ErrorMessage = "PF No is Required..!"), StringLength(100, ErrorMessage = "The max length of PF No is 100 characters..!")]
        public string PFNo { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid DateOfBirth..!")]
        //[Range(typeof(DateTime), "1930-01-01", "2099-12-31", ErrorMessage = "Invalid DateOfBirth..!")]
        //[RegularExpression(@"(((19|20)\d\d)-(0[1-9]|1[0-2])-((0|1)[0-9]|2[0-9]|3[0-1]))", ErrorMessage = "Invalid date format..!")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "Joining Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Joining Date..!")]
        public DateTime? DOJ { get; set; }

        public DateTime? DOR { get; set; }

        [Required(ErrorMessage = "Location is required..!")]
        [StringLength(200, ErrorMessage = "Location Cannot Exceeds 200 characters")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Nationality is required..!")]
        [StringLength(100, ErrorMessage = "Nationality Cannot Exceeds 100 characters")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Apr is required..!")]
        public LeaveTypeEnum Apr { get; set; }

        [Required(ErrorMessage = "May is required..!")]
        public LeaveTypeEnum May { get; set; }

        [Required(ErrorMessage = "Jun is required..!")]
        public LeaveTypeEnum Jun { get; set; }

        [Required(ErrorMessage = "Jul is required..!")]
        public LeaveTypeEnum Jul { get; set; }

        [Required(ErrorMessage = "Aug is required..!")]
        public LeaveTypeEnum Aug { get; set; }

        [Required(ErrorMessage = "Sep is required..!")]
        public LeaveTypeEnum Sep { get; set; }

        [Required(ErrorMessage = "Oct is required..!")]
        public LeaveTypeEnum Oct { get; set; }

        [Required(ErrorMessage = "Nov is required..!")]
        public LeaveTypeEnum Nov { get; set; }

        [Required(ErrorMessage = "Dec is required..!")]
        public LeaveTypeEnum Dec { get; set; }

        [Required(ErrorMessage = "Jan is required..!")]
        public LeaveTypeEnum Jan { get; set; }

        [Required(ErrorMessage = "Feb is required..!")]
        public LeaveTypeEnum Feb { get; set; }

        [Required(ErrorMessage = "Mar is required..!")]
        public LeaveTypeEnum Mar { get; set; }

        #region Foreign Key References

        [ForeignKey("FundMasterRefId")]
        public virtual LWB_FundMaster LWBFundMaster { get; set; }


        #endregion


    }

    public class LWB_Employees_UnpaidWages
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Fund master reference is required.")]
        public Int64 FundMasterRefId { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceeds 20 characters")]
        public string EmpIdInEstablishment { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceeds 50 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can contain only alphabets and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "UniqueKey is required..!")]
        public string UniqueKey { get; set; }

        [Required(ErrorMessage = "Father Or Husband Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Father Or Husband Name is 100 characters..!")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Joining Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Joining Date..!")]
        public DateTime? DateOfJoining { get; set; }

        public DateTime? DateOfResign { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid mobile number length..!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid mobile number..!")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Bank Account No is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Account No is 100 characters..!")]
        [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet: false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Bank Account No..!")]
        public string BankAccountNo { get; set; }

        [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC Code..!")]
        public string IFSCCode { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [StringLength(200, ErrorMessage = "Amount Cannot less then 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        public string Remarks { get; set; }
        public string Description { get; set; }
        public AadharVerificationFormatTypeEnum VerificationFormatType { get; set; }

        public AadharVerificationDemographicTypeEnum VerificationDemographicType { get; set; }

#region Foreign Key References

[ForeignKey("FundMasterRefId")]
        public virtual LWB_FundMaster LWBFundMaster { get; set; }


        #endregion


    }

    public class LWB_LIdAndUnpaidWages
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Fund master reference is required.")]
        public Int64 FundMasterRefId { get; set; }

        [Required(ErrorMessage = "Fund master reference is required.")]
        public Int64 LIdRefId { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceeds 20 characters")]
        public string EmpIdInEstablishment { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(50, ErrorMessage = "Name Cannot Exceeds 50 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can contain only alphabets and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Father Or Husband Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Father Or Husband Name is 100 characters..!")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Joining Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Joining Date..!")]
        public DateTime? DateOfJoining { get; set; }

        public DateTime? DateOfResign { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid mobile number length..!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid mobile number..!")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Bank Account No is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Account No is 100 characters..!")]
        [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet: false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Bank Account No..!")]
        public string BankAccountNo { get; set; }

        [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC Code..!")]
        public string IFSCCode { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        [StringLength(200, ErrorMessage = "Amount Cannot less then 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        public string Remarks { get; set; }

        #region Foreign Key References

        [ForeignKey("FundMasterRefId")]
        public virtual LWB_FundMaster LWBFundMaster { get; set; }

        [ForeignKey("LIdRefId")]
        public virtual LWB_Lid LWB_Lid { get; set; }


        #endregion


        public class LWB_LIdAndFund
        {
            [Key]
            public Int64 Id { get; set; }

            [Required(ErrorMessage = "Fund master reference is required.")]
            public Int64 FundMasterRefId { get; set; }

            [Required(ErrorMessage = "Fund master reference is required.")]
            public Int64 LIdRefId { get; set; }
            [Required(ErrorMessage = "Employee name is required.")]
            [StringLength(50, ErrorMessage = "Name Cannot Exceeds 50 characters")]
            [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can contain only alphabets and spaces.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Father Or Husband Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Father Or Husband Name is 100 characters..!")]
            public string FatherName { get; set; }

            [Required(ErrorMessage = "Joining Date is required..!")]
            [DataType(DataType.DateTime, ErrorMessage = "Invalid Joining Date..!")]
            public DateTime? DateOfJoining { get; set; }

            public DateTime? DateOfResign { get; set; }

            [Required(ErrorMessage = "Mobile Number is required..!")]
            [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid mobile number length..!")]
            [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid mobile number..!")]
            public string Mobile { get; set; }

            [Required(ErrorMessage = "Bank Account No is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Account No is 100 characters..!")]
            [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet: false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Bank Account No..!")]
            public string BankAccountNo { get; set; }

            [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
            public string BankName { get; set; }

            [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
            [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC Code..!")]
            public string IFSCCode { get; set; }

            [Required(ErrorMessage = "Apr is required..!")]
            public LeaveTypeEnum Apr { get; set; }

            [Required(ErrorMessage = "May is required..!")]
            public LeaveTypeEnum May { get; set; }

            [Required(ErrorMessage = "Jun is required..!")]
            public LeaveTypeEnum Jun { get; set; }

            [Required(ErrorMessage = "Jul is required..!")]
            public LeaveTypeEnum Jul { get; set; }

            [Required(ErrorMessage = "Aug is required..!")]
            public LeaveTypeEnum Aug { get; set; }

            [Required(ErrorMessage = "Sep is required..!")]
            public LeaveTypeEnum Sep { get; set; }

            [Required(ErrorMessage = "Oct is required..!")]
            public LeaveTypeEnum Oct { get; set; }

            [Required(ErrorMessage = "Nov is required..!")]
            public LeaveTypeEnum Nov { get; set; }

            [Required(ErrorMessage = "Dec is required..!")]
            public LeaveTypeEnum Dec { get; set; }

            [Required(ErrorMessage = "Jan is required..!")]
            public LeaveTypeEnum Jan { get; set; }

            [Required(ErrorMessage = "Feb is required..!")]
            public LeaveTypeEnum Feb { get; set; }

            [Required(ErrorMessage = "Mar is required..!")]
            public LeaveTypeEnum Mar { get; set; }
            #region Foreign Key References

            [ForeignKey("FundMasterRefId")]
            public virtual LWB_FundMaster LWBFundMaster { get; set; }

            [ForeignKey("LIdRefId")]
            public virtual LWB_Lid LWB_Lid { get; set; }


            #endregion


        }

    }
}
