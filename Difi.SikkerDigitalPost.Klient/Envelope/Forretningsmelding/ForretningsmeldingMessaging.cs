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
        public ForretningsmeldingMessaging(EnvelopeSettings settings, XmlDocument context)
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

            messaging.AppendChild(UserMessageElement());

            return messaging;
        }

        private XmlElement UserMessageElement()
        {
            var userMessage = Context.CreateElement("eb", "UserMessage", NavneromUtility.EbXmlCore);
            userMessage.SetAttribute("mpc", Settings.Forsendelse.Mpc);

            userMessage.AppendChild(MessageInfoElement());
            userMessage.AppendChild(PartyInfoElement());
            userMessage.AppendChild(CollaborationInfoElement());
            userMessage.AppendChild(PayloadInfoElement());

            return userMessage;
        }

        private XmlElement MessageInfoElement()
        {
            var messageInfo = Context.CreateElement("eb", "MessageInfo", NavneromUtility.EbXmlCore);
            {
                var timestamp = messageInfo.AppendChildElement("Timestamp", "eb", NavneromUtility.EbXmlCore, Context);
                timestamp.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                // http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/UserMessage/MessageInfo
                // Unik identifikator, satt av MSH. Kan med fordel benytte SBDH.InstanceIdentifier 
                var messageId = messageInfo.AppendChildElement("MessageId", "eb", NavneromUtility.EbXmlCore, Context);
                messageId.InnerText = Settings.GuidUtility.MessageId;
            }
            return messageInfo;
        }

        private XmlElement PartyInfoElement()
        {
            var partyInfo = Context.CreateElement("eb", "PartyInfo", NavneromUtility.EbXmlCore);
            {
                var from = partyInfo.AppendChildElement("From", "eb", NavneromUtility.EbXmlCore, Context);
                {
                    var partyId = from.AppendChildElement("PartyId", "eb", NavneromUtility.EbXmlCore, Context);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = Settings.Databehandler.Organisasjonsnummer.WithCountryCode;

                    var role = from.AppendChildElement("Role", "eb", NavneromUtility.EbXmlCore, Context);
                    role.InnerText = "urn:sdp:avsender";
                }

                var to = partyInfo.AppendChildElement("To", "eb", NavneromUtility.EbXmlCore, Context);
                {
                    var partyId = to.AppendChildElement("PartyId", "eb", NavneromUtility.EbXmlCore, Context);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = Settings.Konfigurasjon.MeldingsformidlerOrganisasjon.WithCountryCode;

                    var role = to.AppendChildElement("Role", "eb", NavneromUtility.EbXmlCore, Context);
                    role.InnerText = "urn:sdp:meldingsformidler";
                }
            }
            return partyInfo;
        }

        private XmlElement CollaborationInfoElement()
        {
            var collaborationInfo = Context.CreateElement("eb", "CollaborationInfo", NavneromUtility.EbXmlCore);
            {
                var currPmode = Settings.Forsendelse.PostInfo.PMode();
                var currPmodeRef = PModeHelper.EnumToRef(currPmode);

                var agreementRef = collaborationInfo.AppendChildElement("AgreementRef", "eb", NavneromUtility.EbXmlCore, Context);
                agreementRef.InnerText = currPmodeRef;

                var service = collaborationInfo.AppendChildElement("Service", "eb", NavneromUtility.EbXmlCore, Context);
                service.InnerText = "SDP";

                var action = collaborationInfo.AppendChildElement("Action", "eb", NavneromUtility.EbXmlCore, Context);
                action.InnerText = currPmode.ToString();

                var conversationId = collaborationInfo.AppendChildElement("ConversationId", "eb", NavneromUtility.EbXmlCore, Context);
                conversationId.InnerText = Settings.Forsendelse.KonversasjonsId.ToString();
            }
            return collaborationInfo;
        }

        private XmlElement PayloadInfoElement()
        {
            // Mer info på http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/UserMessage/PayloadInfo

            var payloadInfo = Context.CreateElement("eb", "PayloadInfo", NavneromUtility.EbXmlCore);
            {
                var partInfoBody = payloadInfo.AppendChildElement("PartInfo", "eb", NavneromUtility.EbXmlCore, Context);
                partInfoBody.SetAttribute("href", "#" + Settings.GuidUtility.BodyId);

                var partInfoDokumentpakke = payloadInfo.AppendChildElement("PartInfo", "eb", NavneromUtility.EbXmlCore, Context);
                partInfoDokumentpakke.SetAttribute("href", "cid:" + Settings.GuidUtility.DokumentpakkeId);
                {
                    var partProperties = partInfoDokumentpakke.AppendChildElement("PartProperties", "eb", NavneromUtility.EbXmlCore, Context);
                    {
                        var propertyMimeType = partProperties.AppendChildElement("Property", "eb", NavneromUtility.EbXmlCore, Context);
                        propertyMimeType.SetAttribute("name", "MimeType");
                        propertyMimeType.InnerText = "application/cms";

                        var propertyContent = partProperties.AppendChildElement("Property", "eb", NavneromUtility.EbXmlCore, Context);
                        propertyContent.SetAttribute("name", "Content");
                        propertyContent.InnerText = "sdp:Dokumentpakke";
                    }
                }
            }
            return payloadInfo;
        }
    }
}