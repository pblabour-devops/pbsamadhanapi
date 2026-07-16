using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class InvestPunjabShareStatusParmsViewModel
    {
        public Int64 AppRefId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public Int64 IPin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public int StatusId { get; set; }
        public string StatusDesc { get; set; }
        public string Comments { get; set; }
        public string SenderName { get; set; }
        public string SenderDesignation { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverDesignation { get; set; }
        public DateTime ClearanceIssuedOn { get; set; }
        public DateTime? ClearanceExpiredOn { get; set; }
        public string LicenseNo { get; set; }
        public string ClearanceFile { get; set; }
        public DateTime StatusDate { get; set; }
        public string IntegrationSource { get; set; }
        public string IsdeemedApproval { get; set; }
        public Int64 appActionLogId { get; set; }
        public AppActionTypeEnum appActionType { get; set; }
    }

    public class GetTotalTakenTimeByDepartmentViewModel
    {
        public int TotalTime { get; set; }
        public string TimeType { get; set; }
    }

    public class GetShareStatusRequestDetailsViewModel
    {
        public string InvestPunjab_Ipin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public int AppActionType { get; set; }
        public DateTime ActionDate { get; set; }
        public DateTime ClearanceExpiredOn { get; set; }
        public string Remarks { get; set; }
        public int TriedCount { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string SenderRoleName { get; set; }
        public string ReceiverRoleName { get; set; }
        public string LicenceNumber { get; set; }
        public Int64 AppActionLogId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public string ClearanceFile { get; set; }
    }
}
