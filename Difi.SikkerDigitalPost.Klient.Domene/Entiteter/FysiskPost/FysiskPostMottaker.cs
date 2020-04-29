using System;
using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost
{
    public class FysiskPostMottaker : FysiskPostMottakerAbstrakt
    {
        /// <summary>
        ///     Informasjon om mottaker av fysisk post.
        /// </summary>
        /// <param name="navn">Fullt navn på mottaker av fysisk post.</param>
        /// <param name="adresse">Adresse for mottaker av fysisk post.</param>
        public FysiskPostMottaker(string navn, Adresse adresse) : base(navn, adresse)
        {
        }

        [Obsolete]
        public FysiskPostMottaker(string navn, Adresse adresse, X509Certificate2 utskriftstjenesteSertifikat, Organisasjonsnummer organisasjonsnummer)
            : base(navn, adresse, utskriftstjenesteSertifikat, organisasjonsnummer)
        {
        }
    }
}
