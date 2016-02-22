﻿using System;
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

        public DateTime Feilet => Generert;

        public VarslingFeiletKvittering(string meldingsId, Guid konversasjonsId, string bodyReferenceUri, string digestValue) : base(meldingsId, konversasjonsId, bodyReferenceUri, digestValue)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}, Varslingskanal: {1}, Beskrivelse: {2}, Feilet: {3}", base.ToString(), Varslingskanal, Beskrivelse, Feilet);
        }
    }
}
