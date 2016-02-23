using System.Collections;
using System.Collections.Generic;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Enhetstester
{
    [TestClass]
    public class SmsVarselTester
    {
        [TestClass]
        public class Konstruktør : SmsVarselTester
        {
            [TestMethod]
            public void EnkelKonstruktørUtenVarslingstidspunkt()
            {
                //Arrange
                var postInfo = DomeneUtility.GetDigitalPostInfoEnkelMedTestSertifikat();
                var mobilnummer = "12345678";
                var varsel = "Et lite varsel pr SMS.";
                postInfo.SmsVarsel = new SmsVarsel(mobilnummer, varsel);
                var forventedeVarslingerEtterDager = new List<int> {0};

                //Act

                //Assert
                var smsVarsel = postInfo.SmsVarsel;
                Assert.AreEqual(mobilnummer, smsVarsel.Mobilnummer);
                Assert.AreEqual(varsel, smsVarsel.Varslingstekst);
                CollectionAssert.AreEqual(forventedeVarslingerEtterDager, (ICollection) smsVarsel.VarselEtterDager);
            }

            [TestMethod]
            public void KonstruktørMedVarslingstidspunktSomListe()
            {
                //Arrange
                var postInfo = DomeneUtility.GetDigitalPostInfoEnkelMedTestSertifikat();
                var mobilnummer = "12345678";
                var varsel = "Et lite varsel pr SMS.";
                var varslingerEtterDager = new List<int> {0, 10, 15};
                postInfo.SmsVarsel = new SmsVarsel(mobilnummer, varsel, varslingerEtterDager);

                //Act

                //Assert
                var smsVarsel = postInfo.SmsVarsel;
                Assert.AreEqual(mobilnummer, smsVarsel.Mobilnummer);
                Assert.AreEqual(varsel, smsVarsel.Varslingstekst);
                CollectionAssert.AreEqual(varslingerEtterDager, (ICollection) smsVarsel.VarselEtterDager);
            }

            [TestMethod]
            public void KonstruktørMedVarslingstidspunktSomArgumenter()
            {
                //Arrange
                var postInfo = DomeneUtility.GetDigitalPostInfoEnkelMedTestSertifikat();
                var mobilnummer = "12345678";
                var varsel = "Et lite varsel pr SMS.";
                postInfo.SmsVarsel = new SmsVarsel(mobilnummer, varsel, 0, 10, 15);
                var forventedeVarslingerEtterDager = new List<int> {0, 10, 15};

                //Act

                //Assert
                var smsVarsel = postInfo.SmsVarsel;
                Assert.AreEqual(mobilnummer, smsVarsel.Mobilnummer);
                Assert.AreEqual(varsel, smsVarsel.Varslingstekst);
                CollectionAssert.AreEqual(forventedeVarslingerEtterDager, (ICollection) smsVarsel.VarselEtterDager);
            }
        }
    }
}