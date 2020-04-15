using System;

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
    }
}
