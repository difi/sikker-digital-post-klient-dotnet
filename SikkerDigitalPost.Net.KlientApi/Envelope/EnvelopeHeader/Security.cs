using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    class Security
    {
        private readonly XmlDocument _dokument;

        public Security(XmlDocument dokument)
        {
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            return null;
        }
    }
}
