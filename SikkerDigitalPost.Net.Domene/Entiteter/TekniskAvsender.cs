namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class TekniskAvsender
    {
        /// <summary>
        /// Organisasjonsnummeret til avsender av brevet.
        /// </summary>
        public Organisasjonsnummer Organisasjonsnummer { get; set; }

        /// <summary>
        /// Avsenders nøkkelpar: Signert virksomhetssertifikat og tilhørende privatnøkkel.
        /// </summary>
        public Nøkkelpar Nøkkelpar { get; set; }

        /// <param name="organisasjonsnummer">Organisasjonsnummeret til avsender av brevet.</param>
        /// <param name="nøkkelpar">Avsenders nøkkelpar: Signert virksomhetssertifikat og tilhørende privatnøkkel.</param>
        public TekniskAvsender(Organisasjonsnummer organisasjonsnummer, Nøkkelpar nøkkelpar)
        {
            Organisasjonsnummer = organisasjonsnummer;
            Nøkkelpar = nøkkelpar;
        }
    }
}
