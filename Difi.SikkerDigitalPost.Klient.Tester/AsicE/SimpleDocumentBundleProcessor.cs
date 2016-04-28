using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;

namespace Difi.SikkerDigitalPost.Klient.Tester.AsicE
{
    public class SimpleDocumentBundleProcessor : IDokumentpakkeProsessor
    {
        public long StreamLength { get; private set; }

        public long Initialposition { get; private set; }

        public bool CouldReadBytesStream { get; private set; }

        public void Prosesser(Forsendelse forsendelse, Stream bundleStream)
        {
            Initialposition = bundleStream.Position;

            CouldReadBytesStream = bundleStream.CanRead;
            StreamLength = bundleStream.Length;

            DoSomeStreamMessing(bundleStream);
        }

        private void DoSomeStreamMessing(Stream bundleStream)
        {
            bundleStream.Position = 100;
        }
    }
}