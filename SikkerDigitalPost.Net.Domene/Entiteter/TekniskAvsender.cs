namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class TekniskAvsender
    {
        /// <summary>
        /// Organisasjonsnummeret til avsender av brevet.
        /// </summary>
        public Organisasjonsnummer Organisasjonsnummer { get; set; }

        /// <summary>
        /// Avsenders Sertifikatbutikk: Signert virksomhetssertifikat og tilhørende privatnøkkel.
        /// </summary>
        public Sertifikatbutikk Sertifikatbutikk { get; set; }

        /// <param name="organisasjonsnummer">Organisasjonsnummeret til avsender av brevet.</param>
        /// <param name="sertifikatbutikk">Avsenders Sertifikatbutikk: Signert virksomhetssertifikat og tilhørende privatnøkkel.</param>
        public TekniskAvsender(Organisasjonsnummer organisasjonsnummer, Sertifikatbutikk sertifikatbutikk)
        {
            Organisasjonsnummer = organisasjonsnummer;
            Sertifikatbutikk = sertifikatbutikk;
        }
    }
}
