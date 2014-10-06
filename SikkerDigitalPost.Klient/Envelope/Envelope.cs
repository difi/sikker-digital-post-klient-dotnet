using System;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Envelope.EnvelopeBody;
using SikkerDigitalPost.Klient.Envelope.EnvelopeHeader;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class Envelope : ISoapVedlegg
    {
        private readonly XmlDocument _envelopeXml;
        private bool _isCreated = false;

        private readonly Forsendelse _forsendelse;
        private readonly AsicEArkiv _asicEArkiv;
        private readonly Databehandler _databehandler;
        private Header _header;
        private byte[] _bytes;
        private string _contentId;

        public Envelope(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler)
        {
            _forsendelse = forsendelse;
            _asicEArkiv = asicEArkiv;
            _databehandler = databehandler;
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
            var body = new Body(_envelopeXml, _forsendelse, _asicEArkiv, _databehandler);
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
