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


        public Dictionary<string, object> metadataFiler
        {
            get { return _metadataFiler; }
            set { _metadataFiler = value; }
        }

        private Dictionary<string, object> _metadataFiler = new Dictionary<string, object>();

        public DigitalForretningsMelding(string tittel) : base(ForretningsMeldingType.digital)
        {
            this.tittel = tittel;
        }

        public void addMetadataMapping(string dokumentTittel, string metadataDokumentTittel)
        {
            metadataFiler.Add(dokumentTittel, metadataDokumentTittel);
        }
    }

    public enum Språk
    {
        NO,
        NB,
        EN,
    }
}
