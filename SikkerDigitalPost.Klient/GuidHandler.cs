using System;

namespace SikkerDigitalPost.Klient
{
    internal class GuidHandler
    {
        public string StandardBusinessDocumentHeaderId = Guid.NewGuid().ToString();
        public string BodyId = String.Format("id-{0}", Guid.NewGuid());
        public string EbMessagingId = String.Format("id-{0}", Guid.NewGuid());
        public string BinarySecurityTokenId = String.Format("X509-{0}", Guid.NewGuid());
        public string TimestampId = String.Format("TS-{0}", Guid.NewGuid());
        public string DokumentpakkeId = String.Format("cid:{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid());
    }
}
