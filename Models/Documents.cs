using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Document
    {
        [Key]
        [Display(Name = "DocumentId")]
        public Int64 DocumentId { get; set; }

        [Required(ErrorMessage = "Please enter document name!")]
        [RegularExpression("^([0-9a-zA-Z,-/() ]+)$", ErrorMessage = "Document name can only have alphanumerics, space and special characters (, - / ()) !")]
        [StringLength(200, ErrorMessage = "Document name must be 3-200 characters long!", MinimumLength = 3)]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Required(ErrorMessage = "Please enter document type !")]
        [StringLength(60, ErrorMessage = "Document type must be 2-60 characters long !", MinimumLength = 2)]
        [Display(Name = "Document Type")]
        public string DocumentExtensionType { get; set; }

        [Required]
        public int AllowedMaxMB { get; set; }

        [Required]
        public int AllowedMinMB { get; set; }
        public bool IsSampleDoc { get; set; }
        public string SampleDocPath { get; set; }
        public virtual ICollection<ApplicationDocument> ApplicationDocuments { get; set; }
        public virtual ICollection<AppTypeAllowedDocument> AppTypeAllowedDocuments { get; set; }
        public virtual ICollection<RoleWiseAllowedActionCode> RoleWiseAllowedActionCodes { get; set; }
    }
}
