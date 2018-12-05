using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class CertificateValidationProperties
    {
        public CertificateValidationProperties(X509Certificate2Collection allowedChainCertificates, Organisasjonsnummer organisasjonsnummerMeldingsformidler, bool validateResponse)
        {
            AllowedChainCertificates = allowedChainCertificates;
            OrganisasjonsnummerMeldingsformidler = organisasjonsnummerMeldingsformidler;
            ValidateResponse = validateResponse;
        }

        public X509Certificate2Collection AllowedChainCertificates { get; }

        public Organisasjonsnummer OrganisasjonsnummerMeldingsformidler { get; }

        public bool ValidateResponse { get; }
    }
}