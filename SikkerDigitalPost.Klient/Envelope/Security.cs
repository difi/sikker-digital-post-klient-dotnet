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
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope
{
    internal class Security : EnvelopeXmlPart
    {
        public Security(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var securityElement = Context.CreateElement("wsse", "Security", Navnerom.WssecuritySecext10);
            securityElement.SetAttribute("xmlns:wsu", Navnerom.WssecurityUtility10);
            securityElement.SetAttribute("mustUnderstand", Navnerom.SoapEnvelopeEnv12, "true");
            securityElement.AppendChild(BinarySecurityTokenElement());
            securityElement.AppendChild(TimestampElement());
            return securityElement;
        }

        private XmlElement BinarySecurityTokenElement()
        {
            XmlElement binarySecurityToken = Context.CreateElement("wsse", "BinarySecurityToken", Navnerom.WssecuritySecext10);
            binarySecurityToken.SetAttribute("Id", Navnerom.WssecurityUtility10, Settings.GuidHandler.BinarySecurityTokenId);
            binarySecurityToken.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityToken.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityToken.InnerText = Convert.ToBase64String(Settings.Databehandler.Sertifikat.RawData);
            return binarySecurityToken;
        }

        private XmlElement TimestampElement()
        {
            XmlElement timestamp = Context.CreateElement("wsu", "Timestamp", Navnerom.WssecurityUtility10);
            {
                var utcNow = DateTime.UtcNow;
                XmlElement created = timestamp.AppendChildElement("Created", "wsu", Navnerom.WssecurityUtility10, Context);
                created.InnerText = utcNow.ToString(DateUtility.DateFormat);
                
                // http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/WebserviceSecurity
                // Time-to-live skal være 120 sekunder
                XmlElement expires = timestamp.AppendChildElement("Expires", "wsu", Navnerom.WssecurityUtility10, Context);
                expires.InnerText = utcNow.AddSeconds(120).ToString(DateUtility.DateFormat);
            }

            timestamp.SetAttribute("Id", Navnerom.WssecurityUtility10, Settings.GuidHandler.TimestampId);
            return timestamp;
        }
    }
}
