using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Extensions;
using System.Collections.Generic;
using System;
using SikkerDigitalPost.Net.KlientApi.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    public class Security : XmlPart
    {

        private XmlElement _securityElement;

        public Security(XmlDocument dokument, Forsendelse forsendelse)
            : base(dokument, forsendelse)
        {
        }

        public override XmlElement Xml()
        {
            _securityElement = XmlDocument.CreateElement("wsse", "Security", Navnerom.wsse);
            _securityElement.SetAttribute("xmlns:wsu", Navnerom.wsu);
            _securityElement.SetAttribute("mustUnderstand", Navnerom.XmlnsEnv, "true");
            _securityElement.AppendChild(BinarySecurityToken());
            _securityElement.AppendChild(Timestamp());
            return _securityElement;
        }

        private XmlElement BinarySecurityToken()
        {
            var binarySecurityToken = XmlDocument.CreateElement("wsse", "BinarySecurityToken", Navnerom.wsse);
            binarySecurityToken.SetAttribute("id", Navnerom.wsu, "ID_SKAL_SETTES_INN_HER");
            binarySecurityToken.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityToken.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityToken.InnerText = "THE_BINARY_SECURITY_TOKEN";

            return binarySecurityToken;
        }

        private XmlElement Timestamp()
        {
            XmlElement timestamp = XmlDocument.CreateElement("wsu", "Timestamp", Navnerom.wsu);
            {
                var created = timestamp.AppendChildElement("Created", "wsu", Navnerom.wsu, XmlDocument);
                created.InnerText = "TIMESTAMP_CREATED";

                var expires = timestamp.AppendChildElement("Expires", "wsu", Navnerom.wsu, XmlDocument);
                expires.InnerText = "TIMESTAMP_EXPIRES";
            }

            timestamp.SetAttribute("Id", Navnerom.wsu, "WSU_TIMESTAMP_ID");
            return timestamp;
        }

        public void AddSignature()
        {
            var timestampId = Guid.NewGuid();
            var messagingId = Guid.NewGuid();
            var bodyId = Guid.NewGuid();

            var signed = new SignedXmlWithAgnosticId(XmlDocument, Forsendelse.DigitalPost.Mottaker.Sertifikat, "env");

            {
                var bodyReference = new Sha256Reference("#id-" + bodyId);                               //Body
                bodyReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(bodyReference);
            }

            {
                var timestampReference = new Sha256Reference("#TS-" + timestampId);                     //Timestamp
                timestampReference.AddTransform(new XmlDsigExcC14NTransform("wsse env"));
                signed.AddReference(timestampReference);
            }

            {
                var ebMessagingReference = new Sha256Reference("#id-" + messagingId);                   //EbMessaging
                ebMessagingReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(ebMessagingReference);
            }

            {
                var partInfoReference = new Sha256Reference(new byte[] { 21, 53 }); //Filpakke.Data);       //Partinfo/Dokumentpakke
                partInfoReference.Uri = "cid:" + "FILPAKKE_CID"; // Filpakke.ContentId;
                partInfoReference.AddTransform(new AttachmentContentSignatureTransform());
                signed.AddReference(partInfoReference);
            }

            var bstId = "X509-" + Guid.NewGuid();
            signed.KeyInfo.AddClause(new SecurityTokenReferenceClause("#" + bstId));
            signed.ComputeSignature();

            _securityElement.AppendChild(XmlDocument.ImportNode(signed.GetXml(), true));
        }

        //private XmlElement SignedInfo()
        //{
        //    XmlElement signedInfo = XmlDocument.CreateElement("ds", "SignedInfo", Navnerom.ds);
        //    {
        //        //Kanokaliseringsmetode
        //        XmlElement canocalizationMethod = XmlDocument.CreateElement("ds", "CanocalizationMethod", Navnerom.ds);
        //        canocalizationMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/10/xml-exc-c14n#");
        //        {
        //            XmlElement inclusiveNamespaces = canocalizationMethod.AppendChildElement("InclusiveNamespaces", "ec", Navnerom.ec, XmlDocument);
        //            inclusiveNamespaces.SetAttribute("PrefixList", "env");
        //        }
        //        signedInfo.AppendChild(canocalizationMethod);

        //        //Signaturmetode
        //        XmlElement signatureMethod = XmlDocument.CreateElement("ds", "SignatureMethod", Navnerom.ds);
        //        signatureMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");
        //        signedInfo.AppendChild(signatureMethod);

        //        //Referansesignaturer
        //        foreach (XmlElement reference in SignedInfoReferences())
        //        {
        //            signedInfo.AppendChild(reference);
        //        }

        //        //Signaturverdi
        //        XmlElement signatureValue = signedInfo.AppendChildElement("SignatureValue", "ds", Navnerom.ds, XmlDocument);
        //        signatureValue.InnerText = "SIGNATUR_VERDI";

        //        //KeyInfo
        //        signedInfo.AppendChild(KeyInfo());

        //    }

        //    return signedInfo;
        //}
        
        //private IEnumerable<XmlElement> SignedInfoReferences()
        //{
        //    var bodyReference = SignedInfoReference("BODY_URI", "BODY_DIGEST");                                 //Body
        //    var timestampReference = SignedInfoReference("TIMESTAMP_URI", "TIMESTAMP_DIGEST", "wsse", "env");   //Timestamp (I header)
        //    var ebMessagingReference = SignedInfoReference("EB_MESSAGE_URI", "EB_MESSAGE_DIGEST", "");          //eb:messaging (I header)
        //    var partInfoReference = SignedInfoReference("PARTINFO_URI", "PARTINFO_DIGEST");                     //ns6:PartInfo href=.... (I body)

        //    return new List<XmlElement>()
        //    {
        //         bodyReference,
        //         timestampReference,
        //         ebMessagingReference,
        //         partInfoReference
        //    };
        //}

        //private XmlElement SignedInfoReference(string uri, string digestValue, params string[] prefixList)
        //{
        //    XmlElement reference = XmlDocument.CreateElement("ds", "Reference", Navnerom.ds);
        //    reference.SetAttribute("URI", uri);
        //    {
        //        XmlElement transforms = XmlDocument.CreateElement("ds", "Transforms", Navnerom.ds);
        //        {
        //            XmlElement transform = transforms.AppendChildElement("Transform", "ds", Navnerom.ds, XmlDocument);
        //            transform.SetAttribute("Algorithm", "http://www.w3.org/2001/10/xml-exc-c14n#");
        //            {
        //                XmlElement inclusiveNamespaces = transform.AppendChildElement("InclusiveNamespaces", "ec", Navnerom.ec, XmlDocument);
        //                inclusiveNamespaces.SetAttribute("PrefixList", String.Join(" ", prefixList));
        //            }
        //        }
        //        reference.AppendChild(transforms);
        //    }

        //    return reference;
        //}

        private XmlElement KeyInfo()
        {
            XmlElement keyInfo = XmlDocument.CreateElement("ds", "KeyInfo", Navnerom.ds);
            keyInfo.SetAttribute("Id", "KEY_INFO_ID");
            {
                XmlElement securityTokenReference = keyInfo.AppendChildElement("SecurityTokenReference", "wsse", Navnerom.wsse, XmlDocument);
                securityTokenReference.SetAttribute("wsu:Id", "WSU_ID");
                {
                    XmlElement reference = securityTokenReference.AppendChildElement("Reference", "wsse", Navnerom.wsse, XmlDocument);
                    reference.SetAttribute("URI", "URI_REFERENCE");
                    reference.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
                }
            }

            return keyInfo;
        }
    }
}
