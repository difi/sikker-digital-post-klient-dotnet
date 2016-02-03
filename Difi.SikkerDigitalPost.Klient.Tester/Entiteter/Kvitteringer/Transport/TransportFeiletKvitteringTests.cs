using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Transport
{
    [TestClass]
    public class TransportFeiletKvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : TransportFeiletKvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                //Arrange
                var transportFeiletKvittering = new TransportFeiletKvittering();

                //Act

                //Assert
                Assert.AreEqual(string.Empty, transportFeiletKvittering.MeldingsId);
            }
        }
    }
}