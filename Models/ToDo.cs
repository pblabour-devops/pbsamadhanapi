using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class ToDoApplicationActivityMaping
    {
        [Key]
        public Int64 Id { get; set; }

        //[Required(ErrorMessage = "ApplicationType is required..!")]
        //public Int64 ToDoActivityMapingId { get; set; }

        [Required(ErrorMessage = "ApplicationType is required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [Required(ErrorMessage = "ApplicationPurposeType is required..!")]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [Required(ErrorMessage = "ToDoCodeType is required..!")]
        public ToDoCodeTypeEnum ToDoCodeType { get; set; }

        [Required(ErrorMessage = "ToDoActivityModeType is required..!")]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [Required(ErrorMessage = "ToDoActivityCategoryType is required..!")]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [Required(ErrorMessage = "ToDoSerialOrderCount is required..!")]
        public int ToDoSerialOrderCount { get; set; }

        [Required(ErrorMessage = "Version is required..!")]
        public int Version { get; set; }


        [NotMapped]
        public string ApplicationTypeDesc { get; set; }
        [NotMapped]
        public string ApplicationPurposeTypeDesc { get; set; }
        [NotMapped]
        public string ToDoCodeTypeDesc { get; set; }
        [NotMapped]
        public string ToDoActivityModeTypeDesc { get; set; }
        [NotMapped]
        public string ToDoActivityCategoryTypeDesc { get; set; }
    }
    public class ToDoActivityLog
    {
        [Key]
        public Int64 ToDoActivityLogId { get; set; }

        [Required(ErrorMessage = "RootActivityRefId is required..!")]
        [StringLength(100, ErrorMessage = "RootActivityRefId lenght is 100")]
        public string RootActivityRefId { get; set; }

        [Required(ErrorMessage = "AppRefId is required..!")]
        public Int64 AppRefId { get; set; }

        [Required(ErrorMessage = "ApplicationType is required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [Required(ErrorMessage = "ApplicationPurposeType is required..!")]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [Required(ErrorMessage = "ToDoCodeType is required..!")]
        public ToDoCodeTypeEnum ToDoCodeType { get; set; }

        [Required(ErrorMessage = "ToDoActivityModeType is required..!")]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [Required(ErrorMessage = "ToDoActivityMapingVersion is required..!")]
        public int ToDoActivityMapingVersion { get; set; }

        [Required(ErrorMessage = "ToDoActivityCompleteType is required..!")]
        public ToDoActivityCompleteTypeEnum ToDoActivityCompleteType { get; set; }

        [Required(ErrorMessage = "ToDoActivityCategoryType is required..!")]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }

        [Required(ErrorMessage = "ActivityFailedMessage is required..!")]
        public string ActivityFailedMessage { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        [StringLength(100)]
        public string UserRefId { get; set; }

        [Required(ErrorMessage = "InvestPunjab_Ipin is required..!")]
       public Int64 InvestPunjab_Ipin { get; set; }

        [Required(ErrorMessage = "InvestPunjab_AppId is required..!")]
        public Int64 InvestPunjab_AppId { get; set; }

        [Required(ErrorMessage = "TimeStemp is required..!")]
        public DateTime TimeStemp { get; set; }
    }

    public class ToDoApplicationWiseManpowerMapping
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "ApplicationType is required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [Required(ErrorMessage = "ApplicationPurposeType is required..!")]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        
        [Required(ErrorMessage = "UserRefId is required..!")]
        [StringLength(100)]
        public string ManpowerUserRefId { get; set; }
        public virtual ICollection<ToDoTicket> ToDoTickets { get; set; }

    }

    public class ToDoTicket
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "RootActivityRefId is required..!")]
        [StringLength(100, ErrorMessage = "RootActivityRefId lenght is 100")]
        public string RootActivityRefId { get; set; }

        [Required(ErrorMessage = "ManpowerMappingRefId is required..!")]
        [ForeignKey("ToDoApplicationWiseManpowerMapping")]
        public Int64 ManpowerMappingRefId { get; set; }
        public virtual ToDoApplicationWiseManpowerMapping ToDoApplicationWiseManpowerMapping { get; set; }
        public DateTime CreatedOn { get; set; }
        public ToDoTicketStatusTypeEnum ToDoTicketStatusType { get; set; }
    }

    public class TestMaster
    {
        [Key, Required]
        public Int64 Id { get; set; }

        public int Value1 { get; set; }

        public int Value2 { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }


        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }



        [NotMapped]
        public ApplicationTypeEnum ApplicationType { get; set; }
        
        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }

        [NotMapped]
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }
        //[NotMapped]
        public string RootActivityRefId { get; set; }

        [NotMapped]
        public Int64 IPin { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

    }


    public class ToDoTableRootActivityMapping
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "RootActivityRefId is required..!")]
        [StringLength(50)]
        public string RootActivityRefId { get; set; }

        [Required(ErrorMessage = "EntityModelName is required..!")]
        public string EntityModelName { get; set; }

        [Required(ErrorMessage = "PrimaryKeyValue is required..!")]
        public Int64 PrimaryKeyValue { get; set; }

        [Required(ErrorMessage = "ToDoCodeType is required..!")]
        public ToDoCodeTypeEnum ToDoCodeType { get; set; }
    }

}
