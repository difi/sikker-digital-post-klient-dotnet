namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Behandlingsansvarlig
    {
        public readonly string Organisasjonsnummer;
        public string Avsenderidentifikator { get; set; }
        public string Fakturareferanse { get; set; }

        public Behandlingsansvarlig(string organisasjonsnummer)
        {
            Organisasjonsnummer = organisasjonsnummer;
        }
    }
}
