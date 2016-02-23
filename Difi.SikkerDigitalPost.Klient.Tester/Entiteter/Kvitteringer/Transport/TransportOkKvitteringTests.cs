using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Transport
{
    [TestClass]
    public class TransportOkKvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : TransportOkKvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                //Arrange
                var transportOkKvittering = new TransportOkKvittering();

                //Act

                //Assert
                Assert.AreEqual(string.Empty, transportOkKvittering.MeldingsId);
            }
        }
    }
}