using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class TransportOkKvittering : Transportkvittering
    {
        public TransportOkKvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
        }
    }
}
