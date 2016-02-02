using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{

    public abstract class Transportkvittering : Kvittering
    {
        protected Transportkvittering()
        { }

        protected Transportkvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
            : base(document, namespaceManager)
        {
            
        }
    }
}
