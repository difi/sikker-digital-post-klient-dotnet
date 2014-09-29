using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Header
    {
        private readonly XmlElement _dokument;

        public Header(XmlElement dokument)
        {
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            return null;
        }

        public XmlElement SecurityElement()
        {
            return null;
        }

        public XmlElement MessagingElement()
        {
            return null;
        }
    }
}
