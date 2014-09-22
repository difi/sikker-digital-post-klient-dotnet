using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Net.KlientApi;

namespace SikkerDigitalPost.Net.Tests
{
    [TestClass]
    public class ArkivTester
    {
        private static string _testDataMappe = "testdata";

        private static string _vedleggsMappe = "vedlegg";
        private static string _hoveddokumentMappe = "hoveddokument";

        private static string _hoveddokument;
        private static string[] _vedleggsFiler;
        private static string _manifestFil = "manifest.xml";
        private static string _signaturFil = "signatur.xml";

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _testDataMappe = Path.Combine(path1: Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)), path2: _testDataMappe);

            _vedleggsMappe = Path.Combine(_testDataMappe, _vedleggsMappe);
            _hoveddokumentMappe = Path.Combine(_testDataMappe, _hoveddokumentMappe);

            _vedleggsFiler = Directory.GetFiles(_vedleggsMappe);
            _hoveddokument = Directory.GetFiles(_hoveddokumentMappe)[0];
            _signaturFil = Directory.GetFiles(_testDataMappe).Single(f => f.Contains(_signaturFil));
            _manifestFil = Directory.GetFiles(_testDataMappe).Single(f => f.Contains(_manifestFil));
        }

        [TestMethod]
        public void LeggFilerTilDokumentpakkeAntallStemmer()
        {
            var hoveddokument = GenererHoveddokument();
            var vedlegg = GenererVedlegg();

            var dokumentpakke = new Dokumentpakke(hoveddokument);
            dokumentpakke.LeggTilVedlegg(vedlegg);

            Assert.AreEqual(vedlegg.Count, dokumentpakke.Vedlegg.Count);
            Assert.IsNotNull(dokumentpakke.Hoveddokument);
        }

        [TestMethod]
        public void LagArkivOgVerifiserDokumentInnhold()
        {
            var dokumentpakke = GenererDokumentpakke();
            var arkiv = new Arkiv(dokumentpakke, new Signatur(_signaturFil), new Manifest(_manifestFil));

            var arkivstrøm = arkiv.LagArkiv();

            //Åpne zip og generer sjekksum for å verifisere innhold
            using (var zip = new ZipArchive(arkivstrøm, ZipArchiveMode.Read))
            {
                
                //Alle vedlegg
                foreach (var filsti in _vedleggsFiler)
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

                    GenererSjekksum(zip, _signaturFil, arkiv.Signatur.Filsti, out sjekksum1, out sjekksum2);
                    Assert.AreEqual(sjekksum1.ToString(), sjekksum2.ToString());
                }

                //Manifest
                {
                    byte[] sjekksum1;
                    byte[] sjekksum2;

                    GenererSjekksum(zip, _manifestFil, Path.GetFileName(arkiv.Manifest.Filsti), out sjekksum1, out sjekksum2);
                    Assert.AreEqual(sjekksum1.ToString(), sjekksum2.ToString());
                }
            }
        }

        private void GenererSjekksum(ZipArchive zip, string filstiPåDisk, string entryNavnIArkiv, out byte[] hash1, out byte[] hash2)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filstiPåDisk))
                {
                    hash1 = md5.ComputeHash(stream);
                }

                using (var stream = zip.GetEntry(entryNavnIArkiv).Open())
                {
                    hash2 = md5.ComputeHash(stream);
                }
            }
        }

        private Dokument GenererHoveddokument()
        {
            return new Dokument(Path.GetFileName(_hoveddokument), _hoveddokument, "text/xml");
        }

        private List<Dokument> GenererVedlegg()
        {
            return new List<Dokument>(
                    _vedleggsFiler.Select(
                        v => new Dokument(Path.GetFileNameWithoutExtension(v), v, "text/" + Path.GetExtension(_hoveddokument))));
        }

        private Dokumentpakke GenererDokumentpakke()
        {
            var dokumentpakke = new Dokumentpakke(GenererHoveddokument());
            dokumentpakke.LeggTilVedlegg(GenererVedlegg());
            return dokumentpakke;

        }
    }
}
