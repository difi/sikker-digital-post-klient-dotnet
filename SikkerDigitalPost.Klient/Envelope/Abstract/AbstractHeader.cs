using System.Xml;
using SikkerDigitalPost.Domene;

namespace SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class AbstractHeader : XmlPart
    {
        protected XmlNode Security;

        protected AbstractHeader(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var header = Context.CreateElement("env", "Header", Navnerom.env);
            header.AppendChild(SecurityElement());
            header.AppendChild(MessagingElement());
            return header;
        }

        protected abstract XmlNode SecurityElement();

        protected abstract XmlNode MessagingElement();

        public abstract void AddSignatureElement();
    }
}
