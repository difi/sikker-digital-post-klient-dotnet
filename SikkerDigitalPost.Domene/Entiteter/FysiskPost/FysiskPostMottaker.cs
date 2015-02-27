using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Domene.Entiteter.Aktører;

namespace SikkerDigitalPost.Domene.Entiteter.FysiskPost
{
    public class FysiskPostMottaker : PostMottaker
    {
        /// <summary>
        /// Fullt navn på mottaker av fysisk post.
        /// </summary>
        public string Navn { get; set; }

        /// <summary>
        /// Adresse for mottaker av fysisk post.
        /// </summary>
        public Adresse Adresse { get; set; }

        /// <summary>
        /// Informasjon om mottaker av fysisk post.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        /// <param name="utskriftstjenesteSertifikat">Sertifikat for utskriftstjenesten.</param>
        /// <param name="organisasjonsnummer">Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.</param>
        public FysiskPostMottaker(string navn, Adresse adresse, X509Certificate2 utskriftstjenesteSertifikat, string organisasjonsnummer)
            : base(utskriftstjenesteSertifikat, organisasjonsnummer)
        {
            Navn = navn;
            Adresse = adresse;
        }

        /// <summary>
        ///  Informasjon om mottaker av fysisk post. KUN FOR TESTING, da utskriftstjenesteSertifikat og organisasjonsnummer til postkasse ikke blir satt.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        public FysiskPostMottaker(string navn, Adresse adresse)
            : this(navn, adresse, null, "0000000000000")
        {
        }
    }
}
