using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
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
        public class ConstructorMethod : AsiceArchiveTests
        {
            [TestMethod]
            public void ConstructorGeneratesBytes()
            {
                //Arrange
                var message = new Forsendelse(DomeneUtility.GetAvsender(), DomeneUtility.GetDigitalPostInfoEnkel(), DomeneUtility.GetDokumentpakkeMedFlereVedlegg());

                var asiceArchive = DomeneUtility.GetAsiceArchive(message);

                //Act
                var archiveBytes = asiceArchive.UnencryptedBytes;

                //Assert
                using (var memoryStream = new MemoryStream(archiveBytes))
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == "manifest.xml"));
                        Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == "META-INF/signatures.xml"));
                        Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == message.Dokumentpakke.Hoveddokument.Filnavn));

                        foreach (var document in message.Dokumentpakke.Vedlegg)
                        {
                            Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == document.Filnavn));
                        }
                    }
                }
            }
        }

        [TestClass]
        public class ContentBytesCountMethod : AsiceArchiveTests
        {
            [TestMethod]
            public void ReturnsProperBytesCount()
            {
                //Arrange
                var message = DomeneUtility.GetDigitalForsendelseVarselFlereDokumenterHøyereSikkerhet();

                var manifest = new Manifest(message);
                var cryptographicCertificate = message.PostInfo.Mottaker.Sertifikat;
                var signature = new Signature(message, manifest, DomeneUtility.GetAvsenderSertifikat());

                var asiceAttachables = new List<IAsiceAttachable>();
                asiceAttachables.AddRange(message.Dokumentpakke.Vedlegg);
                asiceAttachables.Add(message.Dokumentpakke.Hoveddokument);
                asiceAttachables.Add(manifest);
                asiceAttachables.Add(signature);

                var asiceArchive = new AsiceArchive(cryptographicCertificate, new GuidUtility(), asiceAttachables.ToArray());

                var expectedBytesCount = 0L;
                foreach (var dokument in asiceAttachables)
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