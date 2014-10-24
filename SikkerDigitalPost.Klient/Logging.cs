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
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SikkerDigitalPost.Klient
{
    internal class Logging
    {
        private static Action<TraceEventType, Guid?, string, string> _logAction = null;

        internal static void Initialize(Klientkonfigurasjon konfigurasjon)
        {
            _logAction = konfigurasjon.Logger;
        }

        internal static void Log(TraceEventType severity, string message, [CallerMemberName] string callerMember = null)
        {
            Log(severity, null, message, callerMember);
        }

        internal static void Log(TraceEventType severity, Guid? conversationId, string message, [CallerMemberName] string callerMember = null)
        {
            if (_logAction == null)
                return;

            if (callerMember == null)
                callerMember = new StackFrame(1).GetMethod().Name;

            _logAction(severity, conversationId, callerMember, message);
        }

        internal static Action<TraceEventType, Guid?, string, string> TraceLogger()
        {
            TraceSource _traceSource = new TraceSource("SikkerDigitalPost.Klient");
            return (severity, koversasjonsId, caller, message) =>
            {
                _traceSource.TraceEvent(severity, 1, "[{0}, {1}] {2}", koversasjonsId.GetValueOrDefault(), caller, message);
            };
        }
    }
}
