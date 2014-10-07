using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeHeader
{
    internal class Header : XmlPart
    {
        private Security _security;

        public Header(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var header = Context.CreateElement("env", "Header", Navnerom.env);
            header.AppendChild(SecurityElement());
            header.AppendChild(MessagingElement());
            return header;
        }

        private XmlNode SecurityElement()
        {
            _security = new Security(Settings, Context);
            return _security.Xml();
        }

        private XmlNode MessagingElement()
        {
            var messaging = new Messaging(Settings, Context);
            return messaging.Xml();
        }

        public void AddSignatureElement()
        {
            _security.AddSignatureElement();
        }
    }
}
