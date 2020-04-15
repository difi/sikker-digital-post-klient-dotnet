using System;
using System.Text.Json.Serialization;
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

        [JsonIgnore]
        public string Type { get; set; }
        
        [JsonIgnore]
        public string hoveddokument { get; set; }

        [JsonIgnore]
        private string avsenderId { get; set; }
        
        [JsonIgnore]
        private string fakturaReferanse { get; set; }
        
        public PostMottaker Mottaker { get; set; }
        
        protected PostInfo(PostMottaker mottaker, string type)
        {
            Type = type;
            Mottaker = mottaker;
        }

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