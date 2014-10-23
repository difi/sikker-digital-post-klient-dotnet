using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace SikkerDigitalPost.Klient.Security
{
    /// <summary>
    /// Enhances the core SignedXml provider with namespace agnostic query for Id elements.
    /// </summary>
    /// <remarks>
    /// From: http://stackoverflow.com/questions/5099156/malformed-reference-element-when-adding-a-reference-based-on-an-id-attribute-w
    /// </remarks>
    internal sealed class SignedXmlWithAgnosticId : SignedXml
    {
        private XmlDocument m_containingDocument;

        public SignedXmlWithAgnosticId(XmlDocument xml)
            : base(xml)
        {
            this.m_containingDocument = xml;
        }

        public SignedXmlWithAgnosticId(XmlElement xmlElement)
            : base(xmlElement)
        {
            this.m_containingDocument = xmlElement.OwnerDocument;
        }

        /// <summary>
        /// Sets SHA256 as signaure method and XmlDsigExcC14NTransformUrl as canonicalization method
        /// </summary>
        /// <param name="xml">The document containing the references to be signed.</param>
        /// <param name="certificate">The certificate containing the private key used for signing.</param>
        /// <param name="inclusiveNamespacesPrefixList">An optional list of namespaces to be set as the canonicalization namespace prefix list.</param>
        public SignedXmlWithAgnosticId(XmlDocument xml, X509Certificate2 certificate, string inclusiveNamespacesPrefixList = null)
            : base(xml)
        {
            Initialize(xml, certificate, inclusiveNamespacesPrefixList);
        }

        /// <summary>
        /// Sets SHA256 as signaure method and XmlDsigExcC14NTransformUrl as canonicalization method
        /// </summary>
        /// <param name="xmlElement">The xml element containing the references to be signed.</param>
        /// <param name="certificate">The certificate containing the private key used for sigining.</param>
        /// <param name="inclusiveNamespacesPrefixList">An optional list of namespaces to be set as the canonicalization namespace prefix list.</param>
        public SignedXmlWithAgnosticId(XmlElement xmlElement, X509Certificate2 certificate, string inclusiveNamespacesPrefixList = null)
            : base(xmlElement)
        {
            Initialize(xmlElement.OwnerDocument, certificate, inclusiveNamespacesPrefixList);
        }

        private void Initialize(XmlDocument xml, X509Certificate2 certificate, string inclusiveNamespacesPrefixList = null)
        {
            const string signatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            // Adds signature method to crypto api
            if (CryptoConfig.CreateFromName(signatureMethod) == null)
                CryptoConfig.AddAlgorithm(typeof(RsaPkCs1Sha256SignatureDescription), signatureMethod);

            // Makes sure the signingkey is using Microsoft Enhanced RSA and AES Cryptographic Provider which enables SHA256
            if (!certificate.HasPrivateKey)
                throw new Exception(string.Format("Angitt sertifikat med fingeravtrykk {0} inneholder ikke en privatnøkkel. Dette er påkrevet for å signere xml dokumenter.", certificate.Thumbprint));

            var targetKey = certificate.PrivateKey as RSACryptoServiceProvider;
            if (targetKey == null)
                throw new Exception(string.Format("Privatnøkkelen i sertifikatet med fingeravtrykk {0} er ikke en gyldig RSA asymetrisk nøkkel.", certificate.Thumbprint));

            if (targetKey.CspKeyContainerInfo.ProviderType == 24)
                SigningKey = targetKey;
            else
            {
                SigningKey = new RSACryptoServiceProvider();
                try
                {
                    SigningKey.FromXmlString(certificate.PrivateKey.ToXmlString(true));
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Angitt sertifikat med fingeravtrykk {0} kan ikke eksporteres. Det er nødvendig når sertifikatet ikke er opprettet med 'Microsoft Enhanced RSA and AES Cryptographic Provider' som CryptoAPI provider name (-sp parameter i makecert.exe eller -csp parameter i openssl).", certificate.Thumbprint), e);
                }
            }

            SignedInfo.SignatureMethod = signatureMethod;
            SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
            if (inclusiveNamespacesPrefixList != null)
                ((XmlDsigExcC14NTransform)SignedInfo.CanonicalizationMethodObject).InclusiveNamespacesPrefixList = inclusiveNamespacesPrefixList;

            this.m_containingDocument = xml;
        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // Attemt to find id node using standard methods. If not found, attempt using namespace agnostic method.
            XmlElement idElem = base.GetIdElement(doc, id) ?? FindIdElement(doc, id);

            // Check to se if id element is within the signatures object node. This is used by ESIs Xml Advanced Electronic Signatures (Xades)
            if (idElem == null)
            {
                if (Signature != null && Signature.ObjectList != null)
                {
                    foreach (DataObject dataObject in Signature.ObjectList)
                    {
                        if (dataObject.Data != null && dataObject.Data.Count > 0)
                        {
                            foreach (XmlNode dataNode in dataObject.Data)
                            {
                                idElem = FindIdElement(dataNode, id);
                                if (idElem != null)
                                {
                                    return idElem;
                                }
                            }
                        }
                    }
                }
            }

            return idElem;
        }

        private XmlElement FindIdElement(XmlNode node, string idValue)
        {
            XmlElement result = null;
            foreach (string s in new[] { "Id", "ID", "id" })
            {
                result = node.SelectSingleNode(string.Format("//*[@*[local-name() = '{0}'] = '{1}']", s, idValue)) as XmlElement;
                if (result != null)
                    break;
            }

            return result;
        }

        protected override AsymmetricAlgorithm GetPublicKey()
        {
            AsymmetricAlgorithm result = base.GetPublicKey();

            // If we have no key, attemt to find one via a SecurityTokenReference.
            if (result == null && this.KeyInfo != null)
            {
                var keyinfo = this.KeyInfo.GetXml();
                if (keyinfo != null)
                {
                    XmlNamespaceManager mgr = new XmlNamespaceManager(keyinfo.OwnerDocument.NameTable);
                    mgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                    mgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                    var reference = keyinfo.SelectSingleNode("./wsse:SecurityTokenReference/wsse:Reference", mgr);
                    if (reference != null)
                    {
                        var targetId = reference.Attributes["URI"].Value;
                        if (targetId.StartsWith("#"))
                            targetId = targetId.Substring(1);
                        var keyElement = FindIdElement(m_containingDocument, targetId);
                        if (keyElement != null && !string.IsNullOrEmpty(keyElement.InnerText))
                        {
                            result = new X509Certificate2(Convert.FromBase64String(keyElement.InnerText)).PublicKey.Key;
                        }
                    }
                }
            }

            return result;
        }
    }
}