using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
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
            XmlElement standardBusinessDocumentHeader = Context.CreateElement("ns3", "StandardBusinessDocumentHeader", NavneromUtility.StandardBusinessDocumentHeader);
            {
                XmlElement headerVersion = standardBusinessDocumentHeader.AppendChildElement("HeaderVersion", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                headerVersion.InnerText = "1.0";
            }
            return standardBusinessDocumentHeader;
        }

        private XmlElement SenderElement()
        {
            XmlElement sender = Context.CreateElement("ns3", "Sender", NavneromUtility.StandardBusinessDocumentHeader);
            {
                XmlElement identifier = sender.AppendChildElement("Identifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Settings.Databehandler.Organisasjonsnummer.Iso6523();
            }
            return sender;
        }

        private XmlElement ReceiverElement()
        {
            XmlElement receiver = Context.CreateElement("ns3", "Receiver", NavneromUtility.StandardBusinessDocumentHeader);
            {
                XmlElement identifier = receiver.AppendChildElement("Identifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Settings.Forsendelse.PostInfo.Mottaker.OrganisasjonsnummerPostkasse.Iso6523();
            }
            return receiver;
        }

        private XmlElement DocumentIdentificationElement()
        {
            XmlElement documentIdentification = Context.CreateElement("ns3", "DocumentIdentification", NavneromUtility.StandardBusinessDocumentHeader);
            {
                XmlElement standard = documentIdentification.AppendChildElement("Standard", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                standard.InnerText = SdpVersion;

                XmlElement typeVersion = documentIdentification.AppendChildElement("TypeVersion", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                typeVersion.InnerText = "1.0";

                XmlElement instanceIdentifier = documentIdentification.AppendChildElement("InstanceIdentifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                instanceIdentifier.InnerText = Settings.GuidHandler.StandardBusinessDocumentHeaderId;

                XmlElement type = documentIdentification.AppendChildElement("Type", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                type.InnerText = "digitalPost";

                XmlElement creationDateAndTime = documentIdentification.AppendChildElement("CreationDateAndTime", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                creationDateAndTime.InnerText = _creationDateAndtime.ToString(DateUtility.DateFormat);
            }
            return documentIdentification;
        }

        private XmlElement BusinessScopeElement()
        {
            XmlElement businessScope = Context.CreateElement("ns3", "BusinessScope", NavneromUtility.StandardBusinessDocumentHeader);
            {
                XmlElement scope = businessScope.AppendChildElement("Scope", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                {
                    XmlElement type = scope.AppendChildElement("Type", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                    type.InnerText = "ConversationId";

                    XmlElement instanceIdentifier = scope.AppendChildElement("InstanceIdentifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                    instanceIdentifier.InnerText = Settings.Forsendelse.KonversasjonsId.ToString();

                    XmlElement identifier = scope.AppendChildElement("Identifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                    identifier.InnerText = SdpVersion;
                }
            }
            return businessScope;
        }
    }
}
