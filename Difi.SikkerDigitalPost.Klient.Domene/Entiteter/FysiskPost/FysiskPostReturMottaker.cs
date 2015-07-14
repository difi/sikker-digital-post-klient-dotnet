using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
{
    public class FysiskPostReturMottaker : FysiskPostMottakerAbstrakt
    {
        /// <summary>
        /// Informasjon om mottaker av fysisk post.
        /// </summary>
        /// <param name="navn">Fullt navn på returmottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for returmottaker av fysisk post.</param>
        /// <param name="utskriftstjenesteSertifikat">Sertifikat for utskriftstjenesten.</param>
        /// <param name="organisasjonsnummer">Identifikator (organisasjonsnummer) til virksomheten som er slutt returmottaker i meldingsprosessen.</param>
        public FysiskPostReturMottaker(string navn, Adresse adresse, X509Certificate2 utskriftstjenesteSertifikat, string organisasjonsnummer) : base(navn, adresse, utskriftstjenesteSertifikat, organisasjonsnummer) { }

         /// <summary>
        /// Informasjon om mottaker av fysisk post.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        /// <param name="sertifikatThumbprint">Thumbprint til mottakersertifikatet. Se guide på http://difi.github.io/sikker-digital-post-klient-dotnet/#mottakersertifikat </param>
        /// <param name="organisasjonsnummer">Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i meldingsprosessen.</param>
        public FysiskPostReturMottaker(string navn, Adresse adresse, string sertifikatThumbprint, string organisasjonsnummer)
            : base(navn,adresse,sertifikatThumbprint, organisasjonsnummer)
        {
        }

        /// <summary>
        ///  Informasjon om mottaker av fysisk post. Kun for returmottaker, da utskriftstjenesteSertifikat og organisasjonsnummer til postkasse ikke blir satt.
        /// Bruk overload med utskriftstjenesteSertifkat og organisasjonsnummer for Mottaker.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        public FysiskPostReturMottaker(string navn, Adresse adresse)
            : base(navn, adresse, new X509Certificate2(), "0000000000000")
        {
        }
    }
}
