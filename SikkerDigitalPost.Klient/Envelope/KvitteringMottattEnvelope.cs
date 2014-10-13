using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Envelope.Body.Kvittering;
using SikkerDigitalPost.Klient.Envelope.Header.KvitteringMottatt;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class KvitteringMottattEnvelope : AbstractEnvelope
    {
        public KvitteringMottattEnvelope(EnvelopeSettings settings) : base(settings)
        {
        }

        protected override XmlNode HeaderElement()
        {
           Header = new KvitteringMottattHeader(Settings, EnvelopeXml);
           return Header.Xml();
        }

        protected override XmlNode BodyElement()
        {
            var body = new UtenInnholdBody(Settings, EnvelopeXml);
            return body.Xml();
        }
    }
}
