using System;
using SikkerDigitalPost.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class LeveringsKvittering : Forretningskvittering
    {
        public LeveringsKvittering(EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} {konversasjonsid={1}}" ,GetType().Name, KonversasjonsId);
        }
    }
}
