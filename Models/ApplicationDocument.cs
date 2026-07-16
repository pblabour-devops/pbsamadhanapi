using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class ApplicationDocument
    {
        [Key]
        [Display(Name = "AppDocId")]
        public Int64 AppDocId { get; set; }

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

        public bool IsLocked { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "DocumentRefId is required..!")]
        [ForeignKey("Documents")]
        public Int64 DocumentRefId { get; set; }
        public virtual Document Document { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        public virtual ICollection<ApplicationDocument_Log> ApplicationDocument_Log { get; set; }
    }

    public class ApplicationAddendumDocument
    {
        [Key]
        public Int64 Id { get; set; }

        [Display(Name = "AttachmentName")]
        [StringLength(500, ErrorMessage = "Title must be 3-500 characters long !", MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "AttachmentName")]
        [StringLength(100, ErrorMessage = "Attachment name must be 100 characters long !")]
        public string AttachmentName { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

    }

}
