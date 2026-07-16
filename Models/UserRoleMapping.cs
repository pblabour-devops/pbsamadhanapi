using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    //public class UserRoleMapping
    //{
    //    [Key, Required]
    //    public Int64 RoleMappingId { get; set; }

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

    //    [Required(ErrorMessage = "UserId is required..!")]
    //    [ForeignKey("User")]
    //    public Int64 UserRefId { get; set; }
    //    public virtual User User { get; set; }

    //    [Required(ErrorMessage = "User role is required..!")]
    //    [ForeignKey("UserRole")]
    //    public Int64 UserRoleRefId { get; set; }
    //    public virtual UserRole UserRole { get; set; }
    //}
}
