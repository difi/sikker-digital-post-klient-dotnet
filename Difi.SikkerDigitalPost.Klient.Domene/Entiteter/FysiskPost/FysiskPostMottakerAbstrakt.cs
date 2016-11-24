using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
{
    /// <summary>
    ///     En mottaker av fysisk post.
    /// </summary>
    public abstract class FysiskPostMottakerAbstrakt : PostMottaker
    {
        /// <summary>
        ///     Informasjon om mottaker av fysisk post.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        /// <param name="utskriftstjenesteSertifikat">Sertifikat for utskriftstjenesten.</param>
        /// <param name="organisasjonsnummer">
        ///     Identifikator (organisasjonsnummer) til virksomheten som er sluttmottaker i
        ///     meldingsprosessen.
        /// </param>
        protected FysiskPostMottakerAbstrakt(string navn, Adresse adresse, X509Certificate2 utskriftstjenesteSertifikat, Organisasjonsnummer organisasjonsnummer)
            : base(utskriftstjenesteSertifikat, organisasjonsnummer)
        {
            Navn = navn;
            Adresse = adresse;
        }

        /// <summary>
        ///     Informasjon om mottaker av fysisk post. Kun for returmottaker, da utskriftstjenesteSertifikat og
        ///     organisasjonsnummer til postkasse ikke blir satt.
        ///     Bruk overload med utskriftstjenesteSertifkat og organisasjonsnummer for Mottaker.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        protected FysiskPostMottakerAbstrakt(string navn, Adresse adresse)
            : this(navn, adresse, new X509Certificate2(), new Organisasjonsnummer("000000000"))
        {
        }

        /// <summary>
        ///     Fullt navn på mottaker av fysisk post.
        /// </summary>
        public string Navn { get; set; }

        /// <summary>
        ///     Adresse for mottaker av fysisk post.
        /// </summary>
        public Adresse Adresse { get; set; }
    }
}