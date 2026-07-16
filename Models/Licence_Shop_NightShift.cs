using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Licence_Shop_NightShift_Approval
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "LicenceNumber is required..!")]
        public string LicenceNumber { get; set; }

        [Required(ErrorMessage = "CheckListJson is required..!")]
        public string CheckListJson { get; set; }

        [NotMapped]
        public Int64 ProjectSiteRefId { get; set; }

        [NotMapped]
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }

        [NotMapped]
        public Int64 IPin { get; set; }

        [NotMapped]
        public Int64 InvestPunjab_AppId { get; set; }

        [NotMapped]
        public List<Licence_Shop_NightShift_ChecklistPoint> NightShift_ChecklistPoints { get; set; }

        [NotMapped]
        public int ProjectSiteVersion { get; set; }
    }
    public class Licence_Shop_NightShift_ChecklistPoint
    {
        [Key]
        public Int64 ChecklistID { get; set; }

        [Required(ErrorMessage = "ChecklistDescription is required..!")]
        public string ChecklistDescription { get; set; }

        [Required(ErrorMessage = "DateOfEffective is required..!")]
        public DateTime DateOfEffective { get; set; }

        [Required(ErrorMessage = "OrderNumber is required..!")]
        [StringLength(100)]
        public string OrderNumber { get; set; }
        public Boolean IsEnabled { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

    }
}
