using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.XmlValidering;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    public static class AsiceGenerator
    {
        internal static DocumentBundle Create(Forsendelse forsendelse, GuidUtility guidUtility, X509Certificate2 senderCertificate, IAsiceConfiguration asiceConfiguration)
        {
            var manifest = new Manifest(forsendelse);
            SdpXmlValidator.Validate(manifest.Xml(), "Manifest");

            var signature = new Signature(forsendelse, manifest, senderCertificate);
            SdpXmlValidator.Validate(signature.Xml(), "Signatur");

            var asiceAttachables = new List<IAsiceAttachable>();
            asiceAttachables.AddRange(forsendelse.Dokumentpakke.Vedlegg);
            asiceAttachables.Add(forsendelse.Dokumentpakke.Hoveddokument);
            asiceAttachables.AddRange(forsendelse.Dokumentpakke.DataDokumenter);
            asiceAttachables.Add(manifest);
            asiceAttachables.Add(signature);

            var asiceAttachableProcessors = ConvertDocumentBundleProcessorsToAsiceAttachableProcessors(forsendelse, asiceConfiguration);

            var asiceArchive = new AsiceArchive(forsendelse.PostInfo.Mottaker.Sertifikat, guidUtility, asiceAttachableProcessors, asiceAttachables.ToArray());

            return new DocumentBundle(asiceArchive.Bytes, asiceArchive.UnzippedContentBytesCount, asiceArchive.ContentId);
        }

        private static IEnumerable<AsiceAttachableProcessor> ConvertDocumentBundleProcessorsToAsiceAttachableProcessors(Forsendelse forsendelseForMetadata, IAsiceConfiguration asiceConfiguration)
        {
            return asiceConfiguration.Dokumentpakkeprosessorer.Select(p => new AsiceAttachableProcessor(forsendelseForMetadata, p));
        }
    }
}