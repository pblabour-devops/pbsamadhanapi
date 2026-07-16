using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class InitiateApplicationParmsViewModel
    {
        public ApplicationTypeEnum ApplicationType { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        //public string UserId { get; set; }
        public int ProjectSiteVersion { get; set; } = 1;
        public string RootActivityRefId { get; set; }
        public Int64 IPin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public bool Legacy_IsMigrated { get; set; }
        public Int64 Legacy_AppId { get; set; }
        public Int64 Legacy_AppFormId { get; set; }
        public string Legacy_NAR { get; set; }
        public string Legacy_LicenceNo { get; set; }
    }

    public class ToDoActivityLogViewModel
    {
        //public Int64 ToDoActivityLogId { get; set; }
        public string RootActivityRefId { get; set; }
        public Int64 AppRefId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public string ApplicationTypeDesc { get; set; }

        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public string ApplicationPurposeTypeDesc { get; set; }

        //public ToDoCodeTypeEnum ToDoCodeType { get; set; }
        //public string ToDoCodeTypeDesc { get; set; }

        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }
        public string ToDoActivityModeTypeDesc { get; set; }

        public int ToDoActivityMapingVersion { get; set; }
        //public ToDoActivityCompleteTypeEnum ToDoActivityCompleteType { get; set; }
        //public string ToDoActivityCompleteTypeDesc { get; set; }

        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }
        public string ToDoActivityCategoryTypeDesc { get; set; }

        //public string ActivityFailedMessage { get; set; }
        public string UserRefId { get; set; }
        public string UserFullDetail { get; set; }
        //public string UserName { get; set; }
        //public string UserMobile { get; set; }
        //public string UserEmail { get; set; }
        public DateTime TimeStemp { get; set; }
        public Int64 InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        //public DateTime TimeStemp { get; set; }
        public int TotalSuceeded { get; set; }
        public int TotalFailed { get; set; }
        public int TotalSkipped { get; set; }
        public int TotalSteps { get; set; }
        public int MaxRows { get; set; }
    }

    public class ToDoUserWiseActivityViewModel
    {
        public string UserRefId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public int FailedActivities { get; set; }
        public int SucceededActivities { get; set; }
        public string RootActivityRefIds { get; set; }
    }

    public class ToDoActivityWiseStepViewModel
    {
        public ToDoCodeTypeEnum ToDoCodeType { get; set; }
        public string ToDoCodeTypeDesc { get; set; }
        public ToDoActivityCompleteTypeEnum ToDoActivityCompleteType { get; set; }
        public string ToDoActivityCompleteTypeDesc { get; set; }
        public string ActivityFailedMessage { get; set; }
        public DateTime? TimeStemp { get; set; }
        public int ToDoSerialOrderCount { get; set; }
    }


    public class ToDoTicketDetailViewModel
    {
        public Int64 Id { get; set; }
        public string RootActivityRefId { get; set; }
        public Int64 ManpowerMappingRefId { get; set; }
        public DateTime CreatedOn { get; set; }
        public ToDoTicketStatusTypeEnum ToDoTicketStatusType { get; set; }
        public string ToDoTicketStatusTypeDesc { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public string ApplicationTypeDesc { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public string ApplicationPurposeTypeDesc { get; set; }
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }
        public string ToDoActivityCategoryTypeDesc { get; set; }
        public Int64 AppRefId { get; set; }
        public string UserRefId { get; set; }
        public Int64 InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public string UserLoginName { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
    public class ApplicationLockViewModel
    {
        public Int64 AppRefId { get; set; }
        public int AppActionType { get; set; }
        public string Remarks { get; set; }
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }
        public string RootActivityRefId { get; set; }
    }
}
