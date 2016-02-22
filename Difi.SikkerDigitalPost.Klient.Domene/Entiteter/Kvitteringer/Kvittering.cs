using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer
{
    public abstract class Kvittering
    {
        /// <summary>
        /// Tidspunktet da kvitteringen ble sendt.
        /// </summary>
        public DateTime SendtTidspunkt { get; set; }

        /// <summary>
        /// Unik identifikator for kvitteringen.
        /// </summary>
        public string MeldingsId { get; set; }

        /// <summary>
        /// Refereranse til en annen relatert melding. Refererer til den relaterte meldingens MessageId.
        /// </summary>
        public string ReferanseTilMeldingId { get; set; }

        /// <summary>
        /// Kvitteringen presentert som tekststreng.
        /// </summary>
        public string Rådata { get; set; }

        protected Kvittering(string meldingsId)
        {
            MeldingsId = meldingsId;
        }

        public override string ToString()
        {
            return
                $"[{GetType().Name}] SendtTidspunkt: {SendtTidspunkt}, MeldingsId: {MeldingsId}, ReferanseTilMeldingId: {ReferanseTilMeldingId}";
        }
    }
}
