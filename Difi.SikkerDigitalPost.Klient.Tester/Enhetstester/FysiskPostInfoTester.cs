using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Enhetstester
{
    [TestClass]
    public class FysiskPostInfoTester
    {
        [TestClass]
        public class KontstruktørMetode
        {
            [TestMethod]
            public void SkalFungereMedNyMåteÅInitialiserePå()
            {
                var fysiskPostInfo =
                    new FysiskPostInfo(DomainUtility.GetFysiskPostMottakerWithTestCertificate(), Posttype.A, Utskriftsfarge.Farge,
                        Posthåndtering.DirekteRetur, DomainUtility.GetFysiskPostReturMottaker());

                Assert.IsInstanceOfType(fysiskPostInfo.Returpostmottaker, typeof (FysiskPostReturmottaker));
            }
        }
    }
}