/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Linq;

namespace SikkerDigitalPost.Domene.Entiteter.Varsel
{
    public class EpostVarsel : Varsel
    {
        public readonly string Epostadresse;

        /// <param name="epostadresse">Mottakerens epostadresse som det skal sendes varsel til.</param>
        /// <param name="varslingstekst">Avsenderstyrt varslingstekst som skal inngå i varselet.</param>
        public EpostVarsel(string epostadresse, string varslingstekst, IEnumerable<int> varselEtterDager) : base (varslingstekst, varselEtterDager)
        {
            Epostadresse = epostadresse;
        }

        public EpostVarsel(string epostadresse, string varslingstekst, params int[] varselEtterDager) 
            : this(epostadresse,varslingstekst, varselEtterDager.ToList())
        {
        }

    }
}
