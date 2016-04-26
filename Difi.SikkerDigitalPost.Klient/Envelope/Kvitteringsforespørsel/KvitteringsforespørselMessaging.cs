using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel
{
    internal class KvitteringsforespørselMessaging : EnvelopeXmlPart
    {
        public KvitteringsforespørselMessaging(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var messaging = Context.CreateElement("eb", "Messaging", NavneromUtility.EbXmlCore);
            messaging.SetAttribute("xmlns:wsu", NavneromUtility.WssecurityUtility10);
            var mustUnderstand = Context.CreateAttribute("env", "mustUnderstand", NavneromUtility.SoapEnvelopeEnv12);
            mustUnderstand.InnerText = "true";
            messaging.Attributes.Append(mustUnderstand);

            messaging.SetAttribute("Id", NavneromUtility.WssecurityUtility10, Settings.GuidUtility.EbMessagingId);

            messaging.AppendChild(SignalMessageElement());

            return messaging;
        }

        private XmlElement SignalMessageElement()
        {
            var signalMessage = Context.CreateElement("eb", "SignalMessage", NavneromUtility.EbXmlCore);

            signalMessage.AppendChild(MessageInfoElement());
            signalMessage.AppendChild(PullRequestElement());

            return signalMessage;
        }

        private XmlElement MessageInfoElement()
        {
            var messageInfo = Context.CreateElement("eb", "MessageInfo", NavneromUtility.EbXmlCore);
            {
                var timestamp = messageInfo.AppendChildElement("Timestamp", "eb", NavneromUtility.EbXmlCore, Context);
                timestamp.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                var messageId = messageInfo.AppendChildElement("MessageId", "eb", NavneromUtility.EbXmlCore, Context);
                messageId.InnerText = Settings.GuidUtility.MessageId;
            }
            return messageInfo;
        }

        private XmlElement PullRequestElement()
        {
            var pullRequest = Context.CreateElement("eb", "PullRequest", NavneromUtility.EbXmlCore);
            pullRequest.SetAttribute("mpc", Settings.Kvitteringsforespørsel.Mpc);
            return pullRequest;
        }
    }
}