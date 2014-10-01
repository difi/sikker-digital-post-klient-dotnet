using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Header : XmlPart
    {
        public Header(XmlDocument dokument, Forsendelse forsendelse) : base(dokument, forsendelse)
        {
        }

        public override XmlElement Xml()
        {
            var header = XmlDocument.CreateElement("env","Header",Navnerom.XmlnsEnv);
            header.AppendChild(SecurityElement());
            //////header.AppendChild(MessagingElement());
            return header;
        }

        public XmlElement SecurityElement()
        {
            var securityElement = new Security(XmlDocument,Forsendelse);
            return securityElement.Xml();
        }

        public XmlElement MessagingElement()
        {
            var messaging = new Messaging(XmlDocument, Forsendelse);
            return messaging.Xml();
        }
    }
}
