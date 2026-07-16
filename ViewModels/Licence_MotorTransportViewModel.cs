using pbsamadhannetcoreapi.Models;
using System.Collections.Generic;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class Licence_MotorTransportViewModel
    {
        public Licence_MotorTransport GeneralDetail { get; set; }
        public List<Licence_Motor_Transport_AmendmentDataHistories> Licence_Motor_Transport_AmendmentDataHistories { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
    }
}
