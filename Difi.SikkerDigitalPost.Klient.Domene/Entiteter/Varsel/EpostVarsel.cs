using System;
using System.Collections.Generic;
using System.Linq;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel
{
    public class EpostVarsel : Varsel
    {
        public readonly string Epostadresse;

        /// <param name="varslingstekst">Avsenderstyrt varslingstekst som skal inngå i varselet.</param>
        public EpostVarsel(string varslingstekst) : base(varslingstekst)
        {
        }

        [Obsolete]
        public EpostVarsel(string epostadresse, string varslingstekst, IEnumerable<int> varselEtterDager)
            : base(varslingstekst, varselEtterDager)
        {
            Epostadresse = epostadresse;
        }

        [Obsolete]
        public EpostVarsel(string epostadresse, string varslingstekst, params int[] varselEtterDager)
            : this(epostadresse, varslingstekst, varselEtterDager.ToList())
        {
        }
    }
}
