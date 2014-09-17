namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class TekniskAvsender
    {
        public string Organisasjonsnummer { get; set; }
        public Nøkkelpar Nøkkelpar { get; set; }

        public TekniskAvsender(string organisasjonsnummer, Nøkkelpar nøkkelpar)
        {
            Organisasjonsnummer = organisasjonsnummer;
            Nøkkelpar = nøkkelpar;
        }
    }
}
