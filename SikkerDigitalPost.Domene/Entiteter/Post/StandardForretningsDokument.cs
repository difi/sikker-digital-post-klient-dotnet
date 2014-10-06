using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public class StandardForretningsDokument
    {
        public string KonversasjonsId { get; set; }
        public Forretningskvittering Kvittering { get; set; }
    }
}
