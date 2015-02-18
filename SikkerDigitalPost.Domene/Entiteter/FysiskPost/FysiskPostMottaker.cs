using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Domene.Entiteter.Aktører;

namespace SikkerDigitalPost.Domene.Entiteter.FysiskPost
{
    public class FysiskPostMottaker : PostMottaker
    {
        public string Navn { get; set; }

        public NorskAdresse NorskAdresse { get; set; }

        public FysiskPostMottaker(string navn, NorskAdresse norskAdresse, X509Certificate2 sertifikat, string organisasjonsnummer) :base(sertifikat, organisasjonsnummer)
        {
            Navn = navn;
            NorskAdresse = norskAdresse;
        }

        public FysiskPostMottaker(string navn, NorskAdresse norskAdresse) : this(navn,norskAdresse, null,"0000000000000")
        {
        }
    }
}
