using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    public static class AsiceGenerator
    {
        internal static DocumentBundle Create(Forsendelse forsendelse, GuidUtility guidUtility, X509Certificate2 senderCertificate, IAsiceConfiguration asiceConfiguration)
        {
            var manifest = new Manifest(forsendelse);
            ValidateXmlAndThrowIfInvalid(manifest.Xml(), "Manifest");

            var asiceAttachables = new List<IAsiceAttachable>();
            asiceAttachables.AddRange(forsendelse.Dokumentpakke.Vedlegg);
            asiceAttachables.Add(forsendelse.Dokumentpakke.Hoveddokument);
            asiceAttachables.Add(manifest);

            if (forsendelse.MetadataDocument != null)
            {
                var signature = new Signature(forsendelse, manifest, senderCertificate, forsendelse.MetadataDocument);
                ValidateXmlAndThrowIfInvalid(signature.Xml(), "Signatur");
                
                asiceAttachables.Add(forsendelse.MetadataDocument);
                asiceAttachables.Add(signature);
            }
            else
            {
                var signature = new Signature(forsendelse, manifest, senderCertificate);
                ValidateXmlAndThrowIfInvalid(signature.Xml(), "Signatur");
                
                asiceAttachables.Add(signature);
            }
            
            var asiceAttachableProcessors = ConvertDocumentBundleProcessorsToAsiceAttachableProcessors(forsendelse, asiceConfiguration);

            var asiceArchive = new AsiceArchive(forsendelse.PostInfo.Mottaker.Sertifikat, guidUtility, asiceAttachableProcessors, asiceAttachables.ToArray());
            
            return new DocumentBundle(asiceArchive.Bytes, asiceArchive.UnzippedContentBytesCount, asiceArchive.ContentId);
        }

        private static void ValidateXmlAndThrowIfInvalid(XmlDocument xmlDocument, string messagePrefix)
        {
            List<string> validationMessages;
            var isValid = SdpXmlValidator.Instance.Validate(xmlDocument.OuterXml, out validationMessages);
            if (!isValid)
            {
                throw new XmlValidationException($"{messagePrefix} er ikke gyldig.", validationMessages);
            }
        }

        private static IEnumerable<AsiceAttachableProcessor> ConvertDocumentBundleProcessorsToAsiceAttachableProcessors(Forsendelse forsendelseForMetadata, IAsiceConfiguration asiceConfiguration)
        {
            return asiceConfiguration.Dokumentpakkeprosessorer.Select(p => new AsiceAttachableProcessor(forsendelseForMetadata, p));
        }
    }
}