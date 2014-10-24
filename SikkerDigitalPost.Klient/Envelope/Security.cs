using System;
using System.Xml;
using SikkerDigitalPost.Domene.Extensions;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class Security : EnvelopeXmlPart
    {
        public Security(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var securityElement = Context.CreateElement("wsse", "Security", Navnerom.wsse);
            securityElement.SetAttribute("xmlns:wsu", Navnerom.wsu);
            securityElement.SetAttribute("mustUnderstand", Navnerom.env, "true");
            securityElement.AppendChild(BinarySecurityTokenElement());
            securityElement.AppendChild(TimestampElement());
            return securityElement;
        }

        private XmlElement BinarySecurityTokenElement()
        {
            XmlElement binarySecurityToken = Context.CreateElement("wsse", "BinarySecurityToken", Navnerom.wsse);
            binarySecurityToken.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.BinarySecurityTokenId);
            binarySecurityToken.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityToken.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityToken.InnerText = Convert.ToBase64String(Settings.Databehandler.Sertifikat.RawData);
            return binarySecurityToken;
        }

        private XmlElement TimestampElement()
        {
            XmlElement timestamp = Context.CreateElement("wsu", "Timestamp", Navnerom.wsu);
            {
                var utcNow = DateTime.UtcNow;
                XmlElement created = timestamp.AppendChildElement("Created", "wsu", Navnerom.wsu, Context);
                created.InnerText = utcNow.ToString(DateUtility.DateFormat);
                
                // http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/WebserviceSecurity
                // Time-to-live skal være 120 sekunder
                XmlElement expires = timestamp.AppendChildElement("Expires", "wsu", Navnerom.wsu, Context);
                expires.InnerText = utcNow.AddSeconds(120).ToString(DateUtility.DateFormat);
            }

            timestamp.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.TimestampId);
            return timestamp;
        }
    }
}
