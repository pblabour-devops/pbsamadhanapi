using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class DigitalSignature
    {
        [Key]
        public Int64 DigitalSignatureId { get; set; }

        [Required(ErrorMessage = "Digital signature json data is required..!")]
        public string JsonData { get; set; }

        [Required]
        [StringLength(500)]
        public string SelCertSubject { get; set; }

        [Required]
        [StringLength(500)]
        public string CertThumbPrint { get; set; }

        [Required]
        [StringLength(500)]
        public string PublicKey { get; set; }

        [Required]
        [StringLength(100)]
        public string EMail { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }
        
        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [StringLength(100)]
        public string VerificationUid { get; set; }

        [Required]
        [StringLength(100)]
        public string VerificationPid { get; set; }

        [Required(ErrorMessage = "UserProfileRefId is required..!")]
        [ForeignKey("UserProfileRefId")]
        public Int64 UserProfileRefId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<DigitalSignature_Log> DigitalSignature_Logs { get; set; }
    }

    public class DigitalSignature_Log
    {
        [Key]
        public Int64 DigitalSignatureLogId { get; set; }

        [Required(ErrorMessage = "Digital signature json data is required..!")]
        public string JsonData { get; set; }

        [Required]
        [StringLength(500)]
        public string SelCertSubject { get; set; }

        [Required]
        [StringLength(500)]
        public string CertThumbPrint { get; set; }

        [Required]
        [StringLength(500)]
        public string PublicKey { get; set; }

        [Required]
        [StringLength(100)]
        public string eMail { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [StringLength(100)]
        public string VerificationUid { get; set; }

        [Required]
        [StringLength(100)]
        public string VerificationPid { get; set; }
        public Int64 UserProfileRefId { get; set; }
        public virtual ICollection<Application> Applications { get; set; }

        [Required(ErrorMessage = "DigitalSignatureRefId id is required..!")]
        [ForeignKey("DigitalSignature")]
        public Int64 DigitalSignatureRefId { get; set; }
        public virtual DigitalSignature DigitalSignature { get; set; }
    }
}
