namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
{
    public abstract class Adresse
    {
        public string Adresselinje1 { get; set; }

        public string Adresselinje2 { get; set; }

        public string Adresselinje3 { get; set; }

        public abstract string AdresseLinje(int index1);
    }
}