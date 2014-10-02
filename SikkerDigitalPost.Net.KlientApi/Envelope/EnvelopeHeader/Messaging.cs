using System;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Extensions;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Messaging : XmlPart
    {
        public Messaging(XmlDocument xmlEnvelope, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler) : base(xmlEnvelope, forsendelse, asicEArkiv, databehandler)
        {
        }

        public override XmlElement Xml()
        {
            XmlElement messaging = XmlEnvelope.CreateElement("eb", "Messaging", Navnerom.eb);
            messaging.SetAttribute("xmlns:wsu", Navnerom.wsu);
            messaging.AppendChild(UserMessage());

            return messaging;
        }

        public XmlElement UserMessage()
        {
            XmlElement userMessage = XmlEnvelope.CreateElement("ns6", "UserMessage", Navnerom.Ns6);
            userMessage.SetAttribute("ns2", Navnerom.Ns2);
            userMessage.SetAttribute("ns3", Navnerom.Ns3);
            userMessage.SetAttribute("ns4", Navnerom.Ns4);
            userMessage.SetAttribute("ns5", Navnerom.Ns5);
            userMessage.SetAttribute("ns6", Navnerom.Ns6);
            userMessage.SetAttribute("ns8", Navnerom.Ns8);
            userMessage.SetAttribute("ns9", Navnerom.Ns9);
            userMessage.SetAttribute("ns10", Navnerom.Ns10);
            userMessage.SetAttribute("ns11", Navnerom.Ns11);
            userMessage.SetAttribute("mpc", Forsendelse.MpcId);
            userMessage.SetAttribute("Id", Navnerom.wsu, "EN_ELLER_ANNEN_GUID");

            userMessage.AppendChild(MessageInfo());
            userMessage.AppendChild(PartyInfo());
            userMessage.AppendChild(CollaborationInfo());
            userMessage.AppendChild(PayloadInfo());

            return userMessage;
        }

        public XmlElement MessageInfo()
        {
            XmlElement messageInfo = XmlEnvelope.CreateElement("ns6", "MessageInfo", Navnerom.Ns6);
            {
                XmlElement timestamp =  messageInfo.AppendChildElement("TimeStamp", "ns6", Navnerom.Ns6, XmlEnvelope);
                timestamp.InnerText = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffZ");

                XmlElement messageId = messageInfo.AppendChildElement("MessageId", "ns6", Navnerom.Ns6, XmlEnvelope);
                messageId.InnerText = "HER SKAL DET STÅ SBDH.INSTANCEIDENTIFIER";
            }
            return messageInfo;
        }

        public XmlElement PartyInfo()
        {
            XmlElement partyInfo = XmlEnvelope.CreateElement("ns6", "PartyInfo", Navnerom.Ns6);
            {
                XmlElement from = partyInfo.AppendChildElement("From", "ns6", Navnerom.Ns6, XmlEnvelope);
                {
                    XmlElement partyId = from.AppendChildElement("PartyId", "ns6", Navnerom.Ns6, XmlEnvelope);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = Databehandler.Organisasjonsnummer.Iso6523();

                    XmlElement role = from.AppendChildElement("Role", "ns6", Navnerom.Ns6, XmlEnvelope);
                    role.InnerText = "urn:sdp:avsender";
                }

                XmlElement to = partyInfo.AppendChildElement("To", "ns6", Navnerom.Ns6, XmlEnvelope);
                {
                    XmlElement partyId = to.AppendChildElement("PartyId", "ns6", Navnerom.Ns6, XmlEnvelope);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = "9908:984661185";

                    XmlElement role = to.AppendChildElement("Role", "ns6", Navnerom.Ns6, XmlEnvelope);
                    role.InnerText = "urn:sdp:meldingsformidler";
                }

            }
            return partyInfo;
        }

        public XmlElement CollaborationInfo()
        {
            XmlElement collaborationInfo = XmlEnvelope.CreateElement("ns6", "CollaborationInfo", Navnerom.Ns6);
            {
                XmlElement agreementRef = collaborationInfo.AppendChildElement("AgreementRef","ns6",Navnerom.Ns6,XmlEnvelope);
                agreementRef.InnerText = "http://begrep.difi.no/SikkerDigitalPost/Meldingsutveksling/FormidleDigitalPostForsendelse";

                XmlElement service = collaborationInfo.AppendChildElement("Service", "ns6", Navnerom.Ns6, XmlEnvelope);
                service.InnerText = "SDP";

                XmlElement action = collaborationInfo.AppendChildElement("Action", "ns6", Navnerom.Ns6, XmlEnvelope);
                action.InnerText = "FormidleDigitalPost";

                XmlElement conversationId = collaborationInfo.AppendChildElement("ConversationId", "ns6", Navnerom.Ns6, XmlEnvelope);
                conversationId.InnerText = Forsendelse.KonversasjonsId;

            }

            return collaborationInfo;
        }

        public XmlElement PayloadInfo()
        {
            //Mer info på http://begrep.difi.no/SikkerDigitalPost/1.0.0-rc.2/UserMessage/PayloadInfo
            XmlElement payloadInfo = XmlEnvelope.CreateElement("ns6", "PayloadInfo", Navnerom.Ns6);
            {
                XmlElement partInfoBody = payloadInfo.AppendChildElement("partInfo", "ns6", Navnerom.Ns6, XmlEnvelope);
                partInfoBody.SetAttribute("href", "HER SKAL BODY ID INN");

                XmlElement partInfoDokumentpakke = payloadInfo.AppendChildElement("partInfo", "ns6", Navnerom.Ns6, XmlEnvelope);
                partInfoDokumentpakke.SetAttribute("href", "cid:sdp:asic");
                {
                    XmlElement partProperties = partInfoDokumentpakke.AppendChildElement("PartProperties", "ns6", Navnerom.Ns6, XmlEnvelope);
                    {
                        XmlElement propertyMimeType = partProperties.AppendChildElement("Property", "ns6", Navnerom.Ns6, XmlEnvelope);
                        propertyMimeType.SetAttribute("name", "MimeType");
                        propertyMimeType.InnerText = "application/octet-stream";

                        XmlElement propertyContent = partProperties.AppendChildElement("Property", "ns6", Navnerom.Ns6, XmlEnvelope);
                        propertyContent.SetAttribute("name", "Content");
                        propertyContent.InnerText = "sdp:Dokumentpakke";
                    }
                }
            }
            return payloadInfo;
        }
    }
}
