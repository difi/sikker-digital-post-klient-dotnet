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

        public override string ToString()
        {
            return string.Format("SendtTidspunkt: {0}, MeldingsId: {1}, ReferanseTilMeldingId: {2}", SendtTidspunkt, MeldingsId, ReferanseTilMeldingId);
        }
    }
}
