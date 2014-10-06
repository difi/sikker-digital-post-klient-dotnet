using System;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Utilities;
using SikkerDigitalPost.Domene.Extensions;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeHeader
{
    internal class Messaging : XmlPart
    {
        public Messaging(Envelope rot) : base(rot)
        {
        }

        public override XmlElement Xml()
        {
            XmlElement messaging = Rot.EnvelopeXml.CreateElement("eb", "Messaging", Navnerom.eb);
            messaging.SetAttribute("xmlns:wsu", Navnerom.wsu);
            messaging.SetAttribute("MustUnderstand", Navnerom.env, "true");
            messaging.SetAttribute("Id", Navnerom.wsu, Rot.GuidUtility.EbMessagingId);
            
            messaging.AppendChild(UserMessageElement());

            return messaging;
        }

        public XmlElement UserMessageElement()
        {
            XmlElement userMessage = Rot.EnvelopeXml.CreateElement("ns6", "UserMessageElement", Navnerom.Ns6);
            userMessage.SetAttribute("xmlns:ns2", Navnerom.Ns2);
            userMessage.SetAttribute("xmlns:ns3", Navnerom.Ns3);
            userMessage.SetAttribute("xmlns:ns4", Navnerom.Ns4);
            userMessage.SetAttribute("xmlns:ns5", Navnerom.Ns5);
            userMessage.SetAttribute("xmlns:ns8", Navnerom.Ns8);
            userMessage.SetAttribute("xmlns:ns9", Navnerom.Ns9);
            userMessage.SetAttribute("xmlns:ns10", Navnerom.Ns10);
            userMessage.SetAttribute("xmlns:ns11", Navnerom.Ns11);
            userMessage.SetAttribute("mpc", Rot.Forsendelse.MpcId);

            userMessage.AppendChild(MessageInfoElement());
            userMessage.AppendChild(PartyInfoElement());
            userMessage.AppendChild(CollaborationInfoElement());
            userMessage.AppendChild(PayloadInfoElement());

            return userMessage;
        }

        public XmlElement MessageInfoElement()
        {
            XmlElement messageInfo = Rot.EnvelopeXml.CreateElement("ns6", "MessageInfoElement", Navnerom.Ns6);
            {
                XmlElement timestamp =  messageInfo.AppendChildElement("TimeStamp", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                timestamp.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                XmlElement messageId = messageInfo.AppendChildElement("MessageId", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                messageId.InnerText = Rot.GuidUtility.StandardBusinessDocumentHeaderId;
            }
            return messageInfo;
        }

        public XmlElement PartyInfoElement()
        {
            XmlElement partyInfo = Rot.EnvelopeXml.CreateElement("ns6", "PartyInfoElement", Navnerom.Ns6);
            {
                XmlElement from = partyInfo.AppendChildElement("From", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                {
                    XmlElement partyId = from.AppendChildElement("PartyId", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = Rot.Databehandler.Organisasjonsnummer.Iso6523();

                    XmlElement role = from.AppendChildElement("Role", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                    role.InnerText = "urn:sdp:avsender";
                }

                XmlElement to = partyInfo.AppendChildElement("To", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                {
                    XmlElement partyId = to.AppendChildElement("PartyId", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                    partyId.SetAttribute("type", "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908");
                    partyId.InnerText = "9908:984661185";

                    XmlElement role = to.AppendChildElement("Role", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                    role.InnerText = "urn:sdp:meldingsformidler";
                }

            }
            return partyInfo;
        }

        public XmlElement CollaborationInfoElement()
        {
            XmlElement collaborationInfo = Rot.EnvelopeXml.CreateElement("ns6", "CollaborationInfoElement", Navnerom.Ns6);
            {
                XmlElement agreementRef = collaborationInfo.AppendChildElement("AgreementRef","ns6",Navnerom.Ns6,Rot.EnvelopeXml);
                agreementRef.InnerText = "http://begrep.difi.no/SikkerDigitalPost/Meldingsutveksling/FormidleDigitalPostForsendelse";

                XmlElement service = collaborationInfo.AppendChildElement("Service", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                service.InnerText = "SDP";

                XmlElement action = collaborationInfo.AppendChildElement("Action", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                action.InnerText = "FormidleDigitalPost";

                XmlElement conversationId = collaborationInfo.AppendChildElement("ConversationId", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                conversationId.InnerText = Rot.Forsendelse.KonversasjonsId;

            }

            return collaborationInfo;
        }

        public XmlElement PayloadInfoElement()
        {
            //Mer info på http://begrep.difi.no/SikkerDigitalPost/1.0.0-rc.2/UserMessageElement/PayloadInfoElement

            XmlElement payloadInfo = Rot.EnvelopeXml.CreateElement("ns6", "PayloadInfoElement", Navnerom.Ns6);
            {
                XmlElement partInfoBody = payloadInfo.AppendChildElement("partInfo", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                partInfoBody.SetAttribute("href", Rot.GuidUtility.BodyId);

                XmlElement partInfoDokumentpakke = payloadInfo.AppendChildElement("partInfo", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                partInfoDokumentpakke.SetAttribute("href", Rot.GuidUtility.DokumentpakkeId);
                {
                    XmlElement partProperties = partInfoDokumentpakke.AppendChildElement("PartProperties", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                    {
                        XmlElement propertyMimeType = partProperties.AppendChildElement("Property", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                        propertyMimeType.SetAttribute("name", "MimeType");
                        propertyMimeType.InnerText = "application/octet-stream";

                        XmlElement propertyContent = partProperties.AppendChildElement("Property", "ns6", Navnerom.Ns6, Rot.EnvelopeXml);
                        propertyContent.SetAttribute("name", "Content");
                        propertyContent.InnerText = "sdp:Dokumentpakke";
                    }
                }
            }
            return payloadInfo;
        }
    }
}