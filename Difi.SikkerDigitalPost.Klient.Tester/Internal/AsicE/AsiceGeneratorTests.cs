using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Internal.AsicE
{
    public class AsiceGeneratorTests
    {
        public class CreateMethod : AsiceGeneratorTests
        {
            [Fact]
            public void ThrowsExceptionOnInvalidManifest()
            {
                //Arrange
                const string invalidFileNameNotFourCharacters = "T";
                var asiceConfiguration = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);
                var dokumentpakkeUtenVedlegg = new Dokumentpakke(new Dokument("", new byte[3], "application/pdf", "nb", invalidFileNameNotFourCharacters));
                var forsendelse = new Forsendelse(new Avsender(new Organisasjonsnummer("123456789")), DomainUtility.GetDigitalPostInfoSimple(), dokumentpakkeUtenVedlegg, Guid.NewGuid());

                //Act
                Assert.Throws<XmlValidationException>(() =>
                    AsiceGenerator.Create(forsendelse, new GuidUtility(), DomainUtility.GetAvsenderCertificate(), asiceConfiguration)
                    );
            }
        }
    }
}