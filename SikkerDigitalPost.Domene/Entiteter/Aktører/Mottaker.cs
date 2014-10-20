using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Domene.Entiteter.Aktører
{
    /// <summary>
    /// Mottaker av en digital postmelding.
    /// </summary>
    public class Mottaker : Person
    {
        public X509Certificate2 Sertifikat { get; private set; }
        public Organisasjonsnummer OrganisasjonsnummerPostkasse { get; private set; }

        /// <summary>
        /// Informasjon om mottaker. Vil vanligvis være hentet fra http://begrep.difi.no/Oppslagstjenesten/.
        /// </summary>
        /// <param name="personidentifikator">Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.</param>
        /// <param name="postkasseadresse">Mottakerens adresse hos postkasseleverandøren.</param>
        /// <param name="sertifikat">Mottakerens sertifikat.</param>
        /// <param name="organisasjonsnummerPostkasse">Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.</param>
        public Mottaker(string personidentifikator, string postkasseadresse, X509Certificate2 sertifikat, Organisasjonsnummer organisasjonsnummerPostkasse) : base(personidentifikator, postkasseadresse)
        {
            Sertifikat = sertifikat;
            OrganisasjonsnummerPostkasse = organisasjonsnummerPostkasse;
        }


        /// <summary>
        /// Informasjon om mottaker. Vil vanligvis være hentet fra http://begrep.difi.no/Oppslagstjenesten/.
        /// </summary>
        /// <param name="personidentifikator">Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.</param>
        /// <param name="postkasseadresse">Mottakerens adresse hos postkasseleverandøren.</param>
        /// <param name="sertifikat">Mottakerens sertifikat.</param>
        /// <param name="organisasjonsnummerPostkasse">Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.</param>
        public Mottaker(string personidentifikator, string postkasseadresse, X509Certificate2 sertifikat, string organisasjonsnummerPostkasse)
            : this(personidentifikator,postkasseadresse,sertifikat,new Organisasjonsnummer(organisasjonsnummerPostkasse))
        {
        }
    }
}
