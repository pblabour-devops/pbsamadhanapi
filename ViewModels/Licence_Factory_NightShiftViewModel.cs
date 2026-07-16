using pbsamadhannetcoreapi.Models;
using System;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class Licence_Factory_NightShiftViewModel
    {
        public Licence_Factory_NightShift_Approval GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }

    public class ClearancesDataViewModel
    {
        public Int64 projectSiteRefId { get; set; }
        public Int64 pbLabour_AppId { get; set; }
        public Int32 IsLegacy { get; set; }
        
    }
}