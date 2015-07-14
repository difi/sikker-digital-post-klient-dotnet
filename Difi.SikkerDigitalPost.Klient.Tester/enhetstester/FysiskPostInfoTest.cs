using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.enhetstester
{
    [TestClass]
    public class FysiskPostInfoTest
    {
        [TestMethod]
        public void StøtteForLegacyKonstruktør()
        {
           var fysiskPostInfo =
             new FysiskPostInfo(DomeneUtility.GetFysiskPostMottaker(), Posttype.A, Utskriftsfarge.Farge,
                Posthåndtering.DirekteRetur, DomeneUtility.GetFysiskPostMottaker());
            
            Assert.IsInstanceOfType(fysiskPostInfo.Mottaker,typeof(FysiskPostMottaker));
            Assert.IsInstanceOfType(fysiskPostInfo.ReturMottaker, typeof(FysiskPostMottaker));
        }

        [TestMethod]
        public void SkalFungereMedNyKonstruktør()
        {
            var fysiskPostInfo =
              new FysiskPostInfo(DomeneUtility.GetFysiskPostMottaker(), Posttype.A, Utskriftsfarge.Farge,
                 Posthåndtering.DirekteRetur, DomeneUtility.GetFysiskPostReturMottaker());

            Assert.IsInstanceOfType(fysiskPostInfo.Mottaker, typeof(FysiskPostMottaker));
            Assert.IsInstanceOfType(fysiskPostInfo.ReturMottaker, typeof(FysiskPostReturMottaker));
        }


    }
}
