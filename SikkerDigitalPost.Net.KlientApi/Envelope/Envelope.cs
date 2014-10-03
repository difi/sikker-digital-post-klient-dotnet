using System.Text;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Net.Domene.Entiteter.Post;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    internal class Envelope
    {
        private readonly XmlDocument _envelopeXml;
        private bool _isCreated = false;

        private readonly Forsendelse _forsendelse;
        private readonly AsicEArkiv _asicEArkiv;
        private readonly Databehandler _databehandler;
        private Header _header;
        private byte[] _bytes;

        public Envelope(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler)
        {
            _forsendelse = forsendelse;
            _asicEArkiv = asicEArkiv;
            _databehandler = databehandler;
            _envelopeXml = XmlEnvelope();
        }

        public byte[] Bytes
        {
            get { return _bytes ?? (_bytes = Encoding.UTF8.GetBytes(Xml().OuterXml)); }
        }

        public XmlDocument Xml()
        {
            if (_isCreated) return _envelopeXml;

            _envelopeXml.DocumentElement.AppendChild(HeaderElement());
            _envelopeXml.DocumentElement.AppendChild(BodyElement());
            _header.AddSignatureElement();
            _isCreated = true;

            return _envelopeXml;
        }

        private XmlDocument XmlEnvelope()
        {
            var xmlDokument = new XmlDocument();
            var xmlDeclaration = xmlDokument.CreateXmlDeclaration("1.0", "UTF-8", null);
            var baseNode = xmlDokument.CreateElement("env", "Envelope", Navnerom.env);
            xmlDokument.AppendChild(baseNode);
            xmlDokument.InsertBefore(xmlDeclaration, xmlDokument.DocumentElement);
            return xmlDokument;
        }

        private XmlElement HeaderElement()
        {
            _header = new Header(_envelopeXml, _forsendelse, _asicEArkiv, _databehandler);
            return _header.Xml();
        }

        private XmlElement BodyElement()
        {
            var body = new EnvelopeBody.Body(_envelopeXml, _forsendelse, _asicEArkiv, _databehandler);
            return body.Xml();
        }

        public void SkrivTilFil(string filsti)
        {
            if (!_isCreated)
                Xml();
            
            _envelopeXml.Save(filsti);
        }
    }
}
