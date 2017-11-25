using System.Linq;

namespace Axoom.Extensions.Logging.Console
{
    internal static class StringExtensions
    {
        public static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Where(@char => char.IsLetterOrDigit(@char) || @char.Equals('_'))
                                    .Select((@char, i) => i > 0 && char.IsUpper(@char) ? "_" + @char.ToString() : @char.ToString()))
                         .ToLowerInvariant()
                         .EnsureLeadingUnderscore();
        }

        private static string EnsureLeadingUnderscore(this string str)
        {
            if (!str.StartsWith("_"))
                return "_" + str;

            return str;
        }
    }
}