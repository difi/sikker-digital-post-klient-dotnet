using System;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø
    {

        public Uri Url { get; set; }

        internal Sertifikatvalidator Sertifikatvalidator { get; set; }

        private Miljø(Uri url, Sertifikatvalidator sertifikatvalidator)
        {
            Sertifikatvalidator = sertifikatvalidator;
            Url = url;
        }

        public static Miljø Test
        {
            get
            {
                return new Miljø(
                    new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms"),
                    new SertifikatValidatorTest(SertifikatUtility.TestSertifikater())
                    );

            }
        }

        public static Miljø Produksjon
        {
            get
            {
                return new Miljø(
                    new Uri("https://meldingsformidler.digipost.no/api/ebms"),
                    new SertifikatValidatorProduksjon(SertifikatUtility.ProduksjonsSertifikater())
                    );
            }
        }
    }
}
