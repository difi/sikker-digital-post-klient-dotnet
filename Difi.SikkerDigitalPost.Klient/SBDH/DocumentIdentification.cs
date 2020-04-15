using System;
using System.Text.Json.Serialization;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class DocumentIdentification
    {
        public string standard { get; set; }
        public string typeVersion { get; set; }
        public string instanceIdentifier { get; set; }
        public string type { get; set; }
        
        [JsonIgnore]
        public bool multipleType { get; set; }
        public DateTime creationDateAndTime { get; set; }
    }
}
