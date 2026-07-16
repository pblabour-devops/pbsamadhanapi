using pbsamadhannetcoreapi.Models;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using pbsamadhannetcoreapi.Models.Implementations;

namespace pbsamadhannetcoreapi.ViewModels
{
    
        public class Licence_CL_PE_ContractorDetailViewModel
        {
            public Int64 id { get; set; }
            public Int64 AppRefId { get; set; }
            public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        }

        public class IContractorDetailViewModel
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


        public class Licence_CL_PEViewModel
        {
            public Licence_CL_PE_GeneralDetail GeneralDetail { get; set; }
            public List<Licence_PE_AmendmentDataHistories> Licence_CL_PE_AmendmentDataHistories { get; set; }
            public bool IsLocked { get; set; }
            public bool IsFeeApplicable { get; set; }
            public Int64 AppActionType { get; set; }

        }

        public class Licence_CL_PE_Contractor_ViewModel
        {
            public Int64 EstRegContractorId { get; set; }
            public Int64 AppId { get; set; }
            public string ContractorId { get; set; }
            public string ContractorName { get; set; }
            public string ContractorAddress { get; set; }
            public string WorkNature { get; set; }
            public Int64 ContractLabourEmployed { get; set; }
            public DateTime? ContractWork_CommencingDate { get; set; }
            public DateTime? ContractWork_TerminationDate { get; set; }
            public DateTime? PDate { get; set; }
            public DateTime? TDate { get; set; }
            public string UserID { get; set; }
            public string ContMobileNo { get; set; }
            public string ContAadhaarNo { get; set; }
            public string ContEmail { get; set; }
            public Int64 AppFormId { get; set; }
            public string NAR { get; set; }
            public string DispatchNo { get; set; }


        }


        public class Licence_CL_PE_GeneralDetail_ViewModel
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
            public string EstablishmentName { get; set; }

    }

    
}