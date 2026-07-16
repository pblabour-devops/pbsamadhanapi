using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class TestExcelFileFormat
    {
        [Required(ErrorMessage ="Name is required")]
        [StringLength(100, ErrorMessage ="Emp name 100 chars are allowed")]
        public string Employee_Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1,100, ErrorMessage ="Age value should be between 1 to 100")]
        public int Age { get; set; }

        //public Int16 int16 { get; set; }
        //public Int32 int32 { get; set; }
        //public Int64 int64 { get; set; }

        //public decimal ddd { get; set; }

        //public Decimal DDD { get; set; }

        //public DateTime DT { get; set; }

        //[StringLength(50, ErrorMessage = "Father name 50 chars are allowed")]
        //public string Father_Name { get; set; }
        public EstablishmentTypeEnum EstablishmentType { get; set; }
    }
}
