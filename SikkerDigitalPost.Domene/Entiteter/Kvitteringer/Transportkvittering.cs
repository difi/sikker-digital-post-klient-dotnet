using System;
using System.Xml;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public abstract class Transportkvittering
    {
        private readonly XmlDocument _document;
        private readonly XmlNamespaceManager _namespaceManager;

        public DateTime Tidspunkt { get; protected set; }

        public string MeldingsId { get; protected set; }

        public string ReferanseTilMeldingsId { get; set; }

        public string Rådata { get; set; }

        protected Transportkvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            try
            {
                _document = document;
                _namespaceManager = namespaceManager;
                Tidspunkt = Convert.ToDateTime(DocumentNode("//ns6:Timestamp").InnerText);
                MeldingsId = DocumentNode("//ns6:MessageId").InnerText;
                Rådata = document.OuterXml;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                   String.Format("Feil under bygging av {0} (av type Transportkvittering). Klarte ikke finne alle felter i xml."
                   , GetType()), e);
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
                    String.Format("Feil under henting av dokumentnode i {0} (av type Transportkvittering). Klarte ikke finne alle felter i xml."
                    , GetType()), e);
            }
        }
    }
}
