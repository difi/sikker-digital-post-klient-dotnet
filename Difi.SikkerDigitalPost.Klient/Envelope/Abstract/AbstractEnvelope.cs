using System;
using System.Text;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class AbstractEnvelope : ISoapVedlegg
    {
        protected readonly XmlDocument EnvelopeXml;
        private byte[] _bytes;
        private string _contentId;

        private bool _isXmlGenerated;
        protected AbstractHeader Header;

        protected AbstractEnvelope(EnvelopeSettings envelopeSettings)
        {
            EnvelopeSettings = envelopeSettings;
            EnvelopeXml = LagXmlRotnode();
        }

        public EnvelopeSettings EnvelopeSettings { get; }

        public string Filnavn => "envelope.xml";

        public byte[] Bytes => _bytes ?? (_bytes = Encoding.UTF8.GetBytes(Xml().OuterXml));

        public string Innholdstype => "application/soap+xml; charset=UTF-8";

        public string ContentId => _contentId ?? (_contentId = $"{Guid.NewGuid()}@meldingsformidler.sdp.difi.no");

        public string TransferEncoding => "binary";

        private XmlDocument LagXmlRotnode()
        {
            var xmlDokument = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = xmlDokument.CreateXmlDeclaration("1.0", "UTF-8", null);
            var baseNode = xmlDokument.CreateElement("env", "Envelope", NavneromUtility.SoapEnvelopeEnv12);
            xmlDokument.AppendChild(baseNode);
            xmlDokument.InsertBefore(xmlDeclaration, xmlDokument.DocumentElement);
            return xmlDokument;
        }

        public XmlDocument Xml()
        {
            if (_isXmlGenerated) return EnvelopeXml;

            try
            {
                EnvelopeXml.DocumentElement.AppendChild(HeaderElement());
                EnvelopeXml.DocumentElement.AppendChild(BodyElement());
                Header.AddSignatureElement();
                _isXmlGenerated = true;
            }
            catch (Exception e)
            {
                throw new XmlParseException($"Kunne ikke bygge Xml for {GetType()} (av type AbstractEnvelope).", e);
            }

            return EnvelopeXml;
        }

        protected abstract XmlNode HeaderElement();

        protected abstract XmlNode BodyElement();
    }
}