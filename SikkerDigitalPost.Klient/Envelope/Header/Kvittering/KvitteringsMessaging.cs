using System;
using System.Xml;
using SikkerDigitalPost.Domene.Extensions;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient.Envelope.Header.Kvittering
{
    internal class KvitteringsMessaging : XmlPart
    {
        public KvitteringsMessaging(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            XmlElement messaging = Context.CreateElement("eb", "Messaging", Navnerom.eb);
            messaging.AppendChild(SignalMessageElement());
            return messaging;
        }

        private XmlElement SignalMessageElement()
        {
            XmlElement signalMessage = Context.CreateElement("eb", "SignalMessage", Navnerom.eb);
            return signalMessage;
        }

        private XmlElement MessageInfoElement()
        {
            XmlElement messageInfo = Context.CreateElement("eb", "MessageInfo", Navnerom.eb);
            {
                XmlElement timestamp = messageInfo.AppendChildElement("Timestamp", "eb", Navnerom.eb, Context);
                timestamp.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                XmlElement messageId = messageInfo.AppendChildElement("MessageId", "eb", Navnerom.eb, Context);
                messageId.InnerText = Settings.GuidHandler.StandardBusinessDocumentHeaderId;

                var mpc = Settings.Forsendelse.MpcId == String.Empty
                ? String.Format("urn:{0}", Settings.Forsendelse.Prioritet.ToString().ToLower())
                : String.Format("urn:{0}:{1}", Settings.Forsendelse.Prioritet.ToString().ToLower(), Settings.Forsendelse.MpcId);
                XmlElement pullRequest = messageInfo.AppendChildElement("PullRequest", "eb", Navnerom.eb, Context);
                pullRequest.SetAttribute("mpc", mpc);
            }
            return messageInfo;
        }

    }
}
