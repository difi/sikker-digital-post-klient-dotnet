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

        public static Miljø FunksjoneltTestmiljø
        {
            get
            {
                return new Miljø(
                    new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms"),
                    new SertifikatValidatorFunksjoneltTestmiljø(SertifikatUtility.TestSertifikater())
                    );

            }
        }

        public static Miljø Produksjonsmiljø
        {
            get
            {
                return new Miljø(
                    new Uri("https://meldingsformidler.digipost.no/api/ebms"),
                    new SertifikatValidatorProduksjon(SertifikatUtility.ProduksjonsSertifikater())
                    );
            }
        }

        public static Miljø FunksjoneltTestmiljøNorskHelsenett
        {
            get
            {
                return new Miljø(
                    new Uri("https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/"),
                    new SertifikatValidatorFunksjoneltTestmiljø(SertifikatUtility.TestSertifikater())
                    );

            }
        }

        public static Miljø ProduksjonsmiljøNorskHelsenett
        {
            get
            {
                return new Miljø(
                    new Uri("https://meldingsformidler.nhn.digipost.no:4444/api/"),
                    new SertifikatValidatorProduksjon(SertifikatUtility.ProduksjonsSertifikater())
                    );
            }
        }
    }
}
