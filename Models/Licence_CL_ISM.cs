using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{

    #region ISM Contract Labour
    public class Licence_ISM_ContractLabour_GeneralDetail
    {
        [Key]
        public Int64 ISMContractLabourId { get; set; }

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
        [Required(ErrorMessage = "EstablishmentName is required..!")]
        public string EstablishmentName { get; set; }


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
        public string LicenceForYear { get; set; }
        public int ModifiedCounter { get; set; } = 1;

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

        #endregion

        [NotMapped]
        public string Legacy_LicenceNo { get; set; }
    }

    public class Licence_ISM_ContractLabour_AmendmentDataHistory
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

    public class Licence_ISM_Contract_LabourViewModel
    {
        public Licence_ISM_ContractLabour_GeneralDetail GeneralDetail { get; set; }
        public List<Licence_ISM_ContractLabour_AmendmentDataHistory> Licence_ISM_ContractLabour_AmendmentDataHistories { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
        public Int64 AppActionType { get; set; }

    }

}
