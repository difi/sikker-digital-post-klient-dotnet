using System;
using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører
{
    /// <summary>
    ///     Mottaker av en digital postmelding.
    /// </summary>
    public class DigitalPostMottaker : PostMottaker
    {
        /// <summary>
        ///     Informasjon om mottaker. Vil vanligvis være hentet fra http://begrep.difi.no/Oppslagstjenesten/.
        /// </summary>
        /// <param name="personidentifikator">Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.</param>
        /// <param name="postkasseadresse">Mottakerens adresse hos postkasseleverandøren.</param>
        public DigitalPostMottaker(string personidentifikator)
        {
            Personidentifikator = personidentifikator;
        }
        
        /// <summary>
        ///     Informasjon om mottaker. Vil vanligvis være hentet fra http://begrep.difi.no/Oppslagstjenesten/.
        /// </summary>
        /// <param name="personidentifikator">Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.</param>
        /// <param name="postkasseadresse">Mottakerens adresse hos postkasseleverandøren.</param>
        /// <param name="sertifikat">Mottakerens sertifikat.</param>
        /// <param name="organisasjonsnummerPostkasse">
        ///     Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.
        /// </param>
        [Obsolete]
        public DigitalPostMottaker(string personidentifikator, string postkasseadresse, X509Certificate2 sertifikat, Organisasjonsnummer organisasjonsnummerPostkasse)
            : base(sertifikat, organisasjonsnummerPostkasse)
        {
            Personidentifikator = personidentifikator;
            Postkasseadresse = postkasseadresse;
        }

        /// <summary>
        ///     Identifikator (fødselsnummer eller D-nummer) til mottaker av brevet.
        /// </summary>
        public string Personidentifikator { get; set; }

        /// <summary>
        ///     Mottakerens adresse hos postkasseleverandøren.
        /// </summary>
        public string Postkasseadresse { get; set; }
    }
}
