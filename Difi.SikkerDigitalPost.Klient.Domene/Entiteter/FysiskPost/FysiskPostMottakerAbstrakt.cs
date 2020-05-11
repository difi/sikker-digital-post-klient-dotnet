using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
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
        [Obsolete(message:"Instead use FysiskPostMottakerAbstrakt(string navn, Adresse adresse)")]
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

        [JsonPropertyName("navn")]
        /// <summary>
        ///     Fullt navn på mottaker av fysisk post.
        /// </summary>
        public string Navn { get; set; }

        /// <summary>
        ///     Adresse for mottaker av fysisk post.
        /// </summary>
        [JsonIgnore]
        public Adresse Adresse { get; set; }
        
        public string adresselinje1 => Adresse.Adresselinje1;
        public string adresselinje2 => Adresse.Adresselinje2;
        public string adresselinje3 => Adresse.Adresselinje3;
        public string adresselinje4 => Adresse is UtenlandskAdresse ? (Adresse as UtenlandskAdresse).Adresselinje4 : null;
        public string postnummer => Adresse is NorskAdresse ? (Adresse as NorskAdresse).Postnummer : null;
        public string poststed => Adresse is NorskAdresse ? (Adresse as NorskAdresse).Poststed : null;
        public string land => Adresse is UtenlandskAdresse ? (Adresse as UtenlandskAdresse).Land : "Norway";
        public string landkode => Adresse is UtenlandskAdresse ? (Adresse as UtenlandskAdresse).Landkode : "NO";
    }
}
