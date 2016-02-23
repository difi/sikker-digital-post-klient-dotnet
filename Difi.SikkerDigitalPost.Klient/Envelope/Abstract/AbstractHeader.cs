using System.Xml;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class AbstractHeader : EnvelopeXmlPart
    {
        protected XmlNode Security;

        protected AbstractHeader(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var header = Context.CreateElement("env", "Header", NavneromUtility.SoapEnvelopeEnv12);
            header.AppendChild(SecurityElement());
            header.AppendChild(MessagingElement());
            return header;
        }

        protected abstract XmlNode SecurityElement();

        protected abstract XmlNode MessagingElement();

        public abstract void AddSignatureElement();
    }
}