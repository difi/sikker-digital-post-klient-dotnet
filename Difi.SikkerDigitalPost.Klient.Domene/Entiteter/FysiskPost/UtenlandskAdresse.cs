using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
{
    public class UtenlandskAdresse : Adresse
    {
        /// <summary>
        ///     Utenlandsk adresse
        /// </summary>
        /// <param name="land">
        ///     Landkode eller navn på mottakerland (f.eks. 'SE' eller 'Sverige')
        ///     Om land skrives feil, blir porto satt til 'verden'.
        /// </param>
        /// <param name="adresselinje1">Første adresselinje er obligatorisk for utenlandsk adresse.</param>
        public UtenlandskAdresse(string land, string adresselinje1)
        {
            if (land.Length > 2)
                Land = land;
            else
                Landkode = land;

            Adresselinje1 = adresselinje1;
        }

        public string Landkode { get; set; }

        public string Land { get; set; }

        public string Adresselinje4 { get; set; }

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
                case 4:
                    return Adresselinje4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index1), "Utenlandsk postadrese har bare adresselinje 1, 2, 3 og 4");
            }
        }
    }
}