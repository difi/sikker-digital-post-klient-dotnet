using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
{
    /// <summary>
    ///     Abstrakt klasse for informasjon knyttet til en forsendelse. Kan instansieres
    ///     som FysiskPostInfo og DigitalPostInfo.
    /// </summary>
    public abstract class PostInfo
    {

        public string Type { get; set; }
        
        protected PostInfo(PostMottaker mottaker, string type)
        {
            Type = type;
            Mottaker = mottaker;
        }

        public PostMottaker Mottaker { get; set; }

        internal PMode PMode()
        {
            var type = GetType();

            if (type == typeof (FysiskPostInfo))
                return Enums.PMode.FormidleFysiskPost;

            if (type == typeof (DigitalPostInfo))
                return Enums.PMode.FormidleDigitalPost;

            throw new ArgumentOutOfRangeException("postInfo", type, "PostInfo har feil type.");
        }
    }
}