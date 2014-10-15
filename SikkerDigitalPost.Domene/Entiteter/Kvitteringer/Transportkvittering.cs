using System;
using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public abstract class Transportkvittering
    {
        private XmlDocument _document;
        private readonly XmlNamespaceManager _namespaceManager;

        public DateTime Tidspunkt { get; protected set; }

        public string MeldingsId { get; protected set; }

        public string ReferanseTilMeldingsId { get; set; }

        public string Rådata { get; set; }

        protected Transportkvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            _document = document;
            _namespaceManager = namespaceManager;
            Tidspunkt = Convert.ToDateTime(DocumentNode("//ns6:Timestamp").InnerText);
            MeldingsId = DocumentNode("//ns6:MessageId").InnerText;
            Rådata = document.OuterXml;
        }

        protected XmlNode DocumentNode(string xPath)
        {
            var rot = _document.DocumentElement;
            var targetNode = rot.SelectSingleNode(xPath, _namespaceManager);

            return targetNode;
        }
    }
}
