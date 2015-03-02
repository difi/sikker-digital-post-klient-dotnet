using System;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public abstract class PostInfo
    {
        public PostMottaker Mottaker { get; set; }
        
        protected PostInfo(PostMottaker mottaker)
        {
            Mottaker = mottaker;
        }

        internal PMode PMode()
        {
            var type = GetType();

            if (type == typeof(FysiskPostInfo))
                return Enums.PMode.FormidleFysiskPost;

            if (type == typeof(DigitalPostInfo))
                return Enums.PMode.FormidleDigitalPost;

            throw new ArgumentOutOfRangeException("postInfo", type, "PostInfo har feil type.");
        }


    }
}
