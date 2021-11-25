using System;
using System.Globalization;

namespace TradeStops.Common.Utils
{
    public static class DecimalUtils
    {
        private const int MaxDecimalPlaces = 8;
        private const string UndefinedValueText = "N/A";

        // todo: Feel free to write summary description to make it easier to understand what is 'ExtendedFormattedString' (examples would be great)
        public static string ToExtendedFormattedString(decimal value, int maxDecimalPlaces)
        {
            string format = "#,0.00";
            int decimals = 2; // 2 decimal places in our default number format.

            if (value == 0)
            {
                return value.ToString(format, CultureInfo.CurrentUICulture);
            }

            if (Math.Abs(value) < 0.01m)
            {
                decimal formattedValue = Math.Abs(value);

                while (formattedValue < 0.01m && decimals < maxDecimalPlaces)
                {
                    formattedValue = formattedValue * 10;
                    format += "#";
                    decimals++;
                }
            }

            return value.ToString(format, CultureInfo.CurrentUICulture);
        }

        public static string ToExtendedFormattedString(decimal? value)
        {
            if (value.HasValue)
            {
                return ToExtendedFormattedString(value.Value, MaxDecimalPlaces);
            }

            return UndefinedValueText;
        }

        public static string ToExtendedFormattedString(decimal value)
        {
            return ToExtendedFormattedString(value, MaxDecimalPlaces);
        }

        public static string ToPercentString(decimal value)
        {
            return value.ToString("#,0.00", CultureInfo.CurrentUICulture);
        }

        public static string ToPercentStringWithPercentSign(decimal? value)
        {
            if (value == null)
            {
                return UndefinedValueText;
            }

            return value.Value.ToString("0.##\\%", CultureInfo.CurrentUICulture);
        }

        public static string ToPercentChangeStringWithPercentSign(decimal? value)
        {
            if (value == null)
            {
                return UndefinedValueText;
            }

            var signPrefix = value > 0 ? "+" : string.Empty;

            return $"{signPrefix}{value.Value.ToString("0.##\\%", CultureInfo.CurrentUICulture)}";
        }

        public static string ToRatioString(decimal? value)
        {
            if (value == null)
            {
                return UndefinedValueText;
            }

            return value.Value.ToString("0.##", CultureInfo.CurrentUICulture);
        }

        public static string ToPriceString(decimal value, string currency)
        {
            var signPrefix = value < 0 ? "-" : string.Empty;

            var absoluteValueFormatted = ToExtendedFormattedString(Math.Abs(value));

            return $"{signPrefix}{currency}{absoluteValueFormatted}";
        }

        public static string ToPriceChangeString(decimal value, string currency)
        {
            var signPrefix = value < 0 ? "-" : "+";

            var absoluteValueFormatted = ToExtendedFormattedString(Math.Abs(value));

            return $"{signPrefix}{currency}{absoluteValueFormatted}";
        }

        public static string ToPriceString(decimal? value, string currency)
        {
            if (value.HasValue)
            {
                return ToPriceString(value.Value, currency);
            }

            return UndefinedValueText;
        }

        public static string ToSharesAmountString(decimal? value)
        {
            if (value.HasValue)
            {
                return ToSharesAmountString(value.Value);
            }

            return UndefinedValueText;
        }

        public static string ToSharesAmountString(decimal value)
        {
            string format = Math.Abs(value) >= 0.01m ? "#,0.00" : "#,0.000#";
            return value.ToString(format, CultureInfo.CurrentUICulture);
        }
    }
}
