using System;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Forsendelse
    {
        public DigitalPost DigitalPost { get; set; }
        public Dokumentpakke Dokumentpakke { get; set; }
        public Behandlingsansvarlig Behandlingsansvarlig { get; set; }
        public readonly string KonversasjonsId = Guid.NewGuid().ToString("N");
        public Prioritet Prioritet = Prioritet.Normal;
        public string Språkkode { get; set; }
        public string MpcId { get; set; }
    }
}
