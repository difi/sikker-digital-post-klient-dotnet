using System;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    internal class GuidUtility
    {
        private string standardBusinessDocumentHeaderId = Guid.NewGuid().ToString();
        private string bodyId = "soapBody";
        private string ebMessagingId = String.Format("id-{0}", Guid.NewGuid());
        private string binarySecurityTokenId = String.Format("X509-{0}", Guid.NewGuid());
        private string timestampId = String.Format("TS-{0}", Guid.NewGuid());
        private string dokumentpakkeId = String.Format("{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid());

        public string StandardBusinessDocumentHeaderId
        {
            get { return standardBusinessDocumentHeaderId; }
            internal set { standardBusinessDocumentHeaderId = value; }
        }

        public string BodyId
        {
            get { return bodyId; }
            internal set { bodyId = value; }
        }

        public string EbMessagingId
        {
            get { return ebMessagingId; }
            internal set { ebMessagingId = value; }
        }

        public string BinarySecurityTokenId
        {
            get { return binarySecurityTokenId; }
            internal set { binarySecurityTokenId = value; }
        }

        public string TimestampId
        {
            get { return timestampId; }
            internal set { timestampId = value; }
        }

        public string DokumentpakkeId
        {
            get { return dokumentpakkeId; }
            internal set { dokumentpakkeId = value; }
        }

        public override string ToString()
        {
            return string.Format("StandardBusinessDocumentHeaderId: {0}, BodyId: {1}, EbMessagingId: {2}, BinarySecurityTokenId: {3}, TimestampId: {4}, DokumentpakkeId: {5}", StandardBusinessDocumentHeaderId, BodyId, EbMessagingId, BinarySecurityTokenId, TimestampId, DokumentpakkeId);
        }
    }
}
