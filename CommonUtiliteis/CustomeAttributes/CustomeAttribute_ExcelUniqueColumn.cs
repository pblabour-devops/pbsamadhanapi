using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis.CustomeAttributes
{
    //[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class CustomeAttribute_ExcelUniqueColumn : Attribute
    {
        //public bool allowDuplicateInSheet;

        public CustomeAttribute_ExcelUniqueColumn(bool allowDuplicateInSheet, string candidateColumnsCommaSeparated, string errorMessage)
        {
            AllowDuplicateInSheet = allowDuplicateInSheet;
            CandidateColumnsCommaSeparated = candidateColumnsCommaSeparated;
            ErrorMessage = errorMessage;
        }
        public bool AllowDuplicateInSheet { get; set; }
        public string CandidateColumnsCommaSeparated { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class CustomeAttribute_EnumShortDesc : Attribute
    {
        public CustomeAttribute_EnumShortDesc(string shortDesc)
        {
            ShortDesc = shortDesc;
        }
        public string ShortDesc { get; set; }
    }
}
