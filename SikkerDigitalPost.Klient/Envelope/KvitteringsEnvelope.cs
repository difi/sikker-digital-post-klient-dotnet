using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope
{
    class KvitteringsEnvelope : AbstractEnvelope, ISoapVedlegg
    {
        public KvitteringsEnvelope(EnvelopeSettings settings) : base(settings)
        {
        }

        public override XmlDocument Xml()
        {
            if (IsXmlGenerated) return EnvelopeXml;
            
            //EnvelopeXml.DocumentElement.AppendChild(HeaderElement());
            //EnvelopeXml.DocumentElement.AppendChild(BodyElement());
            Header.AddSignatureElement();
            IsXmlGenerated = true;

            return EnvelopeXml;
        }
        
        public string Filnavn { get; private set; }
        public byte[] Bytes { get; private set; }
        public string Innholdstype { get; private set; }
        public string ContentId { get; private set; }
        public string TransferEncoding { get; private set; }
    }
}
