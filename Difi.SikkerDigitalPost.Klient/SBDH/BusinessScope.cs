using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class BusinessScope
    {
        public List<Scope> scope = new List<Scope>();
    }

    public class Scope
    {
        public string type;

        public string instanceIdentifier;

        public string identifier;

        public List<CorrelationInformation> scopeInformation = new List<CorrelationInformation>();
    }

    public enum ScopeType
    {
        CONVERSATION_ID,
        SENDER_REF,
        RECEIVER_REF,
    }
}
