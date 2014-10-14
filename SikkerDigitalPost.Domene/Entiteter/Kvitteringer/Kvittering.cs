using System;
using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Kvittering
    {
        public DateTime Tidspunkt { get; protected set; }

        protected static XmlNode DocumentNode(XmlDocument document, XmlNamespaceManager namespaceManager, string xPath)
        {
            var rot = document.DocumentElement;
            var targetNode = rot.SelectSingleNode(xPath, namespaceManager);

            return targetNode;
        }
    }
}
