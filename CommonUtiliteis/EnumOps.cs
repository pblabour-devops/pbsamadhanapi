using Microsoft.AspNetCore.Mvc.Rendering;
using pbsamadhannetcoreapi.CommonUtiliteis.CustomeAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class EnumOps
    {
        public static SelectList GetEnumAsSelectList<TEnum>() where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            var enumData = from TEnum e in Enum.GetValues(typeof(TEnum))
                           select new
                           {
                               ID = GetEnumValue<TEnum>(e.ToString()),
                               Name = GetEnumDescriptionName<TEnum>(e.ToString())
                           };
            return new SelectList(enumData, "ID", "Name");
        }

        public static string GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            return  ((int)Enum.Parse(typeof(T), str)).ToString();
        }

        public static string GetEnumDescriptionName<TEnum>(string nameVal) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = typeof(TEnum).GetMember(nameVal); //TEnum.GetType().GetMember(TEnum.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false)
                                   as DescriptionAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].Description;
        }
        public static string GetEnumShortDescriptionName<TEnum>(string nameVal) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = typeof(TEnum).GetMember(nameVal); //TEnum.GetType().GetMember(TEnum.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(CustomeAttribute_EnumShortDesc), false)
                                   as CustomeAttribute_EnumShortDesc[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].ShortDesc;
        }
    }
}
