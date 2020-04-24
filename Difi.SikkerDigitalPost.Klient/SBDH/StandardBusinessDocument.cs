using System;
using System.Linq;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class StandardBusinessDocument
    {
        public StandardBusinessDocumentHeader standardBusinessDocumentHeader { get; set; }

        
        public Object any { get; set; }

        
        public ForretningsMelding GetForretningsMelding()
        {
            return any as ForretningsMelding;
        }

        public string GetConversationId()
        {
            return standardBusinessDocumentHeader.businessScope
                .scope
                .Where(scope => scope.type.Equals("ConversationId"))
                .ToList()
                .First()
                .instanceIdentifier;
        }
    }
}
