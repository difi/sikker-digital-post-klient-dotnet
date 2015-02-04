using System;

namespace SikkerDigitalPost.Klient.Extensions
{
    internal static class DateTimeExtensions
    {
        public static string ToStringWithUtcOffset(this DateTime dateTime)
        {
            const string fullFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff";

            string date = dateTime.ToString(fullFormat);

            TimeZoneInfo timeZone = TimeZoneInfo.Local;
            TimeSpan offset = timeZone.GetUtcOffset(dateTime);

            var format = @"hh':'mm";
            var fortegn = offset.CompareTo(TimeSpan.Zero) >= 0 ? "'+'" : "'-'";
            format = String.Format("{0}{1}", fortegn, format);
            
            return dateTime.ToString(String.Format("{0}{1}", date, offset.ToString(format)));
        }

    }
}
