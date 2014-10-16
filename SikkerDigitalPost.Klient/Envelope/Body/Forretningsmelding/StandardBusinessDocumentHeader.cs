using System;
using System.Xml;
using SikkerDigitalPost.Domene;
using SikkerDigitalPost.Domene.Extensions;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient.Envelope.Body.Forretningsmelding
{
    internal class StandardBusinessDocumentHeader : EnvelopeXmlPart
    {
        private readonly DateTime _creationDateAndtime;
        private const string SdpVersion = "urn:no:difi:sdp:1.0";
        
        public StandardBusinessDocumentHeader(EnvelopeSettings settings, XmlDocument context, DateTime creationDateAndTime) : base(settings, context)
        {
            _creationDateAndtime = creationDateAndTime;
        }

        public override XmlNode Xml()
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
            XmlElement standardBusinessDocumentHeader = Context.CreateElement("ns3", "StandardBusinessDocumentHeader", Navnerom.Ns3);
            {
                XmlElement headerVersion = standardBusinessDocumentHeader.AppendChildElement("HeaderVersion", "ns3", Navnerom.Ns3, Context);
                headerVersion.InnerText = "1.0";
            }
            return standardBusinessDocumentHeader;
        }

        private XmlElement SenderElement()
        {
            XmlElement sender = Context.CreateElement("ns3", "Sender", Navnerom.Ns3);
            {
                XmlElement identifier = sender.AppendChildElement("Identifier", "ns3", Navnerom.Ns3, Context);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Settings.Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
            }
            return sender;
        }

        private XmlElement ReceiverElement()
        {
            XmlElement receiver = Context.CreateElement("ns3", "Receiver", Navnerom.Ns3);
            {
                XmlElement identifier = receiver.AppendChildElement("Identifier", "ns3", Navnerom.Ns3, Context);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Settings.Forsendelse.DigitalPost.Mottaker.OrganisasjonsnummerPostkasse.Iso6523();
            }
            return receiver;
        }

        private XmlElement DocumentIdentificationElement()
        {
            XmlElement documentIdentification = Context.CreateElement("ns3", "DocumentIdentification", Navnerom.Ns3);
            {
                XmlElement standard = documentIdentification.AppendChildElement("Standard", "ns3", Navnerom.Ns3, Context);
                standard.InnerText = SdpVersion;

                XmlElement typeVersion = documentIdentification.AppendChildElement("TypeVersion", "ns3", Navnerom.Ns3, Context);
                typeVersion.InnerText = "1.0";

                XmlElement instanceIdentifier = documentIdentification.AppendChildElement("InstanceIdentifier", "ns3", Navnerom.Ns3, Context);
                instanceIdentifier.InnerText = Settings.GuidHandler.StandardBusinessDocumentHeaderId;

                XmlElement type = documentIdentification.AppendChildElement("Type", "ns3", Navnerom.Ns3, Context);
                type.InnerText = "digitalPost";

                XmlElement creationDateAndTime = documentIdentification.AppendChildElement("CreationDateAndTime", "ns3", Navnerom.Ns3, Context);
                creationDateAndTime.InnerText = _creationDateAndtime.ToString(DateUtility.DateFormat);
            }
            return documentIdentification;
        }

        private XmlElement BusinessScopeElement()
        {
            XmlElement businessScope = Context.CreateElement("ns3", "BusinessScope", Navnerom.Ns3);
            {
                XmlElement scope = businessScope.AppendChildElement("Scope", "ns3", Navnerom.Ns3, Context);
                {
                    XmlElement type = scope.AppendChildElement("Type", "ns3", Navnerom.Ns3, Context);
                    type.InnerText = "ConversationId";

                    XmlElement instanceIdentifier = scope.AppendChildElement("InstanceIdentifier", "ns3", Navnerom.Ns3, Context);
                    instanceIdentifier.InnerText = Settings.Forsendelse.KonversasjonsId;

                    XmlElement identifier = scope.AppendChildElement("Identifier", "ns3", Navnerom.Ns3, Context);
                    identifier.InnerText = SdpVersion;
                }
            }
            return businessScope;
        }
    }
}
