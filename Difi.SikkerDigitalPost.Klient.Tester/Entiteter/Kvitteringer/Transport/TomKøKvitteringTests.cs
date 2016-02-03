using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Transport
{
    [TestClass]
    public class TomKøKvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : TomKøKvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                //Arrange
                var tomKøKvittering = new TomKøKvittering();

                //Act

                //Assert
                Assert.AreEqual(string.Empty, tomKøKvittering.MeldingsId);
            }
        }
    }
}