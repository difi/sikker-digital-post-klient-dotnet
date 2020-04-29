using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører
{
    public abstract class PostMottaker
    {
        protected PostMottaker()
        {
        }

        [Obsolete]
        protected PostMottaker(X509Certificate2 sertifikat, Organisasjonsnummer organisasjonsnummer)
        {
            Sertifikat = sertifikat;
            OrganisasjonsnummerPostkasse = organisasjonsnummer;
        }
        
        
        [Obsolete]
        [JsonIgnore]
        public X509Certificate2 Sertifikat { get; set; }

        [Obsolete]
        [JsonIgnore]
        public Organisasjonsnummer OrganisasjonsnummerPostkasse { get; internal set; }
    }
}
