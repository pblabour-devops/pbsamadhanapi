using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class BusinessFirst_RequestLog
    {
        [Key, Required]
        public int BusinessFirst_RequestLogId { get; set; }

        [Required(ErrorMessage = "Appid is required..!")]
        public Int64 AppId { get; set; }

        [Required(ErrorMessage = "IPin is required..!")]
        public Int64 IPin { get; set; }

        [Required(ErrorMessage = "UserId is required..!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "ServiceCode is required..!")]
        public int ServiceCode { get; set; }

        [Required(ErrorMessage = "CategoryTypeId is required..!")]
        public int CategoryTypeId { get; set; }

        [Required(ErrorMessage = "RequestString is required..!")]
        public string RequestString { get; set; }

        [Required(ErrorMessage = "RequestCount is required..!")]
        public int RequestCount { get; set; }
        public Int64 NativeAppId { get; set; }
        public string NativeUserId { get; set; }

        [Required(ErrorMessage = "IsEnabled is required..!")]
        public bool IsEnabled { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }
    }

    public class BusinessFirstShareStatusLog
    {
        [Key]
        public int StatusLogId { get; set; }

        [Required(ErrorMessage = "AppRefId is required..!")]
        public Int64 AppRefId { get; set; }

        [Required(ErrorMessage = "ApplicationTypeEnum required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [Required(ErrorMessage = "AppActionType required..!")]
        public AppActionTypeEnum AppActionType { get; set; }

        [Required(ErrorMessage = "ApiURL is required..!")]
        public string ApiURL { get; set; }

        [Required(ErrorMessage = "Status Sent On is required..!")]
        public DateTime StatusSentOn { get; set; }

        [Required(ErrorMessage = "RequestJSON is required..!")]
        public string RequestJSON { get; set; }

        [Required(ErrorMessage = "IsRequestCompeleted is required..!")]
        public bool IsRequestCompeleted { get; set; }
        public DateTime? RequestCompletionOn { get; set; }

        public string ResponseJson { get; set; }

        [Required(ErrorMessage = "TriedCount is required..!")]
        public int TriedCount { get; set; }

        public Int64? SyncStatusProcessEngineRefId { get; set; }

    }

    public class SyncStatus_ProcessEngineLog
    {
        [Key]
        public Int64 SyncStatusProcessEngineId { get; set; }
        public DateTime ProcessStartsOn { get; set; }
        public DateTime? ProcessEndsOn { get; set; }
        public int TotalTargetApplications { get; set; }
        public string Description { get; set; }
    }

    public class BusinessFirst_ApprovedFileSeeding
    {
        [Key, Required]
        public int ApprovedFileSeedingId { get; set; }

        [Required(ErrorMessage = "FormData is Required..!")]
        public string FormData { get; set; }

        [Required(ErrorMessage = "IPin is required..!")]
        public Int64 iPin { get; set; }

        [Required(ErrorMessage = "Appid is required..!")]
        public Int64 AppId { get; set; }

        [Required(ErrorMessage = "ServiceName is required..!")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "ServiceCode is required..!")]
        public int ServiceCode { get; set; }

        [Required(ErrorMessage = "ApprovalDate is required..!")]
        public DateTime ApprovalDate { get; set; }

        [Required(ErrorMessage = "LicenceNo is required..!")]
        public string LicenceNo { get; set; }

        [Required(ErrorMessage = "LicencePath is required..!")]
        public string LicencePath { get; set; }

        [Required(ErrorMessage = "ApprovedBy is required..!")]
        public string ApprovedBy { get; set; }

        [Required(ErrorMessage = "ApplicationSubmittedDate is required..!")]
        public DateTime ApplicationSubmittedDate { get; set; }

        [Required(ErrorMessage = "PaymentDetail is required..!")]
        public string PaymentDetail { get; set; }

        [Required(ErrorMessage = "ObjectionDetail is required..!")]
        public string ObjectionDetail { get; set; }
        public int DistrictId { get; set; }
        public int TehsilId { get; set; }
        public int CircleId { get; set; }
        public int CircleType { get; set; }

    }


    public class BusinessFirst_Withdraw_RequestLog
    {
        [Key, Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "IPin is required..!")]
        public Int64 IPin { get; set; }

        [Required(ErrorMessage = "Appid is required..!")]
        public Int64 AppId { get; set; }

        [Required(ErrorMessage = "ServiceCode is required..!")]
        public int ServiceCode { get; set; }

        [Required(ErrorMessage = "CategoryType is required..!")]
        public string CategoryType { get; set; }

        [Required(ErrorMessage = "RequestCount is required..!")]
        public int RequestCount { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }
    }
}
