using System;
using System.Text.RegularExpressions;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter
{
    public class Organisasjonsnummer
    {
        private const string CountryCodeOrganizationNumberNorway = "9908";
        public static readonly string OrganisasjonsnummerPattern = "^([0-9]{4}:)?([0-9]{9})$";

        public Organisasjonsnummer(string organisasjonsnummer)
        {
            Verdi = GetValidatedOrganisasjonsnummerOrThrowException(organisasjonsnummer);
        }

        public string Verdi { get; }

        internal string WithCountryCode => Verdi.StartsWith($"{CountryCodeOrganizationNumberNorway}:") ? Verdi : $"{CountryCodeOrganizationNumberNorway}:{Verdi}";

        /// <summary>
        ///     Organisasjonsnummer på ISO6523-format
        /// </summary>
        /// <returns>Organisasjonsnummer, prefikset med '9908':, som er id for 'Enhetsregistret ved Brønnøysundregisterne'</returns>
        [Obsolete("Blir fjernet fordi navnet er obskurt og i beste fall litt feil, og skal ikke eksponeres ut av biblioteket.")]
        public string Iso6523()
        {
            return WithCountryCode;
        }

        private static string GetValidatedOrganisasjonsnummerOrThrowException(string organisasjonsnummer)
        {
            var match = Regex.Match(organisasjonsnummer, OrganisasjonsnummerPattern);

            if (!match.Success)
            {
                throw new KonfigurasjonsException($"Ugyldig organisasjonsnummer. Fikk følgende organisasjonsnummer: {organisasjonsnummer}. " +
                                                  "Organisasjonsnummeret skal være 9 siffer og kan prefikses med landkode 9908. Eksempler på dette er '9908:984661185' og '984661185'");
            }
            const int organisasjonsnummerWithoutPrefixIndex = 2;
            return match.Groups[organisasjonsnummerWithoutPrefixIndex].ToString();
        }

        [Obsolete("Vil bli fjernet fordi den ikke skal eksponeres ut til brukere av biblioteket.")]
        public static Organisasjonsnummer FraIso6523(string iso6523Orgnr)
        {
            return new Organisasjonsnummer(GetValidatedOrganisasjonsnummerOrThrowException(iso6523Orgnr));
        }
    }
}