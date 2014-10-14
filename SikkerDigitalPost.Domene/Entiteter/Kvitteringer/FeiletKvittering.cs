using System;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Feilmelding : Kvittering
    {
        public readonly Feiltype Feiltype;
        public string Feilkode { get; set; }
        public string Detaljer { get; set; }

        public Feilmelding(DateTime tidspunkt, Feiltype feiltype) : base(tidspunkt)
        {
            Feiltype = feiltype;
        }
    }
}
