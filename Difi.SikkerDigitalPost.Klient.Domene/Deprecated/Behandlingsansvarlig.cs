using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører
{
    [Obsolete("Behandlingsansvarlig heter nå Avsender, da behandlingsansvarlig vil for alle praktiske formål være avsenderen." +
              " Ingen andre endringer er gjort, og overgang kan gjøres uten bieffekter. OBS! Vil bli fjernet fom. neste versjon.")]
    public class Behandlingsansvarlig
    {
        private readonly Avsender _målKlasse;

        public Organisasjonsnummer Organisasjonsnummer
        {
            get { return _målKlasse.Organisasjonsnummer; }
        }

        private string _avsenderidentifikator = String.Empty;
        /// <summary>
        /// Brukes for å identifisere en ansvarlig enhet innenfor en virksomhet. Benyttes dersom det er behov for å skille mellom ulike enheter hos avsender.
        /// I Sikker digital posttjenteste tildeles avsenderidentifikator ved tilkobling til tjenesten.
        /// </summary>
        public string Avsenderidentifikator
        {
            get { return _målKlasse.Avsenderidentifikator; }
            set { _målKlasse.Avsenderidentifikator = value; }
        }


        /// <summary>
        /// Maks 40 tegn.
        /// </summary>
        public string Fakturareferanse
        {
            get { return _målKlasse.Fakturareferanse; }
            set { _målKlasse.Fakturareferanse = value; }
            
        }

        /// <summary>
        /// Lager et nytt instans av Avsender.
        /// </summary>
        /// <param name="organisasjonsnummer">Organisasjonsnummeret til den behandlingsansvarlige.</param>
        public Behandlingsansvarlig(Organisasjonsnummer organisasjonsnummer)
        {
            _målKlasse = new Avsender(organisasjonsnummer);
        }

        /// <summary>
        /// Lager et nytt instans av Avsender.
        /// </summary>
        /// <param name="organisasjonsnummer">Organisasjonsnummeret til den behandlingsansvarlige.</param>
        public Behandlingsansvarlig(string organisasjonsnummer)
            : this(new Organisasjonsnummer(organisasjonsnummer))
        {
            _målKlasse = new Avsender(organisasjonsnummer);
        }
    }
}
