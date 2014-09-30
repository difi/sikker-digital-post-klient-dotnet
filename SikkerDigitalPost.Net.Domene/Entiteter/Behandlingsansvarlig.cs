using System;
using System.CodeDom;
using System.Runtime.Remoting.Messaging;

namespace SikkerDigitalPost.Net.Domene.Entiteter
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
                    throw new ArgumentException("Avsenderidentifikator kan ikke være lengre enn 100 tegn, følgende streng er ikke: " + value);
                    
            }
        }

        private string _fakturaReferanse = String.Empty;

        public string Fakturareferanse
        {
            get { return _fakturaReferanse; }
            set
            {
                if (value.Length <= 40)
                    _fakturaReferanse = value;
                else
                    throw new ArgumentException("Fakturareferanse kan ikke være lengre enn 40 tegn, følgende streng er ikke: " + value);
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
