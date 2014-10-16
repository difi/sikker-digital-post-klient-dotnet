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

        private string _avsenderidentifikator = String.Empty;
        /// <summary>
        /// Brukes for å identifisere en ansvarlig enhet innenfor en virksomhet. Benyttes dersom det er behov for å skille mellom ulike enheter hos avsender.
        /// </summary>
        public string Avsenderidentifikator
        {
            get { return _avsenderidentifikator; }
            set
            {
                if (value.Length <= 100)
                    _avsenderidentifikator = value;
                else
                    throw new KonfigurasjonsException(
                        String.Format("Avsenderidentifikator kan ikke være lengre enn 100 tegn, input streng er {0} tegn lang. Du sendte inn {1}",
                        value.Length, value));
                    
            }
        }

        private string _fakturareferanse = String.Empty;

        public string Fakturareferanse
        {
            get { return _fakturareferanse; }
            set
            {
                if (value.Length <= 40)
                    _fakturareferanse = value;
                else
                    throw new KonfigurasjonsException(
                        String.Format("Fakturareferanse kan ikke være lengre enn 100 tegn, input streng er {0} tegn lang. Du sendte inn {1}",
                        value.Length, value));
            }
        }

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
