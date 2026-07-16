using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class Holiday
    {
        [Key, Required]
        public Int64 HolidayId { get; set; }

        [Display(Name = "Holiday Name")]
        [Required(ErrorMessage = "Please enter Holiday Name !")]
        public string HolidayName { get; set; }

        [Required(ErrorMessage = "Holiday Date is required..!")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid holiday date!")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HolidayDate { get; set; }

        [Required(ErrorMessage = "IsWeekend is required..!")]
        public bool IsWeekend { get; set; }
    }
}
