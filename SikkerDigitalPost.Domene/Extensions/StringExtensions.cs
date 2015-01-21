using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkerDigitalPost.Domene.Extensions
{
    internal static class StringExtensions
    {
        public static string RemoveIllegalCharacters(this string illegalString)
        {
            string illegalChars = "\"<>\\^`{}|~!#$&'()+,/:;=?@][";
            illegalString = illegalString.Replace(" ", "_");
            return illegalChars.Aggregate(illegalString, (current, c) => current.Replace(c.ToString(), ""));
        }
    }
}
