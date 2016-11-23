using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
{
    public class FysiskPostReturmottaker : FysiskPostMottakerAbstrakt
    {
        /// <summary>
        ///     Informasjon om mottaker av fysisk post. Kun for returmottaker, da utskriftstjenesteSertifikat og
        ///     organisasjonsnummer til postkasse ikke blir satt.
        ///     Bruk overload med utskriftstjenesteSertifkat og organisasjonsnummer for Mottaker.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        public FysiskPostReturmottaker(string navn, Adresse adresse)
            : base(navn, adresse, new X509Certificate2(), new Organisasjonsnummer("000000000"))
        {
        }
    }
}