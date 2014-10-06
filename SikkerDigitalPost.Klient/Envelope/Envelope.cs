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
        public readonly XmlDocument EnvelopeXml;
        private bool _isCreated = false;

        public readonly Forsendelse Forsendelse;
        public readonly AsicEArkiv AsicEArkiv;
        public readonly Databehandler Databehandler;
        public readonly GuidHandler GuidHandler;
        private Header _header;
        private byte[] _bytes;
        private string _contentId;

        public Envelope(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler, GuidHandler guidHandler)
        {
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
            EnvelopeXml = XmlEnvelope();
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
            if (_isCreated) return EnvelopeXml;

            EnvelopeXml.DocumentElement.AppendChild(HeaderElement());
            EnvelopeXml.DocumentElement.AppendChild(BodyElement());
            _header.AddSignatureElement();
            _isCreated = true;

            return EnvelopeXml;
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
            _header = new Header(this);
            return _header.Xml();
        }

        private XmlElement BodyElement()
        {
            var body = new EnvelopeBody.Body(this);
            return body.Xml();
        }

        public void SkrivTilFil(string filsti)
        {
            if (!_isCreated)
                Xml();
            
            EnvelopeXml.Save(filsti);
        }
    }
}
