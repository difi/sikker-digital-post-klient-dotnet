using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class FysiskForretningsMelding : ForretningsMelding
    {
        public Adresse mottakerAdresse { get; set; }
        public Posttype posttype { get; set; }
        public Utskriftsfarge utskriftsfarge { get; set; }
        public Posthåndtering returhaandtering { get; set; }
        public Adresse returadresse { get; set; }
        
        public FysiskForretningsMelding() : base(ForretningsMeldingType.PRINT)
        {
            
        }
    }
}
