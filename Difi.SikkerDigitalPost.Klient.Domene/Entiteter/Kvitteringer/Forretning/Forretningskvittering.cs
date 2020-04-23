using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    ///     Abstrakt klasse for forretningskvitteringer.
    /// </summary>
    public abstract class Forretningskvittering : Kvittering
    {
        protected Forretningskvittering(string meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue, long integrasjonspunktId = -1L)
            : base(meldingsId)
        {
            KonversasjonsId = konversasjonsId;
            BodyReferenceUri = bodyReferenceUri;
            DigestValue = digestValue;
            IntegrasjonsPunktId = integrasjonspunktId;
        }

        /// <summary>
        ///     Identifiserer en melding og tilhørende kvitteringer unikt.
        /// </summary>
        public Guid KonversasjonsId { get; internal set; }

        public long IntegrasjonsPunktId { get; set; }
        
        public string BodyReferenceUri { get; internal set; }

        public string DigestValue { get; internal set; }

        public DateTime Generert { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, KonversasjonsId: {KonversasjonsId},  BodyReferenceUri: {BodyReferenceUri}, DigestValue: {DigestValue}";
        }
    }
}