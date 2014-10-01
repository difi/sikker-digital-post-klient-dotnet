using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public class Envelope
    {
        private const string NsXmlnsEnv = "http://www.w3.org/2003/05/soap-envelope";
        private bool isCreated = false;

        private readonly Forsendelse _forsendelse;
        private readonly AsicEArkiv _asicEArkiv;
        private readonly Databehandler _databehandler;
        
        private readonly XmlDocument _envelopeXml;
        
        
        public Envelope(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler)
        {
            _forsendelse = forsendelse;
            _asicEArkiv = asicEArkiv;
            _databehandler = databehandler;
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
            var header = new Header(_envelopeXml, _forsendelse, _asicEArkiv, _databehandler);
            return header.Xml();
        }

        private XmlElement BodyElement()
        {
            var body = new BodyElement(_envelopeXml, _forsendelse, _asicEArkiv, _databehandler);
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
