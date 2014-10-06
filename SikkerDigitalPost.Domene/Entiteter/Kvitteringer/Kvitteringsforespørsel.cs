using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Kvitteringsforespørsel
    {
        public readonly Prioritet Prioritet;

        /// <summary>
        /// Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender. 
        /// En forsendelse gjort med  MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id.
        /// </summary>
        public string MpcId { get; set; }

        
        /// <param name="prioritet">Prioritet for forespørselen.</param>
        public Kvitteringsforespørsel(Prioritet prioritet)
        {
            Prioritet = prioritet;
        }
    }
}
