using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class AuditLog_Factory_Backlog
    {
        [Key, Required]
        public Int64 AuditLog_Factory_BacklogId { get; set; }

        [Required(ErrorMessage = "AppRefId is required..!")]
        public Int64 AppRefId { get; set; }
        public string NAR { get; set; }
        public int AppFormId { get; set; }

        [Required(ErrorMessage = "IsLegacy is required..!")]
        public bool IsLegacy { get; set; }

        [Required(ErrorMessage = "IterationCounter is required..!")]
        public int IterationCounter { get; set; }

        [Required(ErrorMessage = "FieldName is required..!")]
        [StringLength(500, ErrorMessage = "The max length of FieldName is 500 characters..!")]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "OldValue is required..!")]
        [StringLength(500, ErrorMessage = "The max length of OldValue is 500 characters..!")]
        public string OldValue { get; set; }

        [Required(ErrorMessage = "NewValue is required..!")]
        [StringLength(500, ErrorMessage = "The max length of NewValue is 500 characters..!")]
        public string NewValue { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "ProfileRefId is required..!")]
        public Int64 ProfileRefId { get; set; }

        [Required(ErrorMessage = "Remarks is required..!")]
        public string Remarks { get; set; }

    }

    public class UserActivityLog
    {
        [Key, Required]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "UserId is required..!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Username is required..!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "ProfileId is required..!")]
        public Int64 ProfileId { get; set; }

        [Required(ErrorMessage = "ProfileName is required..!")]
        public string ProfileName { get; set; }

        [Required(ErrorMessage = "LoginTime is required..!")]
        public DateTime LoginTime { get; set; }

        [Required(ErrorMessage = "LoginStatus is required..!")]
        public string LoginStatus { get; set; }

        [Required(ErrorMessage = "IpAddress is required..!")]
        public string IpAddress { get; set; }

        [Required(ErrorMessage = "Method is required..!")]
        public string Method { get; set; }

        [Required(ErrorMessage = "Route is required..!")]
        public string Route { get; set; }

        [Required(ErrorMessage = "RequestBody is required..!")]
        public string RequestBody { get; set; }

        [Required(ErrorMessage = "CreatedOn is required..!")]
        public string CreatedOn { get; set; }

        [Required(ErrorMessage = "Latitude is required..!")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required..!")]
        public string Longitude { get; set; }
    }

    public class ErrorLog
    {
        [Key, Required]
        public Int64 Id { get; set; }
        public string ErrRoute { get; set; }
        public string ErrDesc { get; set; }
        public string ErrException { get; set; }
        public string ErrIP { get; set; }
        public DateTime ErrDate { get; set; }
        public int ProjectModuleId { get; set; }
        public string Username { get; set; }
    }
}
