using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    #region OSH - Rules Contract Labour
    public class Contractor_GeneralDetail
    {
        [Key, Required]
        public Int64 ContractLabourId { get; set; }

        [Required(ErrorMessage = "Contractor name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of contractor name is 100 characters..!")]
        public string ContractorName { get; set; }

        [Required(ErrorMessage = "Father name of contractor is Required..!")]
        [StringLength(100, ErrorMessage = "The max length of father name of contractor is 100 characters..!")]
        public string Father_Name { get; set; }

        [Required(ErrorMessage = "Contractor Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Establishment Village or Town is 100 characters..!")]
        public string VillageOrTown { get; set; }

        [Required(ErrorMessage = "Contractor Tehsil is required..!")]
        [ForeignKey("Contractor_TehsilLgd")]
        public Int64 TehsilRefId { get; set; }
        public virtual TehsilLgd Contractor_TehsilLgd { get; set; }

        [Required(ErrorMessage = "Contractor District is required..!")]
        [ForeignKey("Contractor_DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd Contractor_DistrictLgd { get; set; }

        [Required(ErrorMessage = "Contractor Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Nature Of Work..!")]
        [StringLength(100, ErrorMessage = "The max length of nature of work is 200 characters..!")]
        public string NatureOfWork { get; set; }

        [Required(ErrorMessage = "Date of praposed contract work start is required..!")]
        public DateTime DurationOfProposedContractWorkStart { get; set; }

        [Required(ErrorMessage = "Date of praposed contract work end is required..!")]
        public DateTime DurationOfProposedContractWorkEnd { get; set; }

        [Required(ErrorMessage = "Contractor name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of authorize person name of contractor is 100 characters..!")]
        public string NameOfAuthorisedPersonOfContractor { get; set; }

        [Required(ErrorMessage = "Authorize person address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of authorize person address is 500 characters..!")]
        public string AddressOfAuthorisedPersonOfContractor { get; set; }

        [Required(ErrorMessage = "Authorised Person Of Contractor Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Authorised Person Of Contractor Village or Town is 100 characters..!")]
        public string AuthorisedPersonOfContractor_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Tehsil is required..!")]
        [ForeignKey("AuthorisedPersonTehsilLgd")]
        public Int64 AuthorisedPersonTehsilRefId { get; set; }
        public virtual TehsilLgd AuthorisedPersonTehsilLgd { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("AuthorisedPerson_DistrictLgd")]
        public Int64 AuthorisedPersonDistrictRefId { get; set; }
        public virtual DistrictLgd AuthorisedPerson_DistrictLgd { get; set; }

        [Required(ErrorMessage = "Authorised Person Of Contractor Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string AuthorisedPersonOfContractor_PinCode { get; set; }

        [Required(ErrorMessage = "Maximum number of contract labour proposed to be employed is required..! ")]
        public int MaxContractLabourToBeEmployed { get; set; }

        //[Required(ErrorMessage = "No of inter state migrant worker is required..!")]
        //public int NoOfInterStateMigrant { get; set; }

        [Required(ErrorMessage = "Licence fee deposited is required..!")]
        public decimal AmountOfLicenceFee { get; set; }

        [Required(ErrorMessage = "Licence fee deposited is required..!")]
        [StringLength(200, ErrorMessage = "The max length of Particular Of Licence Fee is 200 characters..!")]
        public string ParticularOfLicenceFee { get; set; }
        public decimal AmountOfSecurityFee { get; set; }
        public decimal ParicularOfSecurityFee { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        #region Foreign Key Relations

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }
        public virtual Contractor_PrincipalEmployer Contractor_PrincipalEmployer { get; set; }
        public virtual ContractLabourAndEstablishmentMapping ContractLabourAndEstablishmentMapping { get; set; }

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }
        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public Int64 EstablishmentRefId { get; set; }

        #endregion

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
    }
    public class Contractor_PrincipalEmployer
    {
        #region Principal Employer
        [Key]
        public Int64 Contractor_PrincipalEmployerId { get; set; }

        [Required(ErrorMessage = "Establishment name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of establishment name is 100 characters..!")]
        public string EstablishmentName { get; set; }

        [Required(ErrorMessage = "Establishment address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of establishment address is 500 characters..!")]
        public string Estb_Address { get; set; }

        [Required(ErrorMessage = "Establishment Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Establishment Village or Town is 100 characters..!")]
        public string Estb_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Contractor Tehsil is required..!")]
        [ForeignKey("Contractor_PE_ESTB_TehsilLgd")]
        public Int64 TehsilRefId { get; set; }
        public virtual TehsilLgd Contractor_PE_ESTB_TehsilLgd { get; set; }

        [Required(ErrorMessage = "Contractor District is required..!")]
        [ForeignKey("Contractor_PE_ESTB_DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd Contractor_PE_ESTB_DistrictLgd { get; set; }

        [Required(ErrorMessage = "Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Business type is Required..!")]
        public BusinessTypeEnum BusinessType { get; set; }

        [Required(ErrorMessage = "License number is Required..!"), StringLength(20, ErrorMessage = "The max length of Labour License number is 20 characters..!")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "Date of certificate of license is required..!")]
        public DateTime DateOfCertificateOfLicense { get; set; }

        [Required(ErrorMessage = "Name of principal Employer is Required..!")]
        [StringLength(50, ErrorMessage = "The max length of Name of principal Employer is 50 characters..!")]
        public string PrincipalEmployer_Name { get; set; }

        [Required(ErrorMessage = "Principal Employer address is required..!")]
        [StringLength(500, ErrorMessage = "The max length of Principal Employer address is 500 characters..!")]
        public string PrincipalEmployer_Address { get; set; }

        [Required(ErrorMessage = "Principal Employer Village or Town name is required..!")]
        [StringLength(100, ErrorMessage = "The max length of Establishment Village or Town is 100 characters..!")]
        public string PrincipalEmployer_VillageOrTown { get; set; }

        [Required(ErrorMessage = "Principal Employer Tehsil is required..!")]
        [ForeignKey("Contractor_PE_TehsilLgd")]
        public Int64 PrincipalEmployer_TehsilRefId { get; set; }
        public virtual TehsilLgd Contractor_PE_TehsilLgd { get; set; }

        [Required(ErrorMessage = "Principal employer district ref Id is required..!")]
        [ForeignKey("Contractor_PE_DistrictLgd")]
        public Int64 PrincipalEmployer_DistrictRefId { get; set; }
        public virtual DistrictLgd Contractor_PE_DistrictLgd { get; set; }

        [Required(ErrorMessage = "Principal Employer Pin code is required..!"), StringLength(6, ErrorMessage = "Invalid Pin code..!")]
        public string PrincipalEmployer_PinCode { get; set; }

        [Required(ErrorMessage = "ContractorRefId is required..!")]
        [ForeignKey("Contractor_GeneralDetail")]
        public Int64 ContractorRefId { get; set; }
        public virtual Contractor_GeneralDetail Contractor_GeneralDetail { get; set; }
        #endregion Principal Employer
    }
    public class ContractLabourAndEstablishmentMapping
    {
        [Key, Required]
        public Int64 ContractLabourAndEstablishmentMappingId { get; set; }

        [Required(ErrorMessage = "ContractLabourRefId is required..!")]
        [ForeignKey("Contractor_GeneralDetail")]
        public Int64 ContractLabourRefId { get; set; }
        public virtual Contractor_GeneralDetail Contractor_GeneralDetail { get; set; }

        [Required(ErrorMessage = "EstablishmentRefId is required..!")]
        [ForeignKey("Establishment_GeneralDetail")]
        public Int64 EstablishmentRefId { get; set; }
        public virtual Establishment_GeneralDetail Establishment_GeneralDetail { get; set; }
    }
    #endregion

    #region Contract Labour
    public class Licence_ContractLabour_GeneralDetail
    {
        [Key]
        public Int64 ContractLabourId { get; set; }

        [Required(ErrorMessage = "IsCoopraticSociety is required..!")]
        public string IsCoopraticSociety { get; set; }

        [Required(ErrorMessage = "Contractor name is required..!")]
        public string ContractorName { get; set; }

        [Required(ErrorMessage = "Contractor Father name is required..!")]
        public string ContractorFatherName { get; set; }

        [Required(ErrorMessage = "Contractor Address is required..!")]
        public string ContractorAddress { get; set; }

        [Required(ErrorMessage = "Contractor DOB is required..!")]
        public DateTime? ContractorDOB { get; set; }

        [Required(ErrorMessage = "Type Of Business is required..!")]
        public string TypeOfBusiness { get; set; }

        [Required(ErrorMessage = "Registration Certificate No is required..!")]
        public string RegistrationCertificateNo { get; set; }

        [Required(ErrorMessage = "Registration Certificate date is required..!")]
        public DateTime? RegistrationCertificateDate { get; set; }

        [Required(ErrorMessage = "Principal Employer Name is required..!")]
        public string PrincipalEmployerName { get; set; }

        [Required(ErrorMessage = "Principal Employer Address is required..!")]
        public string PrincipalEmployerAddress { get; set; }

        [Required(ErrorMessage = "Nature Of Work is required..!")]
        public string NatureOfWork { get; set; }

        [Required(ErrorMessage = "Contract Work Commencing Date is required..!")]
        public DateTime? ContractWork_CommencingDate { get; set; }

        [Required(ErrorMessage = "Contract Work Termination Date is required..!")]
        public DateTime? ContractWork_TerminationDate { get; set; }

        [Required(ErrorMessage = "Agent Or Manager Name is required..!")]
        public string AgentOrManagerName { get; set; }

        [Required(ErrorMessage = "Agent Or Manager Address is required..!")]
        public string AgentOrManagerAddress { get; set; }

        [Required(ErrorMessage = "Maximum Number Of Employee is required..!")]
        public Int64 MaximumNumberOfEmployee { get; set; }

        public int? ContractorAge { get; set; }
        public Int64 LicenceForYear { get; set; }
        public int ModifiedCounter { get; set; } = 1;
        public Int64 PEContractorId { get; set; }

        [Required(ErrorMessage = "EstablishmentName is required..!")]
        public string EstablishmentName { get; set; }

        [Required(ErrorMessage = "Pan Or Tan Number is required..!")]
        [StringLength(10)]
        public string PanOrTanNumber { get; set; }

        [Required(ErrorMessage = "Gst Number is required..!")]
        [StringLength(15)]
        public string GstNumber { get; set; }

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
        public Int64 AlcCircleRefId { get; set; }

        [NotMapped]
        public Int64 DistrictRefId { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }


        [NotMapped]
        public DateComponentViewModel ContractorDOB_Json { get; set; }

        [NotMapped]
        public DateComponentViewModel RegistrationCertificateDate_Json { get; set; }

        [NotMapped]
        public DateComponentViewModel ContractWork_CommencingDate_Json { get; set; }

        [NotMapped]
        public DateComponentViewModel ContractWork_TerminationDate_Json { get; set; }

        [NotMapped]
        public string LegacyLicenceNumber { get; set; }

        #endregion
    }

    public class Licence_ContractLabour_AmendmentDataHistory
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "SectionCode is required..!")]
        [StringLength(10, ErrorMessage = "SectionCode max size is 10")]
        public string SectionCode { get; set; }

        [Required(ErrorMessage = "FieldName is required..!")]
        [StringLength(50, ErrorMessage = "FieldName max size is 50")]
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string ModifiedValue { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedCounter { get; set; }
        public bool IsLocked { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }
    }
    #endregion

    public class Licence_Contract_LabourViewModel
    {
        public Licence_ContractLabour_GeneralDetail GeneralDetail { get; set; }
        public List<Licence_ContractLabour_AmendmentDataHistory> Licence_ContractLabour_AmendmentDataHistories { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
        public Int64 AppActionType { get; set; }

    }

}
