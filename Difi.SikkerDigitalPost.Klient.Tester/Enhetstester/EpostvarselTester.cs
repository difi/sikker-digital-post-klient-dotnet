using System.Collections;
using System.Collections.Generic;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Enhetstester
{
    public class EpostvarselTester
    {
        public class KonstruktørMethod : EpostvarselTester
        {
            [Fact]
            public void EnkelKonstruktørUtenVarslingstidspunkt()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoSimple();
                var epostadresse = "tull@ball.no";
                var varsel = "Et lite varsel pr Epost.";
                postInfo.EpostVarsel = new EpostVarsel(epostadresse, varsel);
                var forventedeVarslingerEtterDager = new List<int> {0};

                //Act

                //Assert
                var epostVarsel = postInfo.EpostVarsel;
                Assert.Equal(epostadresse, epostVarsel.Epostadresse);
                Assert.Equal(varsel, epostVarsel.Varslingstekst);
            }

            [Fact]
            public void EnkelKonstruktørMedVarslingstidspunktSomListe()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoSimple();
                var epostadresse = "tull@ball.no";
                var varsel = "Et lite varsel pr Epost.";
                var varslingerEtterDager = new List<int> {0, 10, 15};
                postInfo.EpostVarsel = new EpostVarsel(epostadresse, varsel, varslingerEtterDager);

                //Act

                //Assert
                var epostVarsel = postInfo.EpostVarsel;
                Assert.Equal(epostadresse, epostVarsel.Epostadresse);
                Assert.Equal(varsel, epostVarsel.Varslingstekst);
            }

            [Fact]
            public void EnkelKonstruktørMedVarslingstidspunktSomArgumenter()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoSimple();
                var epostadresse = "tull@ball.no";
                var varsel = "Et lite varsel pr Epost.";
                var varslingerEtterDager = new List<int> {0, 10, 15};
                postInfo.EpostVarsel = new EpostVarsel(epostadresse, varsel, 0, 10, 15);

                //Act

                //Assert
                var epostVarsel = postInfo.EpostVarsel;
                Assert.Equal(epostadresse, epostVarsel.Epostadresse);
                Assert.Equal(varsel, epostVarsel.Varslingstekst);
            }
        }
    }
}
