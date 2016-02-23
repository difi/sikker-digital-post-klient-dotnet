using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.AsicE
{
    internal class AsicEArkiv : ISoapVedlegg
    {
        private readonly Dokumentpakke _dokumentpakke;
        private readonly Forsendelse _forsendelse;

        private readonly GuidUtility _guidHandler;

        private byte[] _bytes;
        private byte[] _ukrypterteBytes;


        public AsicEArkiv(Forsendelse forsendelse, GuidUtility guidHandler, X509Certificate2 avsenderSertifikat)
        {
            Manifest = new Manifest(forsendelse);
            Signatur = new Signatur(forsendelse, Manifest, avsenderSertifikat);

            _forsendelse = forsendelse;
            _dokumentpakke = _forsendelse.Dokumentpakke;
            _guidHandler = guidHandler;
        }

        public Manifest Manifest { get; set; }
        public Signatur Signatur { get; set; }

        private X509Certificate2 Krypteringssertifikat => _forsendelse.PostInfo.Mottaker.Sertifikat;

        internal byte[] UkrypterteBytes
        {
            get
            {
                if (_ukrypterteBytes != null)
                    return _ukrypterteBytes;

                _ukrypterteBytes = LagBytes();
                return _ukrypterteBytes;
            }
        }

        public string Filnavn => "post.asice.zip";

        public byte[] Bytes
        {
            get
            {
                if (_bytes != null)
                    return _bytes;

                _bytes = KrypterteBytes(UkrypterteBytes);
                return _bytes;
            }
        }

        public string Innholdstype => "application/cms";

        public string ContentId => _guidHandler.DokumentpakkeId;

        public string TransferEncoding => "binary";

        private byte[] LagBytes()
        {
            var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                LeggFilTilArkiv(archive, _dokumentpakke.Hoveddokument.FilnavnRådata, _dokumentpakke.Hoveddokument.Bytes);
                LeggFilTilArkiv(archive, Manifest.Filnavn, Manifest.Bytes);
                LeggFilTilArkiv(archive, Signatur.Filnavn, Signatur.Bytes);

                foreach (var dokument in _dokumentpakke.Vedlegg)
                    LeggFilTilArkiv(archive, dokument.FilnavnRådata, dokument.Bytes);
            }

            return stream.ToArray();
        }

        public void LagreTilDisk(params string[] filsti)
        {
            FileUtility.WriteToBasePath(UkrypterteBytes, filsti);
        }

        private void LeggFilTilArkiv(ZipArchive archive, string filename, byte[] data)
        {
            Logging.Log(TraceEventType.Information, Manifest.Forsendelse.KonversasjonsId,
                $"Legger til '{filename}' på {data.Length} bytes til dokumentpakke.");

            var entry = archive.CreateEntry(filename, CompressionLevel.Optimal);
            using (var stream = entry.Open())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        private byte[] KrypterteBytes(byte[] bytes)
        {
            Logging.Log(TraceEventType.Information, Manifest.Forsendelse.KonversasjonsId,
                $"Krypterer dokumentpakke med sertifikat {Krypteringssertifikat.Thumbprint}.");

            var contentInfo = new ContentInfo(bytes);
            var encryptAlgoOid = new Oid("2.16.840.1.101.3.4.1.42"); // AES-256-CBC            
            var envelopedCms = new EnvelopedCms(contentInfo, new AlgorithmIdentifier(encryptAlgoOid));
            var recipient = new CmsRecipient(Krypteringssertifikat);
            envelopedCms.Encrypt(recipient);
            return envelopedCms.Encode();
        }

        public static byte[] Dekrypter(byte[] kryptertData)
        {
            var envelopedCms = new EnvelopedCms();
            envelopedCms.Decode(kryptertData);
            envelopedCms.Decrypt(envelopedCms.RecipientInfos[0]);
            return envelopedCms.ContentInfo.Content;
        }
    }
}