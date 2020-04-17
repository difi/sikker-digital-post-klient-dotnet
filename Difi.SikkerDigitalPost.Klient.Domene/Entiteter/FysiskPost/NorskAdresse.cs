using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
{
    public class NorskAdresse : Adresse
    {
        public NorskAdresse(string postnummer, string poststed)
        {
            Postnummer = postnummer;
            Poststed = poststed;
        }

        public string Postnummer { get; set; }

        public string Poststed { get; set; }

        public override string AdresseLinje(int index1)
        {
            switch (index1)
            {
                case 1:
                    return Adresselinje1;
                case 2:
                    return Adresselinje2;
                case 3:
                    return Adresselinje3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index1), "Norsk postadrese har bare adresselinje 1, 2 og 3");
            }
        }
    }
}