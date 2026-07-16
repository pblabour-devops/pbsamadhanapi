using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class ApplicationDocument_Log
    {
        [Key]
        [Display(Name = "DocumentLogId")]
        public Int64 DocumentLogId { get; set; }

        [Display(Name = "AttachmentName")]
        [StringLength(60, ErrorMessage = "Attachment name must be 7-60 characters long !", MinimumLength = 7)]
        public string AttachmentName { get; set; }

        [Display(Name = "Is Uploaded")]
        [Required]
        public bool IsUploaded { get; set; }

        [Required(ErrorMessage = "Uploaded Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid Uploaded date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Uploadeddate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "DocumentId is required..!")]
        [ForeignKey("Documents")]
        public Int64 DocumentId { get; set; }
        public virtual Document Documents { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("ApplicationDocument")]
        public Int64 AppDocId { get; set; }
        public virtual ApplicationDocument ApplicationDocument { get; set; }
    }
}
