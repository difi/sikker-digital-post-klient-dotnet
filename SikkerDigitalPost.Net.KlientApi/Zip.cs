using System;
using System.IO;
using System.IO.Compression;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi
{
    class Zip
    {
        private readonly Dokument _manifest;
        private readonly Dokument _signatur;
        private readonly Dokumentpakke _dokumentpakke;

        public Zip(Dokumentpakke dokumentpakke, Dokument signatur, Dokument manifest)
        {
            _signatur = signatur;
            _manifest = manifest;
            _dokumentpakke = dokumentpakke;
        }

        public ZipArchive CreateZip()
        {
            var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                LeggFilTilArkiv(archive, _manifest.Filnavn, _manifest.Bytes);
                LeggFilTilArkiv(archive, String.Format("META-INF/{0}",_signatur.Filnavn), _signatur.Bytes);
                
                foreach (var dokument in _dokumentpakke.Vedlegg)
                {
                    LeggFilTilArkiv(archive, dokument.Filnavn, dokument.Bytes);                    
                }
                
                return archive;
            }
        }

        private static void LeggFilTilArkiv(ZipArchive archive, string filename, byte[] data)
        {
            LeggFilTilArkiv(archive, filename, new MemoryStream(data));
        }

        private static void LeggFilTilArkiv(ZipArchive archive, string filename, Stream stream)
        {
            var entry = archive.CreateEntry(filename, CompressionLevel.Optimal);
            using (Stream s = entry.Open())
            {
                stream.CopyTo(s);
                s.Close();
            }
        }

        
        


    }
}
