using System;
using TradeStops.Common.Constants;

// todo: CONSIDER to create separate assembly with utils that can be used in ANY project without tradesmith-related stuff
namespace TradeStops.Common.Utils
{
    public static class DateTimeUtils
    {
        public static string ToShortDateString(DateTime? value)
        {
            if (value.HasValue)
            {
                return value.Value.ToShortDateString();
            }

            return DateFormats.UndefinedDateString;
        }

        public static string ToUsaFormatTimeString(DateTime? value)
        {
            if (value.HasValue)
            {
                return ToUsaFormatTimeString(value.Value);
            }

            return DateFormats.UndefinedDateString;
        }

        public static string ToUsaFormatTimeString(DateTime value)
        {
            return value.ToString("hh:mm tt");
        }

        public static string ToLocalShortDateString(DateTime? value)
        {
            if (value.HasValue)
            {
                return ToLocalShortDateString(value.Value);
            }

            return DateFormats.UndefinedDateString;
        }

        public static string ToLocalShortDateString(DateTime value)
        {
            return value.ToLocalTime().ToShortDateString();
        }

        public static DateTime? ParseOrNull(string text)
        {
            DateTime date;
            if (DateTime.TryParse(text, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

        public static DateTime GetLastWeekDay(DateTime currentDate, DayOfWeek dayOfWeek)
        {
            var daysToAdd = ((int)dayOfWeek - (int)currentDate.DayOfWeek - 7) % 7;

            return currentDate.AddDays(daysToAdd);
        }
    }
}
