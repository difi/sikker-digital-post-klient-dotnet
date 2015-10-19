using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tests
{
    [TestClass()]
    public class KlientkonfigurasjonTester
    {
        [TestClass]
        public class KonstruktørMethod : KlientkonfigurasjonTester
        {
            [TestMethod] public void EnkelKonstruktørMedMiljø()
            {
                //Arrange
                var miljø = Miljø.Test;
                Klientkonfigurasjon klientkonfigurasjon = new Klientkonfigurasjon(miljø);

                //Act

                //Assert
                Assert.AreEqual(miljø, klientkonfigurasjon.Miljø);
            }
        }

    }
}