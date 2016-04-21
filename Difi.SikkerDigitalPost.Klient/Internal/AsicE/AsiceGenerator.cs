using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    public static class AsiceGenerator
    {
        internal static DocumentBundle Create(Forsendelse message, GuidUtility guidUtility, X509Certificate2 senderCertificate, string standardLogPath = "")
        {
            var manifest = new Manifest(message);
            ValidateXmlAndThrowIfInvalid(new ManifestValidator(), manifest.Xml(), messagePrefix: "Manifest");

            var signature = new Signature(message, manifest, senderCertificate);
            ValidateXmlAndThrowIfInvalid(new SignatureValidator(), signature.Xml(), messagePrefix: "Signatur");

            var asiceArchive = new AsiceArchive(message, manifest, signature, guidUtility);
            
            if (!string.IsNullOrEmpty(standardLogPath))
            {
                asiceArchive.LagreTilDisk(standardLogPath, "dokumentpakke", DateUtility.DateForFile() + " - Dokumentpakke.zip");
            }
            
            return new DocumentBundle(asiceArchive.Bytes, asiceArchive.UnzippedContentBytesCount, asiceArchive.ContentId);
        }

        private static void ValidateXmlAndThrowIfInvalid(XmlValidator xmlValidator, XmlDocument xmlDocument, string messagePrefix)
        {
            var isValid = xmlValidator.ValiderDokumentMotXsd(xmlDocument.OuterXml);
            if (!isValid)
            {
                throw new XmlValidationException($"{messagePrefix} er ikke gyldig: {xmlValidator.ValideringsVarsler}");
            }
        }
    }
}
