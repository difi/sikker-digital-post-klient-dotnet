using SikkerDigitalPost.Net.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Kvitteringer
{
    public class LeveringsKvittering : Forretningskvittering
    {
        public LeveringsKvittering(EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
        {
        }
    }
}
