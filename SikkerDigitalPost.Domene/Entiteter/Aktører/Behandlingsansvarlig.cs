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

using System;

namespace SikkerDigitalPost.Domene.Entiteter.Aktører
{
    /// <summary>
    /// Offentlig virksomhet som produserer informasjon/brev/post som skal fomidles. Vil i de aller fleste tilfeller være synonymt med Avsender.
    /// Videre beskrevet på http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer.
    /// </summary>
    public class Behandlingsansvarlig
    {
        public Organisasjonsnummer Organisasjonsnummer { get; private set; }

        private string _avsenderidentifikator = String.Empty;
        /// <summary>
        /// Brukes for å identifisere en ansvarlig enhet innenfor en virksomhet. Benyttes dersom det er behov for å skille mellom ulike enheter hos avsender.
        /// I Sikker digital posttjenteste tildeles avsenderidentifikator ved tilkobling til tjenesten.
        /// </summary>
        public string Avsenderidentifikator
        {
            get { return _avsenderidentifikator; }
            set { _avsenderidentifikator = value; }
        }


        /// <summary>
        /// Maks 40 tegn.
        /// </summary>
        public string Fakturareferanse { get; set; }

        /// <summary>
        /// Lager et nytt instans av Behandlingsansvarlig.
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
