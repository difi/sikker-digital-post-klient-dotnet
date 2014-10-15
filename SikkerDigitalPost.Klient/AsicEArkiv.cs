using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.AsicE;

namespace SikkerDigitalPost.Klient
{
    internal class AsicEArkiv : ISoapVedlegg
    {
        public readonly Manifest Manifest;
        public readonly Signatur Signatur;
        private readonly Dokumentpakke _dokumentpakke;

        private byte[] _bytes;
        private readonly X509Certificate2 _krypteringssertifikat;
        private readonly GuidHandler _guidHandler;


        public AsicEArkiv(Dokumentpakke dokumentpakke, Signatur signatur, Manifest manifest, X509Certificate2 krypteringssertifikat, GuidHandler guidHandler)
        {
            Signatur = signatur;
            Manifest = manifest;
            _dokumentpakke = dokumentpakke;
            _krypteringssertifikat = krypteringssertifikat;
            _guidHandler = guidHandler;
        }


        public string Filnavn
        {
            get { return "post.asice.zip"; }
        }


        public byte[] Bytes
        {
            get
            {
                if (_bytes != null)
                    return _bytes;

                _bytes = KrypterteBytes(LagBytes());
                return _bytes;
            }
        }
        
        public string Innholdstype
        {
            get { return "application/cms"; }
        }

        public string ContentId
        {
            get { return _guidHandler.DokumentpakkeId; }
        }

        public string TransferEncoding
        {
            get { return "binary"; }
        }

        private byte[] LagBytes()
        {
            var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                LeggFilTilArkiv(archive, _dokumentpakke.Hoveddokument.Filnavn, _dokumentpakke.Hoveddokument.Bytes);
                LeggFilTilArkiv(archive, Manifest.Filnavn, Manifest.Bytes);
                LeggFilTilArkiv(archive, Signatur.Filnavn, Signatur.Bytes);

                foreach (var dokument in _dokumentpakke.Vedlegg)
                    LeggFilTilArkiv(archive, dokument.Filnavn, dokument.Bytes);

            }
            return stream.ToArray();
        }

        private static void LeggFilTilArkiv(ZipArchive archive, string filename, byte[] data)
        {
            var entry = archive.CreateEntry(filename, CompressionLevel.Optimal);
            using (Stream s = entry.Open())
            {
                s.Write(data, 0, data.Length);
                s.Close();
            }
        }

        private byte[] KrypterteBytes(byte[] bytes)
        {
            var contentInfo = new ContentInfo(bytes);
            var encryptAlgoOid = new Oid("2.16.840.1.101.3.4.1.42"); // AES-256-CBC            
            var envelopedCms = new EnvelopedCms(contentInfo, new AlgorithmIdentifier(encryptAlgoOid));
            var recipient = new CmsRecipient(_krypteringssertifikat);
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
