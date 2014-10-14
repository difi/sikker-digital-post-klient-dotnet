using System;
using System.Linq;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Leveringskvittering :Kvittering //: Forretningskvittering
    {
        public string MessageId { get; private set; }
        internal XmlNode BodyReference { get; private set; }

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

        private static XmlNode BodyReferenceNode(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var rot = document.DocumentElement;
            var referenceNodes = rot.SelectNodes("//ns5:Reference", namespaceManager);

            return referenceNodes.Cast<XmlNode>()
                .FirstOrDefault(referenceNode => referenceNode.Attributes["URI"]
                    .Value.StartsWith("#id"));
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
