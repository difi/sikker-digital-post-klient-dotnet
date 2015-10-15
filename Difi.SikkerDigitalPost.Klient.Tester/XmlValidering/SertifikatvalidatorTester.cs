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
        ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        [TestClass]
        public class ValiderMethod : SertifikatvalidatorTester
        {
            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "testmottakerFraOppslagstjenesten.pem"));

                //Act
                SertifikatValidatorTest sertifikatValidator = new SertifikatValidatorTest(DomeneUtility.DifiTestkjedesertifikater());

                //Assert
            }
            
             

        }

        
        
    }
}