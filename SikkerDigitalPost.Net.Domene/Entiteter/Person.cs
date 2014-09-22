namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Person
    {
        public string Personidentifikator { get; set; }
        public string Postkasseadresse { get; set; }

        public Person(string personIdentifikator, string postkasseadresse)
        {
            Personidentifikator = personIdentifikator;
            Postkasseadresse = postkasseadresse;
        }
    }
}
