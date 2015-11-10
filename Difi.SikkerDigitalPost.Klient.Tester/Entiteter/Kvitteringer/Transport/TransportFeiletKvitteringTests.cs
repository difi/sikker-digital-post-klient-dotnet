using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport.Tests
{
    [TestClass()]
    public class TransportFeiletKvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : TransportFeiletKvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                var transportFeiletKvitteringRådata = "";

                var kvittering = (Transportkvittering)KvitteringFactory.GetKvittering(transportFeiletKvitteringRådata);

                Assert.IsInstanceOfType(kvittering, typeof(TransportFeiletKvittering));
                
            }
        }

    }
}