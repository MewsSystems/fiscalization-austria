using System;

namespace Mews.Registrierkassen.Dto
{
    public sealed class DateTimeWithTimeZone
    {
        public static TimeZoneInfo AustrianTimezone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");

        public DateTimeWithTimeZone(DateTime dateTime, TimeZoneInfo timezoneInfo)
        {
            DateTime = dateTime;
            TimeZoneInfo = timezoneInfo;
        }

        public static DateTimeWithTimeZone Now
        {
            get
            {
                var dateTime = DateTime.UtcNow;
                var austrianTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, AustrianTimezone);
                return new DateTimeWithTimeZone(austrianTime, AustrianTimezone);
            }
        }

        public DateTime DateTime { get; }

        public TimeZoneInfo TimeZoneInfo { get; }
    }
}
