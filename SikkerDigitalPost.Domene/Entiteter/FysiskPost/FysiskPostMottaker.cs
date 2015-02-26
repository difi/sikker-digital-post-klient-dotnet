using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Domene.Entiteter.Aktører;

namespace SikkerDigitalPost.Domene.Entiteter.FysiskPost
{
    public class FysiskPostMottaker : PostMottaker
    {
        public string Navn { get; set; }

        public Adresse Adresse { get; set; }

        public FysiskPostMottaker(string navn, Adresse adresse, X509Certificate2 sertifikat, string organisasjonsnummer)
            : base(sertifikat, organisasjonsnummer)
        {
            Navn = navn;
            Adresse = adresse;
        }

        public FysiskPostMottaker(string navn, Adresse adresse)
            : this(navn, adresse, null, "0000000000000")
        {
        }
    }
}
