using System.Collections.Generic;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Varsel
{
    public abstract class Varsel
    {
        /// <summary>
        /// Avsenderstyrt varslingstekst som skal inngå i varselet.
        /// </summary>
        protected string Varslingstekst;


        protected IEnumerable<int> VarselEtterDager = new List<int>();

        protected Varsel(string varslingstekst)
        {
            Varslingstekst = varslingstekst;
        }
    }
}
