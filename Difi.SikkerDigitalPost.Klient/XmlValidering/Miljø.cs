using System;
using Difi.Felles.Utility;
using Difi.Felles.Utility.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø : AbstraktMiljø
    {
        
        private Miljø(Uri url, Sertifikatkjedevalidator sertifikatkjedevalidator)
        {
            Sertifikatkjedevalidator = sertifikatkjedevalidator;
            Url = url;
        }

        public static Miljø FunksjoneltTestmiljø => new Miljø(
            new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms"),
            new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater())
            );

        public static Miljø Produksjonsmiljø => new Miljø(
            new Uri("https://meldingsformidler.digipost.no/api/ebms"),
            new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater())
            );

        public static Miljø FunksjoneltTestmiljøNorskHelsenett => new Miljø(
            new Uri("https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/"),
            new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater())
            );

        public static Miljø ProduksjonsmiljøNorskHelsenett => new Miljø(
            new Uri("https://meldingsformidler.nhn.digipost.no:4444/api/"),
            new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater())
            );
    }
}
