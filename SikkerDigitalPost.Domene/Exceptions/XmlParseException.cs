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

namespace SikkerDigitalPost.Domene.Exceptions
{
    public class XmlParseException : KonfigurasjonsException
    {
        private const string ekstrainfo =
            " En mulig grunn til dette kan være at svar fra server har endret seg, eller at en feilaktig endring har blitt gjort.";

        private const string ekstrainnerinfo = " Sjekk inner exception for mer detaljer.";

        public string Rådata { get; set; }

        public XmlParseException()
        {
            
        }

        public XmlParseException(string message) : base(message + ekstrainfo)
        {
            
        }

        public XmlParseException(string message, Exception inner) 
            : base (message + ekstrainfo + ekstrainnerinfo, inner)
        {
            
        }

    }
}
