using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Messaging : XmlPart
    {
        public Messaging(XmlDocument xmlDocument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler) : base(xmlDocument, forsendelse, asicEArkiv, databehandler)
        {
        }

        public override XmlElement Xml()
        {
            //XmlElement messaging = XmlDocument.CreateElement("eb", "Messaging", Navnerom.eb)

            return null;
        }

        public XmlElement UserMessage()
        {
            return null;
        }

        public XmlElement MessageInfo()
        {
            return null;
        }

        public XmlElement PartyInfo()
        {
            //From 


            //To
            return null;
        }

        public XmlElement CollaborationInfo()
        {
            //AgreementRef

            //Service

            //Action

            //ConversationId
            return null;
        }

        public XmlElement PayloadInfo()
        {
            return null;
        }
    }
}
