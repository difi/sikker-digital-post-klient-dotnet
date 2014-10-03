using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Extensions;
using SikkerDigitalPost.Net.KlientApi.Envelope;

namespace SikkerDigitalPost.Net.KlientApi.Xml
{
    internal class QualifyingPropertiesObject : DataObject
    {
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

            var root = doc.CreateElement("QualifyingProperties", Navnerom.Ns11);
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
            var signedProperties = root.AppendChild("SignedProperties", Navnerom.Ns11);
            signedProperties.SetAttribute("Id", "SignedProperties");

            var signedSignatureProperties = signedProperties.AppendChild("SignedSignatureProperties", Navnerom.Ns11);
            signedSignatureProperties.AppendChild("SigningTime", Navnerom.Ns11, DateTime.UtcNow.ToString(DateUtility.DateFormat, CultureInfo.InvariantCulture));
            var signingCertificate = signedSignatureProperties.AppendChild("SigningCertificate", Navnerom.Ns11);

            var cert = signingCertificate.AppendChild("Cert", Navnerom.Ns11);

            var certDigest = cert.AppendChild("CertDigest", Navnerom.Ns11);
            certDigest.AppendChild("DigestMethod", Navnerom.Ns5).SetAttribute("Algorithm", "http://www.w3.org/2000/09/xmldsig#sha1");
            certDigest.AppendChild("DigestValue", Navnerom.Ns5, Convert.ToBase64String(Certificate.GetCertHash()));

            var issuerSerial = cert.AppendChild("IssuerSerial", Navnerom.Ns11);
            issuerSerial.AppendChild("X509IssuerName", Navnerom.Ns5, Certificate.Issuer);
            issuerSerial.AppendChild("X509SerialNumber", Navnerom.Ns5, BigInteger.Parse(Certificate.SerialNumber, NumberStyles.HexNumber).ToString());

            var signedDataObjectProperties = signedProperties.AppendChild("SignedDataObjectProperties", Navnerom.Ns11);
            foreach (var item in References)
            {
                var a = signedDataObjectProperties.AppendChild("DataObjectFormat", Navnerom.Ns11);
                a.SetAttribute("ObjectReference", "#" + item.Filename);
                a.AppendChild("MimeType", Navnerom.Ns11, item.Mimetype);
            }

            return root.SelectNodes(".");
        }        
    }    
}
