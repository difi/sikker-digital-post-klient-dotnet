using System;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Envelope.EnvelopeBody;
using SikkerDigitalPost.Klient.Envelope.EnvelopeHeader;

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

        private XmlNode HeaderElement()
        {
            Header = new Header(Settings, EnvelopeXml);
            return Header.Xml();
        }

        private XmlNode BodyElement()
        {
            var body = new Body(Settings, EnvelopeXml);
            return body.Xml();
        }
       
    }
}
