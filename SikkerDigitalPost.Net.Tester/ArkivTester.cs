using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Net.KlientApi;

namespace SikkerDigitalPost.Net.Tests
{
    [TestClass]
    public class ArkivTester : TestBase
    {
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
              Initialiser();
        }

        [TestMethod]
        public void LeggFilerTilDokumentpakkeAntallStemmer()
        {
            Assert.AreEqual(Vedleggsstier.Length, Dokumentpakke.Vedlegg.Count);
            Assert.IsNotNull(Dokumentpakke.Hoveddokument);
        }

        [TestMethod]
        public void LagArkivOgVerifiserDokumentInnhold()
        {
            var arkiv = new Arkiv(Dokumentpakke, Signatur, Manifest);
            

            var arkivstrøm = new MemoryStream(arkiv.LagArkiv());

            //Åpne zip og generer sjekksum for å verifisere innhold
            using (var zip = new ZipArchive(arkivstrøm, ZipArchiveMode.Read))
            {
                //Alle vedlegg
                foreach (var filsti in Vedleggsstier)
                {
                    byte[] sjekksum1;
                    byte[] sjekksum2;

                    GenererSjekksum(zip, filsti, Path.GetFileName(filsti), out sjekksum1, out sjekksum2);
                    Assert.AreEqual(sjekksum1.ToString(), sjekksum2.ToString());
                }

                //Signaturfil
                {
                    byte[] sjekksum1;
                    byte[] sjekksum2;

                    GenererSjekksum(zip, Signatur.Bytes, arkiv.Signatur.Filnavn, out sjekksum1, out sjekksum2);
                    Assert.AreEqual(sjekksum1.ToString(), sjekksum2.ToString());
                }

                //Manifest
                {
                    byte[] sjekksum1;
                    byte[] sjekksum2;

                    GenererSjekksum(zip, Manifest.Bytes, Path.GetFileName(arkiv.Manifest.Filnavn), out sjekksum1, out sjekksum2);
                    Assert.AreEqual(sjekksum1.ToString(), sjekksum2.ToString());
                }
            }
        }

        [TestMethod]
        public void LagKryptertArkivVerifiserInnholdValiderer()
        {
            var arkiv = new Arkiv(Dokumentpakke, Signatur, Manifest);
            var originalData = arkiv.LagArkiv();

            var krypterteData = arkiv.Krypter(Sertifikat);
            var dekrypterteData = Arkiv.Dekrypter(krypterteData); 

            Assert.AreEqual(originalData.ToString(), dekrypterteData.ToString());
        }
        
        private void GenererSjekksum(ZipArchive zip, string filstiPåDisk, string entryNavnIArkiv, out byte[] hash1, out byte[] hash2)
        {
            GenererSjekksum(zip, File.ReadAllBytes(filstiPåDisk), entryNavnIArkiv, out hash1, out hash2);
        }

        private void GenererSjekksum(ZipArchive zip, byte[] fil, string entryNavnIArkiv, out byte[] hash1, out byte[] hash2)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new MemoryStream(fil))
                {
                    hash1 = md5.ComputeHash(stream);
                }

                using (var stream = zip.GetEntry(entryNavnIArkiv).Open())
                {
                    hash2 = md5.ComputeHash(stream);
                }
            }
        }

    }
}
