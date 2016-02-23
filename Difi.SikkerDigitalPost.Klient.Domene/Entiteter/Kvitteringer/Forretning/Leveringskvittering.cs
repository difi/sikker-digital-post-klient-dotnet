using System;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    ///     En kvittering som Avsender kan oppbevare som garanti på at posten er levert til Mottaker.
    ///     Kvitteringen sendes fra Postkassleverandør når postforsendelsen er validert og de garanterer for at posten vil bli
    ///     tilgjengeliggjort.
    ///     Les mer på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/LeveringsKvittering.
    /// </summary>
    public class Leveringskvittering : Forretningskvittering
    {
        public Leveringskvittering(string meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue)
            : base(meldingsId, konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public DateTime Levert => Generert;

        public override string ToString()
        {
            return $"{base.ToString()}, Levert: {Levert.ToStringWithUtcOffset()}";
        }
    }
}