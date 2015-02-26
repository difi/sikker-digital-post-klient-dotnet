using System;

namespace SikkerDigitalPost.Domene.Entiteter.FysiskPost
{
    public class UtenlandskAdresse : Adresse
    {
        public string Landkode { get; set; }
        public string Land { get; set; }
        public string Adresselinje4 { get; set; }

        /// <summary>
        /// Utenlandsk adresse
        /// </summary>
        /// <param name="land">Landkode eller navn på mottakerland (f.eks. 'SE' eller 'Sverige')
        ///  Om land skrives feil, blir porto satt til 'verden'.</param>
        /// <param name="adresselinje1">Første adresselinje er obligatorisk for utenlandsk adresse.</param>
        public UtenlandskAdresse(string land, string adresselinje1)
        {
            if (land.Length > 2)
                Land = land;
            else
                Landkode = land;

            Adresselinje1 = adresselinje1;
        }

        public string AdresseLinje(int index1)
        {
            if(index1 > 4)
                throw new ArgumentOutOfRangeException("index1", "Utenlandsk postadresse har bare indeks 1, 2, 3 og 4");

            if (index1 == 4)
                return Adresselinje4;

            return base.AdresseLinje(index1);
        }

    }
}
