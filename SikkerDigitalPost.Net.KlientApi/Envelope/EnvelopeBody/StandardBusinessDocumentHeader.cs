using System.Xml;
﻿using System;
﻿using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class StandardBusinessDocumentHeader : XmlPart
    {
       private readonly DateTime _creationDateAndtime;

        public StandardBusinessDocumentHeader(XmlDocument dokument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler, DateTime creationDateAndtime) : base(dokument, forsendelse, asicEArkiv, databehandler)
        {
            _creationDateAndtime = creationDateAndtime;
        }

        public override XmlElement Xml()
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
            XmlElement sbdHeaderElement = XmlEnvelope.CreateElement("ns3", "StandardBusinessDocumentHeader", Navnerom.Ns3);
            {
                XmlElement headerVersion = XmlEnvelope.CreateElement("ns3", "HeaderVersion", Navnerom.Ns3);
                headerVersion.InnerText = "1.0";
                sbdHeaderElement.AppendChild(headerVersion);
            }
            return sbdHeaderElement;

        }

        private XmlElement SenderElement()
        {
            XmlElement senderElement = XmlEnvelope.CreateElement("ns3", "Sender", Navnerom.Ns3);
            {
                XmlElement identifier = XmlEnvelope.CreateElement("ns3", "Identifier", Navnerom.Ns3);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
                senderElement.AppendChild(identifier);
            }
            return senderElement;
        }

        private XmlElement ReceiverElement()
        {
            XmlElement receiverElement = XmlEnvelope.CreateElement("ns3", "Receiver", Navnerom.Ns3);
            {
                XmlElement identifier = XmlEnvelope.CreateElement("ns3", "Identifier", Navnerom.Ns3);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Forsendelse.DigitalPost.Mottaker.OrganisasjonsnummerPostkasse.Iso6523();
                receiverElement.AppendChild(identifier);
            }
            return receiverElement;            
        }

        private XmlElement DocumentIdentificationElement()
        {
            XmlElement documentIdentificationElement = XmlEnvelope.CreateElement("ns3", "DocumentIdentification", Navnerom.Ns3);
            {
                XmlElement standard = XmlEnvelope.CreateElement("ns3", "Standard", Navnerom.Ns3);
                standard.InnerText = "urn:no:difi:sdp:1.0";
                documentIdentificationElement.AppendChild(standard);

                XmlElement typeVersion = XmlEnvelope.CreateElement("ns3", "typeVersion", Navnerom.Ns3);
                typeVersion.InnerText = "1.0";
                documentIdentificationElement.AppendChild(typeVersion);

                XmlElement instanceIdentifier = XmlEnvelope.CreateElement("ns3", "InstanceIdentifier", Navnerom.Ns3);
                instanceIdentifier.InnerText = "HER_SKAL_INSTANCEIDENTIFIER_INN"; // Guid RegEx [a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}
                documentIdentificationElement.AppendChild(instanceIdentifier);

                XmlElement type = XmlEnvelope.CreateElement("ns3", "Type", Navnerom.Ns3);
                type.InnerText = "IKKE FERDIG:DIGIPOST_ELLER_KVITTERING"; // "digitalPost" eller "kvittering"
                documentIdentificationElement.AppendChild(type);

                XmlElement creationDateAndTime = XmlEnvelope.CreateElement("ns3", "CreationDateAndTime", Navnerom.Ns3);
                creationDateAndTime.InnerText = _creationDateAndtime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
                documentIdentificationElement.AppendChild(creationDateAndTime);
            }
            return documentIdentificationElement;
        }

        private XmlElement BusinessScopeElement()
        {
            XmlElement businessScopeElement = XmlEnvelope.CreateElement("ns3", "BusinessScopeElement", Navnerom.Ns3);
            {
                XmlElement scope = XmlEnvelope.CreateElement("ns3", "Scope", Navnerom.Ns3);
                {
                    XmlElement type = XmlEnvelope.CreateElement("ns3", "Type", Navnerom.Ns3);
                    type.InnerText = "ConversationId";
                    scope.AppendChild(type);

                    XmlElement instanceIdentifier = XmlEnvelope.CreateElement("ns3", "InstanceIdentifier", Navnerom.Ns3);
                    instanceIdentifier.InnerText = Forsendelse.KonversasjonsId;
                    scope.AppendChild(instanceIdentifier);

                    XmlElement identifier = XmlEnvelope.CreateElement("ns3", "Identifier", Navnerom.Ns3);
                    identifier.InnerText = "urn:no:difi:sdp:1.0";
                    scope.AppendChild(identifier);
                }
                businessScopeElement.AppendChild(scope);
            }
            return businessScopeElement;
        }
    }
}
