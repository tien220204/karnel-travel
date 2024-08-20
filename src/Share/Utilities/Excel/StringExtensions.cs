using System.Text.RegularExpressions;

namespace KarnelTravel.Share.Utilities.Excel
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        // Replicate SQL IN functionality
        // if (Variable.In("AC", "BC", "EA"))
        public static bool In(this string me, params string[] set)
        {
            return set.Contains(me);
        }

        public static bool NotIn(this string me, params string[] set)
        {
            return !set.Contains(me);
        }

        public static bool NotIn(this string me, IEnumerable<string> set)
        {
            return !set.Contains(me);
        }
    }
}
