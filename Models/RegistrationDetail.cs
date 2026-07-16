using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class RegistrationDetail
    {
        [Key, Required]
        public Int64 RegistrationDetailId { get; set; }

        [Required(ErrorMessage = "RegistrationNumber is Required..!")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Registration date is required..!")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Registration type is Required..!")]
        public ApplicationTypeEnum RegistrationType { get; set; }

        #region Foreign Key Relation
        
        [Required(ErrorMessage = "EstablishmentRefId is required..!")]
        [ForeignKey("Establishment_GeneralDetail")]
        public Int64 EstablishmentRefId { get; set; }
        public virtual Establishment_GeneralDetail Establishment_GeneralDetail { get; set; }

        #endregion Foreign Key Relation
    }
}
