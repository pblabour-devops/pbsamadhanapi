using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Licence_MotorTransport
    {
        [Key]
        [Required(ErrorMessage = "Id is required..!")]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "TransportUndertakingName is required..!")]
        [StringLength(500,ErrorMessage = "TransportUndertakingName max length 500..!")]
        public string TransportUndertakingName { get; set; }

        [Required(ErrorMessage = "CommunicationAddress is required..!")]
        [StringLength(500, ErrorMessage = "CommunicationAddress max length 500..!")]
        public string CommunicationAddress{ get; set; }

        [Required(ErrorMessage = "CommunicationAddress_PinCode is required..!")]
        [StringLength(6, ErrorMessage = "CommunicationAddress_PinCode max length 6..!")]
        public string CommunicationAddress_PinCode { get; set; }

        [Required(ErrorMessage = "CommunicationAddress_DistrictLgdId is required..!")]
        public int CommunicationAddress_DistrictLgdId { get; set; }

        [Required(ErrorMessage = "CommunicationAddress_TehsilLgdId is required..!")]
        public int CommunicationAddress_TehsilLgdId { get; set; }

        [Required(ErrorMessage = "TransportServiceName is required..!")]
        [StringLength(500, ErrorMessage = "TransportServiceName max length 500..!")]
        public string TransportServiceName { get; set; }

        [Required(ErrorMessage = "TotalRoutes is required..!")]
        public int TotalRoutes { get; set; }

        [Required(ErrorMessage = "TotalRouteMileage is required..!")]
        public int TotalRouteMileage { get; set; }

        [Required(ErrorMessage = "TotalVehicles is required..!")]
        public int TotalVehicles { get; set; }

        [Required(ErrorMessage = "MaxTransportWorkers is required..!")]
        public int MaxTransportWorkers { get; set; }

        [Required(ErrorMessage = "NameAddressType is required..!")]
        public MotorTransportNameAddressTypeEnum NameAddressType { get; set; }

        [Required(ErrorMessage = "NameAddressType_Name is required..!")]
        [StringLength(100, ErrorMessage = "NameAddressType_Name max length 100..!")]
        public string NameAddressType_Name { get; set; }

        [Required(ErrorMessage = "NameAddressType_Email is required..!")]
        public string NameAddressType_Email { get; set; }

        [Required(ErrorMessage = "NameAddressType_Mobile is required..!")]
        [StringLength(10, ErrorMessage = "NameAddressType_Mobile max length 10..!")]
        public string NameAddressType_Mobile { get; set; }

        [Required(ErrorMessage = "NameAddressType_Address is required..!")]
        [StringLength(500, ErrorMessage = "NameAddressType_Address max length 500..!")]
        public string NameAddressType_Address { get; set; }

        [Required(ErrorMessage = "IsCompanyRegUnderCompaniesAct is required..!")]
        public int IsCompanyRegUnderCompaniesAct { get; set; }

        [Required(ErrorMessage = "Director_Name is required..!")]
        [StringLength(100, ErrorMessage = "Director_Name max length 100..!")]
        public string Director_Name { get; set; }

        [Required(ErrorMessage = "Director_Email is required..!")]
        public string Director_Email { get; set; }

        [Required(ErrorMessage = "Director_Mobile is required..!")]
        [StringLength(10, ErrorMessage = "Director_Mobile max length 10..!")]
        public string Director_Mobile { get; set; }

        [Required(ErrorMessage = "Director_Address is required..!")]
        [StringLength(500, ErrorMessage = "Director_Address max length 500..!")]
        public string Director_Address { get; set; }

        public int ModifiedCounter { get; set; } = 1;

        public Int64 LicenceForYear { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public Int64 IPin { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        [NotMapped]
        public Int64 AlcCircleRefId { get; set; }  
        
        [NotMapped]
        public int ProjectSiteVersion { get; set; }

        [NotMapped]
        public int OldAppRefId { get; set; }

        [NotMapped]
        public string DistrictName { get; set; }
        [NotMapped]
        public string TehsilName { get; set; }

    }


    public class Licence_Motor_Transport_AmendmentDataHistories
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "SectionCode is required..!")]
        [StringLength(10, ErrorMessage = "SectionCode max size is 10")]
        public string SectionCode { get; set; }

        [Required(ErrorMessage = "FieldName is required..!")]
        [StringLength(50, ErrorMessage = "FieldName max size is 50")]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "PreviousValue is required..!")]
        public string PreviousValue { get; set; }

        [Required(ErrorMessage = "ModifiedValue is required..!")]
        public string ModifiedValue { get; set; }

        [Required(ErrorMessage = "ModifiedOn is required..!")]
        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = "ModifiedCounter is required..!")]
        public int ModifiedCounter { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [ForeignKey("Application")]
        [Required(ErrorMessage = "Application is required..!")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }
    }
}