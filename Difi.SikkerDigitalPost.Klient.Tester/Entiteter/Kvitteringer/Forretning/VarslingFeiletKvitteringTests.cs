using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning.Tests
{
    [TestClass()]
    public class VarslingFeiletKvitteringTests
    {
        [Ignore]
        [TestClass]
        public class KonstruktørMethod : VarslingFeiletKvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                //TODO: skaff testdata og lag denne testen
                var rådata = "";

                var kvittering = KvitteringFactory.GetKvittering(rådata);

                Assert.IsInstanceOfType(kvittering, typeof(Forretningskvittering));
                Assert.IsInstanceOfType(kvittering, typeof(VarslingFeiletKvittering));
            }
        }
    }
}