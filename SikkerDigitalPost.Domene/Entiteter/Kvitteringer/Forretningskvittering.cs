using System;
using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public abstract class Forretningskvittering
    {
        public DateTime Tidspunkt { get; protected set; }

        public string KonversasjonsId { get; protected set; }

        public string MessageId { get; protected set; }

        public string RefToMessageId { get; protected set; }
        
        internal XmlNode BodyReference { get; set; }

        protected static XmlNode DocumentNode(XmlDocument document, XmlNamespaceManager namespaceManager, string xPath)
        {
            var rot = document.DocumentElement;
            var targetNode = rot.SelectSingleNode(xPath, namespaceManager);

            return targetNode;
        }

        protected static XmlNode BodyReferenceNode(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var rot = document.DocumentElement;
            
            var partInfo = rot.SelectSingleNode("//ns6:PartInfo", namespaceManager);
            var partInfoBodyId = "";
            if (partInfo.Attributes.Count > 0)
                partInfoBodyId = partInfo.Attributes["href"].Value;

            var bodyId = rot.SelectSingleNode("//env:Body", namespaceManager).Attributes["wsu:Id"].Value;

            if (!partInfoBodyId.Equals(String.Empty) && !bodyId.Equals(partInfoBodyId))
            {
                throw new Exception("Id i PartInfo og i Body matcher ikke.");
            }
            
            var bodyReferenceNode = rot.SelectSingleNode("//ns5:Reference[@URI = '#" + bodyId + "']", namespaceManager);

            return bodyReferenceNode;
        }
    }
}
