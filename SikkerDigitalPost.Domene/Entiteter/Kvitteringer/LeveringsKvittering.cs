using System;
using SikkerDigitalPost.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Leveringskvittering //: Forretningskvittering
    {
        public string RefToMessageId { get; private set; }
        public string MessagePartNrInformation { get; private set; }

        public Leveringskvittering(string refToMessageId, string messagePartNRInformation)
        {
            RefToMessageId = refToMessageId;
            MessagePartNrInformation = messagePartNRInformation;
        }

        //    public Leveringskvittering(EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
    //    {
    //    }

    //    public override string ToString()
    //    {
    //        return String.Format("{0} {konversasjonsid={1}}" ,GetType().Name, KonversasjonsId);
    //    }
    }
}
