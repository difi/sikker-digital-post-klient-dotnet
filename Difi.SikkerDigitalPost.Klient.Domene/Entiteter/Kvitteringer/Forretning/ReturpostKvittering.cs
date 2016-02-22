using System;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// Dette er Kvittering på at posten har kommet i retur og har blitt makulert.
    /// Les mer på http://begrep.difi.no/SikkerDigitalPost/1.2.0/meldinger/ReturpostKvittering
    /// </summary>
    public class Returpostkvittering : Forretningskvittering
    {
        public DateTime Returnert => Generert;

        public Returpostkvittering(string meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(meldingsId, konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Returnert: {1}", base.ToString(), Returnert.ToStringWithUtcOffset());
        }
    }

}

