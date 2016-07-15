using System;
using System.IO;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class LagreDokumentpakkeTilDiskProsessorTests
    {
        public class ConstructorMethod : LagreDokumentpakkeTilDiskProsessorTests
        {
            [Fact]
            public void SimpleConstructor()
            {
                //Arrange
                var directory = "C:\\directory";

                //Act
                var documentBundleToDiskProcessor = new LagreDokumentpakkeTilDiskProsessor(directory);

                //Assert
                Assert.Equal(directory, documentBundleToDiskProcessor.Directory);
            }
        }

        public class ProcessMethod : LagreDokumentpakkeTilDiskProsessorTests
        {
            [Fact]
            public void PersistsFileToDisk()
            {
                //Arrange
                var tmpDirectory = Path.GetTempPath();
                var processor = new LagreDokumentpakkeTilDiskProsessor(tmpDirectory);
                var message = DomainUtility.GetForsendelseSimple();
                var asiceBytes = DomainUtility.GetAsiceArchive(message).Bytes;
                var asiceStream = new MemoryStream(asiceBytes);

                //Act
                processor.Prosesser(message, asiceStream);
                var processedFileName = processor.LastFileProcessed;
                var tempFile = Path.Combine(tmpDirectory, processedFileName);

                //Assert
                Assert.Equal(asiceBytes.Length, new FileInfo(tempFile).Length);
            }

            [Fact]
            public void FileNameContainsEssentialData()
            {
                //Arrange
                var tmpDirectory = Path.GetTempPath();
                var fileEnding = "asice.zip";

                var processor = new LagreDokumentpakkeTilDiskProsessor(tmpDirectory);
                var message = DomainUtility.GetForsendelseSimple();
                var asiceBytes = DomainUtility.GetAsiceArchive(message).Bytes;
                var asiceStream = new MemoryStream(asiceBytes);

                //Act
                processor.Prosesser(message, asiceStream);
                var processedFileName = processor.LastFileProcessed;
                var tempFileName = Path.Combine(tmpDirectory, processedFileName);

                //Assert
                Assert.True(tempFileName.Contains(tmpDirectory));
                Assert.True(tempFileName.Contains(fileEnding));
                Assert.True(tempFileName.Contains(message.KonversasjonsId.ToString()));
                Assert.True(tempFileName.Contains(DateTime.Now.Year.ToString()));
            }
        }
    }
}