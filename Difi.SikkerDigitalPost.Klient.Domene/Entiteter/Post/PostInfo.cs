using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
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
