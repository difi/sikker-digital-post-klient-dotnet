using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class ForretningsmeldingMessaging : EnvelopeXmlPart
    {
        public ForretningsmeldingMessaging(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
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

            messaging.AppendChild(UserMessageElement());
            
            return messaging;
        }

        private XmlElement UserMessageElement()
        {
            XmlElement userMessage = Context.CreateElement("eb", "UserMessage", NavneromUtility.EbXmlCore);
            userMessage.SetAttribute("mpc", Settings.Forsendelse.Mpc);

            userMessage.AppendChild(MessageInfoElement());
            userMessage.AppendChild(PartyInfoElement());
            userMessage.AppendChild(CollaborationInfoElement());
            userMessage.AppendChild(PayloadInfoElement());

            return userMessage;
        }

        private XmlElement MessageInfoElement()
        {
            XmlElement messageInfo = Context.CreateElement("eb", "MessageInfo", NavneromUtility.EbXmlCore);
            {
                XmlElement timestamp =  messageInfo.AppendChildElement("Timestamp", "eb", NavneromUtility.EbXmlCore, Context);
                timestamp.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                // http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/UserMessage/MessageInfo
                // Unik identifikator, satt av MSH. Kan med fordel benytte SBDH.InstanceIdentifier 
                XmlElement messageId = messageInfo.AppendChildElement("MessageId", "eb", NavneromUtility.EbXmlCore, Context);
                messageId.InnerText = Settings.GuidHandler.StandardBusinessDocumentHeaderId;
            }
            return messageInfo;
        }

        private XmlElement PartyInfoElement()
        {
            XmlElement partyInfo = Context.CreateElement("eb", "PartyInfo", NavneromUtility.EbXmlCore);
            {
                XmlElement from = partyInfo.AppendChildElement("From", "eb", NavneromUtility.EbXmlCore, Context);
                {
                    XmlElement partyId = from.AppendChildElement("PartyId", "eb", NavneromUtility.EbXmlCore, Context);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = Settings.Databehandler.Organisasjonsnummer.Iso6523();

                    XmlElement role = from.AppendChildElement("Role", "eb", NavneromUtility.EbXmlCore, Context);
                    role.InnerText = "urn:sdp:avsender";
                }

                XmlElement to = partyInfo.AppendChildElement("To", "eb", NavneromUtility.EbXmlCore, Context);
                {
                    XmlElement partyId = to.AppendChildElement("PartyId", "eb", NavneromUtility.EbXmlCore, Context);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = Settings.Konfigurasjon.MeldingsformidlerOrganisasjon.Iso6523();

                    XmlElement role = to.AppendChildElement("Role", "eb", NavneromUtility.EbXmlCore, Context);
                    role.InnerText = "urn:sdp:meldingsformidler";
                }
            }
            return partyInfo;
        }

        private XmlElement CollaborationInfoElement()
        {
            XmlElement collaborationInfo = Context.CreateElement("eb", "CollaborationInfo", NavneromUtility.EbXmlCore);
            {
                PMode currPmode = Settings.Forsendelse.PostInfo.PMode();
                var currPmodeRef = PModeHelper.EnumToRef(currPmode);

                XmlElement agreementRef = collaborationInfo.AppendChildElement("AgreementRef","eb",NavneromUtility.EbXmlCore,Context);
                agreementRef.InnerText = currPmodeRef;

                XmlElement service = collaborationInfo.AppendChildElement("Service", "eb", NavneromUtility.EbXmlCore, Context);
                service.InnerText = "SDP";

                XmlElement action = collaborationInfo.AppendChildElement("Action", "eb", NavneromUtility.EbXmlCore, Context);
                action.InnerText = currPmode.ToString();

                XmlElement conversationId = collaborationInfo.AppendChildElement("ConversationId", "eb", NavneromUtility.EbXmlCore, Context);
                conversationId.InnerText = Settings.Forsendelse.KonversasjonsId.ToString();
            }
            return collaborationInfo;
        }

        private XmlElement PayloadInfoElement()
        {
            // Mer info på http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/UserMessage/PayloadInfo

            XmlElement payloadInfo = Context.CreateElement("eb", "PayloadInfo", NavneromUtility.EbXmlCore);
            {
                XmlElement partInfoBody = payloadInfo.AppendChildElement("PartInfo", "eb", NavneromUtility.EbXmlCore, Context);
                partInfoBody.SetAttribute("href", "#"+Settings.GuidHandler.BodyId);

                XmlElement partInfoDokumentpakke = payloadInfo.AppendChildElement("PartInfo", "eb", NavneromUtility.EbXmlCore, Context);
                partInfoDokumentpakke.SetAttribute("href", "cid:"+Settings.GuidHandler.DokumentpakkeId);
                {
                    XmlElement partProperties = partInfoDokumentpakke.AppendChildElement("PartProperties", "eb", NavneromUtility.EbXmlCore, Context);
                    {
                        XmlElement propertyMimeType = partProperties.AppendChildElement("Property", "eb", NavneromUtility.EbXmlCore, Context);
                        propertyMimeType.SetAttribute("name", "MimeType");
                        propertyMimeType.InnerText = "application/cms";

                        XmlElement propertyContent = partProperties.AppendChildElement("Property", "eb", NavneromUtility.EbXmlCore, Context);
                        propertyContent.SetAttribute("name", "Content");
                        propertyContent.InnerText = "sdp:Dokumentpakke";
                    }
                }
            }
            return payloadInfo;
        }
    }
}
