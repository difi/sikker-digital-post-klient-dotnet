using System;
using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Leveringskvittering : Forretningskvittering
    {

        public Leveringskvittering(DateTime tidspunkt, string messageId, XmlNode bodyReference)
        {
            Tidspunkt = tidspunkt;
            MessageId = messageId;
            BodyReference = bodyReference;
        }

        public Leveringskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager)
        {
            Tidspunkt = Convert.ToDateTime(DocumentNode(xmlDocument, namespaceManager, "//ns6:Timestamp").InnerText);
            MessageId = DocumentNode(xmlDocument, namespaceManager, "//ns6:MessageId").InnerText;
            BodyReference = BodyReferenceNode(xmlDocument, namespaceManager);
        }
    }
}
