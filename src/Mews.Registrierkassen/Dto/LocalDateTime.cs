using System;

namespace Mews.Registrierkassen.Dto
{
    public sealed class LocalDateTime
    {
        public static TimeZoneInfo AustrianTimezone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");

        public LocalDateTime(DateTime dateTime, TimeZoneInfo timezoneInfo)
        {
            DateTime = dateTime;
            TimeZoneInfo = timezoneInfo;
        }

        public static LocalDateTime Now
        {
            get { return new LocalDateTime(DateTime.UtcNow, TimeZoneInfo.Utc); }
        }

        public DateTime DateTime { get; }

        public TimeZoneInfo TimeZoneInfo { get; }
    }
}
