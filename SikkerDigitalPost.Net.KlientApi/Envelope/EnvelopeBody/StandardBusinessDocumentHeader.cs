using System;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class StandardBusinessDocumentHeader
    {
        private const string Ns3 = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader";

        private readonly XmlDocument _dokument;
        private readonly Forsendelse _forsendelse;
        private readonly DateTime _creationDateAndtime;

        public StandardBusinessDocumentHeader(XmlDocument dokument, Forsendelse forsendelse, DateTime creationDateAndtime)
        {
            _dokument = dokument;
            _forsendelse = forsendelse;
            _creationDateAndtime = creationDateAndtime;
        }

        public XmlElement Xml()
        {
            XmlElement sbdHeaderElement = SbdHeaderElement();
            {
                sbdHeaderElement.AppendChild(SenderElement());
                sbdHeaderElement.AppendChild(ReceiverElement());
                sbdHeaderElement.AppendChild(DocumentIdentificationElement());
                sbdHeaderElement.AppendChild(BusinessScopeElement());
            }
            return sbdHeaderElement;
        }

        private XmlElement SbdHeaderElement()
        {
            XmlElement sbdHeaderElement = _dokument.CreateElement("ns3", "StandardBusinessDocumentHeader", Ns3);
            {
                XmlElement headerVersion = _dokument.CreateElement("ns3", "HeaderVersion", Ns3);
                headerVersion.InnerText = "1.0";
                sbdHeaderElement.AppendChild(headerVersion);
            }
            return sbdHeaderElement;

        }

        private XmlElement SenderElement()
        {
            XmlElement senderElement = _dokument.CreateElement("ns3", "Sender", Ns3);
            {
                XmlElement identifier = _dokument.CreateElement("ns3", "Identifier", Ns3);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = _forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
                senderElement.AppendChild(identifier);
            }
            return senderElement;
        }

        private XmlElement ReceiverElement()
        {
            XmlElement receiverElement = _dokument.CreateElement("ns3", "Receiver", Ns3);
            {
                XmlElement identifier = _dokument.CreateElement("ns3", "Identifier", Ns3);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = _forsendelse.DigitalPost.Mottaker.OrganisasjonsnummerPostkasse;
                receiverElement.AppendChild(identifier);
            }
            return receiverElement;            
        }

        private XmlElement DocumentIdentificationElement()
        {
            XmlElement documentIdentificationElement = _dokument.CreateElement("ns3", "DocumentIdentification", Ns3);
            {
                XmlElement standard = _dokument.CreateElement("ns3", "Standard", Ns3);
                standard.InnerText = "urn:no:difi:sdp:1.0";
                documentIdentificationElement.AppendChild(standard);

                XmlElement typeVersion = _dokument.CreateElement("ns3", "typeVersion", Ns3);
                typeVersion.InnerText = "1.0";
                documentIdentificationElement.AppendChild(typeVersion);

                XmlElement instanceIdentifier = _dokument.CreateElement("ns3", "InstanceIdentifier", Ns3);
                instanceIdentifier.InnerText = "Instance identifier"; // Guid RegEx [a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}
                documentIdentificationElement.AppendChild(instanceIdentifier);

                XmlElement type = _dokument.CreateElement("ns3", "Type", Ns3);
                type.InnerText = "Placeholder"; // "digitalPost" eller "kvittering"
                documentIdentificationElement.AppendChild(type);

                XmlElement creationDateAndTime = _dokument.CreateElement("ns3", "CreationDateAndTime", Ns3);
                creationDateAndTime.InnerText = _creationDateAndtime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
                documentIdentificationElement.AppendChild(creationDateAndTime);
            }
            return documentIdentificationElement;
        }

        private XmlElement BusinessScopeElement()
        {
            XmlElement businessScopeElement = _dokument.CreateElement("ns3", "BusinessScopeElement", Ns3);
            {
                XmlElement scope = _dokument.CreateElement("ns3", "Scope", Ns3);
                {
                    XmlElement type = _dokument.CreateElement("ns3", "Type", Ns3);
                    type.InnerText = "ConversationId";
                    scope.AppendChild(type);

                    XmlElement instanceIdentifier = _dokument.CreateElement("ns3", "InstanceIdentifier", Ns3);
                    instanceIdentifier.InnerText = _forsendelse.KonversasjonsId;
                    scope.AppendChild(instanceIdentifier);

                    XmlElement identifier = _dokument.CreateElement("ns3", "Identifier", Ns3);
                    identifier.InnerText = "urn:no:difi:sdp:1.0";
                    scope.AppendChild(identifier);
                }
                businessScopeElement.AppendChild(scope);
            }
            return businessScopeElement;
        }
    }
}
