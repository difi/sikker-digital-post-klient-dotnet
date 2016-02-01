using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// Abstrakt klasse for forretningskvitteringer.
    /// </summary>
    public abstract class Forretningskvittering : Kvittering
    {
        public string BodyReferenceUri { get; set; }

        public string DigestValue { get; set; }

        public DateTime Generert { get; set; }

        /// <summary>
        /// Identifiserer en melding og tilhørende kvitteringer unikt.
        /// </summary>
        public Guid KonversasjonsId { get; set; }

        /// <summary>
        /// Alle subklasser skal ha en ToString() som beskriver kvitteringen.
        /// </summary>
        public abstract override string ToString();

        protected Forretningskvittering() { }

        protected Forretningskvittering(Guid konversasjonsId, string bodyReferenceUri, string digestValue)
        {
            KonversasjonsId = konversasjonsId;
            BodyReferenceUri = bodyReferenceUri;
            DigestValue = digestValue;
        }
    }
}
