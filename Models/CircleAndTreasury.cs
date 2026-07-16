using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class FactoryCircle
    {
        [Key]
        public Int64 FactoryCircleId { get; set; }

        [Required(ErrorMessage = "Factory Circle Name is required..!")]
        [StringLength(100)]
        public string FactoryCircleName { get; set; }

        [Required(ErrorMessage = "Juridiction Area is required..!")]
        public string JuridictionArea { get; set; }

        [Required(ErrorMessage = "Max Inspection Count is required..!")]
        public Int64 MaxInspectionCountLimit { get; set; }

        [ForeignKey("DistrictLgdRefId")]
        public Int64 DistrictLgdRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "Medical Circle ID is required..!")]
        public Int64 MedicalCircleID { get; set; }

        [Required(ErrorMessage = "Joint Dir Circle ID is required..!")]
        public Int64 JointDirCircleID { get; set; }
        public virtual FactoryCircleTreasuryMapping FactoryCircleTreasuryMappings { get; set; }
        public virtual ICollection<ApplicationCircleMapping> ApplicationCircleMappings { get; set; }
        public virtual ICollection<UserCircleMapping> UserCircleMapping { get; set; }

        [Required(ErrorMessage = "Version is required..!")]
        public int Version { get; set; }
    }

    public class ALCCircle
    {
        [Key]
        public Int64 ALCCircleId { get; set; }

        [Required(ErrorMessage = "ALC Circle Name is required..!")]
        [StringLength(100)]
        public string ALCCircleName { get; set; }

        [Required(ErrorMessage = "Juridiction Area is required..!")]
        public string JuridictionArea { get; set; }

        [ForeignKey("DistrictLgdRefId")]
        public Int64 DistrictLgdRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "Max Inspection Count is required..!")]
        public Int64 MaxInspectionCountLimit { get; set; }

        [Required(ErrorMessage = "Version is required..!")]
        public int Version { get; set; }
        public virtual ICollection<LabourCircle> LabourCircles { get; set; }
        public virtual ALCCircleTreasuryMapping ALCCircleTreasuryMapping { get; set; }
        public virtual ICollection<ApplicationCircleMapping> ApplicationCircleMappings { get; set; }
        public virtual ICollection<UserCircleMapping> UserCircleMapping { get; set; }
    }

    public class LabourCircle
    {
        [Key]
        public Int64 LabourCircleId { get; set; }

        [Required(ErrorMessage = "Labour Circle Name is required..!")]
        public string LabourCircleName { get; set; }

        [Required(ErrorMessage = "Juridcition Area is required..!")]
        public string JuridcitionArea { get; set; }

        [ForeignKey("ALCCircleRefId")]
        public Int64 ALCCircleRefId { get; set; }
        public virtual ALCCircle ALCCircle { get; set; }

        [Required(ErrorMessage = "Max Inspection Count Limit is required..!")]
        public int MaxInspectionCountLimit { get; set; }

        [Required(ErrorMessage = "Version is required..!")]
        public int Version { get; set; }
        public virtual LabourCircleTreasuryMapping LabourCircleTreasuryMapping { get; set; }
        public virtual ICollection<ApplicationCircleMapping> ApplicationCircleMappings { get; set; }
        public virtual ICollection<UserCircleMapping> UserCircleMapping { get; set; }
        public virtual ICollection<Licence_Factory_AdditionalDetail> Licence_Factory_AdditionalDetails { get; set; }
    }

    public class FactoryCircleTreasuryMapping
    {
        [Key]
        public Int64 FactoryCircleTreasuryMappingId { get; set; }

        [Required(ErrorMessage = "Treasury_Code is required..!")]
        [StringLength(10, ErrorMessage = "Treasury Code must be 5-10 digits long !", MinimumLength = 0)]
        public string Treasury_Code { get; set; }

        [Required(ErrorMessage = "Treasury_Abbre is required..!")]
        [StringLength(5, MinimumLength = 0)]
        public string Treasury_Abbre { get; set; }

        [Required(ErrorMessage = "Treasury_DDOCode is required..!")]
        public string Treasury_DDOCode { get; set; }

        [Required(ErrorMessage = "FactoryCircleRefId is required..!")]
        [ForeignKey("FactoryCircle")]
        public Int64 FactoryCircleRefId { get; set; }
        public virtual FactoryCircle FactoryCircle { get; set; }
    }

    public class ALCCircleTreasuryMapping
    {
        [Key]
        public Int64 ALCCircleTreasuryMappingId { get; set; }

        [Required(ErrorMessage = "Treasury_Code is required..!")]
        [StringLength(10, ErrorMessage = "Treasury Code must be 5-10 digits long !", MinimumLength = 0)]
        public string Treasury_Code { get; set; }

        [Required(ErrorMessage = "Treasury_Abbre is required..!")]
        [StringLength(5, MinimumLength = 0)]
        public string Treasury_Abbre { get; set; }

        [Required(ErrorMessage = "Treasury_DDOCode is required..!")]
        public string Treasury_DDOCode { get; set; }

        [Required(ErrorMessage = "ALCCircleRefId is required..!")]
        [ForeignKey("ALCCircleRefId")]
        public Int64 ALCCircleRefId { get; set; }
        public virtual ALCCircle ALCCircle { get; set; }
    }

    public class LabourCircleTreasuryMapping
    {
        [Key]
        public Int64 LabourCircleTreasuryMappingId { get; set; }

        [Required(ErrorMessage = "Treasury_Code is required..!")]
        [StringLength(10, ErrorMessage = "Treasury Code must be 5-10 digits long !", MinimumLength = 0)]
        public string Treasury_Code { get; set; }

        [Required(ErrorMessage = "Treasury_Abbre is required..!")]
        [StringLength(5, MinimumLength = 0)]
        public string Treasury_Abbre { get; set; }

        [Required(ErrorMessage = "Treasury_DDOCode is required..!")]
        public string Treasury_DDOCode { get; set; }

        [Required(ErrorMessage = "LabourCircleRefId is required..!")]
        [ForeignKey("LabourCircleRefId")]
        public Int64 LabourCircleRefId { get; set; }
        public virtual LabourCircle LabourCircle { get; set; }
    }

    public class ApplicationCircleMapping
    {
        [Key]
        public Int64 AppCircleMappingId { get; set; }

        [ForeignKey("AppRefId")]
        [Required(ErrorMessage = "AppRefId is required..!")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [ForeignKey("FactoryCircleRefId")]
        [Required(ErrorMessage = "FactoryCircleRefId is required..!")]
        public Int64 FactoryCircleRefId { get; set; }
        public virtual FactoryCircle FactoryCircle { get; set; }

        [ForeignKey("ALCCircleRefId")]
        [Required(ErrorMessage = "ALCCircleRefId is required..!")]
        public Int64 ALCCircleRefId { get; set; }
        public virtual ALCCircle ALCCircle { get; set; }

        [ForeignKey("LabourCircleRefId")]
        [Required(ErrorMessage = "LabourCircleRefId is required..!")]
        public Int64 LabourCircleRefId { get; set; }
        public virtual LabourCircle LabourCircle { get; set; }
    }

    public class UserCircleMapping
    {
        [Key]
        public Int64 UserCircleMappingId { get; set; }

        [Required(ErrorMessage = "UserId is required..!")]
        [ForeignKey("User")]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "Factory Circle Ref Id is required..!")]
        [ForeignKey("Factory Circle")]
        public Int64? FactoryCircleRefId { get; set; }

        [Required(ErrorMessage = "Labour Circle Ref Id is required..!")]
        [ForeignKey("Labour Circle")]
        public Int64? LabourCircleRefId { get; set; }

        [Required(ErrorMessage = "Alc Circle Ref Id is required..!")]
        [ForeignKey("ALC Circle")]
        public Int64? AlcCircleRefId { get; set; }

        [Required(ErrorMessage = "Version is required..!")]
        public int Version { get; set; }

        #region Foreign Key Relation
        public virtual User User { get; set; }
        public virtual FactoryCircle FactoryCircle { get; set; }
        public virtual LabourCircle LabourCircle { get; set; }
        public virtual ALCCircle ALCCircle { get; set; }
        #endregion Foreign Key Relation 
    }
    public class CircleUpgradationHistoryLog
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 AppRefId { get; set; }
        public string OldUserRefId { get; set; }
        public Int64 OldUserProfileId { get; set; }
        public string OldUserRoleId { get; set; }

        public string NewUserRefId { get; set; }
        public Int64 NewUserProfileId { get; set; }
        public string NewUserRoleId { get; set; }

        public Int64 OldCircleId { get; set; }
        public Int64 NewCircleId { get; set; }
        public CircleTypeEnum CircleType{ get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
