using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace Divination.ACT.CustomTrigger
{
    public static class Utils
    {
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        private static readonly List<string> WorldNames = new List<string>
        {
            "Alexander", "Bahamut", "Durandal", "Fenrir", "Ifrit", "Ridill", "Tiamat", "Ultima", /* "Valefor",  */
            "Yojimbo", "Zeromus"
        };

        public static string RemoveSuffix(this string source, params string[] suffixes)
        {
            return suffixes.Aggregate(source,
                (before, suffix) =>
                    before.EndsWith(suffix) ? before.Substring(0, before.Length - suffix.Length) : before);
        }


        public static string RemoveUnicodePrefix(this string source)
        {
            if (source.Length == 0)
            {
                return source;
            }

            var regex = new Regex("^\\W");
            return regex.Replace(source, "");
        }

        public static string ReplaceSpecialCharacters(this string source)
        {
            return source.Replace("", ":one:")
                .Replace("", ":two:")
                .Replace("", ":three:")
                .Replace("", ":regional_indicator_s:")
                .Replace("", ":arrow_forward:")
                .Replace("", "ET")
                .Replace("", "HQ");
        }

        public static string ExtractWorldName(this string source)
        {
            return WorldNames.Find(source.EndsWith) ?? "Valefor";
        }
    }
}
