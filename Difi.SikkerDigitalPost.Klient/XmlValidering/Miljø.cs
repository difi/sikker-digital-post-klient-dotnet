using System;
using Difi.Felles.Utility;
using Difi.Felles.Utility.Utilities;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø : AbstractEnvironment
    {
        private Miljø(Uri url, CertificateChainValidator certificateChainValidator)
        {
            CertificateChainValidator = certificateChainValidator;
            Url = url;
        }

        internal Uri UrlWithOrganisasjonsnummer(Organisasjonsnummer databehandler, Organisasjonsnummer avsender)
        {
            return new Uri(Url,$"{databehandler.WithCountryCode}/{avsender.WithCountryCode}");
        }

        public static Miljø FunksjoneltTestmiljø => new Miljø(
            new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms"),
            new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater())
            );

        public static Miljø Produksjonsmiljø => new Miljø(
            new Uri("https://meldingsformidler.digipost.no/api/ebms"),
            new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater())
            );

        public static Miljø FunksjoneltTestmiljøNorskHelsenett => new Miljø(
            new Uri("https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/"),
            new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater())
            );

        public static Miljø ProduksjonsmiljøNorskHelsenett => new Miljø(
            new Uri("https://meldingsformidler.nhn.digipost.no:4444/api/"),
            new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater())
            );
    }
}