using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SikkerDigitalPost.Net.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ArkivTester
    {
        private static string _testDataMappe;
        private static string _vedleggsMappe = "vedlegg";
        private static string _hoveddokumentMapp = "hoveddokument";
        private static readonly string _hoveddokument = "Hoveddokument.docx";
        private static string[] VedleggsFiler;

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance;}
            set{ testContextInstance = value;}
        }


        public ArkivTester()
        {
           
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _testDataMappe = Path.GetDirectoryName(Path.GetDirectoryName(context.TestDir));
            _vedleggsMappe = Path.Combine(_testDataMappe, _vedleggsMappe);
            VedleggsFiler = Directory.GetFiles(_vedleggsMappe);
            _hoveddokument = Directory.Get

        }


        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestCleanup]
        public void TestCleanup()
        {
           
        }

        [TestMethod]
        public void TestMethod1()
        {
            
            var folder = new FileInfo(_testDataMappe).FullName;

            var exists = File.Exists(_testDataMappe + Path.PathSeparator + VedleggsFiler[0]);
        }
        
    }
}
