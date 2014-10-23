using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SikkerDigitalPost.Klient
{
    internal class Logging
    {
        private static TraceSource _traceSource = null;

        private static Action<TraceEventType, Guid?, string, string> _logAction = null;

        internal static void Initialize(Klientkonfigurasjon konfigurasjon)
        {
            if (konfigurasjon != null && konfigurasjon.Logger != null)
                _logAction = konfigurasjon.Logger;
            else
            {
                _traceSource = new TraceSource(typeof(SikkerDigitalPostKlient).Assembly.FullName);

                _logAction = (severity, koversasjonsId, caller, message) =>
                {
                    _traceSource.TraceEvent(severity, 1, "[{0}, {1}] {2}", konfigurasjon.ToString(), caller, message);
                };
            }
        }

        internal static void Log(TraceEventType severity, string message, [CallerMemberName] string callerMember = null)
        {
            Log(severity, null, message, callerMember);
        }

        internal static void Log(TraceEventType severity, Guid? koversasjonsId, string message, [CallerMemberName] string callerMember = null)
        {
            if (_logAction == null)
                return;

            if (callerMember == null)
                callerMember = new StackFrame(1).GetMethod().Name;

            _logAction(severity, koversasjonsId, callerMember, message);
        }
    }
}
