using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Envelope.Body;
using SikkerDigitalPost.Klient.Envelope.Body.Forretningsmelding;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class ForretingsmeldingEnvelope : AbstractEnvelope
    {
        
        public ForretingsmeldingEnvelope(EnvelopeSettings settings) : base (settings)
        {
        }
        
        public override XmlDocument Xml()
        {
            if (IsXmlGenerated) return EnvelopeXml;

            EnvelopeXml.DocumentElement.AppendChild(HeaderElement());
            EnvelopeXml.DocumentElement.AppendChild(BodyElement());
            Header.AddSignatureElement();
            IsXmlGenerated = true;

            return EnvelopeXml;
        }

        protected override XmlNode HeaderElement()
        {
            Header = new Header.Forretningsmelding.Header(Settings, EnvelopeXml);
            return Header.Xml();
        }

        protected override XmlNode BodyElement()
        {
            var body = new ForretningsmeldingBody(Settings, EnvelopeXml);
            return body.Xml();
        }
       
    }
}
