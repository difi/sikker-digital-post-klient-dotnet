using System;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    internal class GuidUtility
    {
        public string StandardBusinessDocumentHeaderId { get; internal set; } = Guid.NewGuid().ToString();

        public string BodyId { get; internal set; } = "soapBody";

        public string EbMessagingId { get; internal set; } = $"id-{Guid.NewGuid()}";

        public string BinarySecurityTokenId { get; internal set; } = $"X509-{Guid.NewGuid()}";

        public string TimestampId { get; internal set; } = $"TS-{Guid.NewGuid()}";

        public string DokumentpakkeId { get; internal set; } = $"{Guid.NewGuid()}@meldingsformidler.sdp.difi.no";

        public override string ToString()
        {
            return
                $"StandardBusinessDocumentHeaderId: {StandardBusinessDocumentHeaderId}, BodyId: {BodyId}, EbMessagingId: {EbMessagingId}, BinarySecurityTokenId: {BinarySecurityTokenId}, TimestampId: {TimestampId}, DokumentpakkeId: {DokumentpakkeId}";
        }
    }
}
