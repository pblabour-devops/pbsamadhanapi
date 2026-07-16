using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class MPR_Factory_DashboardViewModel
    {
        public Int64 Id { get; set; }
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string SubmittedBy_UserRefId { get; set; }
        public string FullName { get; set; }
        public string RoleDesc { get; set; }
        public string FactoryCircleName { get; set; }
        public string Legacy_Role { get; set; }
        public string Legacy_Username { get; set; }
        public Int64 Legacy_MPRId { get; set; }
        public bool IsLegacy { get; set; }
        public int MaxRows { get; set; }
    }

    public class MPR_Labour_DashboardViewModel
    {
        public Int64 Id { get; set; }
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string SubmittedBy_UserRefId { get; set; }
        public string FullName { get; set; }
        public string RoleDesc { get; set; }
        public string LabourCircleName { get; set; }
        public string Legacy_Role { get; set; }
        public string Legacy_Username { get; set; }
        public Int64 Legacy_MPRId { get; set; }
        public bool IsLegacy { get; set; }
        public int MaxRows { get; set; }
    }

    public class InsertNullJson_ViewModel
    {
        public string SubmittedBy_UserRefId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<string> StepCodes { get; set; }
        public Int64 SubmittedBy_ProfileRefId { get; set; }
    }

    public class SaveSteps_ViewModel
    {
        public string StepCodes { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string SubmittedBy_UserRefId { get; set; }
        public string SubmittedBy_ProfileRefId { get; set; }
        public string JsonData { get; set; }
        public bool IsLocked { get; set; }
    }

    public class MPR_Alc_DashboardViewModel
    {
        public Int64 Id { get; set; }
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string SubmittedBy_UserRefId { get; set; }
        public string FullName { get; set; }
        public string RoleDesc { get; set; }
        public string AlcCircleName { get; set; }
        public string Legacy_Role { get; set; }
        public string Legacy_Username { get; set; }
        public Int64 Legacy_MPRId { get; set; }
        public bool IsLegacy { get; set; }
        public int MaxRows { get; set; }
    }
    public class InsertNullJsonAlc_ViewModel
    {
        public string SubmittedBy_UserRefId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<string> StepCodes { get; set; }
        public Int64 SubmittedBy_ProfileRefId { get; set; }
    }
    public class SaveStepsAlc_ViewModel
    {
        public string StepCodes { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string SubmittedBy_UserRefId { get; set; }
        public string SubmittedBy_ProfileRefId { get; set; }
        public string JsonData { get; set; }
        public bool IsLocked { get; set; }
    }

    public class MprDataViewModel
    {
        //for App Type=6
        public int? ApprovedBegin { get; set; }
        public int? ApprovedWorkersBegin { get; set; }
        public int? ApprovedDuring { get; set; }
        public int? ApprovedWorkersDuring { get; set; }
        public int? DeRegisteredDuring { get; set; }
        public int? DeRegisteredWorkers { get; set; }
        public int? TotalShopsEnd { get; set; }
        public int? TotalWorkersEnd { get; set; }

        //for App Type=1001
        public int? SchemesPendingatBeginning { get; set; }
        public int? SchemesApprovedduringMonth { get; set; }
        public int? SchemesReceivedduringMonth { get; set; }
        public int? SchemesObjectionraisedduringMonth { get; set; }
        public int? SchemesPendingatEndofMonth { get; set; }
    }
}
