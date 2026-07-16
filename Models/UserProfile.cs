using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class UserProfile
    {
        [Key, Required]
        public Int64 UserProfileId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter First Name !")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Last Name !")]
        public string LastName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Father Name !")]
        public string FatherName { get; set; }

        [Display(Name = "Mobile No")]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Mobile number can only have numbers, space and special characters (, - / ()) !")]
        [StringLength(10, ErrorMessage = "Mobile number must be 10-20 characters long !", MinimumLength = 10)]
        public string MobileNo { get; set; }

        [StringLength(10, ErrorMessage = "Invalid alternate mobile number..!")]
        public string AlternateMobileNo { get; set; }

        [Required(ErrorMessage = "Email is required..!"), EmailAddress(ErrorMessage = "Invalid email address..!"), StringLength(50, ErrorMessage = "The max length of email is 100 characters..!")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The max length of email is 100 characters..!")]
        public string AlternateEmail { get; set; }

        [Required(ErrorMessage = "Tehsil is required..!")]
        [ForeignKey("Estb_TehsilLgd")]
        public Int64 TehsilId { get; set; }
        public virtual TehsilLgd Estb_TehsilLgd { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("Estb_DistrictLgd")]
        public Int64 DistrictId { get; set; }
        public virtual DistrictLgd Estb_DistrictLgd { get; set; }

        [Required(ErrorMessage = "State is required..!"), StringLength(50, ErrorMessage = "Invalid State Name..!")]
        public string State { get; set; }

        [Required(ErrorMessage = "Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PinCode { get; set; }

        //[Required(ErrorMessage = "User signature is required..!")]
        public string Signature { get; set; }

        //[Required(ErrorMessage = "User profile photo is required..!")]
        public string ProfilePhoto { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(Name = "IsDeleted")]
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Createddate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }
        public virtual UserProfileMapping UserProfileMapping { get; set; }
        public virtual DigitalSignature DigitalSignature { get; set; }
        public virtual ICollection<ApplicationAction> ApplicationActions_Profile_Senders { get; set; }
        public virtual ICollection<ApplicationAction> ApplicationActions_Profile_Receivers { get; set; }
        public virtual UserProfile_HRMS_CodeMapping UserProfile_HRMS_CodeMapping { get; set; }

        public virtual ICollection<ApplicationAction_ParallelProcess> ApplicationActions_ParallelProcess_Profile_Senders { get; set; }
        public virtual ICollection<ApplicationAction_ParallelProcess> ApplicationActions_ParallelProcess_Profile_Receivers { get; set; }
    }
}
