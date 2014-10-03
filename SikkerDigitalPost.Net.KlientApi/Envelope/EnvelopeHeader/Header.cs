using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Net.Domene.Entiteter.Post;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Header : XmlPart
    {
        private Security _security;

        public Header(XmlDocument dokument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler) :
            base(dokument, forsendelse, asicEArkiv, databehandler)
        {
        }

        public override XmlElement Xml()
        {
            var header = XmlEnvelope.CreateElement("env","Header",Navnerom.env);
            header.AppendChild(SecurityElement());
            header.AppendChild(MessagingElement());
            return header;
        }

        private XmlElement SecurityElement()
        {
            _security = new Security(XmlEnvelope,Forsendelse, AsicEArkiv, Databehandler);
            return _security.Xml();
        }

        private XmlElement MessagingElement()
        {
            var messaging = new Messaging(XmlEnvelope, Forsendelse, AsicEArkiv, Databehandler);
            return messaging.Xml();
        }

        public void AddSignatureElement()
        {
            _security.AddSignatureElement();
        }
    }
}
