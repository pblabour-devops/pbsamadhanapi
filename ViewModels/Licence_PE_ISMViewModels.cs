
using pbsamadhannetcoreapi.Models;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pbsamadhannetcoreapi.ViewModels
{
  
        public class Licence_PE_ISM_ContractorDetailViewModel
        {
            public Int64 id { get; set; }
            public Int64 AppRefId { get; set; }
            public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        }


        public class IContractorDetailISMViewModel
        {
            public Int64 id { get; set; }
            public Int64 AppRefId { get; set; }
            public Int64 Totalworkers { get; set; }

            [NotMapped]
            public string RootActivityRefId { get; set; }

            [NotMapped]
            public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

            [NotMapped]
            public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

            [NotMapped]
            public int ProjectSiteVersion { get; set; }

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
        }

        public class Licence_PE_ISMViewModel
        {

            public Licence_PE_ISM_GeneralDetail GeneralDetail { get; set; }
            public List<Licence_PE_ISM_AmendmentDataHistories> Licence_ISM_AmendmentDataHistories { get; set; }
            public bool IsLocked { get; set; }
            public bool IsFeeApplicable { get; set; }
            public Int64 AppActionType { get; set; }


        }

        public class Licence_PE_ISM_GeneralDetail_ViewModel
        {
            public Int64 Id { get; set; }

            public string PE_Name { get; set; }
            public string PE_FatherName { get; set; }
            public string PE_Mobile { get; set; }
            public string PE_Email { get; set; }
            public string PE_Address { get; set; }
            public string Manager_Name { get; set; }
            public string Manager_Mobile { get; set; }
            public string Manager_Email { get; set; }
            public string Manager_Address { get; set; }
            public string NatureOfWork { get; set; }
            public Int64 TotalWorker { get; set; }
            public int ModifiedCounter { get; set; } = 1;
            public Int64 AppRefId { get; set; }
            public DateTime RegistrationDate { get; set; }

        }



        public class InterstatePEContractLabour_ViewModel
        {
            public Int64 ContractLabourId { get; set; }
            public Int64 AppId { get; set; }
            public Int64 AppFormId { get; set; }
            public string ContractorName { get; set; }
            public string ContractorFatherName { get; set; }
            public string ContractorAddress { get; set; }
            public DateTime? DOB { get; set; }
            public bool IsCoopraticSociety { get; set; }
            public string EstablishmentName { get; set; }
            public string EstablishmentAddress { get; set; }
            public string TypeOfBusiness { get; set; }
            public string RegistrationCertificateNo { get; set; }
            public DateTime? RegistrationCertificateDate { get; set; }
            public string PrincipalEmployerName { get; set; }
            public string PrincipalEmployerAddress { get; set; }
            public string WorkNatureString { get; set; }
            public DateTime ContractWork_CommencingDate { get; set; }
            public DateTime ContractWork_TerminationDate { get; set; }
            public string AgentOrManagerName { get; set; }
            public string AgentOrManagerAddress { get; set; }
            public Int64 MaximumNumberEmployee { get; set; }
            public decimal? fees { get; set; }
            public bool IsSecurityAdjusdRequested { get; set; }
            public string SecurityAdjusdReceiptNo { get; set; }
            public DateTime? SecurityAdjusdReceiptDate { get; set; }
            public decimal? SecurityAdjustableAmount { get; set; }
            public bool IsSecurityBalanceAfterAdjusd { get; set; }
            public string SecurityBalanceAfterAdjusdReceiptNo { get; set; }
            public DateTime? SecurityBalanceAfterAdjusdReceiptDate { get; set; }
            public decimal? SecurityBalanceAmountAfterAdjusd { get; set; }
            public DateTime? PDate { get; set; }
            public DateTime? TDate { get; set; }
            public DateTime? ApplicationDate { get; set; }
            public int? Age { get; set; }

            public string LicenseNumber { get; set; }
            public string ContractorId { get; set; }

            public string DispatchNo { get; set; }
            public string UserId { get; set; }

            public string OLNo { get; set; }
            public string OLProof { get; set; }
            public DateTime? OLValidUpto { get; set; }
            public Int32? OLEmps { get; set; }
            public string NAR { get; set; }

            public Int64? ProcessTime { get; set; }

            public string ContMobileNo { get; set; }
            public string ContAadhaarNo { get; set; }
            public string ContEmail { get; set; }
        }
   
}
