using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Enhetstester
{
    
    public class FysiskPostInfoTester
    {
        
        public class KontstruktørMetode
        {
            [Fact]
            public void SkalFungereMedNyMåteÅInitialiserePå()
            {
                var fysiskPostInfo =
                    new FysiskPostInfo(DomainUtility.GetFysiskPostMottakerWithTestCertificate(), Posttype.A, Utskriftsfarge.Farge,
                        Posthåndtering.DirekteRetur, DomainUtility.GetFysiskPostReturMottaker());

                Assert.IsType<FysiskPostReturmottaker>(fysiskPostInfo.Returpostmottaker);
            }
        }
    }
}