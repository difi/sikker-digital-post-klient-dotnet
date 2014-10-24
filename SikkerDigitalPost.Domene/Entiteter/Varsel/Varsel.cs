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
    /// <summary>
    /// Informasjon om hvordan postkasseleverandør skal varsle Mottaker om den nye posten. 
    /// 
    /// Varslingsinformasjonen angitt her vil overstyre Mottaker sine egne varslingspreferanser; det vil kunne 
    /// komme som tillegg til Mottaker sine varslingvalg. Avsender kan med instillingene her styre både 
    /// EpostVarsel og SmsVarsel helt uavhengig av hverandre. Det vil si at Avsender kan velge å varsle i begge
    /// eller en av kanalene. Avsender kan velge selv hvilken kanal som velges, dette kan de gjøre med bakgrunn
    /// i sin egen kanalstrategi, erfaringer i forhold til åpningsgrad og kostnader. Bruk av SmsVarsel vil
    /// medføre egne kostnader for Avsender. Se http://begrep.difi.no/SikkerDigitalPost/1.0.1/begrep/Varsler
    /// for mer informasjon.
    /// </summary>
    public abstract class Varsel
    {
        /// <summary>
        /// Avsenderstyrt varslingstekst som skal inngå i varselet.
        /// </summary>
        public readonly string Varslingstekst;

        /// <summary>
        /// Angir hvor langt tid det skal ta (i dager) fra en postforsendelse er sendt til mottaker skal varsles.
        /// </summary>
        public readonly IEnumerable<int> VarselEtterDager;

        protected Varsel(string varslingstekst, IEnumerable<int> varselEtterDager)
        {
            if (!varselEtterDager.Any())
            {
                varselEtterDager = new List<int> { 0 };
            }
            else
            {
                VarselEtterDager = varselEtterDager;
            }

            Varslingstekst = varslingstekst;
        }
    }
}
