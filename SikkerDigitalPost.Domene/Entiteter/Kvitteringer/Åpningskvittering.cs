using System;
using SikkerDigitalPost.Net.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Kvitteringer
{
    public class Åpningskvittering : Forretningskvittering
    {
        public Åpningskvittering(EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} {konversasjonsId={1}}", GetType(), KonversasjonsId);
        }
    }
}
