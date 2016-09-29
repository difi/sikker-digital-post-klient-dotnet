using System;
using System.Text.RegularExpressions;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter
{
    public class Organisasjonsnummer
    {
        public static readonly string Iso6523Pattern = "^([0-9]{4}:)?([0-9]{9})$";
        private const string CountryCodeOrganizationNumberNorway = "9908";

        /// <summary>
        ///     Stringrepresentasjon av organisasjonsnummeret
        /// </summary>
        public readonly string Verdi;

        /// <param name="verdi">Stringrepresentasjon av organisasjonsnummeret</param>
        public Organisasjonsnummer(string verdi)
        {
            Verdi = verdi;
        }

        /// <summary>
        ///     Organisasjonsnummer på ISO6523-format
        /// </summary>
        /// <returns>Organisasjonsnummer, prefikset med '9908':, som er id for 'Enhetsregistret ved Brønnøysundregisterne'</returns>
        [Obsolete("Bruk OrganisasjonsnummerMedLandkode. Blir fjernet fordi navnet er obskurt og i beste fall litt feil.")]
        public string Iso6523()
        {
            return OrganisasjonsnummerMedLandkode();
        }

        public string OrganisasjonsnummerMedLandkode()
        {
            return Verdi.StartsWith($"{CountryCodeOrganizationNumberNorway}:") ? Verdi : $"{CountryCodeOrganizationNumberNorway}:{Verdi}";
        }

        public static Organisasjonsnummer FraIso6523(string iso6523Orgnr)
        {
            var match = Regex.Match(iso6523Orgnr, Iso6523Pattern);

            if (!match.Success)
            {
                throw new KonfigurasjonsException("Ugyldig organisasjonsnummer. Forventet format er ISO 6523, " +
                                                  $"fikk følgende organisasjonsnummer: {iso6523Orgnr}.");
            }
            return new Organisasjonsnummer(match.Groups[2].ToString());
        }
    }
}