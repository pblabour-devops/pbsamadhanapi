using pbsamadhannetcoreapi.Models.Implementations;
using pbsamadhannetcoreapi.Models;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class Licence_Trade_Union_ViewModels
    {
        public Licence_TradeUnion GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }

    }

    public class IOfficerDetailViewModel
    {
        public Int64 tradeUnionId { get; set; }
        public Int64 AppRefId { get; set; }
        public ApplicationLifeCycleStatusTypeEnum ApplicationLifeCycleStatusType { get; set; }

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


    public class Licence_TradeUnion_Officer_ViewModel
    {
        public Int64 OfficerId { get; set; }
        public Int64 TradeUnionId { get; set; }
        public String Designation { get; set; }
        public string OfficerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public Int32 Age { get; set; }

        public String Occupation { get; set; }

        public string Type { get; set; }
        public bool IsDeleted { get; set; }



    }

    public class TradeUnionLicenceValidateViewModel
    {
        public long AppRefId { get; set; }
        public string LicenceNumber { get; set; }
        public string TUName { get; set; }
        public string TUAddress { get; set; }

    }

}