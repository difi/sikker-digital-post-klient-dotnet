using System;
using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Domene.Entiteter.Aktører
{
    public class Databehandler
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
        public Databehandler(Organisasjonsnummer organisasjonsnummer, X509Certificate2 sertifikat)
        {
            Organisasjonsnummer = organisasjonsnummer;
            Sertifikat = sertifikat;
        }

        public Databehandler(String organisasjonsnummer, X509Certificate2 sertifikat): 
            this(new Organisasjonsnummer(organisasjonsnummer), sertifikat )
        {
        }
    }
}
