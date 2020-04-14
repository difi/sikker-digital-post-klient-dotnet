using System;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class CorrelationInformation
    {
        public DateTime requestingDocumentCreationDateTime { get; set; }
        public string requestingDocumentInstanceIdentifier { get; set; }
        public DateTime expectedResponseDateTime { get; set; }
    }
}
