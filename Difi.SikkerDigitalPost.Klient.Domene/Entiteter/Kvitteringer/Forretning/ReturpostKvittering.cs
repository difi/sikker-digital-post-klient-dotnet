using System;
using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    public class Returpostkvittering : Forretningskvittering
    {
        internal Returpostkvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager)
            : base(xmlDocument, namespaceManager)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nKonversasjonsId: {3}. \nRefererer til melding med id: {4}",
                GetType().Name, MeldingsId, Tidspunkt, KonversasjonsId, ReferanseTilMeldingId);
        }
    }

}

