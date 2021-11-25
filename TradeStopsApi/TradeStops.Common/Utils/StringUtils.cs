using System.Linq;

namespace TradeStops.Common.Utils
{
    public static class StringUtils
    {
        public static string LeaveOnlyDigits(string input)
        {
            if (input == null)
            {
                return null;
            }

            ////return new string(input.ToCharArray().Where(char.IsDigit).ToArray());
            return string.Concat(input.Where(char.IsDigit));
        }

        public static string UpCaseFirstLetter(string input)
        {
            if (input == null)
            {
                return null;
            }

            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
