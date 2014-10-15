using System;
using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Leveringskvittering : Forretningskvittering
    {
        internal Leveringskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager) : base(xmlDocument,namespaceManager)
        {
            Tidspunkt = Convert.ToDateTime(DocumentNode("//ns6:Timestamp").InnerText);
        }
    }
}
