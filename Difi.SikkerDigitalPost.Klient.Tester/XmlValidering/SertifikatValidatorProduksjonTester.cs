using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering.Tests
{
    [TestClass]
    public class SertifikatvalidatorProduksjonTester
    {
        static readonly ResourceUtility _resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        [TestClass]
        public class ErGyldigResponssertifikatMethod : SertifikatvalidatorTestTester
        {
            [TestMethod]
            public void GodkjennerProduksjonssertifikatReturnererKjedeSomOutparameter()
            {
                //Arrange
                var produksjonssertifikat = new X509Certificate2(_resourceUtility.ReadAllBytes(true, "prod", "DigipostVirksomhetssertifikat.pem"));

                //Act
                SertifikatValidatorProduksjon sertifikatValidator = new SertifikatValidatorProduksjon(SertifikatUtility.ProduksjonsSertifikater());
                X509ChainStatus[] kjedestatus;
                var result = sertifikatValidator.ErGyldigResponssertifikat(produksjonssertifikat, out kjedestatus);

                //Assert
                Assert.IsTrue(result);
                Assert.AreEqual(0, kjedestatus.Length);
            }

            [TestMethod]
            public void GodkjennerProduksjonssertifikat()
            {
                //Arrange
                var produksjonssertifikat = new X509Certificate2(_resourceUtility.ReadAllBytes(true, "prod", "DigipostVirksomhetssertifikat.pem"));

                //Act
                SertifikatValidatorProduksjon sertifikatValidator = new SertifikatValidatorProduksjon(SertifikatUtility.ProduksjonsSertifikater());
                var result = sertifikatValidator.ErGyldigResponssertifikat(produksjonssertifikat);

                //Assert
                Assert.IsTrue(result);
            }
        }
    }
}