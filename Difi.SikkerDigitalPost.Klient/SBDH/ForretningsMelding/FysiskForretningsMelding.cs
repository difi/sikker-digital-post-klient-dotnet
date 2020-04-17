using System.Text.Json.Serialization;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class FysiskForretningsMelding : ForretningsMelding
    {
        public FysiskPostMottaker mottaker { get; set; }
        
        public Posttype posttype { get; set; }
        
        public Utskriftsfarge utskriftsfarge { get; set; }
        
        [JsonIgnore]
        public Adresse returadresse { get; set; }
        
        public Retur retur { get; set; }
        
        public FysiskForretningsMelding() : base(ForretningsMeldingType.print)
        {
            
        }
    }
}
