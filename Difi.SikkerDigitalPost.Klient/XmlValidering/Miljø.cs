using System;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø
    {
        public Uri Url { get; set; }

        internal Sertifikatkjedevalidator Sertifikatkjedevalidator { get; set; }

        private Miljø(Uri url, Sertifikatkjedevalidator sertifikatkjedevalidator)
        {
            Sertifikatkjedevalidator = sertifikatkjedevalidator;
            Url = url;
        }

        public static Miljø FunksjoneltTestmiljø
        {
            get
            {
                return new Miljø(
                    new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms"),
                    new SertifikatkjedevalidatorFunksjoneltTestmiljø(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater())
                    );

            }
        }

        public static Miljø Produksjonsmiljø
        {
            get
            {
                return new Miljø(
                    new Uri("https://meldingsformidler.digipost.no/api/ebms"),
                    new SertifikatkjedevalidatorProduksjon(SertifikatkjedeUtility.ProduksjonsSertifikater())
                    );
            }
        }

        public static Miljø FunksjoneltTestmiljøNorskHelsenett
        {
            get
            {
                return new Miljø(
                    new Uri("https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/"),
                    new SertifikatkjedevalidatorFunksjoneltTestmiljø(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater())
                    );

            }
        }

        public static Miljø ProduksjonsmiljøNorskHelsenett
        {
            get
            {
                return new Miljø(
                    new Uri("https://meldingsformidler.nhn.digipost.no:4444/api/"),
                    new SertifikatkjedevalidatorProduksjon(SertifikatkjedeUtility.ProduksjonsSertifikater())
                    );
            }
        }
    }
}
