using System;
using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal abstract class Sertifikatvalidator
    {
        public static X509ChainStatus[] ValiderResponssertifikat(X509Certificate2 sertifikat)
        {
            throw new NotImplementedException();
        }
    }
}
