using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel
{
    internal class KvitteringsforespørselEnvelope : AbstractEnvelope
    {
        public KvitteringsforespørselEnvelope(EnvelopeSettings settings) : base(settings)
        {
        }

        protected override XmlNode HeaderElement()
        {
            Header = new KvitteringsforespørselHeader(Settings, EnvelopeXml);
            return Header.Xml();
        }

        protected override XmlNode BodyElement()
        {
            var body = new UtenInnholdBody(Settings, EnvelopeXml);
            return body.Xml();
        }
    }
}
