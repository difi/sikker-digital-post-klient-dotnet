using System;
using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// Denne Kvitteringen leveres tilbake så fort utskrift og forsendelsestjenesten har mottatt forsendelsen og validert at den kan skrives ut. Forsendelsen vil så legges i kø og tas med i neste utskriftsjobb for denne type post.
    /// Les mer på http://begrep.difi.no/SikkerDigitalPost/1.2.0/meldinger/MottaksKvittering
    /// </summary>
    public class Mottakskvittering : Forretningskvittering
    {
        public Mottakskvittering(Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public DateTime Mottatt
        {
            get { return Generert; }
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nMottatt: {2}. \nKonversasjonsId: {3}. \nRefererer til melding med id: {4}",
                GetType().Name, MeldingsId, Mottatt, KonversasjonsId, ReferanseTilMeldingId);
        }
    }
}
