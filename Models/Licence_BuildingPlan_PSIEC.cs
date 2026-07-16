using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Licence_BuildingPlan_PSIEC_GeneralDetail
    {
        [Key]
        public Int64 PsiecBuildingPlanId { get; set; }

        [Required(ErrorMessage = "IsUnderRightToBusinessAct is required..!")]
        public string IsUnderRightToBusinessAct { get; set; }

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

        #region PSIEC Column

        [Required(ErrorMessage = "Name is required..!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Registration no is required..!")]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "Plot no is required..!")]
        public string PlotNo { get; set; }

        [Required(ErrorMessage = "PSIECPhaseType is required..!")]
        public PSIECPhaseTypeEnum PSIECPhaseType { get; set; }

        #endregion

        #region Site Details

        [Required(ErrorMessage = "PlotAreaSqFt is required..!")]
        public decimal PlotAreaSqFt { get; set; }

        [Required(ErrorMessage = "PlotAreaSqYards is required..!")]
        public decimal PlotAreaSqYards { get; set; }

        #endregion

        #region Not Mapped Column

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [NotMapped]
        public Int64 IPin { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        [NotMapped]
        public Int64 FactoryCircleRefId { get; set; }

        [NotMapped]
        public Int64 DistrictRefId { get; set; }

        [NotMapped]
        public List<OfficerDetailsByRoleNameViewModel> CompetentPersonList { get; set; }

        [NotMapped]
        public List<OfficerDetailsByRoleNameViewModel> EmpaneledArchitectsList { get; set; }

        [NotMapped]
        public List<OfficerDetailsByRoleNameViewModel> EmpaneledEngineersList { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }

        #endregion

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        #endregion
    }
}
