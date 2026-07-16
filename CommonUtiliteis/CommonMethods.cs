using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class CommonMethods
    {
        public static string MaskMobileNumber(string mobileNumber)
        {
            if (string.IsNullOrEmpty(mobileNumber) || mobileNumber.Length < 6)
            {
                return mobileNumber;
            }

            string firstDigit = mobileNumber.Substring(0, 1);
            string lastFourDigits = mobileNumber.Substring(mobileNumber.Length - 4, 4);

            string maskedMiddle = new string('X', mobileNumber.Length - 5);

            string maskedMobileNumber = $"{firstDigit}{maskedMiddle}{lastFourDigits}";

            return maskedMobileNumber;
        }
    }
}
