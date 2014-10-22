using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse
{
    internal class KvitteringsbekreftelseEnvelope : AbstractEnvelope
    {
        public KvitteringsbekreftelseEnvelope(EnvelopeSettings settings) : base(settings)
        {
        }

        protected override XmlNode HeaderElement()
        {
           Header = new KvitteringsbekreftelseHeader(Settings, EnvelopeXml);
           return Header.Xml();
        }

        protected override XmlNode BodyElement()
        {
            var body = new UtenInnholdBody(Settings, EnvelopeXml);
            return body.Xml();
        }
    }
}
