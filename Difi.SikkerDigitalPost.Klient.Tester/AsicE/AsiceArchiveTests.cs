using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.AsicE
{
    public class AsiceArchiveTests
    {
        public class ConstructorMethod : AsiceArchiveTests
        {
            [Fact]
            public void InitializesFieldsProperly()
            {
                //Arrange
                var forsendelse = DomainUtility.GetDigitalDigitalPostWithNotificationMultipleDocumentsAndHigherSecurity();

                var manifest = new Manifest(forsendelse);
                var cryptographicCertificate = forsendelse.PostInfo.Mottaker.Sertifikat;
                var signature = new Signature(forsendelse, manifest, DomainUtility.GetAvsenderCertificate());

                var asiceAttachables = new List<IAsiceAttachable>();
                asiceAttachables.AddRange(forsendelse.Dokumentpakke.Vedlegg);
                asiceAttachables.Add(forsendelse.Dokumentpakke.Hoveddokument);
                asiceAttachables.Add(manifest);
                asiceAttachables.Add(signature);

                var asiceAttachablesArray = asiceAttachables.ToArray();

                var asiceAttachableProcessors = new List<AsiceAttachableProcessor>();

                //Act
                var asiceArchive = new AsiceArchive(cryptographicCertificate, new GuidUtility(), asiceAttachableProcessors, asiceAttachablesArray);

                //Assert
                Assert.Equal(asiceAttachableProcessors, asiceArchive.AsiceAttachableProcessors);
                Assert.Equal(asiceAttachablesArray, asiceArchive.AsiceAttachables);
            }

            [Fact]
            public void ConstructorGeneratesBytes()
            {
                //Arrange
                var message = new Forsendelse(DomainUtility.GetAvsender(), DomainUtility.GetDigitalPostInfoSimple(), DomainUtility.GetDokumentpakkeWithMultipleVedlegg());

                var asiceArchive = DomainUtility.GetAsiceArchive(message);

                //Act
                var archiveBytes = asiceArchive.UnencryptedBytes;

                //Assert
                using (var memoryStream = new MemoryStream(archiveBytes))
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        Assert.True(archive.Entries.Any(entry => entry.FullName == "manifest.xml"));
                        Assert.True(archive.Entries.Any(entry => entry.FullName == "META-INF/signatures.xml"));
                        Assert.True(archive.Entries.Any(entry => entry.FullName == message.Dokumentpakke.Hoveddokument.Filnavn));

                        foreach (var document in message.Dokumentpakke.Vedlegg)
                        {
                            Assert.True(archive.Entries.Any(entry => entry.FullName == document.Filnavn));
                        }
                    }
                }
            }
        }

        public class ContentBytesCountMethod : AsiceArchiveTests
        {
            [Fact]
            public void ReturnsProperBytesCount()
            {
                //Arrange
                var forsendelse = DomainUtility.GetDigitalDigitalPostWithNotificationMultipleDocumentsAndHigherSecurity();

                var manifest = new Manifest(forsendelse);
                var cryptographicCertificate = forsendelse.PostInfo.Mottaker.Sertifikat;
                var signature = new Signature(forsendelse, manifest, DomainUtility.GetAvsenderCertificate());

                var asiceAttachables = new List<IAsiceAttachable>();
                asiceAttachables.AddRange(forsendelse.Dokumentpakke.Vedlegg);
                asiceAttachables.Add(forsendelse.Dokumentpakke.Hoveddokument);
                asiceAttachables.Add(manifest);
                asiceAttachables.Add(signature);

                var asiceArchive = new AsiceArchive(cryptographicCertificate, new GuidUtility(), new List<AsiceAttachableProcessor>(), asiceAttachables.ToArray());

                var expectedBytesCount = 0L;
                foreach (var dokument in asiceAttachables)
                {
                    expectedBytesCount += dokument.Bytes.Length;
                }

                //Act
                var actualBytesCount = asiceArchive.UnzippedContentBytesCount;

                //Assert
                Assert.Equal(expectedBytesCount, actualBytesCount);
            }
        }

        public class BytesMethod : AsiceArchiveTests
        {
            [Fact]
            public void SendsBytesThroughDocumentBundleProcessors()
            {
                //Arrange
                var clientConfiguration = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø)
                {
                    Dokumentpakkeprosessorer = new List<IDokumentpakkeProsessor>
                    {
                        new SimpleDocumentBundleProcessor(),
                        new SimpleDocumentBundleProcessor()
                    }
                };

                var forsendelse = DomainUtility.GetForsendelseSimple();
                var asiceAttachableProcessors = clientConfiguration.Dokumentpakkeprosessorer.Select(p => new AsiceAttachableProcessor(forsendelse, p));
                var asiceAttachables = new IAsiceAttachable[] {DomainUtility.GetHoveddokumentSimple()};

                //Act
                var asiceArchive = new AsiceArchive(DomainUtility.GetMottakerCertificate(), new GuidUtility(), asiceAttachableProcessors, asiceAttachables);
                var bytes = asiceArchive.Bytes;

                //Assert
                foreach (var simpleProcessor in clientConfiguration.Dokumentpakkeprosessorer.Cast<SimpleDocumentBundleProcessor>())
                {
                    Assert.True(simpleProcessor.StreamLength > 1000);
                    Assert.True(simpleProcessor.CouldReadBytesStream);
                    Assert.Equal(0, simpleProcessor.Initialposition);
                }
            }
        }
    }
}