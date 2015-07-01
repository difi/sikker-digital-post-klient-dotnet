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
using System.Collections.Generic;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;

namespace Difi.SikkerDigitalPost.Klient
{
    internal class SoapContainer
    {
        public readonly string Boundary;
        public IList<ISoapVedlegg> Vedlegg { get; set; }
        public ISoapVedlegg Envelope { get; set; }

        public SoapContainer(ISoapVedlegg envelope)
        {
            Envelope = envelope;
            Boundary = Guid.NewGuid().ToString();
            Vedlegg = new List<ISoapVedlegg>();
        }
    }
}
