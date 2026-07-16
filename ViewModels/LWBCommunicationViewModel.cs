using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.CommonUtiliteis.CustomeAttributes;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace pbsamadhannetcoreapi.ViewModels
{
   
        public class RawTokenSerializedDataViewModel
        {
            public string ProjectId { get; set; }
            public string DataCode { get; set; }
        }

        public class AuthTokenSerializedDataViewModel
        {
            public string ProjectId { get; set; }
            public string DataCode { get; set; }
            public DateTime tokenTime { get; set; }
            public DateTime expireOn { get; set; }
        }


        public class LWBApplicationDetailsViewModel
        {
            public Int64 AppId { get; set; }
            public Int64 FCId { get; set; }
            public Int64 LabourInspCircleGradeId { get; set; }
            public string TokenNumber { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string EstdName { get; set; }
            public string SiteAddress { get; set; }
            public Int64 SiteAddTehsilId { get; set; }
            public string SiteAddPinCode { get; set; }
            public string SiteAddCity { get; set; }
            public Int32 AppFor { get; set; }
            public Int64 DistrictId { get; set; }
            public string MobileNo    { get; set; }
            public string Email { get; set; }
            public DateTime ApplicationDate { get; set; }
            public string ApplicationUserID { get; set; }
            public string UserId { get; set; }
            public string UserFirstName { get; set; }
            public string UserMiddleName { get; set; }
            public string UserLastName { get; set; }
            public Int64 UserDistrictId { get; set; }
            public DateTime UserRegistrationDate { get; set; }
            public string UserEmail { get; set; }
            public string UserPhoneNumber { get; set; }
            public string UserName { get; set; }
            public Int64 AppClearanceAppId { get; set; }
            public Int32 ApplicationType { get; set; }
            public Int32 ApplicationPurposeType { get; set; }
            public string LicenceNo { get; set; }
            public DateTime ClearanceIssuedOn { get; set; }
            public string ClearanceExpiredOn { get; set; }
        }

         public class ErrorLogParamsViewModel
         {
            public string ErrRoute { get; set; }
            public string ErrDesc { get; set; }
           public string ErrException { get; set; }
           public string ErrIP { get; set; }
           public DateTime ErrDate { get; set; }
           public int ProjectModuleId { get; set; }
           public string Username { get; set; }
          public string RequestParameter { get; set; }
         }

        public class LWBFormRequestViewModel
        {
            public int ServiceType { get; set; }
            public string FinancialYear { get; set; }
            public string TimeSlot { get; set; }
            public string LicenceNumber { get; set; }
            public int ProjectSiteRefId     { get; set; }
            public int DocId { get; set; }
            public string AppDocId   { get; set; }
            public string Remarks { get; set; }
            public long Id { get; set; }
        }

    public class LWBCsvEmployeeModel
    {
        public string Employee_Name { get; set; }
        public string Father_Or_Husband_Name { get; set; }
        public string Gender { get; set; }
        public string Aadhar_Or_Passport_No { get; set; }
        public string Mobile { get; set; }
        public string Bank_Account_No { get; set; }
        public string Bank_Name { get; set; }
        public string IFSCCode { get; set; }
        public string ESIC_No { get; set; }
        public string PF_No { get; set; }
        public string Date_of_Birth { get; set; }
        public string Date_of_Joining { get; set; }
        public string Date_of_Relieving { get; set; }
        public string Location { get; set; }
        public string Nationality { get; set; }
        public string No_Of_Months_Employee_On_Leave { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
    }

    public class UnpaidWagesCsvEmployeeModel
    {
        public string Employee_Id { get; set; }
        public string Employee_Name { get; set; }
        public string Aadhaar_Number { get; set; }
        public string Father_Name { get; set; }
        public string Date_of_Joining { get; set; }
        public string Date_of_Resign { get; set; }
        public string Mobile { get; set; }
        public string Bank_Account_No { get; set; }
        public string Bank_Name { get; set; }
        public string IFSCCode { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
    }

    public class ExcelUploadViewModel
    {

        [Required(ErrorMessage = "Name of employee is Required..!"), StringLength(100, ErrorMessage = "The max length of Name of employee is 100 characters..!")]
        public string Employee_Name { get; set; }

        [Required(ErrorMessage = "Father Or Husband Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Father Or Husband Name is 100 characters..!")]
        public string Father_Or_Husband_Name { get; set; }

        [Required(ErrorMessage = "Gender is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Gender is 100 characters..!")]
        [RegularExpression("MALE|FEMALE|OTHER", ErrorMessage = "Invalid gender type..!")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Aadhar Number is required..!")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Invalid AadharNumber length..!")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Invalid AadharNumber..!")]
        [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet: false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Aadhar number..!")]
        public string Aadhar_Or_Passport_No { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid mobile number length..!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid mobile number..!")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Bank Account No is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Account No is 100 characters..!")]
        [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet: false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Bank Account No..!")]
        public string Bank_Account_No { get; set; }

        [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
        public string Bank_Name { get; set; }

        [Required(ErrorMessage = "Bank Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Bank Name is 100 characters..!")]
        public string IFSCCode { get; set; }

        [Required(ErrorMessage = "ESIC No is Required..!"), StringLength(100, ErrorMessage = "The max length of ESIC No is 100 characters..!")]
        public string ESIC_No { get; set; }

        [Required(ErrorMessage = "PF No is Required..!"), StringLength(100, ErrorMessage = "The max length of PF No is 100 characters..!")]
        public string PF_No { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid DateOfBirth..!")]
        //[Range(typeof(DateTime), "1930-01-01", "2099-12-31", ErrorMessage = "Invalid DateOfBirth..!")]
        //[RegularExpression(@"(((19|20)\d\d)-(0[1-9]|1[0-2])-((0|1)[0-9]|2[0-9]|3[0-1]))", ErrorMessage = "Invalid date format..!")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Joining Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Joining Date..!")]
        public DateTime Date_of_Joining { get; set; }

        [Required(ErrorMessage = "Date of Relieving is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Date of Relieving..!")]
        public DateTime Date_of_Relieving { get; set; }

        [Required(ErrorMessage = "Location is required..!")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Nationality is required..!")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Apr is required..!")]
        public string Apr { get; set; }

        [Required(ErrorMessage = "May is required..!")]
        public string May { get; set; }

        [Required(ErrorMessage = "Jun is required..!")]
        public string Jun { get; set; }

        [Required(ErrorMessage = "Jul is required..!")]
        public string Jul { get; set; }

        [Required(ErrorMessage = "Aug is required..!")]
        public string Aug { get; set; }

        [Required(ErrorMessage = "Sep is required..!")]
        public string Sep { get; set; }

        [Required(ErrorMessage = "Oct is required..!")]
        public string Oct { get; set; }

        [Required(ErrorMessage = "Nov is required..!")]
        public string Nov { get; set; }

        [Required(ErrorMessage = "Dec is required..!")]
        public string Dec { get; set; }

        [Required(ErrorMessage = "Jan is required..!")]
        public string Jan { get; set; }

        [Required(ErrorMessage = "Feb is required..!")]
        public string Feb { get; set; }

        [Required(ErrorMessage = "Mar is required..!")]
        public string Mar { get; set; }

    }


}
