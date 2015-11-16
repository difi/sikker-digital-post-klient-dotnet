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
using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{
    /// <summary>
    /// Kvittering fra meldingsformidler som indikerer at denne har overtatt ansvaret for videre formidling av meldingen.
    /// </summary>
    public class TransportOkKvittering : Transportkvittering
    {
        public TransportOkKvittering()
        { }
        public TransportOkKvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nRefererer til melding med id: {3}",
                GetType().Name, MeldingsId, SendtTidspunkt, ReferanseTilMeldingId);
        }
    }
}
