using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører
{
    public class Databehandler
    {
        /// <param name="organisasjonsnummer">Organisasjonsnummeret til avsender av brevet.</param>
        /// <param name="sertifikat">Avsenders Sertifikat: Virksomhetssertifikat.</param>
        public Databehandler(Organisasjonsnummer organisasjonsnummer, X509Certificate2 sertifikat)
        {
            Organisasjonsnummer = organisasjonsnummer;
            Sertifikat = sertifikat;
        }

        /// <param name="organisasjonsnummer">Organisasjonsnummeret til avsender av brevet.</param>
        /// <param name="sertifikatThumbprint">
        ///     Thumbprint til databehandlersertifikatet. Se guide på
        ///     http://difi.github.io/sikker-digital-post-klient-dotnet/#databehandlersertifikat
        /// </param>
        public Databehandler(Organisasjonsnummer organisasjonsnummer, string sertifikatThumbprint)
        {
            Organisasjonsnummer = organisasjonsnummer;
            Sertifikat = CertificateUtility.SenderCertificate(sertifikatThumbprint);
        }

        /// <summary>
        ///     Organisasjonsnummeret til avsender av brevet.
        /// </summary>
        public Organisasjonsnummer Organisasjonsnummer { get; private set; }

        /// <summary>
        ///     Avsenders sertifikat: Virksomhetssertifikat.
        /// </summary>
        public X509Certificate2 Sertifikat { get; private set; }
    }
}