using System.Linq;

namespace Difi.SikkerDigitalPost.Klient.Domene.Extensions
{
    internal static class StringExtensions
    {
        public static string RemoveIllegalCharacters(this string illegalString)
        {
            const string illegalChars = "\"<>\\^`{}|~!#$&'()+,/:;=?@][";
            illegalString = illegalString.Replace(" ", "_");
            return illegalChars.Aggregate(illegalString, (current, c) => current.Replace(c.ToString(), ""));
        }
    }
}