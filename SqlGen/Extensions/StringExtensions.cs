using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlGen
{
    public static partial class StringExtensions
    {
        public static string ToPascalCase(this string sqlName)
        {
            return string.Join("", sqlName.Split('_').Select(w => w.ToLower()).Select(w => char.ToUpperInvariant(w[0]) + w.Substring(1)));
        }

        public static string ToPascalCaseV2(this string original)
        {
            original = original.ConvertToASCII();

            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }


        public static string ConvertToASCII(this string str)
        {
            //
            // Encoding.ASCII.GetString(Encoding.GetEncoding(1251).GetBytes(str)) yemez burada
            // 

            string responseStr = str;
            //Önce büyükler ...
            responseStr = responseStr.Replace("Ç", "C");
            responseStr = responseStr.Replace("Ğ", "G");
            responseStr = responseStr.Replace("İ", "I");
            responseStr = responseStr.Replace("Ö", "O");
            responseStr = responseStr.Replace("Ş", "S");
            responseStr = responseStr.Replace("Ü", "U");

            //Şimdi de çücükler ...

            responseStr = responseStr.Replace("ç", "c");
            responseStr = responseStr.Replace("ğ", "g");
            responseStr = responseStr.Replace("ı", "i");
            responseStr = responseStr.Replace("ö", "o");
            responseStr = responseStr.Replace("ş", "s");
            responseStr = responseStr.Replace("ü", "u");

            return responseStr;
        }

        public static string ToUnderLineCase(this string sqlName)
        {

            var result = Regex.Replace(sqlName, "(?<=[a-z0-9])[A-Z]", m => "_" + m.Value);

            return result.ToLowerInvariant();
        }

        public static string EmptyCheck(this string variable)
        {
            return variable == null ? "" : variable;
        }
    }
}