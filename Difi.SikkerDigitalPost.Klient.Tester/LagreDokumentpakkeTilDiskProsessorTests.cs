using System;
using System.IO;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class LagreDokumentpakkeTilDiskProsessorTests
    {
        [TestClass]
        public class ConstructorMethod : LagreDokumentpakkeTilDiskProsessorTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var directory = "C:\\directory";

                //Act
                var documentBundleToDiskProcessor = new LagreDokumentpakkeTilDiskProsessor(directory);

                //Assert
                Assert.AreEqual(directory, documentBundleToDiskProcessor.Directory);
            }
        }

        [TestClass]
        public class ProcessMethod : LagreDokumentpakkeTilDiskProsessorTests
        {
            [TestMethod]
            public void PersistsFileToDisk()
            {
                //Arrange
                var tmpDirectory = Path.GetTempPath();
                var processor = new LagreDokumentpakkeTilDiskProsessor(tmpDirectory);
                var message = DomeneUtility.GetDigitalMessageSimple();
                var asiceBytes = DomeneUtility.GetAsiceArchive(message).Bytes;
                var asiceStream = new MemoryStream(asiceBytes);

                //Act
                processor.Prosesser(message, asiceStream);
                var processedFileName = processor.LastFileProcessed;
                var tempFile = Path.Combine(tmpDirectory, processedFileName);

                //Assert
                Assert.AreEqual(asiceBytes.Length, new FileInfo(tempFile).Length);
            }

            [TestMethod]
            public void FileNameContainsEssentialData()
            {
                //Arrange
                var tmpDirectory = Path.GetTempPath();
                var fileEnding = "asice.zip";

                var processor = new LagreDokumentpakkeTilDiskProsessor(tmpDirectory);
                var message = DomeneUtility.GetDigitalMessageSimple();
                var asiceBytes = DomeneUtility.GetAsiceArchive(message).Bytes;
                var asiceStream = new MemoryStream(asiceBytes);

                //Act
                processor.Prosesser(message, asiceStream);
                var processedFileName = processor.LastFileProcessed;
                var tempFileName = Path.Combine(tmpDirectory, processedFileName);

                //Assert
                Assert.IsTrue(tempFileName.Contains(tmpDirectory));
                Assert.IsTrue(tempFileName.Contains(fileEnding));
                Assert.IsTrue(tempFileName.Contains(message.KonversasjonsId.ToString()));
                Assert.IsTrue(tempFileName.Contains(DateTime.Now.Year.ToString()));
            }
        }
    }
}