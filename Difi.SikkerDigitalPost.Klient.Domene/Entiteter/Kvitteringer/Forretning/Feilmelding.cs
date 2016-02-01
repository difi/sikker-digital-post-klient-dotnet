using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// En feilmelding fra postkasseleverandør med informasjon om en forretningsfeil knyttet til en digital post forsendelse.
    /// Les mer på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/FeilMelding.
    /// </summary>
    public class Feilmelding : Forretningskvittering
    {
        public Feiltype Skyldig { get; set; }

        public string Detaljer { get; set; }

        public DateTime Feilet { get { return Generert; } }

        public Feilmelding(Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public new string ToString()
        {
            return string.Format("Skyldig: {0}, Detaljer: {1}, Feilet: {2}, {3}", Skyldig, Detaljer, Feilet.ToStringWithUtcOffset(), base.ToString());
        }
    }
}
