using System;

namespace TradeStops.Common.Helpers
{
    public static class TimeZoneHelper
    {
        public static DateTime GetSendDate(string timeZone, int alertStartTime, int alertEndTime)
        {
            var userTimezoneValue = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var serverTimezoneValue = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.StandardName);
            var userTimezoneSendDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimezoneValue);
            var serverTimezoneSendDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, serverTimezoneValue);

            var timezoneDifference = serverTimezoneSendDate - userTimezoneSendDate;

            if (userTimezoneSendDate.Hour < alertStartTime)
            {
                userTimezoneSendDate = new DateTime(userTimezoneSendDate.Year, userTimezoneSendDate.Month, userTimezoneSendDate.Day, alertStartTime, 0, 0);
            }
            else if (userTimezoneSendDate.Hour >= alertEndTime)
            {
                userTimezoneSendDate = new DateTime(userTimezoneSendDate.Year, userTimezoneSendDate.Month, userTimezoneSendDate.Day, alertStartTime, 0, 0).AddDays(1);
            }

            return userTimezoneSendDate.AddHours(timezoneDifference.TotalHours);
        }

        public static DateTime ConvertToUserDate(DateTime utcTime, string timeZone)
        {
            var timezoneValue = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timezoneValue);
        }

        public static string GetUserTimeZoneName(string timeZone)
        {
            var timezoneValue = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

            return timezoneValue.DisplayName;
        }
    }
}