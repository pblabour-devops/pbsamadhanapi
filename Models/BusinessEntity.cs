using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    //public class BusinessEntity
    //{
    //    [Key, Required]
    //    public Int64 BusinessEntityId { get; set; }

    //    [Display(Name = "Business Entity Name")]
    //    [Required(ErrorMessage = "Please enter Business Entity Name !")]
    //    public string BusinessEntityName { get; set; }

    //    [Display(Name = "First Name")]
    //    [Required(ErrorMessage = "Please enter First Name !")]
    //    public string ContactPersonFirstName { get; set; }

    //    [Display(Name = "Middle Name")]
    //    public string ContactPersonMiddleName { get; set; }

    //    [Display(Name = "Last Name")]
    //    [Required(ErrorMessage = "Please enter Last Name !")]
    //    public string ContactPersonLastName { get; set; }
    //    [Display(Name = "Mobile No")]
    //    [RegularExpression("^([0-9]+)$", ErrorMessage = "Mobile number can only have numbers, space and special characters (, - / ()) !")]
    //    [StringLength(10, ErrorMessage = "Mobile number must be 10-20 characters long !", MinimumLength = 10)]
    //    public string MobileNo { get; set; }

    //    [Required(ErrorMessage = "Email is required..!"), EmailAddress(ErrorMessage = "Invalid email address..!"), StringLength(50, ErrorMessage = "The max length of email is 100 characters..!")]
    //    public string Email { get; set; }

    //    [Display(Name = "IsActive")]
    //    public bool IsActive { get; set; }

    //    [Display(Name = "IsDeleted")]
    //    public bool IsDeleted { get; set; }

    //    [Required(ErrorMessage = "Created Date is required..!")]
    //    [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime Createddate { get; set; }

    //    [Required(ErrorMessage = "Last modified Date is required..!")]
    //    [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime LastModifiedDate { get; set; }

    //    [Required(ErrorMessage = "UserRefId is required..!")]
    //    [ForeignKey("User")]
    //    public string UserRefId { get; set; }
    //    public virtual User User { get; set; }

    //    public virtual ICollection<ProjectSite> ProjectSites { get; set; }

    //}
}
