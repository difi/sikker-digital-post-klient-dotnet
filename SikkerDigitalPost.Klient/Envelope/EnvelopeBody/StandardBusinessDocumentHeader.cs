using System;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Utilities;
using SikkerDigitalPost.Domene.Extensions;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeBody
{
    internal class StandardBusinessDocumentHeader : XmlPart
    {
        private readonly DateTime _creationDateAndtime;
        private const string _sdpVersion = "urn:no:difi:sdp:1.0";
        
        public StandardBusinessDocumentHeader(Envelope rot, DateTime creationDateAndTime) : base(rot)
        {
            _creationDateAndtime = creationDateAndTime;
        }

        public override XmlElement Xml()
        {
            XmlElement standardBusinessDocumentHeaderElement = StandardBusinessDocumentHeaderElement();
            standardBusinessDocumentHeaderElement.AppendChild(SenderElement());
            standardBusinessDocumentHeaderElement.AppendChild(ReceiverElement());
            standardBusinessDocumentHeaderElement.AppendChild(DocumentIdentificationElement());
            standardBusinessDocumentHeaderElement.AppendChild(BusinessScopeElement());

            return standardBusinessDocumentHeaderElement;
        }

        private XmlElement StandardBusinessDocumentHeaderElement()
        {
            XmlElement standardBusinessDocumentHeader = Rot.EnvelopeXml.CreateElement("ns3", "StandardBusinessDocumentHeader", Navnerom.Ns3);
            {
                XmlElement headerVersion = standardBusinessDocumentHeader.AppendChildElement("HeaderVersion", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                headerVersion.InnerText = "1.0";
            }
            return standardBusinessDocumentHeader;
        }

        private XmlElement SenderElement()
        {
            XmlElement sender = Rot.EnvelopeXml.CreateElement("ns3", "Sender", Navnerom.Ns3);
            {
                XmlElement identifier = sender.AppendChildElement("Identifier", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Rot.Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
            }
            return sender;
        }

        private XmlElement ReceiverElement()
        {
            XmlElement receiver = Rot.EnvelopeXml.CreateElement("ns3", "Receiver", Navnerom.Ns3);
            {
                XmlElement identifier = receiver.AppendChildElement("Identifier", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Rot.Forsendelse.DigitalPost.Mottaker.OrganisasjonsnummerPostkasse.Iso6523();
            }
            return receiver;
        }

        private XmlElement DocumentIdentificationElement()
        {
            XmlElement documentIdentification = Rot.EnvelopeXml.CreateElement("ns3", "DocumentIdentification", Navnerom.Ns3);
            {
                XmlElement standard = documentIdentification.AppendChildElement("Standard", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                standard.InnerText = _sdpVersion;

                XmlElement typeVersion = documentIdentification.AppendChildElement("TypeVersion", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                typeVersion.InnerText = "1.0";

                XmlElement instanceIdentifier = documentIdentification.AppendChildElement("InstanceIdentifier", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                instanceIdentifier.InnerText = Rot.GuidHandler.StandardBusinessDocumentHeaderId;

                XmlElement type = documentIdentification.AppendChildElement("Type", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                type.InnerText = "digitalPost";

                XmlElement creationDateAndTime = documentIdentification.AppendChildElement("CreationDateAndTime", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                creationDateAndTime.InnerText = _creationDateAndtime.ToString(DateUtility.DateFormat);
            }
            return documentIdentification;
        }

        private XmlElement BusinessScopeElement()
        {
            XmlElement businessScope = Rot.EnvelopeXml.CreateElement("ns3", "BusinessScope", Navnerom.Ns3);
            {
                XmlElement scope = businessScope.AppendChildElement("Scope", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                {
                    XmlElement type = scope.AppendChildElement("Type", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                    type.InnerText = "ConversationId";

                    XmlElement instanceIdentifier = scope.AppendChildElement("InstanceIdentifier", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                    instanceIdentifier.InnerText = Rot.Forsendelse.KonversasjonsId;

                    XmlElement identifier = scope.AppendChildElement("Identifier", "ns3", Navnerom.Ns3, Rot.EnvelopeXml);
                    identifier.InnerText = _sdpVersion;
                }
            }
            return businessScope;
        }
    }
}
