using System;
using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public abstract class Forretningskvittering
    {
        private readonly XmlDocument _document;
        private readonly XmlNamespaceManager _namespaceManager;
        
        public DateTime Tidspunkt { get; protected set; }

        public readonly string KonversasjonsId;

        public readonly string MeldingsId;

        public readonly string RefToMessageId;
        
        internal XmlNode BodyReference { get; set; }

        public readonly string Rådata;

        protected Forretningskvittering()
        {
            
        }

        protected Forretningskvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            _document = document;
            _namespaceManager = namespaceManager;

            Tidspunkt = Convert.ToDateTime(DocumentNode("//ns6:Timestamp").InnerText);
            KonversasjonsId = DocumentNode("//ns3:BusinessScope/ns3:Scope/ns3:InstanceIdentifier").InnerText;
            MeldingsId = DocumentNode("//ns6:MessageId").InnerText;
            RefToMessageId = DocumentNode("//ns6:RefToMessageId").InnerText;
            BodyReference = BodyReferenceNode();
            Rådata = document.OuterXml;
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
