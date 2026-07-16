using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class MPR_Factory
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Month is required..!")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Year is required..!")]
        public int Year { get; set; }

        [Required(ErrorMessage = "JsonData is required..!")]
        public string JsonData { get; set; }

        [Required(ErrorMessage = "AccidenInfoListJson is required..!")]
        public string AccidenInfoListJson { get; set; }

        [Required(ErrorMessage = "CourtCaseInfoListJson is required..!")]
        public string CourtCaseInfoListJson { get; set; }

        [Required(ErrorMessage = "AdminDutyInfoListJson is required..!")]
        public string AdminDutyInfoListJson { get; set; }

        [Required(ErrorMessage = "TrainingInfoList is required..!")]
        public string TrainingInfoListJson { get; set; }


        [Required(ErrorMessage = "IsLocked is required..!")]
        public bool IsLocked { get; set; }

        [Required(ErrorMessage = "LastModifiedOn is required..!")]
        public DateTime LastModifiedOn { get; set; }

        [Required(ErrorMessage = "SubmittedBy_UserRefId is required..!")]
        [StringLength(50, ErrorMessage = "SubmittedBy_UserRefId max length 50..!")]
        public string SubmittedBy_UserRefId { get; set; }

        [Required(ErrorMessage = "SubmittedBy_ProfileRefId is required..!")]
        public Int64 SubmittedBy_ProfileRefId { get; set; }

        [Required(ErrorMessage = "SubmittedBy_RoleRefId is required..!")]
        [StringLength(50, ErrorMessage = "SubmittedBy_RoleRefId max length 50..!")]
        public string SubmittedBy_RoleRefId { get; set; }

        [Required(ErrorMessage = "FactoryCircleRefId is required..!")]
        public Int64 FactoryCircleRefId { get; set; }

        [Required(ErrorMessage = "IsLegacy is required..!")]
        public bool IsLegacy { get; set; }
        public string Legacy_Role { get; set; }
        public Int64 Legacy_MPRId { get; set; }
        public string Legacy_UserName { get; set; }

    }

    public class MPR_Labour
    {
        [Key]
        public Int64 Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Int64 LabourCircleRefId { get; set; }
        public string StepCodes { get; set; }
        public string JsonData { get; set; }
        public bool IsLocked { get; set; }
        public Int64 SubmittedBy_ProfileRefId { get; set; }
        public string SubmittedBy_UserRefId { get; set; }
        public string SubmittedBy_RoleRefId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsLegacy { get; set; }
        public string Legacy_Role { get; set; }
        public Int64 Legacy_MPRId { get; set; }
        public string Legacy_UserName { get; set; }
    }

    public class MPR_Alc
    {
        [Key]
        public Int64 Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Int64 AlcCircleRefId { get; set; }
        public string StepCodes { get; set; }
        public string JsonData { get; set; }
        public bool IsLocked { get; set; }
        public Int64 SubmittedBy_ProfileRefId { get; set; }
        public string SubmittedBy_UserRefId { get; set; }
        public string SubmittedBy_RoleRefId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsLegacy { get; set; }
        public string Legacy_Role { get; set; }
        public Int64 Legacy_MPRId { get; set; }
        public string Legacy_UserName { get; set; }
    }


}
