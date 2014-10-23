using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;
using System.Diagnostics;

namespace SikkerDigitalPost.Klient.AsicE
{
    internal class AsicEArkiv : ISoapVedlegg
    {
        public Manifest Manifest { get; private set; }
        public Signatur Signatur { get; private set; }
        private readonly Dokumentpakke _dokumentpakke;
        private readonly Forsendelse _forsendelse;

        private byte[] _bytes;
        private byte[] _ukrypterteBytes;

        private readonly GuidHandler _guidHandler;


        public AsicEArkiv(Forsendelse forsendelse, GuidHandler guidHandler, X509Certificate2 avsenderSertifikat)
        {
            Manifest = new Manifest(forsendelse);
            Signatur = new Signatur(forsendelse, Manifest, avsenderSertifikat);

            _forsendelse = forsendelse;
            _dokumentpakke = _forsendelse.Dokumentpakke;
            _guidHandler = guidHandler;
        }

        private X509Certificate2 _krypteringssertifikat 
        {
            get { return _forsendelse.DigitalPost.Mottaker.Sertifikat; }
        }
        
        public string Filnavn
        {
            get { return "post.asice.zip"; }
        }

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

        private void LeggFilTilArkiv(ZipArchive archive, string filename, byte[] data)
        {
            Logging.Log(TraceEventType.Verbose, Manifest.Forsendelse.KonversasjonsId, string.Format("Legger til '{0}' på {1} bytes til dokumentpakke.", filename, data.Length));

            var entry = archive.CreateEntry(filename, CompressionLevel.Optimal);
            using (Stream s = entry.Open())
            {
                s.Write(data, 0, data.Length);
                s.Close();
            }
        }

        private byte[] KrypterteBytes(byte[] bytes)
        {
            Logging.Log(TraceEventType.Verbose, Manifest.Forsendelse.KonversasjonsId, "Krypterer dokumentpakke med sertifikat " + _krypteringssertifikat.Thumbprint);

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
