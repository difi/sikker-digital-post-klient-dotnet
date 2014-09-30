using System;
using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class TekniskAvsender
    {
        /// <summary>
        /// Organisasjonsnummeret til avsender av brevet.
        /// </summary>
        public Organisasjonsnummer Organisasjonsnummer { get; set; }

        /// <summary>
        /// Avsenders sertifikat: Virksomhetssertifikat.
        /// </summary>
        public X509Certificate2 Sertifikat { get; set; }

        /// <param name="organisasjonsnummer">Organisasjonsnummeret til avsender av brevet.</param>
        /// <param name="sertifikat">Avsenders Sertifikat: Virksomhetssertifikat.</param>
        public TekniskAvsender(Organisasjonsnummer organisasjonsnummer, X509Certificate2 sertifikat)
        {
            Organisasjonsnummer = organisasjonsnummer;
            Sertifikat = sertifikat;
        }

        public TekniskAvsender(String organisasjonsnummer, X509Certificate2 sertifikat): 
            this(new Organisasjonsnummer(organisasjonsnummer), sertifikat )
        {
        }
    }
}
