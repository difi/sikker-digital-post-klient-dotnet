using System;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Åpningskvittering : Forretningskvittering
    {
        public readonly DateTime Åpningstidspunkt;

        internal Åpningskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager):base(xmlDocument,namespaceManager)
        {
            Åpningstidspunkt = Convert.ToDateTime(DocumentNode("ns9:tidspunkt").InnerText);
        }
    }
}
