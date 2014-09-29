using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Mottaker : Person
    {
        public X509Certificate2 MottakerSerfifikat { get; set; }
        public Organisasjonsnummer OrganisasjonsnummerPostkasse { get; set; }

        /// <summary>
        /// Informasjon om mottaker. Vil vanligvis være hentet fra http://begrep.difi.no/Oppslagstjenesten/.
        /// </summary>
        /// <param name="personidentifikator">Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.</param>
        /// <param name="postkasseadresse">Mottakerens adresse hos postkasseleverandøren.</param>
        /// <param name="mottakerSerfifikat">Mottakerens sertifikat.</param>
        /// <param name="organisasjonsnummerPostkasse">Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.</param>
        public Mottaker(string personidentifikator, string postkasseadresse, X509Certificate2 mottakerSerfifikat, Organisasjonsnummer organisasjonsnummerPostkasse) : base(personidentifikator, postkasseadresse)
        {
            MottakerSerfifikat = mottakerSerfifikat;
            OrganisasjonsnummerPostkasse = organisasjonsnummerPostkasse;
        }


        /// <summary>
        /// Informasjon om mottaker. Vil vanligvis være hentet fra http://begrep.difi.no/Oppslagstjenesten/.
        /// </summary>
        /// <param name="personidentifikator">Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.</param>
        /// <param name="postkasseadresse">Mottakerens adresse hos postkasseleverandøren.</param>
        /// <param name="mottakerSerfifikat">Mottakerens sertifikat.</param>
        /// <param name="organisasjonsnummerPostkasse">Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.</param>
        public Mottaker(string personidentifikator, string postkasseadresse, X509Certificate2 mottakerSerfifikat, string organisasjonsnummerPostkasse)
            : this(personidentifikator,postkasseadresse,mottakerSerfifikat,new Organisasjonsnummer(organisasjonsnummerPostkasse))
        {
        }
    }
}
