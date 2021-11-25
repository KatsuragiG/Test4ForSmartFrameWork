using System;

namespace TradeStops.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string firstString, string secondString)
        {
            // explanation why StringComparison.Ordinal:
            // https://stackoverflow.com/questions/492799/difference-between-invariantculture-and-ordinal-string-comparison
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/best-practices-strings
            return string.Equals(firstString, secondString, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsCaseInsensitive(this string str, string search)
        {
            return search != null && str != null && str.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string GetValueOrDefault(this string str, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(str) ? defaultValue : str;
        }

        public static string Truncate(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Substring(0, maxLength);
        }

        // http://stackoverflow.com/questions/5284591/how-to-remove-a-suffix-from-end-of-string
        // consider rename to TrimEnd
        public static string RemoveFromEnd(this string str, string suffix)
        {
            if (str.EndsWith(suffix))
            {
                return str.Substring(0, str.Length - suffix.Length);
            }
            else
            {
                return str;
            }
        }

        public static string RemoveFromStart(this string str, string postfix)
        {
            if (str.StartsWith(postfix))
            {
                return str.Substring(postfix.Length, str.Length - postfix.Length);
            }
            else
            {
                return str;
            }
        }

        // method is used only in cases when there's a chance to receive empty string instead of null in controller
        public static string GetValueOrNullIfEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            return str;
        }
    }
}
