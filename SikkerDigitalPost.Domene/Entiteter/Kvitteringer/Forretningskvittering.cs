using System;
using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public abstract class Forretningskvittering
    {
        private readonly XmlDocument _document;
        private readonly XmlNamespaceManager _namespaceManager;


        public DateTime Tidspunkt { get; protected set; }

        public string KonversasjonsId { get; protected set; }

        public string MessageId { get; protected set; }

        public string RefToMessageId { get; protected set; }
        
        internal XmlNode BodyReference { get; set; }

        protected Forretningskvittering()
        {
            
        }

        protected Forretningskvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            _document = document;
            _namespaceManager = namespaceManager;

            MessageId = DocumentNode("//ns6:MessageId").InnerText;
            BodyReference = BodyReferenceNode();
        }

        protected XmlNode DocumentNode(string xPath)
        {
            var rot = _document.DocumentElement;
            var targetNode = rot.SelectSingleNode(xPath, _namespaceManager);

            return targetNode;
        }

        protected XmlNode BodyReferenceNode()
        {
            var rot = _document.DocumentElement;
            
            var partInfo = rot.SelectSingleNode("//ns6:PartInfo", _namespaceManager);
            var partInfoBodyId = "";
            if (partInfo.Attributes.Count > 0)
                partInfoBodyId = partInfo.Attributes["href"].Value;

            var bodyId = rot.SelectSingleNode("//env:Body", _namespaceManager).Attributes["wsu:Id"].Value;

            if (!partInfoBodyId.Equals(String.Empty) && !bodyId.Equals(partInfoBodyId))
            {
                throw new Exception("Id i PartInfo og i Body matcher ikke.");
            }
            
            var bodyReferenceNode = rot.SelectSingleNode("//ns5:Reference[@URI = '#" + bodyId + "']", _namespaceManager);

            return bodyReferenceNode;
        }
    }
}
