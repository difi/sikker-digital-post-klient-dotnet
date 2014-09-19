using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur
{
    public class Signatur : IAsiceVedlegg
    {
        private readonly byte[] _bytes;

        public Signatur(byte[] bytes)
        {
            _bytes = bytes;
        }
        
        public string Filnavn {
            get { return "META-INF/signatures.xml"; } 
        }
        
        public byte[] Bytes {
            get { return _bytes; }
        }
        
        public string MimeType {
            get { return "application/xml"; }
        }
    }
}
