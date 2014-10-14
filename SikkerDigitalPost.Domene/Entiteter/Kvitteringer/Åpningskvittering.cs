using System;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Åpningskvittering : Forretningskvittering
    {
        public Åpningskvittering(DateTime tidspunkt)
        {
            Tidspunkt = tidspunkt;
        }

        public Åpningskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager)
        {
            Tidspunkt = Convert.ToDateTime(DocumentNode(xmlDocument, namespaceManager, "local-name()='tidspunkt'").InnerText);
        }
    }

    //public class OldÅpningskvittering : Forretningskvittering
    //{
    //    public OldÅpningskvittering(EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
    //    {
    //    }

    //    public override string ToString()
    //    {
    //        return String.Format("{0} {konversasjonsId={1}}", GetType(), KonversasjonsId);
    //    }
    //}
}
