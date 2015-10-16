using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering.Tests
{
    [TestClass]
    public class SertifikatvalidatorQaTester
    {
        readonly ResourceUtility _resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        [TestClass]
        public class ValiderResponssertifikatMethod : SertifikatvalidatorQaTester
        {
            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(_resourceUtility.ReadAllBytes(true, "test", "testmottakerFraOppslagstjenesten.pem"));

                //Act
                SertifikatValidatorQa sertifikatValidator = new SertifikatValidatorQa(SertifikatUtility.TestSertifikater());
                var result = sertifikatValidator.ValiderResponssertifikat(testSertifikat);

                //Assert
                Assert.AreEqual(0, result.Length);
            }
        }

        [TestClass]
        public class ErGyldigResponssertifikatMethod : SertifikatvalidatorQaTester
        {
            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(_resourceUtility.ReadAllBytes(true, "test", "testmottakerFraOppslagstjenesten.pem"));

                //Act
                SertifikatValidatorQa sertifikatValidator = new SertifikatValidatorQa(SertifikatUtility.TestSertifikater());
                var result = sertifikatValidator.ErGyldigResponssertifikat(testSertifikat);

                //Assert
                Assert.IsTrue(result);
            }
        }
    }
}