using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    //public class User: IdentityUser<Int64>
    //{
    //    [Required, Key]
    //    public Int64 UserId { get; set; }
    //    [Required]
    //    public Guid UserPublicId { get; set; }

    //    [Required(ErrorMessage = "User name is required..!")]
    //    [StringLength(100, ErrorMessage = "The max length of User name is 100 characters..!")]
    //    public string UserName { get; set; }

    //    [Required, StringLength(maximumLength: 500, MinimumLength = 1, ErrorMessage = "Password hash can not be more than 500 characters..!")]
    //    public string PasswordHash { get; set; }

    //    [Required, StringLength(maximumLength: 500, MinimumLength =1, ErrorMessage = "Password salt can not be more than 50 characters..!")]
    //    public string PasswordSalt { get; set; }

    //    [Required]
    //    public bool IsEnabled { get; set; }

    //    [Required]
    //    public bool IsDeleted { get; set; }

    //    [Required(ErrorMessage = "Created Date is required..!")]
    //    [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime Createddate { get; set; }

    //    [Required(ErrorMessage = "Last modified Date is required..!")]
    //    [DataType(DataType.DateTime, ErrorMessage = "Invalid created date!")]
    //    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime LastModifiedDate { get; set; }
    //    public virtual UserProfileMapping UserProfileMapping { get; set; }
    //    public virtual UserRoleMapping UserRoleMapping { get; set; }
    //    public virtual UserDepartmentMapping UserDepartmentMapping { get; set; }

    //}
    public class User : IdentityUser
    {

        [Required]
        public  bool IsEnabled { get; set; }
        public bool IsTestUser { get; set; }
        public virtual UserProfileMapping UserProfileMapping { get; set; }
        public virtual UserDepartmentMapping UserDepartmentMapping { get; set; }
        public virtual ICollection<ProjectSite> ProjectSites { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationAction> ApplicationActions_User_Senders { get; set; }
        public virtual ICollection<ApplicationAction> ApplicationActions_User_Receivers { get; set; }
        public virtual UserRegisteredMobileAppDevice UserRegisteredMobileAppDevice { get; set; }
        public virtual UserCircleMapping UserCircleMapping { get; set; }

        public virtual ICollection<ApplicationAction_ParallelProcess> ApplicationActions_ParallelProcess_User_Senders { get; set; }
        public virtual ICollection<ApplicationAction_ParallelProcess> ApplicationActions_ParallelProcess_User_Receivers { get; set; }
        public virtual ICollection<UserPasswordResetLog> UserPasswordResetLogs { get; set; }
        
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class RoleRelationMapping
    {
        [Required, Key]
        public Int64 RoleRelationMappingId { get; set; }
        public string NodeRoleId { get; set; }
        public string TargetRoleId { get; set; }
        public RoleRelationTypeEnum NodeRoleToTargetRoleRelationType { get; set; }
    }

    public class UserArchitectAdditionalInfoMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        [StringLength(100, ErrorMessage = "The max length of UserRefId is 100 characters..!")]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "RegistrationNumber_DOF is required..!")]
        [StringLength(50, ErrorMessage = "The max length of RegistrationNumber_DOF is 50 characters..!")]
        public string RegistrationNumber_DOF { get; set; }

        [Required(ErrorMessage = "CommunicationAddress is required..!")]
        public string CommunicationAddress { get; set; }

        [Required(ErrorMessage = "RegistrationNumber_COA is required..!")]
        [StringLength(50, ErrorMessage = "The max length of RegistrationNumber_COA is 50 characters..!")]
        public string RegistrationNumber_COA { get; set; }

        [Required(ErrorMessage = "RegistrationIssuedOn_COA is required..!")]
        public DateTime RegistrationIssuedOn_COA { get; set; }

        [Required(ErrorMessage = "RegistrationValidUpto_COA is required..!")]
        public DateTime RegistrationValidUpto_COA { get; set; }

        [Required(ErrorMessage = "TermAndCondition1 is required..!")]
        public bool TermAndCondition1 { get; set; }

        [Required(ErrorMessage = "TermAndCondition2 is required..!")]
        public bool TermAndCondition2 { get; set; }

        [Required(ErrorMessage = "CouncilOfArchitectApproval is required..!")]
        public string CouncilOfArchitectApproval { get; set; }

        [Required(ErrorMessage = "SpecimenSignature is required..!")]
        public string SpecimenSignature { get; set; }

        [NotMapped]
        public string ArchitectName { get; set; }

        [NotMapped]
        public string ArchitectfatherName { get; set; }

        [NotMapped]
        public string ContactNumber { get; set; }

        [NotMapped]
        public string Email { get; set; }

        [NotMapped]
        public int EmpanelledType { get; set; }
    }

    public class UserMyOfficeMapping
    {
        [Required, Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        [StringLength(100, ErrorMessage = "The max length of UserRefId is 100 characters..!")]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "UserNumericId is required..!")]
        public Int64 UserNumericId { get; set; }
    }
    public class UserPasswordResetLog
    {
        public Int64 UserPasswordResetLogId { get; set; }

        [Required(ErrorMessage = "User ref id is required..!")]
        [StringLength(450)]
        [ForeignKey("User_Sender")]
        public string UserRefId { get; set; }
        public virtual User User { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RequestToken { get; set; }
    }
    public class UserApiActivityLog
    {
        [Key]
        public Int64 ActivityLogId { get; set; }
        
        [Required, StringLength(100)]
        
        public string RequestId { get; set; }

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public Int64 ProfileRefId { get; set; }

        [Required]
        public DateTime TimeStemp { get; set; }

        [Required]
        public string SourceUrl { get; set; }
        [Required]
        public string DestinationUrl { get; set; }

        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public string RequestType { get; set; }

        [Required, StringLength(50)]
        public string IP { get; set; }

        [Required, StringLength(500)]
        public string Location { get; set; }
    }

    public class UserPasswordChangeLog
    {
        [Key]
        public Int64 Id { get; set; }
        [Required, StringLength(100)]
        public string UserRefId { get; set; }

        [Required]
        public DateTime LastChangedOn { get; set; }
    }

    public class User2FactorVerification
    {
        [Required, Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "ResponseId is required..!")]
        [StringLength(100, ErrorMessage = "The max length of ResponseId is 100 characters..!")]
        public string ResponseId { get; set; }
        public string UserRefId { get; set; }
        public DateTime CreatedOn { get; set; }
        public User2FactorVerificationTypeEnum User2FactorVerificationType { get; set; }
    }
}
