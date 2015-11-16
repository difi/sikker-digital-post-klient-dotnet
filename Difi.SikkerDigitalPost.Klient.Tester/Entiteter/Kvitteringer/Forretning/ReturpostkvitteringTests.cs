using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning.Tests
{
    [TestClass()]
    public class ReturpostkvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : ReturpostkvitteringTests
        {
            [Ignore]
            [TestMethod]
            public void EnkelKonstruktør()
            {
                var rådata = "";
                
                var kvittering  = KvitteringFactory.GetKvittering(rådata);

                Assert.IsInstanceOfType(kvittering, typeof(Forretningskvittering));
                Assert.IsInstanceOfType(kvittering,typeof(Returpostkvittering));
            }
        }
    }
}