using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class Licence_Shop_NightShiftViewModel
    {
        public Licence_Shop_NightShift_Approval GeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
}
