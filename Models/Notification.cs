using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Notification
    {
        [Key]
        public Int64 NotificationId { get; set; }

        [Required(ErrorMessage = "Title is Required..!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Body is Required..!")]
        public string Body { get; set; }

        [Required(ErrorMessage = "NotificationMode is Required..!")]
        public NotificationModeTypeEnum NotificationMode { get; set; }

        [Required(ErrorMessage = "NotificationPurpose is Required..!")]
        public NotificationPurposeTypeEnum NotificationPurpose { get; set; }

        [Required(ErrorMessage = "SendToUserRefId is Required..!"), StringLength(100, ErrorMessage = "The max length of SendToUserRefId is 100 characters..!")]
        public string SendToUserRefId { get; set; }
        public string AnonymusEmailOrMobile { get; set; }

        [Required(ErrorMessage = "NotificationStatus is Required..!")]
        public NotificationStatusTypeEnum NotificationStatus { get; set; }
        public bool IsExpirable { get; set; }

        [Required(ErrorMessage = "ExpireInMinutes is Required..!")]
        public int ExpireInMinutes { get; set; }

        [Required(ErrorMessage = "CreatedOn is Required..!")]
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "ResponseText is Required..!")]
        public string ResponseText { get; set; }

        [Required(ErrorMessage = "ResponseOn is Required..!")]
        public DateTime ResponseOn { get; set; }
    }


}
