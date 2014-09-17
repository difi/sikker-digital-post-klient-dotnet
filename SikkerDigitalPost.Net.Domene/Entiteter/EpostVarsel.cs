using SikkerDigitalPost.Net.Domene.Entiteter.Abstrakt;

namespace SikkerDigitalPost.Net.Domene.Entiteter
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
