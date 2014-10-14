using System;
using System.Xml;
using SikkerDigitalPost.Domene.Extensions;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient.Envelope.Header.KvitteringMottatt
{
    internal class KvitteringMottattMessaging : XmlPart
    {
        public KvitteringMottattMessaging(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            XmlElement messaging = Context.CreateElement("eb", "Messaging", Navnerom.eb);
            messaging.SetAttribute("xmlns:wsu", Navnerom.wsu);
            XmlAttribute mustUnderstand = Context.CreateAttribute("env", "mustUnderstand", Navnerom.env);
            mustUnderstand.InnerText = "true";
            messaging.Attributes.Append(mustUnderstand);

            messaging.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.EbMessagingId);

            messaging.AppendChild(SignalMessageElement());

            return messaging;
        }

        private XmlElement SignalMessageElement()
        {
            XmlElement signalMessage = Context.CreateElement("eb", "SignalMessage", Navnerom.eb);

            signalMessage.AppendChild(MessageInfoElement());
            signalMessage.AppendChild(ReceiptElement());

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

                XmlElement refToMessageId = messageInfo.AppendChildElement("RefToMessageId", "eb", Navnerom.eb, Context);
                refToMessageId.InnerText = Settings.ForrigeKvittering.MessageId;
            }
            return messageInfo;
        }

        private XmlElement ReceiptElement()
        {
            XmlElement receipt = Context.CreateElement("eb", "Receipt", Navnerom.eb);
            {
                XmlElement nonRepudiationInformation = receipt.AppendChildElement("NonRepudiationInformation", "ns7", Navnerom.Ns7, Context);
                {
                    XmlElement messagePartNRInformation = nonRepudiationInformation.AppendChildElement("MessagePartNRInformation", "ns7", Navnerom.Ns7, Context);
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
