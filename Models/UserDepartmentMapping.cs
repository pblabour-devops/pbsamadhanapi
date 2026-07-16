using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class UserDepartmentMapping
    {
        [Key, Required]
        public int UserDeptMappingId { get; set; }

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


        [Required(ErrorMessage = "User role is required..!")]
        [ForeignKey("User")]
        public string UserRefId { get; set; }
        public virtual User User { get; set; }




        [Required(ErrorMessage = "DepartmenrRefId is required..!")]
        [ForeignKey("Department")]
        public Int64 DepartmentRefID { get; set; }
        public virtual Department Department { get; set; }
    }
}
