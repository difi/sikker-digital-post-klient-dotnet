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
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    /// <summary>
    /// En feilmelding fra postkasseleverandør med informasjon om en forretningsfeil knyttet til en digital post forsendelse.
    /// Les mer på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/FeilMelding.
    /// </summary>
    public class Feilmelding : Forretningskvittering
    {
        /// <summary>
        /// Beskriver hvor feilen ligger. Enten Klient eller Server.
        /// </summary>
        public Feiltype Skyldig { get; private set; }

        public string Detaljer { get; private set; }

        public DateTime TidspunktFeilet { get; private set; }

        internal Feilmelding(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager):base(xmlDocument,namespaceManager)
        {
            try
            {
                TidspunktFeilet = Convert.ToDateTime(DocumentNode("//ns9:tidspunkt").InnerText);

                var feiltype = DocumentNode("//ns9:feiltype").InnerText;
                Skyldig = feiltype.ToLower().Equals(Feiltype.Klient.ToString().ToLower())
                    ? Feiltype.Klient
                    : Feiltype.Server;

                Detaljer = DocumentNode("//ns9:detaljer").InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException("Feil under bygging av Feilmelding-kvittering. Klarte ikke finne alle felter i xml.", e);
            }
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nTidspunkt feilet: {3}. \nSkyldig: {4}. \nDetaljer: {5}. \nKonversasjonsId: {6}. \nRefererer til melding med id: {7}", 
                GetType().Name, MeldingsId, Tidspunkt, TidspunktFeilet, Skyldig, Detaljer, KonversasjonsId, RefToMessageId);
        }
    }
}
