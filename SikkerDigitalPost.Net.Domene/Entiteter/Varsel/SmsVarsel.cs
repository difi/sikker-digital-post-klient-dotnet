namespace SikkerDigitalPost.Net.Domene.Entiteter.Varsel
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
