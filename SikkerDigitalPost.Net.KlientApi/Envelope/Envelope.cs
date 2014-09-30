using System.Xml;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public class Envelope
    {
        private const string NsXmlnsEnv = "http://www.w3.org/2003/05/soap-envelope";
        private bool isCreated = false;
        
        public Arkiv arkiv { get; set; }
        
        private readonly XmlDocument _envelopeXml;
         
        
        public Envelope()
        {
            _envelopeXml = EnvelopeDokument();
        }

        public XmlDocument Xml()
        {
            if (isCreated) return _envelopeXml;

            _envelopeXml.DocumentElement.AppendChild(HeaderElement());
            _envelopeXml.DocumentElement.AppendChild(BodyElement());

            return _envelopeXml;
        }

        private XmlDocument EnvelopeDokument()
        {
            var xmlDokument = new XmlDocument();
            var xmlDeclaration = xmlDokument.CreateXmlDeclaration("1.0", "UTF-8", null);
            var baseNode = xmlDokument.CreateElement("env", "Envelope", NsXmlnsEnv);
            xmlDokument.AppendChild(baseNode);
            xmlDokument.InsertBefore(xmlDeclaration, xmlDokument.DocumentElement);
            return xmlDokument;
        }

        private XmlElement HeaderElement()
        {
            var header = new Header(_envelopeXml);
            return header.Xml();
        }

        private XmlElement BodyElement()
        {
            var body = new BodyElement(_envelopeXml);
            return body.Xml();
        }

        public void SkrivTilFil(string filsti)
        {
            if (!isCreated)
                Xml();
            
            _envelopeXml.Save(filsti);
        }
    }
}
