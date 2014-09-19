using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ArkivTester
    {
        private static string _testDataMappe = "testdata";

        private static string _vedleggsMappe = "vedlegg";
        private static string _hoveddokumentMappe = "hoveddokument";

        private static string _hoveddokument;
        private static string[] _vedleggsFiler;

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
        

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _testDataMappe = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)),_testDataMappe);

           _vedleggsMappe = Path.Combine(_testDataMappe, _vedleggsMappe);
           _hoveddokumentMappe = Path.Combine(_testDataMappe, _hoveddokumentMappe);
            
            _vedleggsFiler = Directory.GetFiles(_vedleggsMappe);
            _hoveddokument = Directory.GetFiles(_hoveddokumentMappe)[0];
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
        public void LeggFilerTilDokumentpakkeAntallStemmer()
        {
            var hoveddokument = new Dokument(Path.GetFileName(_hoveddokument),_hoveddokument);
            var vedlegg = new List<Dokument>(_vedleggsFiler.Select(v => new Dokument(Path.GetFileName(v),v)));

            var dokumentpakke = new Dokumentpakke(hoveddokument);
            dokumentpakke.LeggTil(vedlegg);

            Assert.AreEqual(vedlegg.Count, dokumentpakke.Vedlegg.Count);
        }
        
    }
}
