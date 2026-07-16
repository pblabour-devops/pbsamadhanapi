using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class ExtensionMethods
    {
        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        public static bool IsNullOrWhitespace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }
    }
}
