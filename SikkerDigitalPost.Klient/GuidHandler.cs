using System;

namespace SikkerDigitalPost.Klient
{
    internal class GuidHandler
    {
        public readonly string StandardBusinessDocumentHeaderId = Guid.NewGuid().ToString();
        public readonly string BodyId = "soapBody";
        public readonly string EbMessagingId = String.Format("id-{0}", Guid.NewGuid());
        public readonly string BinarySecurityTokenId = String.Format("X509-{0}", Guid.NewGuid());
        public readonly string TimestampId = String.Format("TS-{0}", Guid.NewGuid());
        public readonly string DokumentpakkeId = String.Format("{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid());
    }
}
