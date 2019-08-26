using System;
using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer
{
    public abstract class Kvittering
    {
        protected Kvittering(string meldingsId)
        {
            MeldingsId = meldingsId;
        }

        /// <summary>
        ///     Tidspunktet da kvitteringen ble sendt.
        /// </summary>
        public DateTime SendtTidspunkt { get; set; }

        /// <summary>
        ///     Unik identifikator for kvitteringen.
        /// </summary>
        public string MeldingsId { get; set; }

        /// <summary>
        ///     Refereranse til en annen relatert melding. Refererer til den relaterte meldingens MessageId.
        /// </summary>
        public string ReferanseTilMeldingId { get; set; }

        /// <summary>
        ///     Kvitteringen presentert som tekststreng.
        /// </summary>
        public string Rådata => Xml.OuterXml;

        public XmlDocument Xml { get; set; }

        public override string ToString()
        {
            return
                $"[{GetType().Name}] SendtTidspunkt: {SendtTidspunkt}, MeldingsId: {MeldingsId}, ReferanseTilMeldingId: {ReferanseTilMeldingId}";
        }
    }
}