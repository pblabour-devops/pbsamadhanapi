using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class CommonLicence_GeneralDetail
    {
        [Key, Required]
        public Int64 CommonLicenceId { get; set; }

        [NotMapped]
        public bool IsFactory { get; set; }

        [NotMapped]
        public bool IsEngagementOfContractor { get; set; }

        [NotMapped]
        public bool IsBeediAndCigar { get; set; }

        #region Occupier OR Principal Employer Detail

        [Required(ErrorMessage = "Name of Occupier Or PE is Required..!"), StringLength(50, ErrorMessage = "The max length of Name of Occupier Or PE is 50 characters..!")]
        public string OccupierOrPE_Name { get; set; }

        #region Permanent Address 

        [Required(ErrorMessage = "Employer address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Employer address is 500 characters..!")]
        public string OccupierOrPE_Permanent_Address { get; set; }

        [Required(ErrorMessage = "Occupier Or PE Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Occupier Or PE Village or Town is 100 characters..!")]
        public string OccupierOrPE_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Occupier Or PE Tehsil is required..!")]
        [ForeignKey("Employer_TehsilLgd")]
        public Int64 OccupierOrPE_TehsilRefId { get; set; }
        public virtual TehsilLgd OccupierOrPE_TehsilLgd { get; set; }

        [Required(ErrorMessage = "Occupier Or PE District is required..!")]
        [ForeignKey("Employer_DistrictLgd")]
        public Int64 OccupierOrPE_DistrictRefId { get; set; }
        public virtual DistrictLgd OccupierOrPE_DistrictLgd { get; set; }

        [Required(ErrorMessage = "Employer Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string OccupierOrPE_PinCode { get; set; }

        #endregion Permanent Address

        [Required(ErrorMessage = "Employer email is required..!"), EmailAddress(ErrorMessage = "Invalid occupier or PE email address..!"), StringLength(50, ErrorMessage = "The max length of Employer email is 50 characters..!")]
        public string OccupierOrPE_Email { get; set; }

        [Required(ErrorMessage = "Employer phone number is required..!"), StringLength(10, ErrorMessage = "Invalid Occupier Or PE phone number..!")]
        public string OccupierOrPE_PhoneNumber { get; set; }

        #region Local Address

        [Required(ErrorMessage = "Occupier Or PE Local Address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Employer address is 500 characters..!")]
        public string OccupierOrPE_Local_Address { get; set; }

        [Required(ErrorMessage = "Occupier Or PE local Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Occupier Or PE local Village or Town is 100 characters..!")]
        public string OccupierOrPE_Local_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Occupier Or PE local Tehsil is required..!")]
        [ForeignKey("OccupierOrPE_Local_TehsilLgd")]
        public Int64 OccupierOrPE_Local_TehsilRefId { get; set; }
        public virtual TehsilLgd OccupierOrPE_Local_TehsilLgd { get; set; }

        [Required(ErrorMessage = "Occupier Or PE local District is required..!")]
        [ForeignKey("OccupierOrPE_Local_DistrictLgd")]
        public Int64 OccupierOrPE_Local_DistrictRefId { get; set; }
        public virtual DistrictLgd OccupierOrPE_Local_DistrictLgd { get; set; }

        [Required(ErrorMessage = "occupier or PE local Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string OccupierOrPE_Local_PinCode { get; set; }

        #endregion Local Address

        #endregion Occupier OR Principal Employer Detail

        #region Owner Detail

        [Required(ErrorMessage = "Name of owner is Required..!"), StringLength(50, ErrorMessage = "The max length of Name of owner is 50 characters..!")]
        public string Owner_Name { get; set; }

        [Required(ErrorMessage = "Owner partnership share is required..!")]
        public string Owner_PartnershipShare { get; set; }

        [Required(ErrorMessage = "Owner address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of owner address is 500 characters..!")]
        public string Owner_Address { get; set; }

        [Required(ErrorMessage = "Owner Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Owner Village or Town is 100 characters..!")]
        public string Owner_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Owner Tehsil is required..!")]
        [ForeignKey("Owner_TehsilLgd")]
        public Int64 Owner_TehsilRefId { get; set; }
        public virtual TehsilLgd Owner_TehsilLgd { get; set; }

        [Required(ErrorMessage = "Owner District is required..!")]
        [ForeignKey("Owner_DistrictLgd")]
        public Int64 Owner_DistrictRefId { get; set; }
        public virtual DistrictLgd Owner_DistrictLgd { get; set; }

        [Required(ErrorMessage = "Owner Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string Owner_PinCode { get; set; }

        #endregion Owner Detail

        [Required(ErrorMessage = "Core activity is required..!")]
        [StringLength(100, ErrorMessage = "The max length of core activity is 100 characters..!")]
        public string CoreActivity { get; set; }
        public string NationalIndustrialClassificationCode { get; set; }

        [Required(ErrorMessage = "Total number of worker to be employed in licence is required..!")]
        public int TotalNoWorkersToBeEmployedInLicence { get; set; }

        [Required(ErrorMessage = "Total number of employees to be employed during last year is required..!")]
        public int TotalNoWorkersToBeEmployedDuringLastYear { get; set; }

        [Required(ErrorMessage = "Electric load connected (In killowatts) is required..!")]
        public int ElectricLoadConnectedInKilowatts { get; set; }

        #region Building Plan

        [Required(ErrorMessage = "Building plan number is required..!"), StringLength(15, ErrorMessage = "Invalid Building plan number..!")]
        public string ApprovedBuildingPlanNumber { get; set; }

        [Required(ErrorMessage = "Date of approval of building plan is required..!")]
        public DateTime ApprovedBuildingPlanDate { get; set; }

        [Required(ErrorMessage = "Stability certificate number is required..!"), StringLength(15, ErrorMessage = "Invalid Stability certificate number..!")]
        public string StabilityCertificateNumber { get; set; }

        [Required(ErrorMessage = "Date of approval of stability certificate is required..!")]
        public DateTime DateOfStabilityCertificateApproval { get; set; }

        [Required(ErrorMessage = "Disposal Of Trade is required..!")]
        public string DisposalOfTrade { get; set; }

        #endregion Building Plan

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        #region Foreign Key Relation

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "EstablishmentRefId is required..!")]
        [ForeignKey("Establishment_GeneralDetail")]
        public Int64 EstablishmentRefId { get; set; }
        public virtual CommonLicence_ContractorDetail CommonLicence_ContractorDetail { get; set; }
        public virtual CommonLicences_SelectedLicenceMapping CommonLicences_SelectedLicenceMapping { get; set; }
        public virtual Establishment_GeneralDetail Establishment_GeneralDetail { get; set; }

        #endregion Foreign Key Relation

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
    }
    public class CommonLicence_ContractorDetail
    {
        [Key]
        public Int64 CommonLicence_ContractorDetailId { get; set; }

        [Required(ErrorMessage = "Nature of work of contract labour is required..!"), StringLength(100, ErrorMessage = "The max length of nature of work of contract labour is 100 characters..!")]
        public string NatureOfWorkContractLabour { get; set; }

        [Required(ErrorMessage = "Number of contract labour to be engaged is required..!")]
        public int NumberOfContractLabourToBeEmployed { get; set; }

        [Required(ErrorMessage = "Date of commencement of Each Contract Work Under Each Contractor is required..!")]
        public DateTime DateOfCommencementOfEachContractWorkUnderEachContractor { get; set; }

        [Required(ErrorMessage = "Date Of Termination Of Employement Under Each Contractor is required..!")]
        public DateTime DateOfTerminationOfEmployementUnderEachContractor { get; set; }

        [Required(ErrorMessage = "CommonLicenceRefId is required..!")]
        [ForeignKey("CommonLicence_GeneralDetail")]
        public Int64 CommonLicenceRefId { get; set; }
        public virtual CommonLicence_GeneralDetail CommonLicence_GeneralDetail { get; set; }
    }
    public class CommonLicences_SelectedLicenceMapping
    {
        [Key, Required]
        public Int64 CommonLicence_SelectedLicenceMappingId { get; set; }

        [Required(ErrorMessage = "IsFactory is required..!")]
        public bool IsFactory { get; set; }

        [Required(ErrorMessage = "IsPrincipalEmployer is required..!")]
        public bool IsPrincipalEmployer { get; set; }

        [Required(ErrorMessage = "IsBeediOrCigar is required..!")]
        public bool IsBeediOrCigar { get; set; }

        [Required(ErrorMessage = "CommonLicenceRefId is required..!")]
        [ForeignKey("CommonLicence_GeneralDetail")]
        public Int64 CommonLicenceRefId { get; set; }
        public virtual CommonLicence_GeneralDetail CommonLicence_GeneralDetail { get; set; }
    }
}
