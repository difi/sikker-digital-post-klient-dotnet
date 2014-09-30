using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class BodyElement
    {
        private XmlDocument _dokument;

        private const string NsWsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        private const string NsId = "";

        public BodyElement(XmlDocument dokument)
        {
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            var bodyElement = _dokument.CreateElement("env", "body", _dokument.NamespaceURI);
            bodyElement.SetAttribute("xmlns:wsu", NsWsu);

            return bodyElement;
        }
    }
}
