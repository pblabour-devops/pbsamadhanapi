using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class BuildingPlanFactory_GeneralDetail
    {
        [Key]
        public Int64 BuildingPlanFactoryId { get; set; }

        [Required(ErrorMessage = "BuildingCost is required..!")]
        public decimal BuildingCost { get; set; }

        [Required(ErrorMessage = "Owner name is required..!")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Owner contact no is required..!")]
        public string OwnerContactNo { get; set; }

        [Required(ErrorMessage = "Owner email is required..!")]
        public string OwnerEmail { get; set; }

        [Required(ErrorMessage = "Architect or competent person name is required..!")]
        public string ArchitectOrCompetentPersonName { get; set; }

        [Required(ErrorMessage = "Architect or competent person contact no is required..!")]
        public string ArchitectOrCompetentPersonContactNo { get; set; }

        [Required(ErrorMessage = "Architect or competent person email is required..!")]
        public string ArchitectOrCompetentPersonEmail { get; set; }
        public string DispatchNo { get; set; }
        public string IssueDate { get; set; }

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
        public int ProjectSiteVersion { get; set; }
        #endregion
    }

    public class BuildingPlanFactory_Declaration_Stability_Certificate
    {
        [Key]
        public Int64 DeclarationStabilityCertificateId { get; set; }

        [Required(ErrorMessage = "Is Building Plan Approved is required..!")]
        public int IsBuildingPlanApproved { get; set; }

        [Required(ErrorMessage = "Building Plan Dof Number is required..!")]
        public string BuildingPlanDofNumber { get; set; }
        public DateTime? BuildingPlanDofApprovalDate { get; set; }

        [Required(ErrorMessage = "Is Building Plan Verified is required..!")]
        public bool IsBuildingPlanVerified { get; set; }

        [Required(ErrorMessage = "Is Stability Approved is required..!")]
        public int IsStabilityApproved { get; set; }

        [Required(ErrorMessage = "Stability Dof Number is required..!")]
        public string StabilityPlanDofNumber { get; set; }
        public DateTime? StabilityPlanDofApprovalDate { get; set; }

        [Required(ErrorMessage = "Is Stability Verified is required..!")]
        public bool IsStabilityPlanVerified { get; set; }
        public string CompetentPersonUserId { get; set; }

        [Required(ErrorMessage = "Competent Person name is required..!")]
        public string CompetentPersonName { get; set; }

        [Required(ErrorMessage = "Competent Person contact no is required..!")]
        public string CompetentPersonContactNo { get; set; }

        [Required(ErrorMessage = "Competent Person email is required..!")]
        public string CompetentPersonEmail { get; set; }

        [Required(ErrorMessage = "IsCompetentPersonSubmittedAnyChanges is required..!")]
        public string IsCompetentPersonSubmittedAnyChanges { get; set; }

        [Required(ErrorMessage = "Engineer name is required..!")]
        public string EngineerName { get; set; }

        [Required(ErrorMessage = "Engineer contact no is required..!")]
        public string EngineerContactNo { get; set; }

        [Required(ErrorMessage = "Engineer email is required..!")]
        public string EngineerEmail { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        #endregion

        #region Not Mapped Column

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

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
        public List<OfficerDetailsByRoleNameViewModel> EmpaneledEngineersList { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
        #endregion
    }


    public class BuildingPlanFactory_Declaration_Stability_Randomization
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Month is required..!")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required..!")]
        public int Year { get; set; }

        [Required(ErrorMessage = "CircleId is required..!")]
        public Int64 CircleId { get; set; }

        [Required(ErrorMessage = "Randomization Date is required..!")]
        public DateTime RandomizationDate { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        #endregion
    }
}
