using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører
{
    public class Databehandler
    {

        public Databehandler(Organisasjonsnummer organisasjonsnummer)
        {
            Organisasjonsnummer = organisasjonsnummer;
        }

        /// <param name="organisasjonsnummer">Organisasjonsnummeret til avsender av brevet.</param>
        /// <param name="sertifikat">Avsenders Sertifikat: Virksomhetssertifikat.</param>
        [Obsolete]
        public Databehandler(Organisasjonsnummer organisasjonsnummer, X509Certificate2 sertifikat) : this(organisasjonsnummer)
        {
            Sertifikat = sertifikat;
        }

        /// <param name="organisasjonsnummer">Organisasjonsnummeret til avsender av brevet.</param>
        /// <param name="sertifikatThumbprint">
        ///     Thumbprint til databehandlersertifikatet. Se guide på
        ///     http://difi.github.io/sikker-digital-post-klient-dotnet/#databehandlersertifikat
        /// </param>
        [Obsolete]
        public Databehandler(Organisasjonsnummer organisasjonsnummer, string sertifikatThumbprint) : this(organisasjonsnummer)
        {
            Sertifikat = CertificateUtility.SenderCertificate(sertifikatThumbprint);
        }

        /// <summary>
        ///     Organisasjonsnummeret til avsender av brevet.
        /// </summary>
        public Organisasjonsnummer Organisasjonsnummer { get; private set; }

        /// <summary>
        ///     Avsenders sertifikat: Virksomhetssertifikat.
        /// </summary>
        [Obsolete]
        public X509Certificate2 Sertifikat { get; }
    }
}
