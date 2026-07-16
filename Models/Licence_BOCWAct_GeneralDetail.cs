using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace pbsamadhannetcoreapi.Models
{
    public class Licence_BocwAct_GeneralDetail
    {
        [Key, Required]
        public Int64 BocwEstablishmentRegistrationId { get; set; }

        [Required(ErrorMessage = "BOCW Act Circle Type is required..!")]
        public BOCWActCircleTypeEnum BOCWActCircleType { get; set; }

        [Required(ErrorMessage = "Construction Building Type is required..!")]
        public ConstructionBuildingTypeEnum ConstructionBuildingType { get; set; }

        [Required(ErrorMessage = "Construction Building Desc is required..!")]
        [StringLength(250, ErrorMessage = "Construction Building Desc max size is 250")]
        public string ConstructionBuildingDesc { get; set; }

        [Required(ErrorMessage = "Bocw Engaged Worker Slab Type is required..!")]
        public BocwEngagedWorkerSlabTypeEnum BocwEngagedWorkerSlabType { get; set; }

        [Required(ErrorMessage = "Bocw Registered Type is required..!")]
        public BocwRegisteredTypeEnum BocwRegisteredType { get; set; }

        [Required(ErrorMessage = "Work Commencement Date is required..!")]
        public DateTime Work_CommencementDate { get; set; }

        [Required(ErrorMessage = "Work Completion Date is required..!")]
        public DateTime Work_CompletionDate { get; set; }

        [Required(ErrorMessage = "Maximum Number of Worker is required..!")]
        public int MaximumNoOfWorkers { get; set; }

        [Required(ErrorMessage = "Labour Circle is required..!")]
        public Int64 AlcCircleRefId { get; set; }

        [Required(ErrorMessage = "Factory Circle is required..!")]
        public Int64 FactoryCircleRefId { get; set; }

        [Required(ErrorMessage = "ModifiedCounter is required..!")]
        public int ModifiedCounter { get; set; } = 1;

        #region Construction Site
        [Required(ErrorMessage = "Construction Site Name is required..!")]
        [StringLength(200, ErrorMessage = "FieldName max size is 200")]
        public string ConstructionSite_Name { get; set; }

        [Required(ErrorMessage = "Construction Site Address Name is required..!")]
        [StringLength(500, ErrorMessage = "FieldName max size is 500")]
        public string ConstructionSite_Address { get; set; }

        [Required(ErrorMessage = "Construction Site District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictLgdRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }


        [Required(ErrorMessage = "Construction Site Tehsil is required..!")]
        [ForeignKey("TehsilLgd")]
        public Int64 TehsilLgdRefId { get; set; }
        public virtual TehsilLgd TehsilLgd { get; set; }

        [Required(ErrorMessage = "Construction Site Pin is required..!")]
        public string ConstructionSite_PinCode { get; set; }

        #endregion

        #region If Principal Employer Yes
        [Required(ErrorMessage = "Principal Employer Name is required..!")]
        [StringLength(200, ErrorMessage = "FieldName max size is 200")]
        public string PrincipalEmployerName { get; set; }

        [Required(ErrorMessage = "Principal Employer Father Name is required..!")]
        [StringLength(200, ErrorMessage = "FieldName max size is 200")]
        public string PrincipalEmployerFatherName { get; set; }

        [Required(ErrorMessage = "Principal Employer Address is required..!")]
        [StringLength(500, ErrorMessage = "FieldName max size is 500")]
        public string PrincipalEmployerAddress { get; set; }

        [Required(ErrorMessage = "Principal Employer Mobile No is required..!")]
        [StringLength(10, ErrorMessage = "Mobile No max size is 10")]
        public string PrincipalEmployerMobileNo { get; set; }

        [Required(ErrorMessage = "Principal Employer Email is required..!")]
        public string PrincipalEmployerEmail { get; set; }

        [Required(ErrorMessage = "Engaged Any Contractor Type is required..!")]
        public EngagedAnyContractorTypeEnum EngagedAnyContractorType { get; set; }
        #endregion

        #region If Contractor Yes
        public string ContractLabourLicenceNumber { get; set; }

        #endregion

        #region Manager Details
        [Required(ErrorMessage = "Manager Name is required..!")]
        [StringLength(200, ErrorMessage = "FieldName max size is 200")]
        public string ManagerName { get; set; }

        [Required(ErrorMessage = "Manager Address is required..!")]
        [StringLength(500, ErrorMessage = "FieldName max size is 500")]
        public string ManagerAddress { get; set; }

        [Required(ErrorMessage = "Manager Email is required..!")]
        public string ManagerEmail { get; set; }

        [Required(ErrorMessage = "Manager Mobile is required..!")]
        [StringLength(10, ErrorMessage = "FieldName max size is 10")]
        public string ManagerMobile { get; set; }
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
        public Int64 DistrictRefId { get; set; }

        [NotMapped]
        public string ContractorDetails { get; set; }
        #endregion
        #region Foreign Key References
        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        public virtual ICollection<Licence_BocwAct_ContractorDetail> Licence_BocwAct_ContractorDetail { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
        #endregion
    }

    public class Licence_BocwAct_ContractorDetail
    {
        [Key, Required]
        public Int64 BocwEstablishmentContractorDetailId { get; set; }

        [Required(ErrorMessage = "BocwEstablishmentRegistrationRefId is required..!")]
        [ForeignKey("Contractor_GeneralDetail")]
        public Int64? BocwEstablishmentRegistrationRefId { get; set; }
        public virtual Licence_BocwAct_GeneralDetail Licence_BocwAct_GeneralDetail { get; set; }

        [Required(ErrorMessage = "Contractor Name is required..!")]
        [StringLength(200, ErrorMessage = "FieldName max size is 200")]
        public string ContractorName { get; set; }

        [Required(ErrorMessage = "Contractor Address is required..!")]
        [StringLength(500, ErrorMessage = "FieldName max size is 500")]
        public string ContractorAddress { get; set; }

        [Required(ErrorMessage = "Nature of Construction Work is required..!")]
        [StringLength(500, ErrorMessage = "FieldName max size is 500")]
        public string NatureOfConstructionWork { get; set; }

        [Required(ErrorMessage = "Maximum No of Contractor on any day is required..!")]
        public int maxNoOfContractorOnAnyDay { get; set; }

        [Required(ErrorMessage = "Commencement Date is required..!")]
        public DateTime? Contractor_Work_CommencementDate { get; set; }

        [Required(ErrorMessage = "Completion Date is required..!")]
        public DateTime? Contractor_Work_CompletionDate { get; set; }
    }

    public class Licence_Bocw_AmendmentDataHistories
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "SectionCode is required..!")]
        [StringLength(10, ErrorMessage = "SectionCode max size is 10")]
        public string SectionCode { get; set; }

        [Required(ErrorMessage = "FieldName is required..!")]
        [StringLength(50, ErrorMessage = "FieldName max size is 50")]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "PreviousValue is required..!")]
        public string PreviousValue { get; set; }

        [Required(ErrorMessage = "ModifiedValue is required..!")]
        public string ModifiedValue { get; set; }

        [Required(ErrorMessage = "ModifiedOn is required..!")]
        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = "ModifiedCounter is required..!")]
        public int ModifiedCounter { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [ForeignKey("Application")]
        [Required(ErrorMessage = "Application is required..!")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }
    }
}