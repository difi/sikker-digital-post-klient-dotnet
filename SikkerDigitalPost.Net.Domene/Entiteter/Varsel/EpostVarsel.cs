namespace SikkerDigitalPost.Net.Domene.Entiteter.Varsel
{
    public class EpostVarsel : Varsel
    {
        public readonly string Epostadresse;

        public EpostVarsel(string epostadresse, string varslingstekst) : base (varslingstekst)
        {
            Epostadresse = epostadresse;
        }
    }
}
