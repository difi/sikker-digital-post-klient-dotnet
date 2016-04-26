using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope
{
    internal class Security : EnvelopeXmlPart
    {
        public Security(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var securityElement = Context.CreateElement("wsse", "Security", NavneromUtility.WssecuritySecext10);
            securityElement.SetAttribute("xmlns:wsu", NavneromUtility.WssecurityUtility10);
            securityElement.SetAttribute("mustUnderstand", NavneromUtility.SoapEnvelopeEnv12, "true");
            securityElement.AppendChild(BinarySecurityTokenElement());
            securityElement.AppendChild(TimestampElement());
            return securityElement;
        }

        private XmlElement BinarySecurityTokenElement()
        {
            var binarySecurityToken = Context.CreateElement("wsse", "BinarySecurityToken", NavneromUtility.WssecuritySecext10);
            binarySecurityToken.SetAttribute("Id", NavneromUtility.WssecurityUtility10, Settings.GuidUtility.BinarySecurityTokenId);
            binarySecurityToken.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityToken.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityToken.InnerText = Convert.ToBase64String(Settings.Databehandler.Sertifikat.RawData);
            return binarySecurityToken;
        }

        private XmlElement TimestampElement()
        {
            var timestamp = Context.CreateElement("wsu", "Timestamp", NavneromUtility.WssecurityUtility10);
            {
                var utcNow = DateTime.UtcNow;
                var created = timestamp.AppendChildElement("Created", "wsu", NavneromUtility.WssecurityUtility10, Context);
                created.InnerText = utcNow.ToString(DateUtility.DateFormat);

                // http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/WebserviceSecurity
                // Time-to-live skal være 120 sekunder
                var expires = timestamp.AppendChildElement("Expires", "wsu", NavneromUtility.WssecurityUtility10, Context);
                expires.InnerText = utcNow.AddSeconds(120).ToString(DateUtility.DateFormat);
            }

            timestamp.SetAttribute("Id", NavneromUtility.WssecurityUtility10, Settings.GuidUtility.TimestampId);
            return timestamp;
        }
    }
}