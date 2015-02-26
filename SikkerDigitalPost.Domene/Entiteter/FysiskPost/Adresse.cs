using System;

namespace SikkerDigitalPost.Domene.Entiteter.FysiskPost
{
    public abstract class Adresse
    {
        public string Adresselinje1 { get; set; }

        public string Adresselinje2 { get; set; }

        public string Adresselinje3 { get; set; }

        public string AdresseLinje(int index1)
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
                    throw new ArgumentOutOfRangeException("index1", "Postadrese har bare adresselinje 1, 2 og 3");
            }
        }

    }
}
