using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel
{
    internal class KvitteringsforespørselEnvelope : AbstractEnvelope
    {
        public KvitteringsforespørselEnvelope(EnvelopeSettings envelopeSettings)
            : base(envelopeSettings)
        {
        }

        protected override XmlNode HeaderElement()
        {
            Header = new KvitteringsforespørselHeader(EnvelopeSettings, EnvelopeXml);
            return Header.Xml();
        }

        protected override XmlNode BodyElement()
        {
            var body = new UtenInnholdBody(EnvelopeSettings, EnvelopeXml);
            return body.Xml();
        }
    }
}