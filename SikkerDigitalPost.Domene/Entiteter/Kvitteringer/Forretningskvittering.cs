using System;
using System.Xml;
using SikkerDigitalPost.Domene.Exceptions;

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
            try
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
            catch (Exception e)
            {
                throw new XmlParseException(
                    String.Format("Feil under bygging av {0} (av type Forretningskvittering). Klarte ikke finne alle felter i xml."
                    ,GetType()), e);
            }
        }

        protected XmlNode DocumentNode(string xPath)
        {
            try
            {
                var rot = _document.DocumentElement;
                var targetNode = rot.SelectSingleNode(xPath, _namespaceManager);

                return targetNode;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    String.Format("Feil under henting av dokumentnode i {0} (av type Forretningskvittering). Klarte ikke finne alle felter i xml."
                    , GetType()), e);
            }
        }
        
        protected XmlNode BodyReferenceNode()
        {
            XmlNode bodyReferenceNode;

            try
            {
                XmlNode rotnode = _document.DocumentElement;

                var partInfo = rotnode.SelectSingleNode("//ns6:PartInfo", _namespaceManager);
                var partInfoBodyId = String.Empty;
                if (partInfo.Attributes.Count > 0)
                    partInfoBodyId = partInfo.Attributes["href"].Value;

                string bodyId = rotnode.SelectSingleNode("//env:Body", _namespaceManager).Attributes["wsu:Id"].Value;

                if (!partInfoBodyId.Equals(String.Empty) && !bodyId.Equals(partInfoBodyId))
                {
                    throw new Exception(
                        String.Format(
                        "Id i PartInfo og i Body matcher er ikke like. Partinfo har '{0}', body har '{1}'",
                        partInfoBodyId,
                        bodyId));
                }

                bodyReferenceNode = rotnode.SelectSingleNode("//ns5:Reference[@URI = '#" + bodyId + "']",
                    _namespaceManager);
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                  String.Format("Feil under henting av referanser i {0} (av type Forretningskvittering). ",
                  GetType()), e);
            }

            return bodyReferenceNode;
        }
    }
}
