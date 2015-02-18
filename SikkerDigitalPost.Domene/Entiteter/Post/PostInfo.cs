using SikkerDigitalPost.Domene.Entiteter.Aktører;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public abstract class PostInfo
    {
        public PostMottaker Mottaker { get; set; }

        public PostInfo(PostMottaker mottaker)
        {
            Mottaker = mottaker;
        }
    }
}
