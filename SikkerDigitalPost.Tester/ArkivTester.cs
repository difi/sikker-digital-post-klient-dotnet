/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.AsicE;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class ArkivTester : TestBase
    {
        public TestContext TestContext { get; set; }
        readonly ResourceUtility _resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata");


        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Initialiser();
            //Overkjør arkiv i Base for å bruke et sertifikat vi har privatekey til.
            DigitalPostMottaker.Sertifikat = Mottakersertifikat();
        }

        [TestMethod]
        public void LeggFilerTilDokumentpakkeAntallStemmer()
        {
            Assert.AreEqual(Vedleggsstier.Length, Dokumentpakke.Vedlegg.Count);
            Assert.IsNotNull(Dokumentpakke.Hoveddokument);
        }

        [TestMethod]
        public void LeggTilVedleggOgSjekkIdNummer()
        {
            Dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 1", new byte[] { 0x00 }, "text/plain"));
            Dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 2", new byte[] { 0x00 }, "text/plain"));
            Dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 3", new byte[] { 0x00 }, "text/plain"), new Dokument("Dokument 4", new byte[] { 0x00 }, "text/plain"));

            Assert.AreEqual(Dokumentpakke.Hoveddokument.Id, "Id_2");
            for (int i = 0; i < Dokumentpakke.Vedlegg.Count; i++)
            {
                var vedlegg = Dokumentpakke.Vedlegg[i];
                Assert.AreEqual(vedlegg.Id, "Id_" + (i + 3));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(KonfigurasjonsException), "To like filer ble uriktig godtatt i dokumentpakken.")]
        public void LeggTilVedleggSammeFilnavnKasterException()
        {
            Dokumentpakke.LeggTilVedlegg(new Dokument("DokumentUnikt", new byte[] { 0x00 }, "text/plain", "NO", "Filnavn.txt"));
            Dokumentpakke.LeggTilVedlegg(new Dokument("DokumentDuplikat", new byte[] { 0x00 }, "text/plain", "NO", "Filnavn.txt"));   
        }

        [TestMethod]
        [ExpectedException(typeof(KonfigurasjonsException), "To like filer ble uriktig godtatt i dokumentpakken.")]
        public void LeggTilVedleggSammeNavnSomHoveddokumentKasterException()
        {
            Dokumentpakke.LeggTilVedlegg(new Dokument("DokumentSomHoveddokument", new byte[] { 0x00 }, "text/plain", "NO", Hoveddokument.Filnavn));
        }

        [Ignore]
        [TestMethod]
        public void LagArkivOgVerifiserDokumentInnhold()
        {
            var dekryptertArkivBytes = AsicEArkiv.Dekrypter(Arkiv.Bytes);
            var arkivstrøm = new MemoryStream(dekryptertArkivBytes);

            //Åpne zip og generer sjekksum for å verifisere innhold
            using (var zip = new ZipArchive(arkivstrøm, ZipArchiveMode.Read))
            {
                //Alle vedlegg
                foreach (var filsti in Vedleggsstier)
                {
                    byte[] sjekksum1;
                    byte[] sjekksum2;

                    GenererSjekksum(zip, filsti, _resourceUtility.GetFileName(filsti), out sjekksum1, out sjekksum2);
                    Assert.AreEqual(sjekksum1.ToString(), sjekksum2.ToString());
                }

                //Signaturfil
                {
                    byte[] sjekksum1;
                    byte[] sjekksum2;

                    GenererSjekksum(zip, Arkiv.Signatur.Bytes, Arkiv.Signatur.Filnavn, out sjekksum1, out sjekksum2);
                    Assert.AreEqual(sjekksum1.ToString(), sjekksum2.ToString());
                }

                //Manifest
                {
                    byte[] sjekksum1;
                    byte[] sjekksum2;

                    GenererSjekksum(zip, Arkiv.Manifest.Bytes, _resourceUtility.GetFileName(Arkiv.Manifest.Filnavn), out sjekksum1, out sjekksum2);
                    Assert.AreEqual(sjekksum1.ToString(), sjekksum2.ToString());
                }
            }
        }

        [Ignore]
        [TestMethod]
        public void LagKryptertArkivVerifiserInnholdValiderer()
        {
            var arkiv = new AsicEArkiv(Forsendelse, GuidHandler, Databehandler.Sertifikat);
            var originalData = arkiv.Bytes;

            var krypterteData = arkiv.Bytes;
            var dekrypterteData = AsicEArkiv.Dekrypter(krypterteData); 

            Assert.AreEqual(originalData.ToString(), dekrypterteData.ToString());
        }
        
        private void GenererSjekksum(ZipArchive zip, string filstiPåDisk, string entryNavnIArkiv, out byte[] sjekksum1, out byte[] sjekksum2)
        {
            var bytes = _resourceUtility.ReadAllBytes(false,filstiPåDisk);
            GenererSjekksum(zip, bytes, entryNavnIArkiv, out sjekksum1, out sjekksum2);
        }

        private void GenererSjekksum(ZipArchive zip, byte[] fil, string entryNavnIArkiv, out byte[] sjekksum1, out byte[] sjekksum2)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new MemoryStream(fil))
                {
                    sjekksum1 = md5.ComputeHash(stream);
                }

                using (var stream = zip.GetEntry(entryNavnIArkiv).Open())
                {
                    sjekksum2 = md5.ComputeHash(stream);
                }
            }
        }

        private static X509Certificate2 Mottakersertifikat()
        {
            var storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);
            var sertifikat = storeMy.Certificates[0];
            storeMy.Close();
            return sertifikat;
        }

    }
}
