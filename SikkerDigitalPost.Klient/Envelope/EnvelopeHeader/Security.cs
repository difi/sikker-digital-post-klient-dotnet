using System;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Utilities;
using SikkerDigitalPost.Klient.Xml;
using SikkerDigitalPost.Domene.Extensions;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeHeader
{
    internal class Security : XmlPart
    {
        private XmlElement _securityElement;

        public Security(XmlDocument dokument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler)
            : base(dokument, forsendelse, asicEArkiv, databehandler)
        {
        }

        public override XmlElement Xml()
        {
            _securityElement = XmlEnvelope.CreateElement("wsse", "Security", Navnerom.wsse);
            _securityElement.SetAttribute("xmlns:wsu", Navnerom.wsu);
            _securityElement.SetAttribute("mustUnderstand", Navnerom.env, "true");
            _securityElement.AppendChild(BinarySecurityTokenElement());
            _securityElement.AppendChild(TimestampElement());
            return _securityElement;
        }

        private XmlElement BinarySecurityTokenElement()
        {
            var binarySecurityTokenId = String.Format("X509-{0}", Guid.NewGuid());
            
            XmlElement binarySecurityToken = XmlEnvelope.CreateElement("wsse", "BinarySecurityToken", Navnerom.wsse);
            binarySecurityToken.SetAttribute("id", Navnerom.wsu, binarySecurityTokenId);
            binarySecurityToken.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityToken.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityToken.InnerText = Convert.ToBase64String(Databehandler.Sertifikat.RawData);

            return binarySecurityToken;
        }

        private XmlElement TimestampElement()
        {
            XmlElement timestamp = XmlEnvelope.CreateElement("wsu", "Timestamp", Navnerom.wsu);
            {
                XmlElement created = timestamp.AppendChildElement("Created", "wsu", Navnerom.wsu, XmlEnvelope);
                created.InnerText = DateTime.UtcNow.ToString(DateUtility.DateFormat);

                XmlElement expires = timestamp.AppendChildElement("Expires", "wsu", Navnerom.wsu, XmlEnvelope);
                expires.InnerText = DateTime.UtcNow.AddMinutes(5).ToString(DateUtility.DateFormat);
            }

            timestamp.SetAttribute("Id", Navnerom.wsu, GuidUtility.TimestampId);
            return timestamp;
        }

        public void AddSignatureElement()
        {
            SignedXml signed = new SignedXmlWithAgnosticId(XmlEnvelope, Databehandler.Sertifikat, "env");

            //Body
            {
                var bodyReference = new Sha256Reference("#" + GuidUtility.BodyId);
                bodyReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(bodyReference);
            }

            //TimestampElement
            {
                var timestampReference = new Sha256Reference("#" + GuidUtility.TimestampId);
                timestampReference.AddTransform(new XmlDsigExcC14NTransform("wsse env"));
                signed.AddReference(timestampReference);
            }

            //EbMessaging
            {
                var ebMessagingReference = new Sha256Reference("#" + GuidUtility.EbMessagingId);
                ebMessagingReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(ebMessagingReference);
            }

            //Partinfo/Dokumentpakke
            {
                var partInfoReference = new Sha256Reference(AsicEArkiv.KrypterteBytes(Forsendelse.DigitalPost.Mottaker.Sertifikat));
                partInfoReference.Uri = GuidUtility.DokumentpakkeId;
                partInfoReference.AddTransform(new AttachmentContentSignatureTransform());
                signed.AddReference(partInfoReference);
            }

            signed.KeyInfo.AddClause(new SecurityTokenReferenceClause("#" + GuidUtility.BinarySecurityTokenId));
            signed.ComputeSignature();

            _securityElement.AppendChild(XmlEnvelope.ImportNode(signed.GetXml(), true));
        }
    }
}
