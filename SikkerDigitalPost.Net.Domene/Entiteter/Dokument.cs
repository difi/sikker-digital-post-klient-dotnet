using System.IO;
using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Dokument : IAsiceVedlegg
    {
        public string Tittel { get; private set; }
        /// <summary>
        /// Filnavn
        /// </summary>
        public string Filnavn { get; set; }
        public byte[] Bytes { get; private set; }
        public string Innholdstype { get; private set; }

        public Dokument(string tittel, string dokumentsti, string innholdstype, string filnavn = null)
            : this(tittel, File.ReadAllBytes(dokumentsti), innholdstype, filnavn ?? Path.GetFileName(dokumentsti))
        {
        }

        public Dokument(string tittel, Stream dokumentstrøm, string innholdstype, string filnavn = null)
            : this(tittel, File.ReadAllBytes(new StreamReader(dokumentstrøm).ReadToEnd()), innholdstype, filnavn)
        {
        }

        public Dokument(string tittel, byte[] dokumentbytes, string innholdstype, string filnavn = null)
        {
            Tittel = tittel;
            Bytes = dokumentbytes;
            Innholdstype = innholdstype;
            Filnavn = filnavn ?? Path.GetRandomFileName();
        }
    }
}
