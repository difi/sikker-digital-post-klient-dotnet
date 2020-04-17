using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class Retur
    {
        public FysiskPostMottaker mottaker { get; set; }
        
        public Posthåndtering returhaandtering { get; set; }
    }
}
