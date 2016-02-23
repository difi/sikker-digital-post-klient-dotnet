using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    ///     Sendes fra Postkasse til Avsender dersom Postkasse opplever problemer med å utføre varslingen som spesifisert i
    ///     meldingen.
    ///     Les mer på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/VarslingfeiletKvittering.
    /// </summary>
    public class VarslingFeiletKvittering : Forretningskvittering
    {
        public VarslingFeiletKvittering(string meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue)
            : base(meldingsId, konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        /// <summary>
        ///     Kanal for varsling til eier av postkasse. Varsling og påminnelsesmeldinger skal sendes på den kanal som blir
        ///     spesifisert. Kanalen SMS er priset.
        /// </summary>
        public Varslingskanal Varslingskanal { get; set; }

        public string Beskrivelse { get; set; }

        public DateTime Feilet => Generert;

        public override string ToString()
        {
            return $"{base.ToString()}, Varslingskanal: {Varslingskanal}, Beskrivelse: {Beskrivelse}, Feilet: {Feilet}";
        }
    }
}