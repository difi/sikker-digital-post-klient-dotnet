using System;
using SikkerDigitalPost.Net.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Kvitteringer
{
    public abstract class Forretningskvittering
    {
        public EbmsApplikasjonskvittering EbmsApplikasjonskvittering { get; set; }

        protected Forretningskvittering(EbmsApplikasjonskvittering applikasjonskvittering)
        {
            EbmsApplikasjonskvittering = applikasjonskvittering;
        }
        
        public DateTime Tidspunkt
        {
            get
            { 
                throw new NotImplementedException("Denne metoden skal hente applikasjonskvitteringens standardbusinessdokument sin kvittering. Se AapningsKvittering.java, linje 30");
            }
            set{}
        }
    }
}
