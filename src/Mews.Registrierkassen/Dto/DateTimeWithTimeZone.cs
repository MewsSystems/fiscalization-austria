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
                return new DateTimeWithTimeZone(DateTime.UtcNow, TimeZoneInfo.Utc);
            }
        }

        public DateTime DateTime { get; }

        public TimeZoneInfo TimeZoneInfo { get; }
    }
}
