using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public class Envelope
    {
        public const string NsXlmnsEnv = "http://www.w3.org/2003/05/soap-envelope";

        public Arkiv arkiv { get; set; }
        
        private XmlElement _envelopeXml;
        
        public Envelope()
        {
            _envelopeXml = EnvelopeDokument();
        }

        private XmlDocument EnvelopeDokument()
        {
            var xmlDokument = new XmlDocument();
            var xmlDeclaration = xmlDokument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDokument.AppendChild(xmlDokument.CreateElement("manifest", NsXlmnsEnv));
            xmlDokument.InsertBefore(xmlDeclaration, xmlDokument.DocumentElement);
            return xmlDokument;
        }

        private XmlElement HeaderElement()
        {
            var header = new Header(_envelopeXml);
            //_envelopeXml.DocumentElement.AppendChild(header.Element);
            return null;
        }

        private XmlElement BodyElement()
        {
            return null;
        }
    }
}
