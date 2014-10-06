using System;
using System.Collections.Generic;
using System.Linq;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Varsel
{
    public abstract class Varsel
    {
        /// <summary>
        /// Avsenderstyrt varslingstekst som skal inngå i varselet.
        /// </summary>
        public readonly string Varslingstekst;

        public readonly IEnumerable<int> VarselEtterDager;
        
        protected Varsel(string varslingstekst, IEnumerable<int> varselEtterDager) 
        {
            if (!varselEtterDager.Any())
            {
                varselEtterDager = new List<int> {0};
            }
            else
            {
                VarselEtterDager = varselEtterDager;    
            }
            Varslingstekst = varslingstekst;
            
        }
    }
}
