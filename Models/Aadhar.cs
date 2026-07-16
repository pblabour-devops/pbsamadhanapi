using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class AadharVerificationReqLog
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "RequestData is required..!")]
        public string RequestData { get; set; }

        [Required(ErrorMessage = "RequestSentOn is required..!")]
        public DateTime RequestSentOn { get; set; }

        public string ResponseData { get; set; }
        public DateTime ResponseReceivedOn { get; set; }

        public AadharVerificationTypeEnum AadharVerificationType { get; set; }

        public string Url { get; set; }

    }
}
