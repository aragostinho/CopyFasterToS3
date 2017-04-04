using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class Utils
    {
        public static string RemoveLastSlash(this string text)
        {
            if (text.EndsWith(@"\"))
                return text.Remove(text.Length, 1);

            return text;
        }
        public static string RemoveRootUnit(this string text)
        {
            if (text.Contains(@":\"))
                return text.Substring(2, text.Length-2);

            return text;
        }

        public static string ToS3KeyName(this string text)
        {
            string returnText = string.Empty; 

            if (text.StartsWith(@"\"))
                returnText = text.Substring(1, text.Length-1);

            returnText = returnText.Replace(@"\", "/");

            return returnText;
        }
    }
}
