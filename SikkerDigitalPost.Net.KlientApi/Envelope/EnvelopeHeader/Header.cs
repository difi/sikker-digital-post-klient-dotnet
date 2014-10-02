using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Header : XmlPart
    {
        public Header(XmlDocument dokument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler) : base(dokument, forsendelse, asicEArkiv, databehandler)
        {
        }

        public override XmlElement Xml()
        {
            var header = XmlEnvelope.CreateElement("env","Header",Navnerom.XmlnsEnv);
            header.AppendChild(SecurityElement());
            header.AppendChild(MessagingElement());
            return header;
        }

        public XmlElement SecurityElement()
        {
            var securityElement = new Security(XmlEnvelope,Forsendelse, AsicEArkiv, Databehandler);
            return securityElement.Xml();
        }

        public XmlElement MessagingElement()
        {
            var messaging = new Messaging(XmlEnvelope, Forsendelse, AsicEArkiv, Databehandler);
            return messaging.Xml();
        }
    }
}
