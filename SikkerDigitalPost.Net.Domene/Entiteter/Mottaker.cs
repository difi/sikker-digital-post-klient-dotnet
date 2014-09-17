using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.Domene
{
    public class Mottaker : Person
    {
        public Sertifikat MottakerSerfifikat { get; set; }
        public string OrganisasjonsnummerPostkasse { get; set; }

        public Mottaker(string personidentifikator, string postkasseadresse, Sertifikat mottakerSerfifikat, string organisasjonsnummerPostkasse) : base(personidentifikator,postkasseadresse) 
        {
            MottakerSerfifikat = mottakerSerfifikat;
            OrganisasjonsnummerPostkasse = organisasjonsnummerPostkasse;
        }
    }
}
