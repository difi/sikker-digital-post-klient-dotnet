namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører
{
    public class Person
    {
        public string Personidentifikator { get; set; }
        public string Postkasseadresse { get; set; }

        /// <param name="personIdentifikator">Identifikator (fødselsnummer eller D-nummer)</param>
        /// <param name="postkasseadresse">Adresse hos postkasseleverandøren</param>
        public Person(string personIdentifikator, string postkasseadresse)
        {
            Personidentifikator = personIdentifikator;
            Postkasseadresse = postkasseadresse;
        }
    }
}
