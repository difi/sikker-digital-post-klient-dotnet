using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class ForretningsmeldingEnvelope : AbstractEnvelope
    {
        public ForretningsmeldingEnvelope(EnvelopeSettings settings)
            : base(settings)
        {
        }

        protected override XmlNode HeaderElement()
        {
            Header = new ForretningsmeldingHeader(Settings, EnvelopeXml);
            return Header.Xml();
        }

        protected override XmlNode BodyElement()
        {
            var body = new ForretningsmeldingBody(Settings, EnvelopeXml);
            return body.Xml();
        }
    }
}