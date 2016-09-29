using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class StandardBusinessDocumentHeader : EnvelopeXmlPart
    {
        private const string SdpVersion = "urn:no:difi:sdp:1.0";
        private readonly DateTime _creationDateAndtime;

        public StandardBusinessDocumentHeader(EnvelopeSettings settings, XmlDocument context, DateTime creationDateAndTime)
            : base(settings, context)
        {
            _creationDateAndtime = creationDateAndTime;
        }

        public override XmlNode Xml()
        {
            var standardBusinessDocumentHeaderElement = StandardBusinessDocumentHeaderElement();
            standardBusinessDocumentHeaderElement.AppendChild(SenderElement());
            standardBusinessDocumentHeaderElement.AppendChild(ReceiverElement());
            standardBusinessDocumentHeaderElement.AppendChild(DocumentIdentificationElement());
            standardBusinessDocumentHeaderElement.AppendChild(BusinessScopeElement());

            return standardBusinessDocumentHeaderElement;
        }

        private XmlElement StandardBusinessDocumentHeaderElement()
        {
            var standardBusinessDocumentHeader = Context.CreateElement("ns3", "StandardBusinessDocumentHeader", NavneromUtility.StandardBusinessDocumentHeader);
            {
                var headerVersion = standardBusinessDocumentHeader.AppendChildElement("HeaderVersion", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                headerVersion.InnerText = "1.0";
            }
            return standardBusinessDocumentHeader;
        }

        private XmlElement SenderElement()
        {
            var sender = Context.CreateElement("ns3", "Sender", NavneromUtility.StandardBusinessDocumentHeader);
            {
                var identifier = sender.AppendChildElement("Identifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Settings.Databehandler.Organisasjonsnummer.WithCountryCode;
            }
            return sender;
        }

        private XmlElement ReceiverElement()
        {
            var receiver = Context.CreateElement("ns3", "Receiver", NavneromUtility.StandardBusinessDocumentHeader);
            {
                var identifier = receiver.AppendChildElement("Identifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Settings.Forsendelse.PostInfo.Mottaker.OrganisasjonsnummerPostkasse.WithCountryCode;
            }
            return receiver;
        }

        private XmlElement DocumentIdentificationElement()
        {
            var documentIdentification = Context.CreateElement("ns3", "DocumentIdentification", NavneromUtility.StandardBusinessDocumentHeader);
            {
                var standard = documentIdentification.AppendChildElement("Standard", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                standard.InnerText = SdpVersion;

                var typeVersion = documentIdentification.AppendChildElement("TypeVersion", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                typeVersion.InnerText = "1.0";

                var instanceIdentifier = documentIdentification.AppendChildElement("InstanceIdentifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                instanceIdentifier.InnerText = Settings.GuidUtility.InstanceIdentifier;

                var type = documentIdentification.AppendChildElement("Type", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                type.InnerText = "digitalPost";

                var creationDateAndTime = documentIdentification.AppendChildElement("CreationDateAndTime", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                creationDateAndTime.InnerText = _creationDateAndtime.ToString(DateUtility.DateFormat);
            }
            return documentIdentification;
        }

        private XmlElement BusinessScopeElement()
        {
            var businessScope = Context.CreateElement("ns3", "BusinessScope", NavneromUtility.StandardBusinessDocumentHeader);
            {
                var scope = businessScope.AppendChildElement("Scope", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                {
                    var type = scope.AppendChildElement("Type", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                    type.InnerText = "ConversationId";

                    var instanceIdentifier = scope.AppendChildElement("InstanceIdentifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                    instanceIdentifier.InnerText = Settings.Forsendelse.KonversasjonsId.ToString();

                    var identifier = scope.AppendChildElement("Identifier", "ns3", NavneromUtility.StandardBusinessDocumentHeader, Context);
                    identifier.InnerText = SdpVersion;
                }
            }
            return businessScope;
        }
    }
}