namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal class DocumentBundle
    {
        public DocumentBundle(byte[] bundleBytes, long billableBytes, string contentId)
        {
            BundleBytes = bundleBytes;
            BillableBytes = billableBytes;
            ContentId = contentId;
        }

        public byte[] BundleBytes { get; internal set; }

        public long BillableBytes { get; internal set; }

        public string ContentId { get; set; }

        public string TransferEncoding { get; } = "binary";

        public string ContentType { get; } = "application/cms";
    }
}