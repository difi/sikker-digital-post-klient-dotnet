using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class BusinessScope
    {
        public List<Scope> scope
        {
            get { return _scopes;  }
            set { _scopes = value; }
        }
        
        private List<Scope> _scopes = new List<Scope>();
    }

    public class Scope
    {
        public string type { get; set; }

        public string instanceIdentifier { get; set; }

        public string identifier { get; set; }

        public List<CorrelationInformation> scopeInformation
        {
            get { return _scopeInformation; }
            set { _scopeInformation = value; }
        }
        
        private List<CorrelationInformation> _scopeInformation = new List<CorrelationInformation>();

    }

    public enum ScopeType
    {
        CONVERSATION_ID,
        SENDER_REF,
        RECEIVER_REF,
    }
}
