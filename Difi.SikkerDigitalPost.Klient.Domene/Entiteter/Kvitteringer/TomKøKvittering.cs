using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;


namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer
{
    public class TomKøKvittering : Transportkvittering
    {
        public TomKøKvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
        }
        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nRefererer til melding med id: {4}",
                GetType().Name, MeldingsId, Tidspunkt, ReferanseTilMeldingId);
        }
    }
}
