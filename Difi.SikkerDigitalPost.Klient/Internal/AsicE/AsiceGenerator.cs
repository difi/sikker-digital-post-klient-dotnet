using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    public static class AsiceGenerator
    {
        internal static DocumentBundle Create(Forsendelse message, GuidUtility guidUtility, X509Certificate2 senderCertificate)
        {
            var manifest = new Manifest(message);
            ValidateXmlAndThrowIfInvalid(new ManifestValidator(), manifest.Xml(), "Manifest");

            var signature = new Signature(message, manifest, senderCertificate);
            ValidateXmlAndThrowIfInvalid(new SignatureValidator(), signature.Xml(), "Signatur");

            var asiceAttachables = new List<IAsiceAttachable>();
            asiceAttachables.AddRange(message.Dokumentpakke.Vedlegg);
            asiceAttachables.Add(message.Dokumentpakke.Hoveddokument);
            asiceAttachables.Add(manifest);
            asiceAttachables.Add(signature);

            var asiceArchive = new AsiceArchive(message.PostInfo.Mottaker.Sertifikat, guidUtility, asiceAttachables.ToArray());

            //if (!string.IsNullOrEmpty(standardLogPath))
            //{
            //    asiceArchive.SaveToFile(standardLogPath, "dokumentpakke", DateUtility.DateForFile() + " - Dokumentpakke.zip");
            //}

            return new DocumentBundle(asiceArchive.Bytes, asiceArchive.UnzippedContentBytesCount, asiceArchive.ContentId);
        }

        private static void ValidateXmlAndThrowIfInvalid(XmlValidator xmlValidator, XmlDocument xmlDocument, string messagePrefix)
        {
            var isValid = xmlValidator.Validate(xmlDocument.OuterXml);
            if (!isValid)
            {
                throw new XmlValidationException($"{messagePrefix} er ikke gyldig: {xmlValidator.ValidationWarnings}");
            }
        }
    }
}