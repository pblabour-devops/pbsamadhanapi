using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Licence_Proposed_BuildingPlan_GeneralDetail
    {
        [Key]
        public Int64 ProposedBuildingPlanId { get; set; }

        public int CompetentPersonListId { get; set; }
        public decimal BuildingCost { get; set; }

        public bool IsBuildingConstructedBefore_01_Oct_2008 { get; set; }

        public string DispatchNo { get; set; }

        public DateTime DispatchDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime CompetentPersonVisitDate { get; set; }

        public int BOCW_NoOfWorkers { get; set; }

        #region Architect & Competent Person Details

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
    }

    public class Licence_Existing_BuildingPlan_GeneralDetail
    {
        [Key]
        public Int64 ExistingBuildingPlanId { get; set; }

        public int CompetentPersonListId { get; set; }
        public decimal BuildingCost { get; set; }

        public bool IsBuildingConstructedBefore_01_Oct_2008 { get; set; }

        public string DispatchNo { get; set; }

        public DateTime DispatchDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime CompetentPersonVisitDate { get; set; }

        public int BOCW_NoOfWorkers { get; set; }

        #region Architect & Competent Person Details

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

        //[NotMapped]
        //public List<OfficerDetailsByRoleNameViewModel> EmpaneledArchitectsList { get; set; }

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

    public class Licence_Addition_Amendment_BuildingPlan_GeneralDetail
    {
        [Key]
        public Int64 Addition_AmendmentBuildingPlanId { get; set; }

        public ExistingBuildingPlanTypeEnum ExistingBuildingPlanType { get; set; }

        public int CompetentPersonListId { get; set; }
        public decimal BuildingCost { get; set; }

        public bool IsBuildingConstructedBefore_01_Oct_2008 { get; set; }

        public string DispatchNo { get; set; }

        public DateTime DispatchDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime CompetentPersonVisitDate { get; set; }

        public int BOCW_NoOfWorkers { get; set; }

        #region Architect & Competent Person Details

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

        //[NotMapped]
        //public List<OfficerDetailsByRoleNameViewModel> EmpaneledArchitectsList { get; set; }

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
