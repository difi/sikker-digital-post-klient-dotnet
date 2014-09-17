using System.IO;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Dokument : IAsiceVedlegg
    {
        public string Filnavn { get; private set; }
        public byte[] Bytes { get; private set; }
        public string MimeType { get; private set; }
        public string Tittel { get; private set; }
        
        public Dokument(string tittel, string dokumentsti) //IOStream, Bytearray
        {
            Tittel = tittel;
            Bytes = File.ReadAllBytes(dokumentsti);
        }
    }
}
