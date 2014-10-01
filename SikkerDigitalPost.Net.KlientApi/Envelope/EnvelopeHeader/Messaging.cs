using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Messaging : XmlPart
    {
        public Messaging(XmlDocument xmlDocument, Forsendelse forsendelse, Arkiv arkiv, Databehandler databehandler) : base(xmlDocument, forsendelse, arkiv, databehandler)
        {
        }

        public override XmlElement Xml()
        {
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
