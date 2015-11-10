using System;
using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{
    public class TomKøKvittering : Transportkvittering
    {
        public TomKøKvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
        }
        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nRefererer til melding med id: {3}",
                GetType().Name, MeldingsId, Tidspunkt, ReferanseTilMeldingId);
        }
    }
}
