using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class UserProfileMapping_Log
    {
        [Key, Required]
        public Int64 UserProfileMappingLogId { get; set; }

        [Required(ErrorMessage = "User user id is required..!")]
        public Int64 UserId { get; set; }

        [Required(ErrorMessage = "User profile id is required..!")]
        public Int64 UserProfileId { get; set; }

        [Required(ErrorMessage = "User assign is required..!")]
        public DateTime DateOfUserAssign { get; set; }

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
    }
}
