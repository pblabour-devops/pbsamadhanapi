using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class ShopLicenceViewModel
    {
        public ShopLicence_GeneralDetail GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }

    public class ShopLicence_EmployeeDetailsViewModel
    {
        public string EmpName { get; set; }
        public string EmpFHName { get; set; }
        public string Gender { get; set; }
        public Int16 Age { get; set; }
        public DateTime JoiningDate { get; set; }
        public string WeekOffday { get; set; }
        public DateTime WorkingHoursFrom { get; set; }
        public DateTime WorkingHoursTo { get; set; }
        public DateTime IntervalFrom { get; set; }
        public DateTime IntervalTo { get; set; }
        public DateTime Date_of_Birth { get; set; }
        public string MobileNumber { get; set; }
        public string PanNumber { get; set; }
        public string AadharNumber { get; set; }
    }

    public class EmployeeDetailViewModel: SharedFormProperties
    {
        public int IsHavingEmployee { get; set; }
        public Int64 ShopLicenceId { get; set; }
        public Int64 AppRefId { get; set; }
        //public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public ApplicationLifeCycleStatusTypeEnum ApplicationLifeCycleStatusType { get; set; }
        public List<ShopLicence_EmployeeDetail> EmployeesList { get; set; }
    }

    public class ShopEmployeeDetailViewModel
    {
        public Int64 EmployeeDetailId { get; set; }
        public string Name { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MobileNumber { get; set; }
        public string PanNumber { get; set; }
        public string AadharNumber { get; set; }
        public DateTime JoiningDate { get; set; }
        public ClosingDayTypeEnum ClosingDayType { get; set; }
        public string WorkingHoursFrom { get; set; }
        public string WorkingHoursTo { get; set; }
        public string IntervalFrom { get; set; }
        public string IntervalTo { get; set; }
        public Int64 ShopLicenceRefId { get; set; }
        public int MaxRows { get; set; }
    }

    public class AdhaarVerifyViewModel
    {
        public string UserName { get; set; }
    }
}
