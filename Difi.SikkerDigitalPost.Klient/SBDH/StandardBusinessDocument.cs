using System;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class StandardBusinessDocument
    {
        private StandardBusinessDocumentHeader _standardBusinessDocumentHeader;

        private Object any;

        
        public ForretningsMelding GetForretningsMelding()
        {
            return any as ForretningsMelding;
        }
    }
}
