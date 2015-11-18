using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class ArkivTester
    {
        public TestContext TestContext { get; set; }

        [TestClass]
        public class Validering
        {
            [TestMethod]
            public void LeggFilerTilDokumentpakkeAntallStemmer()
            {
                var dokumentpakke = DomeneUtility.GetDokumentpakkeMedFlereVedlegg(5);

                Assert.AreEqual(DomeneUtility.GetVedleggsFilerStier().Length, dokumentpakke.Vedlegg.Count);
                Assert.IsNotNull(dokumentpakke);
            }

            [TestMethod]
            public void LeggTilVedleggOgSjekkIdNummer()
            {
                var dokumentpakke = DomeneUtility.GetDokumentpakkeUtenVedlegg();

                dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 1", new byte[] {0x00}, "text/plain"));
                dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 2", new byte[] {0x00}, "text/plain"));
                dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 3", new byte[] {0x00}, "text/plain"),
                    new Dokument("Dokument 4", new byte[] {0x00}, "text/plain"));

                Assert.AreEqual(dokumentpakke.Hoveddokument.Id, "Id_2");
                for (int i = 0; i < dokumentpakke.Vedlegg.Count; i++)
                {
                    var vedlegg = dokumentpakke.Vedlegg[i];
                    Assert.AreEqual(vedlegg.Id, "Id_" + (i + 3));
                }
            }
        }

        [TestClass]
        public class Exceptions
        {
            [TestMethod]
            [ExpectedException(typeof (KonfigurasjonsException), "To like filer ble uriktig godtatt i dokumentpakken.")]
            public void LeggTilVedleggSammeFilnavnKasterException()
            {
                var dokumentpakke = DomeneUtility.GetDokumentpakkeUtenVedlegg();

                dokumentpakke.LeggTilVedlegg(new Dokument("DokumentUnikt", new byte[] {0x00}, "text/plain", "NO",
                    "Filnavn.txt"));
                dokumentpakke.LeggTilVedlegg(new Dokument("DokumentDuplikat", new byte[] {0x00}, "text/plain", "NO",
                    "Filnavn.txt"));
            }

            [TestMethod]
            [ExpectedException(typeof (KonfigurasjonsException), "To like filer ble uriktig godtatt i dokumentpakken.")]
            public void LeggTilVedleggSammeNavnSomHoveddokumentKasterException()
            {
                var dokumentpakke = DomeneUtility.GetDokumentpakkeUtenVedlegg();
                dokumentpakke.LeggTilVedlegg(new Dokument("DokumentSomHoveddokument", new byte[] {0x00}, "text/plain",
                    "NO", dokumentpakke.Hoveddokument.Filnavn));
            }
        }

    }
}
