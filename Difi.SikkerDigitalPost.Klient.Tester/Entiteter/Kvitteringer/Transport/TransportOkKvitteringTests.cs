using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport.Tests
{
    [TestClass()]
    public class TransportOkKvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : TransportOkKvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                var transportFeiletKvitteringRådata = "";

                var kvittering = (Transportkvittering)KvitteringFactory.GetKvittering(transportFeiletKvitteringRådata);

                Assert.IsInstanceOfType(kvittering, typeof(TransportOkKvittering));

            }
        }

    }
}