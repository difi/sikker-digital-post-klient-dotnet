using System.Xml;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public class Envelope
    {
        private const string NsXmlnsEnv = "http://www.w3.org/2003/05/soap-envelope";
        
        public Arkiv arkiv { get; set; }
        
        private readonly XmlDocument _envelopeXml;
        
        public Envelope()
        {
            _envelopeXml = EnvelopeDokument();
        }
        
        private XmlDocument EnvelopeDokument()
        {
            var xmlDokument = new XmlDocument();
            var xmlDeclaration = xmlDokument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDokument.AppendChild(xmlDokument.CreateElement("manifest", NsXmlnsEnv));
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
            var body = new BodyElement(_envelopeXml);
            return body.Xml();
        }

        private void SkrivTilFil(string filsti)
        {
            _envelopeXml.Save(filsti);
        }
    }
}
