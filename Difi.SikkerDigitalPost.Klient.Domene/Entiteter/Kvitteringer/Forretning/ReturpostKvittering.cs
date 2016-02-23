using System;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    ///     Dette er Kvittering på at posten har kommet i retur og har blitt makulert.
    ///     Les mer på http://begrep.difi.no/SikkerDigitalPost/1.2.0/meldinger/ReturpostKvittering
    /// </summary>
    public class Returpostkvittering : Forretningskvittering
    {
        public Returpostkvittering(string meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue)
            : base(meldingsId, konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public DateTime Returnert => Generert;

        public override string ToString()
        {
            return $"{base.ToString()}, Returnert: {Returnert.ToStringWithUtcOffset()}";
        }
    }
}