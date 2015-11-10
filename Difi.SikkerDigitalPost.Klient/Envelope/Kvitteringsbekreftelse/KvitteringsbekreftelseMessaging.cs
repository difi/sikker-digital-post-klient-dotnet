using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse
{
    internal class KvitteringsbekreftelseMessaging : EnvelopeXmlPart
    {
        public KvitteringsbekreftelseMessaging(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            XmlElement messaging = Context.CreateElement("eb", "Messaging", NavneromUtility.EbXmlCore);
            messaging.SetAttribute("xmlns:wsu", NavneromUtility.WssecurityUtility10);
            XmlAttribute mustUnderstand = Context.CreateAttribute("env", "mustUnderstand", NavneromUtility.SoapEnvelopeEnv12);
            mustUnderstand.InnerText = "true";
            messaging.Attributes.Append(mustUnderstand);

            messaging.SetAttribute("Id", NavneromUtility.WssecurityUtility10, Settings.GuidHandler.EbMessagingId);

            messaging.AppendChild(SignalMessageElement());

            return messaging;
        }

        private XmlElement SignalMessageElement()
        {
            XmlElement signalMessage = Context.CreateElement("eb", "SignalMessage", NavneromUtility.EbXmlCore);

            signalMessage.AppendChild(MessageInfoElement());
            signalMessage.AppendChild(ReceiptElement());

            return signalMessage;
        }

        private XmlElement MessageInfoElement()
        {
            XmlElement messageInfo = Context.CreateElement("eb", "MessageInfo", NavneromUtility.EbXmlCore);
            {
                XmlElement timestamp = messageInfo.AppendChildElement("Timestamp", "eb", NavneromUtility.EbXmlCore, Context);
                timestamp.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                XmlElement messageId = messageInfo.AppendChildElement("MessageId", "eb", NavneromUtility.EbXmlCore, Context);
                messageId.InnerText = Settings.GuidHandler.StandardBusinessDocumentHeaderId;

                XmlElement refToMessageId = messageInfo.AppendChildElement("RefToMessageId", "eb", NavneromUtility.EbXmlCore, Context);
                refToMessageId.InnerText = Settings.ForrigeKvittering.MeldingsId;
            }
            return messageInfo;
        }

        private XmlElement ReceiptElement()
        {
            XmlElement receipt = Context.CreateElement("eb", "Receipt", NavneromUtility.EbXmlCore);
            {
                XmlElement nonRepudiationInformation = receipt.AppendChildElement("NonRepudiationInformation", "ns7", NavneromUtility.EbppSignals, Context);
                {
                    XmlElement messagePartNRInformation = nonRepudiationInformation.AppendChildElement("MessagePartNRInformation", "ns7", NavneromUtility.EbppSignals, Context);
                    {
                        XmlNode reference = Settings.ForrigeKvittering.BodyReference;                        
                        messagePartNRInformation.AppendChild(Context.ImportNode(reference, true));
                    }
                }
            }
            return receipt;
        }
    }
}
