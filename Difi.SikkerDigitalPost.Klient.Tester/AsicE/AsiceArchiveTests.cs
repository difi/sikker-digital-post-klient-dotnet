using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.AsicE
{
    [TestClass()]
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
                var asicEArkiv = DomeneUtility.GetAsicEArkiv(forsendelse);

                var expectedBytesCount = 0L;
                expectedBytesCount += asicEArkiv.Manifest.Bytes.Length;
                expectedBytesCount += asicEArkiv.Signature.Bytes.Length;
                expectedBytesCount += asicEArkiv.Dokumentpakke.Hoveddokument.Bytes.Length;

                foreach (var dokument in asicEArkiv.Dokumentpakke.Vedlegg)
                {
                    expectedBytesCount+= dokument.Bytes.Length;
                }

                //Act
                var actualBytesCount = asicEArkiv.UnzippedContentBytesCount;

                //Assert
                Assert.AreEqual(expectedBytesCount, actualBytesCount);
            }
        }
    }
}