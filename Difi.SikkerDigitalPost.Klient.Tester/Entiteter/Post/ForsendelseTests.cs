using System.Linq;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Post
{
    public class ForsendelseTests
    {
        public class ConstructorMethod : ForsendelseTests
        {
            [Fact]
            public void SuccessfullySetsLanguageOnDocumentsWithNoLanguageCodeFromMessage()
            {
                //Arrange
                var sender = DomainUtility.GetAvsender();
                var simpleDigitalPostInfo = DomainUtility.GetDigitalPostInfoSimple();

                string undefinedLanguageCode = null;
                var primaryDocument = new Dokument("Tiitle", new byte[3], "application/pdf", undefinedLanguageCode);
                var documentBundle = new Dokumentpakke(primaryDocument);
                var definedLanguageCode = "en";
                documentBundle.LeggTilVedlegg(new Dokument("Appendix", new byte[2], "application/pdf", definedLanguageCode));

                var messageLanguageCode = "no";
                //Act
                var forsendelse = new Forsendelse(sender, simpleDigitalPostInfo, documentBundle, Prioritet.Normal, "mpcId", messageLanguageCode);

                //Assert
                Assert.Equal(messageLanguageCode, documentBundle.Hoveddokument.Språkkode);
                Assert.Equal(definedLanguageCode, documentBundle.Vedlegg.First().Språkkode);
            }
        }
    }
}