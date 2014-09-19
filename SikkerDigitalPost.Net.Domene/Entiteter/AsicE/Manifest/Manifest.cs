using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest
{
    public class Manifest : IAsiceVedlegg
    {
        private readonly byte[] _bytes;
        
        public Manifest(byte[] bytes)
        {
            _bytes = bytes;
        }

        public string Filnavn {
            get { return "manifest.xml"; }
        }
        public byte[] Bytes {
            get { return _bytes; }
        }
        public string MimeType { get; private set; }
        
       
    }
}
