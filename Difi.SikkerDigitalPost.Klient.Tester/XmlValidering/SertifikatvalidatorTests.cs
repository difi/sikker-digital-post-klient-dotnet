using ApiClientShared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering.Tests
{
    [TestClass]
    public class SertifikatvalidatorTests
    {
        ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        [TestClass]
        public class ValiderMethod : SertifikatvalidatorTests
        {
            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = ResourceUtility.ReadAllBytes(true, "test", "testmottakerFraOppslagstjenesten.pem");

                //Act
                Sertifikatvalidator.ValiderResponssertifikat()

                //Assert
            }
        }
    }
}