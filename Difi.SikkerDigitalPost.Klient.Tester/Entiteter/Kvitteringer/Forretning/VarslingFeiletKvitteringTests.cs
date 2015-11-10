using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning.Tests
{
    [TestClass()]
    public class VarslingFeiletKvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : VarslingFeiletKvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                var rådata = "";

                var kvittering = (Forretningskvittering)KvitteringFactory.GetKvittering(rådata);

                Assert.IsInstanceOfType(kvittering, typeof(VarslingFeiletKvittering));
            }
        }
    }
}