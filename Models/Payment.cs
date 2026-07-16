using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class FeesHeader
    {
        [Key, Required]
        public Int64 FeeHeaderId { get; set; }
        
        [Required(ErrorMessage = "Fee Header Title is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Fee Header Title is 500 characters..!")]
        public string FeeHeaderTitle { get; set; }
        public string MajorHead_MajorSubHead_MinorHead_MinorSubHead_00 { get; set; }
        public virtual ICollection<AppFeeDetail> AppFeeDetails { get; set; }
        public virtual ICollection<BuildingPlanHUDPaymentDetail> BuildingPlanHUDPaymentDetails { get; set; }
        public bool HasDedicatedTreasuryCode { get; set; }
        public string DedicatedTreasurCode { get; set; }
        public string DedicatedDDOCode { get; set; }
        public virtual ICollection<Payments_RaisedFee> Payments_RaisedFee { get; set; }
        public PaymentTreasuryTypeEnum PaymentTreasuryType { get; set; }
    }
    public class AppFeeDetail
    {
        [Key, Required]
        public Int64 FeeDetailId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        [Required(ErrorMessage = "Fee Header id is required..!")]
        [ForeignKey("AppFeesHeader")]
        public Int64 FeeHeaderRefId { get; set; }

        [Required(ErrorMessage = "Amount is required..!")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Calculated on is required..!")]
        public DateTime CalculatedOn { get; set; }

        [Required(ErrorMessage = "IsDeduductible is required..!")]
        public bool IsDeduductible { get; set; }

        [Required]
        public int PaymentBatchCounter { get; set; } = 1;
        public virtual Application Application { get; set; }
        public virtual FeesHeader FeesHeader { get; set; }

        [Required(ErrorMessage = "PaymentPartCounter is required..!")]
        public int PaymentPartCounter { get; set; }
        public bool HasDedicatedTreasuryCode { get; set; }
        public string DedicatedTreasurCode { get; set; }
        public string DedicatedDDOCode { get; set; }
        public string Description { get; set; }
    }
    public class AppFeeTransaction
    {
        [Key, Required]
        public Int64 AppFeeTransactionId { get; set; }

        [Required(ErrorMessage = "TransactionInitializationDate is required..!")]
        public DateTime TransactionInitializationDate { get; set; }

        [Required(ErrorMessage = "PaymentGatewayType is required..!")]
        public PaymentGatewayTypeEnum PaymentGatewayType { get; set; }

        [Required(ErrorMessage = "PaymentModeType is required..!")]
        public PaymentModeTypeEnum PaymentModeType { get; set; }

        [Required(ErrorMessage = "PaymentTreasuryTypeEnum is required..!")]
        public PaymentTreasuryTypeEnum PaymentTreasuryType { get; set; }

        [Required(ErrorMessage = "PaymentGatewayTargetUrl is required..!")]
        public string PaymentGatewayTargetUrl { get; set; }

        [Required(ErrorMessage = "PaymentGatewayApiMethodType is required..!")]
        public PaymentGatewayApiMethodTypeEnum PaymentGatewayApiMethodType { get; set; }

        [Required(ErrorMessage = "UniquePaymentGatewayTransactionId is required..!")]
        [StringLength(30)]
        public string UniquePaymentGatewayTransactionId { get; set; }

        [Required(ErrorMessage = "IsWebRequestCycleCompleted is required..!")]
        public bool IsWebRequestCycleCompleted { get; set; }

        [Required(ErrorMessage = "RequestBodyData is required..!")]
        public string RequestBodyData { get; set; }

        public string ResponseBodyData { get; set; }
        public DateTime ResponseReceivedOn { get; set; }

        [StringLength(200)]
        public string ResponseMessage { get; set; }

        [Required(ErrorMessage = "TransactionFinalStatusType is required..!")]
        public TransactionFinalStatusTypeEnum TransactionFinalStatusType { get; set; }

        [Required(ErrorMessage = "AmountCalculated is required..!")]
        public decimal AmountCalculated { get; set; }

        [Required]
        public int PaymentBatchCounter { get; set; } = 1;

        [Required]
        public int PaymentPartCounter { get; set; }

        public string BankTransactionRefNumber1 { get; set; }
        public BankTransactionReferenceTypeEnum BankTransactionRefNumber1_Type { get; set; }
        public string BankTransactionRefNumber2 { get; set; }
        public BankTransactionReferenceTypeEnum BankTransactionRefNumber2_Type { get; set; }
        public DateTime BankSettlementOn { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }
        public virtual AppPaymentSuccessTransactionMapping AppPaymentSuccessTransactionMapping { get; set; }

    }
    public class AppFeeTransactionDetail
    {

        [Key, Required]
        public Int64 AppFeeTransactionDetailId { get; set; }
    }
    public class AppPaymentSuccessTransactionMapping
    {
        [Key, Required]
        public Int64 AppPaymentSuccessTransactionMappingId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required]
        public int PaymentBatchCounter { get; set; } = 1;

        [Required]
        public int PaymentPartCounter { get; set; }

        [Required(ErrorMessage = "AppFeeTransaction ref id is required..!")]
        [ForeignKey("AppFeeTransaction")]
        public Int64 AppFeeTransactionRefId { get; set; }
        public virtual AppFeeTransaction AppFeeTransaction { get; set; }

    }
    public class AppPaymentPart
    {
        [Key, Required]
        public Int64 AppPaymentPartId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }
        public int PaymentBatchCounter { get; set; }
        public int PaymentPartCounter { get; set; }
        public TransactionFinalStatusTypeEnum TransactionFinalStatusType { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
    public class AppPaymentEDCAuthority
    {
        [Key, Required]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Title is required..!")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "BankName is required..!")]
        [StringLength(500)]
        public string BankName { get; set; }

        [Required(ErrorMessage = "BankAccountHolderName is required..!")]
        [StringLength(100)]
        public string BankAccountHolderName { get; set; }

        [Required(ErrorMessage = "BankAccountNumber is required..!")]
        [StringLength(20)]
        public string BankAccountNumber { get; set; }

        [Required(ErrorMessage = "BankIFSC is required..!")]
        [StringLength(20)]
        public string BankIFSC { get; set; }

        [Required(ErrorMessage = "NonTreasuryCode is required..!")]
        [StringLength(20)]
        public string NonTreasuryCode { get; set; }
    }
    public class Payments_RaisedFee
    {
        [Key]
        public Int64 PaymentRaisedFeeId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        [Required(ErrorMessage = "Fee Header id is required..!")]
        [ForeignKey("AppFeesHeader")]
        public Int64 FeeHeaderRefId { get; set; }

        [Required(ErrorMessage = "Payment Batch Counter is required..!")]
        public int PaymentBatchCounter { get; set; } = 1;

        [Required(ErrorMessage = "Amount raised is required..!")]
        public decimal AmountRaised { get; set; }

        public decimal? AmountAlreadyPaid { get; set; }

        [Required(ErrorMessage = "IsFeeApplicable is required..!")]
        public bool IsFeeApplicable { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Createddate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        public virtual Application Application { get; set; }
        public virtual FeesHeader FeesHeader { get; set; }

        [NotMapped]
        public decimal? AmountPayable { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public bool IsForVerification { get; set; }

        [NotMapped]
        public bool HasDedicatedTreasuryCode { get; set; }

        [NotMapped]
        public string DedicatedTreasurCode { get; set; }

        [NotMapped]
        public string DedicatedDDOCode { get; set; }

        [Required(ErrorMessage = "ApplicationActionLogId is required..!")]
        public Int64 ApplicationActionLogId { get; set; }

        [StringLength(20)]
        public string NonTreasuryCode { get; set; }

    }
    public class Payments_RaisedFee_Log
    {
        [Key]
        public Int64 PaymentRaisedFeeLogId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }

        [Required(ErrorMessage = "Fee Header id is required..!")]
        [ForeignKey("AppFeesHeader")]
        public Int64 FeeHeaderRefId { get; set; }

        [Required(ErrorMessage = "Payment Batch Counter is required..!")]
        public int PaymentBatchCounter { get; set; } = 1;

        [Required(ErrorMessage = "Amount raised is required..!")]
        public decimal AmountRaised { get; set; }

        public decimal? AmountAlreadyPaid { get; set; }

        [Required(ErrorMessage = "IsFeeApplicable is required..!")]
        public bool IsFeeApplicable { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Createddate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        public virtual Application Application { get; set; }
        public virtual FeesHeader FeesHeader { get; set; }

        [Required(ErrorMessage = "ApplicationActionLogId is required..!")]
        public Int64 ApplicationActionLogId { get; set; }

        [StringLength(20)]
        public string NonTreasuryCode { get; set; }
    }

    public class ApplicationAndFeeRaiseAllowedMapping
    {
        [Key, Required]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "ApplicationType is required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [Required(ErrorMessage = "ApplicationPurposeType is required..!")]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [Required(ErrorMessage = "Fee Header id is required..!")]
        public Int64 FeeHeaderRefId { get; set; }

        [Required(ErrorMessage = "PrePayable is required..!")]
        public bool IsPrePayable { get; set; }

        [Required(ErrorMessage = "IsActive is required..!")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Sub-Description is required..!")]
        public string Description { get; set; }
    }
}
