using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class ProcessApplicationViewModels
    {
        public int AppActionCodeId { get; set; }
        public int ActionCode { get; set; }
        public string ActionName { get; set; }
        public string ActionPublicName { get; set; }
    }
    public class PreCheckViewModel
    {
        public bool IsPreCheckPassed { get; set; }
    }

    public class ProcessApplicationUsersDetailViewModel
    {
        public Boolean IsPreCheckPassed { get; set; }
        public List<ProcessApplicationUsersViewModel> ProcessApplicationUsers { get; set; }

    }
    public class ProcessApplicationUsersViewModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string DistrictName { get; set; }
    }
    public class NotingLogsViewModel
    {
        public string OfficerName { get; set; }
        public string NormalizedName { get; set; }
        public DateTime ActionDate { get; set; }
        public string Remarks { get; set; }
        public string ActionName { get; set; }
        public string ActionPublicName { get; set; }
        public string Checklist_Json { get; set; }
        public bool IsDocumentUploaded { get; set; }
        public Int64 AppDocumentRefId { get; set; }
        public string AttachmentName { get; set; }
        public Int64 ActionTakenHoursCount { get; set; }
        public Int64 ActionTakenDaysCount { get; set; }
        public Int64 ApplicationActionLogId { get; set; }
    }

    public class LendingOfficerDetailsViewModel
    {
        public string ReceiverUserRefId { get; set; }
        public string ReceiverUserRoleId { get; set; }
    }

    public class RoleWiseAllowedActionCodeViewModel
    {
        public string RoleId { get; set; }
        public Int64 CurrentActionCode { get; set; }
        public bool IsDocumentUploadOption { get; set; }
        public bool IsOptional { get; set; }
        public Int64 DocRefId { get; set; }
        public Int64 AllowedActionCode { get; set; }
        public string ActionName { get; set; }
        public string PrecheckCode { get; set; }
    }

    public class RecordActionResponseViewModel
    {
        public Int64 AppActionId { get; set; }
        public Int64 ApplicationActionLogId { get; set; }
    }

    public class UpdateActionLogInRaisedFeeParmsViewModel
    {
        public Int64 AppRefId { get; set; }
        public int PaymentBatchCounter { get; set; }
        public Int64 ApplicationActionLogId { get; set; }
    }
}
