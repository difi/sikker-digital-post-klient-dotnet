using System;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    ///     En kvitteringsmelding til Avsender om at Mottaker har åpnet forsendelsen i sin postkasse.
    ///     Mer informasjon finnes på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/AapningsKvittering.
    /// </summary>
    public class Åpningskvittering : Forretningskvittering
    {
        public Åpningskvittering(string meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue)
            : base(meldingsId, konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public DateTime Åpnet => Generert;

        public override string ToString()
        {
            return $"{base.ToString()}, Åpnet: {Åpnet.ToStringWithUtcOffset()}";
        }
    }
}