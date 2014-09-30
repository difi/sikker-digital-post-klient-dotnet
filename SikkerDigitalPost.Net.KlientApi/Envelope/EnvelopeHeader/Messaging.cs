using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Messaging : XmlPart
    {
        public Messaging(XmlDocument xmlDocument) : base(xmlDocument)
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
