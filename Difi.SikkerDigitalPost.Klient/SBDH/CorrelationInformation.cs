using System;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class CorrelationInformation
    {
        protected DateTime requestingDocumentCreationDateTime;
        protected string requestingDocumentInstanceIdentifier;
        protected DateTime expectedResponseDateTime;
    }
}
