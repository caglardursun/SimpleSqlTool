﻿using System.Linq;
using System.Text.RegularExpressions;

namespace SqlGen
{
    public static partial class StringExtensions
    {
        public static string ToPascalCase(this string sqlName)
        {
            return string.Join("", sqlName.Split('_').Select(w => w.ToLower()).Select(w => char.ToUpperInvariant(w[0]) + w.Substring(1)));
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