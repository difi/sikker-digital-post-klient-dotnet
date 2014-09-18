using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest
{
    public class Manifest : IAsiceVedlegg
    {
        public string Filnavn { get; private set; }
        public byte[] Bytes { get; private set; }
        public string MimeType { get; private set; }
        
        public Manifest(string mimeType, byte[] bytes, string filnavn)
        {
            Filnavn = filnavn;
            Bytes = bytes;
            MimeType = mimeType;
        }
    }
}
