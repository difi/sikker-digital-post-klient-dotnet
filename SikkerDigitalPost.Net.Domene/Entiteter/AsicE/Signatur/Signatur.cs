using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur
{
    public class Signatur : IAsiceVedlegg
    {
        private readonly byte[] _xmlBytes;

        public Signatur(byte[] xmlBytes)
        {
            _xmlBytes = xmlBytes;
        }
        
        public string Filnavn {
            get { return "META-INF/signatures.xml"; } 
        }
        
        public byte[] Bytes {
            get { return _xmlBytes; }
        }
        
        public string MimeType {
            get { return "application/xml"; }
        }
    }
}
