using System.Collections;
using System.Collections.Generic;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Enhetstester
{
    public class SmsVarselTester
    {
        public class Konstruktør : SmsVarselTester
        {
            [Fact]
            public void EnkelKonstruktørUtenVarslingstidspunkt()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoSimple();
                var mobilnummer = "12345678";
                var varsel = "Et lite varsel pr SMS.";
                postInfo.SmsVarsel = new SmsVarsel(mobilnummer, varsel);

                //Act

                //Assert
                var smsVarsel = postInfo.SmsVarsel;
                Assert.Equal(mobilnummer, smsVarsel.Mobilnummer);
                Assert.Equal(varsel, smsVarsel.Varslingstekst);
            }

            [Fact]
            public void KonstruktørMedVarslingstidspunktSomListe()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoSimple();
                var mobilnummer = "12345678";
                var varsel = "Et lite varsel pr SMS.";
                var varslingerEtterDager = new List<int> {0, 10, 15};
                postInfo.SmsVarsel = new SmsVarsel(mobilnummer, varsel, varslingerEtterDager);

                //Act

                //Assert
                var smsVarsel = postInfo.SmsVarsel;
                Assert.Equal(mobilnummer, smsVarsel.Mobilnummer);
                Assert.Equal(varsel, smsVarsel.Varslingstekst);
            }

            [Fact]
            public void KonstruktørMedVarslingstidspunktSomArgumenter()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoSimple();
                var mobilnummer = "12345678";
                var varsel = "Et lite varsel pr SMS.";
                postInfo.SmsVarsel = new SmsVarsel(mobilnummer, varsel, 0, 10, 15);
                var forventedeVarslingerEtterDager = new List<int> {0, 10, 15};

                //Act

                //Assert
                var smsVarsel = postInfo.SmsVarsel;
                Assert.Equal(mobilnummer, smsVarsel.Mobilnummer);
                Assert.Equal(varsel, smsVarsel.Varslingstekst);
            }
        }
    }
}
