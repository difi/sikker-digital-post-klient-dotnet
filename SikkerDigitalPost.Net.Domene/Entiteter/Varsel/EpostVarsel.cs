namespace SikkerDigitalPost.Net.Domene.Entiteter.Varsel
{
    public class EpostVarsel : Varsel
    {
        public readonly string Epostadresse;

        /// <param name="epostadresse">Mottakerens epostadresse som det skal sendes varsel til.</param>
        /// <param name="varslingstekst">Avsenderstyrt varslingstekst som skal inngå i varselet.</param>
        public EpostVarsel(string epostadresse, string varslingstekst) : base (varslingstekst)
        {
            Epostadresse = epostadresse;
        }
    }
}
