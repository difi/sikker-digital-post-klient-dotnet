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

using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Domene.Entiteter.Aktører
{
    /// <summary>
    /// Mottaker av en digital postmelding.
    /// </summary>
    public class DigitalPostMottaker : PostMottaker
    {
        /// <summary>
        /// Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.
        /// </summary>
        public string Personidentifikator { get; set; }
        
        /// <summary>
        /// Mottakerens adresse hos postkasseleverandøren.
        /// </summary>
        public string Postkasseadresse { get; set; }

       /// <summary>
        /// Informasjon om mottaker. Vil vanligvis være hentet fra http://begrep.difi.no/Oppslagstjenesten/.
        /// </summary>
        /// <param name="personidentifikator">Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.</param>
        /// <param name="postkasseadresse">Mottakerens adresse hos postkasseleverandøren.</param>
        /// <param name="sertifikat">Mottakerens sertifikat.</param>
        /// <param name="organisasjonsnummerPostkasse">Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.</param>
        public DigitalPostMottaker(string personidentifikator, string postkasseadresse, X509Certificate2 sertifikat, string organisasjonsnummerPostkasse)
            : base(sertifikat,organisasjonsnummerPostkasse)
       {
           Personidentifikator = personidentifikator;
           Postkasseadresse = postkasseadresse;
       }
    }
}
