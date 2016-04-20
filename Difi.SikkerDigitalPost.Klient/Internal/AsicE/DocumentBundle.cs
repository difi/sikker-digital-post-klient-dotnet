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

        //TOdo: Assert if temp or not. To make available for RequestHelper
        public string ContentId { get; set; }

        //TOdo: Assert if temp or not. To make available for RequestHelper
        public string TransferEncoding { get; } = "binary";

        public string ContentType { get; } = "application/cms";
    }
}