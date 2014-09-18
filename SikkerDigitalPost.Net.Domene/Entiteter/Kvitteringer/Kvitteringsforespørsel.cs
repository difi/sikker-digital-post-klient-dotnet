namespace SikkerDigitalPost.Net.Domene.Entiteter.Kvitteringer
{
    public class Kvitteringsforespørsel
    {
        public readonly Prioritet Prioritet;
        public string MpcId { get; set; }

        public Kvitteringsforespørsel(Prioritet prioritet)
        {
            Prioritet = prioritet;
        }
    }
}
