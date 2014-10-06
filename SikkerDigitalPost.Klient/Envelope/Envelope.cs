using System;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Envelope.EnvelopeHeader;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class Envelope
    {
        public readonly XmlDocument EnvelopeXml;
        private bool _isCreated = false;

        public readonly Forsendelse Forsendelse;
        public readonly AsicEArkiv AsicEArkiv;
        public readonly Databehandler Databehandler;
        public readonly GuidUtility GuidUtility;
        private Header _header;
        private byte[] _bytes;

        public Envelope(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler, GuidUtility guidUtility)
        {
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
            GuidUtility = guidUtility;
            EnvelopeXml = XmlEnvelope();
        }

        public byte[] Bytes
        {
            get { return _bytes ?? (_bytes = Encoding.UTF8.GetBytes(Xml().OuterXml)); }
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
