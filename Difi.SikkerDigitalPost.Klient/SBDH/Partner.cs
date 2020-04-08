using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class Partner
    {
        protected PartnerIdentification identifier;
        protected List<ContactInformation> contactInformation;

        public Partner(PartnerIdentification identifier)
        {
            this.identifier = identifier;
        }
    }
}
