using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

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

        public Feilmelding(String meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(meldingsId, konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Skyldig: {1}, Detaljer: {2}, Feilet: {3}", base.ToString(), Skyldig, Detaljer, Feilet.ToStringWithUtcOffset());
        }
    }
}
