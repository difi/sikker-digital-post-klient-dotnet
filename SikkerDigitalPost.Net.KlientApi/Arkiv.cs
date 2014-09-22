using System.IO;
using System.IO.Compression;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur;

namespace SikkerDigitalPost.Net.KlientApi
{
    public class Arkiv
    {
        public readonly Manifest Manifest;
        public readonly Signatur Signatur;
        private readonly Dokumentpakke _dokumentpakke;

        public Arkiv(Dokumentpakke dokumentpakke, Signatur signatur, Manifest manifest)
        {
            Signatur = signatur;
            Manifest = manifest;
            _dokumentpakke = dokumentpakke;
        }

        public byte[] LagArkiv()
        {
            var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
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
    }
}
