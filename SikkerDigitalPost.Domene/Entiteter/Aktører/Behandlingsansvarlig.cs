namespace SikkerDigitalPost.Domene.Entiteter.Aktører
{
    /// <summary>
    /// Offentlig virksomhet som produserer informasjon/brev/post som skal fomidles. Vil i de aller fleste tilfeller være synonymt med Avsender.
    /// Videre beskrevet på http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer.
    /// </summary>
    public class Behandlingsansvarlig
    {
        public Organisasjonsnummer Organisasjonsnummer { get; private set; }

        /// <summary>
        /// Brukes for å identifisere en ansvarlig enhet innenfor en virksomhet. Benyttes dersom det er behov for å skille mellom ulike enheter hos avsender.
        /// I Sikker digital posttjenteste tildeles avsenderidentifikator ved tilkobling til tjenesten. Maks 100 tegn.
        /// </summary>
        public string Avsenderidentifikator { get; set; }

        /// <summary>
        /// Maks 40 tegn.
        /// </summary>
        public string Fakturareferanse { get; set; }

        /// <summary>
        /// Lager et nytt instans av Behandlingsansvarlig.
        /// </summary>
        /// <param name="organisasjonsnummer">Organisasjonsnummeret til den behandlingsansvarlige.</param>
        public Behandlingsansvarlig(Organisasjonsnummer organisasjonsnummer)
        {
            Organisasjonsnummer = organisasjonsnummer;
        }

        /// <summary>
        /// Lager et nytt instans av behandlingsansvarlig.
        /// </summary>
        /// <param name="organisasjonsnummer">Organisasjonsnummeret til den behandlingsansvarlige.</param>
        public Behandlingsansvarlig(string organisasjonsnummer) : this(new Organisasjonsnummer(organisasjonsnummer))
        {
        }
    }
}
