using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class DistrictLevelUserMapping
    {
        [Key]
        public Int64 DistrictLevelUserMappingId { get; set; }

        [Required(ErrorMessage = "UserRefId is required..!")]
        [ForeignKey("User")]
        public string UserRefId { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage = "District ref id is required..!")]
        [ForeignKey("Application_DistrictLgd")]
        public Int64 DistrictLgdRefId { get; set; }
        public virtual DistrictLgd Application_DistrictLgd { get; set; }
    }
}
