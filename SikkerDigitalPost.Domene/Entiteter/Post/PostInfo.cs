using SikkerDigitalPost.Domene.Entiteter.Aktører;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public abstract class PostInfo
    {
        //Mottaker av digital post.
        public PostMottaker Mottaker { get; set; }
    }
}
