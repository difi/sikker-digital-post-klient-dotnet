using System;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Aktører
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

        /// <summary>
        /// Lager et nytt instans av behandlingsansvarlig.
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
