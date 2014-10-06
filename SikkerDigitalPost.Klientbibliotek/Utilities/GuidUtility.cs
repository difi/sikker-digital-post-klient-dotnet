using System;

namespace SikkerDigitalPost.Klient.Utilities
{
    internal static class GuidUtility
    {
        public static string StandardBusinessDocumentHeaderId = Guid.NewGuid().ToString();
        public static string BodyId = String.Format("id-{0}", Guid.NewGuid());
        public static string EbMessagingId = String.Format("id-{0}", Guid.NewGuid());
        public static string BinarySecurityTokenId = String.Format("X509-{0}", Guid.NewGuid());
        public static string TimestampId = String.Format("TS-{0}", Guid.NewGuid());
        public static string DokumentpakkeId = String.Format("cid:{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid());
    }
}
