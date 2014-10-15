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

        public Åpningskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager):base(xmlDocument,namespaceManager)
        {
            Tidspunkt = Convert.ToDateTime(DocumentNode("local-name()='tidspunkt'").InnerText);
        }
    }
}
