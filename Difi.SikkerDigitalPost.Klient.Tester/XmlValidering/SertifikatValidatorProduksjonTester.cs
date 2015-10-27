using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering.Tests
{
    [TestClass]
    public class SertifikatvalidatorProduksjonTester
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        [TestClass]
        public class ErGyldigResponssertifikatMethod : SertifikatvalidatorFunksjoneltTestmiljøTester
        {
            [TestMethod]
            public void ErGyldigSertifikatOgKjedestatus()
            {
                //Arrange
                var produksjonssertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "DigipostVirksomhetssertifikat.pem"));
                X509ChainStatus[] kjedestatus;


                //Act
                var sertifikatValidator = new SertifikatValidatorProduksjon(SertifikatUtility.ProduksjonsSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(produksjonssertifikat, out kjedestatus);

                //Assert
                const int forventetAntallStatusElementer = 0;
                Assert.IsTrue(erGyldigResponssertifikat);
                Assert.AreEqual(forventetAntallStatusElementer, kjedestatus.Length);
            }
            
        }
    }
}