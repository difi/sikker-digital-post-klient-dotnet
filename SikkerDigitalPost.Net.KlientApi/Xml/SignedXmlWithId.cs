using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace SikkerDigitalPost.Net.Klient.Xml
{
    /// <summary>
    /// Enhances the core SignedXml provider with namespace agnostic query for Id elements.
    /// </summary>
    /// <remarks>
    /// From: http://stackoverflow.com/questions/5099156/malformed-reference-element-when-adding-a-reference-based-on-an-id-attribute-w
    /// </remarks>
    internal class SignedXmlWithAgnosticId : SignedXml
    {
        public SignedXmlWithAgnosticId(XmlDocument xml)
            : base(xml)
        {
        }

        public SignedXmlWithAgnosticId(XmlElement xmlElement)
            : base(xmlElement)
        {
        }

        /// <summary>
        /// Sets SHA256 as signaure method and XmlDsigExcC14NTransformUrl as canonicalization method
        /// </summary>
        /// <param name="xml">The document containing the references to be signed.</param>
        /// <param name="certificate">The certificate containing the private key used for sigining.</param>
        /// <param name="inclusiveNamespacesPrefixList">An optional list of namespaces to be set as the canonicalization namespace prefix list.</param>
        public SignedXmlWithAgnosticId(XmlDocument xml, X509Certificate2 certificate, string inclusiveNamespacesPrefixList = null)
            : base(xml)
        {
            Initialize(certificate, inclusiveNamespacesPrefixList);
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
            Initialize(certificate, inclusiveNamespacesPrefixList);
        }

        protected virtual void Initialize(X509Certificate2 certificate, string inclusiveNamespacesPrefixList = null)
        {
            string signatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            // Adds signature method to crypto api
            if (CryptoConfig.CreateFromName(signatureMethod) == null)
                CryptoConfig.AddAlgorithm(typeof(RsaPkCs1Sha256SignatureDescription), signatureMethod);

            // Makes sure the signingkey is using Microsoft Enhanced RSA and AES Cryptographic Provider which enables SHA256
            SigningKey = new RSACryptoServiceProvider();
            SigningKey.FromXmlString(certificate.PrivateKey.ToXmlString(true));

            SignedInfo.SignatureMethod = signatureMethod;
            SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
            if (inclusiveNamespacesPrefixList != null)
                ((XmlDsigExcC14NTransform)SignedInfo.CanonicalizationMethodObject).InclusiveNamespacesPrefixList = inclusiveNamespacesPrefixList;
        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // Attemt to find id node using standard methods. If not found, attempt using namespace agnostic method.
            XmlElement idElem = base.GetIdElement(doc, id) ?? FindIdElement(doc, id);

            // Check to se if id element is within the signatures object node. This is used by ESIs Xml Advanced Electronic Signatures (Xades)
            if (idElem == null)
            {
                if (this.Signature != null && this.Signature.ObjectList != null)
                {
                    foreach (DataObject dataObject in this.Signature.ObjectList)
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

        protected virtual XmlElement FindIdElement(XmlNode node, string idValue)
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
    }
}
