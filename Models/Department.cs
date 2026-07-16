using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Department
    {
        [Key]
        [Display(Name = "ID")]
        public Int64 DepartmentID { get; set; }

        [Required(ErrorMessage = "Please enter department name !")]
        [RegularExpression("^([0-9a-zA-Z,-/() ]+)$", ErrorMessage = "Department Name can only have alphanumerics, space and special characters (, - / ()) !")]
        [StringLength(60, ErrorMessage = "Department name must be 3-60 characters long !", MinimumLength = 3)]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Please enter department name abbre !")]
        //[RegularExpression("^([a-zA-Z]+)$", ErrorMessage = "Department Name Abbre can only have alphabets !")]
        [StringLength(8, ErrorMessage = "Department name must be 2-8 characters long !", MinimumLength = 2)]
        [Display(Name = "Department Abbre")]
        public string DepartmentNameAbbre { get; set; }

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

        public virtual ICollection<UserDepartmentMapping> UserDepartmentMappings { get; set; }
        public virtual ICollection<Application> Applications { get; set; }

        public virtual ICollection<DepartmentRoleMapping> DepartmentRoleMappings { get; set; }
    }

    public class DepartmentRoleMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [StringLength(100, ErrorMessage = "The max length of RoleRefId is 100 characters..!")]
        public string RoleRefId { get; set; }

        [Required(ErrorMessage = "DepartmentRefId id is required..!")]
        [ForeignKey("DepartmentRefId")]
        public Int64 DepartmentRefId { get; set; }
        public virtual Department Department { get; set; }
    }
}
