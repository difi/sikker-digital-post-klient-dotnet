namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
{
    public class Printinstruksjon
    {
        public Printinstruksjon(string navn, string verdi)
        {
            Navn = navn;
            Verdi = verdi;
        }

        public string Navn { get; set; }

        public string Verdi { get; set; }
    }
}