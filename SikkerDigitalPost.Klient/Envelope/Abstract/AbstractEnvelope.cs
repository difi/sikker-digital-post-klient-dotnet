using System;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene;
using SikkerDigitalPost.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class AbstractEnvelope : ISoapVedlegg
    {
        protected readonly XmlDocument EnvelopeXml;
        protected readonly EnvelopeSettings Settings; 
        protected AbstractHeader Header;
        
        private bool _isXmlGenerated = false;
        private byte[] _bytes;
        private string _contentId;

        protected AbstractEnvelope(EnvelopeSettings settings)
        {
            Settings = settings;
            EnvelopeXml = LagXmlRotnode();
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

        private XmlDocument LagXmlRotnode()
        {
            var xmlDokument = new XmlDocument();
            xmlDokument.PreserveWhitespace = true;
            var xmlDeclaration = xmlDokument.CreateXmlDeclaration("1.0", "UTF-8", null);
            var baseNode = xmlDokument.CreateElement("env", "Envelope", Navnerom.env);
            xmlDokument.AppendChild(baseNode);
            xmlDokument.InsertBefore(xmlDeclaration, xmlDokument.DocumentElement);
            return xmlDokument;
        }

        public XmlDocument Xml()
        {
            if (_isXmlGenerated) return EnvelopeXml;

            EnvelopeXml.DocumentElement.AppendChild(HeaderElement());
            EnvelopeXml.DocumentElement.AppendChild(BodyElement());
            Header.AddSignatureElement();
            _isXmlGenerated = true;

            return EnvelopeXml;
        }

        public void SkrivTilFil(string filsti)
        {
            if (!_isXmlGenerated)
                Xml();

            EnvelopeXml.Save(filsti);
        }

        protected abstract XmlNode HeaderElement();
        protected abstract XmlNode BodyElement();
    }




}
