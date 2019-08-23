using System;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility.Utilities;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø
    {
        private Miljø(Uri url, X509Certificate2Collection godkjenteKjedeSertifikater)
        {
            GodkjenteKjedeSertifikater = godkjenteKjedeSertifikater;
            Url = url;
        }

        public Uri Url { get; set; }

        public X509Certificate2Collection GodkjenteKjedeSertifikater { get; set; }

        internal Uri UrlWithOrganisasjonsnummer(Organisasjonsnummer databehandler, Organisasjonsnummer avsender)
        {
            return new Uri(Url, $"{databehandler.WithCountryCode}/{avsender.WithCountryCode}");
        }

        public static Miljø FunksjoneltTestmiljø => new Miljø(
            new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/"),
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater()
            );

        public static Miljø Ytelsestestmiljø => new Miljø(
            new Uri("https://qa.meldingsformidler.digipost.no/api/"),
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater()
            );

        public static Miljø Produksjonsmiljø => new Miljø(
            new Uri("https://meldingsformidler.digipost.no/api/"),
            CertificateChainUtility.ProduksjonsSertifikater()
            );

        public static Miljø FunksjoneltTestmiljøNorskHelsenett => new Miljø(
            new Uri("https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/"),
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater()
            );

        public static Miljø ProduksjonsmiljøNorskHelsenett => new Miljø(
            new Uri("https://meldingsformidler.nhn.digipost.no:4444/api/"),
            CertificateChainUtility.ProduksjonsSertifikater()
            );

        public static  Miljø LokalMFMiljø => new Miljø(
            new Uri("http://127.0.0.1:8049/"),
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater()
            );
        
        internal static Miljø Localhost => new Miljø(
            new Uri("http://192.168.36.1:8049"),
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater()
            );
    }
}