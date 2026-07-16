using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    //public class UserRole : IdentityRole<Int64>
    //{
    //    public UserRole() : base()
    //    {

    //    }

    //    public UserRole(string roleName)
    //    {
    //        Name = roleName;
    //    }
    //    [Key, Required]
    //    public Int64 RoleId { get; set; }

    //    [Required(ErrorMessage = "Role name is required..!")]
    //    [StringLength(100, ErrorMessage = "The max length of role name is 100 characters..!")]
    //    public string RoleName { get; set; }

    //    [Required(ErrorMessage = "Role code is required..!")]
    //    [StringLength(10, ErrorMessage = "The max length of role code is 10 characters..!")]
    //    public string RoleCode { get; set; }

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

    //    public virtual UserRoleMapping UserRoleMapping { get; set; }
    //}
}
