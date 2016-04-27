using System.Collections;
using System.Collections.Generic;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Enhetstester
{
    [TestClass]
    public class EpostvarselTester
    {
        [TestClass]
        public class KonstruktørMethod : EpostvarselTester
        {
            [TestMethod]
            public void EnkelKonstruktørUtenVarslingstidspunkt()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoWithTestCertificate();
                var epostadresse = "tull@ball.no";
                var varsel = "Et lite varsel pr Epost.";
                postInfo.EpostVarsel = new EpostVarsel(epostadresse, varsel);
                var forventedeVarslingerEtterDager = new List<int> {0};

                //Act

                //Assert
                var epostVarsel = postInfo.EpostVarsel;
                Assert.AreEqual(epostadresse, epostVarsel.Epostadresse);
                Assert.AreEqual(varsel, epostVarsel.Varslingstekst);
                CollectionAssert.AreEqual(forventedeVarslingerEtterDager, (ICollection) epostVarsel.VarselEtterDager);
            }

            [TestMethod]
            public void EnkelKonstruktørMedVarslingstidspunktSomListe()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoWithTestCertificate();
                var epostadresse = "tull@ball.no";
                var varsel = "Et lite varsel pr Epost.";
                var varslingerEtterDager = new List<int> {0, 10, 15};
                postInfo.EpostVarsel = new EpostVarsel(epostadresse, varsel, varslingerEtterDager);

                //Act

                //Assert
                var epostVarsel = postInfo.EpostVarsel;
                Assert.AreEqual(epostadresse, epostVarsel.Epostadresse);
                Assert.AreEqual(varsel, epostVarsel.Varslingstekst);
                CollectionAssert.AreEqual(varslingerEtterDager, (ICollection) epostVarsel.VarselEtterDager);
            }

            [TestMethod]
            public void EnkelKonstruktørMedVarslingstidspunktSomArgumenter()
            {
                //Arrange
                var postInfo = DomainUtility.GetDigitalPostInfoWithTestCertificate();
                var epostadresse = "tull@ball.no";
                var varsel = "Et lite varsel pr Epost.";
                var varslingerEtterDager = new List<int> {0, 10, 15};
                postInfo.EpostVarsel = new EpostVarsel(epostadresse, varsel, 0, 10, 15);

                //Act

                //Assert
                var epostVarsel = postInfo.EpostVarsel;
                Assert.AreEqual(epostadresse, epostVarsel.Epostadresse);
                Assert.AreEqual(varsel, epostVarsel.Varslingstekst);
                CollectionAssert.AreEqual(varslingerEtterDager, (ICollection) epostVarsel.VarselEtterDager);
            }
        }
    }
}