using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Header
    {
        private readonly XmlDocument _dokument;

        public Header(XmlDocument dokument)
        {
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            return _dokument.CreateElement("Header");
        }

        public XmlElement SecurityElement()
        {
            var securityElement = new Security(_dokument);
            return securityElement.Xml();
        }

        public XmlElement MessagingElement()
        {
            return null;
        }
    }
}
