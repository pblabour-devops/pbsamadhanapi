using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class FormHViewModel
    {
        public Int64 AppID { get; set; }
        public string NicDesc { get; set; }
        public string LicenceNumber { get; set; }
        public string PublicAppRefNum { get; set; }
        public string EstablishmentName { get; set; }
        public Int32 ShopType { get; set; }
        public Int32 EmployeeCount { get; set; }
        public Int32 EmployeeMCount { get; set; }
        public Int32 EmployeeFCount { get; set; }
        public string OwnerName { get; set; }
        public string OwnerFatherOrHusbandName { get; set; }
        public string Address { get; set; }
        public string TehsilName { get; set; }
        public string MobileNo   { get; set; }
        public string AlternateMobileNo { get; set; }
        public string Email { get; set; }
        public string AadharNumber { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime ApplicationLifeCycleLastStatusOn { get; set; }
        public Int32 EstablishmentConstitutionType { get; set; }
        public string Sender_UserRefId { get; set; }
        public string List_Output { get; set; }
        public Int32 MaxRows { get; set; }
    }

    
}
