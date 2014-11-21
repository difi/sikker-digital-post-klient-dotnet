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
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    /// <summary>
    /// En kvitteringsmelding til Avsender om at Mottaker har åpnet forsendelsen i sin postkasse.
    /// Mer informasjon finnes på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/AapningsKvittering.
    /// </summary>
    public class Åpningskvittering : Forretningskvittering
    {
        /// <summary>
        /// Tidspunkt for når borger åpnet posten i sin postkasse.
        /// </summary>
        public DateTime Åpningstidspunkt { get; protected set; }
        public Åpningskvittering() { }
        internal Åpningskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager):base(xmlDocument,namespaceManager)
        {
            try
            {
                Åpningstidspunkt = Convert.ToDateTime(DocumentNode("//ns9:tidspunkt").InnerText);
            }
            catch (Exception e)
            {
                throw new XmlParseException("Feil under bygging av Åpningskvittering. Klarte ikke finne alle felter i xml.", e);
            }
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nÅpningstidspunkt: {3}. \nKonversasjonsId: {4}. \nRefererer til melding med id: {5}", 
                GetType().Name, MeldingsId, Tidspunkt, Åpningstidspunkt, KonversasjonsId, RefToMessageId);
        }
    }
}
