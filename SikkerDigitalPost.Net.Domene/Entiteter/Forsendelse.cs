using System;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Forsendelse
    {
        /// <summary>
        /// Informasjon som brukes av postkasseleverandør for å behandle den digitale posten.
        /// </summary>
        public DigitalPost DigitalPost { get; set; }
        
        /// <summary>
        /// Pakke med hoveddokument og ev. vedlegg som skal sendes.
        /// </summary>
        public Dokumentpakke Dokumentpakke { get; set; }

        /// <summary>
        /// Ansvarlig avsender av forsendelsen. Dette vil i de aller fleste tilfeller vøre den offentlige virksomheten som er ansvarlig for brevet som skal sendes.
        /// </summary>
        public Behandlingsansvarlig Behandlingsansvarlig { get; set; }

        /// <summary>
        /// Unik ID opprettet og definert i en initiell melding og siden brukt i alle tilhørende kvitteringer knyttet til den opprinnelige meldingen.
        /// Skal være unik for en avsender.
        /// </summary>
        public readonly string KonversasjonsId = Guid.NewGuid().ToString();

        /// <summary>
        /// Setter forsendelsens prioritet. Standard er Prioritet.Normal
        /// </summary>
        public Prioritet Prioritet = Prioritet.Normal;

        /// <summary>
        /// Språkkode i henhold til ISO-639-1 (2 bokstaver). 
        /// Brukes til å informere postkassen om hvilket språk som benyttes, slik at varselet om mulig kan vises i riktig kontekst.
        /// 
        /// Standard er NO.
        /// </summary>
        public string Språkkode = "NO";
        
        /// <summary>
        /// Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender. 
        /// En forsendelse gjort med en MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id.
        /// 
        /// Standardverdi er blank MPC Id.
        /// </summary>
        public string MpcId { get; set; }
    }
}
