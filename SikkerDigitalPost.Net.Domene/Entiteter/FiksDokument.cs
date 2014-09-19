using System.IO;
using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Dokument : IAsiceVedlegg
    {
        public string Tittel { get; private set; }
        public string Filsti { get; private set; }
        public byte[] Bytes { get; private set; }
        public string Innholdstype { get; private set; }

        public Dokument(string tittel, string dokumentsti, string innholdstype, string filnavn = null)
        {
            Filsti = filnavn == null ? Path.GetFileName(dokumentsti) : null;
            SettOppDokument(tittel, File.ReadAllBytes(dokumentsti), innholdstype, Filsti);
        }

        public Dokument(string tittel, System.IO.StreamReader dokumentstrøm, string innholdstype, string filnavn = null)
        {
            SettOppDokument(tittel,File.ReadAllBytes(dokumentstrøm.ReadToEnd()),innholdstype,filnavn);
        }

        public Dokument(string tittel, byte[] dokumentbytes, string innholdstype, string filnavn = null)
        {
            SettOppDokument(tittel, dokumentbytes, innholdstype,filnavn);
        }

        private void SettOppDokument(string tittel, byte[] bytes, string innholdstype, string filnavn = null)
        {
            Tittel = tittel;
            Bytes = bytes;
            Innholdstype = innholdstype;
            Filsti = filnavn == null ? Path.GetRandomFileName() : filnavn;
        }
    }
}
