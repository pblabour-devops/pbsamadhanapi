using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Establishment_GeneralDetail
    {
        [Key, Required]
        public Int64 EstablishmentId { get; set; }

        //[Required(ErrorMessage = "Establishment name is required..!")]
        //[StringLength(100, ErrorMessage = "The max length of establishment name is 100 characters..!")]
        //public string EstablishmentName { get; set; }

        #region EstablishmentAddess

        //[Required(ErrorMessage = "Establishment address is required..!")]
        //[StringLength(500, ErrorMessage = "The max length of establishment address is 500 characters..!")]
        //public string Estb_Address { get; set; }

        //[Required(ErrorMessage = "Establishment Village or Town name is required..!")]
        //[StringLength(100, ErrorMessage = "The max length of Establishment Village or Town is 100 characters..!")]
        //public string Estb_VillageOrTown { get; set; }


        //[Required(ErrorMessage = "Establishment Tehsil is required..!")]
        //[ForeignKey("Estb_TehsilLgd")]
        //public Int64 Estb_TehsilRefId { get; set; }
        //public virtual TehsilLgd Estb_TehsilLgd { get; set; }


        //[Required(ErrorMessage = "Establishment District is required..!")]
        //[ForeignKey("Estb_DistrictLgd")]
        //public Int64 Estb_DistrictRefId { get; set; }
        //public virtual DistrictLgd Estb_DistrictLgd { get; set; }

        //[Required(ErrorMessage = "Establishment Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        //public string Estb_PinCode { get; set; }

        [Required(ErrorMessage = "Gst Number is required..!"), StringLength(15, ErrorMessage = "Invalid Gst_Number..!")]
        public string GstNumber { get; set; }

        #endregion EstablishmentAddess


        #region CommunicationAddess

        [Required(ErrorMessage = "Communication address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of communication address is 500 characters..!")]
        public string Comm_Address { get; set; }

        [Required(ErrorMessage = "Communication Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Communication Village or Town is 100 characters..!")]
        public string Comm_VillageOrTown { get; set; }


        [Required(ErrorMessage = "Communication Tehsil is required..!")]
        [ForeignKey("Comm_TehsilLgd")]
        public Int64 Comm_TehsilRefId { get; set; }
        public virtual TehsilLgd Comm_TehsilLgd { get; set; }


        [Required(ErrorMessage = "Communication District is required..!")]
        [ForeignKey("Comm_DistrictLgd")]
        public Int64 Comm_DistrictRefId { get; set; }
        public virtual DistrictLgd Comm_DistrictLgd { get; set; }


        [Required(ErrorMessage = "Communication Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string Comm_PinCode { get; set; }

        #endregion CommunicationAddess

        [Required(ErrorMessage = "Establishment type is Required..!")]
        public EstablishmentTypeEnum EstablishmentType { get; set; }

        [Required(ErrorMessage = "Labour identification number is Required..!"), StringLength(20, ErrorMessage = "The max length of Labour Identification number is 20 characters..!")]
        public string LabourIdentificationNum { get; set; }

        [Required(ErrorMessage = "Electric load connected (In killowatts) is required..!")]
        public int ElectricLoadConnectedInKilowatts { get; set; }

        [Required(ErrorMessage = "Consitution of establishment is required..!")]
        public EstablishmentConstitutionTypeEnum EstablishmentConstitutionType { get; set; }

        [Required(ErrorMessage = "Building in which establishment situated is required..!")]
        public EstablishmentBuildingTypeEnum EstablishmentBuildingType { get; set; }

        [Required(ErrorMessage = "Establishment employing or will employ inter state migrant workers is required..!")]
        public bool IsEmployingInterStateMigrantWorkers { get; set; }

        [Required(ErrorMessage = "National industrial classification code is required..!"), StringLength(50, ErrorMessage = "The max length of Employer email is 50 characters..!")]
        public string NationalIndustrialClassificationCode { get; set; }
        
        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        public virtual Establishment_EmployerDetail Establishment_EmployerDetail { get; set; }
        public virtual ICollection<Establishment_ContractorDetail> Establishment_ContractorsDetail { get; set; }
        public virtual ICollection<Establishment_Migrantworker> Establishment_Migrantworkers { get; set; }
        public virtual ICollection<CommonLicence_GeneralDetail> CommonLicence_GeneralDetail { get; set; }
        public virtual ICollection<RegistrationDetail> RegistrationDetail { get; set; }
        public virtual ICollection<ContractLabourAndEstablishmentMapping> ContractLabourAndEstablishmentMapping { get; set; }
        public virtual ICollection<BuildingPlan> BuildingPlan { get; set; }

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }
        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
    }
    public class Establishment_EmployerDetail
    {
        [Key]
        public Int64 Establishment_EmployerDetailId { get; set; }

        #region EmployerDetail

        [Required(ErrorMessage = "Name of employer is Required..!"), StringLength(50, ErrorMessage = "The max length of Name of employer is 50 characters..!")]
        public string Employer_Name { get; set; }

        [Required(ErrorMessage = "Employer address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Employer address is 500 characters..!")]
        public string Employer_Address { get; set; }

        [Required(ErrorMessage = "Employer Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Employer Village or Town is 100 characters..!")]
        public string Employer_VillageOrTown { get; set; }


        [Required(ErrorMessage = "Employer Tehsil is required..!")]
        [ForeignKey("Employer_TehsilLgd")]
        public Int64 Employer_TehsilRefId { get; set; }
        public virtual TehsilLgd Employer_TehsilLgd { get; set; }


        [Required(ErrorMessage = "Employer District is required..!")]
        [ForeignKey("Employer_DistrictLgd")]
        public Int64 Employer_DistrictRefId { get; set; }
        public virtual DistrictLgd Employer_DistrictLgd { get; set; }


        [Required(ErrorMessage = "Employer Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string Employer_PinCode { get; set; }

        [Required(ErrorMessage = "Employer email is required..!"), EmailAddress(ErrorMessage = "Invalid employer email address..!"), StringLength(50, ErrorMessage = "The max length of Employer email is 50 characters..!")]
        public string Employer_Email { get; set; }

        [Required(ErrorMessage = "Employer phone is required..!"), StringLength(10, ErrorMessage = "Invalid employer phone number..!")]
        public string Employer_Phone { get; set; }

        #endregion EmployerDetail

        [Required(ErrorMessage = "Maximum number of employees to be employed on any day during the year is required..!")]
        public int MaxEmployeesToBeEmployedAnyDay { get; set; }

        [Required(ErrorMessage = "Maximum number of employees were employed on any day during the last twelve months is required..!")]
        public int MaxEmployeesWereEmployedAnyDay { get; set; }

        [Required(ErrorMessage = "Date of commencement of activity in the establishment is required..!")]
        public DateTime DateOfCommencementOfActivityInEstb { get; set; }



        [Required(ErrorMessage = "EstablishmentRefId is required..!")]
        [ForeignKey("Establishment_GeneralDetail")]
        public Int64 EstablishmentRefId { get; set; }
        public virtual Establishment_GeneralDetail Establishment_GeneralDetail { get; set; }
    }
    public class Establishment_ContractorDetail
    {
        #region ContractorDetail
        [Key]
        public Int64 Establishment_ContractorDetailId { get; set; }

        [Required(ErrorMessage = "Name of contractor is Required..!")]
        [StringLength(50, ErrorMessage = "The max length of Name of contractor is 50 characters..!")]
        public string Contractor_Name { get; set; }

        [Required(ErrorMessage = "Contractor address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Contractor address is 500 characters..!")]
        public string Contractor_Address { get; set; }

        [Required(ErrorMessage = "Contractor Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of contractor Village or Town is 100 characters..!")]
        public string Contractor_VillageOrTown { get; set; }


        [Required(ErrorMessage = "Contractor Tehsil is required..!")]
        [ForeignKey("Contractor_TehsilLgd")]
        public Int64 Contractor_TehsilRefId { get; set; }
        public virtual TehsilLgd Contractor_TehsilLgd { get; set; }


        [Required(ErrorMessage = "Contractor District is required..!")]
        [ForeignKey("Contractor_DistrictLgd")]
        public Int64 Contractor_DistrictRefId { get; set; }
        public virtual DistrictLgd Contractor_DistrictLgd { get; set; }


        [Required(ErrorMessage = "Contractor Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string Contractor_PinCode { get; set; }

        [Required(ErrorMessage = "Contractor email is required..!"), EmailAddress(ErrorMessage = "Invalid contractor email address..!"), StringLength(50, ErrorMessage = "The max length of contractor email is 50 characters..!")]
        public string Contractor_Email { get; set; }

        [Required(ErrorMessage = "Contractor phone is required..!"), StringLength(10, ErrorMessage = "Invalid contractor phone number..!")]
        public string Contractor_Phone { get; set; }

        [Required(ErrorMessage = "Number of contract labour to be engaged is required..!")]
        public int NumberOfContractLabourToBeEmployed { get; set; }

        [Required(ErrorMessage = "Nature of work of contract labour is required..!"), StringLength(100, ErrorMessage = "The max length of nature of work of contract labour is 100 characters..!")]
        public string NatureOfWorkContractLabour { get; set; }

        [Required(ErrorMessage = "EstablishmentRefId is required..!")]
        [ForeignKey("Establishment_GeneralDetail")]
        public Int64 EstablishmentRefId { get; set; }
        public virtual Establishment_GeneralDetail Establishment_GeneralDetail { get; set; }

        #endregion ContractorDetail
    }
    public class Establishment_Migrantworker
    {
        [Key]
        public Int64 EstablishmentMigrantworkerId { get; set; }

        [Required(ErrorMessage = "Name of worker is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of Name of contractor is 100 characters..!")]
        public string Worker_Name { get; set; }

        [Required(ErrorMessage = "Father name of contractor is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of father name of contractor is 100 characters..!")]
        public string Worker_Father_Husband_Name { get; set; }

        [Required(ErrorMessage = "Worker address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of worker address is 500 characters..!")]
        public string Worker_Permanent_Address { get; set; }

        [Required(ErrorMessage = "Worker Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of worker Village or Town is 100 characters..!")]
        public string Worker_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Worker Tehsil is required..!")]
        [ForeignKey("Worker_TehsileLgd")]
        public string Worker_Tehsil { get; set; }

        [Required(ErrorMessage = "Worker District is required..!")]
        [ForeignKey("Worker_DistrictLgd")]
        public string Worker_District { get; set; }

        [Required(ErrorMessage = "Name of contractor is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of Name of contractor is 50 characters..!")]
        public string Worker_State { get; set; }

        [Required(ErrorMessage = "Name of contractor is Required..!")]
        [StringLength(12, ErrorMessage = "Invalid worker aadhar number..!")]
        public string Worker_Aadhar_Number { get; set; }

        [Required(ErrorMessage = "Name of contractor is Required..!")]
        [StringLength(10, ErrorMessage = "Invalid worker phone number..!")]
        public string Worker_Mobile_Number { get; set; }

        [Required(ErrorMessage = "EstablishmentRefId is required..!")]
        [ForeignKey("Establishment_GeneralDetail")]
        public Int64 EstablishmentRefId { get; set; }
        public virtual Establishment_GeneralDetail Establishment_GeneralDetail { get; set; }
    }
    public class IntReturn
    {
        public int Value { get; set; }
    }
}
