using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Blueprint.Utils
{
    public class Utility
    {
        public static string EnterByDash(string value)
        {
            string[] words = value.Split(' ');
            string result = "";

            foreach (var word in words)
            {
                result = result + word + "-";
            }

            return new String(result.Take(result.Length - 1).ToArray());
        }

        public static string ConvertToUnsign(string str)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty)
                        .Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}