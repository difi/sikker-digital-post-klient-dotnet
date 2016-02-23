using System.Collections.Generic;
using System.Linq;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel
{
    public class EpostVarsel : Varsel
    {
        public readonly string Epostadresse;

        /// <param name="epostadresse">Mottakerens epostadresse som det skal sendes varsel til.</param>
        /// <param name="varslingstekst">Avsenderstyrt varslingstekst som skal inngå i varselet.</param>
        /// <param name="varselEtterDager">Hvor mange dager etter at meldingen er levert at varselet skal leveres.</param>
        public EpostVarsel(string epostadresse, string varslingstekst, IEnumerable<int> varselEtterDager)
            : base(varslingstekst, varselEtterDager)
        {
            Epostadresse = epostadresse;
        }

        public EpostVarsel(string epostadresse, string varslingstekst, params int[] varselEtterDager)
            : this(epostadresse, varslingstekst, varselEtterDager.ToList())
        {
        }
    }
}