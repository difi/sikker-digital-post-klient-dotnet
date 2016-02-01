using System;
using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// En kvitteringsmelding til Avsender om at Mottaker har åpnet forsendelsen i sin postkasse.
    /// Mer informasjon finnes på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/AapningsKvittering.
    /// </summary>
    public class Åpningskvittering : Forretningskvittering
    {
        public DateTime Åpnet { get { return Generert; } }

        public Åpningskvittering(Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nÅpnet: {2}.  \nKonversasjonsId: {3}. \nRefererer til melding med id: {4}", 
                GetType().Name, MeldingsId, Åpnet, KonversasjonsId, ReferanseTilMeldingId);
        }
    }
}
