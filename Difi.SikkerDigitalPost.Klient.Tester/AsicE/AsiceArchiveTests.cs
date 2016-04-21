using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.AsicE
{
    [TestClass]
    public class AsiceArchiveTests
    {
        [TestClass]
        public class ContentBytesCountMethod : AsiceArchiveTests
        {
            [TestMethod]
            public void ReturnsProperBytesCount()
            {
                //Arrange
                var forsendelse = DomeneUtility.GetDigitalForsendelseVarselFlereDokumenterHøyereSikkerhet();

                var manifest = new Manifest(forsendelse);
                var asiceArchive = new AsiceArchive(forsendelse, manifest, new Signature(forsendelse, manifest, DomeneUtility.GetAvsenderEnhetstesterSertifikat()), new GuidUtility());

                var expectedBytesCount = 0L;
                expectedBytesCount += asiceArchive.Manifest.Bytes.Length;
                expectedBytesCount += asiceArchive.Signature.Bytes.Length;
                expectedBytesCount += asiceArchive.Dokumentpakke.Hoveddokument.Bytes.Length;

                foreach (var dokument in asiceArchive.Dokumentpakke.Vedlegg)
                {
                    expectedBytesCount += dokument.Bytes.Length;
                }

                //Act
                var actualBytesCount = asiceArchive.UnzippedContentBytesCount;

                //Assert
                Assert.AreEqual(expectedBytesCount, actualBytesCount);
            }
        }
    }
}