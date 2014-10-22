using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class ForretningsmeldingEnvelope : AbstractEnvelope
    {
        
        public ForretningsmeldingEnvelope(EnvelopeSettings settings) : base (settings)
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
