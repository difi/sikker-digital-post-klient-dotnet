using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public class FysiskPostInfo : PostInfo
    {
        public Posttype Posttype { get; set; }

        public Utskriftsfarge Utskriftsfarge { get; set; }

        public Posthåndtering Posthåndtering { get; set; }

    }
}
