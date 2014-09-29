using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class StandardBusinessDocumentHeader
    {
        private readonly XmlDocument _dokument;

        public StandardBusinessDocumentHeader(XmlDocument dokument)
        {
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            var sbdHeaderElement = _dokument.CreateElement("ns3", "StandardBusinessDocumentHeader", _dokument.NamespaceURI);
            var headerVersion = _dokument.CreateElement("ns3", "StandarBusinessDocumentHeader", _dokument.NamespaceURI);
            headerVersion.InnerText = "1.0";
            sbdHeaderElement.AppendChild(headerVersion);

            return sbdHeaderElement;
        }
    }
}
