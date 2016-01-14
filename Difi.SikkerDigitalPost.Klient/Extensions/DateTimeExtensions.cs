using System;

namespace Difi.SikkerDigitalPost.Klient.Extensions
{
    internal static class DateTimeExtensions
    {
        public static string ToStringWithUtcOffset(this DateTime dateTime)
        {
            return dateTime.ToString("O");
        }

    }
}
