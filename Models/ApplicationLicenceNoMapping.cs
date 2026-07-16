using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class ApplicationLicenceNoMapping
    {
        [Key, Required]
        public Int64 ApplicationLicenceNoMappingId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "Licence Number is required..!")]
        public string LicenceNumber { get; set; }

        //[Required(ErrorMessage = "Valid From is required..!")]
        //public DateTime? ValidFrom { get; set; }

        //[Required(ErrorMessage = "Valid Upto is required..!")]
        //public DateTime? ValidUpto { get; set; }
    }

    public class ApplicationDisputeNoMapping
    {
        [Key, Required]
        public int ApplicationDisputeNoMappingId { get; set; }

        [Required(ErrorMessage = "EstablishmentId is required..!")]
        public string EstablishmentId { get; set; }

        [Required(ErrorMessage = "Dispute Number is required..!")]
        public string DisputeNumber { get; set; }

        public int IsNotSent { get; set; }

        public DateTime SentDateTime { get; set; }

        public Int64 EstablishmentEPFOLogsId { get; set; }

    }
}
