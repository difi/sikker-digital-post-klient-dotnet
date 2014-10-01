using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter.Interface;
using SikkerDigitalPost.Net.Domene.Extensions;

namespace SikkerDigitalPost.Net.KlientApi.Xml
{
    internal class QualifyingPropertiesObject : DataObject
    {
        private const string DateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffZ";

        public X509Certificate2 Certificate { get; private set; }

        public IAsiceVedlegg[] References { get; private set; }

        /// <summary>
        /// The mandatory Target attribute refers to the XML signature in which the qualifying properties are associated.
        /// </summary>
        public string Target { get; private set; }

        /// <param name="certificate">The certificate used for signing.</param>
        /// <param name="target">The target attribute value of the QualifyingProperties element.</param>
        /// <param name="references">List of DataObjectFormat elements to be included.</param>
        /// <param name="context">The element where the signature will be placed. Used for extracting namespaces.</param>
        public QualifyingPropertiesObject(X509Certificate2 certificate, string target, IAsiceVedlegg[] references, XmlElement context)
        {
            Certificate = certificate;
            Target = target;
            References = references;
            Data = CreateNodes(context);
        }

        private XmlNodeList CreateNodes(XmlElement context)
        {
            // To get the digest value for the qualifying properties node, the namespaces of the containing document needs to be available. This is problematic, as the signature element is not added
            // to the document until after the signature has been calculated. To circumvent this, we take the position where the signature will be added (XmlElement context parameter), clone its document
            // and add the QualifyingProperties inside a 'dummy' signature element. This ensures that the canoncalization process will process the QualifyingProperties parent nodes for namespaces.

            // Clone of the target document for the signature.
            var clone = (XmlDocument)context.OwnerDocument.Clone();
            clone.PreserveWhitespace = true;

            // Find where the signature is to be inserted in the cloned document. In our scenario, the signature is placed as a child of the root XAdESSignatures element.
            var cloneContext = (XmlElement)clone.DocumentElement;

            // Create a 'dummy' signature node where the QualifyingProperties will be positioned.
            var signature = cloneContext.AppendChild("Signature", "http://www.w3.org/2000/09/xmldsig#");

            // Add the QualifyingProperties node as normal. This node will be set as the Objects Data property.
            var root = signature.AppendChild("QualifyingProperties", "http://uri.etsi.org/01903/v1.3.2#");
            root.SetAttribute("Target", this.Target);

            var signedProperties = root.AppendChild("SignedProperties", "http://uri.etsi.org/01903/v1.3.2#");
            signedProperties.SetAttribute("Id", "SignedProperties");

            var signedSignatureProperties = signedProperties.AppendChild("SignedSignatureProperties", "http://uri.etsi.org/01903/v1.3.2#");
            signedSignatureProperties.AppendChild("SigningTime", "http://uri.etsi.org/01903/v1.3.2#", DateTime.UtcNow.ToString(DateFormat, CultureInfo.InvariantCulture));
            var signingCertificate = signedSignatureProperties.AppendChild("SigningCertificate", "http://uri.etsi.org/01903/v1.3.2#");

            var cert = signingCertificate.AppendChild("Cert", "http://uri.etsi.org/01903/v1.3.2#");

            var certDigest = cert.AppendChild("CertDigest", "http://uri.etsi.org/01903/v1.3.2#");
            certDigest.AppendChild("DigestMethod", "http://www.w3.org/2000/09/xmldsig#").SetAttribute("Algorithm", "http://www.w3.org/2000/09/xmldsig#sha1");
            certDigest.AppendChild("DigestValue", "http://www.w3.org/2000/09/xmldsig#", System.Convert.ToBase64String(Certificate.GetCertHash()));

            var issuerSerial = cert.AppendChild("IssuerSerial", "http://uri.etsi.org/01903/v1.3.2#");
            issuerSerial.AppendChild("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#", Certificate.Issuer);
            issuerSerial.AppendChild("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#", BigInteger.Parse(Certificate.SerialNumber, NumberStyles.HexNumber).ToString());

            var signedDataObjectProperties = signedProperties.AppendChild("SignedDataObjectProperties", "http://uri.etsi.org/01903/v1.3.2#");
            foreach (var item in References)
            {
                var a = signedDataObjectProperties.AppendChild("DataObjectFormat", "http://uri.etsi.org/01903/v1.3.2#");
                a.SetAttribute("ObjectReference", "#" + item.Filnavn);
                a.AppendChild("MimeType", "http://uri.etsi.org/01903/v1.3.2#", item.Innholdstype);
            }

            return root.SelectNodes(".");
        }
    }
}
