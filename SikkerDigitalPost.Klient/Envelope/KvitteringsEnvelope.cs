using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Envelope.Body.Kvittering;
using SikkerDigitalPost.Klient.Envelope.Header.Kvittering;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class KvitteringsEnvelope : AbstractEnvelope
    {

        public KvitteringsEnvelope(EnvelopeSettings settings) : base(settings)
        {
        }

        protected override XmlNode HeaderElement()
        {
            Header = new KvitteringsHeader(Settings, EnvelopeXml);
            return Header.Xml();
        }


        protected override XmlNode BodyElement()
        {
            var body = new KvitteringsBody(Settings, EnvelopeXml);
            return body.Xml();
        }
    }
}
