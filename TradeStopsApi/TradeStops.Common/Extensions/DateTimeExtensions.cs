using System;
using TradeStops.Common.Constants;

namespace TradeStops.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateString(this DateTime? value)
        {
            if (value.HasValue)
            {
                return value.Value.ToShortDateString();
            }

            return DateFormats.UndefinedDateString;
        }
    }
}