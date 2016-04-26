using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse
{
    internal class KvitteringsbekreftelseMessaging : EnvelopeXmlPart
    {
        public KvitteringsbekreftelseMessaging(EnvelopeSettings settings, XmlDocument context)
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
            signalMessage.AppendChild(ReceiptElement());

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

                var refToMessageId = messageInfo.AppendChildElement("RefToMessageId", "eb", NavneromUtility.EbXmlCore, Context);
                refToMessageId.InnerText = Settings.ForrigeKvittering.MeldingsId;
            }
            return messageInfo;
        }

        private XmlElement ReceiptElement()
        {
            var receipt = Context.CreateElement("eb", "Receipt", NavneromUtility.EbXmlCore);
            {
                var nonRepudiationInformation = receipt.AppendChildElement("NonRepudiationInformation", "ns7", NavneromUtility.EbppSignals, Context);
                {
                    var messagePartNrInformation = nonRepudiationInformation.AppendChildElement("MessagePartNRInformation", "ns7", NavneromUtility.EbppSignals, Context);
                    {
                        var reference = messagePartNrInformation.AppendChildElement("Reference", "ds", NavneromUtility.XmlDsig, Context);
                        reference.SetAttribute("URI", Settings.ForrigeKvittering.BodyReferenceUri);
                        {
                            var transforms = reference.AppendChildElement("Transforms", "ds", NavneromUtility.XmlDsig, Context);
                            {
                                var transform = transforms.AppendChildElement("Transform", "ds", NavneromUtility.XmlDsig, Context);
                                transform.SetAttribute("Algorithm", "http://www.w3.org/2001/10/xml-exc-c14n#");
                                {
                                    var inclusiveNamespaces = transform.AppendChildElement("InclusiveNamespaces", "ec", NavneromUtility.XmlExcC14N, Context);
                                    inclusiveNamespaces.SetAttribute("PrefixList", string.Empty);
                                }
                            }

                            var digestMethod = reference.AppendChildElement("DigestMethod", "ds", NavneromUtility.XmlDsig, Context);
                            digestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");

                            var digestValue = reference.AppendChildElement("DigestValue", "ds", NavneromUtility.XmlDsig, Context);
                            digestValue.InnerText = Settings.ForrigeKvittering.DigestValue;
                        }
                    }
                }
            }
            return receipt;
        }
    }
}