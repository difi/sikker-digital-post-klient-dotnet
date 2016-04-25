using System;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Security
{
    internal class QualifyingPropertiesObject : DataObject
    {
        /// <param name="certificate">The certificate used for signing.</param>
        /// <param name="target">The target attribute value of the QualifyingProperties element.</param>
        /// <param name="references">List of DataObjectFormat elements to be included.</param>
        /// <param name="context">The element where the signature will be placed. Used for extracting namespaces.</param>
        public QualifyingPropertiesObject(X509Certificate2 certificate, string target, IAsiceAttachable[] references, XmlElement context)
        {
            Certificate = certificate;
            Target = target;
            References = references;
            Data = CreateNodes(context);
        }

        public X509Certificate2 Certificate { get; }

        public IAsiceAttachable[] References { get; }

        /// <summary>
        ///     The mandatory Target attribute refers to the XML signature in which the qualifying properties are associated.
        /// </summary>
        public string Target { get; }

        private XmlNodeList CreateNodes(XmlElement context)
        {
            // To get the digest value for the qualifying properties node, the namespaces of the containing document needs to be available. This is problematic, as the signature element is not added
            // to the document until after the signature has been calculated. To circumvent this, we take the position where the signature will be added (XmlElement context parameter), clone its document
            // and add the QualifyingProperties inside a 'dummy' signature element. This ensures that the canoncalization process will process the QualifyingProperties parent nodes for namespaces.

            // Clone of the target document for the signature.
            var clone = (XmlDocument) context.OwnerDocument.Clone();
            clone.PreserveWhitespace = true;

            // Find where the signature is to be inserted in the cloned document. In our scenario, the signature is placed as a child of the root XAdESSignatures element.
            var cloneContext = clone.DocumentElement;

            // Create a 'dummy' signature node where the QualifyingProperties will be positioned.
            var signature = cloneContext.AppendChild("Signature", NavneromUtility.XmlDsig);

            // Add the QualifyingProperties node as normal. This node will be set as the Objects Data property.
            var root = signature.AppendChild("QualifyingProperties", NavneromUtility.UriEtsi132);
            root.SetAttribute("Target", Target);

            // Create Xml Node List
            var signedProperties = root.AppendChild("SignedProperties", NavneromUtility.UriEtsi132);

            signedProperties.SetAttribute("Id", "SignedProperties");

            var signedSignatureProperties = signedProperties.AppendChild("SignedSignatureProperties", NavneromUtility.UriEtsi132);
            signedSignatureProperties.AppendChild("SigningTime", NavneromUtility.UriEtsi132, DateTime.UtcNow.ToString(DateUtility.DateFormat, CultureInfo.InvariantCulture));
            var signingCertificate = signedSignatureProperties.AppendChild("SigningCertificate", NavneromUtility.UriEtsi132);

            var cert = signingCertificate.AppendChild("Cert", NavneromUtility.UriEtsi132);

            var certDigest = cert.AppendChild("CertDigest", NavneromUtility.UriEtsi132);
            certDigest.AppendChild("DigestMethod", NavneromUtility.XmlDsig).SetAttribute("Algorithm", "http://www.w3.org/2000/09/xmldsig#sha1");
            certDigest.AppendChild("DigestValue", NavneromUtility.XmlDsig, Convert.ToBase64String(Certificate.GetCertHash()));

            var issuerSerial = cert.AppendChild("IssuerSerial", NavneromUtility.UriEtsi132);
            issuerSerial.AppendChild("X509IssuerName", NavneromUtility.XmlDsig, Certificate.Issuer);
            issuerSerial.AppendChild("X509SerialNumber", NavneromUtility.XmlDsig, BigInteger.Parse(Certificate.SerialNumber, NumberStyles.HexNumber).ToString());

            var signedDataObjectProperties = signedProperties.AppendChild("SignedDataObjectProperties", NavneromUtility.UriEtsi132);
            foreach (var item in References)
            {
                var a = signedDataObjectProperties.AppendChild("DataObjectFormat", NavneromUtility.UriEtsi132);
                a.SetAttribute("ObjectReference", "#" + item.Id);
                a.AppendChild("MimeType", "http://uri.etsi.org/01903/v1.3.2#", item.MimeType);
            }

            return root.SelectNodes(".");
        }
    }
}