using System.ComponentModel.DataAnnotations;
using System;

namespace pbsamadhannetcoreapi.Models
{
    public class Annual_Return
    {
        [Key]
        public Int64 ReturnId { get; set; }

        [Required(ErrorMessage = "AppId is required.")]
        public Int64 AppId { get; set; }

        [Required(ErrorMessage = "Return year is required.")]
        [StringLength(9, ErrorMessage = "Return year format should be like 2024.")]
        public string ReturnYear { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        [StringLength(50, ErrorMessage = "UserId cannot exceed 50 characters.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "ProjectSiteRefId is required.")]
        public Int64 ProjectSiteRefId { get; set; }


        [Required(ErrorMessage = "StepCodes are required.")]
        public string StepCodes { get; set; }

        [Required(ErrorMessage = "Licence Number is required.")]

        public string LicenceNumber { get; set; }

        public string JsonData { get; set; }

        public bool IsLocked { get; set; }

        [Required(ErrorMessage = "ActIds is required.")]
        public string ActIds { get; set; }

        public string AcknowledgementNo { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime LastModifiedOn { get; set; }
    }

}
