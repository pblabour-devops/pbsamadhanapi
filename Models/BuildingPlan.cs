using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class BP_GeneralDetail
    {
        [Key, Required]
        public Int64 BuildingPlanId { get; set; }

        //[Required(ErrorMessage = "Application purpose type is required..!")]
        //public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [Required(ErrorMessage = "Establishment name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of establishment name is 100 characters..!")]
        public string EstablishmentName { get; set; }

        [Required(ErrorMessage = "Establishment address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of establishment address is 500 characters..!")]
        public string Establishment_Address { get; set; }

        [Required(ErrorMessage = "Is building contructed before is required..!")]
        public YesNoEnum IsBuildingContructedBefore_01_10_2008 { get; set; }

        [Required(ErrorMessage = "No Of Workers is required..!")]
        public int NoOfWorkers { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }
       
    }
    public class BuildingPlan
    {
        [Key, Required]
        public Int64 BuildingPlanId { get; set; }

        [Required(ErrorMessage = "Applicant name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of applicant name is 100 characters..!")]
        public string ApplicantName { get; set; }

        #region ApplicantAddess

        [Required(ErrorMessage = "Applicant address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of applicant address is 500 characters..!")]
        public string Applicant_Address { get; set; }

        [Required(ErrorMessage = "Applicant Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Applicant Village or Town is 100 characters..!")]
        public string Applicant_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Applicant Tehsil is required..!")]
        [ForeignKey("Applicant_TehsilLgd")]
        public Int64 Applicant_TehsilRefId { get; set; }
        public virtual TehsilLgd Applicant_TehsilLgd { get; set; }

        [Required(ErrorMessage = "Applicant District is required..!")]
        [ForeignKey("Applicant_DistrictLgd")]
        public Int64 Applicant_DistrictRefId { get; set; }
        public virtual DistrictLgd Applicant_DistrictLgd { get; set; }

        [Required(ErrorMessage = "Applicant Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string Applicant_PinCode { get; set; }

        #endregion ApplicantAddess

        [Required(ErrorMessage = "Applicant relation to factory is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Applicant relation to factory is 100 characters..!")]
        public string ApplicantRelationToFactory { get; set; }

        [Required(ErrorMessage = "Name of factory is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Name of factory is 200 characters..!")]
        public string NameOfFactory { get; set; }

        //#region Factory Addess

        //[Required(ErrorMessage = "Factory Village or Town name is required..!")]
        //[StringLength(100, ErrorMessage = "The max length of Factory Village or Town is 100 characters..!")]
        //public string Factory_VillageOrTown { get; set; }

        //[Required(ErrorMessage = "Factory Tehsil is required..!")]
        //[ForeignKey("Factory_TehsilLgd")]
        //public Int64 Factory_TehsilRefId { get; set; }
        //public virtual TehsilLgd Factory_TehsilLgd { get; set; }

        //[Required(ErrorMessage = "Factory District is required..!")]
        //[ForeignKey("Factory_DistrictLgd")]
        //public Int64 Factory_DistrictRefId { get; set; }
        //public virtual DistrictLgd Factory_DistrictLgd { get; set; }

        //[Required(ErrorMessage = "Factory Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        //public string Factory_PinCode { get; set; }

        //#endregion Factory Addess

        [StringLength(100, ErrorMessage = "The max length of khasra number or Town is 100 characters..!")]
        public string KhasraNumber { get; set; }

        [StringLength(100, ErrorMessage = "The max length of ward number or Town is 100 characters..!")]
        public string WardNumber { get; set; }

        [StringLength(100, ErrorMessage = "The max length of plot number or Town is 100 characters..!")]
        public string PlotNumber { get; set; }

        [StringLength(100, ErrorMessage = "The max length of floor number or Town is 100 characters..!")]
        public string FloorNumber { get; set; }

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "EstablishmentRefId is required..!")]
        [ForeignKey("Establishment_GeneralDetail")]
        public Int64 EstablishmentRefId { get; set; }
        public virtual ICollection<BuildingPlan_AreaDetail> BuildingPlan_AreaDetail { get; set; }
        public virtual Establishment_GeneralDetail Establishment_GeneralDetail { get; set; }

        #endregion Foreign Key References

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
    }
    public class BuildingPlan_AreaDetail
    {
        [Key]
        public Int64 BuildingPlan_AreaDetailId { get; set; }

        [Required(ErrorMessage = "Number of rooms is required..!")]
        public int NumberOfRooms { get; set; }

        [Required(ErrorMessage = "Length of room is required..!")]
        public int LengthOfRoom_X_Axis { get; set; }

        [Required(ErrorMessage = "Breadth of room is required..!")]
        public int BreadthOfRoom_Y_Axis { get; set; }

        [Required(ErrorMessage = "Maximum z-axis height is required..!")]
        public int Maximum_Z_Axis_Height { get; set; }

        [Required(ErrorMessage = "Minimum z-axis height is required..!")]
        public int Minimum_Z_Axis_Height { get; set; }

        [Required(ErrorMessage = "Area is required..!")]
        public int Area { get; set; }

        [Required(ErrorMessage = "Area Occupied By Machine is required..!")]
        public int AreaOccupiedByMachine { get; set; }

        [Required(ErrorMessage = "Volume is required..!")]
        public int Volume { get; set; }

        [Required(ErrorMessage = "Breathing Space is required..!")]
        public string BreathingSpace { get; set; }

        [Required(ErrorMessage = "Ventilation is required..!")]
        public string Ventilation { get; set; }

        [Required(ErrorMessage = "LightingLevel is required..!")]
        public string LightingLevel { get; set; }

        [Required(ErrorMessage = "Maximum Capicity Of Room is required..!")]
        public int MaximumCapicityOfRoom { get; set; }

        [Required(ErrorMessage = "Number Of Persons To Employed In Room is required..!")]
        public int NumberOfPersonsToEmployedInRoom { get; set; }

        [Required(ErrorMessage = "Purpose Of Room is required..!")]
        public string PurposeOfRoom { get; set; }

        [Required(ErrorMessage = "Construction Period is required..!")]
        public string ConstructionPeriod { get; set; }

        [StringLength(500, ErrorMessage = "Remarks is 500 characters..!")]
        public string Remarks { get; set; }

        #region Foreign Key References

        [Required(ErrorMessage = "BuildingPlanRefId is required..!")]
        [ForeignKey("BuildingPlan")]
        public Int64 BuildingPlanRefId { get; set; }
        public virtual BuildingPlan BuildingPlan { get; set; }

        #endregion Foreign Key References
    }
}
