using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering.Tests
{
    [TestClass]
    public class SertifikatvalidatorTester
    {
        readonly ResourceUtility _resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        [TestClass]
        public class ValiderResponssertifikatMethod : SertifikatvalidatorTester
        {
            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(_resourceUtility.ReadAllBytes(true, "test", "testmottakerFraOppslagstjenesten.pem"));

                //Act
                SertifikatValidatorTest sertifikatValidator = new SertifikatValidatorTest(DomeneUtility.DifiTestkjedesertifikater());
                var result = sertifikatValidator.ValiderResponssertifikat(testSertifikat);

                //Assert
                Assert.AreEqual(0, result.Length);
            }
        }

        [TestClass]
        public class ErGyldigResponssertifikatMethod : SertifikatvalidatorTester
        {
            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(_resourceUtility.ReadAllBytes(true, "test", "testmottakerFraOppslagstjenesten.pem"));

                //Act
                SertifikatValidatorTest sertifikatValidator = new SertifikatValidatorTest(DomeneUtility.DifiTestkjedesertifikater());
                var result = sertifikatValidator.ErGyldigResponssertifikat(testSertifikat);

                //Assert
                Assert.IsTrue(result);
            }
        }
    }
}