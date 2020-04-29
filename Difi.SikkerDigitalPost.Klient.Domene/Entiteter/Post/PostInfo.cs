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
        
        public PostMottaker Mottaker { get; set; }
        
        protected PostInfo(PostMottaker mottaker, string type)
        {
            Type = type;
            Mottaker = mottaker;
        }
    }
}
