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
using System.Text.RegularExpressions;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter
{
    public class Organisasjonsnummer
    {
        public static readonly string Iso6523_Pattern = "^([0-9]{4}:)?([0-9]{9})$";
        
        /// <summary>
        /// Stringrepresentasjon av organisasjonsnummeret
        /// </summary>
        public readonly string Verdi;
        
        /// <param name="verdi">Stringrepresentasjon av organisasjonsnummeret</param>
        public Organisasjonsnummer(string verdi)
        {
            Verdi = verdi;
        }

        /// <summary>
        /// Organisasjonsnummer på ISO6523-format 
        /// </summary>
        /// <returns>Organisasjonsnummer, prefikset med '9908':, som er id for 'Enhetsregistret ved Brønnøysundregisterne'</returns>
        public string Iso6523()
        {
            return Verdi.StartsWith("9908:") ? Verdi : String.Format("9908:{0}", Verdi);
        }

        public static Organisasjonsnummer FraIso6523(string iso6523Orgnr)
        {
            var match = Regex.Match(iso6523Orgnr, Iso6523_Pattern);
            
            if (!match.Success)
            {
                throw new KonfigurasjonsException(String.Format("Ugyldig organisasjonsnummer. Forventet format er ISO 6523, " +
                                                          "fikk følgende organisasjonsnummer: {0}.", iso6523Orgnr));
            }
            return new Organisasjonsnummer(match.Groups[2].ToString());
        }
    }
}
