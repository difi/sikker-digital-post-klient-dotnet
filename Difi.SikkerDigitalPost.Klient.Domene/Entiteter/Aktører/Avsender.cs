using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører
{
    /// <summary>
    ///     Offentlig virksomhet som produserer informasjon/brev/post som skal fomidles. Vil i de aller fleste tilfeller være
    ///     synonymt med Avsender.
    ///     Videre beskrevet på http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer.
    /// </summary>
    public class Avsender
    {
        /// <summary>
        ///     Lager et nytt instans av Avsender.
        /// </summary>
        /// <param name="organisasjonsnummer">Organisasjonsnummeret til den behandlingsansvarlige.</param>
        public Avsender(Organisasjonsnummer organisasjonsnummer)
        {
            Organisasjonsnummer = organisasjonsnummer;
        }

        public Organisasjonsnummer Organisasjonsnummer { get; private set; }

        /// <summary>
        ///     Brukes for å identifisere en ansvarlig enhet innenfor en virksomhet. Benyttes dersom det er behov for å skille
        ///     mellom ulike enheter hos avsender.
        ///     I Sikker digital posttjenteste tildeles avsenderidentifikator ved tilkobling til tjenesten.
        /// </summary>
        public string Avsenderidentifikator { get; set; } = string.Empty;

        /// <summary>
        ///     Maks 40 tegn.
        /// </summary>
        public string Fakturareferanse { get; set; }
    }
}