using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class ProjectSite
    {
        [Key, Required]
        public Int64 ProjectSiteId { get; set; }

        [Display(Name = "Establishment Name")]
        [Required(ErrorMessage = "Please enter Establishment Name !")]
        public string EstablishmentName { get; set; }

        #region Addess
        [Required(ErrorMessage = "Address is required..!")]
        //[StringLength(100, ErrorMessage = "The max length of address is 100 characters..!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Village or Town name is required..!")]
        //[StringLength(100, ErrorMessage = "The max length of Village or Town is 100 characters..!")]
        public string VillageOrTown { get; set; }

        [Required(ErrorMessage = "Tehsil is required..!")]
        [ForeignKey("TehsilLgd")]
        public Int64 TehsilRefId { get; set; }
        public virtual TehsilLgd TehsilLgd { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "Pin code is required..!")]
        public string PinCode { get; set; }

        #endregion Addess

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(Name = "IsDeleted")]
        public bool IsDeleted { get; set; }

        public string ApplicantAadharNumber { get; set; }
        public string ApplicantAadharAttachment { get; set; }
        public string ApplicantPanNumber { get; set; }
        public string ApplicantPanAttachment { get; set; }
        public string CompanyPanNumber { get; set; }
        public string CompanyPanAttachment { get; set; }

        public string ProjectPurpose { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Createddate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "FactoryCircleRefId is required..!")]
        public Int64 FactoryCircleRefId { get; set; }

        [Required(ErrorMessage = "LabourCircleRefId is required..!")]
        public Int64 LabourCircleRefId { get; set; }

        [Required(ErrorMessage = "AlcCircleRefId is required..!")]
        public Int64 AlcCircleRefId { get; set; } = 0;

        [Required(ErrorMessage = "Please enter Contact Person First Name !")]
        public string ContactPersonFirstName { get; set; }

        public string ContactPersonMiddleName { get; set; }

        [Required(ErrorMessage = "Please enter Contact Person Last Name !")]
        public string ContactPersonLastName { get; set; }

        [RegularExpression("^([0-9]+)$", ErrorMessage = "Contact Person Mobile number can only have numbers, space and special characters (, - / ()) !")]
        [StringLength(10, ErrorMessage = "Contact Person Mobile number must be 10-20 characters long !", MinimumLength = 10)]
        public string ContactPersonMobileNo { get; set; }

        [Required(ErrorMessage = "Contact Person Email is required..!"), EmailAddress(ErrorMessage = "Invalid email address..!"), StringLength(50, ErrorMessage = "The max length of email is 100 characters..!")]
        public string ContactPersonEmail { get; set; }

        [RegularExpression("^([0-9]+)$", ErrorMessage = "Alternate Mobile number can only have numbers, space and special characters (, - / ()) !")]
        [StringLength(10, ErrorMessage = "Alternate Mobile number must be 10-20 characters long !", MinimumLength = 10)]
        public string AlternateMobileNo { get; set; }

        [Required(ErrorMessage = "Alternate Email is required..!"), EmailAddress(ErrorMessage = "Invalid email address..!"), StringLength(50, ErrorMessage = "The max length of email is 100 characters..!")]
        public string AlternateEmail { get; set; }


        [Required(ErrorMessage = "UserRefId is required..!")]
        [ForeignKey("User")]
        public string UserRefId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<FactoryCircle> FactoryCircles { get; set; }
        public virtual ICollection<LabourCircle> LabourCircles { get; set; }
        public virtual ProjectSite_IpinMapping ProjectSite_IpinMapping { get; set; }

        [Required(ErrorMessage = "ProjectSiteVersion is required..!")]
        public int ProjectSiteVersion { get; set; } = 1;
    }

    public class ProjectSite_IpinMapping
    {
        [Key]
        public Int64 IpinMappingId { get; set; }
        public string IPin { get; set; }

        [Required(ErrorMessage = "ProjectSiteRefId is required..!")]
        [ForeignKey("ProjectSite")]
        public Int64 ProjectSiteRefId { get; set; }
        public virtual ProjectSite ProjectSite { get; set; }
    }

    public class ProjectSiteLog
    {
        [Key, Required]
        public Int64 ProjectSiteLogId { get; set; }

        public Int64 ProjectSiteRefId { get; set; }

        [Display(Name = "Establishment Name")]
        [Required(ErrorMessage = "Please enter Establishment Name !")]
        public string EstablishmentName { get; set; }

        #region Addess
        [Required(ErrorMessage = "Address is required..!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Village or Town name is required..!")]
        public string VillageOrTown { get; set; }

        [Required(ErrorMessage = "Tehsil is required..!")]
        public Int64 TehsilRefId { get; set; }

        [Required(ErrorMessage = "District is required..!")]
        public Int64 DistrictRefId { get; set; }

        [Required(ErrorMessage = "Pin code is required..!")]
        public string PinCode { get; set; }

        #endregion Addess

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(Name = "IsDeleted")]
        public bool IsDeleted { get; set; }

        public string ApplicantAadharNumber { get; set; }
        public string ApplicantAadharAttachment { get; set; }
        public string ApplicantPanNumber { get; set; }
        public string ApplicantPanAttachment { get; set; }
        public string CompanyPanNumber { get; set; }
        public string CompanyPanAttachment { get; set; }

        public string ProjectPurpose { get; set; }

        [Required(ErrorMessage = "Created Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Createddate { get; set; }

        [Required(ErrorMessage = "Last modified Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; }

        [Required(ErrorMessage = "FactoryCircleRefId is required..!")]
        public Int64 FactoryCircleRefId { get; set; }

        [Required(ErrorMessage = "LabourCircleRefId is required..!")]
        public Int64 LabourCircleRefId { get; set; }

        [Required(ErrorMessage = "AlcCircleRefId is required..!")]
        public Int64 AlcCircleRefId { get; set; } = 0;

        [Required(ErrorMessage = "Please enter Contact Person First Name !")]
        public string ContactPersonFirstName { get; set; }

        public string ContactPersonMiddleName { get; set; }

        [Required(ErrorMessage = "Please enter Contact Person Last Name !")]
        public string ContactPersonLastName { get; set; }

        [RegularExpression("^([0-9]+)$", ErrorMessage = "Contact Person Mobile number can only have numbers, space and special characters (, - / ()) !")]
        [StringLength(10, ErrorMessage = "Contact Person Mobile number must be 10-20 characters long !", MinimumLength = 10)]
        public string ContactPersonMobileNo { get; set; }

        [Required(ErrorMessage = "Contact Person Email is required..!"), EmailAddress(ErrorMessage = "Invalid email address..!"), StringLength(50, ErrorMessage = "The max length of email is 100 characters..!")]
        public string ContactPersonEmail { get; set; }

        [RegularExpression("^([0-9]+)$", ErrorMessage = "Contact Person Mobile number can only have numbers, space and special characters (, - / ()) !")]
        [StringLength(10, ErrorMessage = "Contact Person Mobile number must be 10-20 characters long !", MinimumLength = 10)]
        public string AlternateMobileNo { get; set; }

        [Required(ErrorMessage = "Contact Person Email is required..!"), EmailAddress(ErrorMessage = "Invalid email address..!"), StringLength(50, ErrorMessage = "The max length of email is 100 characters..!")]
        public string AlternateEmail { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "ProjectSiteVersion is required..!")]
        public int ProjectSiteVersion { get; set; } = 1;
    }
}
