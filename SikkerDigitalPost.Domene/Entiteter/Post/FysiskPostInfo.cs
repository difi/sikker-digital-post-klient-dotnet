using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
{
    public class FysiskPostInfo : PostInfo
    {
        public Posttype Posttype { get; set; }

        public Utskriftsfarge Utskriftsfarge { get; set; }

        public Posthåndtering Posthåndtering { get; set; }

        public FysiskPostMottaker ReturMottaker { get; set; }

        public FysiskPostInfo(PostMottaker mottaker, Posttype posttype, Utskriftsfarge utskriftsfarge, Posthåndtering posthåndtering, FysiskPostMottaker returMottaker) : base(mottaker)
        {
            Posttype = posttype;
            Utskriftsfarge = utskriftsfarge;
            Posthåndtering = posthåndtering;
            ReturMottaker = returMottaker;
        }
    }
}
