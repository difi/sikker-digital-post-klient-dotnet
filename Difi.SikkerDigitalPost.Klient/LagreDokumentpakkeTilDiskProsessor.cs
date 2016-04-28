using System;
using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient
{
    public class LagreDokumentpakkeTilDiskProsessor : IDokumentpakkeProsessor
    {
        public LagreDokumentpakkeTilDiskProsessor(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; }

        public string LastFileProcessed { get; set; }

        public void Prosesser(Forsendelse forsendelse, Stream bundleStream)
        {
            LastFileProcessed = FileNameWithTimeStamp(forsendelse.KonversasjonsId.ToString());
            using (var fileStream = File.Create(Path.Combine(Directory, LastFileProcessed)))
            {
                bundleStream.CopyTo(fileStream);
            }
        }

        private static string FileNameWithTimeStamp(string reference)
        {
            return $"{DateTime.Now.ToString(DateUtility.DateForFile())} - {reference}.asice.zip";
        }
    }
}