using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Internal.AsicE
{
    public class AsiceAttachableProcessorTests
    {
        public class ProcessMethod : AsiceAttachableProcessorTests
        {
            [Fact]
            public void ProcessesCorrectly()
            {
                //Arrange
                var message = DomainUtility.GetForsendelseSimple();
                var documentBundleProcessor = new MockAttachableProcessor();
                var stream = new MemoryStream(new byte[] {0x42, 0x32});
                var asiceAttachableProcessor = new AsiceAttachableProcessor(message, documentBundleProcessor);

                //Act
                asiceAttachableProcessor.Process(stream);

                //Assert
            }

            private class MockAttachableProcessor : IDokumentpakkeProsessor
            {
                public void Prosesser(Forsendelse signatureJob, Stream bundleStream)
                {
                    if (signatureJob == null || !bundleStream.CanRead)
                    {
                        throw new InvalidDataException("Properties not properly initialized in wrapping class.");
                    }
                }
            }
        }
    }
}