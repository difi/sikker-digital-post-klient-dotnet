using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse
{
    internal class KvitteringsbekreftelseEnvelope : AbstractEnvelope
    {
        public KvitteringsbekreftelseEnvelope(EnvelopeSettings envelopeSettings)
            : base(envelopeSettings)
        {
        }

        protected override XmlNode HeaderElement()
        {
            Header = new KvitteringsbekreftelseHeader(EnvelopeSettings, EnvelopeXml);
            return Header.Xml();
        }

        protected override XmlNode BodyElement()
        {
            var body = new UtenInnholdBody(EnvelopeSettings, EnvelopeXml);
            return body.Xml();
        }
    }
}