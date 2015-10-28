using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering.Tests
{
    [TestClass]
    public class SertifikatvalidatorFunksjoneltTestmiljøTester
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        [TestClass]
        public class ErGyldigResponssertifikatMethod : SertifikatvalidatorFunksjoneltTestmiljøTester
        {
            [TestMethod]
            public void ErGyldigSertifikatOgKjedestatus()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "testmottakerFraOppslagstjenesten.pem"));
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new SertifikatValidatorFunksjoneltTestmiljø(SertifikatUtility.TestSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(testSertifikat, out kjedestatus);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "testmottakerFraOppslagstjenesten.pem"));

                //Act
                var sertifikatValidator = new SertifikatValidatorFunksjoneltTestmiljø(SertifikatUtility.TestSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(testSertifikat);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
            }
        }
    }
}