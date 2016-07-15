using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Xunit.Assert;

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
                var forsendelse = new Forsendelse(new Avsender("123"), DomainUtility.GetDigitalPostInfoSimple(), dokumentpakkeUtenVedlegg, Guid.NewGuid());

                //Act
                Assert.Throws<XmlValidationException>(() =>
                    AsiceGenerator.Create(forsendelse, new GuidUtility(), DomainUtility.GetAvsenderCertificate(), asiceConfiguration)
                    );
                
            }
        }
    }
}