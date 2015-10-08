using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Enhetstester
{
    [TestClass]
    public class SmsVarselTester
    {
        [TestClass]
        public class Konstruktør : SmsVarselTester
        {
            [TestMethod]
            public void EnkelKonstruktørUtenVarslingstidspunkt()
            {
                //Arrange
                var postInfo = DomeneUtility.GetDigitalPostInfoEnkel();
                postInfo.SmsVarsel = new SmsVarsel("12345678", "Et lite varsel pr SMS.");

                //Act

                //Assert
                var sikkerDigitalPostKlient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();

                sikkerDigitalPostKlient.

                var forsendelse = DomeneUtility.GetFysiskForsendelseEnkel();

            }             
        }

    }
}
