using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class LegacyApprovedClearenceMapping
    {
        [Key, Required]
        public Int64 LegacyApprovedClearenceMappingId { get; set; }

        [Required(ErrorMessage = "Legacy AppId is required..!")]
        public Int64 Legacy_AppId { get; set; }

        [Required(ErrorMessage = "Legacy AppFormId is required..!")]
        public Int64 Legacy_AppFormId { get; set; }

        [Required(ErrorMessage = "Legacy NAR is required..!")]
        public string Legacy_NAR { get; set; }

        //[Required(ErrorMessage = "Approved by user id is required..!")]
        //public string ApprovedByUserId { get; set; }

        //[Required(ErrorMessage = "Approved by profile id is required..!")]
        //public Int64 ApprovedByProfileId { get; set; }

        //[Required(ErrorMessage = "Approved by role id is required..!")]
        //public string ApprovedByRoleId { get; set; }

        [Required(ErrorMessage = "Approved On is required..!")]
        public DateTime ApprovedOn { get; set; }

        [Required(ErrorMessage = "Attachment Name is required..!")]
        public string AttachmentName { get; set; }

        public string LegacyLicenceNo { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 Sys_N_AppRefId { get; set; }
        public virtual Application Application { get; set; }
    }

    public class SharedIsDirtyFlagRecordLog
    {
        [Key, Required]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Request received on is required..!")]
        public DateTime RequestReceivedOn { get; set; }

        [Required(ErrorMessage = "ResponseJson is required..!")]
        public string ResponseJson { get; set; }
    }

    public class LegacyDocumentMapping
    {
        [Key, Required]
        public Int64 Id { get; set; }
        public string Legacy_DocumentName { get; set; }
        public int Legacy_DocumentId { get; set; }
        public string NewDocumentName { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public Int64 Legacy_AppFormId { get; set; }
        public string Legacy_NAR { get; set; }
        public Int64 AppRefId { get; set; }
        public bool IsMoved { get; set; } = false;

        public bool IsApproval { get; set; } = false;
    }

    public class ApplicationSeedingLog
    {
        [Key, Required]
        public Int64 Id { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public Int64 Legacy_AppFormId { get; set; }
        public string Legacy_NAR { get; set; }
        public SeedStatusTypeEnum IsMasterSeeded { get; set; }
        public SeedStatusTypeEnum IsDocumentSeeded { get; set; }
        public SeedStatusTypeEnum IsPaymentSeeded { get; set; }
        public SeedStatusTypeEnum IsApplicationLogsSeeded { get; set; }
        public SeedStatusTypeEnum IsApprovalsSeeded { get; set; }
        public bool OverAllSeedStatus { get; set; }
        public string SeedStatusDiscription { get; set; }
        public SeedStatusTypeEnum IsUserSeeded { get; set; }
        public SeedStatusTypeEnum IsProfileSeeded { get; set; }
        public SeedStatusTypeEnum IsProjectSiteSeeded { get; set; }
        public SeedStatusTypeEnum IsBusinessRequestLogStatusSeeded { get; set; }
        public Int64 NewAppId { get; set; }
    }

    public class UserProfile_HRMS_CodeMapping
    {
        [Key, Required]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "User profile ref id is required..!")]
        [ForeignKey("UserProfile")]
        public Int64 UserProfileRefId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        [Required(ErrorMessage = "HRMS_Code is required..!")]
        public Int64 HRMS_Code { get; set; }
    }

    public class UserPortalSwitchLog
    {
        [Key]
        public Int64 Id { get; set; }
        public string UserId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string Token { get; set; }
        public string SwitchPortal { get; set; }
        public string RouteActivity { get; set; }

    }
}
