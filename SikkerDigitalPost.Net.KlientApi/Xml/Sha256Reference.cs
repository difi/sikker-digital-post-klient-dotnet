using System.IO;

namespace SikkerDigitalPost.Net.KlientApi.Xml
{
    /// <summary>
    /// Represents the &lt;reference&gt; element of an Xml Signature with http://www.w3.org/2001/04/xmlenc#sha256 as the digest method.
    /// </summary>
    internal class Sha256Reference : System.Security.Cryptography.Xml.Reference
    {
        public Sha256Reference()
        {
            DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }

        public Sha256Reference(Stream stream): base(stream)
        {
            DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }

        public Sha256Reference(string uri): base(uri)
        {
            DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }

        public Sha256Reference(byte[] bytes): base(new MemoryStream(bytes))
        {
            DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }
    }
}
