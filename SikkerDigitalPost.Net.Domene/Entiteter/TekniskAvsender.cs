namespace SikkerDigitalPost.Net.Domene
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
