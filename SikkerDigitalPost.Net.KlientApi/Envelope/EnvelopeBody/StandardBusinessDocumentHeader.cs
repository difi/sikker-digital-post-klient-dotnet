using System.Xml;
using SikkerDigitalPost.Net.Domene.Extensions;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class StandardBusinessDocumentHeader
    {
        private const string Ns3 = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader";

        private readonly XmlDocument _dokument;

        public StandardBusinessDocumentHeader(XmlDocument dokument)
        {
            _dokument = dokument;
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
                XmlElement headerVersion = _dokument.CreateElement("ns3", "StandarBusinessDocumentHeader", Ns3);
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
                identifier.InnerText = "";
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
                identifier.InnerText = "";
                receiverElement.AppendChild(identifier);
            }
            return receiverElement;            
        }

        private XmlElement DocumentIdentificationElement()
        {
            var documentIdentificationElement = _dokument.CreateElement("ns3", "DocumentIdentification", Ns3);
            {
                var standard = _dokument.CreateElement("ns3", "Standard", Ns3);
                standard.InnerText = "urn:no:difi:sdp:1.0";
                documentIdentificationElement.AppendChild(standard);

                var typeVersion = _dokument.CreateElement("ns3", "typeVersion", Ns3);
                typeVersion.InnerText = "1.0";
                documentIdentificationElement.AppendChild(typeVersion);

                var instanceIdentifier = _dokument.CreateElement("ns3", "InstanceIdentifier", Ns3);
                instanceIdentifier.InnerText = "";
                documentIdentificationElement.AppendChild(instanceIdentifier);

                var type = _dokument.CreateElement("ns3", "Type", Ns3);

                var creationDateAndTime = _dokument.CreateElement("ns3", "CreationDateAndTime", Ns3);
            }
            return documentIdentificationElement;
        }

        private XmlElement BusinessScopeElement()
        {
            var businessScopeElement = _dokument.CreateElement("ns3", "BusinessScopeElement", Ns3);
            
            return businessScopeElement;
        }
    }
}
