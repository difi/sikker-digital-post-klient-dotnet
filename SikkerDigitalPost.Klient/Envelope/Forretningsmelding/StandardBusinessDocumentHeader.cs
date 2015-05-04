/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Xml;
using SikkerDigitalPost.Domene.Extensions;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient.Envelope.Forretningsmelding
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
            XmlElement standardBusinessDocumentHeader = Context.CreateElement("ns3", "StandardBusinessDocumentHeader", Navnerom.StandardBusinessDocumentHeader);
            {
                XmlElement headerVersion = standardBusinessDocumentHeader.AppendChildElement("HeaderVersion", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                headerVersion.InnerText = "1.0";
            }
            return standardBusinessDocumentHeader;
        }

        private XmlElement SenderElement()
        {
            XmlElement sender = Context.CreateElement("ns3", "Sender", Navnerom.StandardBusinessDocumentHeader);
            {
                XmlElement identifier = sender.AppendChildElement("Identifier", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Settings.Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
            }
            return sender;
        }

        private XmlElement ReceiverElement()
        {
            XmlElement receiver = Context.CreateElement("ns3", "Receiver", Navnerom.StandardBusinessDocumentHeader);
            {
                XmlElement identifier = receiver.AppendChildElement("Identifier", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                identifier.SetAttribute("Authority", "iso6523-actorid-upis");
                identifier.InnerText = Settings.Forsendelse.PostInfo.Mottaker.OrganisasjonsnummerPostkasse.Iso6523();
            }
            return receiver;
        }

        private XmlElement DocumentIdentificationElement()
        {
            XmlElement documentIdentification = Context.CreateElement("ns3", "DocumentIdentification", Navnerom.StandardBusinessDocumentHeader);
            {
                XmlElement standard = documentIdentification.AppendChildElement("Standard", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                standard.InnerText = SdpVersion;

                XmlElement typeVersion = documentIdentification.AppendChildElement("TypeVersion", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                typeVersion.InnerText = "1.0";

                XmlElement instanceIdentifier = documentIdentification.AppendChildElement("InstanceIdentifier", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                instanceIdentifier.InnerText = Settings.GuidHandler.StandardBusinessDocumentHeaderId;

                XmlElement type = documentIdentification.AppendChildElement("Type", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                type.InnerText = "digitalPost";

                XmlElement creationDateAndTime = documentIdentification.AppendChildElement("CreationDateAndTime", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                creationDateAndTime.InnerText = _creationDateAndtime.ToString(DateUtility.DateFormat);
            }
            return documentIdentification;
        }

        private XmlElement BusinessScopeElement()
        {
            XmlElement businessScope = Context.CreateElement("ns3", "BusinessScope", Navnerom.StandardBusinessDocumentHeader);
            {
                XmlElement scope = businessScope.AppendChildElement("Scope", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                {
                    XmlElement type = scope.AppendChildElement("Type", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                    type.InnerText = "ConversationId";

                    XmlElement instanceIdentifier = scope.AppendChildElement("InstanceIdentifier", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                    instanceIdentifier.InnerText = Settings.Forsendelse.KonversasjonsId.ToString();

                    XmlElement identifier = scope.AppendChildElement("Identifier", "ns3", Navnerom.StandardBusinessDocumentHeader, Context);
                    identifier.InnerText = SdpVersion;
                }
            }
            return businessScope;
        }
    }
}
