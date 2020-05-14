using System;
using System.Linq;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
{
    public class Forsendelse
    {
        /// <param name="avsender">
        ///     Ansvarlig avsender av forsendelsen. Dette vil i de aller fleste tilfeller være den offentlige
        ///     virksomheten som er ansvarlig for brevet som skal sendes.
        /// </param>
        /// <param name="postInfo">Informasjon som brukes av postkasseleverandør for å behandle den digitale posten.</param>
        /// <param name="dokumentpakke">Pakke med hoveddokument og ev. vedlegg som skal sendes.</param>
        /// <param name="prioritet">Setter forsendelsens prioritet. Standard er Prioritet.Normal</param>
        /// <param name="språkkode">
        ///     Språkkode i henhold til ISO-639-1 (2 bokstaver). Brukes til å informere postkassen om hvilket
        ///     språk som benyttes, slik at varselet om mulig kan vises i riktig kontekst. Standard er NO.
        /// </param>
        /// <param name="mpcId">
        ///     Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender. En forsendelse gjort
        ///     med en MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id. Standardverdi er "".
        /// </param>
        public Forsendelse(Avsender avsender, PostInfo postInfo, Dokumentpakke dokumentpakke, Prioritet prioritet = Prioritet.Normal, string mpcId = "", string språkkode = "NO")
            : this(avsender, postInfo, dokumentpakke, Guid.NewGuid(), prioritet, mpcId, språkkode)
        {
            SetLanguageIfNotSetOnContainingDocuments();
        }

        /// <param name="avsender">
        ///     Ansvarlig avsender av forsendelsen. Dette vil i de aller fleste tilfeller være den offentlige
        ///     virksomheten som er ansvarlig for brevet som skal sendes.
        /// </param>
        /// <param name="postInfo">Informasjon som brukes av postkasseleverandør for å behandle den digitale posten.</param>
        /// <param name="dokumentpakke">Pakke med hoveddokument og ev. vedlegg som skal sendes.</param>
        /// <param name="konversasjonsId">
        ///     Sett en eksplisitt konversasjonsid. Dette er id som kan brukes for spore alle ledd i
        ///     opprettelse av et brev, og vil være i svar fra Meldingsformidler.
        /// </param>
        /// <param name="prioritet">Setter forsendelsens prioritet. Standard er Prioritet.Normal</param>
        /// <param name="språkkode">
        ///     Språkkode i henhold til ISO-639-1 (2 bokstaver). Brukes til å informere postkassen om hvilket
        ///     språk som benyttes, slik at varselet om mulig kan vises i riktig kontekst. Standard er NO.
        /// </param>
        /// <param name="mpcId">
        ///     Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender. En forsendelse gjort
        ///     med en MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id. Standardverdi er "".
        /// </param>
        public Forsendelse(Avsender avsender, PostInfo postInfo, Dokumentpakke dokumentpakke, Guid konversasjonsId, Prioritet prioritet = Prioritet.Normal, string mpcId = "", string språkkode = "NO")
        {
            Avsender = avsender;
            PostInfo = postInfo;
            Dokumentpakke = dokumentpakke;
            Prioritet = prioritet;
            Språkkode = språkkode;
            MpcId = mpcId;
            KonversasjonsId = konversasjonsId;
            
            MottakerPersonIdentifikator = postInfo is FysiskPostInfo fysiskPostInfo ? fysiskPostInfo.Personidentifikator : (PostInfo.Mottaker as DigitalPostMottaker).Personidentifikator;
        }

        /// <summary>
        ///     Ansvarlig avsender av forsendelsen. Dette vil i de aller fleste tilfeller være den offentlige virksomheten som er
        ///     ansvarlig for brevet som skal sendes.
        /// </summary>
        public Avsender Avsender { get; set; }

        public string MottakerPersonIdentifikator { get; set; }
        
        /// <summary>
        ///     Informasjon som brukes av postkasseleverandør for å behandle den digitale posten.
        /// </summary>
        /// <summary>
        ///     Informasjon som brukes av postkasseleverandør for å behandle posten
        /// </summary>
        public PostInfo PostInfo { get; set; }

        /// <summary>
        ///     Pakke med hoveddokument og ev. vedlegg som skal sendes.
        /// </summary>
        public Dokumentpakke Dokumentpakke { get; set; }

        /// <summary>
        ///     Unik ID opprettet og definert i en initiell melding og siden brukt i alle tilhørende kvitteringer knyttet til den
        ///     opprinnelige meldingen.
        ///     Skal være unik for en avsender.
        /// </summary>
        public Guid KonversasjonsId { get; internal set; }

        /// <summary>
        ///     Setter forsendelsens prioritet. Standard er Prioritet.Normal
        /// </summary>
        public Prioritet Prioritet { get; set; }

        /// <summary>
        ///     Språkkode i henhold til ISO-639-1 (2 bokstaver).
        ///     Brukes til å informere postkassen om hvilket språk som benyttes, slik at varselet om mulig kan vises i riktig
        ///     kontekst.
        ///     Standard er NO.
        /// </summary>
        public string Språkkode { get; set; }

        /// <summary>
        ///     Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender.
        ///     En forsendelse gjort med en MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id.
        ///     Standardverdi er "".
        /// </summary>
        public string MpcId { get; set; }

        public MetadataDocument MetadataDocument { get; set; }
        
        /// <summary>
        ///     Returnerer en ferdig formattert mpc-Forretningskvittering.
        /// </summary>
        public string Mpc => MpcId == string.Empty
            ? $"urn:{Prioritet.ToString().ToLower()}"
            : $"urn:{Prioritet.ToString().ToLower()}:{MpcId}";

        private void SetLanguageIfNotSetOnContainingDocuments()
        {
            Dokumentpakke.Hoveddokument.Språkkode = Dokumentpakke.Hoveddokument.Språkkode ?? Språkkode;
            Dokumentpakke.Vedlegg.Select(p => p.Språkkode = p.Språkkode ?? Språkkode);
        }

        public bool Sendes(Postmetode posttype)
        {
            switch (posttype)
            {
                case Postmetode.Fysisk:
                    return PostInfo.GetType() == typeof (FysiskPostInfo);
                case Postmetode.Digital:
                    return PostInfo.GetType() == typeof (DigitalPostInfo);
                default:
                    throw new ArgumentOutOfRangeException(nameof(posttype), posttype, "Posttype er ikke gyldig.");
            }
        }
    }
}
