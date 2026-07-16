using pbsamadhannetcoreapi.CommonUtiliteis.CustomeAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class ShopLicence_GeneralDetail
    {
        [Key, Required]
        public Int64 ShopLicenceId { get; set; }

        [Required(ErrorMessage = "Closing Day is required..!")]
        public ClosingDayTypeEnum ClosingDay { get; set; }

        [Required(ErrorMessage = "Opening Hours Of Establishment is required..!")]
        public string OpeningHoursOfEstablishment { get; set; }

        [Required(ErrorMessage = "Closing Hours Of Establishment is required..!")]
        public string ClosingHoursOfEstablishment { get; set; }

        [Required(ErrorMessage = "Owner Name is required..!")]
        [StringLength(100)]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Owner Father Or Husband Name is required..!")]
        [StringLength(100)]
        public string OwnerFatherOrHusbandName { get; set; }

        [Required(ErrorMessage = "Aadhar Number is required..!")]
        public string AadharNumber { get; set; }

        [Required(ErrorMessage = "Establishment Constitution Type is required..!")]
        public EstablishmentConstitutionTypeEnum EstablishmentConstitutionType { get; set; }

        [StringLength(100)]
        public string ManagerName { get; set; }

        [Required(ErrorMessage = "Shop EstablishmentType Type is required..!")]
        public ShopTypeEnum ShopType { get; set; }

        [Required(ErrorMessage = "National Industrial Classification Code is required..!")]
        public string NationalIndustrialClassificationCode { get; set; }

        [Required(ErrorMessage = "Is Having Employee is required..!")]
        public int IsHavingEmployee { get; set; }

        //[Required(ErrorMessage = "RootActivityRefId is required..!")]
        //[StringLength(50)]
        //public string RootActivityRefId { get; set; }

        //[Required(ErrorMessage = "RootActivityRefId is required..!")]
        //[StringLength(50)]
        //public string RootActivityRefId { get; set; }

        //[Required(ErrorMessage = "Pan Or Tan Number is required..!")]
        //[StringLength(10)]
        //public string PanOrTanNumber { get; set; }

        //[Required(ErrorMessage = "Gst Number is required..!")]
        //[StringLength(15)]
        //public string GstNumber { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

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
        public Int64 LabourCircleRefId { get; set; }

        [NotMapped]
        public Int64 DistrictRefId { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }


        [NotMapped]
        public List<ShopLicence_EmployeeDetail> EmployeeList { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }

        public virtual ICollection<ShopLicence_EmployeeDetail> ShopLicence_EmployeeDetails { get; set; }

    }
    public class ShopLicence_EmployeeDetail
    {
        [Key]
        public Int64 EmployeeDetailId { get; set; }

        [Required(ErrorMessage = "Name of employee is Required..!"), StringLength(100, ErrorMessage = "The max length of Name of employee is 100 characters..!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Father Or Husband Name is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Father Or Husband Name is 100 characters..!")]
        public string FatherOrHusbandName { get; set; }

        [Required(ErrorMessage = "Gender is Required..!"), StringLength(100, ErrorMessage = "The max length of Employee Gender is 100 characters..!")]
        [RegularExpression("MALE|FEMALE|OTHER", ErrorMessage = "Invalid gender type..!")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid DateOfBirth..!")]
        //[Range(typeof(DateTime), "1930-01-01", "2099-12-31", ErrorMessage = "Invalid DateOfBirth..!")]
        //[RegularExpression(@"(((19|20)\d\d)-(0[1-9]|1[0-2])-((0|1)[0-9]|2[0-9]|3[0-1]))", ErrorMessage = "Invalid date format..!")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Mobile Number is required..!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage ="Invalid mobile number length..!")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid mobile number..!")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Pan Number is required..!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid PanNumber length..!")]
        [RegularExpression(@"[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessage = "Invalid PanNumber..!")]
        public string PanNumber { get; set; }

        [Required(ErrorMessage = "Aadhar Number is required..!")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Invalid AadharNumber length..!")]
        [RegularExpression(@"^[2-9]{1}[0-9]{11}$", ErrorMessage = "Invalid AadharNumber..!")]
        [CustomeAttribute_ExcelUniqueColumn(allowDuplicateInSheet : false, candidateColumnsCommaSeparated: "Name", errorMessage: "Duplicate Aadhar number..!")]
        public string AadharNumber { get; set; }

        [Required(ErrorMessage = "Joining Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid JoiningDate..!")]
        public DateTime JoiningDate { get; set; }

        [Required(ErrorMessage = "ClosingDayType is required..!")]
        [Range(1, 8, ErrorMessage = "Invalid ClosingDayType..!")]
        public ClosingDayTypeEnum ClosingDayType { get; set; }

        [Required(ErrorMessage = "Working Hours From is required..!")]
        [RegularExpression("0:00|0:15|0:30|0:45|1:00|1:15|1:30|1:45|2:00|2:15|2:30|2:45|3:00|3:15|3:30|3:45|4:00|4:15|4:30|4:45|5:00|5:15|5:30|5:45|6:00|6:15|6:30|6:45|7:00|7:15|7:30|7:45|8:00|8:15|8:30|8:45|9:00|9:15|9:30|9:45|10:00|10:15|10:30|10:45|11:00|11:15|11:30|11:45|12:00|12:15|12:30|12:45|13:00|13:15|13:30|13:45|14:00|14:15|14:30|14:45|15:00|15:15|15:30|15:45|16:00|16:15|16:30|16:45|17:00|17:15|17:30|17:45|18:00|18:15|18:30|18:45|19:00|19:15|19:30|19:45|20:00|20:15|20:30|20:45|21:00|21:15|21:30|21:45|22:00|22:15|22:30|22:45|23:00|23:15|23:30|23:45", 
            ErrorMessage = "Invalid WorkingHoursFrom..!")]
        public string WorkingHoursFrom { get; set; }

        [Required(ErrorMessage = "Working Hours To is required..!")]
        [RegularExpression("0:00|0:15|0:30|0:45|1:00|1:15|1:30|1:45|2:00|2:15|2:30|2:45|3:00|3:15|3:30|3:45|4:00|4:15|4:30|4:45|5:00|5:15|5:30|5:45|6:00|6:15|6:30|6:45|7:00|7:15|7:30|7:45|8:00|8:15|8:30|8:45|9:00|9:15|9:30|9:45|10:00|10:15|10:30|10:45|11:00|11:15|11:30|11:45|12:00|12:15|12:30|12:45|13:00|13:15|13:30|13:45|14:00|14:15|14:30|14:45|15:00|15:15|15:30|15:45|16:00|16:15|16:30|16:45|17:00|17:15|17:30|17:45|18:00|18:15|18:30|18:45|19:00|19:15|19:30|19:45|20:00|20:15|20:30|20:45|21:00|21:15|21:30|21:45|22:00|22:15|22:30|22:45|23:00|23:15|23:30|23:45", 
            ErrorMessage = "Invalid WorkingHoursTo..!")]

        public string WorkingHoursTo { get; set; }

        [Required(ErrorMessage = "Interval From is required..!")]
        [RegularExpression("0:00|0:15|0:30|0:45|1:00|1:15|1:30|1:45|2:00|2:15|2:30|2:45|3:00|3:15|3:30|3:45|4:00|4:15|4:30|4:45|5:00|5:15|5:30|5:45|6:00|6:15|6:30|6:45|7:00|7:15|7:30|7:45|8:00|8:15|8:30|8:45|9:00|9:15|9:30|9:45|10:00|10:15|10:30|10:45|11:00|11:15|11:30|11:45|12:00|12:15|12:30|12:45|13:00|13:15|13:30|13:45|14:00|14:15|14:30|14:45|15:00|15:15|15:30|15:45|16:00|16:15|16:30|16:45|17:00|17:15|17:30|17:45|18:00|18:15|18:30|18:45|19:00|19:15|19:30|19:45|20:00|20:15|20:30|20:45|21:00|21:15|21:30|21:45|22:00|22:15|22:30|22:45|23:00|23:15|23:30|23:45", 
            ErrorMessage = "Invalid IntervalFrom..!")]

        public string IntervalFrom { get; set; }
        [RegularExpression("0:00|0:15|0:30|0:45|1:00|1:15|1:30|1:45|2:00|2:15|2:30|2:45|3:00|3:15|3:30|3:45|4:00|4:15|4:30|4:45|5:00|5:15|5:30|5:45|6:00|6:15|6:30|6:45|7:00|7:15|7:30|7:45|8:00|8:15|8:30|8:45|9:00|9:15|9:30|9:45|10:00|10:15|10:30|10:45|11:00|11:15|11:30|11:45|12:00|12:15|12:30|12:45|13:00|13:15|13:30|13:45|14:00|14:15|14:30|14:45|15:00|15:15|15:30|15:45|16:00|16:15|16:30|16:45|17:00|17:15|17:30|17:45|18:00|18:15|18:30|18:45|19:00|19:15|19:30|19:45|20:00|20:15|20:30|20:45|21:00|21:15|21:30|21:45|22:00|22:15|22:30|22:45|23:00|23:15|23:30|23:45", 
            ErrorMessage = "Invalid IntervalFrom..!")]
        [Required(ErrorMessage = "Interval To is required..!")]
        public string IntervalTo { get; set; }

        [Required(ErrorMessage = "ShopLicenceRefId is required..!")]
        [ForeignKey("ShopLicence_GeneralDetail")]
        public Int64 ShopLicenceRefId { get; set; }
        public virtual ShopLicence_GeneralDetail ShopLicence_GeneralDetail { get; set; }

        //[NotMapped]
        //public bool IsValidRow { get; set; }

        //[NotMapped]
        //public string ValidationError { get; set; }
    }
}