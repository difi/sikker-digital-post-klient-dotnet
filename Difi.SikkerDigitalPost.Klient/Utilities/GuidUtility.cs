using System;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    internal class GuidUtility
    {
        private string _standardBusinessDocumentHeaderId = Guid.NewGuid().ToString();
        private string _bodyId = "soapBody";
        private string _ebMessagingId = String.Format("id-{0}", Guid.NewGuid());
        private string _binarySecurityTokenId = String.Format("X509-{0}", Guid.NewGuid());
        private string _timestampId = String.Format("TS-{0}", Guid.NewGuid());
        private string _dokumentpakkeId = String.Format("{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid());

        public string StandardBusinessDocumentHeaderId
        {
            get { return _standardBusinessDocumentHeaderId; }
            internal set { _standardBusinessDocumentHeaderId = value; }
        }

        public string BodyId
        {
            get { return _bodyId; }
            internal set { _bodyId = value; }
        }

        public string EbMessagingId
        {
            get { return _ebMessagingId; }
            internal set { _ebMessagingId = value; }
        }

        public string BinarySecurityTokenId
        {
            get { return _binarySecurityTokenId; }
            internal set { _binarySecurityTokenId = value; }
        }

        public string TimestampId
        {
            get { return _timestampId; }
            internal set { _timestampId = value; }
        }

        public string DokumentpakkeId
        {
            get { return _dokumentpakkeId; }
            internal set { _dokumentpakkeId = value; }
        }

        public override string ToString()
        {
            return string.Format("StandardBusinessDocumentHeaderId: {0}, BodyId: {1}, EbMessagingId: {2}, BinarySecurityTokenId: {3}, TimestampId: {4}, DokumentpakkeId: {5}", StandardBusinessDocumentHeaderId, BodyId, EbMessagingId, BinarySecurityTokenId, TimestampId, DokumentpakkeId);
        }
    }
}
