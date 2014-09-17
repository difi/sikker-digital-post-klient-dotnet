using SikkerDigitalPost.Net.Domene.Entiteter.Abstrakt;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class SmsVarsel : Varsel
    {
        public readonly string Mobilnummer;

        public SmsVarsel(string mobilnummer,string varslingstekst) : base(varslingstekst)
        {
            Mobilnummer = mobilnummer;
        }
    }
}
