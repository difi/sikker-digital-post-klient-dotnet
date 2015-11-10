using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer
{
    public class Kvitteringsforespørsel
    {
        public readonly Prioritet Prioritet;

        /// <summary>
        /// Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender. 
        /// En forsendelse gjort med  MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id.
        /// </summary>
        public string MpcId { get; private set; }

        public string Mpc
        {
            get
            {
                return MpcId == String.Empty
                    ? String.Format("urn:{0}", Prioritet.ToString().ToLower())
                    : String.Format("urn:{0}:{1}", Prioritet.ToString().ToLower(), MpcId);
            }
        }

        /// <param name="prioritet">Hvilken prioritet det forespørres kvittering for. De ulike prioritene kan ses på som egne køer for kvitteringer.
        /// Dersom en forsendelse er sendt med normal prioritet, vil den kun dukke opp dersom det spørres om kvittering på normal prioritet.</param>
        /// <param name="mpcId">Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender. 
        /// En forsendelse gjort med en MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id. Standardverdi er "".</param>
        public Kvitteringsforespørsel(Prioritet prioritet = Prioritet.Normal, string mpcId = "")
        {
            Prioritet = prioritet;
            MpcId = mpcId;
        }
    }
}
