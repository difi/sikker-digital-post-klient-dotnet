using System;
using SikkerDigitalPost.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public abstract class Forretningskvittering
    {
        public EbmsApplikasjonskvittering EbmsApplikasjonskvittering { get; set; }

        protected Forretningskvittering(EbmsApplikasjonskvittering applikasjonskvittering)
        {
            EbmsApplikasjonskvittering = applikasjonskvittering;
        }
        
        public string KonversasjonsId
        {
            get { return EbmsApplikasjonskvittering.StandardForretningsDokument.KonversasjonsId; }
            set { }
        }

        public abstract override string ToString();

        public DateTime Tidspunkt()
        {
            DateTime tidspunkt = EbmsApplikasjonskvittering.StandardForretningsDokument.Kvittering.Tidspunkt();
            return tidspunkt;
        }
    }
}
