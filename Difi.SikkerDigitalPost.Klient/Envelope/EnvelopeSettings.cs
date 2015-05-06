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

using Difi.SikkerDigitalPost.Klient.AsicE;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using KvitteringsForespørsel = Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Kvitteringsforespørsel;

namespace Difi.SikkerDigitalPost.Klient.Envelope
{
    internal class EnvelopeSettings
    {
        public Forsendelse Forsendelse { get; private set; }
        public Databehandler Databehandler { get; private set; }
        public KvitteringsForespørsel Kvitteringsforespørsel { get; private set; }
        public Forretningskvittering ForrigeKvittering { get; private set; }
        internal AsicEArkiv AsicEArkiv { get; private set; }
        internal GuidHandler GuidHandler { get; private set; }
        internal Klientkonfigurasjon Konfigurasjon { get; private set; }
        
        /// <summary>
        /// Settings for KvitteringsEnvelope
        /// </summary>
        public EnvelopeSettings(KvitteringsForespørsel kvitteringsforespørsel, Databehandler databehandler, GuidHandler guidHandler)
        {
            Kvitteringsforespørsel = kvitteringsforespørsel;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }

        /// <summary>
        /// Settings for DigitalPostForsendelse
        /// </summary>
        public EnvelopeSettings(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler, GuidHandler guidHandler, Klientkonfigurasjon konfigurasjon)
        {
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
            Konfigurasjon = konfigurasjon;
        }
        
        /// <summary>
        /// Settings for BekreftKvittering
        /// </summary>
        public EnvelopeSettings(Forretningskvittering forrigeKvittering, Databehandler databehandler, GuidHandler guidHandler)
        {
            ForrigeKvittering = forrigeKvittering;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }
    }
}
