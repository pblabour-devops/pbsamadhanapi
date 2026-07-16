using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class NICCode
    {
        [Key, Required]
        public Int64 NicCodeId { get; set; }

        [Required, StringLength(10)]
        public string NicCode { get; set; }

        [StringLength(10)]
        public string ParentNicCode { get; set; }

        [StringLength(30)]
        public string CodeName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }
    }
}
