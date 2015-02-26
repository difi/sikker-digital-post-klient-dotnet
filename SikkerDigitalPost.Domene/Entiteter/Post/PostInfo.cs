using SikkerDigitalPost.Domene.Entiteter.Aktører;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public abstract class PostInfo
    {
        public PostMottaker Mottaker { get; set; }

        protected PostInfo(PostMottaker mottaker)
        {
            Mottaker = mottaker;
        }
    }
}
