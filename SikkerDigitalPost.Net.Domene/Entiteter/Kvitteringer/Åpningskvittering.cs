using System;
using SikkerDigitalPost.Net.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Kvitteringer
{
    public class Åpningskvittering : Forretningskvittering
    {
        public Åpningskvittering(EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
        {
        }
    }
}
