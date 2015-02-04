/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Runtime.Remoting.Messaging;

namespace SikkerDigitalPost.Klient.Utilities
{
    internal static class DateUtility
    {
        public const string DateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffZ";

        private const string DateFormatUtcStart = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff";

        public static string UtcWithOffset(DateTime dateTime)
        {
            string date = dateTime.ToString(DateFormatUtcStart);

            TimeZoneInfo timeZone = TimeZoneInfo.Local;
            TimeSpan offset = timeZone.GetUtcOffset(dateTime);

            var format = @"hh':'mm";
            var fortegn = offset.CompareTo(TimeSpan.Zero) > 0 ? "'+'" : "'-'";
            format = String.Format("{0}{1}", fortegn, format);

            return dateTime.ToString(String.Format("{0}{1}", date, offset.ToString(format)));
            
        }

        public static string DateForFile()
        {
            return DateTime.Now.ToString("yyyy'-'MM'-'dd HH'.'mm'.'ss");
        } 
    }
}
