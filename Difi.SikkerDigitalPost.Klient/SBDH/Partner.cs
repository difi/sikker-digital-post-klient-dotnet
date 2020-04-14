using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class Partner
    {
        public PartnerIdentification identifier { get; set; }

        public List<ContactInformation> contactInformation
        {
            get { return _contactInformation; }
            set { _contactInformation = value; }
        }
        
        private List<ContactInformation> _contactInformation = new List<ContactInformation>();

        public Partner(PartnerIdentification identifier)
        {
            this.identifier = identifier;
        }
    }
}
