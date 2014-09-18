using System;
using System.Text.RegularExpressions;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.Domene
{
    public class Organisasjonsnummer
    {
        public readonly string Orgnummer;
        
        public static readonly string Iso6523_Pattern = "^([0-9]{4}:)?([0-9]{9})$";
        public static string Iso6523_ActorId = PMode.PartyIdType;
        public readonly string Iso6523_ActorOld = "iso6523-actorid-upis";

        public readonly Organisasjonsnummer Null = new Organisasjonsnummer(String.Empty);

        public Organisasjonsnummer(string orgnummer)
        {
            Orgnummer = orgnummer;
        }

        public string Iso6523()
        {
            return String.Format("9908:{0}", Orgnummer);
        }

        public static Organisasjonsnummer FraIso6523(string iso6523Orgnr)
        {
            throw new Exception("Denne metoden er ikke testet.");
            
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
