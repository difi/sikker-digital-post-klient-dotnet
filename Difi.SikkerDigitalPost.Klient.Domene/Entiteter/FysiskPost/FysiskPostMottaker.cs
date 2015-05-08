using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
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
        /// Informasjon om mottaker av fysisk post.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        /// <param name="sertifikatThumbprint">Thumbprint til mottakersertifikatet. Se guide på http://difi.github.io/sikker-digital-post-klient-dotnet/#mottakersertifikat </param>
        /// <param name="organisasjonsnummer">Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.</param>
        public FysiskPostMottaker(string navn, Adresse adresse, string sertifikatThumbprint, string organisasjonsnummer)
            : base(sertifikatThumbprint, organisasjonsnummer)
        {
            Navn = navn;
            Adresse = adresse;
        }

        /// <summary>
        ///  Informasjon om mottaker av fysisk post. Kun for returmottaker, da utskriftstjenesteSertifikat og organisasjonsnummer til postkasse ikke blir satt.
        /// Bruk overload med utskriftstjenesteSertifkat og organisasjonsnummer for Mottaker.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        public FysiskPostMottaker(string navn, Adresse adresse)
            : this(navn, adresse, new X509Certificate2(), "0000000000000")
        {
        }
    }
}
