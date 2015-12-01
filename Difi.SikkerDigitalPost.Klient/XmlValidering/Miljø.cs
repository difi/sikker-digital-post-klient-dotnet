using System;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø : AbstraktMiljø
    {
        
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
                    new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater())
                    );

            }
        }

        public static Miljø Produksjonsmiljø
        {
            get
            {
                return new Miljø(
                    new Uri("https://meldingsformidler.digipost.no/api/ebms"),
                    new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater())
                    );
            }
        }

        public static Miljø FunksjoneltTestmiljøNorskHelsenett
        {
            get
            {
                return new Miljø(
                    new Uri("https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/"),
                    new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater())
                    );

            }
        }

        public static Miljø ProduksjonsmiljøNorskHelsenett
        {
            get
            {
                return new Miljø(
                    new Uri("https://meldingsformidler.nhn.digipost.no:4444/api/"),
                    new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater())
                    );
            }
        }
    }
}
