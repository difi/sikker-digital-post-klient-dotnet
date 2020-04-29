using System;
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

        /// <param name="varslingstekst">Avsenderstyrt varslingstekst som skal inngå i varselet.</param>
        public SmsVarsel(string varslingstekst) : base(varslingstekst)
        {
        }

        [Obsolete]
        public SmsVarsel(string mobilnummer, string varslingstekst, IEnumerable<int> varselEtterDager)
            : base(varslingstekst, varselEtterDager)
        {
            Mobilnummer = mobilnummer;
        }

        [Obsolete]
        public SmsVarsel(string mobilnummer, string varslingstekst, params int[] varselEtterDager)
            : this(mobilnummer, varslingstekst, varselEtterDager.ToList())
        {
        }
    }
}
