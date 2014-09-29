using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public class Envelope
    {
        public const string NsXlmnsEnv = "http://www.w3.org/2003/05/soap-envelope";

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
            return null;
        }
    }
}
