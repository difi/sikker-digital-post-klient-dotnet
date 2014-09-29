using System.Xml;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public class Envelope
    {
        private const string NsXmlnsEnv = "http://www.w3.org/2003/05/soap-envelope";
        
        public Arkiv arkiv { get; set; }
        
        private XmlDocument _envelopeXml;
        
        public Envelope()
        {
            _envelopeXml = EnvelopeDokument();
        }

        private XmlDocument EnvelopeDokument()
        {
            var xmlDokument = new XmlDocument();
            return xmlDokument;
        }

        private XmlElement HeaderElement()
        {
            //var header = new Header(_envelopeXml.CreateElement("header", "namespace", "..."));
            //_envelopeXml.DocumentElement.AppendChild(header.Element);
            return null;
        }

        private XmlElement BodyElement()
        {
            var body = new BodyElement(_envelopeXml);
            return body.Xml();
        }
    }
}
