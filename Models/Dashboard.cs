using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class MISDashboardData
    {
        [Key, Required]
        public Int64 Id { get; set; }

        //[Required(ErrorMessage = "PublicAppRefNum is required..!")]
        [StringLength(20)]
        public string PublicAppRefNum { get; set; }

        //[Required(ErrorMessage = "ApplicationType is required..!")]
        public int ApplicationType { get; set; }

        //[Required(ErrorMessage = "ApplicationPurposeType is required..!")]
        public int ApplicationPurposeType { get; set; }

        //[Required(ErrorMessage = "EstablishmentName is required..!")]
        [StringLength(500)]
        public string EstablishmentName { get; set; }

        //[Required(ErrorMessage = "Establishment address is required..!")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Sender_UserRefId is required..!")]
        [StringLength(200)]
        public string Sender_UserRefId { get; set; }

        //[Required(ErrorMessage = "Sender_ProfileRefId is required..!")]
        public int Sender_ProfileRefId { get; set; }

        //[Required(ErrorMessage = "SenderName is required..!")]
        [StringLength(200)]
        public string SenderName { get; set; }

        //[Required(ErrorMessage = "SenderRoleId is required..!")]
        [StringLength(200)]
        public string SenderRoleId { get; set; }

        //[Required(ErrorMessage = "SenderRoleName is required..!")]
        [StringLength(50)]
        public string SenderRoleName { get; set; }

        //[Required(ErrorMessage = "Receiver_UserRefId is required..!")]
        [StringLength(200)]
        public string Receiver_UserRefId { get; set; }

        //[Required(ErrorMessage = "Receiver_ProfileRefId is required..!")]
        public int Receiver_ProfileRefId { get; set; }

        //[Required(ErrorMessage = "ReceiverName is required..!")]
        [StringLength(200)]
        public string ReceiverName { get; set; }

        //[Required(ErrorMessage = "ReceiverRoleId is required..!")]
        [StringLength(200)]
        public string ReceiverRoleId { get; set; }

        //[Required(ErrorMessage = "ReceiverRoleName is required..!")]
        [StringLength(200)]
        public string ReceiverRoleName { get; set; }

        //[Required(ErrorMessage = "AppActionType is required..!")]
        public int AppActionType { get; set; }

        //[Required(ErrorMessage = "ActionDate is required..!")]
        public DateTime ActionDate { get; set; }

        //[Required(ErrorMessage = "ApplicationLifeCycleStatusType is required..!")]
        public int ApplicationLifeCycleStatusType { get; set; }

        //[Required(ErrorMessage = "ApplicationLifeCycleLastStatusOn is required..!")]
        public DateTime ApplicationLifeCycleLastStatusOn { get; set; }

        //[Required(ErrorMessage = "CircleName is required..!")]
        public string CircleName { get; set; }

        // [Required(ErrorMessage = "CircleRefId is required..!")]
        public int CircleRefId { get; set; }

        //  [Required(ErrorMessage = "Revenue is required..!")]
        public Decimal Revenue { get; set; }

        //[Required(ErrorMessage = "Remarks is required..!")]
        public string Remarks { get; set; }

        // [Required(ErrorMessage = "ActionTakenDaysCount is required..!")]
        public int ActionTakenDaysCount { get; set; }

        //[Required(ErrorMessage = "ActionTakenHoursCount is required..!")]
        public int ActionTakenHoursCount { get; set; }

        //[Required(ErrorMessage = "ActionPublicName is required..!")]
        public string ActionPublicName { get; set; }

        //[Required(ErrorMessage = "ProjectSiteRefId is required..!")]
        public int ProjectSiteRefId { get; set; }

        //[Required(ErrorMessage = "DistrictName is required..!")]
        public string DistrictName { get; set; }

        //[Required(ErrorMessage = "Legacy_NAR is required..!")]
        public string Legacy_NAR { get; set; }

        // [Required(ErrorMessage = "Legacy_AppFormId is required..!")]
        public string Legacy_AppFormId { get; set; }

        public Int64 AppId { get; set; }

        public Int64 ApplicationActionLogRefId { get; set; }
    }

    public class RoleWiseAllowedHRMSServiceCodes
    {
        [Key]
        public Int64 RoleWiseAllowedHRMSServiceCodeId { get; set; }

        [Required(ErrorMessage = "RoleId is required..!")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "HRMSServiceCode is required..!")]
        public int HRMSServiceCode { get; set; }

        [Required(ErrorMessage = "ApplicationType is required..!")]
        public int ApplicationType { get; set; }

        [Required(ErrorMessage = "ApplicationPurposeType is required..!")]
        public int ApplicationPurposeType { get; set; }

        [Required(ErrorMessage = "ServiceName is required..!")]
        public string ServiceName { get; set; }
        [Required(ErrorMessage = "ActCode is required..!")]
        public int ActCode { get; set; }

    }

    public class WhatsNewInPortal
    {
        [Key, Required]
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UsageType { get; set; }
        public string? Role { get; set; }
    }

    public class EmpUniqueId
    {
        [Key]
        public Int64 ID { get; set; }
        public string UniqueID { get; set; }
        public int IsVerified { get; set; }
    }
    public class OfflineReportLogs
    {
        [Key, Required]
        public Int64 Id { get; set; }
        public string ReportId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public string Actions { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserRefId { get; set; }
        public string RoleId { get; set; }
        public int UserProfileRefId { get; set; }
        public OfflineReportStatusTypeEnum ReportStatusType { get; set; }
    }

    public class LicenceWiseRelationshipMapping
    {
        [Key, Required]
        public Int64 Id { get; set; }
        public string LicenceNumber { get; set; }
        public string NewUserId { get; set; }
        public string NewUserName { get; set; }
        public string NewInvestPunjab_Ipin { get; set; }
        public Int64 NewInvestPunjab_AppId { get; set; }
        public Int64 NewAppId { get; set; }
        public string OldUserId { get; set; }
        public string OldUserName { get; set; }
        public Int64 OldAppId { get; set; }
        public Int64 OldAppFormId { get; set; }
        public string OldNAR { get; set; }
        public string OldInvestPunjab_Ipin { get; set; }
        public Int64 OldInvestPunjab_AppId { get; set; }
        public Int64 ProjectSiteRefId { get; set; }

    }

}
