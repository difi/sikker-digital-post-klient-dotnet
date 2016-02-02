using System;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// En kvittering som Avsender kan oppbevare som garanti på at posten er levert til Mottaker. 
    /// Kvitteringen sendes fra Postkassleverandør når postforsendelsen er validert og de garanterer for at posten vil bli tilgjengeliggjort.
    /// Les mer på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/LeveringsKvittering.
    /// </summary>
    public class Leveringskvittering : Forretningskvittering
    {
        public DateTime Levert {get { return Generert; } }

        public Leveringskvittering(Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public override string ToString()
        {
            return string.Format("Levert: {0}, {1}", Levert.ToStringWithUtcOffset(), base.ToString());
        }
    }
}
