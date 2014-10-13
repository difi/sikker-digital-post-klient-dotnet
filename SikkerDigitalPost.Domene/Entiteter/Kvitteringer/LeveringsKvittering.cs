using System;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Leveringskvittering //: Forretningskvittering
    {
        public string RefToMessageId { get; private set; }
        public XmlNode BodyReference { get; private set; }

        public Leveringskvittering(string refToMessageId, XmlNode bodyReference)
        {
            RefToMessageId = refToMessageId;
            BodyReference = bodyReference;
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
