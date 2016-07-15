using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Internal.AsicE
{
    public class DocumentBundleTests
    {
        public class ConstructorMethod : DocumentBundleTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                var bundleBytes = new byte[] {0x21, 0x22};
                const int billableBytes = 2;
                const string id = "id";

                //Act
                var documentBundle = new DocumentBundle(bundleBytes, billableBytes, id);

                //Assert
                Assert.Equal(bundleBytes, documentBundle.BundleBytes);
                Assert.Equal(billableBytes, documentBundle.BillableBytes);
            }
        }

        public class TransferEncodingMethod : DocumentBundleTests
        {
            [Fact]
            public void ReturnsBinaryEncoding()
            {
                //Arrange

                //Act
                var documentBundle = new DocumentBundle(null, 0, string.Empty);

                //Assert
                Assert.Equal(documentBundle.TransferEncoding, "binary");
            }
        }

        public class ContentTypeMethod : DocumentBundleTests
        {
            [Fact]
            public void ReturnsCmsContentType()
            {
                //Arrange

                //Act
                var documentBundle = new DocumentBundle(null, 0, string.Empty);

                //Assert
                Assert.Equal(documentBundle.ContentType, "application/cms");
            }
        }
    }
}