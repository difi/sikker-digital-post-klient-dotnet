using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer
{
    public class Kvitteringsforespørsel
    {
        [Obsolete(message:"Prioritet settes av integrasjonspunkt")]
        public readonly Prioritet Prioritet;

        /// <param name="prioritet">
        ///     Hvilken prioritet det forespørres kvittering for. De ulike prioritene kan ses på som egne køer for kvitteringer.
        ///     Dersom en forsendelse er sendt med normal prioritet, vil den kun dukke opp dersom det spørres om kvittering på
        ///     normal prioritet.
        /// </param>
        /// <param name="mpcId">
        ///     Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender.
        ///     En forsendelse gjort med en MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id. Standardverdi er "".
        /// </param>
        public Kvitteringsforespørsel(Prioritet prioritet = Prioritet.Normal, string mpcId = "")
        {
            Prioritet = prioritet;
            MpcId = mpcId;
        }

        /// <summary>
        ///     Brukes til å skille mellom ulike kvitteringskøer for samme tekniske avsender.
        ///     En forsendelse gjort med  MPC Id vil kun dukke opp i kvitteringskøen med samme MPC Id.
        /// </summary>
        [Obsolete(message:"MpcId settes av integrasjonspunkt")]
        public string MpcId { get; }

        [Obsolete(message:"MpcId settes av integrasjonspunkt")]
        public string Mpc => MpcId == string.Empty
            ? $"urn:{Prioritet.ToString().ToLower()}"
            : $"urn:{Prioritet.ToString().ToLower()}:{MpcId}";
    }
}
