using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Xml
{
    internal class QualifyingPropertiesObject : DataObject
    {
        private const string DateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffZ";

        public X509Certificate2 Certificate { get; private set; }

        public IQualifyingPropertiesReference[] References { get; private set; }

        /// <summary>
        /// The mandatory Target attribute refers to the XML signature in which the qualifying properties are associated.
        /// </summary>
        public string Target { get; private set; }

        /// <param name="certificate">The certificate used for signing.</param>
        /// <param name="target">The target attribute value of the QualifyingProperties element.</param>
        /// <param name="references">List of DataObjectFormat elements to be included.</param>
        /// <param name="context">The element where the signature will be placed. Used for extracting namespaces.</param>
        public QualifyingPropertiesObject(X509Certificate2 certificate, string target, IQualifyingPropertiesReference[] references, XmlElement context)
        {
            Certificate = certificate;
            Target = target;
            References = references;
            Data = CreateNodes(context);
        }

        private XmlNodeList CreateNodes(XmlElement context)
        {
            var doc = new XmlDocument(context.OwnerDocument.NameTable) {PreserveWhitespace = true};

            var root = doc.CreateElement("QualifyingProperties", "http://uri.etsi.org/01903/v1.3.2#");
            root.SetAttribute("Target", this.Target);

            /* The SignedXml class will calculate the signature before the data object is added to the main xml document. Because of this, the owner documents namespaces will not be 
             * propagated when the SignedProperties element is concatenated. 
             * This method copies in the namespaces to prevent this.
             */
            while (context != null)
            {
              /*  if (context.Prefix != null && !root.Attributes.OfType<XmlAttribute>().Any(a => a.Value == context.NamespaceURI && a.LocalName == context.Prefix))
                    root.SetAttribute("xmlns:" + context.Prefix, context.NamespaceURI);
                */
                foreach (XmlAttribute item in context.Attributes)
                {
                    if (item.Prefix == "xmlns")
                    {
                        var localName = item.LocalName;
                        var ns = item.Value;

                        if (!root.Attributes.OfType<XmlAttribute>().Any(a => a.Value == ns && a.LocalName == localName))
                            root.SetAttribute("xmlns:" + localName, ns);
                    }
                }

                context = context.ParentNode as XmlElement;
            }

            // Create Xml Node List
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
                a.SetAttribute("ObjectReference", "#" + item.Filename);
                a.AppendChild("MimeType", "http://uri.etsi.org/01903/v1.3.2#", item.Mimetype);
            }

            return root.SelectNodes(".");
        }        
    }    
}
