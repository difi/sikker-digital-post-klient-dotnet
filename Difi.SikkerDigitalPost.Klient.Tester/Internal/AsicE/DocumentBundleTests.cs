using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Internal.AsicE
{
    [TestClass]
    public class DocumentBundleTests
    {
        [TestClass]
        public class ConstructorMethod : DocumentBundleTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var bundleBytes = new byte[] {0x21, 0x22};
                const int billableBytes = 2;
                const string id = "id";

                //Act
                var documentBundle = new DocumentBundle(bundleBytes, billableBytes, id);

                //Assert
                Assert.AreEqual(bundleBytes, documentBundle.BundleBytes);
                Assert.AreEqual(billableBytes, documentBundle.BillableBytes);
            }
        }

        [TestClass]
        public class TransferEncodingMethod : DocumentBundleTests
        {
            [TestMethod]
            public void ReturnsBinaryEncoding()
            {
                //Arrange

                //Act
                var documentBundle = new DocumentBundle(null, 0, string.Empty);

                //Assert
                Assert.AreEqual(documentBundle.TransferEncoding, "binary");
            }
        }

        [TestClass]
        public class ContentTypeMethod : DocumentBundleTests
        {
            [TestMethod]
            public void ReturnsCmsContentType()
            {
                //Arrange

                //Act
                var documentBundle = new DocumentBundle(null, 0, string.Empty);

                //Assert
                Assert.AreEqual(documentBundle.ContentType, "application/cms");
            }
        }
    }
}