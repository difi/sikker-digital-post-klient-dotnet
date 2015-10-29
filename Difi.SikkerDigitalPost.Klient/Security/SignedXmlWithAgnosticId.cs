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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Security
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
        public SignedXmlWithAgnosticId(XmlDocument xmlDocument)
            : base(xmlDocument)
        {
            this.m_containingDocument = xmlDocument;
        }

        public SignedXmlWithAgnosticId(XmlElement xmlElement)
            : base(xmlElement)
        {
            this.m_containingDocument = xmlElement.OwnerDocument;
        }

        /// <summary>
        /// Sets SHA256 as signaure method and XmlDsigExcC14NTransformUrl as canonicalization method
        /// </summary>
        /// <param name="xmlDocument">The document containing the references to be signed.</param>
        /// <param name="certificate">The certificate containing the private key used for signing.</param>
        /// <param name="inclusiveNamespacesPrefixList">An optional list of namespaces to be set as the canonicalization namespace prefix list.</param>
        public SignedXmlWithAgnosticId(XmlDocument xmlDocument, X509Certificate2 certificate, string inclusiveNamespacesPrefixList = null)
            : base(xmlDocument)
        {
            Initialize(xmlDocument, certificate, inclusiveNamespacesPrefixList);
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
           PublicKey = HentNesteKeySomViSkalSjekkeSignaturMot();
           return PublicKey;
        }

        AsymmetricAlgorithm PublicKey = null;

        private AsymmetricAlgorithm HentNesteKeySomViSkalSjekkeSignaturMot()
        {
            var harHentetPublicKeyTidligere = PublicKey != null;
            AsymmetricAlgorithm publicKey = null;

            if (this.KeyInfo == null)
            {
                throw new CryptographicException("Kryptografi_Xml_Keyinfo nødvendig");
            }

            if (!harHentetPublicKeyTidligere)
            {
                var keyInfoXml = KeyInfo.GetXml();
                if (keyInfoXml != null)
                {
                    var keyInfoNamespaceMananger = GetKeyInfoNamespaceMananger(keyInfoXml);
                    var securityTokenReference = SecurityTokenReference(keyInfoXml, keyInfoNamespaceMananger);

                    if (securityTokenReference != null)
                    {
                        var securityTokenReferanseUri = HentSecurityTokenReferanseUri(securityTokenReference);
                        var binarySecurityTokenSertifikat = HentBinarySecurityToken(securityTokenReferanseUri);

                        publicKey = binarySecurityTokenSertifikat.PublicKey.Key;
                    }
                }
            }

            return publicKey;
        }

        private static XmlNamespaceManager GetKeyInfoNamespaceMananger(XmlElement keyInfoXml)
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(keyInfoXml.OwnerDocument.NameTable);
            mgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            mgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

            return mgr; 
        }

        private static XmlNode SecurityTokenReference(XmlElement keyInfoXml, XmlNamespaceManager keyInfoNamespaceMananger)
        {
            return keyInfoXml.SelectSingleNode("./wsse:SecurityTokenReference/wsse:Reference", keyInfoNamespaceMananger);
        }

        private string HentSecurityTokenReferanseUri(XmlNode reference)
        {
            var uriRefereanseAttributt = reference.Attributes["URI"];

            if (uriRefereanseAttributt == null)
            {
                throw new SdpSecurityException("Klarte ikke finne SecurityTokenReferenceUri.");
            }

            var referanseUriVerdi = uriRefereanseAttributt.Value;
            if (referanseUriVerdi.StartsWith("#"))
            {
                referanseUriVerdi = referanseUriVerdi.Substring(1);
            }

            return referanseUriVerdi;
            
        }

        private X509Certificate2 HentBinarySecurityToken(string securityTokenReferanseUri)
        {
            X509Certificate2 publicsertifikat = null;

            var keyElement = FindIdElement(m_containingDocument, securityTokenReferanseUri);
            if (keyElement != null && !string.IsNullOrEmpty(keyElement.InnerText))
            {
                publicsertifikat = new X509Certificate2(Convert.FromBase64String(keyElement.InnerText));
            }

            return publicsertifikat;
        }
    }
}
