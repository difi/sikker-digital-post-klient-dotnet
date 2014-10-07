using System;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Klient.Utilities;
using SikkerDigitalPost.Klient.Xml;
using SikkerDigitalPost.Domene.Extensions;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeHeader
{
    internal class Security : XmlPart
    {
        private XmlElement _securityElement;

        public Security(Envelope rot) : base(rot)
        {
        }

        public override XmlElement Xml()
        {
            _securityElement = Rot.EnvelopeXml.CreateElement("wsse", "Security", Navnerom.wsse);
            _securityElement.SetAttribute("xmlns:wsu", Navnerom.wsu);
            _securityElement.SetAttribute("mustUnderstand", Navnerom.env, "true");
            _securityElement.AppendChild(BinarySecurityTokenElement());
            _securityElement.AppendChild(TimestampElement());
            return _securityElement;
        }

        private XmlElement BinarySecurityTokenElement()
        {
            XmlElement binarySecurityToken = Rot.EnvelopeXml.CreateElement("wsse", "BinarySecurityToken", Navnerom.wsse);
            binarySecurityToken.SetAttribute("Id", Navnerom.wsu, Rot.GuidHandler.BinarySecurityTokenId);
            binarySecurityToken.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityToken.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityToken.InnerText = Convert.ToBase64String(Rot.Databehandler.Sertifikat.RawData);
            return binarySecurityToken;
        }

        private XmlElement TimestampElement()
        {
            XmlElement timestamp = Rot.EnvelopeXml.CreateElement("wsu", "Timestamp", Navnerom.wsu);
            {
                XmlElement created = timestamp.AppendChildElement("Created", "wsu", Navnerom.wsu, Rot.EnvelopeXml);
                created.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);
                
                // http://begrep.difi.no/SikkerDigitalPost/1.0.2/transportlag/WebserviceSecurity
                // Time-to-live skal være 120 sekunder
                XmlElement expires = timestamp.AppendChildElement("Expires", "wsu", Navnerom.wsu, Rot.EnvelopeXml);
                expires.InnerText = DateTime.UtcNow.AddSeconds(120).ToString(DateUtility.DateFormat);
            }

            timestamp.SetAttribute("Id", Navnerom.wsu, Rot.GuidHandler.TimestampId);
            return timestamp;
        }

        public void AddSignatureElement()
        {
            SignedXml signed = new SignedXmlWithAgnosticId(Rot.EnvelopeXml, Rot.Forsendelse.DigitalPost.Mottaker.Sertifikat, "env");

            //Body
            {
                var bodyReference = new Sha256Reference("#" + Rot.GuidHandler.BodyId);
                bodyReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(bodyReference);
            }

            //TimestampElement
            {
                var timestampReference = new Sha256Reference("#" + Rot.GuidHandler.TimestampId);
                timestampReference.AddTransform(new XmlDsigExcC14NTransform("wsse env"));
                signed.AddReference(timestampReference);
            }

            //EbMessaging
            {
                var ebMessagingReference = new Sha256Reference("#" + Rot.GuidHandler.EbMessagingId);
                ebMessagingReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(ebMessagingReference);
            }

            //Partinfo/Dokumentpakke
            {
                var partInfoReference = new Sha256Reference(Rot.AsicEArkiv.KrypterteBytes(Rot.Forsendelse.DigitalPost.Mottaker.Sertifikat));
                partInfoReference.Uri = Rot.GuidHandler.DokumentpakkeId;
                partInfoReference.AddTransform(new AttachmentContentSignatureTransform());
                signed.AddReference(partInfoReference);
            }

            signed.KeyInfo.AddClause(new SecurityTokenReferenceClause("#" + Rot.GuidHandler.BinarySecurityTokenId));
            signed.ComputeSignature();

            _securityElement.AppendChild(Rot.EnvelopeXml.ImportNode(signed.GetXml(), true));
        }
    }
}
