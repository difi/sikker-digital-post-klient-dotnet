using System;
using System.Xml;
using SikkerDigitalPost.Klient.Utilities;
using SikkerDigitalPost.Domene.Extensions;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeHeader
{
    internal class Messaging : XmlPart
    {
        public Messaging(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            XmlElement messaging = Context.CreateElement("eb", "Messaging", Navnerom.eb);
            messaging.SetAttribute("xmlns:wsu", Navnerom.wsu);
            messaging.SetAttribute("mustUnderstand", Navnerom.env, "true");
            messaging.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.EbMessagingId);
            messaging.AppendChild(UserMessageElement());

            return messaging;
        }

        public XmlElement UserMessageElement()
        {

            XmlElement userMessage = Context.CreateElement("ns6", "UserMessage", Navnerom.Ns6);
            userMessage.SetAttribute("mpc", Settings.Forsendelse.MpcId);

            userMessage.AppendChild(MessageInfoElement());
            userMessage.AppendChild(PartyInfoElement());
            userMessage.AppendChild(CollaborationInfoElement());
            userMessage.AppendChild(PayloadInfoElement());

            return userMessage;
        }

        public XmlElement MessageInfoElement()
        {
            XmlElement messageInfo = Context.CreateElement("ns6", "MessageInfo", Navnerom.Ns6);

            {
                XmlElement timestamp =  messageInfo.AppendChildElement("Timestamp", "ns6", Navnerom.Ns6, Context);
                timestamp.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                // http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/UserMessage/MessageInfo
                // Unik identifikator, satt av MSH. Kan med fordel benytte SBDH.InstanceIdentifier 
                XmlElement messageId = messageInfo.AppendChildElement("MessageId", "ns6", Navnerom.Ns6, Context);
                messageId.InnerText = Settings.GuidHandler.StandardBusinessDocumentHeaderId;
            }
            return messageInfo;
        }

        public XmlElement PartyInfoElement()
        {
            XmlElement partyInfo = Context.CreateElement("ns6", "PartyInfo", Navnerom.Ns6);

            {
                XmlElement from = partyInfo.AppendChildElement("From", "ns6", Navnerom.Ns6, Context);
                {
                    XmlElement partyId = from.AppendChildElement("PartyId", "ns6", Navnerom.Ns6, Context);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = Settings.Databehandler.Organisasjonsnummer.Iso6523();

                    XmlElement role = from.AppendChildElement("Role", "ns6", Navnerom.Ns6, Context);
                    role.InnerText = "urn:sdp:avsender";
                }

                XmlElement to = partyInfo.AppendChildElement("To", "ns6", Navnerom.Ns6, Context);
                {
                    XmlElement partyId = to.AppendChildElement("PartyId", "ns6", Navnerom.Ns6, Context);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = "9908:984661185";

                    XmlElement role = to.AppendChildElement("Role", "ns6", Navnerom.Ns6, Context);
                    role.InnerText = "urn:sdp:meldingsformidler";
                }

            }
            return partyInfo;
        }

        public XmlElement CollaborationInfoElement()
        {
            XmlElement collaborationInfo = Context.CreateElement("ns6", "CollaborationInfo", Navnerom.Ns6);

            {
                XmlElement agreementRef = collaborationInfo.AppendChildElement("AgreementRef","ns6",Navnerom.Ns6,Context);
                agreementRef.InnerText = "http://begrep.difi.no/SikkerDigitalPost/Meldingsutveksling/FormidleDigitalPostForsendelse";

                XmlElement service = collaborationInfo.AppendChildElement("Service", "ns6", Navnerom.Ns6, Context);
                service.InnerText = "SDP";

                XmlElement action = collaborationInfo.AppendChildElement("Action", "ns6", Navnerom.Ns6, Context);
                action.InnerText = "FormidleDigitalPost";

                XmlElement conversationId = collaborationInfo.AppendChildElement("ConversationId", "ns6", Navnerom.Ns6, Context);
                conversationId.InnerText = Settings.Forsendelse.KonversasjonsId;

            }

            return collaborationInfo;
        }

        public XmlElement PayloadInfoElement()
        {
            //Mer info på http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/UserMessage/PayloadInfo

            XmlElement payloadInfo = Context.CreateElement("ns6", "PayloadInfo", Navnerom.Ns6);
            {
                XmlElement partInfoBody = payloadInfo.AppendChildElement("PartInfo", "ns6", Navnerom.Ns6, Context);
                partInfoBody.SetAttribute("href", Settings.GuidHandler.BodyId);

                XmlElement partInfoDokumentpakke = payloadInfo.AppendChildElement("PartInfo", "ns6", Navnerom.Ns6, Context);
                partInfoDokumentpakke.SetAttribute("href", Settings.GuidHandler.DokumentpakkeId);
                {
                    XmlElement partProperties = partInfoDokumentpakke.AppendChildElement("PartProperties", "ns6", Navnerom.Ns6, Context);
                    {
                        XmlElement propertyMimeType = partProperties.AppendChildElement("Property", "ns6", Navnerom.Ns6, Context);
                        propertyMimeType.SetAttribute("name", "MimeType");
                        propertyMimeType.InnerText = "application/cms";

                        XmlElement propertyContent = partProperties.AppendChildElement("Property", "ns6", Navnerom.Ns6, Context);
                        propertyContent.SetAttribute("name", "Content");
                        propertyContent.InnerText = "sdp:Dokumentpakke";
                    }
                }
            }
            return payloadInfo;
        }
    }
}