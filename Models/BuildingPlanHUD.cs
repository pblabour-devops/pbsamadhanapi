using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class BuildingPlanHUD_GeneralDetail
    {
        [Key]
        public Int64 BuildingPlanHUDId { get; set; }

        [Required(ErrorMessage = "IsUnderRightToBusinessAct is required..!")]
        public string IsUnderRightToBusinessAct { get; set; }

        [StringLength(100)]
        public string HadbustNumber { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Principal Approval Date Invalid!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string DateOfPrincipalApproval { get; set; }

        [Required(ErrorMessage = "Project Identification No is required..!")]
        [StringLength(100)]
        public string ProjectIdentificationNo { get; set; }

        [Required(ErrorMessage = "AppIdRightToBusinessAct is required..!")]
        [StringLength(100)]
        public string AppIdRightToBusinessAct { get; set; }

        [Required(ErrorMessage = "BuidingPlanHUDApprovalType is required..!")]
        public BuidingPlanHUDApprovalTypeEnum BuidingPlanHUDApprovalType { get; set; }

        [Required(ErrorMessage = "BuildingType is required..!")]
        public BuildingTypeEnum BuildingType { get; set; }

        [Required(ErrorMessage = "IsBuildingHeightMoreThen15Meter is required..!")]
        public string IsBuildingHeightMoreThen15Meter { get; set; }

        [Required(ErrorMessage = "InspectionType is required..!")]
        public InspectionTypeEnum InspectionType { get; set; }

        [Required(ErrorMessage = "PlotAreaSqFt is required..!")]
        public decimal PlotAreaSqFt { get; set; }

        [Required(ErrorMessage = "PlotAreaAcres is required..!")]
        public decimal PlotAreaAcres { get; set; }

        [Required(ErrorMessage = "Project Purpose is required..!")]
        public string ProjectPurpose { get; set; }

        [Required(ErrorMessage = "ProjectType is required..!")]
        public ProjectTypeEnum ProjectType { get; set; }

        [Required(ErrorMessage = "BuildingCost is required..!")]
        public decimal BuildingCost { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }
        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        #endregion

        #region Not Mapped Column
        [NotMapped]
        public Int64 IPin { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        [NotMapped]
        public PrincipalApproval_RBA_DetailsViewModel PrincipalApproval_RBA_Details { get; set; }

        #endregion

        [Required(ErrorMessage = "IsGasOrFuelPipeLinePassWithin150Meter is required..!")]
        public string IsGasOrFuelPipeLinePassWithin150Meter { get; set; }

        [Required(ErrorMessage = "IsHistoricalSiteIsLocatedWithin100Meter is required..!")]
        public string IsHistoricalSiteIsLocatedWithin100Meter { get; set; }

        [Required(ErrorMessage = "CoveredAreaSqFt is required..!")]
        public decimal CoveredAreaSqFt { get; set; }

        [Required(ErrorMessage = "IndustryType is required..!")]
        public IndustryColorCodeByPPCBTypeEnum IndustryType { get; set; }

        #region Owner, Architect & Competent Person Details

        [Required(ErrorMessage = "Owner name is required..!")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Owner contact no is required..!")]
        public string OwnerContactNo { get; set; }

        [Required(ErrorMessage = "Owner email is required..!")]
        public string OwnerEmail { get; set; }

        [Required(ErrorMessage = "Competent Person name is required..!")]
        public string CompetentPersonName { get; set; }

        [Required(ErrorMessage = "Competent Person contact no is required..!")]
        public string CompetentPersonContactNo { get; set; }

        [Required(ErrorMessage = "Competent Person email is required..!")]
        public string CompetentPersonEmail { get; set; }

        [Required(ErrorMessage = "Architect name is required..!")]
        public string ArchitectName { get; set; }

        [Required(ErrorMessage = "Architect contact no is required..!")]
        public string ArchitectContactNo { get; set; }

        [Required(ErrorMessage = "Architect email is required..!")]
        public string ArchitectEmail { get; set; }

        [Required(ErrorMessage = "Engineer name is required..!")]
        public string EngineerName { get; set; }

        [Required(ErrorMessage = "Engineer contact no is required..!")]
        public string EngineerContactNo { get; set; }

        [Required(ErrorMessage = "Engineer email is required..!")]
        public string EngineerEmail { get; set; }

        [Required(ErrorMessage = "Building plan approval authority type is required..!")]
        public BuildingPlanApprovalAuthorityTypeEnum BuildingPlanApprovalAuthorityType { get; set; }

        #endregion

        [NotMapped]
        public List<OfficerDetailsByRoleNameViewModel> CompetentPersonList { get; set; }

        [NotMapped]
        public List<OfficerDetailsByRoleNameViewModel> EmpaneledArchitectsList { get; set; }

        [NotMapped]
        public List<OfficerDetailsByRoleNameViewModel> EmpaneledEngineersList { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
    }

    public class BuildingPlanHUDPaymentDetail
    {
        [Key]
        public Int64 PaymentDetailId { get; set; }

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

        //[Required(ErrorMessage = "Remarks is required..!")]
        //public string Remarks { get; set; }
    }


    public class BuildingPlanHUDPaymentDetail_log
    {
        [Key]
        public Int64 PaymentDetailLogId { get; set; }

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

        //[Required(ErrorMessage = "Remarks is required..!")]
        //public string Remarks { get; set; }
    }


    public class BuildingPlanHUD_RTB_Mapping
    {
        [Key,Required]
        public Int64 BuildingPlanHUD_RTB_MappingId { get; set; }

        [Required(ErrorMessage = "Project Identification No is required..!")]
        [StringLength(100)]
        public string ProjectIdentificationNo { get; set; }

        [Required(ErrorMessage = "AppIdRightToBusinessAct is required..!")]
        [StringLength(100)]
        public string AppIdRightToBusinessAct { get; set; }

        [Required(ErrorMessage = "ResponseJson is required..!")]
        public string ResponseJson { get; set; }

        [Required(ErrorMessage = "AppRefId is required..!")]
        public Int64 AppRefId { get; set; }
    }
}