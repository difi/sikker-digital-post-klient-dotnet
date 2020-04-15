using System.Collections.Generic;
using System.Text.Json.Serialization;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class DigitalForretningsMelding : ForretningsMelding
    {
        public string tittel { get; set; }

        public DigitalPostInfo digitalPostInfo { get; set; }
        
        public Sikkerhetsnivå sikkerhetsnivaa { get; set; } = Sikkerhetsnivå.Nivå4;
        public DigitaltVarsel varsler { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Språk spraak { get; set; } = Språk.NO;
        
        public Dictionary<string, string> metadatafiler = new Dictionary<string, string>();
        
        public DigitalForretningsMelding(string tittel) : base(ForretningsMeldingType.DIGITAL)
        {
            this.tittel = tittel;
        }
    }

    public enum Språk
    {
        NO,
        NB,
        EN,
    }
}
