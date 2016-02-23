using System;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    ///     Denne Kvitteringen leveres tilbake så fort utskrift og forsendelsestjenesten har mottatt forsendelsen og validert
    ///     at den kan skrives ut. Forsendelsen vil så legges i kø og tas med i neste utskriftsjobb for denne type post.
    ///     Les mer på http://begrep.difi.no/SikkerDigitalPost/1.2.0/meldinger/MottaksKvittering
    /// </summary>
    public class Mottakskvittering : Forretningskvittering
    {
        public Mottakskvittering(string meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue)
            : base(meldingsId, konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public DateTime Mottatt => Generert;

        public override string ToString()
        {
            return $"{base.ToString()}, Mottatt: {Mottatt.ToStringWithUtcOffset()}";
        }
    }
}