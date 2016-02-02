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
        /// <summary>
        /// Identifiserer en melding og tilhørende kvitteringer unikt.
        /// </summary>
        public Guid KonversasjonsId { get; }

        public string BodyReferenceUri { get; }

        public string DigestValue { get; }

       public DateTime Generert { get; set; }

        protected Forretningskvittering(Guid konversasjonsId, string bodyReferenceUri, string digestValue)
        {
            KonversasjonsId = konversasjonsId;
            BodyReferenceUri = bodyReferenceUri;
            DigestValue = digestValue;
        }

        public new string ToString()
        {
            return string.Format("BodyReferenceUri: {0}, DigestValue: {1}, {2}", BodyReferenceUri, DigestValue, base.ToString());
        }
    }
}
