using System;
using SikkerDigitalPost.Net.Domene.Entiteter.Varsel;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class DigitalPost
    {
        public Mottaker Mottaker { get; set; }
        public DateTime Virkningsdato { get; set; }
        public bool Åpningskvittering { get; set; }
        public Sikkerhetsnivå Sikkerhetsnivå { get; set; }
        public string IkkeSensitivTittel { get; set; }
        public EpostVarsel EpostVarsel { get; set; }
        public SmsVarsel SmsVarsel { get; set; }
    }
}
