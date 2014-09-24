using System.IO;

namespace SikkerDigitalPost.Net.Klient.Xml
{
    /// <summary>
    /// Represents the &lt;reference&gt; element of an Xml Signature with http://www.w3.org/2001/04/xmlenc#sha256 as the digest method.
    /// </summary>
    internal class Sha256Reference : System.Security.Cryptography.Xml.Reference
    {
          public Sha256Reference()
            : base()
        {
            this.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }

        public Sha256Reference(Stream stream)
            : base(stream)
        {
            this.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }

        public Sha256Reference(string uri)
            : base(uri)
        {
            this.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }

        public Sha256Reference(byte[] bytes) : base(new System.IO.MemoryStream(bytes))
        {
            this.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }
    }
}
