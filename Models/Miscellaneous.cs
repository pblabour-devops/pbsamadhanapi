using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Establishment_EPFO
    {
        public string EstablishmentId { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress1 { get; set; }
        public string EstablishmentAddress2 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PinCode { get; set; }
        public string CoverDate { get; set; }
        public string EstablishmentStatus { get; set; }
        public string EstablishmentType { get; set; }
        public string Industry_GRP_Id { get; set; }
        public string Industry_Code { get; set; }
        public string DSC { get; set; }
        //public bool HasSent { get; set; }
        //public string SentRemarks { get; set; }
        //public DateTime SentDate { get; set; }
        //public string Sender_UserRefId { get; set; }
        //public Int64 SenderProfileRefId { get; set; }
        //public string SenderRoleId { get; set; }
    }
    public class Establishment_EPFO_Logs
    {
        [Key]
        public Int64 EstablishmentEPFOLogsId { get; set; }
        public string EstablishmentRefId { get; set; }
        public bool HasSent { get; set; }
        public string SentRemarks { get; set; }
        public DateTime SentDate { get; set; } 
        public string Sender_UserRefId { get; set; }
        public Int64 SenderProfileRefId { get; set; }
        public string SenderRoleId { get; set; }

        [NotMapped]
        public string PdfNameGUID { get; set; }
    }

    public class Establishment_EPFO_Report
    {
        public string EstablishmentId { get; set; }

        public Int64 EstablishmentEPFOLogsId { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress1 { get; set; }
        public string EstablishmentType { get; set; }
        public string SentRemarks { get; set; }
        public DateTime SentDate { get; set; }
        public string DisputeNumber { get; set; }
        public int MaxRows { get; set; }
    }

    //public class LicenceValidDates
    //{
    //    public DateTime ValidFrom { get; set; }
    //    public DateTime ValidUpto { get; set; }
  
    //}
}
