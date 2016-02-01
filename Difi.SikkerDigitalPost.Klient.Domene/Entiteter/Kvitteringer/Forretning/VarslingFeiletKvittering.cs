using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// Sendes fra Postkasse til Avsender dersom Postkasse opplever problemer med å utføre varslingen som spesifisert i meldingen.
    /// Les mer på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/VarslingfeiletKvittering.
    /// </summary>
    public class VarslingFeiletKvittering : Forretningskvittering
    {
        /// <summary>
        /// Kanal for varsling til eier av postkasse. Varsling og påminnelsesmeldinger skal sendes på den kanal som blir spesifisert. Kanalen SMS er priset.
        /// </summary>
        public Varslingskanal Varslingskanal { get; set; }
        
        public string Beskrivelse { get; set; }

        public VarslingFeiletKvittering() { }

        public VarslingFeiletKvittering(Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public DateTime Feilet
        {
            get { return Generert; }
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nFeilet: {2}. \nVarslingskanal: {3}. \nBeskrivelse: {4}. \nKonversasjonsId: {5}. \nRefererer til melding med id: {6}", 
                GetType().Name, MeldingsId, Feilet, Varslingskanal, Beskrivelse, KonversasjonsId, ReferanseTilMeldingId);
        }
    }
}
