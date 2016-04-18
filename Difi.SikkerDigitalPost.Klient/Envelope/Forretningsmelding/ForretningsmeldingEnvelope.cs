using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class ForretningsmeldingEnvelope : AbstractEnvelope
    {
        public ForretningsmeldingEnvelope(EnvelopeSettings envelopeSettings)
            : base(envelopeSettings)
        {
        }

        protected override XmlNode HeaderElement()
        {
            Header = new ForretningsmeldingHeader(EnvelopeSettings, EnvelopeXml);
            return Header.Xml();
        }

        protected override XmlNode BodyElement()
        {
            var body = new ForretningsmeldingBody(EnvelopeSettings, EnvelopeXml);
            return body.Xml();
        }
    }
}