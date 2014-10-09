using System.Xml;

namespace SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class AbstractHeader : XmlPart
    {
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
    }
}
