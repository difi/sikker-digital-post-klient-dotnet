using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Mottaker : Person
    {
        public X509Certificate2 MottakerSerfifikat { get; set; }
        public string OrganisasjonsnummerPostkasse { get; set; }

        public Mottaker(string personidentifikator, string postkasseadresse, X509Certificate2 mottakerSerfifikat, string organisasjonsnummerPostkasse) : base(personidentifikator, postkasseadresse)
        {
            MottakerSerfifikat = mottakerSerfifikat;
            OrganisasjonsnummerPostkasse = organisasjonsnummerPostkasse;
        }
    }
}
