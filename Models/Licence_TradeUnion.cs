using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Licence_TradeUnion
    {
        [Key]
        [Required(ErrorMessage = "Id is required..!")]
        public Int64 TradeUnionId { get; set; }

        [Required(ErrorMessage = "Please Enter Name of Establishment")]
        [RegularExpression("^([0-9a-zA-Z,-/()& ]+)$", ErrorMessage = "Name of Establishment can only have alphanumerics, space and special characters (, - / ()) !")]
        [StringLength(100, ErrorMessage = "Name of Establishment must be 3 - 100 characters long!", MinimumLength = 3)]
        public string TUName { get; set; }

        [Required(ErrorMessage = "Please enter Address of Union")]
        [RegularExpression("^([0-9a-zA-Z,-/()&\r\n# ]+)$", ErrorMessage = "Address can only have alphanumerics, space and special characters (, - / ()) !")]
        [StringLength(200, ErrorMessage = "Address must be 3 - 100 characters long!", MinimumLength = 3)]
        public string TUAddress { get; set; }
        public string WorkEngaged { get; set; }

        public DateTime Exisetencedate { get; set; }

        public int ModifiedCounter { get; set; } = 1;

        public int IsVerifybyLBIN { get; set; } = 1;
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
        public Int64 LabourCircleRefId { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }

        [NotMapped]
        public int OldAppRefId { get; set; }

        [NotMapped]
        public string oldLicenceNo { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [NotMapped]
        public List<Licence_TradeUnion_Officer> OfficerList { get; set; }


        [NotMapped]
        public Int64 DistrictRefId { get; set; }

        public virtual ICollection<Licence_TradeUnion_Officer> Licence_TradeUnion_Officer { get; set; }

    }


    public class Licence_TradeUnion_AmendmentDataHistories
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



    public class Licence_TradeUnion_Officer
    {
        [Key]
        [Display(Name = "ID")]
        public Int64 OfficerId { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please Enter Title")]
        [StringLength(60, ErrorMessage = "Officer Father/Husband Name must be 1 - 100 characters long!", MinimumLength = 1)]
        public String Designation { get; set; }

        [Display(Name = "Name of Officer ")]
        [RegularExpression("^([0-9a-zA-Z,-/()& ]+)$", ErrorMessage = "Officer Name can only have alphanumerics, space and special characters (, - / ()) !")]
        [StringLength(100, ErrorMessage = "Officer Name must be 3 - 100 characters long!", MinimumLength = 3)]
        public string OfficerName { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Officer Address must be 3 - 100 characters long!", MinimumLength = 3)]
        public string Address { get; set; }

        [Display(Name = "Phone")]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Phone can only have numerics !")]
        [StringLength(10, ErrorMessage = "Phone must be 10 characters long!", MinimumLength = 10)]
        public string Phone { get; set; }

        public int Age { get; set; }

        public String Occupation { get; set; }

        public int ModifiedCounter { get; set; } = 1;

        [Required(ErrorMessage = "TradeUnionRefId is required..!")]
        [ForeignKey("Licence_TradeUnion")]
        public Int64? TradeUnionRefId { get; set; }
        public virtual Licence_TradeUnion Licence_TradeUnion { get; set; }


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
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }


        [NotMapped]
        public Int64 AppRefId { get; set; }


    }
}