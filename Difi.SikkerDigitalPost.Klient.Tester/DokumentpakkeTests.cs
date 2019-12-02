using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class DokumentpakkeTests
    {
        public class Validering
        {
            [Fact]
            public void LeggFilerTilDokumentpakkeAntallStemmer()
            {
                var dokumentpakke = DomainUtility.GetDokumentpakkeWithMultipleVedlegg(5);

                Assert.Equal(DomainUtility.GetVedleggFilesPaths().Length, dokumentpakke.Vedlegg.Count);
                Assert.NotNull(dokumentpakke);
            }

            [Fact]
            public void LeggTilVedleggOgSjekkIdNummer()
            {
                var dokumentpakke = DomainUtility.GetDokumentpakkeWithoutAttachments();

                dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 1", new byte[] {0x00}, "text/plain"));
                dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 2", new byte[] {0x00}, "text/plain"));
                dokumentpakke.LeggTilVedlegg(new Dokument("Dokument 3", new byte[] {0x00}, "text/plain"),
                    new Dokument("Dokument 4", new byte[] {0x00}, "text/plain"));

                Assert.Equal(dokumentpakke.Hoveddokument.Id, "Id_2");
                for (var i = 0; i < dokumentpakke.Vedlegg.Count; i++)
                {
                    var vedlegg = dokumentpakke.Vedlegg[i];
                    Assert.Equal(vedlegg.Id, "Id_" + (i + Dokumentpakke.VEDLEGG_START_ID));
                }
            }
        }

        public class Exceptions
        {
            [Fact]
            public void LeggTilVedleggSammeFilnavnKasterException()
            {
                var dokumentpakke = DomainUtility.GetDokumentpakkeWithoutAttachments();

                dokumentpakke.LeggTilVedlegg(new Dokument("DokumentUnikt", new byte[] {0x00}, "text/plain", "NO",
                    "Filnavn.txt"));

                Assert.Throws<KonfigurasjonsException>(() =>
                    dokumentpakke.LeggTilVedlegg(new Dokument("DokumentDuplikat", new byte[] {0x00}, "text/plain", "NO",
                        "Filnavn.txt"))
                    );
            }

            [Fact]
            public void LeggTilVedleggSammeNavnSomHoveddokumentKasterException()
            {
                var dokumentpakke = DomainUtility.GetDokumentpakkeWithoutAttachments();

                Assert.Throws<KonfigurasjonsException>(() =>
                    dokumentpakke.LeggTilVedlegg(new Dokument("DokumentSomHoveddokument", new byte[] {0x00}, "text/plain", "NO", dokumentpakke.Hoveddokument.Filnavn))
                    );
            }
        }
    }
}