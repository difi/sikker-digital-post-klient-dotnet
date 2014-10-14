using System;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class VarslingFeiletKvittering : Forretningskvittering
    {
        public readonly Varslingskanal Varslingskanal;
        public string Beskrivelse { get; set; }

        public VarslingFeiletKvittering(DateTime tidspunkt, Varslingskanal varslingskanal)
        {
            Tidspunkt = tidspunkt;
            Varslingskanal = varslingskanal;
        }
    }
}
