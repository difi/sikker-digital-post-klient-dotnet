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
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public class Forsendelse
    {

        /// <param name="behandlingsansvarlig">Ansvarlig avsender av forsendelsen. Dette vil i de aller fleste tilfeller være den offentlige virksomheten som er ansvarlig for brevet som skal sendes.</param>
        /// <param name="postInfo">Informasjon som brukes av postkasseleverandør for å behandle den digitale posten.</param>
        /// <param name="dokumentpakke">Pakke med hoveddokument og ev. vedlegg som skal sendes.</param>
        /// <param name="prioritet">Setter forsendelsens prioritet. Standard er Prioritet.Normal</param>
        /// <param name="språkkode">Språkkode i henhold til ISO-639-1 (2 bokstaver). Brukes til å informere postkassen om hvilket språk som benyttes, slik at varselet om mulig kan vises i riktig kontekst. Standard er NO.</param>
        /// <param name="mpcId">Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender. En forsendelse gjort med en MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id. Standardverdi er "".</param>
        public Forsendelse(Behandlingsansvarlig behandlingsansvarlig, PostInfo postInfo,
            Dokumentpakke dokumentpakke, Prioritet prioritet = Prioritet.Normal, string mpcId = "", string språkkode = "NO")
        {
            Behandlingsansvarlig = behandlingsansvarlig;
            PostInfo = postInfo;
            Dokumentpakke = dokumentpakke;
            Prioritet = prioritet;
            Språkkode = språkkode;
            MpcId = mpcId;
        }
        
        /// <summary>
        /// Ansvarlig avsender av forsendelsen. Dette vil i de aller fleste tilfeller være den offentlige virksomheten som er ansvarlig for brevet som skal sendes.
        /// </summary>
        public Behandlingsansvarlig Behandlingsansvarlig { get; set; }

        /// <summary>
        /// Informasjon som brukes av postkasseleverandør for å behandle den digitale posten.
        /// </summary>
        //public DigitalPostInfo DigitalPostInfo { get; set; }

        /// <summary>
        /// Informasjon som brukes av postkasseleverandør for å behandle posten
        /// </summary>
        public PostInfo PostInfo { get; set; }

        /// <summary>
        /// Pakke med hoveddokument og ev. vedlegg som skal sendes.
        /// </summary>
        public Dokumentpakke Dokumentpakke { get; set; }

        /// <summary>
        /// Unik ID opprettet og definert i en initiell melding og siden brukt i alle tilhørende kvitteringer knyttet til den opprinnelige meldingen.
        /// Skal være unik for en avsender.
        /// </summary>
        public readonly Guid KonversasjonsId = Guid.NewGuid();

        /// <summary>
        /// Setter forsendelsens prioritet. Standard er Prioritet.Normal
        /// </summary>
        public Prioritet Prioritet { get; set; }

        /// <summary>
        /// Språkkode i henhold til ISO-639-1 (2 bokstaver). 
        /// Brukes til å informere postkassen om hvilket språk som benyttes, slik at varselet om mulig kan vises i riktig kontekst.
        /// 
        /// Standard er NO.
        /// </summary>
        public string Språkkode { get; set; }

        /// <summary>
        /// Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender. 
        /// En forsendelse gjort med en MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id.
        /// 
        /// Standardverdi er "".
        /// </summary>
        public string MpcId { get; set; }

        /// <summary>
        /// Returnerer en ferdig formattert mpc-Forretningskvittering.
        /// </summary>
        public string Mpc
        {
            get
            {
                return MpcId == String.Empty
                ? String.Format("urn:{0}", Prioritet.ToString().ToLower())
                : String.Format("urn:{0}:{1}", Prioritet.ToString().ToLower(), MpcId);
            }
        }
    }
}
