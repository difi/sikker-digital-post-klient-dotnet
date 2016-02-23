using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToStringWithUtcOffset(this DateTime dateTime)
        {
            return dateTime.ToString("O");
        }
    }
}