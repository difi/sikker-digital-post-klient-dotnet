using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Domene.Entiteter.Aktører
{
    public abstract class PostMottaker
    {
        public X509Certificate2 Sertifikat { get; internal set; }
        public Organisasjonsnummer OrganisasjonsnummerPostkasse { get; internal set; }

        protected PostMottaker(X509Certificate2 sertifikat, Organisasjonsnummer organisasjonsnummer)
        {
            Sertifikat = sertifikat;
            OrganisasjonsnummerPostkasse = organisasjonsnummer;
        }

        protected PostMottaker(X509Certificate2 sertifikat, string organisasjonsnummer) : this(sertifikat, new Organisasjonsnummer(organisasjonsnummer))
        {
        }
        
    }
}