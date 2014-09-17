using System.Collections.Generic;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Abstrakt
{
    public abstract class Varsel
    {
        protected string Varslingstekst;
        protected IEnumerable<int> VarselEtterDager = new List<int>();

        protected Varsel(string varslingstekst)
        {
            Varslingstekst = varslingstekst;
        }
    }
}
