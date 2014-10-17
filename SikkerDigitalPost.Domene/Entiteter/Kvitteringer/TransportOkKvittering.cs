using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    /// <summary>
    /// Kvittering fra meldingsformidler som indikerer at denne har overtatt ansvaret for videre formidling av meldingen.
    /// </summary>
    public class TransportOkKvittering : Transportkvittering
    {
        public TransportOkKvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
        }
    }
}
