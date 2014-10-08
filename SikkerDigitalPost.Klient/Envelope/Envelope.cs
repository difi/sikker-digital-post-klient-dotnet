using System;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Klient.Envelope.EnvelopeBody;
using SikkerDigitalPost.Klient.Envelope.EnvelopeHeader;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class Envelope : ISoapVedlegg
    {
        private readonly XmlDocument _envelopeXml;
        private bool _isCreated = false;

        private EnvelopeSettings _settings;
        private Header _header;
        private byte[] _bytes;
        private string _contentId;

        public Envelope(EnvelopeSettings settings)
        {
            _settings = settings;
            _envelopeXml = XmlEnvelope();
        }


        public string Filnavn
        {
            get { return "envelope.xml"; }
        }

        public byte[] Bytes
        {
            get { return _bytes ?? (_bytes = Encoding.UTF8.GetBytes(Xml().OuterXml)); }
        }

        public string Innholdstype
        {
            get { return "application/soap+xml; charset=UTF-8"; }
        }

        public string ContentId
        {
            get { return _contentId ?? (_contentId = String.Format("{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid())); }
        }

        public string TransferEncoding
        {
            get { return "binary"; }
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
            xmlDokument.PreserveWhitespace = true;
            var xmlDeclaration = xmlDokument.CreateXmlDeclaration("1.0", "UTF-8", null);
            var baseNode = xmlDokument.CreateElement("env", "Envelope", Navnerom.env);
            xmlDokument.AppendChild(baseNode);
            xmlDokument.InsertBefore(xmlDeclaration, xmlDokument.DocumentElement);
            return xmlDokument;
        }

        private XmlNode HeaderElement()
        {
            _header = new Header(_settings, _envelopeXml);
            return _header.Xml();
        }

        private XmlNode BodyElement()
        {
            var body = new Body(_settings, _envelopeXml);
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
