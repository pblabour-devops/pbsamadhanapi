using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Licence_CL_PE_GeneralDetail
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "PE_Name is required..!")]
        [StringLength(100, ErrorMessage = "PE_Name max size is 100")]
        public string PE_Name { get; set; }

        [Required(ErrorMessage = "PE_FatherName is required..!")]
        [StringLength(100, ErrorMessage = "PE_FatherName max size is 100")]
        public string PE_FatherName { get; set; }

        [Required(ErrorMessage = "PE_Mobile is required..!")]
        [StringLength(10, ErrorMessage = "PE_Mobile max size is 10")]
        public string PE_Mobile { get; set; }

        [Required(ErrorMessage = "PE_Email is required..!")]
        public string PE_Email { get; set; }

        [Required(ErrorMessage = "PE_Address is required..!")]
        [StringLength(250, ErrorMessage = "PE_Address max size is 250")]
        public string PE_Address { get; set; }

        [Required(ErrorMessage = "Manager_Name is required..!")]
        [StringLength(100, ErrorMessage = "Manager_Name max size is 100")]
        public string Manager_Name { get; set; }

        [Required(ErrorMessage = "Manager_Mobile is required..!")]
        [StringLength(10, ErrorMessage = "Manager_Mobile max size is 10")]
        public string Manager_Mobile { get; set; }

        [Required(ErrorMessage = "Manager_Email is required..!")]
        public string Manager_Email { get; set; }

        [Required(ErrorMessage = "Manager_Address is required..!")]
        [StringLength(250, ErrorMessage = "Manager_Address max size is 250")]
        public string Manager_Address { get; set; }

        [Required(ErrorMessage = "NatureOfWork is required..!")]
        public string NatureOfWork { get; set; }

        public Int64 TotalWorker { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "ModifiedCounter is required..!")]
        public int ModifiedCounter { get; set; } = 1;

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

        public virtual ICollection<Licence_CL_PE_Contrator> Licence_CL_PE_Contrators { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }

        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [NotMapped]
        public string RootActivityRefId { get; set; }


        [NotMapped]
        public Int64 DistrictRefId { get; set; }
    }


    public class Licence_CL_PE_Contrator
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Name is required..!")]
        [StringLength(100, ErrorMessage = "Name max size is 100")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required..!")]
        [StringLength(250, ErrorMessage = "Address max size is 250")]
        public string Address { get; set; }

        [Required(ErrorMessage = "NatureOfWork is required..!")]
        public string NatureOfWork { get; set; }

        [Required(ErrorMessage = "MaxLabourWorkerEmployed is required..!")]
        public int MaxLabourWorkerEmployed { get; set; }

        [Required(ErrorMessage = "DateOfCommencement is required..!")]
        public DateTime? DateOfCommencement { get; set; }

        [Required(ErrorMessage = "DateOfTermination is required..!")]
        public DateTime? DateOfTermination { get; set; }

        [Required(ErrorMessage = "ModifiedCounter is required..!")]
        public int ModifiedCounter { get; set; } = 1;

        public int IsFormVGenerate { get; set; } = 0;

        public DateTime FormVGenerateDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Licence_CL_PE_GeneralDetailRefId is required..!")]
        [ForeignKey("Licence_CL_PE_GeneralDetail")]
        public Int64? Licence_CL_PE_GeneralDetailRefId { get; set; }

        public virtual Licence_CL_PE_GeneralDetail Licence_CL_PE_GeneralDetail { get; set; }

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

    public class Licence_PE_AmendmentDataHistories
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
