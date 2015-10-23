using System.Security.Policy;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.XmlValidering
{
    [TestClass]
    public class MiljøTester
    {
        [TestClass]
        public class GetMiljøMethod : MiljøTester
        {
            [TestMethod]
            public void ReturnererInitialisertTestmiljø()
            {
                //Arrange
                var url = "https://qaoffentlig.meldingsformidler.digipost.no/api/ebms";
                var miljø = Miljø.Test;
                var sertifikater = SertifikatUtility.TestSertifikater();

                //Act

                //Assert
                Assert.IsNotNull(miljø.Sertifikatvalidator);
                Assert.AreEqual(url, miljø.Url.AbsoluteUri);
                CollectionAssert.AreEqual(sertifikater, miljø.Sertifikatvalidator.SertifikatLager);
            }

            [TestMethod]
            public void ReturnererInitialisertProduksjonsmiljø()
            {
                //Arrange
                var url = "https://meldingsformidler.digipost.no/api/ebms";
                var miljø = Miljø.Produksjon;
                var sertifikater = SertifikatUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                Assert.IsNotNull(miljø.Sertifikatvalidator);
                Assert.AreEqual(url, miljø.Url.ToString());
                CollectionAssert.AreEqual(sertifikater, miljø.Sertifikatvalidator.SertifikatLager);
            }
        }
    }
}
