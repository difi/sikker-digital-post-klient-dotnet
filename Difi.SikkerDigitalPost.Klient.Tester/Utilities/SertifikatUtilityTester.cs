using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Utilities.Tests
{
    [TestClass]
    public class SertifikatUtilityTester
    {
        [TestClass]
        public class TestsertifikaterMethod : SertifikatUtilityTester
        {
            [TestMethod]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = SertifikatUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.IsNotNull(sertifikat.Thumbprint);  
                }
            } 
        }

        [TestClass]
        public class ProduksjonssertifikaterMethod : SertifikatUtilityTester
        {
            [TestMethod]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = SertifikatUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.IsNotNull(sertifikat.Thumbprint);
                }
            }
        }

    }
}