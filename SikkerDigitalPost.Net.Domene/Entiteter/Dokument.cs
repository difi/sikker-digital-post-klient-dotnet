using System.IO;
using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Dokument : IAsiceVedlegg
    {
        public string Tittel { get; private set; }
        public string Filnavn { get; private set; }
        public byte[] Bytes { get; private set; }
        public string MimeType { get; private set; }

        private Dokument(string tittel)
        {
            Tittel = tittel;
        }

        public Dokument(string tittel, string dokumentsti) : this(tittel)
        {
            Bytes = File.ReadAllBytes(dokumentsti);
        }

        public Dokument(string tittel, System.IO.StreamReader dokumentstrøm) : this(tittel)
        {
            Bytes = File.ReadAllBytes(dokumentstrøm.ReadToEnd());
        }

        public Dokument(string tittel, byte[] dokumentbytes) : this(tittel)
        {
            Bytes = dokumentbytes;
        }
    }
}
