using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Circle
    {
        [Key, Required]
        public Int64 CircleId { get; set; }

        [Required(ErrorMessage = "Circle Name is required..!")]
        [StringLength(100)]
        public string CircleName { get; set; }

        [Required(ErrorMessage = "CircleType is required..!")]
        public CircleTypeEnum CircleType { get; set; }

        [Required(ErrorMessage = "Juridiction Area is required..!")]
        public string JuridictionArea { get; set; }

        [Required(ErrorMessage = "ALCCircleRefId is required..!")]
        public Int64 ALCCircleRefId { get; set; }

        [Required(ErrorMessage = "Max Inspection Count is required..!")]
        public Int64 MaxInspectionCountLimit { get; set; }

        [ForeignKey("DistrictLgdRefId")]
        public Int64 DistrictLgdRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        [Required(ErrorMessage = "Medical Circle ID is required..!")]
        public Int64 MedicalCircleID { get; set; }

        [Required(ErrorMessage = "Joint Dir Circle ID is required..!")]
        public Int64 JointDirCircleID { get; set; }
    }
}
