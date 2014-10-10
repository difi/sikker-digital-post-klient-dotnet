using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Envelope.Body.Forretningsmelding;
using SikkerDigitalPost.Klient.Envelope.Header.Forretningsmelding;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class ForretingsmeldingEnvelope : AbstractEnvelope
    {
        
        public ForretingsmeldingEnvelope(EnvelopeSettings settings) : base (settings)
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
