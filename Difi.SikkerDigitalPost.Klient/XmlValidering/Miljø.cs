using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø
    {
        public X509Certificate2Collection Sertifikatlager { get; set; }

        public Uri Url { get; set; }

        internal Sertifikatvalidator Sertifikatvalidator { get; set; }

        private Miljø(X509Certificate2Collection sertifikatlager, Uri url)
        {
            Sertifikatlager = sertifikatlager;
            Url = url;
        }

        public static Miljø Produksjon
        {
            get
            {
                return new Miljø(new X509Certificate2Collection(), new Uri("https://meldingsformidler.digipost.no/api/ebms"));
            }
        }

        public static Miljø Test
        {
            get
            {
                return new Miljø(SertifikatUtility.TestSertifikater(), new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms"));
            }
        }
    }
}
