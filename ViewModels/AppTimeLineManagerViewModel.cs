using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class WorkingHourSlabViewModel
    {
        public DateTime SlabDate { get; set; }
        public double TotalWorkingTime { get; set; }
        public SlabDayTypeEnum SlabDayType { get; set; }
        public string SlabDayDesc { get; set; }
        public DateTime WorkStartsTime { get; set; }
        public DateTime WorkEndsTime { get; set; }
    }

    public class ElapsedApplicationViewModel
    {
        public Int64 AppRefId { get; set; }
        public Int64 AppActionLogRefId { get; set; }
        public int AllowedAppActionType { get; set; }
        public DateTime ActionCanTakenUpto { get; set; }
        public Int64 ApplicationActionLogId { get; set; }
        public int AppActionType { get; set; }
        public string Sender_UserRefId { get; set; }
        public Int64 Sender_ProfileRefId { get; set; }
        public string Receiver_UserRefId { get; set; }
        public Int64 Receiver_ProfileRefId { get; set; }
        public DateTime ActionDate { get; set; }
        public string ReceiverRoleId { get; set; }
        public string SenderRoleId { get; set; }
        public Int64 ApplicationType { get; set; }
        public ApplicationProcessPhaseLogTypeEnum ApplicationProcessPhaseLogType { get; set; }
    }

    public class AllowedActionWithTimeLimitViewModel
    {
        public AppActionTypeEnum AppActionType { get; set; }
        public int MaxHoursAllowed { get; set; }
        public bool IsAutoAction { get; set; }
        public bool IsDocumentUploadOption { get; set; }
        public bool IsOptional { get; set; }
        public Int64 DocRefId { get; set; }

        public string UserRefId { get; set; }
        public string PrecheckCode { get; set; }
        public bool IsExpiredByAutoProcess { get; set; }
        public bool IsHidden { get; set; }

    }

    public class ApplicationToBeElapsedViewModel
    {
        public Int64 AppRefId { get; set; }
        public Int64 AppActionLogRefId { get; set; }
        public int AllowedAppActionType { get; set; }
        public DateTime ACtionCanTakenUpto { get; set; }
        public Int64 ApplicationActionLogId { get; set; }
        public int AppActionType { get; set; }
        public string Sender_UserRefId { get; set; }
        public Int64 Sender_ProfileRefId { get; set; }
        public string Receiver_UserRefId { get; set; }
        public Int64 Receiver_ProfileRefId { get; set; }
        public DateTime ActionDate { get; set; }
        public string ReceiverRoleId { get; set; }
        public string SenderRoleId { get; set; }
        public Int32 ApplicationType { get; set; }
        public string UserRefId { get; set; }
        public Int64 TimelineId { get; set; }
        public string DefaultRoleName { get; set; }
    }

    public class  AllowedReceiverDetailsViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string DistrictName { get; set; }
        
    }

    public class ElapsedPhaseApplicationViewModel
    {
        public ApplicationProcessPhaseLogTypeEnum ApplicationProcessPhaseLogType { get; set; }
        public Int64 AppRefId { get; set; }
        public Int64 AppTimelineId { get; set; }

    }
}
