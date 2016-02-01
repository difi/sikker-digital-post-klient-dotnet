using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

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

        public Feilmelding() { }

        public Feilmelding(Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nFeilet: {2}.. \nSkyldig: {3}. \nDetaljer: {4}. \nKonversasjonsId: {5}. \nRefererer til melding med id: {6}", 
                GetType().Name, MeldingsId, Feilet, Skyldig, Detaljer, KonversasjonsId, ReferanseTilMeldingId);
        }
    }
}
