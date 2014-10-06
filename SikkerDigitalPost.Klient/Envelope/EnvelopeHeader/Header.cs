using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeHeader
{
    internal class Header : XmlPart
    {
        private Security _security;

        public Header(Envelope rot) : base(rot)
        {
        }

        public override XmlElement Xml()
        {
            var header = Rot.EnvelopeXml.CreateElement("env", "Header", Navnerom.env);
            header.AppendChild(SecurityElement());
            header.AppendChild(MessagingElement());
            return header;
        }

        private XmlElement SecurityElement()
        {
            _security = new Security(Rot);
            return _security.Xml();
        }

        private XmlElement MessagingElement()
        {
            var messaging = new Messaging(Rot);
            return messaging.Xml();
        }

        public void AddSignatureElement()
        {
            _security.AddSignatureElement();
        }
    }
}
