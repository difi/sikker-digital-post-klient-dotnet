using System;
using System.Text.RegularExpressions;
using SikkerDigitalPost.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Domene.Entiteter
{
    public class Organisasjonsnummer
    {
        public static readonly string Iso6523_Pattern = "^([0-9]{4}:)?([0-9]{9})$";
        public static string Iso6523_ActorId = PMode.PartyIdType;
        
        /// <summary>
        /// Stringrepresentasjon av organisasjonsnummeret
        /// </summary>
        public readonly string Verdi;
        
        /// <param name="verdi">Stringrepresentasjon av organisasjonsnummeret</param>
        public Organisasjonsnummer(string verdi)
        {
            Verdi = verdi;
        }

        /// <summary>
        /// Organisasjonsnummer på ISO6523-format 
        /// </summary>
        /// <returns>Organisasjonsnummer, prefikset med '9908':, som er id for 'Enhetsregistret ved Brønnøysundregisterne'</returns>
        public string Iso6523()
        {
            return String.Format("9908:{0}", Verdi);
        }

        public static Organisasjonsnummer FraIso6523(string iso6523Orgnr)
        {
            var match = Regex.Match(iso6523Orgnr, Iso6523_Pattern);
            
            if (!match.Success)
            {
                throw new ArgumentException(String.Format("Ugyldig organisasjonsnummer. Forventet format er ISO 6523, " +
                                                          "fikk følgende organisasjonsnummer: {0}.", iso6523Orgnr));
            }
            return new Organisasjonsnummer(match.Groups[2].ToString());
        }
    }
}
