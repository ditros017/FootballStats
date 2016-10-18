using System;
using System.Data.SqlTypes;

namespace FootballStats.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsEmpty(this DateTime self)
        {
            return self == DateTime.MinValue;
        }

        public static DateTime Time(this DateTime date, DateTime time)
        {
            return date.Time(time.Hour, time.Minute, time.Second, time.Millisecond);
        }

        public static DateTime Time(this DateTime date, int hour, int minute = 0, int second = 0, int millisecond = 0)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second, millisecond);
        }

        public static DateTime NullifyDate(this DateTime date, bool sql = false)
        {
            var nullDate = !sql ? DateTime.MinValue : (DateTime) SqlDateTime.MinValue;
            return nullDate.Time(date.Hour, date.Minute, date.Second);
        }

        public static DateTime NullifyTime(this DateTime date)
        {
            return date.Time(0);
        }

        public static Int64 GetJsTime(this DateTime self)
        {
            return (Int64) ((self.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds + 0.5);
        }

        public static DateTime GetDateEnd(this DateTime date)
        {
            return date.AddDays(1).AddSeconds(-1);
        }

        public static string ToTimeString(this TimeSpan duration)
        {
            if (duration == TimeSpan.Zero)
                return "N/A";

            if (duration.Days == 1 || (duration.Days == 0 && duration.Minutes == 0))
                return $"{duration.TotalHours:0.#} h.";

            if (duration.Days == 0 && duration.Hours != 0 && duration.Minutes != 0)
                return $"{duration.Hours} h. {duration.Minutes} min.";

            if (duration.Hours == 0)
                return $"{duration.Minutes} min.";

            return $"{duration.TotalDays:0.#} days";
        }
    }
}