using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Utilities.Tests
{
    [TestClass]
    public class SertifikatkjedeUtilityTester
    {
        [TestClass]
        public class TestsertifikaterMethod : SertifikatkjedeUtilityTester
        {
            [TestMethod]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                foreach (var sertifikat in sertifikater)
                {
                    Assert.IsNotNull(sertifikat.Thumbprint);  
                }
            } 
        }

        [TestClass]
        public class ProduksjonssertifikaterMethod : SertifikatkjedeUtilityTester
        {
            [TestMethod]
            public void ReturnererFireSertifikaterMedThumbprint()
            {
                //Arrange
                var sertifikater = SertifikatkjedeUtility.ProduksjonsSertifikater();

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