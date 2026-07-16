using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class UserRegisteredMobileAppDevice
    {
        [Key, Required]
        public Int64 UserRegisteredMobileAppDeviceId { get; set; }

        [Required(ErrorMessage = "DeviceUniqueId is required..!")]
        [StringLength(100, ErrorMessage = "The max length of DeviceUniqueId is 100 characters..!")]
        public string DeviceUniqueId{ get; set; }

        [StringLength(50)]
        public string DeviceBrandName { get; set; }

        [StringLength(200)]
        public string DeviceModelInfo { get; set; }

        [StringLength(10)]
        public string LoginTokenKey { get; set; }

        [Required(ErrorMessage ="DeviceRegisterdOn is required")]
        public DateTime DeviceRegisterdOn { get; set; }

        [Required]
        [StringLength(300)]
        public string PushNotificationDeviceToken { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        [ForeignKey("User")]
        public string UserRefId { get; set; }
        public virtual User User { get; set; }
    }
}
