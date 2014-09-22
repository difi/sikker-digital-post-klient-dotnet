namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    /// <summary>
    /// Behandlingsansvarlig som beskrevet i http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer.
    /// </summary>
    public class Behandlingsansvarlig
    {
        public readonly Organisasjonsnummer Organisasjonsnummer;

        /// <summary>
        /// Brukes for å identifisere en ansvarlig enhet innenfor en virksomhet. Benyttes dersom det er behov for å skille mellom ulike enheter hos avsender.
        /// </summary>
        public string Avsenderidentifikator { get; set; }

        public string Fakturareferanse { get; set; }

        public Behandlingsansvarlig(Organisasjonsnummer organisasjonsnummer)
        {
            Organisasjonsnummer = organisasjonsnummer;
        }
    }
}
