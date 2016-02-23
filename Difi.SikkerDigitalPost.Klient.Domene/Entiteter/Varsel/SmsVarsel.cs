using System.Collections.Generic;
using System.Linq;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel
{
    public class SmsVarsel : Varsel
    {
        /// <summary>
        ///     Mobiltelefonnummeret varselet skal sendes til. For informasjon om validering, se
        ///     http://begrep.difi.no/Felles/mobiltelefonnummer.
        /// </summary>
        /// <remarks></remarks>
        public readonly string Mobilnummer;

        /// <param name="mobilnummer">Mobiltelefonnummer varselet skal sendes til.</param>
        /// <param name="varslingstekst">Avsenderstyrt varslingstekst som skal inngå i varselet.</param>
        /// <param name="varselEtterDager">Hvor mange dager etter at meldingen er levert at varselet skal leveres.</param>
        public SmsVarsel(string mobilnummer, string varslingstekst, IEnumerable<int> varselEtterDager)
            : base(varslingstekst, varselEtterDager)
        {
            Mobilnummer = mobilnummer;
        }

        public SmsVarsel(string mobilnummer, string varslingstekst, params int[] varselEtterDager)
            : this(mobilnummer, varslingstekst, varselEtterDager.ToList())
        {
        }
    }
}