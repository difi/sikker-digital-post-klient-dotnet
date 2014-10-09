using System;
using System.Xml;
using SikkerDigitalPost.Domene.Extensions;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient.Envelope.Header.Forretningsmelding
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
            XmlAttribute mustUnderstand = Context.CreateAttribute("env", "mustUnderstand", Navnerom.env);
            mustUnderstand.InnerText = "true";
            messaging.Attributes.Append(mustUnderstand);

            messaging.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.EbMessagingId);

            messaging.AppendChild(UserMessageElement());
            
            return messaging;
        }

        private XmlElement UserMessageElement()
        {
            XmlElement userMessage = Context.CreateElement("eb", "UserMessage", Navnerom.eb);
            userMessage.SetAttribute("mpc", Settings.Forsendelse.Mpc);

            userMessage.AppendChild(MessageInfoElement());
            userMessage.AppendChild(PartyInfoElement());
            userMessage.AppendChild(CollaborationInfoElement());
            userMessage.AppendChild(PayloadInfoElement());

            return userMessage;
        }

        private XmlElement MessageInfoElement()
        {
            XmlElement messageInfo = Context.CreateElement("eb", "MessageInfo", Navnerom.eb);
            {
                XmlElement timestamp =  messageInfo.AppendChildElement("Timestamp", "eb", Navnerom.eb, Context);
                timestamp.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                // http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/UserMessage/MessageInfo
                // Unik identifikator, satt av MSH. Kan med fordel benytte SBDH.InstanceIdentifier 
                XmlElement messageId = messageInfo.AppendChildElement("MessageId", "eb", Navnerom.eb, Context);
                messageId.InnerText = Settings.GuidHandler.StandardBusinessDocumentHeaderId;
            }
            return messageInfo;
        }

        private XmlElement PartyInfoElement()
        {
            XmlElement partyInfo = Context.CreateElement("eb", "PartyInfo", Navnerom.eb);
            {
                XmlElement from = partyInfo.AppendChildElement("From", "eb", Navnerom.eb, Context);
                {
                    XmlElement partyId = from.AppendChildElement("PartyId", "eb", Navnerom.eb, Context);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = Settings.Databehandler.Organisasjonsnummer.Iso6523();

                    XmlElement role = from.AppendChildElement("Role", "eb", Navnerom.eb, Context);
                    role.InnerText = "urn:sdp:avsender";
                }

                XmlElement to = partyInfo.AppendChildElement("To", "eb", Navnerom.eb, Context);
                {
                    XmlElement partyId = to.AppendChildElement("PartyId", "eb", Navnerom.eb, Context);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = "9908:984661185";

                    XmlElement role = to.AppendChildElement("Role", "eb", Navnerom.eb, Context);
                    role.InnerText = "urn:sdp:meldingsformidler";
                }
            }
            return partyInfo;
        }

        private XmlElement CollaborationInfoElement()
        {
            XmlElement collaborationInfo = Context.CreateElement("eb", "CollaborationInfo", Navnerom.eb);
            {
                XmlElement agreementRef = collaborationInfo.AppendChildElement("AgreementRef","eb",Navnerom.eb,Context);
                agreementRef.InnerText = "http://begrep.difi.no/SikkerDigitalPost/Meldingsutveksling/FormidleDigitalPostForsendelse";

                XmlElement service = collaborationInfo.AppendChildElement("Service", "eb", Navnerom.eb, Context);
                service.InnerText = "SDP";

                XmlElement action = collaborationInfo.AppendChildElement("Action", "eb", Navnerom.eb, Context);
                action.InnerText = "FormidleDigitalPost";

                XmlElement conversationId = collaborationInfo.AppendChildElement("ConversationId", "eb", Navnerom.eb, Context);
                conversationId.InnerText = Settings.Forsendelse.KonversasjonsId;
            }
            return collaborationInfo;
        }

        private XmlElement PayloadInfoElement()
        {
            //Mer info på http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/UserMessage/PayloadInfo

            XmlElement payloadInfo = Context.CreateElement("eb", "PayloadInfo", Navnerom.eb);
            {
                XmlElement partInfoBody = payloadInfo.AppendChildElement("PartInfo", "eb", Navnerom.eb, Context);
                partInfoBody.SetAttribute("href", "#"+Settings.GuidHandler.BodyId);

                XmlElement partInfoDokumentpakke = payloadInfo.AppendChildElement("PartInfo", "eb", Navnerom.eb, Context);
                partInfoDokumentpakke.SetAttribute("href", "cid:"+Settings.GuidHandler.DokumentpakkeId);
                {
                    XmlElement partProperties = partInfoDokumentpakke.AppendChildElement("PartProperties", "eb", Navnerom.eb, Context);
                    {
                        XmlElement propertyMimeType = partProperties.AppendChildElement("Property", "eb", Navnerom.eb, Context);
                        propertyMimeType.SetAttribute("name", "MimeType");
                        propertyMimeType.InnerText = "application/cms";

                        XmlElement propertyContent = partProperties.AppendChildElement("Property", "eb", Navnerom.eb, Context);
                        propertyContent.SetAttribute("name", "Content");
                        propertyContent.InnerText = "sdp:Dokumentpakke";
                    }
                }
            }
            return payloadInfo;
        }
    }
}